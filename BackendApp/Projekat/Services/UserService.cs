using AutoMapper;
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
        private readonly UserRepository _userRepository;

        private string SecretKey { get; set; }

        public UserService(IMapper mapper, IConfiguration config, UserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            SecretKey = config.GetSection("Authentication:SecretKey").Value;
        }

        public UserRegisterDto AddUser(UserRegisterDto account)
        {
            User user = _mapper.Map<User>(account);

            try
            {
                if (_userRepository.FindUser(account.UserEmail) != null )
                {
                    throw new Exception();
                }

      
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _userRepository.AddUser(user);

                    if (user.Type == UserType.SELLER)
                        user.Status= VerificationStatus.IN_PROCESS;
            }
            catch (Exception e)
            {
                return null;
            }
                return _mapper.Map<UserRegisterDto>(user);
         
        }

        public void UpdateVerificationStatus(bool isVerified, string email)
        {
            User user = _userRepository.FindUser(email);

            if (isVerified)
            {
                
                user.Status = VerificationStatus.ACCEPTED;
            }
            else
            {
                user.Status = VerificationStatus.DENIED;
            }

            _userRepository.SaveChanges();
        }

        public string LoginUser(UserLoginDto account)
        {
            User user;

            try
            {
                user = _userRepository.FindUser(account.Email);

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
                    return GenerateToken(claims);
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
            try
            {
                User userDb = _userRepository.UpdateUserRepo(id, account);
                return _mapper.Map<UserRegisterDto>(userDb);
            }
            catch (Exception e)
            {
                return null;  
            }


        }

        public UserRegisterDto GetByEmail(string email)
        {
            try
            {
                return _mapper.Map<UserRegisterDto>(_userRepository.FindUser(email));
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
                return _mapper.Map<UserRegisterDto>(_userRepository.FindUserById(id));

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
                return _mapper.Map<List<UserRegisterDto>>(_userRepository.FindNonAdminUsers());

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
                user = _userRepository.FindUser(account.Email);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "buyer"));

                return GenerateToken(claims);
            }
            catch (Exception ex)
            {
                User newUser = _mapper.Map<User>(account);
                newUser.Type = UserType.BUYER;
                _userRepository.SaveUser(newUser);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "buyer"));

                return GenerateToken(claims);
            }
        }

        private string GenerateToken(List<Claim> claims)
        {
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:7293", //url servera koji je izdao token
                claims: claims, //claimovi
                expires: DateTime.Now.AddMinutes(20), //vazenje tokena u minutama
                signingCredentials: signinCredentials //kredencijali za potpis
            );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

    }
}
