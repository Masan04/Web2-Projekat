using Projekat.Dto;
using Projekat.Models;

namespace Projekat.Repository
{
    public class UserRepository
    {

        private readonly DataContext _dataContext; 

        public UserRepository(DataContext dataContext)
        {

            _dataContext = dataContext;

        }

        public User FindUser(string email)

        {
            User? user = _dataContext.Users.FirstOrDefault(u => u.UserEmail.Equals(email));
            return user;
        }

        public void AddUser(User user)
        {
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
        }
        public void SaveUser(User newUser)
        {
            _dataContext.Users.Add(newUser);
            _dataContext.SaveChanges();
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }


        public List<User> FindNonAdminUsers()
        {
            return _dataContext.Users.ToList().FindAll(x => x.Type != UserType.ADMIN);
        }


        public  User? FindUserById(long id)
        {
            return _dataContext.Users.Find(id);
        }

        public User UpdateUserRepo(long id, UserRegisterDto account)
        {
            User userDb = _dataContext.Users.Find(id);
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            userDb.UserEmail = account.UserEmail;
            userDb.Password = account.Password;
            userDb.Name = account.Name;
            userDb.Surname = account.Surname;
            userDb.UserName = account.Username;
            userDb.Date = account.Date;
            userDb.Address = account.Address;
            userDb.Picture = account.Picture;

            _dataContext.SaveChanges();
            return userDb;
        }
    }
}
