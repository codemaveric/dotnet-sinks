using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maverick.DataAccess.Models
{
    public class ApplicationRole: IdentityRole<int>
    {
        [StringLength(250)]
        public string Description { get; set; }
    }
}
