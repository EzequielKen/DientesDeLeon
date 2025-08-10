using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _00___acceso_a_base_de_datos
{
    public class cls_conexion
    {
        #region constructor
        public cls_conexion()
        {
            servidor = "localhost";
            base_de_datos = "dientesdeleon";
            usuario = "aiden";
            password = "tiempo1794";
            puerto = "3306";

            //cadena_conexion = $"server={servidor};port={puerto};user id={usuario};password={password};database={base_de_datos};CharSet=utf8;";
            cadena_conexion = $"server={servidor};port={puerto};user id={usuario};password={password};database={base_de_datos};CharSet=utf8;SslMode=None;AllowPublicKeyRetrieval=True;";

            conexion = new MySqlConnection(cadena_conexion);
        }
        #endregion 

        #region atributos
        MySqlConnection conexion;
        MySqlDataAdapter adapter;
        MySqlCommand comando;
        MySqlDataReader lector;

        private string servidor;
        private string base_de_datos;
        private string usuario;
        private string password;
        private string puerto;
        private string cadena_conexion;

        private static readonly object _lock = new object(); // Definido a nivel de clase
        #endregion

        #region conexion
        private async Task abrir_conexion()
        {
            try
            {
                await conexion.OpenAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task cerrar_conexion()
        {
            try
            {
                conexion.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CRUD
        public async Task<bool> CREATE(string query)
        {
            bool retorno = false;
            try
            {
                await abrir_conexion();
                comando = new MySqlCommand(query, conexion);
                await comando.ExecuteReaderAsync();
                retorno = true;
            }
            catch (Exception ex)
            {
                retorno = false;
                throw ex;
            }
            finally
            {
                cerrar_conexion();
            }

            return retorno;
        }

        public async Task<DataTable> READ(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                await abrir_conexion();
                adapter = new MySqlDataAdapter(query, conexion);
                await adapter.FillAsync(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cerrar_conexion();
            }
            return dataTable;
        }

        public async Task<bool> UPDATE(string query)
        {
            bool retorno = false;
            try
            {
                await abrir_conexion();
                comando = new MySqlCommand(query, conexion);
                await comando.ExecuteReaderAsync();
                retorno = true;
            }
            catch (Exception ex)
            {
                retorno = false;
                throw ex;
            }
            finally
            { 
                cerrar_conexion();
            }

            return retorno;
        }
        #endregion
    }
}
