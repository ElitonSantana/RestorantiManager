using Entities.Entities.Base;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    [Table("UserInternal")]
    public class UserInternal : BaseModel
    {
        [Key]
        public int EmployeeId { get; set; }

        /// <summary>
        /// Nome do usuário ( administrador ou funcionário )
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Telefone do usuário
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// Email do usuário
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Tipo de perfil
        /// </summary>
        public EProfile Profile { get; set; }

        /// <summary>
        /// Nome de usuário
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Confirmação da senha do usuário
        /// </summary>
        public string ConfirmPassword { get; set; }

    }
}
