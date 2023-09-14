using Projekat.Dto;

namespace Projekat.Interfaces
{
    public interface IUserService
    {
        UserRegisterDto AddUser(UserRegisterDto account);
        public void UpdateVerificationStatus(UserRegisterDto user, long id);

        string LoginUser(UserLoginDto account);

        UserRegisterDto UpdateUser(long id, UserRegisterDto account);

        UserRegisterDto GetByEmail(string email);

        UserRegisterDto GetUserById(long id);

        List<UserRegisterDto> GetAll();

        string LoginGoogle(UserRegisterDto account);

    }
}
