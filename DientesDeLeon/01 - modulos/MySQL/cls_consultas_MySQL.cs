using _00___acceso_a_base_de_datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01___modulos.MySQL
{
    public class cls_consultas_MySQL
    {
        #region constructor
        public cls_consultas_MySQL()
        {
            base_de_datos = "dientesdeleon";
        }
        #endregion

        #region atributos
        private string base_de_datos;
        #endregion

        #region consulta a tablas

        public async Task<DataTable> consultar_en_tabla_segun_id(string tabla, string id)
        {
            string query = "SELECT * FROM " + base_de_datos + "." + tabla + " where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_tabla(string tabla)
        {
            cls_conexion base_datos = new cls_conexion();
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla + " where activo=1";
                retorno = await base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public async Task<DataTable> consultar_tabla_completa(string tabla)
        {
            cls_conexion base_datos = new cls_conexion();
            DataTable retorno;
            string query;

            try
            {
                query = "SELECT * FROM " + base_de_datos + "." + tabla;
                retorno = await base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public async Task<DataTable> consultar_query(string query)
        {
            cls_conexion base_datos = new cls_conexion();
            DataTable retorno;

            try
            {
                retorno = await base_datos.READ(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return retorno;
        }
        public async Task<bool> insertar_en_tabla(string tabla, string columnas, string valores)
        {
            cls_conexion base_datos = new cls_conexion();
            bool retorno = false;
            string query;
            try
            {
                query = "INSERT INTO " + "`" + base_de_datos + "`" + "." + "`" + tabla + "`" + "(" + columnas + ") VALUES (" + valores + ");";
                retorno = await base_datos.CREATE(query);
            }
            catch (Exception ex)
            {
                retorno = false;
                throw ex;
            }

            return retorno;
        }

        public async Task<bool> actualizar_tabla(string tabla, string actualizar, string id)
        {
            cls_conexion base_datos = new cls_conexion();
            bool retorno = false;
            string query;
            try
            {//`ultimo_pedido_enviado` = '8'
                query = "UPDATE `" + base_de_datos + "`.`" + tabla + "` SET " + actualizar + " WHERE(`id` = '" + id + "');";
                retorno = await base_datos.UPDATE(query);
            }
            catch (Exception ex)
            {

                throw ex;

            }
            return retorno;
        }
        #endregion

        #region consultas personalizadas

        #region Login/Registro
        public async Task<DataTable> consultar_usuario_segun_login(string usuario, string contraseña)
        {
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where usuario='" + usuario + "' and contraseña='" + contraseña + "' and activo=1";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_usuario_segun_mail_o_usuario(string mail, string usuario)
        {
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where mail='" + mail + "' or usuario='" + usuario + "' and activo=1";
            return await consultar_query(query);
        }
        #endregion

        #region usuario
        public async Task<DataTable> consultar_usuario_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where id='" + id + "' and activo=1";
            return await consultar_query(query);
        }

        public async Task<DataTable> consultar_usuario(string Usuario)
        {
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where Usuario='" + Usuario + "'";
            return await consultar_query(query);
        }

        public async Task<DataTable> consultar_pacientes()
        {
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where activo=1 and Rol='paciente'";
            return await consultar_query(query);
        }

        public async Task<DataTable> consultar_paciente(string Usuario)
        {
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where Usuario='" + Usuario + "' and Rol='Paciente'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_paciente_por_id(string Id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".usuarios where Id='" + Id + "' and Rol='Paciente'";
            return await consultar_query(query);
        }
        #endregion

        #region servicios
        public async Task<DataTable> consultar_servicios_activos_de_consultorios(string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".servicios where id_Consultorio='" + id_Consultorio + "' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_servicios_de_consultorios(string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".servicios where id_Consultorio='" + id_Consultorio + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_servicio_de_consultorio(string servicio, string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".servicios where Servicio='" + servicio + "' and id_Consultorio='" + id_Consultorio + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_servicio_por_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".servicios where id='" + id + "'";
            return await consultar_query(query);
        }
        #endregion

        #region salas
        public async Task<DataTable> consultar_sala_existente(string sala,string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".salas where Sala='"+sala+"' and id_Consultorio='" + id_Consultorio + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_salas_de_consultorios(string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".salas where id_Consultorio='" + id_Consultorio + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_sala_por_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".salas where id='" + id + "'";
            return await consultar_query(query);
        }
        #endregion

        #region atencion de sala
        public async Task<DataTable> consultar_atencion_de_sala(string id_sala, string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".atencion_de_sala where id_sala='" + id_sala + "' and id_Consultorio='" + id_Consultorio + "' and activo='1'";
            return await consultar_query(query);
        }

        public async Task<DataTable> consultar_atencion(string id_sala, string id_servicio, string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".atencion_de_sala where id_sala='" + id_sala + "' and id_servicio='" + id_servicio + "'  and id_Consultorio='" + id_Consultorio + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_atencion_por_servicio(string id_servicio, string id_Consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".atencion_de_sala where id_servicio='" + id_servicio + "'  and id_Consultorio='" + id_Consultorio + "' and activo=1";
            return await consultar_query(query);
        }
        #endregion

        #region atencion
        public async Task<DataTable> consultar_atencion_de_usuario(string id_consultorio, string id_usuario)
        {
            string query = "SELECT * FROM " + base_de_datos + ".atencion where id_consultorio='" + id_consultorio + "'  and id_usuario='" + id_usuario + "' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        public async Task<DataTable> consultar_atencion_de_tecnico(string id_tecnico, string id_consultorio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".atencion_de_tecnico where id_tecnico='" + id_tecnico + "'  and id_consultorio='" + id_consultorio + "' and activo='1'";
            return await consultar_query(query);
        }
        #region tecnico

        #endregion
        #endregion
    }
}
