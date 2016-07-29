using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public class TimeStamps
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public TimeStamps()
        {
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}