using _02___sistemas;
using _02___sistemas._01___Paciente;
using PaginaWeb.Models;
using System.Data;

namespace PaginaWeb.Servicios._01___Paciente
{
    public class CrearPacienteServicio
    {
        cls_CrearPaciente crearPaciente = new cls_CrearPaciente();
        cls_Twilio twilio = new cls_Twilio();
        public bool verificarNumeroE164(string codigoPais, string codigoArea, string numeroLocal)
        {
            if (string.IsNullOrWhiteSpace(codigoPais) || string.IsNullOrWhiteSpace(codigoArea) || string.IsNullOrWhiteSpace(numeroLocal))
            {
                return false;
            }
            // Verificar que el código de país comience con '+'
            if (!codigoPais.StartsWith("+"))
            {
                return false;
            }
            // Verificar que el código de área y el número local sean numéricos
            if (!int.TryParse(codigoArea, out _) || !int.TryParse(numeroLocal, out _))
            {
                return false;
            }

            var verificado = twilio.FormatearNumeroE164(codigoPais, codigoArea, numeroLocal);
            return verificado.verificado;
        }

        public async Task<(string mensaje, bool resultado)> CrearPaciente(PacienteViewModel PacienteNuevo)
        {
            //AQUI QUIERO PROCESAR EL LOGOFILE
            DataTable paciente_tabla = await crearPaciente.getClone();

            paciente_tabla.Columns["FechaNacimiento"].DataType = typeof(string);
            paciente_tabla.Rows.Add();
            paciente_tabla.Rows[0]["Usuario"] = PacienteNuevo.Usuario;
            paciente_tabla.Rows[0]["Contraseña"] = PacienteNuevo.Contraseña;
            paciente_tabla.Rows[0]["Nombre"] = PacienteNuevo.Nombre;
            paciente_tabla.Rows[0]["Apellido"] = PacienteNuevo.Apellido;
            paciente_tabla.Rows[0]["FechaNacimiento"] = PacienteNuevo.FechaNacimiento;
            paciente_tabla.Rows[0]["Telefono"] = PacienteNuevo.CodigoPais + PacienteNuevo.CodigoArea + PacienteNuevo.Telefono;
            paciente_tabla.Rows[0]["Rol"] = "paciente";

            return await crearPaciente.crear_paciente(paciente_tabla);
        }
    }
}
