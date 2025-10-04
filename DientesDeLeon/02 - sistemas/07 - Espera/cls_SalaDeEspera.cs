using _01___modulos.MySQL;
using _02___sistemas._06___Tecnico;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02___sistemas._07___Espera
{
    public class cls_SalaDeEspera
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();
        cls_Tecnico tecnico = new cls_Tecnico();
        cls_funciones funciones = new cls_funciones();
        DataTable PacientesEnEspera;
        DataTable atencion_de_tecnico;
        #region metodos consultas
        private async Task consultar_PacientesEnEspera()
        {
            PacientesEnEspera = await consultas.consultar_Pacientes_En_Espera();
        }
        private async Task consultar_AtencionDeTecnico(string id_tecnico, string id_consultorio)
        {
            atencion_de_tecnico = await tecnico.getServiciosDeTecnico(id_tecnico, id_consultorio);
        }
        #endregion

        #region metodos get
        public async Task<DataTable> get_PacientesEnEspera(string id_tecnico, string id_consultorio)
        {
            await consultar_PacientesEnEspera();
            await consultar_AtencionDeTecnico(id_tecnico,id_consultorio);
            return PacientesEnEspera;
        }
        #endregion
    }
}
