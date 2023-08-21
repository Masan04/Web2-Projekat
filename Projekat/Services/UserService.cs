﻿using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Projekat.Dto;
using Projekat.Interfaces;
using Projekat.Models;
using Projekat.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Projekat.Services
{
         
    public class UserService : IUserService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IVerificationService _verificationService;
        private readonly UserRepository _userRepository;

        private string SecretKey { get; set; }

        public UserService(IMapper mapper,DataContext dataContext, IConfiguration config, IVerificationService verificationService, UserRepository userRepository)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _verificationService = verificationService;
            _userRepository = userRepository;
            SecretKey = config.GetSection("Authentication:SecretKey").Value;
        }

        public UserRegisterDto AddUser(UserRegisterDto account)
        {
            User user = _mapper.Map<User>(account);

            try
            {
                if ( _userRepository.FindUser(account.UserEmail) != null)
                    return null;
            }
            catch (Exception e)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _userRepository.AddUser(user);

                if (user.Type == UserType.SELLER)
                    _verificationService.CreateVerification(user.Id);
            }

            return _mapper.Map<UserRegisterDto>(user);
        }

        public string LoginUser(UserLoginDto account)
        {
            User user;

            try
            {
                user = _dataContext.Users.First(u => u.UserEmail == account.Email);

                if (BCrypt.Net.BCrypt.Verify(account.Password, user.Password))//Uporedjujemo hes pasvorda iz baze i unetog pasvorda
                {
                    List<Claim> claims = new List<Claim>();
                    //Mozemo dodati Claimove u token, oni ce biti vidljivi u tokenu i mozemo ih koristiti za autorizaciju
                    if (user.Type == UserType.ADMIN)
                        claims.Add(new Claim(ClaimTypes.Role, "admin")); //Add user type to claim
                    if (user.Type == UserType.BUYER)
                        claims.Add(new Claim(ClaimTypes.Role, "buyer")); //Add user type to claim
                    if (user.Type == UserType.SELLER)
                        claims.Add(new Claim(ClaimTypes.Role, "seller")); //Add user type to claim

                    //Kreiramo kredencijale za potpisivanje tokena. Token mora biti potpisan privatnim kljucem
                    //kako bi se sprecile njegove neovlascene izmene
                    SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:7194", //url servera koji je izdao token
                        claims: claims, //claimovi
                        expires: DateTime.Now.AddMinutes(20), //vazenje tokena u minutama
                        signingCredentials: signinCredentials //kredencijali za potpis
                    );
                    string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    return tokenString;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }

        public UserRegisterDto UpdateUser(long id, UserRegisterDto account)
        {
            User newUser = _mapper.Map<User>(account);
            User userDb = _dataContext.Users.Find(id);

            try
            {
                if (_dataContext.Users.First(u => u.UserEmail == newUser.UserEmail && u.Id != userDb.Id) != null)
                    return null;
            }
            catch (Exception e)
            {
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                userDb.UserEmail = newUser.UserEmail;
                userDb.Password = newUser.Password;
                userDb.Name = newUser.Name;
                userDb.Surname = newUser.Surname;
                userDb.UserName = newUser.UserName;
                userDb.Date = newUser.Date;
                userDb.Address = newUser.Address;
                userDb.Picture = newUser.Picture;

                _dataContext.SaveChanges();
            }

            return _mapper.Map<UserRegisterDto>(userDb);  
        }

        public UserRegisterDto GetByEmail(string email)
        {
            try
            {
                return _mapper.Map<UserRegisterDto>(_dataContext.Users.First(u => u.UserEmail == email));
            }
            catch (Exception)
            {
                return null;
            }
        }

       
        public UserRegisterDto GetUserById(long id)
        {
            try
            {
                return _mapper.Map<UserRegisterDto>(_dataContext.Users.Find(id));

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<UserRegisterDto> GetAll()
        {
            try
            {
                return _mapper.Map<List<UserRegisterDto>>(_dataContext.Users.ToList().FindAll(x => x.Type != UserType.ADMIN));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string LoginGoogle(UserLoginDto account)
        {
            User user;
            try
            {
                user = _dataContext.Users.First(u => u.UserEmail == account.Email);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "buyer"));

                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:7194",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signinCredentials
                );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return tokenString;
            }
            catch (Exception ex)
            {
                User newUser = _mapper.Map<User>(account);
                newUser.Type = UserType.BUYER;
                _dataContext.Users.Add(newUser);
                _dataContext.SaveChanges();

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "buyer"));

                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:7194",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signinCredentials
                );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return tokenString;
            }
        }
    }
}
