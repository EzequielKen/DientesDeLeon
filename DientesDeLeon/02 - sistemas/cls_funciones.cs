using _01___modulos.MySQL;
using _01___modulos.PDF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Base;

namespace _02___sistemas
{
    public class cls_funciones
    {
        cls_consultas_MySQL consultas = new cls_consultas_MySQL();
        cls_QuestPDF PDF = new cls_QuestPDF();
        #region PDF
        public void generar_pdf(string ruta, byte[] logo, DataTable ordenDeCompra, DataTable orden_de_compra_detalle, DataTable proveedor, DataTable negocio)
        {
            string id_orden = ordenDeCompra.Rows[0]["id"].ToString();
            // Simulación de la obtención de datos, reemplaza estos con los reales



            // Llamar a la función para generar el PDF en memoria
            PDF.GenerarPDF_ordenCompra(
                ruta,
                logo,
                ordenDeCompra,
                orden_de_compra_detalle,
                proveedor,
                negocio
            );
        }

        public void generar_pdf_CuentaPorPagar(string ruta, byte[] logo, byte[] logo_negocio, DataTable ordenDeCompra, DataTable orden_de_compra_detalle, DataTable proveedor, DataTable negocio)
        {
            string id_orden = ordenDeCompra.Rows[0]["id"].ToString();
            // Simulación de la obtención de datos, reemplaza estos con los reales



            // Llamar a la función para generar el PDF en memoria
            PDF.GenerarPDF_cuentasPorPagar(
                ruta,
                logo,
                logo_negocio,
                ordenDeCompra,
                orden_de_compra_detalle,
                proveedor,
                negocio
            );
        }
        public void generar_pdf_Resumen_pedido(string rutaDestino, byte[] logo, byte[] logo_negocio, DataTable negocio, DataTable resumen)
        {
            PDF.GenerarPDF_ResumenPedidos(rutaDestino, logo, logo_negocio, negocio, resumen);
        }
        #endregion

