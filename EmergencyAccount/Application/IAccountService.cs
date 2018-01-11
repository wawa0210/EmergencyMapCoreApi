using EmergencyAccount.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyAccount.Application
{
    public interface IAccountService
    {

        EntityAccountManager GetAccountManager(string userName);

        Task<EntityAccountManager> GetAccountManagerInfo(string useId);

        /// <summary>
        /// 校验登录密码是否正确
        /// </summary>
        /// <param name="inputPwd"></param>
        /// <param name="salt"></param>
        /// <param name="dbPwd"></param>
        /// <returns></returns>
        bool CheckLoginInfo(string inputPwd, string salt, string dbPwd);
    }
}
