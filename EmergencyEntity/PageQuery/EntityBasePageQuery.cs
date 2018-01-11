using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyEntity.PageQuery
{
    /// <summary>
    /// 分页基本查询参数
    /// </summary>
    public class EntityBasePageQuery
    {
        public EntityBasePageQuery()
        {
            CurrentPage = 1;
            PageSize = 20;
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize { get; set; }
    }
}
