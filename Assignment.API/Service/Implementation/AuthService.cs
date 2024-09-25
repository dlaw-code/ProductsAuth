using Assignment.API.Data;
using Assignment.API.Entity;
using Assignment.API.Model;
using Assignment.API.Model.Dto.Request;
using Assignment.API.Model.Dto.Response;
using Assignment.API.Service.Interface;
using Microsoft.AspNetCore.Identity;

namespace Assignment.API.Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _emailService = emailService;
        }


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);


            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //If user was found, Generate JWT token

            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }


        public async Task<ResponseDto<UserDto>> Register(RegistrationRequestDto registrationRequestDto)
        {
            var user = new ApplicationUser
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
            };

            var response = new ResponseDto<UserDto>();

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    // Generate email confirmation token
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Hardcoded confirmation link
                    var confirmationLink = $"https://localhost:44376/api/Auth/confirm-email?token={token}&email={user.Email}";

                    // Prepare the email message
                    var emailBody = $"<h1>Confirm your email</h1>" +
                                    $"<p>Please confirm your email by clicking the link below:</p>" +
                                    $"<a href='{confirmationLink}'>Confirm Email</a>";

                    var email = new Message("Confirm Email", new List<string> { user.Email }, emailBody);

                    // Send confirmation email
                    await _emailService.Send(email);

                    // Create the UserDto to return
                    var userResponse = new UserDto
                    {
                        ID = user.Id, // Use the ID property from the user object
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };

                    response.IsSuccess = true;
                    response.Result = userResponse; // Set the result
                    response.Message = "Registration successful. Please check your email for confirmation.";
                }
                else
                {
                    // Handle registration errors
                    response.IsSuccess = false;
                    response.Message = "Registration failed. Please check your details and try again.";
                    response.Errors.AddRange(result.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                // Log exception
                response.IsSuccess = false;
                response.Message = "Error encountered: " + ex.Message;
            }

            return response; // Return the response DTO
        }

    }
}
