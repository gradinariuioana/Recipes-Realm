using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserAccessor
    {
        public static void ShowUsers()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Users ---");

                var users = context.Users.ToList();
                foreach (User item in users)
                {
                    Console.WriteLine("User Name: {0}\nEmail Address: {1}\nRegistration Date: {2}\n\n", 
                                       item.User_Name, item.Email_Address, item.Registration_Date.ToString("dd-MM-yyyy"));
                }
            }
        }

        public static string GetUserName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var user = context.Users.FirstOrDefault(u => u.User_ID == idx);
                return user.User_Name;
            }
        }
    }
}
