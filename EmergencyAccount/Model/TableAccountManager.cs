using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmergencyAccount.Model
{

    [Table("T_Sys_Manager")]
    public class TableAccountManager
    {
        #region 私有成员
        //在这里设置字段的默认值
        private string _Id;
        private int _RoleId;
        private int _DeptId;
        private string _UserName = string.Empty;
        private string _UserPwd = string.Empty;
        private string _UserSalt = string.Empty;
        private string _RealName = string.Empty;
        private string _Tel = string.Empty;
        private int _IsLock;
        private int _Level;
        private DateTime? _AddTime;
        #endregion

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TableAccountManager()
        {
        }
        /// <summary>
        /// 全参数构造函数
        /// </summary>
        public TableAccountManager(string Id, int RoleId, int DeptId, string UserName, string UserPwd, string UserSalt, string RealName, string Tel, int IsLock, int Level, DateTime? AddTime)
        {
            _Id = Id;
            _RoleId = RoleId;
            _DeptId = DeptId;
            _UserName = UserName;
            _UserPwd = UserPwd;
            _UserSalt = UserSalt;
            _RealName = RealName;
            _Tel = Tel;
            _IsLock = IsLock;
            _Level = Level;
            _AddTime = AddTime;

        }
        #endregion

        #region 属性

        /// <summary>
        /// 属性: 
        /// </summary>
        [Column("Id")]
        [Key]
        [Description("")]
        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }

        /// <summary>
        /// 属性: 角色编号
        /// </summary>
        [Column("RoleId")]
        [Description("角色编号")]
        public int RoleId
        {
            get
            {
                return _RoleId;
            }
            set
            {
                _RoleId = value;
            }
        }

        /// <summary>
        /// 属性: 部门编号
        /// </summary>
        [Column("DeptId")]
        [Description("部门编号")]
        public int DeptId
        {
            get
            {
                return _DeptId;
            }
            set
            {
                _DeptId = value;
            }
        }

        /// <summary>
        /// 属性: 用户名
        /// </summary>
        [Column("UserName")]
        [Description("用户名")]
        public string UserName
        {
            get
            {
                return _UserName == null ? string.Empty : _UserName.Trim();
            }
            set
            {
                _UserName = value;
            }
        }

        /// <summary>
        /// 属性: 用户密码
        /// </summary>
        [Column("UserPwd")]
        [Description("用户密码")]
        public string UserPwd
        {
            get
            {
                return _UserPwd == null ? string.Empty : _UserPwd.Trim();
            }
            set
            {
                _UserPwd = value;
            }
        }

        /// <summary>
        /// 属性: 密码盐
        /// </summary>
        [Column("UserSalt")]
        [Description("密码盐")]
        public string UserSalt
        {
            get
            {
                return _UserSalt == null ? string.Empty : _UserSalt.Trim();
            }
            set
            {
                _UserSalt = value;
            }
        }

        /// <summary>
        /// 属性: 真实姓名
        /// </summary>
        [Column("RealName")]
        [Description("真实姓名")]
        public string RealName
        {
            get
            {
                return _RealName == null ? string.Empty : _RealName.Trim();
            }
            set
            {
                _RealName = value;
            }
        }

        /// <summary>
        /// 属性: 电话号码
        /// </summary>
        [Column("Tel")]
        [Description("电话号码")]
        public string Tel
        {
            get
            {
                return _Tel == null ? string.Empty : _Tel.Trim();
            }
            set
            {
                _Tel = value;
            }
        }

        /// <summary>
        /// 属性: 是否可用
        /// </summary>
        [Column("IsLock")]
        [Description("是否可用")]
        public int IsLock
        {
            get
            {
                return _IsLock;
            }
            set
            {
                _IsLock = value;
            }
        }

        /// <summary>
        /// 属性: 是否可用
        /// </summary>
        [Column("Level")]
        [Description("是否可用")]
        public int Level
        {
            get
            {
                return _Level;
            }
            set
            {
                _Level = value;
            }
        }

        /// <summary>
        /// 属性: 添加时间
        /// </summary>
        [Column("AddTime")]
        [Description("添加时间")]
        public DateTime? AddTime
        {
            get
            {
                return _AddTime;
            }
            set
            {
                _AddTime = value;
            }
        }
        #endregion
    }
}
