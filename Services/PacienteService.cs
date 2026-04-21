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

        // Registrar paciente en base de datos (ASYNC)
        public async Task<string> RegistrarPacienteAsync(Paciente paciente)
        {
            // Se usa async para no bloquear mientras se guarda en BD
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

            return $"Paciente {paciente.Nombre} registrado correctamente";
        }

        // Obtener lista de pacientes (ASYNC)
        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        // Simulación de procesos en paralelo
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

            var resultados = await Task.WhenAll(historialTask, citaTask, notificacionTask);

            return string.Join(" | ", resultados);
        }

        // Comparación WhenAll vs WhenAny
        public async Task<string> CompararTareasAsync()
        {
            var t1 = Task.Run(async () =>
            {
                await Task.Delay(3000);
                return "Proceso largo terminado";
            });

            var t2 = Task.Run(async () =>
            {
                await Task.Delay(1000);
                return "Proceso rápido terminado";
            });

            var t3 = Task.Run(async () =>
            {
                await Task.Delay(2000);
                return "Proceso medio terminado";
            });

            // Espera la primera que termine
            var primera = await Task.WhenAny(t1, t2, t3);

            string resultadoPrimero = await primera;

            // Espera todas
            var todos = await Task.WhenAll(t1, t2, t3);

            return $"Primero: {resultadoPrimero} | Todos: {string.Join(", ", todos)}";
        }

        // Registrar múltiples pacientes de forma segura (uno por uno)
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
        // Se guarda uno por uno para evitar conflictos con DbContext
        await RegistrarPacienteAsync(paciente);
   		 }

    	return "Todos los pacientes fueron registrados correctamente";
}
}
}