// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 09-20-2016
// ***********************************************************************
// <copyright file="CSVExport.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Windows.Storage;

namespace Core.Common
{
    /// <summary>
    /// Class CSVExport.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CSVExport<T> where T : class
    {
        /// <summary>
        /// The list separator
        /// </summary>
        private const string ListSeparator = ";";

        /// <summary>
        /// Gets or sets the excluded properties.
        /// </summary>
        /// <value>The excluded properties.</value>
        public List<String> ExcludedProperties { get; set; }

        /// <summary>
        /// The objects
        /// </summary>
        public IList<T> Objects;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVExport{T}" /> class.
        /// </summary>
        /// <param name="objects">The objects.</param>
        public CSVExport(IList<T> objects)
        {
            Objects = objects;
        }

        /// <summary>
        /// Exports this instance.
        /// </summary>
        /// <returns>System.String.</returns>
        public string Export()
        {
            //Logs.Instance.ErrorLog<CSVExport<T>>("Calls the Export method to include header line in the data to be exported to Audit file.",
              //  "Export", Severity.Debug);
            return Export(true);
        }

        /// <summary>
        /// Exports the specified include header line.
        /// </summary>
        /// <param name="includeHeaderLine">if set to <c>true</c> [include header line].</param>
        /// <returns>System.String.</returns>
        public string Export(bool includeHeaderLine)
        {
            //Logs.Instance.ErrorLog<CSVExport<T>>("Method execution begins to create string to be exported to audit file.", "Export", Severity.Debug);
            var sb = new StringBuilder();

            //Logs.Instance.ErrorLog<CSVExport<T>>("Get the property Infos of type : AuditTrailExport.", "Export", Severity.Debug);
            //Get properties using reflection.
            var propertyInfos = typeof(T).GetTypeInfo();

            try
            {
                if (includeHeaderLine)
                {
                    //add header line.
                    //Logs.Instance.ErrorLog<CSVExport<T>>("Check each property header if its an excluded property or not.", "Export", Severity.Debug);
                    foreach (var propertyInfo in propertyInfos.DeclaredProperties)
                    {
                        if (ExcludedProperties != null && !ExcludedProperties.Contains(propertyInfo.Name))
                        {
                            sb.Append(propertyInfo.Name).Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        }
                    }
                    sb.Remove(sb.Length - 1, 1).AppendLine();
                    //Logs.Instance.ErrorLog<CSVExport<T>>("Header line included.", "Export", Severity.Debug);
                }
                else
                {
                    //Logs.Instance.ErrorLog<CSVExport<T>>("Header line excluded.", "Export", Severity.Debug);
                }

                //add value for each property.
                //Logs.Instance.ErrorLog<CSVExport<T>>("Check each property value if its an excluded property or not.", "Export", Severity.Debug);
                foreach (T obj in Objects)
                {
                    foreach (var propertyInfo in propertyInfos.DeclaredProperties)
                    {
                        if (ExcludedProperties != null && !ExcludedProperties.Contains(propertyInfo.Name))
                        {
                            sb.Append(MakeValueCsvFriendly(propertyInfo.GetValue(obj, null))).Append(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        }
                    }

                    sb.Remove(sb.Length - 1, 1).AppendLine();
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<CSVExport<T>>(ex, "Export", Severity.Warning);
            }
            //Logs.Instance.ErrorLog<CSVExport<T>>("Method exceuted.Return formatted string.", "Export", Severity.Debug);
            return sb.ToString();
        }

        //export to a file.
        /// <summary>
        /// Exports to file.
        /// </summary>
        /// <param name="path">The path.</param>
        public async void ExportToFile(string path)
        {
            //Logs.Instance.ErrorLog<CSVExport<T>>("Method execution begins to export file to given path.", "ExportToFile", Severity.Debug);
            try
            {
                var storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await storageFolder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                //Logs.Instance.ErrorLog<CSVExport<T>>("Created audit log file in given path.", "ExportToFile", Severity.Debug);
                await FileIO.WriteTextAsync(file, Export());
                //Logs.Instance.ErrorLog<CSVExport<T>>("Logs written to audit file.", "ExportToFile", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<CSVExport<T>>(ex, "ExportToFile", Severity.Warning);
            }
            //Logs.Instance.ErrorLog<CSVExport<T>>("Method executed.", "ExportToFile", Severity.Debug);
        }

        //export as binary data.
        /// <summary>
        /// Exports to bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        public byte[] ExportToBytes()
        {
            return Encoding.UTF8.GetBytes(Export());
        }

        //get the csv value for field.
        /// <summary>
        /// Makes the value CSV friendly.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        private string MakeValueCsvFriendly(object value)
        {
            //Logs.Instance.ErrorLog<CSVExport<T>>("Method execution begins to format the CSV file.", "ExportToBytes", Severity.Debug);
            string output = null;
            try
            {
                if (value == null)
                {
                    //Logs.Instance.ErrorLog<CSVExport<T>>("Null object.Return empty string.", "ExportToBytes", Severity.Debug);
                    return "";
                }

                if (value is DateTime)
                {
                    if ( (int) ((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    {
                        return ((DateTime)value).ToString("yyyy-MM-dd");
                    }
                    return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                }
                output = value.ToString();

                if (output.Contains(",") || output.Contains("\""))
                {
                    output = '"' + output.Replace("\"", "\"\"") + '"';
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<CSVExport<T>>(ex, "MakeValueCsvFriendly", Severity.Warning);
            }
            //Logs.Instance.ErrorLog<CSVExport<T>>("Method executed.Return formatted string.", "ExportToBytes", Severity.Debug);
            return output;
        }
    }
}