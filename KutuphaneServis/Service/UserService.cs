using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace KutuphaneServis.Service
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IGenericRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public IResponse<UserCreateDto> CreateUser(UserCreateDto user)
        {
            if (user == null)
            {
                ResponseGeneric<UserCreateDto>.Error("Kullanıcı bilgileri boş olamaz.");
            }

            if (string.IsNullOrEmpty(user.Username) && string.IsNullOrEmpty(user.Email))
            {
                return ResponseGeneric<UserCreateDto>.Error("Kullanıcı adı veya e-posta adresi boş olamaz.");
            }

            var existingUser = _userRepository.GetAll()
                .FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);
            if (existingUser != null)
            {
                return ResponseGeneric<UserCreateDto>.Error("Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor.");
            }


            var hashedPassword = HashPassword(user.Password);

            //gelen dto'yu User entity'sine dönüştür
            var newUser = new User
            {
                Name = user.Name,
                Surname = user.Surname,
                Username = user.Username,
                Email = user.Email,
                Password = hashedPassword,
                Role = "User",
                RecordDate = DateTime.Now
            };
            _userRepository.Create(newUser);

            return ResponseGeneric<UserCreateDto>.Success(user, "Kullanıcı kaydı oluşturuldu.");

        }

        public IResponse<string> Login(UserLoginDto user)
        {
            if (user.Username == null || user.Email == null && user.Password == null)
            {
                return ResponseGeneric<string>.Error("Kullanıcı adı veya e-posta adresi boş olamaz.");
            }

            var checkUser = _userRepository.GetAll()
                 .FirstOrDefault(u => (u.Username == user.Username || u.Email == user.Email) && u.Password == HashPassword(user.Password));

            if (checkUser == null)
            {
                return ResponseGeneric<string>.Error("Kullanıcı adı veya şifre hatalı.");
            }

            var generateToken = GenerateJwtToken(checkUser);

            return ResponseGeneric<string>.Success(generateToken, "Giriş başarılı.");
        }


        private string HashPassword(string password)
        {
            string secretKey = "nPU.4I|ll@r${4g";
            using (var sha256 = SHA256.Create())
            {
                var combinedPassword = password + secretKey;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                var hashedPassword = Convert.ToBase64String(bytes);
                return hashedPassword;
            }
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("name", user.Name),
                new Claim("sid", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),    
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
