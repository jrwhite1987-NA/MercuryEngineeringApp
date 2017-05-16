// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="LoginBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.LoginBAL;
using Core.Constants;
using Core.Interfaces.LoginInterface;
using Core.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class LoginBusinessTest.
    /// </summary>
    [TestClass]
    public class LoginBusinessTest
    {
        /// <summary>
        /// Checks the validate login credentials.
        /// </summary>
        [TestMethod]
        public void CheckValidateLoginCredentials()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string userName = "admin";
            string password = "password";

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.ValidateLoginCredentials(userName)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateLoginCredentials(userName, password);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the validate login credentials null.
        /// </summary>
        [TestMethod]
        public void CheckValidateLoginCredentialsNull()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string userName = null;
            string password = null;

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.ValidateLoginCredentials(userName)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateLoginCredentials(userName, password);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the validate login credentials username null.
        /// </summary>
        [TestMethod]
        public void CheckValidateLoginCredentialsUsernameNull()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string userName = null;
            string password = "password";

            mockLoginRepository.Setup(x => x.ValidateLoginCredentials(userName)).Returns(new Login());

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateLoginCredentials(userName, password);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the validate login credentials password null.
        /// </summary>
        [TestMethod]
        public void CheckValidateLoginCredentialsPasswordNull()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string userName = "admin";
            string password = null;

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.ValidateLoginCredentials(userName)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateLoginCredentials(userName, password);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the generate salt key.
        /// </summary>
        [TestMethod]
        public void CheckGenerateSaltKey()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.GenerateSaltKey();

            //Assert
            Assert.IsTrue(actualResult is string);
        }

        /// <summary>
        /// Checks the generate final string.
        /// </summary>
        [TestMethod]
        public void CheckGenerateFinalString()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string password = "password";
            string saltKey = "M+nvo+eE";
            string saltedFinalString = "M+nvo+eE" + Constants.ApplicationConstant + "password";

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.GenerateFinalString(password, saltKey);

            //Assert
            Assert.AreEqual(saltedFinalString, actualResult);
        }

        /// <summary>
        /// Checks the generate hashed password successful.
        /// </summary>
        [TestMethod]
        public void CheckGenerateHashedPasswordSuccessful()
        {
            //Create Mock object
            var mockBusinessRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string saltedFinalString = "M+nvo+eE" + Constants.ApplicationConstant + "password";
            string hashAlgorithmName = Constants.HashAlgorithm;
            string hashedPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw=";

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockBusinessRepository.Object);
            var actualResult = loginBusiness.GenerateHashedPassword(saltedFinalString, hashAlgorithmName);

            //Assert
            Assert.AreEqual(hashedPassword, actualResult);
        }

        /// <summary>
        /// Checks the generate hashed password salt string null.
        /// </summary>
        [TestMethod]
        public void CheckGenerateHashedPasswordSaltStringNull()
        {
            //Create Mock object
            var mockBusinessRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string saltedFinalString = "M+nvo+eE" + Constants.ApplicationConstant + "password";
            string hashAlgorithmName = null;
            string hashedPassword = string.Empty;

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockBusinessRepository.Object);
            var actualResult = loginBusiness.GenerateHashedPassword(saltedFinalString, hashAlgorithmName);

            //Assert
            Assert.AreEqual(hashedPassword, actualResult);
        }

        /// <summary>
        /// Checks the generate hashed password hash algorithm name null.
        /// </summary>
        [TestMethod]
        public void CheckGenerateHashedPasswordHashAlgorithmNameNull()
        {
            //Create Mock object
            var mockBusinessRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string saltedFinalString = null;
            string hashAlgorithmName = Constants.HashAlgorithm;
            string hashedPassword = string.Empty;

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockBusinessRepository.Object);
            var actualResult = loginBusiness.GenerateHashedPassword(saltedFinalString, hashAlgorithmName);

            //Assert
            Assert.AreEqual(hashedPassword, actualResult);
        }

        /// <summary>
        /// Checks the validate hashed password success.
        /// </summary>
        [TestMethod]
        public void CheckValidateHashedPasswordSuccess()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string password = "password";

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateHashedPassword(password, login);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the validate hashed password null parameters.
        /// </summary>
        [TestMethod]
        public void CheckValidateHashedPasswordNullParameters()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string password = null;

            Login login = null;

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateHashedPassword(password, login);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the validate hashed password null.
        /// </summary>
        [TestMethod]
        public void CheckValidateHashedPasswordNull()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string password = null;

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateHashedPassword(password, login);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the validate hashed password login null.
        /// </summary>
        [TestMethod]
        public void CheckValidateHashedPasswordLoginNull()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string password = "password";

            Login login = null;

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateHashedPassword(password, login);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the save login success.
        /// </summary>
        [TestMethod]
        public void CheckSaveLoginSuccess()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string loginName = "admin";
            string password = "password";
            int userId = 1;
            bool isFirstLogin = false;
            string saltKey = "M+nvo+eE";
            string hashedPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw=";

            mockLoginRepository.Setup(x => x.SaveLogin(loginName, saltKey, hashedPassword, userId, isFirstLogin)).Returns(true);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.SaveLogin(loginName, password, userId, isFirstLogin);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the delete login of user.
        /// </summary>
        [TestMethod]
        public void CheckDeleteLoginOfUser()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;

            mockLoginRepository.Setup(x => x.DeleteLoginByUserId(userId)).Returns(true);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.DeleteLoginByUserId(userId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the delete login of user identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckDeleteLoginOfUserIdNotExist()
        {
            //Create Mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;

            mockLoginRepository.Setup(x => x.DeleteLoginByUserId(userId)).Returns(false);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.DeleteLoginByUserId(userId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the get login by user identifier.
        /// </summary>
        [TestMethod]
        public void CheckGetLoginByUserID()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.GetLoginByUserID(userId)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.GetLoginByUserID(userId);

            //Assert
            Assert.AreEqual(login, actualResult);
        }

        /// <summary>
        /// Checks the get login by user identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetLoginByUserIDNotExist()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            Login login = new Login();

            mockLoginRepository.Setup(x => x.GetLoginByUserID(userId)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.GetLoginByUserID(userId);

            //Assert
            Assert.AreEqual(login, actualResult);
        }

        /// <summary>
        /// Checks the name of the get login by login.
        /// </summary>
        [TestMethod]
        public void CheckGetLoginByLoginName()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string loginName = "admin";
            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.GetLoginByLoginName(loginName)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.GetLoginByLoginName(loginName);

            //Assert
            Assert.AreEqual(login, actualResult);
        }

        /// <summary>
        /// Checks the get login by login name null.
        /// </summary>
        [TestMethod]
        public void CheckGetLoginByLoginNameNull()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            string loginName = null;
            Login login = new Login();

            mockLoginRepository.Setup(x => x.GetLoginByLoginName(loginName)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.GetLoginByLoginName(loginName);

            //Assert
            Assert.AreEqual(login, actualResult);
        }

        /// <summary>
        /// Checks the update password.
        /// </summary>
        [TestMethod]
        public void CheckUpdatePassword()
        {
            //create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            string newPassword = "Password";
            bool isFirstLogin = true;

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = true,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.UpdatePassword(login)).Returns(true);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.UpdatePassword(newPassword, userId, isFirstLogin);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update password is first login false.
        /// </summary>
        [TestMethod]
        public void CheckUpdatePasswordIsFirstLoginFalse()
        {
            //create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            string newPassword = "Password";
            bool isFirstLogin = false;

            mockLoginRepository.Setup(x => x.UpdatePassword(It.IsAny<Login>())).Returns(false);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.UpdatePassword(newPassword, userId, isFirstLogin);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update na tech login success.
        /// </summary>
        [TestMethod]
        public void CheckUpdateNaTechLoginSuccess()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            string newPassword = "password";
            bool isFirstLogin = true;
            string loginName = "na_service_tech";
            Login login = new Login
            {
                UserID = 1,
                IsFirstLogin = true,
                LoginName = "na_service_tech",
                LastLogin = DateTime.Now,
                HashPassword = "hash",
                SaltKey = "saltKey"
            };

            mockLoginRepository.Setup(x => x.UpdateNaTechLogin(login)).Returns(true);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.UpdateNaTechLogin(newPassword, userId, isFirstLogin, loginName);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update na tech login failed.
        /// </summary>
        [TestMethod]
        public void CheckUpdateNaTechLoginFailed()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            string newPassword = "password";
            bool isFirstLogin = true;
            string loginName = "na_service_tech";
            Login login = new Login
            {
                UserID = 1,
                IsFirstLogin = true,
                LoginName = "na_service_tech",
                LastLogin = DateTime.Now,
                HashPassword = "hash",
                SaltKey = "saltKey"
            };

            mockLoginRepository.Setup(x => x.UpdateNaTechLogin(login)).Returns(false);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.UpdateNaTechLogin(newPassword, userId, isFirstLogin, loginName);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the validate old password.
        /// </summary>
        [TestMethod]
        public void CheckValidateOldPassword()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            string oldPassword = "password";

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.GetLoginByUserID(userId)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateOldPassword(userId, oldPassword);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the validate old password null.
        /// </summary>
        [TestMethod]
        public void CheckValidateOldPasswordNull()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            string oldPassword = null;

            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginRepository.Setup(x => x.GetLoginByUserID(userId)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateOldPassword(userId, oldPassword);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the validate old password login null.
        /// </summary>
        [TestMethod]
        public void CheckValidateOldPasswordLoginNull()
        {
            //Create mock object
            var mockLoginRepository = Mock.Create<ILoginRepository>();

            //Arrange
            int userId = 1;
            string oldPassword = "password";

            Login login = null;

            mockLoginRepository.Setup(x => x.GetLoginByUserID(userId)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginRepository.Object);
            var actualResult = loginBusiness.ValidateOldPassword(userId, oldPassword);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the name of the validate user.
        /// </summary>
        [TestMethod]
        public void CheckValidateUserName()
        {
            //create mock object
            var mockLoginBusiness = Mock.Create<ILoginRepository>();

            //Arrange
            string userName = "admin";
            Login login = new Login
            {
                UserID = 1,
                LoginName = "admin",
                SaltKey = "M+nvo+eE",
                IsFirstLogin = false,
                LastLogin = DateTime.Now,
                HashPassword = "XFbsIm47olpNHQhfngwlC2YBElMQNs3m/SHnb5jt8bw="
            };

            mockLoginBusiness.Setup(x => x.ValidateUserName(userName)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginBusiness.Object);
            var actualResult = loginBusiness.ValidateUserName(userName);

            //Assert
            Assert.AreEqual(login, actualResult);
        }

        /// <summary>
        /// Checks the validate user name null.
        /// </summary>
        [TestMethod]
        public void CheckValidateUserNameNull()
        {
            //create mock object
            var mockLoginBusiness = Mock.Create<ILoginRepository>();

            //Arrange
            string userName = null;
            Login login = new Login();

            mockLoginBusiness.Setup(x => x.ValidateUserName(userName)).Returns(login);

            //Act
            LoginBusiness loginBusiness = new LoginBusiness(mockLoginBusiness.Object);
            var actualResult = loginBusiness.ValidateUserName(userName);

            //Assert
            Assert.AreEqual(login, actualResult);
        }
    }
}