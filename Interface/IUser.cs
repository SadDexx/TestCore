using NETCORE.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCORE.Interface
{
    public interface IUser
    {
        List<User> Alls();
        User Get(int id);
        void Create(User user);
    }
    public class users : IUser
    {
        public List<User> Alls()
        {
            var us = new List<User>
            {
                new User { Id=1, Name="Tom", Age=35},
                new User { Id=2, Name="Alice", Age=29},
                new User { Id=3, Name="Sam", Age=32},
                new User { Id=4, Name="k", Age=30}
            };
            return us;
        }

        public void Create(User user)
        {
            new User { };
        }

        public User Get(int id)
        {
            return Alls().Single(p => p.Id == id);
        }
    }
}
