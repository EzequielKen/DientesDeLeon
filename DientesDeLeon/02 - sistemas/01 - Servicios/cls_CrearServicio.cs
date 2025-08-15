using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._01___Servicios
{
    public class cls_CrearServicio
    {
        public cls_CrearServicio()
        {
            consultas = new cls_consultas_MySQL();
            funciones = new cls_funciones();
        }

        #region atributos
        private cls_consultas_MySQL consultas;
        private cls_funciones funciones;
        private DataTable servicioBD;
        #endregion

        #region carga a base de datos
        public async Task<(string mensaje, bool resultado)> crear_servicio(DataTable ServicioNuevo, string id_Consultorio)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            if (!await verificar_si_existe(ServicioNuevo, id_Consultorio))
            {
                var query = funciones.armar_query_insertar(ServicioNuevo);
                resultado = await consultas.insertar_en_tabla("servicios", query.columnas, query.valores);
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
                mensaje = "El ServicioNuevo ya existe";
            }
            return (mensaje, resultado);
        }

        private async Task<bool> verificar_si_existe(DataTable ServicioNuevo, string id_Consultorio)
        {
            bool verificado = false;
            string Servicio = ServicioNuevo.Rows[0]["Servicio"].ToString();
            await consultar_servicio(Servicio, id_Consultorio);

            if (servicioBD.Rows.Count > 0)
            {
                verificado = true;
            }

            return verificado;
        }
        #endregion

        #region consultas
        private async Task consultar_servicio(string servicio, string id_Consultorio)
        {
            servicioBD = await consultas.consultar_servicio_de_consultorio(servicio, id_Consultorio);
        }
        #endregion

        #region metodos get
        public async Task<DataTable> getClone()
        {
            DataTable clone = await consultas.consultar_tabla("servicios");
            return clone.Clone();
        }
        #endregion
    }
}
