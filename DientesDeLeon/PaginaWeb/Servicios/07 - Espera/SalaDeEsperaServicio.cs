using _02___sistemas._07___Espera;
using System.Data;

namespace PaginaWeb.Servicios._07___Espera
{
    public class SalaDeEsperaServicio
    {
        cls_SalaDeEspera salaDeEspera = new cls_SalaDeEspera();
        public async Task<DataTable> get_PacientesEnEspera(string id_tecnico, string id_consultorio)
        {
            return await salaDeEspera.get_PacientesEnEspera(id_tecnico, id_consultorio);
        }
    }
}
