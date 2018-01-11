using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.FrameWork
{
    /// <summary>
    /// automapper 映射
    /// </summary>
    public class MapperInit
    {
        public static void InitMapping()
        {
            Mapper.Initialize(cfg =>
            {
                //cfg.CreateMap<TableMuseum, EntityMuseum>()
                //   .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                //   .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                //   .ForMember(x => x.IsEnable, y => y.MapFrom(z => z.IsEnable))
                //   .ForMember(x => x.CreateTime, y => y.MapFrom(z => z.CreateTime))
                //   .ForMember(x => x.Remark, y => y.MapFrom(z => z.Remark))
                //   .ForAllOtherMembers(x => x.Ignore());
            }
            );
        }
    }
}
