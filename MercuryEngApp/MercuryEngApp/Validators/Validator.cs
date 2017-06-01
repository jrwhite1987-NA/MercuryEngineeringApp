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
    public class NumberValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public string ControlName { get; set; }
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

    public class ComboBoxRequiredValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public string ControlName { get; set; }
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

    public class FloatValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public string ControlName { get; set; }
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

    public class MaxLengthValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public int MaxLength { get; set; }
        public string ControlName { get; set; }
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

    public class BlankValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public string ControlName { get; set; }

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

    public class RangeValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }
        public int StartStange { get; set; }
        public int MaxRange { get; set; }
        public string ControlName { get; set; }
        int number = 0;
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

    public class ValidationRules
    {
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
                    //errorList.Append(Environment.NewLine);
                }
            }
            errorMessage = errorList.ToString();
            return !hasError;
        }
    }
}
