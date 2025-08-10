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
        public async Task<DataTable> consultar_usuario_segun_mail_o_usuario(string mail,string usuario)
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
        #endregion

        #region Negocio
        public async Task<DataTable> consultar_negocio_segun_id_usuario_y_nombre(string id_usuario, string nombre)
        {
            string query = "SELECT * FROM " + base_de_datos + ".negocio where id_usuario='" + id_usuario + "' and nombre='" + nombre + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_negocios_segun_id_usuario(string id_usuario)
        {
            string query = "SELECT * FROM " + base_de_datos + ".negocio where id_usuario='" + id_usuario + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_negocio_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".negocio where id='" + id + "'";
            return await consultar_query(query);
        }
        #endregion

        #region proveedor
        public async Task<DataTable> consultar_proveedor_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".proveedor where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_proveedor_segun_id_negocio_y_nombre(string id_negocio, string nombre)
        {
            string query = "SELECT * FROM " + base_de_datos + ".proveedor where id_negocio='" + id_negocio + "' and nombre='" + nombre + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_proveedor_existente(string id_negocio, string id_usuario, string nombre)
        {
            string query = "SELECT * FROM " + base_de_datos + ".proveedor where id_negocio='" + id_negocio + "' and id_usuario='" + id_usuario + "' and nombre='" + nombre + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_proveedor_segun_id_negocio(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".proveedor where id_negocio='" + id_negocio + "' and id_registrado='N/A'";
            return await consultar_query(query);
        }
        #endregion

        #region cliente
        public async Task<DataTable> consultar_cliente_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cliente where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_cliente_segun_id_negocio_y_nombre(string id_negocio, string nombre)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cliente where id_negocio='" + id_negocio + "' and nombre='" + nombre + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_cliente_existente(string id_negocio, string id_usuario, string nombre)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cliente where id_negocio='" + id_negocio + "' and id_usuario='" + id_usuario + "' and nombre='" + nombre + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_cliente_segun_id_negocio(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cliente where id_negocio='" + id_negocio + "'";
            return await consultar_query(query);
        }
        #endregion

        #region productos proveedor
        public async Task<DataTable> consultar_productos_where_id_proveedor(string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_proveedor='" + id_proveedor + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_where_id_proveedor_Activos(string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_proveedor='" + id_proveedor + "' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_venta(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_negocio='" + id_negocio + "' and venta='si'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_where_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_where_producto(string id_proveedor, string producto)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_proveedor='" + id_proveedor + "' and producto='" + producto + "'";
            return await consultar_query(query);
        }
        #endregion

        #region productos negocio proveedor
        public async Task<DataTable> consultar_productos_where_id_negocioProveedor(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_negocio='" + id_negocio + "' and venta='si'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_where_id_negocioProveedor_Activos(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_negocio='" + id_negocio + "' and activo='1' and venta='si'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_venta_negocioProveedor(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_negocio='" + id_negocio + "' and venta='si'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_negocioProveedor_where_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id='" + id + "' and venta='si'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_productos_where_producto_negocioProveedor(string id_negocio, string producto)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_negocio='" + id_negocio + "' and producto='" + producto + "' and venta='si'";
            return await consultar_query(query);
        }
        #endregion

        #region productos venta
        public async Task<DataTable> consultar_productos_where_id_negocio(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".producto where id_negocio='" + id_negocio + "' and id_proveedor='N/A'";
            return await consultar_query(query);
        }

        #endregion

        #region orden de compra
        public async Task<DataTable> consultar_orden_de_compra_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_compra(string id_negocio, string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_negocio='" + id_negocio + "' and id_proveedor='" + id_proveedor + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_nueva_orden_de_compra(string id_negocio, string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_negocio='" + id_negocio + "' and id_proveedor='" + id_proveedor + "' and estado='NuevaOrden'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_compra_segun_mes_y_año(string id_negocio, string id_proveedor, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_negocio='" + id_negocio + "' and id_proveedor='" + id_proveedor + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "'";
            return await consultar_query(query);
        }

        #endregion
        #region orden de compra de usuarios registrados
        public async Task<DataTable> consultar_ordenes_de_compra_cerradas_segun_mes_y_año(string id_negocio, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_proveedor_registrado='" + id_negocio + "' and id_proveedor='N/A' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and estado='Cerrada'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_compra_cerradas(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_proveedor_registrado='" + id_negocio + "' and id_proveedor='N/A' and estado='Cerrada'";
            return await consultar_query(query);
        }
        #endregion
        #region historial de compra
        public async Task<DataTable> consultar_orden_de_compra_segun_id_entreUsuarios(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_compra_entreUsuarios(string id_negocio, string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_negocio='" + id_negocio + "' and id_proveedor_registrado='" + id_proveedor + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_compra_entreUsuarios_procesadas(string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where  id_proveedor_registrado='" + id_proveedor + "' and envio='Procesada'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_nueva_orden_de_compra_entreUsuarios(string id_negocio, string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_negocio='" + id_negocio + "' and id_proveedor_registrado='" + id_proveedor + "' and estado='NuevaOrden'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_compra_segun_mes_y_año_entreUsuarios(string id_negocio, string id_proveedor, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra where id_negocio='" + id_negocio + "' and id_proveedor_registrado='" + id_proveedor + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "'";
            return await consultar_query(query);
        }
        #endregion

        #region orden de compra detalle
        public async Task<DataTable> consultar_detalle_orden_de_compra_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra_detalle where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_detalle_ordenes_de_compra(string id_orden_de_compra)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra_detalle where id_orden_de_compra='" + id_orden_de_compra + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_detalle_ordenes_de_compra_activo(string id_orden_de_venta)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra_detalle where activo='1' and id_orden_de_compra='" + id_orden_de_venta + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_sub_totales_sactivo_de_orden(string id_orden_de_compra)
        {
            string query = "SELECT sub_total FROM " + base_de_datos + ".orden_de_compra_detalle WHERE activo = '1' AND id_orden_de_compra = '" + id_orden_de_compra + "'";
            return await consultar_query(query);
        }

        public async Task<DataTable> consultar_detalle_ordenes_de_compra_segun_producto(string id_orden_de_compra, string id_producto)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_compra_detalle where id_orden_de_compra='" + id_orden_de_compra + "' and id_producto='" + id_producto + "'";
            return await consultar_query(query);
        }
        #endregion

        #region orden de venta
        public async Task<DataTable> consultar_orden_de_venta_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_venta(string id_negocio, string id_cliente)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_venta_cerradas(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta where id_negocio='" + id_negocio + "' and activo='1' and estado='Cerrada'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_nueva_orden_de_venta(string id_negocio, string id_cliente)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and estado='NuevaOrden'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_ordenes_de_venta_segun_mes_y_año(string id_negocio, string id_cliente, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "'";
            return await consultar_query(query);
        }
        #endregion

        #region orden de compra detalle
        public async Task<DataTable> consultar_detalle_orden_de_venta_segun_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta_detalle where id='" + id + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_detalle_ordenes_de_venta(string id_orden_de_venta)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta_detalle where id_orden_de_venta='" + id_orden_de_venta + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_detalle_ordenes_de_venta_activo(string id_orden_de_venta)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta_detalle where activo='1' and id_orden_de_venta='" + id_orden_de_venta + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_sub_totales_activos_de_orden_venta(string id_orden_de_venta)
        {
            string query = "SELECT sub_total FROM " + base_de_datos + ".orden_de_venta_detalle WHERE activo = '1' AND id_orden_de_venta = '" + id_orden_de_venta + "'";
            return await consultar_query(query);
        }

        public async Task<DataTable> consultar_detalle_ordenes_de_venta_segun_producto(string id_orden_de_venta, string id_producto)
        {
            string query = "SELECT * FROM " + base_de_datos + ".orden_de_venta_detalle where id_orden_de_venta='" + id_orden_de_venta + "' and id_producto='" + id_producto + "'";
            return await  consultar_query(query);
        }
        #endregion

        #region cuenta por pagar
        #region compra
        public async Task<DataTable> consultar_todas_cuentas_por_pagar_activas(string id_negocio, string id_proveedor)
        {
            string query = "SELECT fecha,total FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio + "' and id_proveedor='" + id_proveedor + "' and tipo='compra' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_cuentas_por_pagar_activas_delMes(string id_negocio, string id_cliente, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio + "' and id_proveedor='" + id_cliente + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and tipo='compra' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        #region venta
        public async Task<DataTable> consultar_todas_cuentas_por_pagar_activas_venta(string id_negocio, string id_cliente)
        {
            string query = "SELECT fecha,total FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and tipo='venta' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_cuentas_por_pagar_activas_delMes_venta(string id_negocio, string id_cliente, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and tipo='venta' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        #region compra usuarios
        public async Task<DataTable> consultar_todas_cuentas_por_pagar_activasUsuarios(string id_negocio, string id_cliente)
        {
            string query = "SELECT fecha,total,autorizado FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and tipo='usuarios' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_cuentas_por_pagar_activas_delMesUsuarios(string id_negocio_proveedor, string id_cliente, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio_proveedor + "' and id_cliente='" + id_cliente + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and tipo='usuarios' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        #region venta usuarios
        public async Task<DataTable> consultar_todas_cuentas_por_pagar_activas_ventaUsuarios(string id_negocio, string id_cliente)
        {
            string query = "SELECT fecha,total,autorizado FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and tipo='usuarios' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_cuentas_por_pagar_activas_delMes_ventaUsuarios(string id_negocio, string id_cliente, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".cuentas_por_pagar where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and tipo='usuarios' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        #region pago
        #region compra
        public async Task<DataTable> consultar_todos_pago_a_proveedor_activas(string id_negocio, string id_proveedor)
        {
            string query = "SELECT fecha,total FROM " + base_de_datos + ".pago_a_proveedor where id_negocio='" + id_negocio + "' and id_proveedor='" + id_proveedor + "' and tipo='pago' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_pago_a_proveedor_activas_delMes(string id_negocio, string id_proveedor, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".pago_a_proveedor where id_negocio='" + id_negocio + "' and id_proveedor='" + id_proveedor + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and tipo='pago' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        #region venta
        public async Task<DataTable> consultar_todos_pago_a_proveedor_activas_venta(string id_negocio, string id_cliente)
        {
            string query = "SELECT fecha,total ,autorizado FROM " + base_de_datos + ".pago_a_proveedor where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and tipo='cobro' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_pago_a_proveedor_activas_delMes_venta(string id_negocio, string id_cliente, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".pago_a_proveedor where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and tipo='cobro' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        #region venta usuarios
        public async Task<DataTable> consultar_todos_pago_a_proveedor_activas_ventaUsuarios(string id_negocio, string id_cliente)
        {
            string query = "SELECT fecha,total,autorizado FROM " + base_de_datos + ".pago_a_proveedor where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and tipo='usuarios' and activo='1'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_pago_a_proveedor_activas_delMes_ventaUsuarios(string id_negocio, string id_cliente, string mes, string año)
        {
            string query = "SELECT * FROM " + base_de_datos + ".pago_a_proveedor where id_negocio='" + id_negocio + "' and id_cliente='" + id_cliente + "' and MONTH(fecha) = '" + mes + "' and YEAR(fecha) = '" + año + "' and tipo='usuarios' and activo='1'";
            return await consultar_query(query);
        }
        #endregion
        #endregion
        #endregion

        #region mis proveedores
        public async Task<DataTable> consultar_relacion(string id_negocio, string id_proveedor)
        {
            string query = "SELECT * FROM " + base_de_datos + ".mis_proveedores where id_negocio='" + id_negocio + "' and id_proveedor='" + id_proveedor + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_relacion_de_negocio(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".mis_proveedores where id_negocio='" + id_negocio + "' and activo='1'";
            return await consultar_query(query);
        }
        #endregion

        #region mis clientes
        public async Task<DataTable> consultar_relacion_cliente_por_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".mis_clientes where id='" + id + "'";
            return await consultar_query(query);
        } 
        public async Task<DataTable> consultar_relacion_cliente(string id_negocio, string id_negocio_cliente)
        {
            string query = "SELECT * FROM " + base_de_datos + ".mis_clientes where id_negocio='" + id_negocio + "' and id_negocio_cliente='" + id_negocio_cliente + "'";
            return await consultar_query(query);
        }
        public async Task<DataTable> consultar_relacion_de_negocio_cliente(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".mis_clientes where id_negocio='" + id_negocio + "' and activo='1'";
            return await consultar_query(query);
        }
        #endregion

        #region calendario entrega semanal
        public async Task<DataTable> consultar_calendario_semanal_por_id(string id)
        {
            string query = "SELECT * FROM " + base_de_datos + ".calendario_entrega_semanal where id='" + id + "'";
            return await consultar_query(query);
        }

        public async Task<DataTable> consultar_calendario_semanal_de_negocio(string id_negocio)
        {
            string query = "SELECT * FROM " + base_de_datos + ".calendario_entrega_semanal where id_negocio='" + id_negocio + "'";
            return await consultar_query(query);
        }
        #endregion

        #endregion
    }
}
