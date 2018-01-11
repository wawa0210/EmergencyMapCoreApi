using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Entity
{
    public class EntityCompany
    {
        public EntityCompany()
        {
            AddressDetail = "";
            CompanyDetail = "";
            ZipCode = "";
            FoundedTime = DateTime.Now;
            IssureTime = DateTime.Now;
            IndustryCode = "";
            Owner = "";
            CompanyScale = "";
            CompanyIncome = 0M;
            CompanyProductDetail = "";
            CreateTime = DateTime.Now;
            Memo = "";
            Status = 1;
        }

        /// <summary>
        /// 企业编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 公司名称 
        /// </summary>
        public string CompanyName
        {
            get; set;
        }

        /// <summary>
        /// 省 
        /// </summary>
        [Required(ErrorMessage = "省不能为空")]
        public string Provice
        {
            get; set;
        }
        /// <summary>
        /// 省编号
        /// </summary>
        [Required(ErrorMessage = "省编号不能为空")]
        public string ProvCode
        {
            get; set;
        }
        /// <summary>
        /// 市 
        /// </summary>
        [Required(ErrorMessage = "市 不能为空")]
        public string City
        {
            get; set;
        }
        /// <summary>
        /// 市 编号
        /// </summary>
        [Required(ErrorMessage = "市编号不能为空")]
        public string CityCode
        {
            get; set;
        }
        /// <summary>
        /// 县区
        /// </summary>
        [Required(ErrorMessage = "区县不能为空")]
        public string County
        {
            get; set;
        }
        /// <summary>
        /// 县区编号
        /// </summary>
        [Required(ErrorMessage = "区县编号不能为空")]
        public string CountyCode
        {
            get; set;
        }
        /// <summary>
        /// 详细地址
        /// </summary>
        [Required(ErrorMessage = "详细地址不能为空")]
        public string AddressDetail
        {
            get; set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [Required(ErrorMessage = "经度不能为空")]
        public string Longitude
        {
            get; set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [Required(ErrorMessage = "纬度不能为空")]
        public string Latitude
        {
            get; set;
        }

        /// <summary>
        /// 行业
        /// </summary>
        public string Industry
        {
            get; set;
        }

        /// <summary>
        /// 经济类型
        /// </summary>
        public string Economy
        {
            get; set;
        }

        /// <summary>
        /// 公司详细描述
        /// </summary>

        public string CompanyDetail
        {
            get; set;
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode
        {
            get; set;
        }

        /// <summary>
        /// 成立时间
        /// </summary>
        public DateTime? FoundedTime
        {
            get; set;
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? IssureTime
        {
            get; set;
        }


        /// <summary>
        /// 行业代码
        /// </summary>
        public string IndustryCode
        {
            get; set;
        }

        /// <summary>
        /// 法人名称
        /// </summary>
        public string Owner
        {
            get; set;
        }

        /// <summary>
        /// 企业规模
        /// </summary>
        public string CompanyScale
        {
            get; set;
        }

        /// <summary>
        /// 企业营业收入
        /// </summary>
        public decimal CompanyIncome
        {
            get; set;
        }

        /// <summary>
        /// 分管安全负责人
        /// </summary>
        public string ChiefSafeyName
        {
            get; set;
        }

        /// <summary>
        /// 分管安全负责人电话
        /// </summary>
        public string ChiefSafeyPhone
        {
            get; set;
        }

        /// <summary>
        /// 安全生产管理机构负责人
        /// </summary>
        public string ViceSafeyName
        {
            get; set;
        }

        /// <summary>
        /// 安全生产管理机构负责人电话
        /// </summary>
        public string ViceSafeyPhone
        {
            get; set;
        }

        /// <summary>
        /// 安全值班电话
        /// </summary>
        public string OnDutyPhone
        {
            get; set;
        }

        /// <summary>
        /// 应急资讯电话
        /// </summary>
        public string EmergencyPhone
        {
            get; set;
        }

        /// <summary>
        /// 主要产品及生产规模
        /// </summary>
        public string CompanyProductDetail
        {
            get; set;
        }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime CreateTime
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        {
            get; set;
        }


        /// <summary>
        /// 当前状态
        /// </summary>
        public int? Status
        {
            get; set;
        }

        /// <summary>
        /// 风险级别 1，2，3，4  默认为1
        /// </summary>
        public int RiskLevel
        {
            get; set;
        }
    }
}
