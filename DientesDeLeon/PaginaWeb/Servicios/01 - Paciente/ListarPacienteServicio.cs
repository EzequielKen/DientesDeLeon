using _02___sistemas._01___Paciente;
using System.Data;

namespace PaginaWeb.Servicios._01___Paciente
{
    public class ListarPacienteServicio
    {
        cls_ListaPacientes listarNegocios = new cls_ListaPacientes();
        public async Task<DataTable> ObtenerPaciente(string Usuario)
        {
            DataTable pacientesBD = await listarNegocios.getPaciente(Usuario);
            DataTable listaPacientes = new DataTable();
            listaPacientes.Columns.Add("ID", typeof(string));
            listaPacientes.Columns.Add("Usuario", typeof(string));
            listaPacientes.Columns.Add("Contraseña", typeof(string));
            listaPacientes.Columns.Add("Nombre", typeof(string));
            listaPacientes.Columns.Add("Apellido", typeof(string));
            listaPacientes.Columns.Add("Fecha de nacimiento", typeof(string));
            listaPacientes.Columns.Add("Telefono", typeof(string));

            int ultimaFila;
            for (int fila = 0; fila <= pacientesBD.Rows.Count - 1; fila++)
            {
                listaPacientes.Rows.Add();
                ultimaFila = listaPacientes.Rows.Count - 1;
                listaPacientes.Rows[ultimaFila]["ID"] = pacientesBD.Rows[fila]["id"].ToString();
                listaPacientes.Rows[ultimaFila]["Usuario"] = pacientesBD.Rows[fila]["usuario"].ToString();
                listaPacientes.Rows[ultimaFila]["Contraseña"] = pacientesBD.Rows[fila]["contraseña"].ToString();
                listaPacientes.Rows[ultimaFila]["Nombre"] = pacientesBD.Rows[fila]["nombre"].ToString();
                listaPacientes.Rows[ultimaFila]["Apellido"] = pacientesBD.Rows[fila]["apellido"].ToString();
                listaPacientes.Rows[ultimaFila]["Fecha de nacimiento"] = pacientesBD.Rows[fila]["FechaNacimiento"].ToString();
                listaPacientes.Rows[ultimaFila]["Telefono"] = pacientesBD.Rows[fila]["telefono"].ToString();
            }

            return listaPacientes;
        }

        public async Task<DataTable> ObtenerTodosPacientes()
        {
            DataTable pacientesBD = await listarNegocios.getPacientes();
            DataTable listaPacientes = new DataTable();
            listaPacientes.Columns.Add("ID", typeof(string));
            listaPacientes.Columns.Add("Usuario", typeof(string));
            listaPacientes.Columns.Add("Contraseña", typeof(string));
            listaPacientes.Columns.Add("Nombre", typeof(string));
            listaPacientes.Columns.Add("Apellido", typeof(string));
            listaPacientes.Columns.Add("Fecha de nacimiento", typeof(string));
            listaPacientes.Columns.Add("Telefono", typeof(string));

            int ultimaFila;
            for (int fila = 0; fila <= pacientesBD.Rows.Count - 1; fila++)
            {
                listaPacientes.Rows.Add();
                ultimaFila = listaPacientes.Rows.Count - 1;
                listaPacientes.Rows[ultimaFila]["ID"] = pacientesBD.Rows[fila]["id"].ToString();
                listaPacientes.Rows[ultimaFila]["Usuario"] = pacientesBD.Rows[fila]["usuario"].ToString();
                listaPacientes.Rows[ultimaFila]["Contraseña"] = pacientesBD.Rows[fila]["contraseña"].ToString();
                listaPacientes.Rows[ultimaFila]["Nombre"] = pacientesBD.Rows[fila]["nombre"].ToString();
                listaPacientes.Rows[ultimaFila]["Apellido"] = pacientesBD.Rows[fila]["apellido"].ToString();
                listaPacientes.Rows[ultimaFila]["Fecha de nacimiento"] = pacientesBD.Rows[fila]["FechaNacimiento"].ToString();
                listaPacientes.Rows[ultimaFila]["Telefono"] = pacientesBD.Rows[fila]["telefono"].ToString();
            }

            return listaPacientes;
        }
    }
}
