using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EmergencyAccount.Entity
{
    public class EntityAccountManager
    {
        public string Id { get; set; }
        public int RoleId { get; set; }
        public int DeptId { get; set; }
        public string UserName { get; set; }

        [JsonIgnore]
        public string UserPwd
        {
            get; set;
        }
        [JsonIgnore]

        public string UserSalt { get; set; }
        public string RealName { get; set; }
        public string Tel { get; set; }

        [JsonIgnore]

        public int IsLock { get; set; }
        public int Level { get; set; }
        public DateTime AddTime { get; set; }
    }

    public class EntityAccountNewManager
    {

        public EntityAccountNewManager()
        {
            Level = 2;
        }

        [Required(ErrorMessage = "登录名不能为空")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]

        public string UserPwd
        {
            get; set;
        }

        [Required(ErrorMessage = "真实名称不能为空")]

        public string RealName { get; set; }

        [Required(ErrorMessage = "电话号码不能为空")]

        public string Tel { get; set; }


        public int Level { get; set; }
    }

    public class EntityAccountDelete
    {
        [Required(ErrorMessage = "账户编号不能为空")]
        public string Id { get; set; }
    }
}
