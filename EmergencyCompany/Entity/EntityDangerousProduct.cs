using CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Entity
{
    public class EntityDangerousProduct
    {
        public EntityDangerousProduct()
        {
            AliasName = "";
            Status = 1;
            Memo = "";
            CreateTime = DateTime.Now;
            ProductAttributes = "";
            Manufacturability = 0M;
            ProductReserve = 0M;
            YearProduct = 0M;
            Cas = "";
            Un = "";
            Instructions = "";
            ExpertOpinion = "";
            ManagementPlan = "";
            RegesterId = Utils.GetNewId();
        }

        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 登记号
        /// </summary>
        public string RegesterId
        {
            get;
            set;
        }

        /// <summary>
        /// 公司编号
        /// </summary>
        [Required(ErrorMessage = "企业编号不能为空")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 危险源商品名称
        /// </summary>
        [Required(ErrorMessage = "危险源商品名称不能为空")]
        public string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string AliasName
        {
            get;
            set;
        }

        /// <summary>
        /// 属性
        /// </summary>
        public string ProductAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// 成产能力
        /// </summary>
        public decimal Manufacturability
        {
            get;
            set;
        }

        /// <summary>
        /// 最大储能
        /// </summary>
        public decimal ProductReserve
        {
            get;
            set;
        }

        /// <summary>
        /// 年生产量
        /// </summary>
        public decimal YearProduct
        {
            get;
            set;
        }

        /// <summary>
        /// cas
        /// </summary>
        public string Cas
        {
            get;
            set;
        }

        /// <summary>
        /// un
        /// </summary>
        public string Un
        {
            get;
            set;
        }

        public int IsToxicity
        {
            get;
            set;
        }

        /// <summary>
        /// 说明书
        /// </summary>
        public string Instructions
        {
            get;
            set;
        }

        public string ExpertOpinion
        {
            get;
            set;
        }

        public string ManagementPlan
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>

        public string Memo
        {
            get;
            set;
        }


        public DateTime CreateTime
        {
            get;
            set;
        }



        public int Status
        {
            get;
            set;
        }

    }
}
