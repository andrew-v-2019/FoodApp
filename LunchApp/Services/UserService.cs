using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Models;
using Services.Interfaces;
using ViewModels.User;

namespace Services
{
    public class UserService:IUserService
    {
        private readonly Context _context;
        public UserService(Context context)
        {
            _context = context;
        }


        public UserViewModel GetByCompName(string compName, string ip)
        {
            var user = _context.Users.FirstOrDefault(u => u.CompName == compName) ?? new User()
            {
                Id = 0,
                Name = string.Empty,
                CompName = compName,
                Ip = ip
            };
            var viewModel = new UserViewModel()
            {
                Id = user.Id,
                CompName = user.CompName,
                Ip = user.Ip,
                Name = user.Name
            };
            return viewModel;
        }

        public UserViewModel UpdateUser(UserViewModel model)
        {
            var user = new User()
            {
                Id = model.Id,
                Name = model.Name,
                CompName = model.CompName,
                Ip = model.Ip
            };
            var updatedUser = AddOrUpdate(user);
            model.Id = updatedUser.Id;
            return model;
        }

        private User AddOrUpdate(User user)
        {
            var domain = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (domain == null)
            {
                domain = _context.Users.Add(user).Entity;
            }
            else
            {
                domain.Name = user.Name;
            }
            _context.SaveChanges();
            return domain;
        }
    }
}
