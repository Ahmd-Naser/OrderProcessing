using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Tags.Commands.DeleteTag;
using OrderProcessing.Domain.Entities;
 
namespace OrderProcessing.Application.Tests.Tags.Commands;

public class DeleteTagCommandHandlerTests
{
    private readonly IApplicationDbContext _contextMock;
    private readonly DeleteTagCommandHandler _handler;
    public DeleteTagCommandHandlerTests()
    {
        _contextMock = Substitute.For<IApplicationDbContext>();
        _handler = new DeleteTagCommandHandler(_contextMock);

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
    public async Task Handle_TagDoesNotExist_ReturnsFailureResult()
    {
        var command = new DeleteTagCommand(99); // نبحث عن Tag غير موجود


        var result = await _handler.Handle(command, CancellationToken.None);


        result.IsFailure.Should().BeTrue ();

        result.Error.Should().Be(TagErrors.NotFound(99));

    }

    [Fact]
    public async Task Handle_TagExistsAndDeletionSucceeded_ReturnsSuccessResult()
    {
        var command = new DeleteTagCommand(1); // نبحث عن Tag موجود مسبقاً (C#)

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

    }

}