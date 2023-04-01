using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IRequestService
    {
        Task<MessageResponse<Request>> RequestServiceAsync(int TableNumber, int Type);
        Task<MessageResponse<List<Request>>> RequestGetList();
        Task<MessageResponse<Request>> AcceptServiceAsync(int EmployeeId, int RequestId); 
    }
}
