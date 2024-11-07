using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Dtos.Auth
{
    public class SetRoleToUserDto
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
