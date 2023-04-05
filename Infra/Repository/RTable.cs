using Entities.Entities;
using Infra.Context;
using Infra.Repository.Generics;
using Infra.Repository.Generics.Interface;
using Microsoft.EntityFrameworkCore;
using static Entities.Enums.Enumerators;

namespace Infra.Repository
{
    public class RTable : RestorantiManagerRepository<Table>, IRTable
    {
        private readonly DbContextOptions<RestorantiManagerContext> _optionsBuilder;
        public RTable()
        {
            this._optionsBuilder = new DbContextOptions<RestorantiManagerContext>();
        }

        #region :: Make Order ::
        public async Task<MessageResponse<Request>> CancelMakeOrder(Request request)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                try
                {
                    request.isActive = false;
                    var result = data.Update(request);
                    await data.SaveChangesAsync();

                    return new MessageResponse<Request> { HasError = false, Message = "Solicitação cancelada com sucesso! - CancelMakeOrder" };
                }
                catch (Exception ex)
                {
                    return new MessageResponse<Request> { HasError = true, Message = $"Erro ao cancelar a solicitação! - CancelMakeOrder - {ex.Message}." };
                }
            }
        }

        public async Task<MessageResponse<Request>> CreateMakeOrder(int TableNumber)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var request = new Request
                {
                    TableNumber = TableNumber,
                    Type = ERequestType.MakeAOrder,
                    isActive = true
                };

                AddModelBase(request);

                var result = data.AddAsync(request);
                await data.SaveChangesAsync();

                if (result.IsCompleted && result.IsCompletedSuccessfully)
                    return new MessageResponse<Request> { HasError = false, Message = "Solicitação enviada com sucesso! - CreateMakeOrder" };
                else
                    return new MessageResponse<Request> { HasError = true, Message = "Erro ao enviar a solicitação! - CreateMakeOrder" };
            }
        }

        public async Task<MessageResponse<Request>> GetMakeOrderActive(int TableNumber)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var makeOrderExistent = data.Set<Request>().Where(x => x.TableNumber == TableNumber && x.isActive == true && x.Type == ERequestType.MakeAOrder).FirstOrDefault();

                if (makeOrderExistent != null)
                    return new MessageResponse<Request> { Entity = makeOrderExistent };
                else
                    return null;
            }
        }

        #endregion

        #region :: Help ::

        public async Task<MessageResponse<Request>> GetHelpActive(int TableNumber)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var HelpExistent = data.Set<Request>().Where(x => x.TableNumber == TableNumber && x.isActive == true && x.Type == ERequestType.Help).FirstOrDefault();

                if (HelpExistent != null)
                    return new MessageResponse<Request> { Entity = HelpExistent };
                else
                    return null;
            }
        }

        public async Task<MessageResponse<Request>> CreateHelp(int TableNumber)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var request = new Request
                {
                    TableNumber = TableNumber,
                    Type = ERequestType.Help,
                    isActive = true
                };

                AddModelBase(request);

                var result = data.AddAsync(request);
                await data.SaveChangesAsync();

                if (result.IsCompleted && result.IsCompletedSuccessfully)
                    return new MessageResponse<Request> { HasError = false, Message = "Solicitação enviada com sucesso! - CreateHelp" };
                else
                    return new MessageResponse<Request> { HasError = true, Message = "Erro ao enviar a solicitação! - CreateHelp" };
            }
        }

        public async Task<MessageResponse<Request>> CancelHelp(Request request)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                try
                {
                    request.isActive = false;
                    var result = data.Update(request);
                    await data.SaveChangesAsync();

                    return new MessageResponse<Request> { HasError = false, Message = "Solicitação cancelada com sucesso! - CancelHelp" };
                }
                catch (Exception ex)
                {
                    return new MessageResponse<Request> { HasError = true, Message = $"Erro ao cancelar a solicitação! - CancelHelp {ex.Message}." };
                }
            }
        }

        #endregion

        #region :: CloseAccount ::

        public async Task<MessageResponse<Request>> GetCloseAccountActive(int TableNumber)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var HelpExistent = data.Set<Request>().Where(x => x.TableNumber == TableNumber && x.isActive == true && x.Type == ERequestType.CloseAccount).FirstOrDefault();

                if (HelpExistent != null)
                    return new MessageResponse<Request> { Entity = HelpExistent };
                else
                    return null;
            }
        }

        public async Task<MessageResponse<Request>> CreateCloseAccount(int TableNumber)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var request = new Request
                {
                    TableNumber = TableNumber,
                    Type = ERequestType.Help,
                    isActive = true
                };

                AddModelBase(request);

                var result = data.AddAsync(request);
                await data.SaveChangesAsync();

                if (result.IsCompleted && result.IsCompletedSuccessfully)
                    return new MessageResponse<Request> { HasError = false, Message = "Solicitação enviada com sucesso! - CreateCloseAccount" };
                else
                    return new MessageResponse<Request> { HasError = true, Message = "Erro ao enviar a solicitação! - CreateCloseAccount" };
            }
        }

        public async Task<MessageResponse<Request>> CancelCloseAccount(Request request)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                try
                {
                    request.isActive = false;
                    var result = data.Update(request);
                    await data.SaveChangesAsync();

                    return new MessageResponse<Request> { HasError = false, Message = "Solicitação cancelada com sucesso! - CancelCloseAccount" };
                }
                catch (Exception ex)
                {
                    return new MessageResponse<Request> { HasError = true, Message = $"Erro ao cancelar a solicitação! - CancelCloseAccount {ex.Message}." };
                }
            }
        }

        #endregion


        public async Task<MessageResponse<List<Request>>> RequestGetList()
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var result = await data.Set<Request>().ToListAsync();

                if (result != null)
                {
                    result = result.Where(x => x.isActive == true).ToList();

                    if (result.Count > 0)
                        return new MessageResponse<List<Request>> { HasError = false, Entity = result, Message = "Solicitações encontradas com sucesso." };
                    else
                        return new MessageResponse<List<Request>> { HasError = true, Message = "Não foi encontrada nenhuma Solicitação ativa." };
                }
                else
                {
                    return new MessageResponse<List<Request>> { HasError = true, Message = "Não foi encontrada nenhuma Solicitação ativa. - null" };
                }
            }
        }
        public async Task<Table> GetByTableNumber(int TableNumber)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var result = await data.Set<Table>().ToListAsync();

                if (result != null)
                {
                    return result.Where(x => x.TableNumber == TableNumber && x.IsActive == true).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public void AddModelBase(Request user)
        {
            user.CreationDate = DateTime.Now;
        }

    }
}
