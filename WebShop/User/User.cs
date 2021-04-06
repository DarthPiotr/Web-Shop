using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop
{
    public class User
    {
    
        public int Id { set; get; }
        public string Name { set; get; }
        public string Lastname { set; get; }
        public string Username { set; get; }
        public string Email { set; get; }
        public string AccountType { set; get; }
        public bool IsAdmin { get { return AccountType == "admin"; } }
        public bool Active { set; get; }

        public User(int id, string name, string lastname, string username, string email, string type, bool active)
        {
            Id = id;
            Name = name;
            Lastname = lastname;
            Username = username;
            Email = email;
            AccountType = type;
            Active = active;
        }
    }
}