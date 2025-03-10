using System.Threading.Tasks;

using CafeManager.Contracts.Dto.Orders;
using CafeManager.Contracts.Dto.Orders.Enums;
using CafeManager.Core.Exceptions;
using CafeManager.Core.Mapping.Configurations;
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Models.Orders;
using CafeManager.Core.Models.Orders.Enums;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services;
using CafeManager.Core.Services.Interfaces;

using MapsterMapper;

using Moq;

namespace CafeManager.Core.Tests;

public class OrderServiceTests
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private IMapper _mapper;
    private IOrderService _orderService;

    [SetUp]
    public void Setup()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();

        OrderConfiguration.Configure();
        _mapper = new Mapper();

        _orderService = new OrderService(_orderRepositoryMock.Object, _mapper);
    }

    [Test]
    public async Task GetOrders_WithValidData_ShouldReturnOrders()
    {
        var orders = new List<Order>
        {
            new()
            {
                Id = 1,
                CompletedAt = DateTime.Now,
                CustomerName = "John Doe",
                PaymentType = PaymentType.Card,
                Status = OrderStatus.Completed,
                MenuItems = []
            },
            new()
            {
                Id = 2,
                CompletedAt = DateTime.Now,
                CustomerName = "John Doe",
                PaymentType = PaymentType.Card,
                Status = OrderStatus.InWork,
                MenuItems = []
            }
        };

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<GetOrderFilter>()))
            .Returns<GetOrderFilter>(filter => orders.ToAsyncEnumerable());

        var orderDto = await _orderService.GetAsync(new()).ToListAsync();

        Assert.That(orderDto, Has.Count.EqualTo(2));

        Assert.Multiple(() =>
        {
            Assert.That(orderDto[0].Id, Is.EqualTo(1));
            Assert.That(orderDto[1].Id, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task CreateOrder_WithValidData_ShouldCreateAndReturnOrder()
    {
        var addOrderDto = new AddOrderDto(
            "John Doe",
            DateTime.Now,
            PaymentTypeDto.Card,
            []);

        var orders = new List<Order>();

        _orderRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<Order, CancellationToken, IOrderRepository, Order>((order, _) =>
            {
                orders.Add(order);
                return order;
            });

        var orderDto = await _orderService.CreateAsync(addOrderDto, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(orderDto.CustomerName, Is.EqualTo(addOrderDto.CustomerName));
            Assert.That(orderDto.CompletedAt, Is.EqualTo(addOrderDto.CompletedAt));
            Assert.That(orderDto.PaymentType, Is.EqualTo(addOrderDto.PaymentType));
            Assert.That(orderDto.Status, Is.EqualTo(OrderStatusDto.InWork));
            Assert.That(orderDto.MenuItems, Is.Empty);
        });

        Assert.Multiple(() =>
        {
            Assert.That(orders, Has.Count.EqualTo(1));
            Assert.That(orders[0].CustomerName, Is.EqualTo(addOrderDto.CustomerName));
            Assert.That(orders[0].CompletedAt, Is.EqualTo(addOrderDto.CompletedAt));
            Assert.That(orders[0].PaymentType, Is.EqualTo(PaymentType.Card));
            Assert.That(orders[0].Status, Is.EqualTo(OrderStatus.InWork));
            Assert.That(orders[0].MenuItems, Is.Empty);
        });
    }

    [Test]
    public void CreateOrder_WithInvalidMenuItemId_ShouldThrowException()
    {
        var addOrderDto = new AddOrderDto(
            "John Doe",
            DateTime.Now,
            PaymentTypeDto.Card,
            [0]);

        Assert.ThrowsAsync<InvalidInputDataException>(async () => await _orderService.CreateAsync(addOrderDto, CancellationToken.None));
    }

    [Test]
    public void CreateOrder_WithConflict_ShouldThrowException()
    {
        var addOrderDto = new AddOrderDto(
            "John Doe",
            DateTime.Now,
            PaymentTypeDto.Card,
            []);

        _orderRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityConflictException());

        Assert.ThrowsAsync<EntityConflictException>(async () => await _orderService.CreateAsync(addOrderDto, CancellationToken.None));
    }

    [Test]
    public async Task PartialUpdateOrder_WithValidData_ShouldUpdateAndReturnOrder()
    {
        var orders = new Dictionary<long, Order>
        {
            {
                1,
                new()
                {
                    Id = 1,
                    CustomerName = "John Doe",
                    CompletedAt = DateTime.Now,
                    PaymentType = PaymentType.Card,
                    Status = OrderStatus.InWork,
                    MenuItems = []
                }
            }
        };

        var newDate = DateTime.Now.AddDays(1);
        var partialUpdateOrderDto = new PartialUpdateOrderDto("Test", newDate, PaymentTypeDto.Cash, [1]);
        var partialUpdateOrderDto2 = new PartialUpdateOrderDto(null, null, null, null);

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IOrderRepository, Order>((id, _) => orders[id]);
        _orderRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<Order, CancellationToken, IOrderRepository, Order>((order, _) => orders[order.Id] = order);

        var orderDto = await _orderService.PartialUpdateAsync(1, partialUpdateOrderDto, CancellationToken.None);
        var orderDto2 = await _orderService.PartialUpdateAsync(1, partialUpdateOrderDto2, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(orderDto.Id, Is.EqualTo(1));
            Assert.That(orderDto.CustomerName, Is.EqualTo(partialUpdateOrderDto.CustomerName));
            Assert.That(orderDto.CompletedAt, Is.EqualTo(partialUpdateOrderDto.CompletedAt));
            Assert.That(orderDto.PaymentType, Is.EqualTo(partialUpdateOrderDto.PaymentType));
            Assert.That(orderDto.Status, Is.EqualTo(OrderStatusDto.InWork));
            Assert.That(orderDto.MenuItems, Has.Count.EqualTo(1));
        });

        Assert.Multiple(() =>
        {
            Assert.That(orderDto2.Id, Is.EqualTo(orderDto.Id));
            Assert.That(orderDto2.CustomerName, Is.EqualTo(orderDto.CustomerName));
            Assert.That(orderDto2.CompletedAt, Is.EqualTo(orderDto.CompletedAt));
            Assert.That(orderDto2.PaymentType, Is.EqualTo(orderDto.PaymentType));
            Assert.That(orderDto2.Status, Is.EqualTo(orderDto.Status));
            Assert.That(orderDto2.MenuItems, Has.Count.EqualTo(orderDto.MenuItems.Count));
        });

        Assert.Multiple(() =>
        {
            Assert.That(orders[1].Id, Is.EqualTo(1));
            Assert.That(orders[1].CustomerName, Is.EqualTo(partialUpdateOrderDto.CustomerName));
            Assert.That(orders[1].CompletedAt, Is.EqualTo(partialUpdateOrderDto.CompletedAt));
            Assert.That(orders[1].PaymentType, Is.EqualTo(PaymentType.Cash));
            Assert.That(orders[1].Status, Is.EqualTo(OrderStatus.InWork));
            Assert.That(orders[1].MenuItems, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void PartialUpdateOrder_WithInvalidMenuItemId_ShouldThrowException()
    {
        var partialUpdateOrderDto = new PartialUpdateOrderDto(null, null, null, [0]);

        Assert.ThrowsAsync<InvalidInputDataException>(async () => await _orderService.PartialUpdateAsync(1, partialUpdateOrderDto, CancellationToken.None));
    }

    [Test]
    public void PartialUpdateOrder_WithNotInWorkOrder_ShouldThrowException()
    {
        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            CompletedAt = DateTime.Now,
            PaymentType = PaymentType.Card,
            Status = OrderStatus.Completed,
            MenuItems = []
        };

        var partialUpdateOrderDto = new PartialUpdateOrderDto(null, null, null, []);

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IOrderRepository, Order>((id, _) => order);

        Assert.ThrowsAsync<EntityConflictException>(async () => await _orderService.PartialUpdateAsync(1, partialUpdateOrderDto, CancellationToken.None));
    }

    [Test]
    public void PartialUpdateOrder_WithNotFound_ShouldThrowException()
    {
        var partialUpdateOrderDto = new PartialUpdateOrderDto(null, null, null, null);

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException());

        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _orderService.PartialUpdateAsync(1, partialUpdateOrderDto, CancellationToken.None));
    }

    [Test]
    public void PartialUpdateOrder_WithConflict_ShouldThrowException()
    {
        var order = new Order()
        {
            Id = 1,
            CustomerName = "John Doe",
            CompletedAt = DateTime.Now,
            PaymentType = PaymentType.Card,
            Status = OrderStatus.InWork,
            MenuItems = []
        };

        var partialUpdateOrderDto = new PartialUpdateOrderDto(null, null, null, null);

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IOrderRepository, Order>((id, _) => order);

        _orderRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityConflictException());

        Assert.ThrowsAsync<EntityConflictException>(async () => await _orderService.PartialUpdateAsync(1, partialUpdateOrderDto, CancellationToken.None));
    }

    [Test]
    public async Task CompleteOrder_WithValidData_ShouldCompleteOrder()
    {
        var orderId = 1;
        var orderDate = DateTimeOffset.Now;
        var customerName = "John Doe";
        var orderPaymentType = PaymentType.Card;
        var orderStatus = OrderStatus.InWork;
        var orderMenuItems = new List<MenuItem>();

        var orders = new Dictionary<long, Order>()
        {
            {
                orderId,
                new()
                {
                    Id = orderId,
                    CustomerName = customerName,
                    CompletedAt = orderDate,
                    PaymentType = orderPaymentType,
                    Status = orderStatus,
                    MenuItems = orderMenuItems
                }
            }
        };

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IOrderRepository, Order>((id, _) => orders[id]);
        _orderRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<Order, CancellationToken, IOrderRepository, Order>((order, _) =>
            {
                orders[order.Id] = order;
                return order;
            });

        await _orderService.CompleteOrderAsync(orderId, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(orders[orderId].Status, Is.EqualTo(OrderStatus.Completed));
            Assert.That(orders[orderId].CompletedAt, Is.EqualTo(orderDate));
            Assert.That(orders[orderId].PaymentType, Is.EqualTo(orderPaymentType));
            Assert.That(orders[orderId].CustomerName, Is.EqualTo(customerName));
            Assert.That(orders[orderId].MenuItems, Is.EqualTo(orderMenuItems));
        });
    }

    [Test]
    public void CompleteOrder_WithNotFound_ShouldThrowException()
    {
        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException());

        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _orderService.CompleteOrderAsync(1, CancellationToken.None));
    }

    [Test]
    public void CompleteOrder_WithConflict_ShouldThrowException()
    {
        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            CompletedAt = DateTime.Now,
            PaymentType = PaymentType.Card,
            Status = OrderStatus.Completed,
            MenuItems = []
        };

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _orderRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityConflictException());

        Assert.ThrowsAsync<EntityConflictException>(async () => await _orderService.CompleteOrderAsync(1, CancellationToken.None));
    }

    [Test]
    public async Task CancelOrder_WithValidData_ShouldCompleteOrder()
    {
        var orderId = 1;
        var orderDate = DateTimeOffset.Now;
        var customerName = "John Doe";
        var orderPaymentType = PaymentType.Card;
        var orderStatus = OrderStatus.InWork;
        var orderMenuItems = new List<MenuItem>();

        var orders = new Dictionary<long, Order>()
        {
            {
                orderId,
                new()
                {
                    Id = orderId,
                    CustomerName = customerName,
                    CompletedAt = orderDate,
                    PaymentType = orderPaymentType,
                    Status = orderStatus,
                    MenuItems = orderMenuItems
                }
            }
        };

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IOrderRepository, Order>((id, _) => orders[id]);
        _orderRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<Order, CancellationToken, IOrderRepository, Order>((order, _) =>
            {
                orders[order.Id] = order;
                return order;
            });

        await _orderService.CancelOrderAsync(orderId, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(orders[orderId].Status, Is.EqualTo(OrderStatus.Canceled));
            Assert.That(orders[orderId].CompletedAt, Is.EqualTo(orderDate));
            Assert.That(orders[orderId].PaymentType, Is.EqualTo(orderPaymentType));
            Assert.That(orders[orderId].CustomerName, Is.EqualTo(customerName));
            Assert.That(orders[orderId].MenuItems, Is.EqualTo(orderMenuItems));
        });
    }

    [Test]
    public void CancelOrder_WithNotFound_ShouldThrowException()
    {
        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException());

        Assert.ThrowsAsync<EntityNotFoundException>(async () => await _orderService.CancelOrderAsync(1, CancellationToken.None));
    }

    [Test]
    public void CancelOrder_WithConflict_ShouldThrowException()
    {
        var order = new Order
        {
            Id = 1,
            CustomerName = "John Doe",
            CompletedAt = DateTime.Now,
            PaymentType = PaymentType.Card,
            Status = OrderStatus.Completed,
            MenuItems = []
        };

        _orderRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _orderRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityConflictException());

        Assert.ThrowsAsync<EntityConflictException>(async () => await _orderService.CancelOrderAsync(1, CancellationToken.None));
    }
}