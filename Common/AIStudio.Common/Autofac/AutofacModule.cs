using Autofac;

namespace AIStudio.Common.Autofac
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //把服务的注入规则写在这里
            //builder.RegisterType<ValuesService>().As<IValuesService>();
            builder.RegisterType<AutofacAOP>();
            //builder.RegisterType<ValuesService>().As<IValuesService>().EnableInterfaceInterceptors();

            ////程序集批量注入
            //System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("AutofacTest");
            //// System.Reflection.Assembly assembly =  System.Reflection.Assembly.GetExecutingAssembly();
            //builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().InstancePerDependency();
        }
    }
}
