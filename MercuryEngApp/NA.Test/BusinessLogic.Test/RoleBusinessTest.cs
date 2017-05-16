// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="RoleBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.RoleBAL;
using Core.Interfaces.RoleInterface;
using Core.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class RoleBusinessTest.
    /// </summary>
    [TestClass]
    public class RoleBusinessTest
    {
        /// <summary>
        /// Checks the get all roles successful.
        /// </summary>
        [TestMethod]
        public void CheckGetAllRolesSuccessful()
        {
            //Create Mock objects
            var mockRoleRepository = Mock.Create<IRoleRepository>();

            //Arrange
            Role adminRole = new Role { RoleID = 1, RoleName = "Administrator", IsDeleted = false };
            Role userRole = new Role { RoleID = 2, RoleName = "User", IsDeleted = false };

            Roles roles = new Roles();
            roles.RoleList.Add(adminRole);
            roles.RoleList.Add(userRole);

            mockRoleRepository.Setup(x => x.GetAllRoles()).Returns(roles);

            //Act
            RoleBusiness roleBusiness = new RoleBusiness(mockRoleRepository.Object);
            Roles actualRoles = roleBusiness.GetAllRoles();

            //Assert
            Assert.AreEqual(actualRoles, roles);
        }

        /// <summary>
        /// Checks the get all roles failed.
        /// </summary>
        [TestMethod]
        public void CheckGetAllRolesFailed()
        {
            //Create Mock objects
            var mockRoleRepository = Mock.Create<IRoleRepository>();

            //Arrange
            Roles roles = new Roles();

            mockRoleRepository.Setup(x => x.GetAllRoles()).Returns(roles);

            //Act
            RoleBusiness roleBusiness = new RoleBusiness(mockRoleRepository.Object);
            Roles actualRoles = roleBusiness.GetAllRoles();

            //Assert
            Assert.AreEqual(actualRoles, roles);
        }
    }
}