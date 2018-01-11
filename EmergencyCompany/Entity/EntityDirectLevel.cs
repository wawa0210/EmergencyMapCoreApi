using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyCompany.Entity
{
    public class EntityDirectLevel
    {
        /// <summary>
        /// 区县编号
        /// </summary>
        public string CountyCode { get; set; }

        /// <summary>
        /// 最低安全级别
        /// </summary>
        public int MinRiskLevel { get; set; }
    }
}
