using Domain.Interface;
using Entities.Entities;
using Infra.Repository.Generics.Interface;
using Microsoft.AspNetCore.Mvc;

namespace RestorantiManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserInternalController : ControllerBase
    {

        private readonly IRUserInternal _rUserInternal;
        private readonly IUserInternalService _userService;
        public UserInternalController(IRUserInternal rUserInternal,
                              IUserInternalService userService
                              )
        {
            this._rUserInternal = rUserInternal;
            this._userService = userService;
        }

        #region Login

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserInternal request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Create(request);
                if (!result.HasError)
                    return Ok(result);
                else
                    return BadRequest(result.Message);
            }
            else
                return BadRequest("Model is not valid!");
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserInternal request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(request);
                if (!result.HasError)
                    return Ok(result);
                else
                    return BadRequest(result.Message);
            }
            else
            {
                return BadRequest("Model is not valid!");
            }
        }

        [Route("Update")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserInternal request)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUser(request);
                if (!result.HasError)
                    return Ok(result);
                else
                    return BadRequest(result.Message);
            }
            else
            {
                return BadRequest("Model is not valid!");
            }
        }

        [Route("Delete/{EmployeeId}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int EmployeeId)
        {
            if (ModelState.IsValid)
            {
                if (EmployeeId != 0)
                {
                    var result = await _userService.DeleteUser(EmployeeId);
                    if (!result.HasError)
                        return Ok(result);
                    else
                        return BadRequest(result.Message);
                }
                else
                    return BadRequest("Id inválido.");
            }
            else
            {
                return BadRequest("Model is not valid!");
            }
        }

        #endregion

        /// <summary>
        /// Listagem de usuários
        /// </summary>
        /// <returns></returns>
        [Route("GetUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.List();
            if (!result.HasError)
                return Ok(result);
            else
                return BadRequest(result);
        }

        /// <summary>
        /// ENDPOINT PARA VALIDAR SE UTILIZOU SENHA DE ADMINISTRADOR PARA PROCESSOS.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("PasswordAdministratorConfirm/{password}")]
        [HttpPost]
        public async Task<IActionResult> PasswordAdministratorConfirm(string password)
        {
            var result = _userService.ValidatePasswordConfirm(password).Result;
            if (result)
                return Ok(result);
            else
                return BadRequest("Senha incorreta!");
        }

       

    }
}
