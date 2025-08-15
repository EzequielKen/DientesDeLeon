using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._01___Servicios
{
    public class cls_ListaServicio
    {
        #region atributos
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();
        private DataTable Servicios;
        #endregion

        #region carga a base de datos
        public async Task cambiar_estado_servicio(string servicioID,string estado)
        {
            string actualizar = "`activo` = '" + estado + "'";
            consultas.actualizar_tabla("servicios", actualizar, servicioID);
        }
        #endregion

        #region metodos consultas
        private async Task consultar_servicios_activos(string id_consultorio)
        {
            Servicios = await consultas.consultar_servicios_activos_de_consultorios(id_consultorio);
        }
        private async Task consultar_servicios(string id_consultorio)
        {
            Servicios = await consultas.consultar_servicios_de_consultorios(id_consultorio);
        }
        private async Task consultar_servicio(string servicioID)
        {
            Servicios = await consultas.consultar_servicio_por_id(servicioID);
        }
        #endregion

        #region metodos get/set
        public async Task<DataTable> getServicio_id(string id_servicio)
        {
            await consultar_servicio(id_servicio);
            return Servicios;
        }
        public async Task<DataTable> getServiciosActivos(string id_consultorio)
        {
            await consultar_servicios_activos(id_consultorio);
            return Servicios;
        }
        public async Task<DataTable> getServicios(string id_consultorio)
        {
            await consultar_servicios(id_consultorio);
            return Servicios;
        }
        #endregion
    }
}
