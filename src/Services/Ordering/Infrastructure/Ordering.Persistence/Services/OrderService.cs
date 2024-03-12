using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Helpers;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Application.Services;
using Ordering.Application.Validations.UnitOfWorks;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Persistence.ExternalApiServices.Contracts;
using Shared.DataAccess.Abstract;
using Shared.Exceptions;

namespace Ordering.Persistence.Services
{
    public class OrderService : IOrderService
    {
        private readonly IReadRepository<Order> _orderReadRepository;
        private readonly IWriteRepository<Order> _orderWriteRepository;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ICustomerService _customerService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Validators
        private readonly IValidator<OrderUpdateDto> _orderUpdateDtoValidator;
        private readonly IValidator<OrderCreateDto> _orderCreateDtoValidator;
        #endregion

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ICustomerService customerService, IHttpContextAccessor httpContextAccessor,
            IValidator<OrderUpdateDto> orderUpdateDtoValidator, IValidator<OrderCreateDto> orderCreateDtoValidator)
        {
            _orderReadRepository = unitOfWork.GetReadRepository<Order>();
            _orderWriteRepository = unitOfWork.GetWriteRepository<Order>();
            _mapper = mapper;
            _customerService = customerService;
            _orderUpdateDtoValidator = orderUpdateDtoValidator;
            _orderCreateDtoValidator = orderCreateDtoValidator;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Guid> Create(OrderCreateDto order)
        {
            await Validate(order);
            var createdOrder = _mapper.Map<Order>(order);
            createdOrder.Status = Enum.GetName(OrderStatus.Pending);
            createdOrder.Address = await GetCustomerAddress(order.CustomerId);
            createdOrder.Product.ImageUrl = await UploadProductImage();

            var affectedRows = await _orderWriteRepository.CreateAsync(createdOrder);

            if (affectedRows == 0)
            {
                if (createdOrder.Product.ImageUrl != null)
                    _fileService.RemoveFile("orders", createdOrder.Product.ImageUrl);
                throw new InternalServerErrorException();
            }

            return createdOrder.Id;
        }

        public async Task<bool> Delete(Guid orderId)
        {
            var order = await _orderReadRepository.GetByIdAsync(orderId, includeProperties: _ => _.Product);
            ThrowOrderNotFoundIfOrderNotExist(order);
            var effectedRows = await _orderWriteRepository.DeleteAsync(order);
            if (effectedRows != 0)
            {
                if (order.Product.ImageUrl != null)
                    _fileService.RemoveFile("orders", order.Product.ImageUrl);
                return true;
            }

            return false;
        }

        public async Task<List<OrderListDto>> Get()
        {
            var orders = _orderReadRepository.Get(tracking: false, includeProperties: [_ => _.Product, _ => _.Address]);
            return _mapper.Map<List<OrderListDto>>(orders);
        }

        public async Task<OrderListDto> Get(Guid orderId)
        {
            var order = await _orderReadRepository.GetByIdAsync(orderId, includeProperties: [_ => _.Product, _ => _.Address]);
            ThrowOrderNotFoundIfOrderNotExist(order);

            return _mapper.Map<OrderListDto>(order);
        }

        public async Task<bool> Update(OrderUpdateDto order)
        {
            await Validate(order);
            var orderToUpdate = await _orderReadRepository.GetByIdAsync(order.Id, includeProperties: [_ => _.Address]);
            ThrowOrderNotFoundIfOrderNotExist(orderToUpdate);
            orderToUpdate.Address = _mapper.Map<Address>(order.Address);
            var effectedRows = await _orderWriteRepository.UpdateAsync(orderToUpdate);

            return effectedRows != 0;
        }

        public async Task<bool> ChangeOrderStatus(Guid orderId, string status)
        {
            var order = await _orderReadRepository.GetByIdAsync(orderId);
            ThrowOrderNotFoundIfOrderNotExist(order);
            order.Status = status;
            var effectedRows = await _orderWriteRepository.UpdateAsync(order);

            return effectedRows != 0;
        }

        private void ThrowOrderNotFoundIfOrderNotExist(Order order)
        {
            if (order is null)
                throw new NotFoundException("Order not found!");
        }

        private async Task<Address> GetCustomerAddress(Guid customerId)
        {
            var customer = await _customerService.GetCustomerById(customerId);
            return _mapper.Map<Address>(customer.Address);
        }

        private async Task Validate(OrderCreateDto customerDto)
        {
            var validation = await _orderCreateDtoValidator.ValidateAsync(customerDto);
            if (!validation.IsValid)
                throw new BadRequestException(validation.Errors.First().ErrorMessage);
        }

        private async Task Validate(OrderUpdateDto customerDto)
        {
            var validation = await _orderUpdateDtoValidator.ValidateAsync(customerDto);
            if (!validation.IsValid)
                throw new BadRequestException(validation.Errors.First().ErrorMessage);
        }

        private async Task<string> UploadProductImage()
        {
            var containsImg = _httpContextAccessor.HttpContext.Request.Form.Files.FirstOrDefault();
            return containsImg != null ? await _fileService.UploadFile("orders", containsImg) : null;
        }
    }
}
