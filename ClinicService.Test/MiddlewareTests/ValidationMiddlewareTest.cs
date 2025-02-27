using System.ComponentModel.DataAnnotations;
using System.Net;
using ClinicService.API.Middleware;
using ClinicService.BLL.Utilities.Messages;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ClinicService.Test.MiddlewareTests;
public class ValidationMiddlewareTest
{
    [Fact]
    public async Task InvokeAsync_ValidationException_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextMock = new Mock<RequestDelegate>();
        var exceptionMiddleware = new ExceptionMiddleware(nextMock.Object);
        var validationException = new ValidationException(NotificationMessages.validationExeptionMessage);

        nextMock.Setup(next => next(context)).ThrowsAsync(validationException);

        // Act
        await exceptionMiddleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_OtherException_Returns500InternalServerError()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextMock = new Mock<RequestDelegate>();
        var exceptionMiddleware = new ExceptionMiddleware(nextMock.Object);
        var unexpectedException = new Exception("Something went wrong");

        nextMock.Setup(next => next(context)).ThrowsAsync(unexpectedException);

        // Act
        await exceptionMiddleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_NoException_CallsNextMiddleware()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var nextMock = new Mock<RequestDelegate>();
        var exceptionMiddleware = new ExceptionMiddleware(nextMock.Object);

        // Act
        await exceptionMiddleware.InvokeAsync(context);

        // Assert
        nextMock.Verify(next => next(context), Times.Once);
    }
}
