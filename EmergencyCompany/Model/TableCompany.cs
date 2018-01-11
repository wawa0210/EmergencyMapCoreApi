using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Model
{
    [Table("T_Company")]
    public class TableCompany
    {

        public TableCompany()
        {
            CompanyName = "";
            Provice = "";
            ProvCode = "";
            City = "";
            CityCode = "";
            County = "";
            CountyCode = "";
            AddressDetail = "";
            Longitude = "";
            Latitude = "";
            Industry = "";
            Economy = "";
            CompanyDetail = "";
            ZipCode = "";
            FoundedTime = DateTime.Now;
            IssureTime = DateTime.Now;
            IndustryCode = "";
            Owner = "";
            CompanyScale = "";
            CompanyIncome = 0M;
            ChiefSafeyName = "";
            ChiefSafeyPhone = "";
            ViceSafeyName = "";
            ViceSafeyPhone = "";
            OnDutyPhone = "";
            EmergencyPhone = "";
            CompanyProductDetail = "";
            CreateTime = DateTime.Now;
            Memo = "";
            RiskLevel = 3;
        }

        /// <summary>
        /// 属性: 
        /// </summary>
        [Column("Id")]
        [Key]
        [Description("")]
        public string Id
        {
            get; set;
        }

        /// <summary>
        /// 公司名称 
        /// </summary>
        [Column("CompanyName")]
        [Description("")]
        public string CompanyName
        {
            get; set;
        }

        /// <summary>
        /// 省 
        /// </summary>
        [Column("Provice")]
        [Description("")]
        public string Provice
        {
            get; set;
        }
        /// <summary>
        /// 省编号
        /// </summary>
        [Column("ProvCode")]
        [Description("")]
        public string ProvCode
        {
            get; set;
        }
        /// <summary>
        /// 市 
        /// </summary>
        [Column("City")]
        [Description("")]
        public string City
        {
            get; set;
        }
        /// <summary>
        /// 市 编号
        /// </summary>
        [Column("CityCode")]
        [Description("")]
        public string CityCode
        {
            get; set;
        }
        /// <summary>
        /// 县区
        /// </summary>
        [Column("County")]
        [Description("")]
        public string County
        {
            get; set;
        }
        /// <summary>
        /// 县区编号
        /// </summary>
        [Column("CountyCode")]
        [Description("")]
        public string CountyCode
        {
            get; set;
        }
        /// <summary>
        /// 详细地址
        /// </summary>
        [Column("AddressDetail")]
        [Description("")]
        public string AddressDetail
        {
            get; set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [Column("Longitude")]
        [Description("")]
        public string Longitude
        {
            get; set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [Column("Latitude")]
        [Description("")]
        public string Latitude
        {
            get; set;
        }

        /// <summary>
        /// 行业
        /// </summary>
        [Column("Industry")]
        [Description("")]
        public string Industry
        {
            get; set;
        }

        /// <summary>
        /// 经济类型
        /// </summary>
        [Column("Economy")]
        [Description("")]
        public string Economy
        {
            get; set;
        }

        /// <summary>
        /// 公司详细描述
        /// </summary>
        [Column("CompanyDetail")]
        [Description("")]
        public string CompanyDetail
        {
            get; set;
        }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [Column("ZipCode")]
        [Description("")]
        public string ZipCode
        {
            get; set;
        }

        /// <summary>
        /// 成立时间
        /// </summary>
        [Column("FoundedTime")]
        [Description("")]
        public DateTime FoundedTime
        {
            get; set;
        }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Column("IssureTime")]
        [Description("")]
        public DateTime IssureTime
        {
            get; set;
        }


        /// <summary>
        /// 行业代码
        /// </summary>
        [Column("IndustryCode")]
        [Description("")]
        public string IndustryCode
        {
            get; set;
        }

        /// <summary>
        /// 法人名称
        /// </summary>
        [Column("Owner")]
        [Description("")]
        public string Owner
        {
            get; set;
        }

        /// <summary>
        /// 企业规模
        /// </summary>
        [Column("CompanyScale")]
        [Description("")]
        public string CompanyScale
        {
            get; set;
        }

        /// <summary>
        /// 企业营业收入
        /// </summary>
        [Column("CompanyIncome")]
        [Description("")]
        public decimal CompanyIncome
        {
            get; set;
        }

        /// <summary>
        /// 分管安全负责人
        /// </summary>
        [Column("ChiefSafeyName")]
        [Description("")]
        public string ChiefSafeyName
        {
            get; set;
        }

        /// <summary>
        /// 分管安全负责人电话
        /// </summary>
        [Column("ChiefSafeyPhone")]
        [Description("")]
        public string ChiefSafeyPhone
        {
            get; set;
        }

        /// <summary>
        /// 安全生产管理机构负责人
        /// </summary>
        [Column("ViceSafeyName")]
        [Description("")]
        public string ViceSafeyName
        {
            get; set;
        }

        /// <summary>
        /// 安全生产管理机构负责人电话
        /// </summary>
        [Column("ViceSafeyPhone")]
        [Description("")]
        public string ViceSafeyPhone
        {
            get; set;
        }

        /// <summary>
        /// 安全值班电话
        /// </summary>
        [Column("OnDutyPhone")]
        [Description("")]
        public string OnDutyPhone
        {
            get; set;
        }

        /// <summary>
        /// 应急资讯电话
        /// </summary>
        [Column("EmergencyPhone")]
        [Description("")]
        public string EmergencyPhone
        {
            get; set;
        }

        /// <summary>
        /// 主要产品及生产规模
        /// </summary>
        [Column("CompanyProductDetail")]
        [Description("")]
        public string CompanyProductDetail
        {
            get; set;
        }

        /// <summary>
        /// 插入时间
        /// </summary>
        [Column("CreateTime")]
        [Description("")]
        public DateTime CreateTime
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Memo")]
        [Description("")]
        public string Memo
        {
            get; set;
        }


        /// <summary>
        /// 当前状态
        /// </summary>
        [Column("Status")]
        [Description("")]
        public int? Status
        {
            get; set;
        }

        /// <summary>
        /// 风险级别 1，2，3 默认为3
        /// </summary>
        [Column("RiskLevel")]
        [Description("")]
        public int RiskLevel
        {
            get; set;
        }
    }
}
