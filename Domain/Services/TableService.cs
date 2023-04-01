
using Domain.Interface;
using Entities.Entities;
using Infra.Repository;
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
        public async Task<MessageResponse<Table>> UpdateTable(Table table)
        {
            try
            {
                var tableExistent = _rTable.GetById(table.Id).Result;

                if (tableExistent != null)
                {
                    tableExistent.TableNumber = table.TableNumber;
                    tableExistent.IsAvailable = table.IsAvailable;
                    tableExistent.IsActive = table.IsActive;
                    tableExistent.ModifiedDate = DateTime.Now;

                    await _rTable.Update(tableExistent);

                    return new MessageResponse<Table> { HasError = false, Entity = tableExistent, Message = "Mesa atualizado com sucesso!" };
                }
                else
                    return new MessageResponse<Table> { HasError = true, Message = "Erro ao encontrar uma mesa para atualizar." };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro técnico ao realizar o update da mesa, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}");
                return new MessageResponse<Table> { HasError = true, Message = "Ocorreu um erro técnico ao realizar o update da mesa, verificar. Exception: {ex.Message}  StackTrace: {ex.StackTrace}" };
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
        

    }
}
