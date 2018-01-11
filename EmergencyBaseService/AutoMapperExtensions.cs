using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmergencyBaseService
{
    public static class AutoMapperExtensions
    {
        public static T MapToObject<T, U>(this U model, T target)
        {
            return Mapper.Map<U, T>(model);
        }
    }
}
