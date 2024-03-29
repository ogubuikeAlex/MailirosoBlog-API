using AutoMapper;
using MalirosoBlog.Data.Interfaces;
using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.Entities;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MalirosoBlog.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IServiceFactory _serviceFactory;
        private readonly IMapper _mapper;
        private readonly IRepository<ApplicationRole> _roleRepo;        
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();
            _userManager = _serviceFactory.GetService<UserManager<ApplicationUser>>();
            _roleManager = _serviceFactory.GetService<RoleManager<ApplicationRole>>();
            _roleRepo = _unitOfWork.GetRepository<ApplicationRole>();            
            _mapper = _serviceFactory.GetService<IMapper>();
        }
        public async Task AddUserToRole(AddUserToRoleRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email.Trim().ToLower()) 
                ?? throw new InvalidOperationException($"User '{request.Email}' does not Exist!");

            await _userManager.AddToRoleAsync(user, request.Role.ToLower().Trim());           
        }

        public async Task RemoveUserFromRole(AddUserToRoleRequest request)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(request.Email.Trim().ToLower())
                ?? throw new InvalidOperationException($"User {request.Email} does not exist");

            bool userIsInRole = await _roleRepo.GetQueryable().Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .AnyAsync(r => r.UserRoles.Any(ur => ur.Role.Active));

            if (!userIsInRole)
                throw new InvalidOperationException($"User not in {request.Role} Role");

            await _userManager.RemoveFromRoleAsync(user, request.Role);
        }
    }
}
