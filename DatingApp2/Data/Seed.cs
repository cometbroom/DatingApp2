using System.Collections.Generic;
using System.Linq;
using DatingApp2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DatingApp2.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if (!context.Users.Any())
            {
                // Read json data and turn it into an object.
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var format = "yyyy-MM-dd";
                var dateTimeConvert = new IsoDateTimeConverter { DateTimeFormat = format };
                var users = JsonConvert.DeserializeObject<List<User>>(userData, dateTimeConvert);

                //Loop for creating hash/salt and adding users to the context
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;

                    CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();      //Take from RAM add to hard drive
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
           
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
		}
    }
}