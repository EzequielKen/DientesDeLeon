using _02___sistemas;
using _02___sistemas._05___Recepcion;
using System.Data;

namespace PaginaWeb.Servicios._05___Recepcion
{
    public class RecepcionServicio
    {
        cls_Recepcion recepcion = new cls_Recepcion();
        cls_funciones funciones = new cls_funciones();
        public async Task<DataTable> GetPacientePorId(string Id)
        {
            return await recepcion.getPacientePorId(Id);
        }
        public async Task<DataTable> GetServicios(string id_consultorio)
        {
            return await recepcion.getServicios(id_consultorio);
        }
        public async Task<DataTable> getAtencionDePaciente(string id_consultorio, string id_usuario)
        {
            return await recepcion.getAtencionDePaciente(id_consultorio, id_usuario);
        }
        public async Task<bool> EnviarPacienteAEspera(string id_consultorio, string id_usuario, string id_servicio)
        {
            DataTable pacienteEspera = new DataTable();
            pacienteEspera.Columns.Add("id_consultorio", typeof(string));
            pacienteEspera.Columns.Add("id_usuario", typeof(string));
            pacienteEspera.Columns.Add("id_servicio", typeof(string));
            pacienteEspera.Columns.Add("servicio", typeof(string));
            pacienteEspera.Columns.Add("id_sala", typeof(string));
            pacienteEspera.Columns.Add("fecha", typeof(string));
            pacienteEspera.Columns.Add("precio", typeof(string));
            pacienteEspera.Columns.Add("estado", typeof(string));

            pacienteEspera.Rows.Add();

            pacienteEspera.Rows[0]["id_consultorio"] = id_consultorio;
            pacienteEspera.Rows[0]["id_usuario"] = id_usuario;
            pacienteEspera.Rows[0]["id_servicio"] = id_servicio;
            pacienteEspera.Rows[0]["fecha"] = funciones.get_fecha();
            pacienteEspera.Rows[0]["estado"] = "En Espera";

            // Implement the logic to send the patient to the waiting list
            // This is a placeholder implementation and should be replaced with actual logic
            bool resultado = await recepcion.enviar_paciente_a_espera(pacienteEspera);
            return resultado;
        }

        public async Task<bool> EliminarAtencion(string id_atencion)
        {
            return await recepcion.eliminar_atencion(id_atencion);
        }

        public async Task<DataTable> BuscarServicio(string buscar, string id_consultorio)
        {
            return await recepcion.BuscarServicioActivo(buscar,id_consultorio);
        }
        public async Task<DataTable> BuscarAtencionDePaciente(string Atencion, string id_consultorio, string id_usuario)
        {
            return await recepcion.BuscarAtencionDePaciente(Atencion, id_consultorio, id_usuario);
        }
    }
}
