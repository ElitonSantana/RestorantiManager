using Domain.Interface;
using Entities.Entities;
using Infra.Repository.Generics.Interface;
using Microsoft.AspNetCore.Mvc;
using static Entities.Enums.Enumerators;

namespace RestorantiManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            this._tableService = tableService;
        }
        
        #region :: Reservas ::

        [Route("BookATable/{TableNumber}/{Type}")]
        [HttpGet]
        public async Task<IActionResult> BookATable(int TableNumber, int Type)
        {
            if (ModelState.IsValid)
            {
                var result = await _tableService.BookATable(new Request { TableNumber = TableNumber, Type = (ERequestType)Type });

                if (result.HasError)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            else
                return BadRequest("Model is not valid!");
        }

        #endregion

        #region :: Cadastro ::

        /// <summary>
        /// Método de criação de uma nova mesa - CADASTRO
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Table request)
        {
            if (ModelState.IsValid)
            {
                request.ModifiedDate = null;

                var result = await _tableService.Add(request);
                if (!result.HasError)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            else
            {
                return BadRequest("Model is not valid!");
            }
        }

        /// <summary>
        /// retorna a lista de mesas cadastradas.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = _tableService.GetList().Result;

                if (!result.HasError)
                    return Ok(result.Entity);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Houve um problema ao tentar buscar as mesas. Ex: {ex.Message}");
            }
        }

        /// <summary>
        /// Retorna informação de apenas uma mesa a partir do ID
        /// </summary>
        /// <param name="Id">Id da mesa</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                if (Id == 0)
                    return BadRequest("Não foi informado o Id da mesa.");

                var result = _tableService.GetTableById(Id).Result;

                if (!result.HasError)
                    return Ok(result.Entity);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Houve um problema ao tentar buscar a mesa de id {Id}. Ex: {ex.Message}");
            }
        }


        /// <summary>
        /// Método para deletar uma mesa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete/{TableId}")]
        public async Task<IActionResult> Delete(int TableId)
        {
            if (ModelState.IsValid)
            {
                var result = _tableService.Delete(TableId).Result;

                if (!result.HasError)
                    return Ok(result);
                else
                    return BadRequest(result);

            }
            else
                return BadRequest("Model is not valid!");
        }

        #endregion

        #region :: Requisições ::

        //Rota para fazer todas as solicitações de serviços.
        [Route("Request/{TableNumber}/{Type}")]
        [HttpGet]
        public async Task<IActionResult> RequestService(int TableNumber, int Type)
        {
            if (ModelState.IsValid)
            {
                var eType = (ERequestType)Type;

                if (eType == ERequestType.None)
                    return BadRequest("Tipo de requisição inválida.");

                if (TableNumber == 0)
                    return BadRequest("Número da mesa não foi informado.");

                var result = await _tableService.RequestService(TableNumber, Type);

                if (result.HasError)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            else
                return BadRequest("Model is not valid!");
        }

        #endregion
    }
}
