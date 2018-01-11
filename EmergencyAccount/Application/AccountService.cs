using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommonLib;
using EmergencyAccount.Entity;
using EmergencyAccount.Model;
using EmergencyBaseService;

namespace EmergencyAccount.Application
{
    public class AccountService : BaseAppService,IAccountService
    {
        public EntityAccountManager GetAccountManager(string userName)
        {
            var accountRep = GetRepositoryInstance<TableAccountManager>();
            var restult = accountRep.Find(x => x.UserName == userName);
            return Mapper.Map<TableAccountManager, EntityAccountManager>(restult);
        }

        public async Task<EntityAccountManager> GetAccountManagerInfo(string useId)
        {
            throw new NotImplementedException();
        }

        public bool CheckLoginInfo(string inputPwd, string salt, string dbPwd)
        {
            var userPwd = DESEncrypt.Encrypt(inputPwd, salt);
            return userPwd == dbPwd;
        }
    }
}
