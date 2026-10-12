using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Tags.Commands.CreateTag;
using OrderProcessing.Application.Tags.Queries.GetTagById;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Application.Tests.Tags.Queries;

public class GetTagByIdHandlerTests
{
    private readonly IApplicationDbContext _contextMock;
    private readonly GetTagByIdHandler _handler;

    // الـ Constructor هنا بيشتغل قبل كل [Fact]، فبنحضر فيه الحاجات المشتركة
    public GetTagByIdHandlerTests()
    {
        _contextMock = Substitute.For<IApplicationDbContext>();
        _handler = new GetTagByIdHandler(_contextMock);

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
        // Arrange
        var query = new GetTagByIdQuery(99); // نبحث عن Tag غير موجود

        // Act
        // 1. اكمل هنا... قم باستدعاء الدالة Handle من الـ _handler ومرر لها الـ query 
        // var result = ...

        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        // 2. اكمل هنا... تأكد أن النتيجة IsFailure تساوي True
        // 3. اكمل هنا... تأكد أن كود الخطأ يطابق TagErrors.NotFound(99).Code

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(TagErrors.NotFound(99).Code);
    }

    [Fact]
    public async Task Handle_TagExists_ReturnsSuccessResultWithData()
    {
        // Arrange
        var query = new GetTagByIdQuery(1); // نبحث عن Tag موجود مسبقاً (C#)

        // Act
        // 1. اكمل هنا... قم باستدعاء الدالة Handle

        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        // 2. اكمل هنا... تأكد أن النتيجة IsSuccess تساوي True
        // 3. اكمل هنا... تأكد أن الـ result.Value ليس null
        // 4. اكمل هنا... تأكد أن الـ result.Value.Id يساوي 1، والـ Name يساوي "C#"

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new TagResponse (1 , "C#" ));
    }

   
}
