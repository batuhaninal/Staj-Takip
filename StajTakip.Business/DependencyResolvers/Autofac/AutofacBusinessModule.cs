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
            builder.RegisterType<UserManager>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<EFUserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

            builder.RegisterType<StudentUserManager>().As<IStudentUserService>().InstancePerLifetimeScope();
            builder.RegisterType<EFStudentUserRepository>().As<IStudentUserRepository>().InstancePerLifetimeScope();

            builder.RegisterType<AdminUserManager>().As<IAdminUserService>().InstancePerLifetimeScope();
            builder.RegisterType<EFAdminUserRepository>().As<IAdminUserRepository>().InstancePerLifetimeScope();

            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>().InstancePerLifetimeScope();
            builder.RegisterType<EFUserOperationClaimRepository>().As<IUserOperationClaimRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MessageManager>().As<IMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<EFMessageRepository>().As<IMessageRepository>().InstancePerLifetimeScope();

            builder.RegisterType<SignatureManager>().As<ISignatureService>().InstancePerLifetimeScope();
            builder.RegisterType<EFSignatureRepository>().As<ISignatureRepository>().InstancePerLifetimeScope();

            builder.RegisterType<AdminStudentRelationManager>().As<IAdminStudentRelationService>().InstancePerLifetimeScope();
            builder.RegisterType<EFAdminStudentRelationRepository>().As<IAdminStudentRelationRepository>().InstancePerLifetimeScope();


            builder.RegisterType<AuthManager>().As<IAuthService>().InstancePerLifetimeScope();


            //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();


            builder.RegisterType<MailManager>().As<IMailService>().InstancePerLifetimeScope();
        }
    }
}
