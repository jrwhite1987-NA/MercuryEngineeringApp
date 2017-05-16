// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="UserRoleBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.UserRoleBAL;
using Core.Interfaces.UserRoleInterface;
using Core.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class UserRoleBusinessTest.
    /// </summary>
    [TestClass]
    public class UserRoleBusinessTest
    {
        /// <summary>
        /// The admin role
        /// </summary>
        private Role ADMIN_ROLE = null;

        /// <summary>
        /// The user role
        /// </summary>
        private Role USER_ROLE = null;

        /// <summary>
        /// The user admin
        /// </summary>
        private User USER_ADMIN = null;

        /// <summary>
        /// The user
        /// </summary>
        private User USER = null;

        /// <summary>
        /// The role identifier
        /// </summary>
        private const int ROLE_ID = 1;

        /// <summary>
        /// The user identifier
        /// </summary>
        private const int USER_ID = 1;

        /// <summary>
        /// The no exception thrown
        /// </summary>
        private const string NO_EXCEPTION_THROWN = "No Exception Thrown";

        /// <summary>
        /// The users
        /// </summary>
        private Users users = null;

        /// <summary>
        /// The roles
        /// </summary>
        private Roles roles = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleBusinessTest"/> class.
        /// </summary>
        public UserRoleBusinessTest()
        {
            const int ADMIN_ROLE_ID = 1;
            const int USER_ROLE_ID = 2;

            const int ADMIN_USER_ID = 1;
            const int USER_USER_ID = 1;

            const int IS_DELETED = 0;
            const int SYS_ADMIN = 1;
            const int NON_SYS_ADMIN = 0;

            users = new Users();
            users.UserList.Add(USER_ADMIN);
            users.UserList.Add(USER);

            roles = new Roles();
            roles.RoleList.Add(ADMIN_ROLE);
            roles.RoleList.Add(USER_ROLE);

            ADMIN_ROLE = new Role { RoleID = ADMIN_ROLE_ID, RoleName = "Administrator", IsDeleted = false };
            USER_ROLE = new Role { RoleID = USER_ROLE_ID, RoleName = "User", IsDeleted = false };
            USER_ADMIN = new User { UserID = ADMIN_USER_ID, FirstName = "Mike", LastName = "Tyson", IsDeleted = IS_DELETED, IsSysAdmin = SYS_ADMIN };
            USER = new User { UserID = USER_USER_ID, FirstName = "Mike", LastName = "Jakson", IsDeleted = IS_DELETED, IsSysAdmin = NON_SYS_ADMIN };
        }

        /// <summary>
        /// Verifies the assign roles to user.
        /// </summary>
        [TestMethod]
        public void VerifyAssignRolesToUser()
        {
            //Create Mock objects
            var mockUserRoleRepository = Mock.Create<IUserRoleRepository>();

            mockUserRoleRepository.Setup(x => x.AssignRolesToUser(USER_ID, roles)).Verifiable();

            //Act
            UserRoleBusiness userRoleBusiness = new UserRoleBusiness(mockUserRoleRepository.Object);
            userRoleBusiness.AssignRolesToUser(USER_ID, roles);

            //Assert
            mockUserRoleRepository.Verify(x => x.AssignRolesToUser(It.IsInRange<int>(1, 1, Range.Inclusive), It.Is<Roles>(y => y != null)));
        }

        /// <summary>
        /// Checks the delete roles of user successful.
        /// </summary>
        [TestMethod]
        public void CheckDeleteRolesOfUserSuccessful()
        {
            //Create Mock objects
            var mockUserRoleRepository = Mock.Create<IUserRoleRepository>();

            mockUserRoleRepository.Setup(x => x.DeleteRolesOfUser(USER_ID)).Returns(true);

            //Act
            UserRoleBusiness userRoleBusiness = new UserRoleBusiness(mockUserRoleRepository.Object);
            bool actualResult = userRoleBusiness.DeleteRolesOfUser(USER_ID);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the delete roles of user identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckDeleteRolesOfUserIdNotExist()
        {
            //Create Mock objects
            var mockUserRoleRepository = Mock.Create<IUserRoleRepository>();

            mockUserRoleRepository.Setup(x => x.DeleteRolesOfUser(USER_ID)).Returns(false);

            //Act
            UserRoleBusiness userRoleBusiness = new UserRoleBusiness(mockUserRoleRepository.Object);
            bool actualResult = userRoleBusiness.DeleteRolesOfUser(USER_ID);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the hard delete roles of user successful.
        /// </summary>
        [TestMethod]
        public void CheckHardDeleteRolesOfUserSuccessful()
        {
            //Create Mock objects
            var mockUserRoleRepository = Mock.Create<IUserRoleRepository>();

            mockUserRoleRepository.Setup(x => x.HardDeleteRolesOfUser(USER_ID)).Returns(true);

            //Act
            UserRoleBusiness userRoleBusiness = new UserRoleBusiness(mockUserRoleRepository.Object);
            bool actualResult = userRoleBusiness.HardDeleteRolesOfUser(USER_ID);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the hard delete roles of user identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckHardDeleteRolesOfUserIdNotExist()
        {
            //Create Mock objects
            var mockUserRoleRepository = Mock.Create<IUserRoleRepository>();

            mockUserRoleRepository.Setup(x => x.HardDeleteRolesOfUser(USER_ID)).Returns(false);

            //Act
            UserRoleBusiness userRoleBusiness = new UserRoleBusiness(mockUserRoleRepository.Object);
            bool actualResult = userRoleBusiness.HardDeleteRolesOfUser(USER_ID);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the get roles by user identifier successful.
        /// </summary>
        [TestMethod]
        public void CheckGetRolesByUserIdSuccessful()
        {
            //Create Mock objects
            var mockUserRoleRepository = Mock.Create<IUserRoleRepository>();

            mockUserRoleRepository.Setup(x => x.GetRolesByUserId(USER_ID)).Returns(roles);

            //Act
            UserRoleBusiness userRoleBusiness = new UserRoleBusiness(mockUserRoleRepository.Object);
            Roles actualRoles = userRoleBusiness.GetRolesByUserId(USER_ID);

            //Assert
            Assert.AreEqual(actualRoles, roles);
        }

        /// <summary>
        /// Checks the get roles by user identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetRolesByUserIdNotExist()
        {
            //Create Mock objects
            var mockUserRoleRepository = Mock.Create<IUserRoleRepository>();

            Roles emptyRole = new Roles();

            mockUserRoleRepository.Setup(x => x.GetRolesByUserId(USER_ID)).Returns(emptyRole);

            //Act
            UserRoleBusiness userRoleBusiness = new UserRoleBusiness(mockUserRoleRepository.Object);
            Roles actualRoles = userRoleBusiness.GetRolesByUserId(USER_ID);

            //Assert
            Assert.AreEqual(actualRoles, emptyRole);
        }
    }
}