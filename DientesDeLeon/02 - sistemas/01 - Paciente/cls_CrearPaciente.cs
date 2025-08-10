using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._01___Paciente
{
    public class cls_CrearPaciente
    {
        public cls_CrearPaciente()
        {
            consultas = new cls_consultas_MySQL();
            funciones = new cls_funciones();
        }

        #region atributos
        private cls_consultas_MySQL consultas;
        private cls_funciones funciones;
        private DataTable negocioBD;
        #endregion

        #region carga a base de datos
        public async Task<(string mensaje, bool resultado)> crear_paciente(DataTable PacienteNuevo)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            if (!await verificar_si_existe(PacienteNuevo))
            {
                var query = funciones.armar_query_insertar(PacienteNuevo);
                resultado = await consultas.insertar_en_tabla("usuarios", query.columnas, query.valores);
                if (resultado)
                {
                    mensaje = "Negocio creado con exito";
                }
                else
                {
                    mensaje = "Error al conectar con la base de datos. Vuelva a intentar mas tarde.";
                }
            }
            else
            {
                mensaje = "El PacienteNuevo ya existe";
            }
            return (mensaje, resultado);
        }

        private async Task<bool> verificar_si_existe(DataTable PacienteNuevo)
        {
            bool verificado = false;
            string Usuario = PacienteNuevo.Rows[0]["Usuario"].ToString();
            await consultar_paciente(Usuario);

            if (negocioBD.Rows.Count > 0)
            {
                verificado = true;
            }

            return verificado;
        }
        #endregion

        #region consultas
        private async Task consultar_paciente(string Usuario)
        {
            negocioBD = await consultas.consultar_usuario(Usuario);
        }
        #endregion

        #region metodos get
        public async Task<DataTable> getClone()
        {
            DataTable clone = await consultas.consultar_tabla("usuarios");
            return clone.Clone();
        }
        #endregion
    }
}
