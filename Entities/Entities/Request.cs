using Entities.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enums.Enumerators;

namespace Entities.Entities
{
    [Table("Request")]
    public class Request:BaseModel
    {
        public int Id { get; set; }
        public ERequestType Type { get; set; }
        public int TableNumber { get; set; }
        /// <summary>
        /// Solicitação em aberto.
        /// </summary>
        public bool isActive { get; set; }
        /// <summary>
        /// Id do funcionário que irá atender a solicitação
        /// </summary>
        public int EmployeeId { get; set; }
    }
}
