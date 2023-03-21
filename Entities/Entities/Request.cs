using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enums.Enumerators;

namespace Entities.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public ERequestType Type { get; set; }
        public int TableNumber { get; set; }

    }
}
