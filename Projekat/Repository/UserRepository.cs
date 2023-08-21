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
            return _dataContext.Users.FirstOrDefault(u => u.UserEmail == email);
        }

        public void AddUser(User user)
        {
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
        }
    }
}
