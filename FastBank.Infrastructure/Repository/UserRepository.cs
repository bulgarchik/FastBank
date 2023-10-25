using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Context;

namespace FastBank.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Repository _repo;

        public UserRepository()
        {
            _repo = new Repository(new FastBankDbContext());
        }

        public List<User> GetAll()
        {
            var users = _repo.SetNoTracking<UserDTO>();

            
            return new List<User>();
        }

        public User? GetByEmail(string email) 
        {
            var user = _repo.SetNoTracking<UserDTO>()
                                .Where(c => c.Email == email)
                                .Select(a => a.ToDomainObj())
                                .ToList()
                                .FirstOrDefault();
            return user;
        }

        public void Add(User user)
        {
            var userDTO = new UserDTO(user);
            _repo.Add(userDTO);
        }
    }
}
