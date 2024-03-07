using AutoMapper;
using Customer.Business.Models.Dtos.Address;
using Customer.Business.Models.Dtos.Customer;
using Customer.Business.Services;
using Customer.DataAccess.UnitOfWorks;
using Customer.Entity.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Shared.Exceptions;
using System.Linq.Expressions;
using Entities = Customer.Entity.Entities;

namespace Customer.UnitTest.ServiceTests
{
    public class CustomerServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork = new();
        private Mock<IMapper> _mapper = new();
        private Mock<IValidator<CustomerCreateDto>> _customerCreateDtoValidator = new();
        private Mock<IValidator<CustomerUpdateDto>> _customerUpdateDtoValidator = new();
        private List<Entities.Customer> _customers = new();

        public CustomerServiceTests()
        {
            var customers = new List<Entities.Customer>{
                new Entities.Customer
                {
                    Id = Guid.Parse("BD2C946F-1936-40C6-9C4B-0856DCB526CE"),
                    Email = "ahmet@gmail.com",
                    Name = "ahmet",
                    CreatedAt = DateTime.Now,
                    Address = new Address
                    {
                        Id = Guid.Parse("AD2C946F-1936-40C6-9C4B-0856DCB526CE"),
                        AddressLine = "Home",
                        City = "İzmir",
                        Country = "Türkiye",
                        CityCode = 35,
                        CreatedAt = DateTime.Now,
                    }
                },
                 new Entities.Customer
                 {
                     Id = Guid.Parse("1D2C946F-1936-40C6-9C4B-0856DCB526CE"),
                     Email = "firat@gmail.com",
                     Name = "firat",
                     CreatedAt = DateTime.Now,
                     Address = new Address
                     {
                         Id = Guid.Parse("2D2C946F-1936-40C6-9C4B-0856DCB526CE"),
                         AddressLine = "Home",
                         City = "İstanbul",
                         Country = "Türkiye",
                         CityCode = 34,
                         CreatedAt = DateTime.Now,
                     }
                 }};
            _customers.AddRange(customers);
        }

        [Fact]
        public async Task Get_WithGivenCustomerId_ReturnsCustomerWithGivenId()
        {
            // Arrange
            var customerDto = new CustomerListDto
            {
                Id = _customers[0].Id,
                Email = _customers[0].Email,
                Name = _customers[0].Name,
                Address = new AddressListDto
                {
                    Id = _customers[0].Address.Id,
                    AddressLine = _customers[0].Address.AddressLine,
                    City = _customers[0].Address.City,
                    Country = _customers[0].Address.Country,
                    CityCode = _customers[0].Address.CityCode
                }
            };
            _unitOfWork.Setup(_ => _.GetReadRepository<Entities.Customer>().GetByIdAsync(customerDto.Id, It.IsAny<bool>(), _ => _.Address)).ReturnsAsync(_customers[0]);
            _mapper.Setup(_ => _.Map<CustomerListDto>(_customers[0])).Returns(customerDto);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = await customerService.Get(customerDto.Id);

            // Assert
            Assert.Equal(customerDto, result);
        }

        [Fact]
        public async Task Get_WithInvalidCustomerId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidUserId = Guid.NewGuid();
            _unitOfWork.Setup(_ => _.GetReadRepository<Entities.Customer>().GetByIdAsync(invalidUserId, false, _ => _.Address)).ReturnsAsync((Entities.Customer)null);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = async () => await customerService.Get(invalidUserId);

