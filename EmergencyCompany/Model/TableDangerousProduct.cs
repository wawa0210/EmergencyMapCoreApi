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
    [Table("T_DangerousProduct")]
    public class TableDangerousProduct
    {
        public TableDangerousProduct()
        {
            Status = 1;
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 登记号
        /// </summary>
        [Column("Id")]
        [Key]
        [Description("")]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// 登记号
        /// </summary>
        [Column("RegesterId")]
        [Description("")]
        public string RegesterId
        {
            get;
            set;
        }

        /// <summary>
        /// 公司编号
        /// </summary>
        [Column("CompanyId")]
        [Description("")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 危险源商品明细
        /// </summary>
        [Column("ProductName")]
        [Description("")]
        public string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// 别名
        /// </summary>
        [Column("AliasName")]
        [Description("")]
        public string AliasName
        {
            get;
            set;
        }

        /// <summary>
        /// 属性
        /// </summary>
        [Column("ProductAttributes")]
        [Description("")]
        public string ProductAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// 成产能力
        /// </summary>
        [Column("Manufacturability")]
        [Description("")]
        public decimal Manufacturability
        {
            get;
            set;
        }

        /// <summary>
        /// 最大储能
        /// </summary>
        [Column("ProductReserve")]
        [Description("")]
        public decimal ProductReserve
        {
            get;
            set;
        }

        /// <summary>
        /// 年生产量
        /// </summary>
        [Column("YearProduct")]
        [Description("")]
        public decimal YearProduct
        {
            get;
            set;
        }

        /// <summary>
        /// cas
        /// </summary>
        [Column("Cas")]
        [Description("")]
        public string Cas
        {
            get;
            set;
        }

        /// <summary>
        /// un
        /// </summary>
        [Column("Un")]
        [Description("")]
        public string Un
        {
            get;
            set;
        }
        /// <summary>
        /// 是否剧毒
        /// </summary>
        [Column("IsToxicity")]
        [Description("")]
        public int IsToxicity
        {
            get;
            set;
        }

        /// <summary>
        /// 说明书
        /// </summary>
        [Column("Instructions")]
        [Description("")]
        public string Instructions
        {
            get;
            set;
        }

        /// <summary>
        /// 专家建议
        /// </summary>
        [Column("ExpertOpinion")]
        [Description("")]
        public string ExpertOpinion
        {
            get;
            set;
        }

        /// <summary>
        /// 管控预案
        /// </summary>
        [Column("ManagementPlan")]
        [Description("")]
        public string ManagementPlan
        {
            get;
            set;
        }


        /// <summary>
        /// 描述
        /// </summary>
        [Column("Memo")]
        [Description("")]
        public string Memo
        {
            get;
            set;
        }

        [Column("CreateTime")]
        [Description("")]
        public DateTime CreateTime
        {
            get;
            set;
        }


        [Column("Status")]
        [Description("")]
        public int Status
        {
            get;
            set;
        }
    }
}
