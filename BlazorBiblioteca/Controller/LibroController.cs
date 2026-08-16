using BlazorBiblioteca.Data;
using BlazorBiblioteca.shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;



namespace BlazorBiblioteca.Controller
{
    [Route("api/libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly LibrosDBContext _context;

        public LibrosController(LibrosDBContext context)
        {
            _context = context;
        }

        //ruta para comunicacion al servidor
        [HttpGet("ConexionServidor")]
        public ActionResult<string> GetConexionServidor()
        {
            return Ok("conexion lograda");
        }

        //Conec a la BBDD
        [HttpGet("ConexionBBDD")]
        public async Task<ActionResult<string>> GetConexionLibros()
        {
            try
            {
                var respuesta = await _context.Libro.FirstOrDefaultAsync();
                return "correctamemnte conectado a la bbdd";
            }
            catch (Exception ex)
            {
                return $"Error al conectar a la base de datos: {ex.Message}";
            }

        }

        //agregar un libro a la BBDD
        [HttpPost("AgregarLibro")]
        public async Task<ActionResult<string>> HandleAgregarLibro([FromBody] Libro libro)
        {
            try
            {
                // IMPORTANTE: Al agregar, debemos asegurarnos de que el ID sea 0 para que la BBDD lo autogenere.
                libro.Id = 0;

                await _context.Libro.AddAsync(libro);
                await _context.SaveChangesAsync();

                return Ok("Libro agregado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar el libro: {ex.Message}");
            }
        }

        // traer todos los libros de la BBDD
        [HttpGet("ObtenerLibros")]
        public async Task<ActionResult<List<Libro>>> HandleObtenerLibros()
        {
            try
            {
                return await _context.Libro.ToListAsync();
            }
            catch (Exception ex)
            {
                // Esto provocará el error 500 en el cliente, pero nos da una pista en los logs
                return StatusCode(500, $"Error interno al obtener libros: {ex.Message}");
            }

        }

        [HttpDelete("EliminarLibro/{id}")]
        public async Task<ActionResult<string>> HandleEliminarLibro([FromRoute] int id)
        {
            var libroAEliminar = await _context.Libro.FindAsync(id);
            if (libroAEliminar == null)
            {
                return NotFound("No se encontró el libro a eliminar.");
            }

            _context.Libro.Remove(libroAEliminar);
            var resultado = await _context.SaveChangesAsync();

            if (resultado == 1) return Ok("Libro eliminado");
            else return BadRequest("No se pudo eliminar el libro.");
        }

        [HttpPut("ActualizarLibro")]
        public async Task<ActionResult<string>> HandleUpdateLibro([FromBody] Libro libro)
        {
            try
            {
                // Verificamos que el libro exista en la BBDD ANTES de intentar modificarlo
                var libroDB = await _context.Libro.FindAsync(libro.Id);

                if (libroDB == null)
                {
                    return NotFound($"No se encontró el libro con ID {libro.Id} para actualizar.");
                }

                // Actualizamos los campos manualmente
                libroDB.NombreLibro = libro.NombreLibro;
                libroDB.AutorLibro = libro.AutorLibro;
                libroDB.NumPaginas = libro.NumPaginas;
                libroDB.FechaPublicacion = libro.FechaPublicacion;

                // Entity Framework detecta los cambios y guarda
                await _context.SaveChangesAsync();

                return Ok("Libro actualizado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar libro: {ex.Message}");
            }
        }

    }
}