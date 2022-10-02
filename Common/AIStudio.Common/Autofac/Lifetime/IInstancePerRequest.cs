using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Autofac.Lifetime
{
    /// <summary>
    /// 每次请求 InstancePerRequest：不同的请求获取的服务实例不一样；
    /// </summary>
    public interface IInstancePerRequest
    {
    }
}
