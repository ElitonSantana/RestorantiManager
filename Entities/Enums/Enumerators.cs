using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public class Enumerators
    {
        /// <summary>
        /// Tipos de solicitações
        /// </summary>
        public enum ERequestType
        {
            None,
            /// <summary>
            /// Reserva de uma mesa;
            /// </summary>
            BookATable = 10,
            /// <summary>
            /// Fazer um pedido;
            /// </summary>
            MakeAOrder = 20,
            /// <summary>
            /// Pedir ajuda;
            /// </summary>
            Help = 30
        }
    }
}
