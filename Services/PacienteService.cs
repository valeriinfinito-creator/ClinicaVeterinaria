using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Services
{
    public class PacienteService
    {
        private readonly MySqlDbContext _context;

        public PacienteService(MySqlDbContext context)
        {
            _context = context;
        }

        // Método asíncrono para registrar un paciente en la base de datos
        // Se utiliza async/await para evitar bloquear el hilo principal
        public async Task<string> RegistrarPacienteAsync(Paciente paciente)
        {
            Console.WriteLine("Iniciando registro de paciente...");

            // Simulación de proceso lento (ej: conexión a base de datos)
            await Task.Delay(1000);

            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

            Console.WriteLine("Paciente registrado correctamente");

            return $"Paciente {paciente.Nombre} registrado";
        }

        // Método asíncrono para obtener todos los pacientes
        // Mejora la eficiencia al no bloquear mientras se consulta la BD
        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        // Simulación de procesos en paralelo dentro de la clínica
        // Se usan tareas independientes que no afectan la base de datos
        public async Task<string> ProcesosClinicaAsync()
        {
            var historialTask = Task.Run(async () =>
            {
                await Task.Delay(3000);
                return "Historial clínico cargado";
            });

            var citaTask = Task.Run(async () =>
            {
                await Task.Delay(2000);
                return "Cita agendada";
            });

            var notificacionTask = Task.Run(async () =>
            {
                await Task.Delay(1000);
                return "Notificación enviada";
            });

            // Task.WhenAll permite ejecutar múltiples tareas en paralelo
            var resultados = await Task.WhenAll(historialTask, citaTask, notificacionTask);

            return string.Join(" | ", resultados);
        }

        // Comparación entre Task.WhenAll y Task.WhenAny
        public async Task<string> CompararTareasAsync()
        {
            var tareaLarga = Task.Run(async () =>
            {
                await Task.Delay(3000);
                return "Proceso largo terminado";
            });

            var tareaRapida = Task.Run(async () =>
            {
                await Task.Delay(1000);
                return "Proceso rápido terminado";
            });

            var tareaMedia = Task.Run(async () =>
            {
                await Task.Delay(2000);
                return "Proceso medio terminado";
            });

            // Task.WhenAny devuelve la primera tarea que finaliza
            var primera = await Task.WhenAny(tareaLarga, tareaRapida, tareaMedia);
            string resultadoPrimero = await primera;

            // Task.WhenAll espera que todas las tareas finalicen
            var todos = await Task.WhenAll(tareaLarga, tareaRapida, tareaMedia);

            return $"Primero: {resultadoPrimero} | Todos: {string.Join(", ", todos)}";
        }

        // Registro de múltiples pacientes
        public async Task<string> RegistrarMultiplesPacientesAsync()
        {
            var pacientes = new List<Paciente>
            {
                new Paciente { Nombre = "Max", Especie = "Perro", Edad = 3 },
                new Paciente { Nombre = "Luna", Especie = "Gato", Edad = 2 },
                new Paciente { Nombre = "Rocky", Especie = "Perro", Edad = 5 }
            };

            foreach (var paciente in pacientes)
            {
                await RegistrarPacienteAsync(paciente);
            }

            return "Todos los pacientes fueron registrados correctamente";
        }
    }
}