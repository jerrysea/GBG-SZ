using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;


namespace Instinct.CAP.FraudCheckService.Util
{
    public class AutofacUtil
    {
        /// <summary>
        /// Autofac容器对象
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// 初始化autofac
        /// </summary>
        public static void InitAutofac()
        {
            var builder = new ContainerBuilder();           

            // 注册autofac打标签模式
            //builder.RegisterModule(new AutofacAnnotationModule(typeof(AnotationTest).Assembly));
            //如果需要开启支持循环注入
            //builder.RegisterModule(new AutofacAnnotationModule(typeof(AnotationTest).Assembly).SetAllowCircularDependencies(true));

            ////配置Dapper接口依赖
            //builder.RegisterGeneric(typeof(DapperRepository<>)).As(typeof(IDPRepository<>));

            ////配置EntityFrameworkCore接口依赖
            //builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IEFRepository<>));

            ////注入service
            //builder.RegisterAssemblyTypes(Assembly.Load("Zhang.Contract"), Assembly.Load("Zhang.Application"))
            //       .Where(x => x.Name.EndsWith("Service"))
            //       .AsImplementedInterfaces();
            ////注入repository
            //builder.RegisterAssemblyTypes(Assembly.Load("Zhang.Core"), Assembly.Load("Zhang.Dapper"))
            //       .Where(x => x.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces();

            _container = builder.Build();
            
        }

        /// <summary>
        /// 从Autofac容器获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetFromFac<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
