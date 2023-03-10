using Microsoft.AspNetCore.Mvc;

namespace RestorantiManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {

        public TableController()
        {

        }

        #region Reservas

        [Route("BookATable")]
        [HttpPost]
        public async Task<IActionResult> BookATable([FromBody] string request)
        {
            if (ModelState.IsValid)
            {
                //Implementar chamada de serviço para fazer a validação e reservar ou liberar uma mesa.
                return Ok("Mesa reservada com sucesso!");
            }
            else
                return BadRequest("Model is not valid!");
        }

        #endregion

    }
}
