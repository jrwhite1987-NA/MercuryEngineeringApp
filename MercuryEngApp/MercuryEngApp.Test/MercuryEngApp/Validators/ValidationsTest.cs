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
    [TestClass]
    public class ValidationsTest
    {
        private TestContext testContextInstance;
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

            WriteOutput(result);
            //assert
            Assert.IsFalse(result.IsValid);
        }

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

            WriteOutput(result);
            //assert
            Assert.IsFalse(result.IsValid);
        }

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

            WriteOutput(result);
            //assert
            Assert.IsTrue(result.IsValid);
        }
        #endregion

        #region RangeValidationRule
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
            WriteOutput(result);
        }

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
            WriteOutput(result);
        }
        #endregion

        #region MaxValidationRule
        [TestMethod]
        public void MaxLengthValidationMoreThanMaxLength()
        {
            var validator = new MaxLengthValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.MaxLength = Constants.VALUE_10;
            validator.ErrorMessage = Resources.MaxLength;

            var result = validator.Validate(13213213133, new System.Globalization.CultureInfo("en-US"));

            WriteOutput(result);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void MaxLengthValidationEqualToMaxLength()
        {
            var validator = new MaxLengthValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.MaxLength = Constants.VALUE_10;
            validator.ErrorMessage = Resources.MaxLength;

            var result = validator.Validate("1234567890", new System.Globalization.CultureInfo("en-US"));

            WriteOutput(result);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void MaxLengthValidationLessThanMaxLength()
        {
            var validator = new MaxLengthValidationRule();
            validator.ControlName = "Test";
            validator.ValidatesOnTargetUpdated = true;
            validator.MaxLength = Constants.VALUE_10;
            validator.ErrorMessage = Resources.MaxLength;

            var result = validator.Validate("123", new System.Globalization.CultureInfo("en-US"));

            WriteOutput(result);
            Assert.IsTrue(result.IsValid);
        }
        #endregion

        #region FloatValidationRule
        [TestMethod]
        public void FloatValidationWithNonFloat()
        {
            var validator = new FloatValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeFloat;
            validator.ValidatesOnTargetUpdated = true;

            var result = validator.Validate("FFFFF", new System.Globalization.CultureInfo("en-US"));

            WriteOutput(result);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void FloatValidationWithFloat()
        {
            var validator = new FloatValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeFloat;
            validator.ValidatesOnTargetUpdated = true;

            var result = validator.Validate("134554", new System.Globalization.CultureInfo("en-US"));

            WriteOutput(result);
            Assert.IsTrue(result.IsValid);
        }
        #endregion

        #region NumberValidationRule
        [TestMethod]
        public void NumberValidateWithNonInteger()
        {
            var validator = new NumberValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeNumber;

            var result = validator.Validate("123.12", new System.Globalization.CultureInfo("en-US"));

            WriteOutput(result);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void NumberValidateWithInteger()
        {
            var validator = new NumberValidationRule();
            validator.ControlName = "Test";
            validator.ErrorMessage = Resources.MustBeNumber;

            var result = validator.Validate("123", new System.Globalization.CultureInfo("en-US"));

            WriteOutput(result);
            Assert.IsTrue(result.IsValid);
        }
        #endregion

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
