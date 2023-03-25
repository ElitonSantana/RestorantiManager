
using Domain.Interface;
using Entities.Entities;
using Infra.Repository.Generics.Interface;
using static Entities.Enums.Enumerators;

namespace Domain.Services
{
    public class TableService : ITableService
    {
        private readonly IRTable _rTable;
        public TableService(IRTable rTable)
        {
            this._rTable = rTable;
        }

        public async Task<MessageResponse<Table>> Add(Table request)
        {
            try
            {
                if (request != null)
                {
                    var tables = _rTable.GetList().Result;
                    if (tables.Any())
                    {
                        var tableNumber = tables.OrderByDescending(x => x.CreationDate).ToList().FirstOrDefault().TableNumber;
                        request.TableNumber = tableNumber + 1;
                    }
                    else
                        request.TableNumber = 1;

                    request.IsActive = true;

                    await _rTable.AddAsync(request);
                    return new MessageResponse<Table> { Entity = request, Message = "Mesa criada com sucesso!" };
                }
                else
                {
                    return new MessageResponse<Table> { HasError = true, Message = "Não há nenhuma informação sobre a nova mesa. (null)" };
                }
            }
            catch (Exception ex)
            {
                return new MessageResponse<Table> { HasError = true, Message = $"Não possível adicionar uma nova mesa - {ex.Message}" };
            }
        }
        public async Task<MessageResponse<List<Table>>> GetList()
        {
            try
            {
                var entityList = await _rTable.GetList();
                return new MessageResponse<List<Table>> { Entity = entityList };
            }
            catch (Exception ex)
            {
                return new MessageResponse<List<Table>> { HasError = true, Message = $"Ocorreu um problema ao tentar buscar as informações de mesas. Ex: {ex.Message}" };
            }
        }
        public async Task<MessageResponse<Table>> Delete(int TableId)
        {
            try
            {
                if (TableId != 0)
                {
                    var table = await _rTable.GetById(TableId);
                    if (table != null)
                    {
                        await _rTable.Delete(table);

                        return new MessageResponse<Table> { Entity = table, Message = "Mesa removida com sucesso!" };
                    }
                    else
                        return new MessageResponse<Table> { Entity = table, Message = "Mesa não encontrada para ser removida!" };
                }
                else
                {
                    return new MessageResponse<Table> { HasError = true, Message = $"Não há nenhuma mesa com o Id {TableId}!" };
                }
            }
            catch (Exception ex)
            {
                return new MessageResponse<Table> { HasError = true, Message = $"Não foi possível excluir a mesa - {ex.Message}" };
            }

        }
        public async Task<MessageResponse<Table>> GetTableById(int Id)
        {
            try
            {
                if (Id != 0)
                {
                    var table = await _rTable.GetById(Id);
                    if (table != null)
                        return new MessageResponse<Table> { Entity = table };
                    else
                        return new MessageResponse<Table> { HasError = true, Message = "Mesa não encontrada!" };
                }
                else
                {
                    return new MessageResponse<Table> { HasError = true, Message = $"Não há nenhuma mesa com o Id {Id}!" };
                }
            }
            catch (Exception ex)
            {
                return new MessageResponse<Table> { HasError = true, Message = $"Não foi possível buscar as informações da mesa de Id {Id} - {ex.Message}" };
            }

        }
        public async Task<MessageResponse<Table>> BookATable(Request request)
        {
            try
            {
                if (request.TableNumber != 0)
                {
                    var tables = _rTable.GetList().Result;
                    if (tables.Any())
                    {
                        var table = tables.Where(x => x.TableNumber == request.TableNumber).FirstOrDefault();

                        if (table != null)
                        {

                            //Caso esteja disponível
                            if (table.IsAvailable)
                            {
                                table.IsAvailable = false;
                                await _rTable.Update(table);
                            }
                            else
                            {
                                table.IsAvailable = true;
                                await _rTable.Update(table);
                            }
                            return new MessageResponse<Table> { Entity = table, Message = "Mesa reservada com sucesso!" };
                        }
                        else
                            return new MessageResponse<Table> { HasError = true, Message = "A mesa informada não existe!" };

                    }

                    return new MessageResponse<Table> { HasError = true, Message = "Nenhuma mesa foi encontrada." };
                }
                else
                {
                    return new MessageResponse<Table> { HasError = true, Message = $"Não existe nenhuma mesa com o número {request.TableNumber}." };
                }
            }
            catch (Exception ex)
            {
                return new MessageResponse<Table> { HasError = true, Message = $"Não possível realizar a reserva da mesa - {ex.Message}" };
            }
        }
        public async Task<MessageResponse<Request>> RequestService(int TableNumber, int Type)
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
                return new MessageResponse<Request> { HasError = true, Message = $"Não possível realizar a requisição de serviço - RequestService - {ex.Message}" };
            }
        }

    }
}
