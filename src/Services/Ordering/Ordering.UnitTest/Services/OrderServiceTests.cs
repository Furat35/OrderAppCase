using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Ordering.Application.Helpers;
using Ordering.Application.Models.Dtos;
using Ordering.Application.Models.Dtos.Addresses;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Application.UnitOfWorks;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Persistence.ExternalApiServices.Contracts;
using Ordering.Persistence.Services;
using Shared.Exceptions;
using System.Linq.Expressions;

namespace Ordering.UnitTest.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<ICustomerService> _customerService = new();
        private readonly Mock<IFileService> _fileService = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
        private readonly Mock<IValidator<OrderUpdateDto>> _orderUpdateDtoValidator = new();
        private readonly Mock<IValidator<OrderCreateDto>> _orderCreateDtoValidator = new();
        private readonly List<Order> orders;

        public OrderServiceTests()
        {
            _httpContextAccessor.Setup(_ => _.HttpContext.Request.Form.Files).Returns(new FormFileCollection());
            _fileService.Setup(_ => _.UploadFile(It.IsAny<string>(), It.IsAny<IFormFile>())).ReturnsAsync("ok");
            _fileService.Setup(_ => _.RemoveFile(It.IsAny<string>(), It.IsAny<string>()));
            orders = new List<Order>
            {
                new (){ Id = Guid.Parse("77D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01") ,CustomerId = Guid.Parse("E2D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), Quantity = 5, Price = 35,
                    Status = Enum.GetName(OrderStatus.Pending), Address = new (){Id =  Guid.Parse("66D8732C-4B73-4FCC-ADB0-F9E8A9F9CB02"), AddressLine = "home", City = "izmir",
                    Country = "Turkiye", CityCode = 35 }, Product = new(){ Id = Guid.Parse("13D8732C-4B73-4FCC-ADB0-F9E8A9F9CB02"),ImageUrl = "iphone.png", Name = "iphone" }, CreatedAt = DateTime.Now},
                new (){ Id = Guid.Parse("37D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01") ,CustomerId = Guid.Parse("E2D2E7D0-4C92-490C-B4D2-C1A85ADBE8F8"), Quantity = 2, Price = 1200,
                    Status = Enum.GetName(OrderStatus.Pending), Address = new (){Id =  Guid.Parse("55D8732C-4B73-4FCC-ADB0-F9E8A9F9CB44"), AddressLine = "office", City = "istanbul",
                    Country = "Turkiye", CityCode = 34 }, Product = new(){ Id = Guid.Parse("89D8732C-4B73-4FCC-ADB0-F9E8A9F9CB11"),ImageUrl = "tv.png", Name = "samsung" }, CreatedAt = DateTime.Now}
            };
        }

        [Fact]
        public async Task Get_WithValidOrderId_ReturnsOrderWithGivenId()
        {
            // Arrange
            var order = new OrderListDto
            {
                Id = orders[0].Id,
                Price = orders[0].Price,
                Quantity = orders[0].Quantity,
                Status = orders[0].Status,
                CustomerId = orders[0].CustomerId,
                Address = new()
                {
                    Id = orders[0].Address.Id,
                    AddressLine = orders[0].Address.AddressLine,
                    City = orders[0].Address.City,
                    CityCode = orders[0].Address.CityCode,
                    Country = orders[0].Address.Country
                },
                Product = new() { Id = orders[0].Product.Id, ImageUrl = orders[0].Product.ImageUrl, Name = orders[0].Product.Name }
            };
            _unitOfWork.Setup(_ => _.GetReadRepository<Order>().GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync(orders[0]);
            _mapper.Setup(_ => _.Map<OrderListDto>(It.IsAny<Order>())).Returns(order);
            var orderService = CreateOrderService();

            // Act
            var orderResponse = await orderService.Get(order.Id);

            // Assert
            Assert.Equal(order, orderResponse);
        }

        [Fact]
        public async Task Get_WithInvalidOrderId_ThrowsNotFoundException()
        {
            // Arrange
            var order = new OrderListDto
            {
                Id = orders[0].Id,
                Price = orders[0].Price,
                Quantity = orders[0].Quantity,
                Status = orders[0].Status,
                CustomerId = orders[0].CustomerId,
                Address = new()
                {
                    Id = orders[0].Address.Id,
                    AddressLine = orders[0].Address.AddressLine,
                    City = orders[0].Address.City,
                    CityCode = orders[0].Address.CityCode,
                    Country = orders[0].Address.Country
                },
                Product = new() { Id = orders[0].Product.Id, ImageUrl = orders[0].Product.ImageUrl, Name = orders[0].Product.Name }
            };
            _unitOfWork.Setup(_ => _.GetReadRepository<Order>().GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync((Order)null);
            var orderService = CreateOrderService();

            // Act
            var orderResponse = async () => await orderService.Get(order.Id);

            // Assert
            await Assert.ThrowsAsync(typeof(NotFoundException), orderResponse);
        }

        [Fact]
        public async Task Get_NoneParameter_ReturnsOrders()
        {
            // Arrange
            var orderList = new List<OrderListDto>
            {
                new OrderListDto{
                    Id = orders[0].Id,
                    Price = orders[0].Price,
                    Quantity = orders[0].Quantity,
                    Status = orders[0].Status,
                    CustomerId = orders[0].CustomerId,
                    Address = new() { Id = orders[0].Address.Id, AddressLine = orders[0].Address.AddressLine, City = orders[0].Address.City,
                        CityCode = orders[0].Address.CityCode, Country = orders[0].Address.Country },
                    Product = new() { Id = orders[0].Product.Id, ImageUrl = orders[0].Product.ImageUrl, Name = orders[0].Product.Name }
                },
                new OrderListDto{
                    Id = orders[1].Id,
                    Price = orders[1].Price,
                    Quantity = orders[1].Quantity,
                    Status = orders[1].Status,
                    CustomerId = orders[1].CustomerId,
                    Address = new() { Id = orders[1].Address.Id, AddressLine = orders[1].Address.AddressLine, City = orders[1].Address.City,
                        CityCode = orders[1].Address.CityCode, Country = orders[1].Address.Country },
                    Product = new() { Id = orders[1].Product.Id, ImageUrl = orders[1].Product.ImageUrl, Name = orders[1].Product.Name }
                }
            };
            _unitOfWork.Setup(_ => _.GetReadRepository<Order>().Get(It.IsAny<bool>(), It.IsAny<Expression<Func<Order, object>>[]>())).Returns(orders.AsQueryable());
            _mapper.Setup(_ => _.Map<List<OrderListDto>>(It.IsAny<IQueryable<Order>>())).Returns(orderList);
            var orderService = CreateOrderService();

            // Act
            var orderResponse = await orderService.Get();

            // Assert
            Assert.Equal(orderList, orderResponse);
        }

        [Fact]
        public async Task Delete_WithValidOrderId_ReturnsTrue()
        {
            // Arrange
            _unitOfWork.Setup(_ => _.GetReadRepository<Order>().GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync(orders[0]);
            _unitOfWork.Setup(_ => _.GetWriteRepository<Order>().DeleteAsync(It.IsAny<Order>())).ReturnsAsync(1);
            var orderService = CreateOrderService();

            // Act
            var isDeleted = await orderService.Delete(orders[0].Id);

            // Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task Delete_WithInvalidOrderId_ThrowsNotFoundException()
        {
            // Arrange
            _unitOfWork.Setup(_ => _.GetReadRepository<Order>().GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync((Order)null);
            var orderService = CreateOrderService();

            // Act
            var orderResult = async () => await orderService.Delete(orders[0].Id);

            // Assert
            await Assert.ThrowsAsync(typeof(NotFoundException), orderResult);
        }

        [Fact]
        public async Task Create_WithGivenOrderProperties_ReturnsOrderId()
        {
            // Arrange
            var customer = new CustomerListDto
            {
                Id = Guid.Parse("33D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01"),
                Name = "firat",
                Address = new()
                {
                    Id = orders[0].Address.Id,
                    AddressLine = orders[0].Address.AddressLine,
                    City = orders[0].Address.City,
                    CityCode = orders[0].Address.CityCode,
                    Country = orders[0].Address.Country
                },
                Email = "customer@gmail.com",
            };
            var address = new Address
            {
                Id = customer.Address.Id,
                AddressLine = customer.Address.AddressLine,
                City = customer.Address.City,
                CityCode = customer.Address.CityCode,
                Country = customer.Address.Country
            };
            var orderToCreate = new OrderCreateDto { CustomerId = orders[0].CustomerId, Price = orders[0].Price, Quantity = orders[0].Quantity };
            _orderCreateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<OrderCreateDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            _unitOfWork.Setup(_ => _.GetWriteRepository<Order>().CreateAsync(It.IsAny<Order>())).ReturnsAsync(1);
            _mapper.Setup(_ => _.Map<Order>(It.IsAny<OrderCreateDto>())).Returns(orders[0]);
            _mapper.Setup(_ => _.Map<Address>(It.IsAny<AddressListDto>())).Returns(address);
            _customerService.Setup(_ => _.GetCustomerById(It.IsAny<Guid>())).ReturnsAsync(customer);
            var orderService = CreateOrderService();

            // Act
            var orderId = await orderService.Create(orderToCreate);

            // Assert
            Assert.Equal(orders[0].Id, orderId);
        }

        [Fact]
        public async Task Create_WithInvalidOrderProperties_ThrowsBadRequestException()
        {
            // Arrange
            var customer = new CustomerListDto
            {
                Id = Guid.Parse("33D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01"),
                Name = "firat",
                Address = new()
                {
                    Id = orders[0].Address.Id,
                    AddressLine = orders[0].Address.AddressLine,
                    City = orders[0].Address.City,
                    CityCode = orders[0].Address.CityCode,
                    Country = orders[0].Address.Country
                },
                Email = "customer@gmail.com",
            };
            var address = new Address
            {
                Id = customer.Address.Id,
                AddressLine = customer.Address.AddressLine,
                City = customer.Address.City,
                CityCode = customer.Address.CityCode,
                Country = customer.Address.Country
            };
            var orderToCreate = new OrderCreateDto { CustomerId = orders[0].CustomerId, Price = orders[0].Price, Quantity = orders[0].Quantity };
            _orderCreateDtoValidator
                .Setup(_ => _.ValidateAsync(It.IsAny<OrderCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure> { new() { ErrorMessage = "Error occured!" } } });
            var orderService = CreateOrderService();

            // Act
            var orderResult = async () => await orderService.Create(orderToCreate);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), orderResult);
        }

        [Fact]
        public async Task Update_WithValidOrderUpdateDtoProperties_ReturnsTrue()
        {
            // Arrange
            var customer = new CustomerListDto
            {
                Id = Guid.Parse("33D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01"),
                Name = "firat",
                Address = new() { Id = orders[0].Address.Id, AddressLine = orders[0].Address.AddressLine, City = orders[0].Address.City, CityCode = orders[0].Address.CityCode, Country = orders[0].Address.Country },
                Email = "customer@gmail.com",
            };
            var orderToUpdate = new OrderUpdateDto
            {
                Id = orders[0].Id,
                Address = new()
                {
                    AddressLine = orders[0].Address.AddressLine,
                    City = orders[0].Address.City,
                    CityCode = orders[0].Address.CityCode,
                    Country = orders[0].Address.Country
                },
            };
            var address = new Address
            {
                Id = customer.Address.Id,
                AddressLine = customer.Address.AddressLine,
                City = customer.Address.City,
                CityCode = customer.Address.CityCode,
                Country = customer.Address.Country
            };
            _orderUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<OrderUpdateDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            _mapper.Setup(_ => _.Map<Address>(It.IsAny<AddressUpdateDto>())).Returns(orders[0].Address);
            _unitOfWork.Setup(_ => _.GetWriteRepository<Order>().UpdateAsync(It.IsAny<Order>())).ReturnsAsync(1);
            _unitOfWork.Setup(_ => _.GetReadRepository<Order>().GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync(orders[0]);
            var orderService = CreateOrderService();

            // Act
            var isUpdated = await orderService.Update(new OrderUpdateDto());

            // Assert
            Assert.True(isUpdated);
        }

        [Fact]
        public async Task Update_WithInvalidOrderUpdateDtoProperties_ThrowsBadRequestException()
        {
            // Arrange
            var customer = new CustomerListDto
            {
                Id = Guid.Parse("33D8732C-4B73-4FCC-ADB0-F9E8A9F9CB01"),
                Name = "firat",
                Address = new()
                {
                    Id = orders[0].Address.Id,
                    AddressLine = orders[0].Address.AddressLine,
                    City = orders[0].Address.City,
                    CityCode = orders[0].Address.CityCode,
                    Country = orders[0].Address.Country
                },
                Email = "customer@gmail.com",
            };
            var orderToUpdate = new OrderUpdateDto
            {
                Id = orders[0].Id,
                Address = new()
                {
                    AddressLine = orders[0].Address.AddressLine,
                    City = orders[0].Address.City,
                    CityCode = orders[0].Address.CityCode,
                    Country = orders[0].Address.Country
                },
            };
            var address = new Address
            {
                Id = customer.Address.Id,
                AddressLine = customer.Address.AddressLine,
                City = customer.Address.City,
                CityCode = customer.Address.CityCode,
                Country = customer.Address.Country
            };
            _orderUpdateDtoValidator
                .Setup(_ => _.ValidateAsync(It.IsAny<OrderUpdateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure> { new() { ErrorMessage = "Error occured!" } } });
            var orderService = CreateOrderService();
            // Act
            var orderResult = async () => await orderService.Update(new OrderUpdateDto());

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), orderResult);
        }

        private OrderService CreateOrderService()
            => new OrderService(_unitOfWork.Object, _mapper.Object, _fileService.Object, _customerService.Object,
                _httpContextAccessor.Object, _orderUpdateDtoValidator.Object, _orderCreateDtoValidator.Object);

    }
}
