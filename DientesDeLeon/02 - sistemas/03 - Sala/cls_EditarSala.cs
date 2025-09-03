using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._03___Sala
{
    public class cls_EditarSala
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();

        public async Task<(bool resultado, string mensaje)> EditarSala(DataTable SalaEditar)
        {
            string actualizar, campo, valor;
            bool resultado = false;
            for (int columna = 1; columna <= SalaEditar.Columns.Count - 1; columna++)
            {
                campo = SalaEditar.Columns[columna].ColumnName;
                valor = SalaEditar.Rows[0][columna].ToString();
                actualizar = "`" + campo + "` = '" + valor + "'";
                resultado = await consultas.actualizar_tabla("salas", actualizar, SalaEditar.Rows[0]["id"].ToString());
                actualizar = string.Empty;
            }
            if (resultado)
            {
                return (true, "Sala editada correctamente.");
            }
            else
            {
                return (false, "Error al editar el Sala.");
            }
        }

        public async Task<DataTable> ObtenerSalaPorId(string salaID)
        {
            return await consultas.consultar_sala_por_id(salaID);
        }
    }
}
