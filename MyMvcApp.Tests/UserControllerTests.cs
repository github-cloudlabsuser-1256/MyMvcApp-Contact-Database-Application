using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using System.Linq;

namespace MyMvcApp.Tests;

public class UserControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult_WithUserList()
    {
        // Arrange
        UserController.userlist.Clear();
        UserController.userlist.Add(new User { Id = 1, Name = "Test", Email = "test@test.com" });
        var controller = new UserController();

        // Act
        var result = controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<System.Collections.Generic.List<User>>(result.Model);
        Assert.NotNull(result.Model);
        Assert.Single((System.Collections.Generic.List<User>)result.Model);
    }

    [Fact]
    public void Details_ReturnsViewResult_WhenUserExists()
    {
        // Arrange
        UserController.userlist.Clear();
        UserController.userlist.Add(new User { Id = 2, Name = "User2", Email = "user2@test.com" });
        var controller = new UserController();

        // Act
        var result = controller.Details(2) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
        var user = result.Model as User;
        Assert.NotNull(user);
        Assert.Equal(2, user.Id);
    }

    [Fact]
    public void Details_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        UserController.userlist.Clear();
        var controller = new UserController();

        // Act
        var result = controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Create_Post_AddsUser_AndRedirects()
    {
        // Arrange
        UserController.userlist.Clear();
        var controller = new UserController();
        var user = new User { Name = "NewUser", Email = "new@user.com" };

        // Act
        var result = controller.Create(user) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Single(UserController.userlist);
        Assert.Equal("NewUser", UserController.userlist.First().Name);
    }

    [Fact]
    public void Edit_Post_UpdatesUser_AndRedirects()
    {
        // Arrange
        UserController.userlist.Clear();
        UserController.userlist.Add(new User { Id = 5, Name = "Old", Email = "old@user.com" });
        var controller = new UserController();
        var updatedUser = new User { Id = 5, Name = "Updated", Email = "updated@user.com" };

        // Act
        var result = controller.Edit(5, updatedUser) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Updated", UserController.userlist.First().Name);
    }

    [Fact]
    public void Delete_Post_RemovesUser_AndRedirects()
    {
        // Arrange
        UserController.userlist.Clear();
        UserController.userlist.Add(new User { Id = 10, Name = "ToDelete", Email = "delete@user.com" });
        var controller = new UserController();

        // Act
        var emptyForm = new Microsoft.AspNetCore.Http.FormCollection(new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());
        var result = controller.Delete(10, emptyForm) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Empty(UserController.userlist);
    }
}