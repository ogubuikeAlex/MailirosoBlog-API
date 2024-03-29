using MalirosoBlog.Data.Context;
using MalirosoBlog.Data.Implementation;
using MalirosoBlog.Data.Interfaces;
using MalirosoBlog.Services.Implementation;
using MalirosoBlog.Services.Infrastructure;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace MalirosoBlog.Services.Extensions
{
    public static class MiddleWareExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {            
            services.AddTransient<IJWTAuthenticator, JWTAuthenticator>();
            services.AddTransient<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddTransient<IUnitOfWork, UnitOfWork<MailRosoBlogDbContext>>();
            services.AddTransient<IServiceFactory, ServiceFactory>();            
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IBlogPostService, BlogPostService>();
        }
    }
}
