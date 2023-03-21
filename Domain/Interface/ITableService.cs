using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ITableService
    {
        Task<MessageResponse<Table>> Add(Table request);
        Task<MessageResponse<List<Table>>> GetList();
        Task<MessageResponse<Table>> Delete(int TableId);
        Task<MessageResponse<Table>> BookATable(Request request);
        Task<MessageResponse<Table>> GetTableById(int Id);
        

    }
}
