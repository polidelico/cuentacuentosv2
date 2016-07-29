using Cuentos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cuentos.Lib
{
    interface IDataContext
    {
        CuentosContext Db { get; }
    }
}