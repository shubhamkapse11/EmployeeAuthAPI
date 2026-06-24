using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webApp.Data;
using webApp.Dto;
using webApp.Iservices;

namespace webApp.Services
{
    public class AuthService : IAuthService
    {

        //dependencies injection
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tuple<int, string>> LoginUser(UserDto dto)
        {
            try
            {
                var userEmailExist = await _context.AccountUser.FirstOrDefaultAsync(x => x.Email == dto.Email);

                if (userEmailExist == null)
                {
                    return new Tuple<int, string>(0, "Email not found");
                }

                //if (userEmailExist.Password != dto.Password)
                //{
                //    return new Tuple<int, string>(1, "Password is wrong");

                //}
                var passwordHasher = new PasswordHasher<string>();

                var verificationResult = passwordHasher.VerifyHashedPassword(dto.Email, userEmailExist.Password, dto.Password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    return new Tuple<int, string>(2, "Login sucessful");
                }
                else if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    // If the password is valid but needs rehashing, you can update the stored hash

                    userEmailExist.Password = PasswordHashing(dto);
                    _context.AccountUser.Update(userEmailExist);
                    _context.SaveChanges();
                    return new Tuple<int, string>(2, "Login sucessful");

                }
                else if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return new Tuple<int, string>(1, "Password is wrong");
                }
                else
                {
                    return new Tuple<int, string>(1, "An error occurred during password verification");
                }
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(-1, "An error occurred: " + ex.Message);
            }
        }

        public async Task<Tuple<int,string>> RegisterUser(UserDto dto)
        {
            try
            {
                var userExist = await _context.AccountUser.AnyAsync(x => x.Email == dto.Email);
                if (userExist)
                {
                    return new Tuple<int, string>(0, "User alrady exists,Plase Login");
                }
                _context.AccountUser.Add(new Entities.User {
                    Id = Guid.NewGuid(),
                    Name = dto.Name ,
                    Email = dto.Email,
                    Password = PasswordHashing(dto)
                });

                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, "User register Successfully");
                
            }
            catch(Exception ex)
            {
                return null;
            }

        }

       private string PasswordHashing(UserDto userdto)
        {
            var hashedPassword = new PasswordHasher<string>();
            var hash = hashedPassword.HashPassword(userdto.Email, userdto.Password);

            return hash;
           
        }

    }
}
