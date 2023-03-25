
using Entities.Entities;

namespace Infra.Repository.Generics.Interface
{
    public interface IRTable : IRestorantiRepositoryGeneric<Table>
    {

        Task<Table> GetByTableNumber(int TableNumber);  

        #region ::Make Order::
        Task<MessageResponse<Request>> GetMakeOrderActive(int TableNumber);
        Task<MessageResponse<Request>> CreateMakeOrder(int TableNumber);
        Task<MessageResponse<Request>> CancelMakeOrder(Request request);
        #endregion

        #region ::Help::
        Task<MessageResponse<Request>> GetHelpActive(int TableNumber);
        Task<MessageResponse<Request>> CreateHelp(int TableNumber);
        Task<MessageResponse<Request>> CancelHelp(Request request);
        #endregion

        #region ::CloseAccount::
        Task<MessageResponse<Request>> GetCloseAccountActive(int TableNumber);
        Task<MessageResponse<Request>> CreateCloseAccount(int TableNumber);
        Task<MessageResponse<Request>> CancelCloseAccount(Request request);
        #endregion



    }
}
