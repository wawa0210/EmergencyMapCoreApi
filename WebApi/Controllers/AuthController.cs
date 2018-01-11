using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLib;
using EmergencyAccount.Application;
using EmergencyAccount.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("v0/auth")]
    public class AuthController: BaseApiController
    {
        private IAccountService IAccountService { get; set; }

        /// <summary>
        /// 初始化(autofac 已经注入)
        /// </summary>
        public AuthController(IAccountService _iAccountService)
        {
            IAccountService = _iAccountService;
        }

        /// <summary>
        /// 获得token 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [AllowAnonymous]
        [Route("token")]
        public ResponseModel GetAccountAuth([FromBody]EntityLoginModel loginModel)
        {
            var result = IAccountService.GetAccountManager(loginModel.UserName);

            if (result == null) return Fail(ErrorCodeEnum.UserIsNull);

            var checkResult = IAccountService.CheckLoginInfo(loginModel.UserPwd, result.UserSalt, result.UserPwd);
            if (!checkResult) return Fail(ErrorCodeEnum.UserPwdCheckFaild);

            return Success(new
            {
                token = AesHelper.Encrypt(JsonConvert.SerializeObject(result)),
                userInfo = result
            });
        }
    }
}