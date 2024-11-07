using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Dtos.Auth
{
    public class UserRoleListDto
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
