using MalirosoBlog.Data.Interfaces;
using MalirosoBlog.Models.Entities;
using MalirosoBlog.Services.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MalirosoBlog.Services.Infrastructure
{
    public class CustomAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IUnitOfWork _unitOfWork;
       
        private readonly IRepository<ApplicationUserRole> _userRoleRepo;

        public CustomAuthorizationHandler(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
            _userRoleRepo = _unitOfWork.GetRepository<ApplicationUserRole>();            
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            if (string.IsNullOrEmpty(context.User.Identity?.Name))
            {
                return Task.CompletedTask;
            }

            string? userId = context.User.GetUserId();            

            IEnumerable<ApplicationUserRole> userRoles = await _userRoleRepo.GetQueryable()
                .Include(x => x.Role)
                .ToListAsync();           

            bool userRoleHasClaim = userRoles.Any(x => x.Role.Active);            

            if (userRoleHasClaim)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