            // Assert
            await Assert.ThrowsAsync(typeof(NotFoundException), result);
        }


        [Fact]
        public void Get_NoneParamter_ReturnsCustomers()
        {
            // Arrange
            _unitOfWork.Setup(_ => _.GetReadRepository<Entities.Customer>().Get(false, _ => _.Address)).Returns(_customers.AsQueryable());
            var customerDtos = new List<CustomerListDto>
            {
                 new CustomerListDto
                {
                    Id = _customers[0].Id,
                    Email = _customers[0].Email,
                    Name = _customers[0].Name,
                    Address = new AddressListDto
                    {
                        Id = _customers[0].Address.Id,
                        AddressLine = _customers[0].Address.AddressLine,
                        City = _customers[0].Address.City,
                        Country = _customers[0].Address.Country,
                        CityCode = _customers[0].Address.CityCode
                    }
                },
                new CustomerListDto
                {
                    Id = _customers[1].Id,
                    Email = _customers[1].Email,
                    Name = _customers[1].Name,
                    Address = new AddressListDto
                    {
                        Id = _customers[1].Address.Id,
                        AddressLine = _customers[1].Address.AddressLine,
                        City = _customers[1].Address.City,
                        Country = _customers[1].Address.Country,
                        CityCode = _customers[1].Address.CityCode
                    }
                }
            };
            _mapper.Setup(_ => _.Map<List<CustomerListDto>>(_customers)).Returns(customerDtos);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = customerService.Get();

            // Assert
            Assert.Equal(customerDtos, result);
        }

        [Fact]
        public async Task Create_WithGivenCustomerProperties_ReturnsCustomerId()
        {
            // Arrange
            var customerCreateDto = new CustomerCreateDto
            {
                Email = _customers[0].Email,
                Name = _customers[0].Name,
                Address = new AddressCreateDto
                {
                    AddressLine = _customers[0].Address.AddressLine,
                    City = _customers[0].Address.City,
                    Country = _customers[0].Address.Country,
                    CityCode = _customers[0].Address.CityCode
                }
            };
            var customerToCreate = _customers[0];
            _customerCreateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CustomerCreateDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _mapper.Setup(_ => _.Map<Entities.Customer>(It.IsAny<CustomerCreateDto>())).Returns(customerToCreate);
            _unitOfWork.Setup(_ => _.GetWriteRepository<Entities.Customer>().CreateAsync(customerToCreate)).ReturnsAsync(1);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = await customerService.Create(customerCreateDto);

            // Assert
            Assert.Equal(result, customerToCreate.Id);
        }

        [Fact]
        public async Task Create_WithInvalidCustomerProperties_ThrowsBadRequestException()
        {
            // Arrange
            var customerCreateDto = new CustomerCreateDto
            {
                Email = _customers[0].Email,
                Name = _customers[0].Name,
                Address = new AddressCreateDto
                {
                    AddressLine = _customers[0].Address.AddressLine,
                    City = _customers[0].Address.City,
                    Country = _customers[0].Address.Country,
                    CityCode = _customers[0].Address.CityCode
                }
            };
            var customerToCreate = _customers[0];
            _customerCreateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CustomerCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorMessage = "Error occured!" } } });
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = async () => await customerService.Create(customerCreateDto);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), result);
        }

        [Fact]
        public async Task Update_WithValidCustomerUpdateDtoProperties_ReturnsTrue()
        {
            // Arrange
            var customerUpdateDto = new CustomerUpdateDto
            {
                Id = _customers[0].Id,
                Email = "updated@gmail",
                Name = "updatedUser",
                Address = new AddressUpdateDto
                {
                    Id = _customers[0].Address.Id,
                    AddressLine = "updatedAddressLines",
                    City = "updatedCity",
                    Country = "updatedCountry",
                    CityCode = 34
                }
            };
            var customerToUpdate = new Entities.Customer
            {
                Id = customerUpdateDto.Id,
                Email = customerUpdateDto.Email,
                Name = customerUpdateDto.Name,
                UpdatedAt = DateTime.Now,
                CreatedAt = _customers[0].CreatedAt,
                Address = new Address
                {
                    Id = Guid.Parse("2D2C946F-1936-40C6-9C4B-0856DCB526CE"),
                    AddressLine = customerUpdateDto.Address.AddressLine,
                    City = customerUpdateDto.Address.City,
                    Country = customerUpdateDto.Address.Country,
                    CityCode = customerUpdateDto.Address.CityCode,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = _customers[0].Address.CreatedAt,
                }
            };
            _customerUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CustomerUpdateDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            _unitOfWork.Setup(_ => _.GetReadRepository<Entities.Customer>().GetByIdAsync(customerUpdateDto.Id, It.IsAny<bool>(), _ => _.Address)).ReturnsAsync(_customers[0]);
            _unitOfWork.Setup(_ => _.GetWriteRepository<Entities.Customer>().UpdateAsync(It.IsAny<Entities.Customer>())).ReturnsAsync(1);
            _mapper.Setup(_ => _.Map(It.IsAny<CustomerUpdateDto>(), It.IsAny<Entities.Customer>())).Returns(customerToUpdate);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = await customerService.Update(customerUpdateDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Update_WithInvalidCustomerId_ThrowsNotFoundException()
        {
            // Arrange
            var customerUpdateDto = new CustomerUpdateDto
            {
                Id = _customers[0].Id,
                Email = "updated@gmail",
                Name = "updatedUser",
                Address = new AddressUpdateDto
                {
                    Id = _customers[0].Address.Id,
                    AddressLine = "updatedAddressLines",
                    City = "updatedCity",
                    Country = "updatedCountry",
                    CityCode = 34
                }
            };
            var customerToUpdate = new Entities.Customer
            {
                Id = customerUpdateDto.Id,
                Email = customerUpdateDto.Email,
                Name = customerUpdateDto.Name,
                UpdatedAt = DateTime.Now,
                CreatedAt = _customers[0].CreatedAt,
                Address = new Address
                {
                    Id = Guid.Parse("2D2C946F-1936-40C6-9C4B-0856DCB526CE"),
                    AddressLine = customerUpdateDto.Address.AddressLine,
                    City = customerUpdateDto.Address.City,
                    Country = customerUpdateDto.Address.Country,
                    CityCode = customerUpdateDto.Address.CityCode,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = _customers[0].Address.CreatedAt,
                }
            };
            _customerUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CustomerUpdateDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
            _unitOfWork.Setup(_ => _.GetReadRepository<Entities.Customer>().GetByIdAsync(customerUpdateDto.Id, It.IsAny<bool>(), _ => _.Address)).ReturnsAsync((Entities.Customer)null);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = async () => await customerService.Update(customerUpdateDto);

            // Assert
            await Assert.ThrowsAsync(typeof(NotFoundException), result);
        }

        [Fact]
        public async Task Update_WithInvalidCustomerUpdateDtoProperties_ThrowsBadRequestException()
        {
            // Arrange
            var customerUpdateDto = new CustomerUpdateDto
            {
                Id = _customers[0].Id,
                Email = "updated@gmail",
                Name = "updatedUser",
                Address = new AddressUpdateDto
                {
                    Id = _customers[0].Address.Id,
                    AddressLine = "updatedAddressLines",
                    City = "updatedCity",
                    Country = "updatedCountry",
                    CityCode = 34
                }
            };
            var customerToUpdate = new Entities.Customer
            {
                Id = customerUpdateDto.Id,
                Email = customerUpdateDto.Email,
                Name = customerUpdateDto.Name,
                UpdatedAt = DateTime.Now,
                CreatedAt = _customers[0].CreatedAt,
                Address = new Address
                {
                    Id = Guid.Parse("2D2C946F-1936-40C6-9C4B-0856DCB526CE"),
                    AddressLine = customerUpdateDto.Address.AddressLine,
                    City = customerUpdateDto.Address.City,
                    Country = customerUpdateDto.Address.Country,
                    CityCode = customerUpdateDto.Address.CityCode,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = _customers[0].Address.CreatedAt,
                }
            };
            _customerUpdateDtoValidator.Setup(_ => _.ValidateAsync(It.IsAny<CustomerUpdateDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult { Errors = new List<ValidationFailure> { new ValidationFailure { ErrorMessage = "Error occured" } } });
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = async () => await customerService.Update(customerUpdateDto);

            // Assert
            await Assert.ThrowsAsync(typeof(BadRequestException), result);
        }

        [Fact]
        public async Task Delete_WithValidCustomerId_ReturnsTrue()
        {
            // Arrange
            var customer = _customers[0];
            _unitOfWork.Setup(_ => _.GetReadRepository<Entities.Customer>().GetByIdAsync(customer.Id, It.IsAny<bool>(), It.IsAny<Expression<Func<Entities.Customer, object>>[]>())).ReturnsAsync(customer);
            _unitOfWork.Setup(_ => _.GetWriteRepository<Entities.Customer>().DeleteAsync(customer)).ReturnsAsync(1);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = await customerService.Delete(customer.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_WithInvalidCustomerId_ThrowsNotFoundException()
        {
            // Arrange
            var customer = _customers[0];
            _unitOfWork.Setup(_ => _.GetReadRepository<Entities.Customer>().GetByIdAsync(customer.Id, It.IsAny<bool>(), It.IsAny<Expression<Func<Entities.Customer, object>>>())).ReturnsAsync((Entities.Customer)null);
            var customerService = new CustomerService(_unitOfWork.Object, _mapper.Object, _customerCreateDtoValidator.Object, _customerUpdateDtoValidator.Object);

            // Act
            var result = async () => await customerService.Delete(customer.Id);

            // Assert
            await Assert.ThrowsAsync(typeof(NotFoundException), result);
        }
    }
}
