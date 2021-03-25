using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using NETCORE.Interface;
using NETCORE.Models;
using System.Linq;
using NETCORE.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestNETCORE
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexReturnsAViewResultWithAListOfUsers()
        {
            var mock = new Mock<IUser>();
            mock.Setup(IUser => IUser.Alls()).Returns(GetTestUsers());
            var controller = new HomeController(mock.Object);
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.Model);
            Assert.Equal(GetTestUsers().Count, model.Count());
        }
        private List<User> GetTestUsers()
        {
            var users = new List<User>
            {
                new User { Id=1, Name="Tom", Age=35},
                new User { Id=2, Name="Alice", Age=29},
                new User { Id=3, Name="Sam", Age=32}
            };
            return users;
        }
        [Fact]
        public void AddUserReturns()
        {
            var mock = new Mock<IUser>();
            var controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            User newUser = new User();
            var result = controller.AddUser(newUser);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(newUser, viewResult?.Model);
        }

        [Fact]
        public void RedirectAndAddsUser()
        {
            // Arrange
            var mock = new Mock<IUser>();
            var controller = new HomeController(mock.Object);
            var newUser = new User()
            {
                Name = "Ben"
            };
            var result = controller.AddUser(newUser);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mock.Verify(r => r.Create(newUser));
        }

        [Fact]
        public void GetUserReturnsBadRequestResultWhenIdIsNull()
        {
            var mock = new Mock<IUser>();
            var controller = new HomeController(mock.Object);
            var result = controller.GetUser(null);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetUserReturnsNotFoundResultWhenUserNotFound()
        {
            int testUserId = 6;
            var mock = new Mock<IUser>();
            mock.Setup(repo => repo.Get(testUserId))
                .Returns(null as User);
            var controller = new HomeController(mock.Object);
            var result = controller.GetUser(testUserId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetUserReturnsViewResultWithUser()
        {
            int testUserId = 1;
            var mock = new Mock<IUser>();
            mock.Setup(repo => repo.Get(testUserId))
                .Returns(GetTestUsers().FirstOrDefault(p => p.Id == testUserId));
            var controller = new HomeController(mock.Object);
            var result = controller.GetUser(testUserId);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<User>(viewResult.ViewData.Model);
            Assert.Equal("Tom", model.Name);
            Assert.Equal(35, model.Age);
            Assert.Equal(testUserId, model.Id);
        }
    }
}
