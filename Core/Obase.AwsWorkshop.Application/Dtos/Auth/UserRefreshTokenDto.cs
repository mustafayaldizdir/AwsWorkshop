﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Application.Dtos.Auth
{
    public class UserRefreshTokenDto
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
