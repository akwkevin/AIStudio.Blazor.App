using AIStudio.Common.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yitter.IdGenerator;

namespace AIStudio.Common.IdGenerator
{
    public static class IdHelper
    {
        public static void SetWorkId(ushort workerId)
        {          
            // 设置雪花id的workerId，确保每个实例workerId都应不同
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions { WorkerId = workerId });
        }

        public static string GetId()
        {
            return YitIdHelper.NextId().ToString();
        }

        public static long GetlongId()
        {
            return YitIdHelper.NextId();
        }
    }
}
