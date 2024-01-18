using DatabaseRepository;
using ModelsLibrary;
using System;
using System.Linq;

namespace DataAccess {
    public class UserAccessor : IUserAccessor {

        public RecipesRealmContext context;

        public UserAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public User AddUser(User user) {
            user.Registration_Date = DateTime.Now;

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public bool CheckUserEmailExists(string userEmail) {
            User user = context.Users.FirstOrDefault(t => t.Email_Address.ToLower().Trim() == userEmail.ToLower().Trim());

            return user != null;
        }

        public string GetUserName(long id) {
            User user = context.Users.FirstOrDefault(t => t.User_ID == id);

            return user.User_Name;
        }

        public User CheckUserForLogin(User user) {
            User loggedInUser = context.Users.FirstOrDefault(u => u.Email_Address == user.Email_Address && u.Password == user.Password);

            return loggedInUser;
        }
    }

    public interface IUserAccessor {

        User AddUser(User user);

        bool CheckUserEmailExists(string userEmail);

        string GetUserName(long id);

        User CheckUserForLogin(User user);
    }
}
