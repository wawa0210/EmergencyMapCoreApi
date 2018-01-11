using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{

    public enum ErrorCodeEnum : int
    {
        #region base

        [Description("成功")]
        Success = 0,

        [Description("请求错误")]
        RequestError = 400,

        [Description("无接口操作权限")]
        Forbidden = 403,

        [Description("未找到对应的资源")]
        NotFound = 404,

        [Description("服务器错误")]
        ServerError = 500,

        [Description("参数校验失败")]
        ParamsInvalid = 9008,

        #endregion

        #region 全局公共Code（参数为空，时间参数不合法等） 8000~8999

        [Description("参数为空")]
        ParamIsNullArgument = 8000,

        [Description("单页数据最大不能超过100条")]
        MoreThanMaxSize = 8004,

        [Description("api接口请求验证不通过")]
        ApiRequestForbidden = 8006,

        [Description("Token过期，查询不到用户信息")]
        TokenIsExpired = 8005,

        [Description("登录信息不存在")]
        UserIsNull = 8006,

        [Description("用户密码不正确")]
        UserPwdCheckFaild = 8007,
        #endregion

        [Description("文物分类不正确(必须指定父级分类)")]
        AntiquesClassError = 8008,
        [Description("该大分类下已存在同名文物")]
        AntiquesRepeat = 8009,
        [Description("标识不能为空")]
        IdentifyIsNull = 8010
    }
}
