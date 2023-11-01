using FastBank.Domain.RepositoryInterfaces;
using FastBank.Infrastructure.Repository;
using System.Text.RegularExpressions;

namespace FastBank.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMenuService _menuService;

        public UserService()
        {
            _userRepo = new UserRepository();
            _menuService = new MenuService();
        }

        public List<User> GetAll()
        {
            return _userRepo.GetAll();
        }

        public void Add(User user)
        {
            _userRepo.Add(user);
        }

        public void Add(string name, string email, DateTime birthday, string password, Roles role, bool inactive)
        {
            var user = new User(Guid.NewGuid(), name, email, birthday, password, role, inactive);
            
            if (user.Role == Roles.Customer)
            {
                user = new Customer(user);
            }

            var validationErrors = ValidatеUser(user);
            if (validationErrors.Any())
            {
                Console.WriteLine("\nUser data is not valid:");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Please try again!");
                Console.ReadKey();
            }
            else
            {
                Add(user);
            }
        }

        public List<string> ValidatеUser(User user)
        {
            var validationErrors = new List<string>();
            UserExist(user, validationErrors);

            //TODO validate password 
            //ValidateEmail(user.Email, validationErrors);
            UserAgeIsValid(user, validationErrors);
            //TODO validate role
            return validationErrors;
        }

        public List<string> UserExist(User user, List<string> validationErrors)
        {
            if (_userRepo.GetByEmail(user.Email) != null)
            {
                validationErrors.Add($"User with email: {user.Email} already exist");
            }
            return validationErrors;
        }

        public List<string> UserAgeIsValid(User user, List<string> validationErrors)
        {
            var age = DateTime.Now.Year - user.Birthday.Year;
            if (DateTime.Now.DayOfYear < user.Birthday.DayOfYear)
            {
                age = age - 1;
            }

            if (age < 18)
            {
                validationErrors.Add($"The user is under 18 years old");
            }
            else if (age > 100)
            {
                validationErrors.Add($"The user is over 100 years old");
            }
            return validationErrors;
        }

        public List<string> CheckLoginUserName(string email)
        {
            var validationErrors = new List<string>();
            var user = _userRepo.GetByEmail(email);
            if (user == null)
            {
                validationErrors.Add($"User with username(email): {email} not exist");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Unauthorized");
                Console.WriteLine("Please try again!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            return validationErrors;
        }

        public User? Login(string email, string password)
        {
            var user = _userRepo.GetByEmail(email);
            
            if (user == null)
            {
                Console.WriteLine($"\nUser with name: {email} not exist. Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                if (user.Inactive)
                {
                    Console.WriteLine($"User with name: {email} deactivated. Please contact Administration");
                    user = null;
                    return user;
                }
                var passwordtries = 0;
                var menuServie = new MenuService();
                while (passwordtries < 2)
                {
                    if (user.Password != password)
                    {
                        Console.WriteLine($"Wrong password! Press any key to try again!");
                        var keyIsEnter = Console.ReadKey();
                        new MenuService().MoveToPreviousLine(keyIsEnter, 2);
                        passwordtries++;
                        Console.WriteLine("Please input password:");
                        
                        password = menuServie.PasswordStaredInput()??string.Empty;
                        password = Console.ReadLine()??string.Empty;
                        if (password == string.Empty)
                        {
                            new MenuService().MoveToPreviousLine(keyIsEnter, 1);
                        }
                    }
                    else
                    {
                        return user;
                    }
                }
                if (passwordtries == 2)
                {
                    Console.WriteLine("You try to login with wrong password 3 times! Press any key to continue...");
                    Console.ReadKey(true);
                    user = null;
                }
            }
                        
            return user;
        }

        public List<User> GetUserFriends(User user)
        {
            return _userRepo.GetUserFriends(user);
        }

        public void AddFriend(User user, List<User> friendsList)
        {
            var inquiryMsg = "Please input user's email to add him as a friend (type \"quit\" for exit):";
            var emailTypeToInput = "Friend email:";
            var emailFriend = _menuService.InputEmail(inquiryMsg, emailTypeToInput);

            if (emailFriend == "quit")
                return;

            var friend = _userRepo.GetByEmail(emailFriend);

            if (friend != null)
            {
                if (friendsList.Any(u => u.Id == friend.Id))
                {
                    Console.WriteLine("This user is your friend already... Press any key to continue!");
                    Console.ReadKey(true);
                    return;
                }

                _userRepo.AddFriend(user, friend);
            }
            else
            {
                Console.WriteLine("We have no user with such email... Press any key to continue!");
                Console.ReadKey(true);
            }
        }

        public void RemoveFriend(User user)
        {
            throw new NotImplementedException();
        }
    }
}
