using _02___sistemas;
using _02___sistemas._01___Servicios;
using Org.BouncyCastle.Asn1;
using PaginaWeb.Models;
using System.Data;

namespace PaginaWeb.Servicios._01___Servicios
{
    public class ListarServicioServicio
    {
        cls_ListaServicio listaServicio = new cls_ListaServicio();
        cls_funciones funciones = new cls_funciones();
        ServicioViewModel ServicioEditar = new ServicioViewModel();
        public async Task<DataTable> ObtenerTodosLosServicios(string id_consultorio)
        {
            DataTable serviciosBD = await listaServicio.getServicios(id_consultorio);
            DataTable listaServicios = new DataTable();
            listaServicios.Columns.Add("activo", typeof(string));
            listaServicios.Columns.Add("ID", typeof(string));
            listaServicios.Columns.Add("Servicio", typeof(string));
            listaServicios.Columns.Add("Precio", typeof(string));

            int ultimaFila;
            for (int fila = 0; fila <= serviciosBD.Rows.Count - 1; fila++)
            {
                listaServicios.Rows.Add();
                ultimaFila = listaServicios.Rows.Count - 1;
                listaServicios.Rows[ultimaFila]["activo"] = serviciosBD.Rows[fila]["activo"].ToString();
                listaServicios.Rows[ultimaFila]["ID"] = serviciosBD.Rows[fila]["id"].ToString();
                listaServicios.Rows[ultimaFila]["Servicio"] = serviciosBD.Rows[fila]["Servicio"].ToString();
                listaServicios.Rows[ultimaFila]["Precio"] = serviciosBD.Rows[fila]["Precio"].ToString();
            }

            return listaServicios;
        }

        public async Task<ServicioViewModel> ObtenerServicioPorId(string idServicio)
        {
            DataTable servicioBD = await listaServicio.getServicio_id(idServicio);
            ServicioEditar.id = servicioBD.Rows[0]["id"].ToString();
            ServicioEditar.servicio = servicioBD.Rows[0]["Servicio"].ToString();
            ServicioEditar.precio = servicioBD.Rows[0]["Precio"].ToString();
            return ServicioEditar;
        }

        public async Task<DataTable> CambiarEstadoDeServicio(string idServicio)
        {
            DataTable serviciosBD = await listaServicio.getServicio_id(idServicio);

            string estado = serviciosBD.Rows[0]["activo"].ToString();
            string cambiarEstado;
            if (estado == "1")
            {
                cambiarEstado = "0"; // Cambiar a inactivo
            }
            else
            {
                cambiarEstado = "1"; // Cambiar a activo
            }
            listaServicio.cambiar_estado_servicio(idServicio, cambiarEstado);

            return serviciosBD;
        }
    }
}
