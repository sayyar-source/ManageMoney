using ManageMoney.Application.Services;
using ManageMoney.Data.Dtos;
using ManageMoney.Data.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageMoney.UnitTest
{
    public class AccountServiceFacke : IAccountService
    {
        private readonly List<Account> _accountLst;
        public AccountServiceFacke()
        {
            _accountLst = new List<Account>()
            {
               
                new Account() { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                    Name = "EUR",AccountType=AccountType.EUR,UserId=new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c201")},

                new Account() { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c300"),
                    Name = "USD",AccountType=AccountType.USD,UserId=new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c301")},

                new Account() { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c400"),
                    Name = "TR",AccountType=AccountType.TR,UserId=new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c401")},
            };

        }

        public async Task<Account> AddAccountAsync(AccountDto accountDto)
        {
            accountDto.UserId = new Guid();
            var account = accountDto.Adapt<Account>();
            _accountLst.Add(account);
            return account;
        }

        public async Task DeleteAccountAsync(Guid id)
        {
            var existing = _accountLst.First(a => a.Id == id);
            _accountLst.Remove(existing);
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return _accountLst.Where(a => a.Id == id)
               .FirstOrDefault();
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return _accountLst;
        }

        public async Task UpdateAccountAsync(AccountUpdateDto accountUpdate)
        {
            var existing = _accountLst.First(a => a.Id == accountUpdate.Id);
            existing.Name= accountUpdate.Name;
            existing.AccountType= accountUpdate.AccountType;
        }
    }
}
