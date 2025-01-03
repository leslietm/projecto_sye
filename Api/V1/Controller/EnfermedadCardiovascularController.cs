using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TsaakAPI.Entities;
using TsaakAPI.Model.DAO;

namespace TsaakAPI.Api.V1.Controller
{
    [ApiController]
    [Route("tsaak/api/v1/[controller]")]
    public class EnfermedadCardiovascularController : ControllerBase
    {
        private readonly EnfermedadCardiovascularDao _enfermedadCardiovascularDao;
        private readonly IConfiguration _configuration;

        public EnfermedadCardiovascularController(EnfermedadCardiovascularDao enfermedadCardiovascularDao, IConfiguration configuration )
        {
            _enfermedadCardiovascularDao = enfermedadCardiovascularDao;
            _configuration = configuration;
           
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Llamada al DAO para obtener el registro
            var result = await _enfermedadCardiovascularDao.GetByIdAsync(id);

            // Verifica si la operación fue exitosa
            if (result.Success)
            {
                // Si es exitosa, devuelve el resultado con un estado 200 OK
                return Ok(result.Result);
            }
            else
            {
                // Si no fue exitosa, devuelve un error con el detalle
                return BadRequest(new { message = result.Messages });
            }
        }
 
    

        [HttpGet]
        public async Task<IActionResult> Get( )
        {
             var result = await _enfermedadCardiovascularDao.GetAllperson();

            if (result.Success)
            {
                return Ok(result.Result);
            }
            else
            {
               
                return BadRequest(new { message = result.Messages });
            }
        }
       //agregado
       
         [HttpPost]
        public async Task<IActionResult> Post([FromBody] EnfermedadCardiovascular enf)
        {
           
            var result = await _enfermedadCardiovascularDao.AddAsync(enf);

            
            if (result.Success)
            {
               
                return CreatedAtAction(nameof(Post), new { id = result.Result }, new { message = "Registro agregado exitosamente.", id = result.Result });
            }
            else
            {
               
                return BadRequest(new { message = result.Messages });
            }
            
        }  
         [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromBody] EnfermedadCardiovascular enf, int id)
        {
            enf.id_enf_cardiovascular = id;
            // Llamada al DAO para actualizar el registro
            var result = await _enfermedadCardiovascularDao.UpdAsync(enf, id);

            // Verifica si la operación fue exitosa
            if (result.Success)
            {
                // Si es exitosa, devuelve un mensaje con estado 200 OK
                return Ok(new { message = "Registro editado exitosamente.", id = enf.id_enf_cardiovascular });
            }
            else
            {
                // Si no fue exitosa, devuelve un error con el detalle
                return BadRequest(new { message = result.Messages });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            // Llamada al DAO para actualizar el registro
            var result = await _enfermedadCardiovascularDao.DeleteAsync(id);

            // Verifica si la operación fue exitosa
            if (result.Success)
            {
                // Si es exitosa, devuelve un mensaje con estado 200 OK
                return Ok(new { message = "Registro eliminado exitosamente.", id = id});
            }
            else
            {
                // Si no fue exitosa, devuelve un error con el detalle
                return BadRequest(new { message = result.Messages });
            }
        }
        
        
       
    }
}




    
