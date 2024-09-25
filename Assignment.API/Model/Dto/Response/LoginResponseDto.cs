using Assignment.API.Model.Dto.Request;

namespace Assignment.API.Model.Dto.Response
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
