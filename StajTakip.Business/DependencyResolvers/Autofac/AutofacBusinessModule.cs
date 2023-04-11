using Autofac;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TempService>().As<ITempService>().InstancePerLifetimeScope();
            builder.RegisterType<TempRepository>().As<ITempRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TempRepositoryAsync>().As<ITempRepositoryAsync>().InstancePerLifetimeScope();
        }
    }
}
