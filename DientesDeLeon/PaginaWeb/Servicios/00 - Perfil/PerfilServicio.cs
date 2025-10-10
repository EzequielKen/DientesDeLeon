using _00___acceso_a_base_de_datos;
using _02___sistemas._00___Perfil;
using PaginaWeb.Models;

namespace PaginaWeb.Servicios._00___Perfil
{
    public class PerfilServicio
    {
        cls_Perfil perfil = new cls_Perfil();
        public async Task<PerfilViewModel> get_usuario(string id_usuario)
        {
            var usuarioDT = await perfil.get_usuario(id_usuario);
            if (usuarioDT.Rows.Count == 0)
            {
                return null; // O manejar el caso de usuario no encontrado según tus necesidades
            }
            var row = usuarioDT.Rows[0];
            var perfilViewModel = new PerfilViewModel
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                //Contraseña = row["Contraseña"].ToString(),
                //RepetirContraseña = row["Contraseña"].ToString(), // Asumiendo que quieres mostrar la misma contraseña
                //FechaNacimiento = Convert.ToDateTime(row["FechaNacimiento"]),
                Telefono = row["Telefono"].ToString()
            };
            return perfilViewModel;
        }

        public async Task actualizar_usuario(PerfilViewModel perfilViewModel)
        {
            var usuarioDT = await perfil.get_clone();
            var newRow = usuarioDT.NewRow();
            newRow["Nombre"] = perfilViewModel.Nombre;
            newRow["Apellido"] = perfilViewModel.Apellido;
            usuarioDT.Rows.Add(newRow);
            await perfil.actualizar_usuario(usuarioDT, perfilViewModel.Id.ToString());
        }
    }
}
