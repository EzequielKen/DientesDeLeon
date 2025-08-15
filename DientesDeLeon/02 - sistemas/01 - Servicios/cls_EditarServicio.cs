using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._01___Servicios
{
    public class cls_EditarServicio
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();

        public async Task<(bool resultado, string mensaje)> EditarServicio(DataTable ServicioEditar)
        {
            string actualizar, campo, valor;
            bool resultado = false;
            for (int columna = 1; columna <= ServicioEditar.Columns.Count - 1; columna++)
            {
                campo = ServicioEditar.Columns[columna].ColumnName;
                valor = ServicioEditar.Rows[0][columna].ToString();
                actualizar = "`" + campo + "` = '" + valor + "'";
                resultado = await consultas.actualizar_tabla("servicios", actualizar, ServicioEditar.Rows[0]["id"].ToString());
                actualizar = string.Empty;
            }
            if (resultado)
            {
                return (true, "Servicio editado correctamente.");
            }
            else
            {
                return (false, "Error al editar el servicio.");
            }
        }

        public async Task<DataTable> ObtenerServicioPorId(string servicioID)
        {
            return await consultas.consultar_servicio_por_id(servicioID);
        }
    }
}
