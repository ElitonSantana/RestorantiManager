using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities.Base
{
    public class BaseModel
    {
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }
}
