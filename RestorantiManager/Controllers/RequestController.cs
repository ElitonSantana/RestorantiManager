using Domain.Interface;
using Domain.Services;
using Entities.Entities;
using Infra.Repository;
using Infra.Repository.Generics.Interface;
using Microsoft.AspNetCore.Mvc;
using static Entities.Enums.Enumerators;

namespace RestorantiManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService ;
        public RequestController( IRequestService requestService)
        {
            this._requestService = requestService;
        }

        #region :: Requisições - Clientes::

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

                var result = await _requestService.RequestServiceAsync(TableNumber, Type);

                if (result.HasError)
                    return BadRequest(result.Message);

                return Ok(result.Message);
            }
            else
                return BadRequest("Model is not valid!");
        }

        [Route("Request/GetList")]
        [HttpGet]
        public async Task<IActionResult> RequestGetList()
        {
            if (ModelState.IsValid)
            {
                var result = await _requestService.RequestGetList();

                if (result.HasError)
                    return BadRequest(result.Message);

                return Ok(result);
            }
            else
                return BadRequest("Model is not valid!");
        }

        #endregion

        #region :: Requisições - Funcionários ::

        [Route("AcceptService/{UserId}/{RequestId}")]
        [HttpPost]
        public async Task<IActionResult> AcceptService(int UserId, int RequestId)
        {
            if (ModelState.IsValid)
            {
                var result = await _requestService.AcceptServiceAsync(UserId, RequestId);
                if (!result.HasError)
                    return Ok(result);
                else
                    return BadRequest(result.Message);
            }
            else
                return BadRequest("Model is not valid!");
        }

        #endregion

    }
}
