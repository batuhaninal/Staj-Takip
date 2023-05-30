using Autofac;
using StajTakip.Business.Abstract;
using StajTakip.Business.Concrete;
using StajTakip.DataAccess.Abstract;
using StajTakip.DataAccess.Concrete.EntityFramework.Repositories;

namespace StajTakip.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TempService>().As<ITempService>().InstancePerLifetimeScope();
            builder.RegisterType<TempRepository>().As<ITempRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TempRepositoryAsync>().As<ITempRepositoryAsync>().InstancePerLifetimeScope();

            builder.RegisterType<InternshipsBookManager>().As<IInternshipsBookService>().InstancePerLifetimeScope();
            builder.RegisterType<EFInternshipsBookRepository>().As<IInternshipsBookRepository>().InstancePerLifetimeScope();

            builder.RegisterType<BookTemplateManager>().As<IBookTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<EFBookTemplate>().As<IBookTemplateRepository>().InstancePerLifetimeScope();

            builder.RegisterType<InternshipDocumentManager>().As<IInternshipDocumentService>().InstancePerLifetimeScope();
            builder.RegisterType<EfInternshipDocumentRepository>().As<IInternshipDocumentRepository>().InstancePerLifetimeScope();

            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();


            builder.RegisterType<MailManager>().As<IMailService>().InstancePerLifetimeScope();
        }
    }
}
