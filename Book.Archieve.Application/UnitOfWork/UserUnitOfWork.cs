using Book.Archieve.Application.Repo.UserGroup;
using Book.Archieve.Domain.DTO.Request.User;
using Book.Archieve.Domain.DTO.Response.User;
using Book.Archieve.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.UnitOfWork
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        public IUserRepository UserRepository { get; private set; }
        public IAuthRepository AuthRepository { get; private set; }

        public UserUnitOfWork(IUserRepository userRepository, IAuthRepository authRepository)
        {
            UserRepository = userRepository;
            AuthRepository = authRepository;
        }

        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            User user = UserRepository.Register(registerRequest);
            string token = AuthRepository.GenerateToken(user);
            return new RegisterResponse
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Token = token
            };
        }

        public string Login(LoginRequest loginRequest)
        {
            User? user = UserRepository.Login(loginRequest);
            if (user == null)
                throw new Exception("Mail veya şifre yanlış");
            return AuthRepository.GenerateToken(user);
        }

        public void UpdateShareSetting(int shareId, int userId)
        {
            UserRepository.Update(shareId, userId);
        }

        public void AddFriend(int userId2Add, int userId)
        {
           UserRepository.AddFriend(userId2Add, userId);
        }

        public void DeleteFriend(int userId2Delete, int userId)
        {
            UserRepository.DeleteFriend(userId2Delete, userId);
        }
    }

    public interface IUserUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IAuthRepository AuthRepository { get; }

        RegisterResponse Register(RegisterRequest registerRequest);
        string Login(LoginRequest loginRequest);
        void UpdateShareSetting(int shareId, int userId);
        void AddFriend(int userId2Add,int userId);
        void DeleteFriend(int userId2Delete, int userId);
    }
}
