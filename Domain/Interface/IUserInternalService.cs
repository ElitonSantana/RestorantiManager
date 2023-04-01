using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IUserInternalService
    {
        Task<MessageResponse<UserInternal>> Create(UserInternal user, bool isTest = false);
        Task<MessageResponse<UserInternal>> Login(UserInternal user, bool isTest = false);
        Task<MessageResponse<UserInternal>> UpdateUser(UserInternal user);
        Task<MessageResponse<UserInternal>> DeleteUser(int Id);
        Task<MessageResponse<List<UserInternal>>> List();
        Task<bool> ValidatePasswordConfirm(string password, bool isTest = false);
    }
}
