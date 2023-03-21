using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities.Base;

namespace Entities.Entities
{
    [Table("Tables")]
    public class Table : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
    }
}
