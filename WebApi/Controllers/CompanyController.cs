using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmergencyCompany.Application;
using EmergencyCompany.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("v0/companies")]
    public class CompanyController : BaseApiController
    {
        private ICompanyService ICompanyService { get; set; }

        private IDangerousProductService IDangerousProductService { get; set; }

        /// <summary>
        /// 初始化(autofac 已经注入)
        /// </summary>
        public CompanyController(ICompanyService _iCompanyService, IDangerousProductService _iDangerousProductService)
        {
            ICompanyService = _iCompanyService;
            IDangerousProductService = _iDangerousProductService;
        }

        /// <summary>
        /// 新增企业信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("")]
        public async Task<ResponseModel> AddCompanyInfo(EntityCompany entityCompany)
        {
            var result = ICompanyService.GetCompanyInfoByName(entityCompany.CompanyName);
            if (result.Result != null) return Fail(ErrorCodeEnum.CompanyAlreadyExist);
            await ICompanyService.InsertCompanyInfoSync(entityCompany);
            return Success("保存成功");
        }

        /// <summary>
        /// 根据区县企业风险等级信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("counties")]
        public async Task<ResponseModel> GetCompanyInfo()
        {
            var result = await ICompanyService.GetCountyRiskLevelInfo();
            return Success(result);
        }

        /// <summary>
        /// 编辑企业信息
        /// </summary>
        /// <returns></returns>
        [HttpPut, HttpOptions]
        [Route("")]
        public async Task<ResponseModel> UpdateCompanyInfo(EntityCompany entityCompany)
        {
            if (string.IsNullOrEmpty(entityCompany.Id)) return Fail(ErrorCodeEnum.ParamIsNullArgument);

            await ICompanyService.UpdateCompanyInfo(entityCompany);
            return Success("保存成功");
        }

        ///// <summary>
        ///// 获得所有企业信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet, HttpOptions]
        //[Route("")]
        //public async Task<ResponseModel> GetCompanyInfo()
        //{
        //    var result = await ICompanyService.GetAllCompanyInfo();
        //    return Success(result);
        //}

        /// <summary>
        /// 根据搜索获得企业信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("")]
        public async Task<ResponseModel> GetCompanyInfo([FromQuery] EntityCompanySearch entityCompany)
        {
            if (entityCompany == null) entityCompany = new EntityCompanySearch();

            var result = await ICompanyService.GetCompanyInfo(entityCompany);
            return Success(result);
        }

        /// <summary>
        /// 根据搜索获得企业信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("{id}")]
        public async Task<ResponseModel> GetCompanyInfo(string id)
        {
            if (string.IsNullOrEmpty(id)) return Fail(ErrorCodeEnum.ParamIsNullArgument);

            var result = await ICompanyService.GetCompanyInfo(id);
            return Success(result);
        }

        /// <summary>
        /// 删除企业信息
        /// </summary>
        /// <returns></returns>
        [HttpDelete, HttpOptions]
        [Route("{id}")]
        public async Task<ResponseModel> DeleteCompanyInfo(string id)
        {
            if (string.IsNullOrEmpty(id)) return Fail(ErrorCodeEnum.ParamIsNullArgument);

            await ICompanyService.DeleteCompanyInfo(id);
            await IDangerousProductService.DeleteDangerousProduct(id);

            return Success("删除成功");
        }

        /// <summary>
        /// 根据搜索获得企业信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpOptions]
        [Route("pageinfo")]
        public async Task<ResponseModel> GetPageCompanyInfo([FromQuery] EntityCompanyPageQuery companyPageQuery)
        {
            if (companyPageQuery == null) return Fail(ErrorCodeEnum.ParamIsNullArgument);

            var result = await ICompanyService.GetPageCompanyInfo(companyPageQuery);
            return Success(result);
        }
    }
}