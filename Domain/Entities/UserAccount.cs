using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Domain.Entities
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long AccountNumber { get; set; }
        public string FullName { get; set; }
        public decimal AccountBalance { get; set; }
        public bool Account_Type { get; set; }
        public int TotalLogin { get; set; }
        public bool IsLocked { get; set; }

        //public UserAccount(int Id, string UserName, string Email, int Password, long AccountNumber, string FullName, decimal AccountBalance, int TotalLogin, bool IsLocked)
        //{
        //    this.Id = Id;
        //    this.UserName = UserName;
        //    this.Email = Email;
        //    this.Password = Password;
        //    this.AccountNumber = AccountNumber;
        //    this.FullName = FullName;
        //    this.AccountBalance = AccountBalance;
        //    this.TotalLogin = TotalLogin;
        //    this.IsLocked = IsLocked;
        //}

        public UserAccount(int id, string fullName, int accountNumber, bool account_Type, string userName, string email, string password, decimal v1, bool v2)
        {
            Id = id;
            FullName = fullName;
            AccountNumber = accountNumber;
            Account_Type = account_Type;
            UserName = userName;
            Email = email;
            Password = password;
        }

        public UserAccount()
        {
        }
    }
}
