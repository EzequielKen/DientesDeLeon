using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._01___Paciente
{
    public class cls_ListaPacientes
    {
        public cls_ListaPacientes()
        {
            consultas = new cls_consultas_MySQL();
            funciones = new cls_funciones();
        }
        #region atributos
        private cls_consultas_MySQL consultas;
        private cls_funciones funciones;
        private DataTable paciente;
        private DataTable negocio;
        #endregion

        #region metodos consultas
        private async Task consultar_paciente(string usuario)
        {
            paciente = await consultas.consultar_paciente(usuario);
        }
        #endregion

        #region metodos get/set
        public async Task<DataTable> getPaciente(string usuario)
        {
            await consultar_paciente(usuario);
            return paciente;
        }

        public async Task<DataTable> getPacientes()
        {
            return await consultas.consultar_pacientes();
        }
        #endregion
    }
}
