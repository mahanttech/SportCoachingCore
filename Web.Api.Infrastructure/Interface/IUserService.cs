using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Core.Context;
using Web.Api.Core.Entity;
using Web.Api.Infrastructure.Helper;

namespace Web.Api.Infrastructure.Interface
{
    public interface IUserService
    {
        SystemUsers Authenticate(string username, string password);
        IEnumerable<SystemUsers> GetAll();
        SystemUsers GetById(int id);
        SystemUsers Create(SystemUsers user, string password);
        void Update(SystemUsers user, string password = null);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private SportTestContext _context;

        public UserService(SportTestContext context)
        {
            _context = context;
        }

        public SystemUsers Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.SystemUsers.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<SystemUsers> GetAll()
        {
            return _context.SystemUsers;
        }

        public SystemUsers GetById(int id)
        {
            return _context.SystemUsers.Find(id);
        }

        public SystemUsers Create(SystemUsers user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.SystemUsers.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.SystemUsers.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(SystemUsers userParam, string password = null)
        {
            var user = _context.SystemUsers.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_context.SystemUsers.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.SystemUsers.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.SystemUsers.Find(id);
            if (user != null)
            {
                _context.SystemUsers.Remove(user);
                _context.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
