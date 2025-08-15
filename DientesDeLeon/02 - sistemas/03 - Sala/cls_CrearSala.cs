using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._03___Sala
{
    public class cls_CrearSala
    {
        public cls_CrearSala()
        {
            consultas = new cls_consultas_MySQL();
            funciones = new cls_funciones();
        }

        #region atributos
        private cls_consultas_MySQL consultas;
        private cls_funciones funciones;
        private DataTable salaBD;
        #endregion

        #region carga a base de datos
        public async Task<(string mensaje, bool resultado)> crear_sala(DataTable SalaNueva)
        {
            bool resultado = false;
            string mensaje = string.Empty;
            if (!await verificar_si_existe(SalaNueva))
            {
                var query = funciones.armar_query_insertar(SalaNueva);
                resultado = await consultas.insertar_en_tabla("salas", query.columnas, query.valores);
                if (resultado)
                {
                    mensaje = "Salas creado con exito";
                }
                else
                {
                    mensaje = "Error al conectar con la base de datos. Vuelva a intentar mas tarde.";
                }
            }
            else
            {
                mensaje = "La Sala ya existe en el consultorio";
            }
            return (mensaje, resultado);
        }

        private async Task<bool> verificar_si_existe(DataTable SalaNueva)
        {
            bool verificado = false;
            string id_Consultorio = SalaNueva.Rows[0]["id_Consultorio"].ToString();
            string Sala = SalaNueva.Rows[0]["Sala"].ToString();
            await consultar_sala(Sala,id_Consultorio);

            if (salaBD.Rows.Count > 0)
            {
                verificado = true;
            }

            return verificado;
        }
        #endregion

        #region consultas
        private async Task consultar_sala(string sala,string id_consultorio)
        {
            salaBD = await consultas.consultar_sala_existente(sala,id_consultorio);
        }
        #endregion

        #region metodos get
        public async Task<DataTable> getClone()
        {
            DataTable clone = await consultas.consultar_tabla("salas");
            return clone.Clone();
        }
        #endregion
    }
}
