using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._00___Perfil
{
    public class cls_Perfil
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();
        cls_funciones funciones = new cls_funciones();
        DataTable usuario;

        #region carga a base de dates
        public async Task actualizar_usuario(DataTable perfil,string id)
        {
            string actualizar = "`Nombre` = '"+ perfil.Rows[0]["Nombre"].ToString() +"'";

            await consultas.actualizar_tabla("usuarios", actualizar, id);

            actualizar = "`Apellido` = '" + perfil.Rows[0]["Apellido"].ToString() + "'";

            await consultas.actualizar_tabla("usuarios", actualizar, id);

        }
        #endregion
        #region metodos consultas
        private async Task consultar_usuario_id(string id_usuario)
        {
            usuario = await consultas.consultar_usuario_segun_id(id_usuario);
        }
        #endregion

        #region metodos get
        public async Task<DataTable> get_usuario(string id_usuario)
        {
            await consultar_usuario_id(id_usuario);
            return usuario;
        }

        public async Task<DataTable> get_clone()
        {
            DataTable clone = await consultas.consultar_tabla("usuarios");
            return clone.Clone();
        }
        #endregion
    }
}
