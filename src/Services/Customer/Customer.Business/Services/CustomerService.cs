using AutoMapper;
using Customer.Business.Models.Dtos.Customer;
using Customer.Business.Services.Abstract;
using Customer.DataAccess.UnitOfWorks;
using FluentValidation;
using Shared.DataAccess.Abstract;
using Shared.Exceptions;
using Entities = Customer.Entity.Entities;

namespace Customer.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IReadRepository<Entities.Customer> _customerReadRepository;
        private readonly IWriteRepository<Entities.Customer> _customerWriteRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CustomerCreateDto> _customerCreateDtoValidator;
        private readonly IValidator<CustomerUpdateDto> _customerUpdateDtoValidator;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IValidator<CustomerCreateDto> customerCreateDtoValidator,
            IValidator<CustomerUpdateDto> customerUpdateDtoValidator)
        {
            _customerReadRepository = unitOfWork.GetReadRepository<Entities.Customer>();
            _customerWriteRepository = unitOfWork.GetWriteRepository<Entities.Customer>();
            _mapper = mapper;
            _customerCreateDtoValidator = customerCreateDtoValidator;
            _customerUpdateDtoValidator = customerUpdateDtoValidator;
        }

        public async Task<Guid> Create(CustomerCreateDto customerDto)
        {
            await Validate(customerDto);
            var customer = _mapper.Map<Entities.Customer>(customerDto);
            int effectedRows = await _customerWriteRepository.CreateAsync(customer);
            if (effectedRows == 0)
                throw new InternalServerErrorException();

            return customer.Id;
        }

        public async Task<bool> Update(CustomerUpdateDto customerDto)
        {
            await Validate(customerDto);
            var customer = await _customerReadRepository.GetByIdAsync(customerDto.Id, includeProperties: _ => _.Address);
            ThrowNotFoundIfCustomerNotExist(customer);
            _mapper.Map(customerDto, customer);
            var effectedRows = await _customerWriteRepository.UpdateAsync(customer);

            return effectedRows != 0;
        }

        public async Task<bool> Delete(Guid customerId)
        {
            var customer = await _customerReadRepository.GetByIdAsync(customerId);
            ThrowNotFoundIfCustomerNotExist(customer);
            var effectedRows = await _customerWriteRepository.DeleteAsync(customer);

            return effectedRows != 0;
        }

        public async Task<CustomerListDto> Get(Guid customerId)
        {
            var customer = await _customerReadRepository.GetByIdAsync(customerId, tracking: false, includeProperties: _ => _.Address);
            ThrowNotFoundIfCustomerNotExist(customer);

            return _mapper.Map<CustomerListDto>(customer);
        }

        public List<CustomerListDto> Get()
        {
            var customers = _customerReadRepository.Get(tracking: false, includeProperties: _ => _.Address).ToList();
            return _mapper.Map<List<CustomerListDto>>(customers);
        }

        private void ThrowNotFoundIfCustomerNotExist(Entities.Customer customer)
        {
            if (customer is null)
                throw new NotFoundException("Customer not found!");
        }

        private async Task Validate(CustomerCreateDto customerDto)
        {
            var validation = await _customerCreateDtoValidator.ValidateAsync(customerDto);
            if (!validation.IsValid)
                throw new BadRequestException();
        }

        private async Task Validate(CustomerUpdateDto customerDto)
        {
            var validation = await _customerUpdateDtoValidator.ValidateAsync(customerDto);
            if (!validation.IsValid)
                throw new BadRequestException();
        }
    }
}