        #region MySQL
        public async Task<bool> alctualizarDatosMyQL(string tabla, string campo, string dato, string id)
        {
            string actualizar = "`" + campo + "` = '" + dato + "'";
            return await consultas.actualizar_tabla(tabla, actualizar, id);
        }
        public (string columnas, string valores) armar_query_insertar(DataTable dt)
        {
            string columnas = string.Empty;
            string valores = string.Empty;
            bool ultima_columna = false;
            for (int col = 0; col <= dt.Columns.Count - 1; col++)
            {
                if (dt.Rows[0][col].ToString() == string.Empty)
                {
                    dt.Columns.Remove(dt.Columns[col]);
                    col = -1;
                }
            }
            for (int columna = 0; columna <= dt.Columns.Count - 1; columna++)
            {
                if (dt.Columns[columna].Ordinal == dt.Columns.Count - 1)
                {
                    ultima_columna = true;
                }
                columnas = armar_query_columna(columnas, dt.Columns[columna].ColumnName, ultima_columna);
                valores = armar_query_valores(valores, dt.Rows[0][columna].ToString(), ultima_columna);
            }
            return (columnas, valores);
        }
        public string armar_query_columna(string columnas, string columna_valor, bool ultimo_item)
        {
            string retorno = "";
            string separador_columna = "`";

            retorno = columnas + separador_columna + columna_valor + separador_columna;

            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        public string armar_query_valores(string valores, string valor_a_insertar, bool ultimo_item)
        {
            string retorno = "";
            string separador_valores = "'";

            retorno = valores + separador_valores + valor_a_insertar + separador_valores;
            if (!ultimo_item)
            {
                retorno = retorno + ",";
            }

            return retorno;
        }
        #endregion

        #region funciones de busqueda
        public bool buscarAlgunaCoincidencia(string datoUsuario, string datoBaseDatos)
        {
            bool coincidencia = false;
            if (datoUsuario == string.Empty)
            {
                coincidencia = true;
            }
            else
            {
                if (datoBaseDatos.IndexOf(datoUsuario, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    coincidencia = true;
                }
            }
            return coincidencia;
        }

        public int buscar_fila_por_dato(string dato, string columna, DataTable dt)
        {
            int retorno = -1;
            int fila = 0;
            while (fila <= dt.Rows.Count - 1)
            {
                if (dato == dt.Rows[fila][columna].ToString())
                {
                    retorno = fila;
                    break;
                }
                fila++;
            }
            return retorno;
        }

        #endregion

        #region funciones monetarias
        public string formatCurrency(string dato)
        {
            double valor = double.Parse(dato);
            return string.Format("{0:C2}", valor);
        }
        #endregion

        #region funciones de llenado
    
        public DataTable llenar_datatable(DataTable dt, string busqueda, string campo_busqueda, string campo_categoria, string categoria)
        {
            DataTable retorno = dt.Clone();

            for (int fila = 0; fila <= dt.Rows.Count - 1; fila++)
            {
                if (campo_categoria == "N/A")
                {
                    if (buscarAlgunaCoincidencia(busqueda, dt.Rows[fila][campo_busqueda].ToString()))
                    {
                        retorno.ImportRow(dt.Rows[fila]);
                    }
                }
                else
                {
                    if (buscarAlgunaCoincidencia(busqueda, dt.Rows[fila][campo_busqueda].ToString()) && dt.Rows[fila][campo_categoria].ToString() == categoria)
                    {
                        retorno.ImportRow(dt.Rows[fila]);
                    }
                }
            }
            return retorno;
        }
        #endregion

        #region fechas
        public string get_fecha()
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return fecha;
        }
        public bool verificar_fecha(string fecha_1_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year == int.Parse(año_dato) && fecha_1.Month == int.Parse(mes_dato))
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_fecha_completa(string fecha_1_dato, string dia_dato, string mes_dato, string año_dato)
        {
            bool retorno = false;
            DateTime fecha_1;
            fecha_1 = DateTime.Parse(fecha_1_dato);

            if (fecha_1.Year == int.Parse(año_dato) && fecha_1.Month == int.Parse(mes_dato) &&
                fecha_1.Day == int.Parse(dia_dato))
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_fecha_anterior(string fecha_dato, string mes, string año)
        {
            bool retorno = false;
            System.DateTime fecha = DateTime.Parse(fecha_dato);

            if (fecha.Year < int.Parse(año))
            {
                retorno = true;
            }
            else if (fecha.Year.ToString() == año && fecha.Month < int.Parse(mes))
            {
                retorno = true;
            }
            return retorno;
        }
        public bool verificar_fecha_anterior_con_dia(string fecha_dato, string dia, string mes, string año)
        {
            bool retorno = false;
            System.DateTime fecha = DateTime.Parse(fecha_dato);

            if (fecha.Year < int.Parse(año))
            {
                retorno = true;
            }
            else if (fecha.Year.ToString() == año && fecha.Month < int.Parse(mes))
            {
                retorno = true;
            }
            else if (fecha.Year.ToString() == año && fecha.Day < int.Parse(dia))
            {
                retorno = true;
            }
            return retorno;
        }
        #endregion

        #region carga
        public (string rutaCrearPDF, string rutaAbrirPDF) getRutaPDF(string rutaServer, DataTable usuario)
        {
            string id_usuario = usuario.Rows[0]["id"].ToString();
            crear_directorio_perfil(rutaServer);
            string rutaCrearPDF = rutaServer + "/PDF" + id_usuario + ".pdf";
            string rutaAbrirPDF = "~/PDF/PDF" + id_usuario + ".pdf";
            if (File.Exists(rutaCrearPDF))
            {
                File.Delete(rutaCrearPDF);
            }
            return (rutaCrearPDF, rutaAbrirPDF);
        }
        private void crear_directorio_perfil(string carpetaDestino)
        {
            // Crea la carpeta si no existe
            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }
        }

        #endregion

    }
}
