using Assignment.API.Model.Dto.Request;
using Assignment.API.Model.Dto.Response;

namespace Assignment.API.Service.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto<UserDto>> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
