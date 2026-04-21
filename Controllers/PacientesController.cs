using Microsoft.AspNetCore.Mvc;
using ClinicaVeterinaria.Models;
using ClinicaVeterinaria.Services;

namespace ClinicaVeterinaria.Controllers
{
    public class PacientesController : Controller
    {
        private readonly PacienteService _service;

        // Inyección de dependencias
        public PacientesController(PacienteService service)
        {
            _service = service;
        }

        // GET: /Pacientes
        public async Task<IActionResult> Index()
        {
            var pacientes = await _service.ObtenerPacientesAsync();
            return View(pacientes);
        }

        // GET: /Pacientes/Registrar
        public IActionResult Registrar()
        {
            return View();
        }

        // POST: /Pacientes/Registrar
        [HttpPost]
        public async Task<IActionResult> Registrar(Paciente paciente)
        {
            if (!ModelState.IsValid)
            {
                return View(paciente);
            }

            try
            {
                var resultado = await _service.RegistrarPacienteAsync(paciente);
                TempData["Mensaje"] = resultado;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejo básico de errores
                ModelState.AddModelError("", "Error al registrar el paciente");
                Console.WriteLine(ex.Message);

                return View(paciente);
            }
        }

        // Simulación de procesos en paralelo
        public async Task<IActionResult> Procesos()
        {
            var resultado = await _service.ProcesosClinicaAsync();
            ViewBag.Resultado = resultado;

            return View("Index");
        }

        // Comparación WhenAll vs WhenAny
        public async Task<IActionResult> Comparar()
        {
            var resultado = await _service.CompararTareasAsync();
            ViewBag.Resultado = resultado;

            return View("Index");
        }

        // Registro de múltiples pacientes en paralelo
        public async Task<IActionResult> Multiples()
        {
            var resultado = await _service.RegistrarMultiplesPacientesAsync();
            ViewBag.Resultado = resultado;

            return View("Index");
        }
    }
}