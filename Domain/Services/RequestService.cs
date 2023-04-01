using Domain.Interface;
using Entities.Entities;
using Infra.Repository.Generics.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enums.Enumerators;

namespace Domain.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRRequest _rRequest;
        private readonly IRTable _rTable;
        private readonly IRUserInternal _rUserInternal;
        public RequestService(IRRequest rRequest, IRTable rTable, IRUserInternal rUserInternal)
        {
            this._rRequest = rRequest;
            this._rTable = rTable;
            this._rUserInternal = rUserInternal;
        }

        /// <summary>
        /// Validação para criar uma solicitação - Clientes
        /// </summary>
        /// <param name="TableNumber">Número da mesa</param>
        /// <param name="Type">Tipo de solicitação - ENUM </param>
        /// <returns></returns>
        public async Task<MessageResponse<Request>> RequestServiceAsync(int TableNumber, int Type)
        {
            try
            {
                var tableExistent = await _rTable.GetByTableNumber(TableNumber);
                if (tableExistent != null)
                {

                    ERequestType eType = (ERequestType)Type;
                    MessageResponse<Request> response = new MessageResponse<Request>();

                    switch (eType)
                    {
                        case ERequestType.None:
                            return new MessageResponse<Request> { HasError = true, Message = $"Nenhum serviço está relacionado ao tipo 0." };
                        case ERequestType.MakeAOrder:
                            var makeOrderexistent = await _rTable.GetMakeOrderActive(TableNumber);
                            if (makeOrderexistent == null)
                                response = await _rTable.CreateMakeOrder(TableNumber);
                            else
                                response = await _rTable.CancelMakeOrder(makeOrderexistent.Entity);
                            return response;
                        case ERequestType.Help:
                            var HelpExistent = await _rTable.GetHelpActive(TableNumber);
                            if (HelpExistent == null)
                                response = await _rTable.CreateHelp(TableNumber);
                            else
                                response = await _rTable.CancelHelp(HelpExistent.Entity);
                            return response;
                        case ERequestType.CloseAccount:
                            var CloseAccountExistent = await _rTable.GetCloseAccountActive(TableNumber);
                            if (CloseAccountExistent == null)
                                response = await _rTable.CreateCloseAccount(TableNumber);
                            else
                                response = await _rTable.CancelCloseAccount(CloseAccountExistent.Entity);
                            return response;
                        default:
                            return new MessageResponse<Request> { HasError = true, Message = $"Tipo de requisição não encontrada." };
                    }
                }
                else
                {
                    return new MessageResponse<Request> { HasError = true, Message = $"Nenhuma mesa ativa foi encontrada para o número {TableNumber}" };
                }
            }
            catch (Exception ex)
            {
                return new MessageResponse<Request> { HasError = true, Message = $"Não possível realizar a requisição de serviço - RequestServiceAsync - {ex.Message}" };
            }
        }
        public async Task<MessageResponse<List<Request>>> RequestGetList()
        {
            try
            {
                var entityList = await _rTable.RequestGetList();
                return entityList;
            }
            catch (Exception ex)
            {
                return new MessageResponse<List<Request>> { HasError = true, Message = $"Ocorreu um problema ao tentar buscar a lista de requisições de serviço. Ex: {ex.Message}" };
            }
        }

        /// <summary>
        /// Validação para aceitar uma solicitar - Funcionários
        /// </summary>
        /// <param name="EmployeeId"> Id do Funcionário</param>
        /// <param name="RequestId">Id da solicitação</param>
        /// <returns></returns>
        public async Task<MessageResponse<Request>> AcceptServiceAsync(int EmployeeId, int RequestId)
        {
            try
            {
                var userExistent = _rUserInternal.GetById(EmployeeId).Result;

                if (userExistent != null)
                {
                    var requestExistent = _rRequest.GetById(RequestId).Result;
                    if (requestExistent != null)
                    {
                        requestExistent.EmployeeId = EmployeeId;
                        requestExistent.ModifiedDate = DateTime.Now;

                        await _rRequest.Update(requestExistent);

                        return new MessageResponse<Request> { HasError = false, Entity = requestExistent, Message = $"Pedido aceito!" };
                    }
                    else
                        return new MessageResponse<Request> { HasError = true, Message = $"Solicitação não encontrada!" };
                }
                else
                    return new MessageResponse<Request> { HasError = true, Message = $"Usuário não encontrado!" };
            }
            catch (Exception ex)
            {
                return new MessageResponse<Request> { HasError = true, Message = $"Não possível aceitar a solicitação de serviço - AcceptServiceAsync - {ex.Message}" };
            }
        }

    }
}
