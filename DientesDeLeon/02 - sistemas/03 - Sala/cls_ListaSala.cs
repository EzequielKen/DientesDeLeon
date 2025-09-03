using _01___modulos.MySQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._03___Sala
{
    public class cls_ListaSala
    {
        public cls_ListaSala()
        {
            consultas = new cls_consultas_MySQL();
            funciones = new cls_funciones();
        }
        #region atributos
        private cls_consultas_MySQL consultas;
        private cls_funciones funciones;
        private DataTable salas;
        #endregion

        #region metodos consultas
        private async Task consultar_salas_de_consultorio(string id_consultorio)
        {
            salas = await consultas.consultar_salas_de_consultorios(id_consultorio);
        }
        #endregion

        #region metodos get/set
        public async Task<DataTable> getSalas(string id_consultorio)
        {
            await consultar_salas_de_consultorio(id_consultorio);
            return salas;
        }
        #endregion
    }
}
