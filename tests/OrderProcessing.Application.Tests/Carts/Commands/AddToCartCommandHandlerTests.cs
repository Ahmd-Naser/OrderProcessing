using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using OrderProcessing.Application.Carts.Commands.AddToCart;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Entities;
using Xunit;

namespace OrderProcessing.Application.Tests.Carts.Commands;

public class AddToCartCommandHandlerTests
{
    private readonly IApplicationDbContext _contextMock;
    private readonly AddToCartCommandHandler _handler;

    // المرجع الخاص بالسلة المزيفة لنتمكن من فحصه في الـ Assert
    private readonly Cart _fakeCart;

    public AddToCartCommandHandlerTests()
    {
        _contextMock = Substitute.For<IApplicationDbContext>();
        _handler = new AddToCartCommandHandler(_contextMock);

        // 1. تجهيز المنتجات المزيفة
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", IsActive = true, Price = 1000 , Stock = 10 },
            new Product { Id = 2, Name = "Mouse", IsActive = false, Price = 50 , Stock = 15}, // منتج غير مفعل
            new Product { Id = 3, Name = "keyboard", IsActive = true, Price = 530 , Stock = 17}
        };

        var productsDbSetMock = products.BuildMockDbSet();
        _contextMock.Products.Returns(productsDbSetMock);

        // 2. تجهيز السلة المزيفة (اليوزر user-1 عنده في السلة 2 لاب توب أصلاً)
        _fakeCart = new Cart
        {
            Id = 1,
            UserId = "user-1",
            CartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2 }
            }
        };

        var carts = new List<Cart> { _fakeCart };

        var cartsDbSetMock = carts.BuildMockDbSet();
        _contextMock.Carts.Returns(cartsDbSetMock);
    }

    [Fact]
    public async Task Handle_ProductDoesNotExist_ReturnsFailureResult()
    {
        // Arrange
        // يوزر صالح، لكن بيحاول يضيف منتج مش موجود (مثلاً Id = 99)
        var command = new AddToCartCommand("user-1", 99, 1);

        // Act
        // 1. اكمل هنا... قم باستدعاء الـ Handle

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // 2. اكمل هنا... تأكد من الفشل IsFailure
        // 3. اكمل هنا... تأكد أن الخطأ هو ProductErrors.NotFound(99).Code
        // 4. اكمل هنا... تأكد أن الدالة SaveChangesAsync لم تُستدعى أبداً (DidNotReceive)

        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBeNull();
        result.Error.Should().Be(ProductErrors.NotFound(99));

        await _contextMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ProductExistsAndAlreadyInCart_IncreasesQuantity()
    {
        // Arrange
        // اليوزر user-1 عنده أصلاً Laptop (Id=1) بكمية 2 في السلة
        // هنبعت كوماند يضيف نفس المنتج بكمية 3 إضافية
        var command = new AddToCartCommand("user-1", 1, 3);

        // Act
        // 1. اكمل هنا... قم باستدعاء الـ Handle
        
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // 2. اكمل هنا... تأكد من النجاح IsSuccess
        // 3. اكمل هنا... تأكد إن الـ Quantity الخاصة بالمنتج في السلة المزيفة (_fakeCart) بقت 5 (2 قديم + 3 جديد)
        // 4. اكمل هنا... تأكد من استدعاء SaveChangesAsync مرة واحدة (Received(1))

        result.IsSuccess.Should().BeTrue();

        var updatedItem = _fakeCart.CartItems.Single(ci => ci.ProductId == 1);
        updatedItem.Quantity.Should().Be(5);

        // 4. التأكد من استدعاء SaveChangesAsync مرة واحدة لحفظ التعديل
        await _contextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

    }

    [Fact]
    public async Task Handle_ProductExistsButNotInCart_AddsNewCartItem()
    {

        var command = new AddToCartCommand("user-1", 3, 1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        await _contextMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

    }

    [Fact]
    public async Task Handle_InActiveProduct_ReturnsFailureResult()
    {

        var command = new AddToCartCommand("user-1", 2, 1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();

        result.Error.Should().NotBeNull();

        result.Error.Should().Be(ProductErrors.NotFound(2));

        await _contextMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());

    }
}