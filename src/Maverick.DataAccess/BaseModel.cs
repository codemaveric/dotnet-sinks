using System;
using System.Collections.Generic;
using System.Text;

namespace Maverick.DataAccess
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
    }
}
