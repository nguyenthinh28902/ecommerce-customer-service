using AutoMapper;
using CustomerIdentityService.Core.Abstractions.Interfaces.Security;
using CustomerIdentityService.Core.Abstractions.Persistence;
using CustomerIdentityService.Core.Dtos.Customers;
using CustomerIdentityService.Core.Dtos.Google;
using CustomerIdentityService.Core.Entities;
using CustomerIdentityService.Core.Interfaces.Services;
using CustomerIdentityService.Core.Models;

namespace CustomerIdentityService.Application.Services.CustomerServices
{
    public class Customerservice : ICustomerservice
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public Customerservice(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }



        public async Task<Result<Customer>> Registration(Customer customer)
        {
            await _unitOfWork.Repository<Customer>().AddAsync(customer);
            await _unitOfWork.SaveChangesAsync(); // Để lấy ID
            if (customer.Id != null && customer.Id != 0)
            {
                return Result<Customer>.Success(customer, "Đăng ký thành công");

            }
            return Result<Customer>.Failure("Đăng ký thất bại");
        }

        public async Task<Result<CustomerDto>> GetAuthenticatedCustomer()
        {
            var userId = _currentUserService.Id;

            var customer = await _unitOfWork.Repository<Customer>().FindAsync(userId);
            if (customer == null) return Result<CustomerDto>.Failure("Thông tin khách hàng không tồn tại");
            var newCustomerDto = _mapper.Map<CustomerDto>(customer);

            return Result<CustomerDto>.Success(newCustomerDto, "Thông tin khách hàng");
        }
        public async Task<Result<ConfirmCustomerInforResponse>> CreateCustomerSingin(UserInfoSigninDto googleUser, string ProviderName)
        {
            var provider = await _unitOfWork.Repository<CustomerAuthProvider>().FirstOrDefaultAsync(p => p.Provider == ProviderName
             && p.ProviderUserId == googleUser.ProviderUserId);
            var confirmCustomerInfor = _mapper.Map<Customer>(googleUser);
            if (provider == null)
            {
                // 3. Nếu chưa có: Tạo mới Customer và tạo liên kết Provider

                var newCustomerAuthProvider = new CustomerAuthProvider();
                newCustomerAuthProvider.Provider = ProviderName;
                newCustomerAuthProvider.ProviderUserId = googleUser.ProviderUserId;
                newCustomerAuthProvider.CreatedAt = DateTime.Now;
                confirmCustomerInfor.CustomerAuthProviders.Add(newCustomerAuthProvider);

                var result = await Registration(confirmCustomerInfor);
                if (result.IsSuccess == false) { throw new Exception("Xác thực Google thất bại"); }
                provider = result.Data?.CustomerAuthProviders.FirstOrDefault();
            }
            else
            {
                // 4. Nếu có rồi: Lấy thông tin Customer từ liên kết Provider
                var existingCustomer = await _unitOfWork.Repository<Customer>().FindAsync(provider.CustomerId);
                if (existingCustomer == null)
                {
                    throw new Exception("Xác thực Google thất bại - Khách hàng không tồn tại");
                }

                confirmCustomerInfor = existingCustomer;
            }
            var confirmCustomerInforResponse = new ConfirmCustomerInforResponse();
            confirmCustomerInforResponse.Id = confirmCustomerInfor.Id;
            confirmCustomerInforResponse.Email = confirmCustomerInfor.Email ?? string.Empty;
            confirmCustomerInforResponse.PhoneNumber = confirmCustomerInfor.PhoneNumber ?? string.Empty;

            return Result<ConfirmCustomerInforResponse>.Success(confirmCustomerInforResponse, "Đăng ký thành công");
        }

    }
}
