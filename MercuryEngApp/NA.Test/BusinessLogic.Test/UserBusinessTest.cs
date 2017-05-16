// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="UserBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.UserBAL;
using Core.Interfaces.UserInterface;
using Core.Models;
using Core.Models.UsersWithRolesModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace NA.Test.BusinessLogicTest
{
    /// <summary>
    /// Class UserBusinessTest.
    /// </summary>
    [TestClass]
    public class UserBusinessTest
    {
        /// <summary>
        /// Checks the get all users with roles successful.
        /// </summary>
        [TestMethod]
        public void CheckGetAllUsersWithRolesSuccessful()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            UserWithRole userWithAdminRole = new UserWithRole { FirstName = "Mike", LastName = "Tyson", RoleID = 1, RoleName = "Administrator", UserID = 1 };
            UserWithRole userWithUserRole = new UserWithRole { FirstName = "Mike", LastName = "Jakson", RoleID = 2, RoleName = "User", UserID = 2 };

            UserWithRoles usersWithRoles = new UserWithRoles();
            usersWithRoles.UsersRolesDataList.Add(userWithAdminRole);
            usersWithRoles.UsersRolesDataList.Add(userWithUserRole);

            mockUserRepository.Setup(x => x.GetAllUsersWithRoles()).Returns(usersWithRoles);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            UserWithRoles actualResult = userBusiness.GetAllUsersWithRoles();

            //Assert
            Assert.AreEqual<UserWithRoles>(usersWithRoles, actualResult);
        }

        /// <summary>
        /// Checks the get all users with roles failed.
        /// </summary>
        [TestMethod]
        public void CheckGetAllUsersWithRolesFailed()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            UserWithRoles usersWithRolesEmpty = new UserWithRoles();

            mockUserRepository.Setup(x => x.GetAllUsersWithRoles()).Returns(usersWithRolesEmpty);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            UserWithRoles actualResult = userBusiness.GetAllUsersWithRoles();

            //Assert
            Assert.AreEqual<UserWithRoles>(usersWithRolesEmpty, actualResult);
        }

        /// <summary>
        /// Checks the get all users successful.
        /// </summary>
        [TestMethod]
        public void CheckGetAllUsersSuccessful()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            User userAdmin = new User { UserID = 1, FirstName = "Mike", LastName = "Tyson", IsDeleted = 0, IsSysAdmin = 1 };
            User user = new User { UserID = 1, FirstName = "Mike", LastName = "Jakson", IsDeleted = 0, IsSysAdmin = 0 };

            Users users = new Users();
            users.UserList.Add(userAdmin);
            users.UserList.Add(user);

            mockUserRepository.Setup(x => x.GetAllUsers()).Returns(users);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            Users actualResult = userBusiness.GetAllUsers();

            //Assert
            Assert.AreEqual<Users>(users, actualResult);
        }

        /// <summary>
        /// Checks the get all users failed.
        /// </summary>
        [TestMethod]
        public void CheckGetAllUsersFailed()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            Users usersEmpty = new Users();

            mockUserRepository.Setup(x => x.GetAllUsers()).Returns(usersEmpty);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            Users actualResult = userBusiness.GetAllUsers();

            //Assert
            Assert.AreEqual<Users>(usersEmpty, actualResult);
        }

        /// <summary>
        /// Checks the save user is successful.
        /// </summary>
        [TestMethod]
        public void CheckSaveUserIsSuccessful()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            User newUser = new User { UserID = 1, FirstName = "John", LastName = "Dikosta", IsDeleted = 0, IsSysAdmin = 1 };
            mockUserRepository.Setup(x => x.SaveUser(newUser)).Returns(newUser);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            User actualResult = userBusiness.SaveUser(newUser);

            //Assert
            Assert.AreEqual(newUser, actualResult);
        }

        /// <summary>
        /// Checks the save user failed.
        /// </summary>
        [TestMethod]
        public void CheckSaveUserFailed()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            User newUser = new User { UserID = 1, FirstName = "John", LastName = "Dikosta", IsDeleted = 0, IsSysAdmin = 1 };
            User savedUser = null;
            mockUserRepository.Setup(x => x.SaveUser(newUser)).Returns(savedUser);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            User actualResult = userBusiness.SaveUser(newUser);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the get user successful.
        /// </summary>
        [TestMethod]
        public void CheckGetUserSuccessful()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            int userID = 1;
            User user = new User { UserID = 1, FirstName = "Richard", LastName = "Gere", IsDeleted = 0, IsSysAdmin = 1 };
            mockUserRepository.Setup(x => x.GetUser(userID)).Returns(user);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            User actualResult = userBusiness.GetUser(userID);

            //Assert
            Assert.AreEqual(user, actualResult);
        }

        /// <summary>
        /// Checks the get user failed.
        /// </summary>
        [TestMethod]
        public void CheckGetUserFailed()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            int userID = 1;
            User user = new User();
            mockUserRepository.Setup(x => x.GetUser(userID)).Returns(user);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            User actualResult = userBusiness.GetUser(userID);

            //Assert
            Assert.AreEqual(user, actualResult);
        }

        /// <summary>
        /// Checks the update user is successful.
        /// </summary>
        [TestMethod]
        public void CheckUpdateUserIsSuccessful()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            User existingUser = new User { UserID = 1, FirstName = "John", LastName = "Dikosta", IsDeleted = 0, IsSysAdmin = 1 };
            mockUserRepository.Setup(x => x.UpdateUser(existingUser)).Returns(true);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            bool actualResult = userBusiness.UpdateUser(existingUser);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update user failed.
        /// </summary>
        [TestMethod]
        public void CheckUpdateUserFailed()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            User existingUser = new User { UserID = 1, FirstName = "John", LastName = "Dikosta", IsDeleted = 0, IsSysAdmin = 1 };
            mockUserRepository.Setup(x => x.UpdateUser(existingUser)).Returns(false);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            bool actualResult = userBusiness.UpdateUser(existingUser);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the delete user is successful.
        /// </summary>
        [TestMethod]
        public void CheckDeleteUserIsSuccessful()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            int userID = 6;
            mockUserRepository.Setup(x => x.DeleteUser(userID)).Returns(true);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            bool actualResult = userBusiness.DeleteUser(userID);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the delete user failed.
        /// </summary>
        [TestMethod]
        public void CheckDeleteUserFailed()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            int userID = 6;
            mockUserRepository.Setup(x => x.DeleteUser(userID)).Returns(false);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            bool actualResult = userBusiness.DeleteUser(userID);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the search users with roles successful.
        /// </summary>
        [TestMethod]
        public void CheckSearchUsersWithRolesSuccessful()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            UserWithRole userWithAdminRole = new UserWithRole { FirstName = "Mike", LastName = "Tyson", RoleID = 1, RoleName = "Administrator", UserID = 1 };
            UserWithRole userWithAdminRole2 = new UserWithRole { FirstName = "Mike", LastName = "Jakson", RoleID = 1, RoleName = "Administrator", UserID = 2 };

            UserWithRoles usersWithRoles = new UserWithRoles();
            usersWithRoles.UsersRolesDataList.Add(userWithAdminRole);
            usersWithRoles.UsersRolesDataList.Add(userWithAdminRole2);

            mockUserRepository.Setup(x => x.SearchUsers(userWithAdminRole)).Returns(usersWithRoles);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            UserWithRoles actualResult = userBusiness.SearchUsers(userWithAdminRole);

            //Assert
            Assert.AreEqual<UserWithRoles>(usersWithRoles, actualResult);
        }

        /// <summary>
        /// Checks the search users with roles empty.
        /// </summary>
        [TestMethod]
        public void CheckSearchUsersWithRolesEmpty()
        {
            //Create Mock objects
            var mockUserRepository = Mock.Create<IUserRepository>();

            //Arrange
            UserWithRole userWithAdminRole = new UserWithRole { FirstName = "Mike", LastName = "Tyson", RoleID = 1, RoleName = "Administrator", UserID = 1 };
            UserWithRoles usersWithRoles = new UserWithRoles();

            mockUserRepository.Setup(x => x.SearchUsers(userWithAdminRole)).Returns(usersWithRoles);

            //Act
            UserBusiness userBusiness = new UserBusiness(mockUserRepository.Object);
            UserWithRoles actualResult = userBusiness.SearchUsers(userWithAdminRole);

            //Assert
            Assert.AreEqual<UserWithRoles>(usersWithRoles, actualResult);
        }
    }
}