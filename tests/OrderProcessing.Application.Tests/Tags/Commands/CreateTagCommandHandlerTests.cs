using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Tags.Commands.CreateTag;
using OrderProcessing.Application.Tags.Queries.GetTagById;
using OrderProcessing.Domain.Entities;
using Xunit;

namespace OrderProcessing.Application.Tests.Tags.Commands;

public class CreateTagCommandHandlerTests
{
    private readonly IApplicationDbContext _contextMock;
    private readonly CreateTagCommandHandler _handler;

    // الـ Constructor هنا بيشتغل قبل كل [Fact]، فبنحضر فيه الحاجات المشتركة
    public CreateTagCommandHandlerTests()
    {
        _contextMock = Substitute.For<IApplicationDbContext>();
        _handler = new CreateTagCommandHandler(_contextMock);

        // تجهيز الـ Mock الخاص بالـ Tags مرة واحدة ببيانات مبدئية
        var existingTags = new List<Tag>
        {
            new Tag { Id = 1, Name = "C#" },
            new Tag { Id = 2, Name = "Angular" }
        };
        var dbSetMock = existingTags.BuildMockDbSet();
        _contextMock.Tags.Returns(dbSetMock);
    }

    [Fact]
    public async Task Handle_TagAlreadyExists_ReturnsFailureResult()
    {
        // Arrange
        var command = new CreateTagCommand("C#"); // "C#" موجود مسبقاً في الداتا اللي جهزناها في الـ Constructor

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(TagErrors.AlreadyExists("C#").Code);

        // التأكد من عدم الحفظ
        await _contextMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_TagIsNew_ReturnsSuccessResultAndSavesToDatabase()
    {
        // Arrange
        var command = new CreateTagCommand("Java"); // "Java" غير موجود في الداتا اللي جهزناها

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // التأكد من الإضافة
        _contextMock.Tags.Received(1).Add(Arg.Is<Tag>(t => t.Name == "Java"));

        // التأكد من الحفظ
        await _contextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    
}