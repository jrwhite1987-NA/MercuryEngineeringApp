using log4net;
using System.IO;
namespace Core.Common
{
    public class Helper
    {
        public static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Byteses to string converted.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <returns>System.String.</returns>
        public static string ConvertBytesToString(byte[] bytes, int index, int count)
        {
            using (var stream = new MemoryStream(bytes, index, count))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}