using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._00___Login
{
    public class cls_Login
    {

        public cls_Login()
        {
            consultas = new cls_consultas_MySQL();
            funciones = new cls_funciones();
        }
        #region atributos
        private cls_consultas_MySQL consultas;
        private cls_funciones funciones;

        DataTable usuarioBD;
        #endregion

        #region metodos consultas
        private async Task consultar_usuario(string usuario, string contraseña)
        {
            usuarioBD = await consultas.consultar_usuario_segun_login(usuario, contraseña);
        }
        #endregion

        #region metodos get/set
        public async Task<(bool logeo, DataTable usuario)> login(string usuario, string contraseña)
        {
            bool retorno = false;

            await consultar_usuario(usuario, contraseña);

            if (usuarioBD.Rows.Count > 0)
            {
                retorno = true;
            }

            return (retorno, usuarioBD);
        }

        public async Task<DataTable> getUsuario(string usuario, string contraseña)
        {
            await consultar_usuario(usuario, contraseña);
            return usuarioBD;
        }
        #endregion
    }
}
