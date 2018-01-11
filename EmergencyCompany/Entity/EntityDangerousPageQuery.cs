using EmergencyEntity.PageQuery;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Entity
{
    public class EntityDangerousPageQuery:EntityBasePageQuery
    {
        /// <summary>
        /// 企业编号
        /// </summary>
        [Required(ErrorMessage ="企业编号不能为空")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 危险源名称
        /// </summary>
        public string ProductName { get; set; }
    }
}
