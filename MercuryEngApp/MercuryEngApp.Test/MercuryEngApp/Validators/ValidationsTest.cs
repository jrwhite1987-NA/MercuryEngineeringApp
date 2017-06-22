using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MercuryEngApp.Validators;
using System.Windows.Controls;
using Core.Constants;

namespace MercuryEngApp.Test.MercuryEngApp.Validators
{
    /// <summary>
    /// Test Class for Validations Unit testing
    /// </summary>
    [TestClass]
    public class ValidationsTest
    {
        private TestContext testContextInstance;
        /// <summary>
        /// Gets or sets the TestContext
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region BlankValidationRule
        /// <summary>
        /// Test Method to validate blank with null
        /// </summary>
        [TestMethod]
        public void BlankValidationWithNull()
        {
            //arrange
            var validator = new BlankValidationRule();
            validator.ControlName = "TextBox";
            validator.ValidatesOnTargetUpdated = true;
            validator.ErrorMessage = "{0} cannot be blank";
            System.Globalization.CultureInfo MyCulture = new System.Globalization.CultureInfo("en-US");
            
            //act
            var result = validator.Validate(null, MyCulture);

            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Test Method to validate blank
        /// </summary>
        [TestMethod]
        public void BlankValidationWithBlank()
        {
            //arrange
            var validator = new BlankValidationRule();
            validator.ControlName = "TextBox";
            validator.ValidatesOnTargetUpdated = true;
            validator.ErrorMessage = "{0} cannot be blank";
            System.Globalization.CultureInfo MyCulture = new System.Globalization.CultureInfo("en-US");

            //act
            var result = validator.Validate(string.Empty, MyCulture);

            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Test Method to validate blank with non blank
        /// </summary>
        [TestMethod]
        public void BlankValidationWithNonBlank()
        {
            //arrange
            var validator = new BlankValidationRule();
            validator.ControlName = "TextBox";
            validator.ValidatesOnTargetUpdated = true;
            validator.ErrorMessage = "{0} cannot be blank";
            System.Globalization.CultureInfo MyCulture = new System.Globalization.CultureInfo("en-US");

            //act
            var result = validator.Validate("Test", MyCulture);
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsTrue(result.IsValid);
        }
        #endregion

        #region RangeValidationRule
        /// <summary>
        /// Test Method to validate Range with out of range
        /// </summary>
        [TestMethod]
        public void RangeValidationOutOfRange()
        {
            //arrange
            var validator = new RangeValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.StartStange = Constants.VALUE_0;
            validator.MaxRange = Constants.VALUE_255;
            validator.ErrorMessage = Resources.RangeValue;

            //act
            var result = validator.Validate(Constants.VALUE_256, new System.Globalization.CultureInfo("en-US"));

            //Assert
            Assert.IsFalse(result.IsValid);
            //Write output in Test Context
            WriteOutput(result);
        }

        /// <summary>
        /// Test Method to validate Range within the range
        /// </summary>
        [TestMethod]
        public void RangeValidationWithInRange()
        {
            //arrange
            var validator = new RangeValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.StartStange = Constants.VALUE_0;
            validator.MaxRange = Constants.VALUE_255;
            validator.ErrorMessage = Resources.RangeValue;

            //act
            var result = validator.Validate(Constants.VALUE_12, new System.Globalization.CultureInfo("en-US"));

            //Assert
            Assert.IsTrue(result.IsValid);
            //Write output in Test Context
            WriteOutput(result);
        }
        #endregion

        #region MaxValidationRule
        /// <summary>
        /// Test Method to validate Maxlength more than specified max length
        /// </summary>
        [TestMethod]
        public void MaxLengthValidationMoreThanMaxLength()
        {
            //arrange
            var validator = new MaxLengthValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.MaxLength = Constants.VALUE_10;
            validator.ErrorMessage = Resources.MaxLength;

            //act
            var result = validator.Validate(13213213133, new System.Globalization.CultureInfo("en-US"));
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Test Method to validate Maxlength equal to specified max length
        /// </summary>
        [TestMethod]
        public void MaxLengthValidationEqualToMaxLength()
        {
            //arrange
            var validator = new MaxLengthValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.MaxLength = Constants.VALUE_10;
            validator.ErrorMessage = Resources.MaxLength;

            //act
            var result = validator.Validate("1234567890", new System.Globalization.CultureInfo("en-US"));
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsTrue(result.IsValid);
        }

        /// <summary>
        /// Test Method to validate Maxlength less than specified max length
        /// </summary>
        [TestMethod]
        public void MaxLengthValidationLessThanMaxLength()
        {
            //arrange
            var validator = new MaxLengthValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.MaxLength = Constants.VALUE_10;
            validator.ErrorMessage = Resources.MaxLength;

            //act
            var result = validator.Validate("123", new System.Globalization.CultureInfo("en-US"));
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsTrue(result.IsValid);
        }
        #endregion

        #region FloatValidationRule
        /// <summary>
        /// Test method to validate float with non float value
        /// </summary>
        [TestMethod]
        public void FloatValidationWithNonFloat()
        {
            //arrange
            var validator = new FloatValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeFloat;
            validator.ValidatesOnTargetUpdated = true;

            //act
            var result = validator.Validate("FFFFF", new System.Globalization.CultureInfo("en-US"));
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Test method to validate float with float value
        /// </summary>
        [TestMethod]
        public void FloatValidationWithFloat()
        {
            //arrange
            var validator = new FloatValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeFloat;
            validator.ValidatesOnTargetUpdated = true;

            //act
            var result = validator.Validate("134554", new System.Globalization.CultureInfo("en-US"));
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsTrue(result.IsValid);
        }
        #endregion

        #region NumberValidationRule
        /// <summary>
        /// Test method to validate number with non number
        /// </summary>
        [TestMethod]
        public void NumberValidateWithNonInteger()
        {
            //arrange
            var validator = new NumberValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeNumber;

            //act
            var result = validator.Validate("123.12", new System.Globalization.CultureInfo("en-US"));
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsFalse(result.IsValid);
        }

        /// <summary>
        /// Test method to validate number with number
        /// </summary>
        [TestMethod]
        public void NumberValidateWithInteger()
        {
            //arrange
            var validator = new NumberValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeNumber;

            //act
            var result = validator.Validate("123", new System.Globalization.CultureInfo("en-US"));
            //Write output in Test Context
            WriteOutput(result);
            //assert
            Assert.IsTrue(result.IsValid);
        }
        #endregion

        /// <summary>
        /// Write output in Test Context
        /// </summary>
        /// <param name="result"></param>
        private void WriteOutput(ValidationResult result)
        {
            if (result.IsValid)
            {
                TestContext.WriteLine("{0}", "Success");
            }
            else
            {
                TestContext.WriteLine("{0}", result.ErrorContent);
            }
        }
    }
}
