using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class UserAccessor
    {
        public static IEnumerable<User> GetUsersList() {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var users = context.Users.ToList();
                return users;
            }
        }

        public static long AddUser(User user) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.Users.Add(user);
                context.SaveChanges();

                return user.User_ID;
            }
        }

        public static bool CheckUserNameExists(string userName) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                User user = context.Users.FirstOrDefault(t => t.User_Name.ToLower().Trim() == userName.ToLower().Trim());

                return user != null;
            }
        }

        public static bool CheckUserEmailExists(string userEmail) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                User user = context.Users.FirstOrDefault(t => t.Email_Address.ToLower().Trim() == userEmail.ToLower().Trim());

                return user != null;
            }
        }

        public static void DeleteUser(User user) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        public static void UpdateUser(User user) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                User oldUser = context.Users.FirstOrDefault(r => r.User_ID == user.User_ID);
                oldUser.User_Name = user.User_Name;
                oldUser.Profile_Picture_Path = user.Profile_Picture_Path;
                oldUser.Password = user.Password;

                context.SaveChanges();
            }
        }

        public static string GetUserName(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                User user = context.Users.FirstOrDefault(t => t.User_ID == id);

                return user.User_Name;
            }
        }

        public static long GetUserIdByName(string userName) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                User user = context.Users.FirstOrDefault(t => t.User_Name.ToLower().Trim() == userName.ToLower().Trim());

                return user.User_ID;
            }
        }
    }
}
