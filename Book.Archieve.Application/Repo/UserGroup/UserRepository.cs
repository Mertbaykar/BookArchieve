using Book.Archieve.Domain.DTO.Request.User;
using Book.Archieve.Domain.Entity;
using Book.Archieve.Infrastructure.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Application.Repo.UserGroup
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly IValidator<User> validator;

        public UserRepository(BookContext bookContext, IValidator<User> validator) : base(bookContext)
        {
            this.validator = validator;
        }

        private void ValidateByDB(User user)
        {
            if (MasterDataDb.User.Any(u => u.Email == user.Email || u.FirstName.ToLower() == user.FirstName.ToLower() && u.LastName.ToLower() == user.LastName.ToLower()))
                throw new Exception("Kullanıcı zaten tanımlanmış");
        }

        public User Register(RegisterRequest registerRequest)
        {
            User user = User.Create(registerRequest);
            base.Validate(user, validator);
            ValidateByDB(user);
            return base.Add(user);
        }

        public User? Login(LoginRequest loginRequest)
        {
            return MasterDataDb.User.Where(u => u.IsActive && u.Email == loginRequest.Email && u.Password == loginRequest.Password).FirstOrDefault();
        }

        public void Update(int shareId, int userId)
        {
            var user = MasterDataDb.User.Where(x => x.Id == userId && x.IsActive).FirstOrDefault();
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");
            user.UpdateShareSetting(shareId);
            base.Save();
        }

        public void AddFriend(int userId2Add, int userId)
        {
            MasterDataDb.FriendShip.Add(new UserFriend
            {
                FriendId = userId2Add,
                UserId = userId,
            });
            base.Save();
        }

        public void DeleteFriend(int userId2Delete, int userId)
        {
            // arkadaşlık mutualist bir ilişki
            MasterDataDb.FriendShip
                .Where(x => (x.UserId == userId && x.FriendId == userId2Delete) || (x.UserId == userId2Delete && x.FriendId == userId))
                .ExecuteDelete();
            base.Save();
        }
    }

    public interface IUserRepository
    {
        User Register(RegisterRequest registerRequest);
        User? Login(LoginRequest loginRequest);
        void Update(int shareId, int userId);
        void AddFriend(int userId2Add, int userId);
        void DeleteFriend(int userId2Delete, int userId);
    }
}
