using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLib;
using EmergencyAccount.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class BaseApiController : Controller
    {

        /// <summary>
        /// 获取登陆用户的相关信息
        /// </summary>
        protected EntityAccountManager CurrentUser
        {
            get { return GetUserContext(); }
        }

        /// <summary>
        ///     获取登陆用户的相关信息
        /// </summary>
        /// <returns></returns>
        protected EntityAccountManager GetUserContext()
        {

            return new EntityAccountManager();

            //if (!Request.Properties.ContainsKey(Constants.GlobalUserContextKeyName))
            //{
            //    return null;
            //}

            //return Request.Properties[Constants.GlobalUserContextKeyName] as EntityAccountManager;
        }

        /// <summary>
        ///     Model参数校验  (校验不通过直接返回错误model)
        /// </summary>
        /// <param name="checkResult"></param>
        /// <returns></returns>
        protected bool CheckModelParams(out ResponseModel checkResult)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(s => s.Errors).First();
                if (error != null)
                {
                    checkResult = new ResponseModel
                    {
                        Code = (int)ErrorCodeEnum.ParamsInvalid,
                        Message = error.ErrorMessage,
                        Data = null
                    };
                    return true;
                }
            }
            checkResult = null;
            return false;
        }


        /// <summary>
        ///     返回成功结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected ResponseModel Success(object data)
        {
            if (data != null && data is ResponseModel)
            {
                var result = (ResponseModel)data;
                result.Code = result.Code;
                result.Message = result.Message;
                return result;
            }

            return new ResponseModel
            {
                Code = (int)ErrorCodeEnum.Success,
                Message = "success",
                Data = data
            };
        }
        /// <summary>
        ///     返回失败结果
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        protected ResponseModel Fail(ErrorCodeEnum errorCode)
        {
            //获得枚举值对应错误信息
            var errorDescription = EnumExtensionHelper.ToEnumDescriptionString((int)errorCode, typeof(ErrorCodeEnum));
            if (string.IsNullOrEmpty(errorDescription))
            {
                errorDescription = "未知错误";
            }
            return new ResponseModel
            {
                Code = (int)errorCode,
                Message = errorDescription
            };
        }
        /// <summary>
        ///     校验请求参数是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestParam"></param>
        /// <param name="checkResult"></param>
        /// <returns></returns>
        public bool CheckRequestParamIsNull<T>(T requestParam, out ResponseModel checkResult)
        {
            checkResult = null;
            if (requestParam != null) return true;
            checkResult = Fail(ErrorCodeEnum.ParamIsNullArgument);
            return false;
        }
    }
}