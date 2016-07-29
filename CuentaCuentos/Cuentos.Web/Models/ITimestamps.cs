using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Models
{
    public interface ITimestamps
    {
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}