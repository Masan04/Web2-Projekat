using Projekat.Dto;

namespace Projekat.Interfaces
{
    public interface IUserService
    {
        UserRegisterDto AddUser(UserRegisterDto account);
        public void UpdateVerificationStatus(bool isVerified, string email);

        string LoginUser(UserLoginDto account);

        UserRegisterDto UpdateUser(long id, UserRegisterDto account);

        UserRegisterDto GetByEmail(string email);

        UserRegisterDto GetUserById(long id);

        List<UserRegisterDto> GetAll();

        string LoginGoogle(UserLoginDto account);

    }
}
