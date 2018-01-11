using EmergencyEntity.PageQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Entity
{
    public class EntityCompanyPageQuery : EntityBasePageQuery
    {
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
    }
}
