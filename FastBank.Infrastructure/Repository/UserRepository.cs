using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.DTOs;
using FastBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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
            var users = _repo.SetNoTracking<UserDTO>().Select(u => u.ToDomainObj()).ToList();

            return users;
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
            var userDTO =
                new UserDTO(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Birthday,
                    user.Password,
                    user.Role,
                    user.Inactive);

            _repo.Add(userDTO);
        }

        public List<User> GetUserFriends(User user)
        {
            var friends = _repo.SetNoTracking<UserFriendDTO>()
                                    .Where(u => u.UserId == user.Id)
                                    .Include(u => u.User)
                                    .Include(u => u.Friend)
                                    .Select(u => u.Friend.ToDomainObj())
                                    .ToList();
            return friends;
        }

        public void AddFriend(User user, User friend)
        {
            _repo.Add<UserFriendDTO>(new UserFriendDTO(Guid.NewGuid(), user, friend, false));
        }

        public void RemoveFriend(User user, User friend)
        {
            var friendRelation = _repo.Set<UserFriendDTO>().Where(u => u.UserId == user.Id && u.FriendId == friend.Id).FirstOrDefault();
            if (friendRelation != null)
            {
                _repo.Delete<UserFriendDTO>(friendRelation);
            }
        }
    }
}
