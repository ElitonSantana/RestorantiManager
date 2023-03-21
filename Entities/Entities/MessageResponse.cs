using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    /// <summary>
    /// Classe para response genérica.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageResponse<T> where T : class
    {
        public bool HasError { get; set; } = false;
        public string? Message { get; set; }

        public T? Entity { get; set; }
    }
}
