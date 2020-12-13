using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string UsernamePattern = @"^(?=.{5,20}$)[a-zA-Z0-9._]+$"; //5-20 symbols, [_, .], but only one in a row and can't be last or first symbol
        private const string PasswordPattern = @"^(?=^.{8,}$)(?=.*\d)(?!.*\s)(?=.*[A-Z])(?=.*[a-z]).*$"; //8+ symbols, numeral, uppercase, under-case, without whitespaces
        private const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        private static readonly Regex UsernameRegex;
        private static readonly Regex PasswordRegex;
        private static readonly Regex EmailRegex;


        private readonly IHasher _hasher;
        private readonly IForumUnitOfWork _unitOfWork;

        private readonly ILogger<AuthenticationService> _logger;


        public AuthenticationService(IHasher hasher, IForumUnitOfWork unitOfWork, ILogger<AuthenticationService> logger)
        {
            _hasher = hasher;
            _unitOfWork = unitOfWork;

            _logger = logger;
        }

        static AuthenticationService()
        {
            UsernameRegex = new Regex(UsernamePattern);
            PasswordRegex = new Regex(PasswordPattern);
            EmailRegex = new Regex(EmailPattern);
        }


        public async Task<ServiceResult<SignUpError>> SignUp(SignUpDto signUpDto)
        {
            var isValidUsername = CheckIfValidUsername(signUpDto.Username);
            if (!isValidUsername)
            {
                return ServiceResult.CreateFailed(SignUpError.InvalidUsername);
            }
            var isValidPassword = CheckIfValidPassword(signUpDto.Password);
            if (!isValidPassword)
            {
                return ServiceResult.CreateFailed(SignUpError.InvalidPassword);
            }
            var isValidEmail = CheckIfValidEmail(signUpDto.Email);
            if (!isValidEmail)
            {
                return ServiceResult.CreateFailed(SignUpError.InvalidEmail);
            }
            var isValidPasswordConfirmation = signUpDto.PasswordConfirmation == signUpDto.Password;
            if (!isValidPasswordConfirmation)
            {
                return ServiceResult.CreateFailed(SignUpError.PasswordAndConfirmationNotSame);
            }

            var userRepository = _unitOfWork.UserRepository;
            var duplicateUser = await userRepository.FirstOrDefaultAsync(u => u.Username == signUpDto.Username);
            if (duplicateUser != null)
            {
                return ServiceResult.CreateFailed(SignUpError.DuplicateUsername);
            }
            duplicateUser = await userRepository.FirstOrDefaultAsync(u => u.Email == signUpDto.Email);
            if (duplicateUser != null)
            {
                return ServiceResult.CreateFailed(SignUpError.DuplicateEmail);
            }

            var passwordHash = _hasher.GetHash(signUpDto.Password);
            var newUser = new User
            {
                Username = signUpDto.Username,
                Email = signUpDto.Email,
                PasswordHash = passwordHash,
                ProfileImageId = null
            };
            userRepository.Create(newUser);
            await _unitOfWork.SaveAsync();

            return ServiceResult<SignUpError>.CreateSuccess();
        }


        private static bool CheckIfValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            var isUsernameValid = UsernameRegex.IsMatch(username);

            return isUsernameValid;
        }

        private static bool CheckIfValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var isPasswordValid = PasswordRegex.IsMatch(password);

            return isPasswordValid;
        }

        private static bool CheckIfValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var isEmailValid = EmailRegex.IsMatch(email);

            return isEmailValid;
        }
    }
}