using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Orders.Commands.PlaceOrder;
using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tests.Orders.Commands;

public class PlaceOrderCommandHandlerTests
{
    private readonly IApplicationDbContext _contextMock;
    private readonly PlaceOrderCommandHandler _handler;

    private readonly Product _laptopProduct;

    public PlaceOrderCommandHandlerTests()
    {
        _contextMock = Substitute.For<IApplicationDbContext>();
        _handler = new PlaceOrderCommandHandler(_contextMock);
        _laptopProduct = new Product { Id = 1, Name = "Laptop", IsActive = true, Price = 1000, Stock = 5 };

        // 1. تجهيز المنتجات (المنتج رقم 1 مخزونه 5 فقط)
        var products = new List<Product>
        {
            _laptopProduct
        };
        var productsDbSetMock = products.BuildMockDbSet();
        _contextMock.Products.Returns(productsDbSetMock);
    }

    [Fact]
    public async Task Handle_EmptyCart_ReturnsFailureResult()
    {
        // Arrange
        var command = new PlaceOrderCommand("user-empty", Guid.NewGuid());

        // سلة فارغة تماماً لهذا المستخدم
        var emptyCarts = new List<Cart>();
        var emptyCartsDbSetMock = emptyCarts.BuildMockDbSet();
        _contextMock.Carts.Returns(emptyCartsDbSetMock);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CartErrors.EmptyCart() );
    }

    [Fact]
    public async Task Handle_ProductStockIsLessThanCartQuantity_ReturnsFailureResult()
    {
        // Arrange
        var command = new PlaceOrderCommand("user-1", Guid.NewGuid());

        var cartItem = new CartItem
        {
            ProductId = 1,
            Quantity = 10, // كمية أكبر من المخزون (المخزون 5 فقط)
            Product = _laptopProduct
        };

        var cart = new Cart
        {
            UserId = "user-1",
            CartItems = new List<CartItem> { cartItem }

        };

        var CartsDbSetMock = new List<Cart> { cart }.BuildMockDbSet();
        _contextMock.Carts.Returns(CartsDbSetMock) ;

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CartErrors.ProductNotAvailable());

        
    }
}
