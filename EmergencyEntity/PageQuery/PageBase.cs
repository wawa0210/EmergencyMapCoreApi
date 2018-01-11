using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyEntity.PageQuery
{
    /// <summary>
    /// 分页信息类
    /// </summary>
    public class PageBase<T> where T : new()
    {
        /// <summary>
        /// 当前页索引
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 数据总条数
        /// </summary>
        public int TotalCounts { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 详细数据
        /// </summary>
        public List<T> Items { get; set; }
    }
}
