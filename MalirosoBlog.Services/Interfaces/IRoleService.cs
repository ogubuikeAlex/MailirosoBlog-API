using MalirosoBlog.Models.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalirosoBlog.Services.Interfaces
{
    internal interface IRoleService
    {
        Task AddUserToRole(AddUserToRoleRequest request);

        Task RemoveUserFromRole(AddUserToRoleRequest request);
    }
}
