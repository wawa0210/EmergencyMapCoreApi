using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Entity
{
    public class EntityCompanySearch
    {

        /// <summary>
        /// 区域编号
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }


        /// <summary>
        /// 风险等级
        /// </summary>
        public int RiskLevel { get; set; }
    }
}
