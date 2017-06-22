using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MercuryEngApp.Validators
{
    /// <summary>
    /// Class to Validate Number
    /// Inherits from Validation Rule Class
    /// </summary>
    public class NumberValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Gets or sets the Control Name
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// Override the method of ValidationResult class to validate the fields
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int temp = 0;
            bool valid = true;

            ValidationResult result = new ValidationResult(valid, null);

            if (!int.TryParse(value as string, out temp))
            {
                valid = false;
                result = new ValidationResult(valid, string.Format(ErrorMessage, ControlName));
            }
            return result;
        }
    }

    /// <summary>
    /// Class to Validate ComboBox Required
    /// Inherits from Validation Rule Class
    /// </summary>
    public class ComboBoxRequiredValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Gets or sets the Control Name
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// Override the method of ValidationResult class to validate the fields
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            bool valid = true;

            ValidationResult result = new ValidationResult(valid, null);

            if (value == null || object.Equals(value, string.Empty))
            {
                valid = false;
                result = new ValidationResult(valid, string.Format(ErrorMessage, ControlName));
            }
            return result;
        }
    }

    /// <summary>
    /// Class to Validate Float Value
    /// Inherits from Validation Rule Class
    /// </summary>
    public class FloatValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Gets or sets the Control Name
        /// </summary>
        public string ControlName { get; set; }
        
        /// <summary>
        /// Override the method of ValidationResult class to validate the fields
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            float temp = 0;
            bool valid = true;

            ValidationResult result = new ValidationResult(valid, null);

            if (!float.TryParse(value as string, out temp))
            {
                valid = false;
                result = new ValidationResult(valid, string.Format(ErrorMessage, ControlName));
            }
            return result;
        }
    }

    /// <summary>
    /// Class to Validate Max Length
    /// Inherits from Validation Rule Class
    /// </summary>
    public class MaxLengthValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the Max Length
        /// </summary>
        public int MaxLength { get; set; }
        
        /// <summary>
        /// Gets or sets the Control Name
        /// </summary>
        public string ControlName { get; set; }
        
        /// <summary>
        /// Override the method of ValidationResult class to validate the fields
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            bool valid = true;
            ValidationResult result = new ValidationResult(valid, null);
            if (value.ToString().Length > MaxLength)
            {
                valid = false;
                result = new ValidationResult(valid, string.Format(ErrorMessage, ControlName, MaxLength));
            }
            return result;
        }
    }

    /// <summary>
    /// Class to Validate Blank Value
    /// Inherits from Validation Rule Class
    /// </summary>
    public class BlankValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the Control Name
        /// </summary>
        public string ControlName { get; set; }

        /// <summary>
        /// Override the method of ValidationResult class to validate the fields
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            bool valid = true;
            ValidationResult result = new ValidationResult(valid, null);
            if(value==null || object.Equals(value, string.Empty))
            {
                valid = false;
                result = new ValidationResult(valid, string.Format(ErrorMessage, ControlName));
            }
            return result;
        }
    }

    /// <summary>
    /// Class to Validate Range
    /// Inherits from Validation Rule Class
    /// </summary>
    public class RangeValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the Error Message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the Start Range
        /// </summary>
        public int StartStange { get; set; }

        /// <summary>
        /// Gets or sets the Max Range
        /// </summary>
        public int MaxRange { get; set; }
        
        /// <summary>
        /// Gets or sets the Control Name
        /// </summary>
        public string ControlName { get; set; }
        
        int number = 0;
        
        /// <summary>
        /// Override the method of ValidationResult class to validate the fields
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            bool valid = true;
            ValidationResult result = new ValidationResult(valid, null);

            if (int.TryParse(value.ToString(), out number))
            {
                if (number >= StartStange && number <= MaxRange)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
                result = new ValidationResult(valid, string.Format(ErrorMessage, ControlName, StartStange, MaxRange));
            }
            else
            {
                valid = false;
                result = new ValidationResult(valid, string.Format(ErrorMessage, ControlName, StartStange, MaxRange));
            }
            return result;
        }
    }

    /// <summary>
    /// Class for Validate Rules
    /// </summary>
    public class ValidationRules
    {
        /// <summary>
        /// Validates the Control
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static bool ValidateControl(DependencyObject dependencyObject, out string errorMessage)
        {
            StringBuilder errorList = new StringBuilder();
            bool hasError = Validation.GetHasError(dependencyObject);
            if (hasError)
            {
                ReadOnlyObservableCollection<ValidationError> errors = Validation.GetErrors(dependencyObject);
                foreach (var error in errors)
                {
                    errorList.Append(error.ErrorContent);                    
                }
            }
            errorMessage = errorList.ToString();
            return !hasError;
        }
    }
}
