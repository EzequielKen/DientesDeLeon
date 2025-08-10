using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static QuestPDF.Infrastructure.Unit;    // para poder usar Auto
using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using System.Data;
using QuestPDF.Drawing;
using System.IO;
using _02___sistemas;

namespace _01___modulos.PDF
{
    public class cls_QuestPDF
    {
        private string formatCurrency(string dato)
        {
            double valor = double.Parse(dato);
            return string.Format("{0:C2}", valor);
        }
        public void GenerarPDF_ordenCompra(string rutaDestino, byte[] logo, DataTable orden_de_compra, DataTable orden_de_compra_detalle, DataTable proveedor, DataTable negocio)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);

                    // Encabezado
                    page.Header().ShowOnce().Row(row =>
                    {
                        // Logo
                        row.ConstantItem(120).Container().AlignCenter().Element(container =>
                        {
                            container.Image(logo);
                        });

                        // Información de la orden
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("ORDEN DE COMPRA")
                                .FontSize(18).Bold().FontColor("#3A3A3A");

                            col.Item().AlignCenter().Text("Proveedor: " + proveedor.Rows[0]["nombre"].ToString())
                                .FontSize(12).FontColor("#4F4F4F");
                        });

                        // Número de orden y fecha
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Container()
                                .Background("#006D77")  // Establecer el color de fondo
                                .Padding(5)              // Aplicar padding
                                .Text("Número de Orden: " + orden_de_compra.Rows[0]["id"].ToString())
                                .FontSize(12).FontColor("#ffffff").Bold();

                            col.Item().AlignCenter().Text("Fecha Orden: " + orden_de_compra.Rows[0]["fecha"].ToString())
                                .FontSize(12).FontColor("#4F4F4F");

                            col.Item().AlignCenter().Text("Fecha Entrega: " + orden_de_compra.Rows[0]["fecha_entrega_estimada"].ToString())
                                .FontSize(12).FontColor("#4F4F4F");
                        });
                    });

                    // Contenido de la orden de compra
                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        // Información del negocio
                        col.Item().Column(col_negocio =>
                        {
                            col_negocio.Item().Text("Datos del Negocio").Underline().Bold().FontSize(14).FontColor("#006D77");
                            col_negocio.Item().Text(txt =>
                            {
                                txt.Span("Negocio: ").SemiBold();
                                txt.Span(negocio.Rows[0]["nombre"].ToString());
                            });
                            col_negocio.Item().Text(txt =>
                            {
                                txt.Span("Dirección: ").SemiBold();
                                txt.Span(negocio.Rows[0]["direccion"].ToString());
                            });
                            col_negocio.Item().Text(txt =>
                            {
                                txt.Span("Teléfono: ").SemiBold();
                                txt.Span(negocio.Rows[0]["telefono"].ToString());
                            });
                        });

                        col.Item().LineHorizontal(1).LineColor("#E6E6E6");

                        // Tabla de detalles de la orden
                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Encabezado de la tabla
                            tabla.Header(header =>
                            {
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Producto").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Categoria").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Cantidad").Bold().FontSize(10);
                                });

                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Presentacion").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Precio Compra").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Sub Total").Bold().FontSize(10);
                                });
                            });

                            // Detalles de la orden
                            foreach (DataRow row in orden_de_compra_detalle.Rows)
                            {
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["producto"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["categoria"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["cantidad"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["presentacion"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(formatCurrency(row["precio_compra"].ToString())).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(formatCurrency(row["sub_total"].ToString())).FontSize(10);
                                });
                            }
                        });

                        // Total
                        col.Item().AlignRight().PaddingTop(10).Text("TOTAL: " + formatCurrency(orden_de_compra.Rows[0]["total"].ToString()))
                            .Bold().FontSize(12).FontColor("#006D77");

                        // Espacio para firma
                        col.Item().PaddingTop(30).Column(firma =>
                        {
                            firma.Item().Text("Entrega conforme / firma: _____________________________").FontSize(10);
                            firma.Item().Text("Recibe conforme / firma: _____________________________").FontSize(10);
                        });
                    });

                    // Pie de página con numeración
                    page.Footer().AlignRight().Text(txt =>
                    {
                        txt.Span("Página ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);
                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(rutaDestino);
        }

        public void GenerarPDF_cuentasPorPagar(string rutaDestino, byte[] logo, byte[] logo_negocio, DataTable orden_de_compra, DataTable orden_de_compra_detalle, DataTable proveedor, DataTable negocio)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Margin(30);

                    // Encabezado
                    page.Header().ShowOnce().Row(row =>
                    {
                        // Logo
                        row.ConstantItem(120).Container().AlignCenter().Element(container =>
                        {
                            container.Image(logo_negocio).FitArea();
                        });

                        // Información de la orden
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("ORDEN DE COMPRA")
                                .FontSize(18).Bold().FontColor("#3A3A3A");

                            col.Item().AlignCenter().Text("Proveedor: " + proveedor.Rows[0]["nombre"].ToString())
                                .FontSize(12).FontColor("#4F4F4F");
                        });

                        // Número de orden y fecha
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Container()
                                .Background("#006D77")  // Establecer el color de fondo
                                .Padding(5)              // Aplicar padding
                                .Text("Número de Orden: " + orden_de_compra.Rows[0]["id"].ToString())
                                .FontSize(12).FontColor("#ffffff").Bold();

                            col.Item().AlignCenter().Text("Fecha Orden: " + orden_de_compra.Rows[0]["fecha"].ToString())
                                .FontSize(12).FontColor("#4F4F4F");

                            col.Item().AlignCenter().Text("Fecha Entrega: " + orden_de_compra.Rows[0]["fecha_entrega_estimada"].ToString())
                                .FontSize(12).FontColor("#4F4F4F");
                        });
                    });

                    // Contenido de la orden de compra
                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        // Información del negocio
                        col.Item().Column(col_negocio =>
                        {
                            // —————— Título con subrayado continuo ——————
                            col_negocio.Item().Row(row =>
                            {
                                row.AutoItem()                             // ① el item se ajusta al ancho del texto
                                    .BorderBottom(1)                       // ② línea continua de 1 pt
                                    .BorderColor("#006D77")                //     mismo color que el texto
                                    .PaddingBottom(2)                      // ③ un poco de espacio antes de la línea
                                    .Text("Datos del Cliente")
                                        .Bold()
                                        .FontSize(14)
                                        .FontColor("#006D77");

                                row.RelativeItem();                        // ④ ocupa el resto del ancho sin interferir
                            });

                            // —————— Resto de campos ——————
                            col_negocio.Item().Text(txt =>
                            {
                                txt.Span("Negocio: ").SemiBold();
                                txt.Span(negocio.Rows[0]["nombre"].ToString());
                            });
                            col_negocio.Item().Text(txt =>
                            {
                                txt.Span("Dirección: ").SemiBold();
                                txt.Span(negocio.Rows[0]["direccion"].ToString());
                            });
                            col_negocio.Item().Text(txt =>
                            {
                                txt.Span("Teléfono: ").SemiBold();
                                txt.Span(negocio.Rows[0]["telefono"].ToString());
                            });
                        });

                        col.Item().LineHorizontal(1).LineColor("#E6E6E6");

                        // Tabla de detalles de la orden
                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Encabezado de la tabla
                            tabla.Header(header =>
                            {
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Producto").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Categoria").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Cantidad").Bold().FontSize(10);
                                });

                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Presentacion").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Precio Compra").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Sub Total").Bold().FontSize(10);
                                });
                            });

                            // Detalles de la orden
                            foreach (DataRow row in orden_de_compra_detalle.Rows)
                            {
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["producto"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["categoria"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["entrega"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(row["presentacion"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(formatCurrency(row["precio_compra"].ToString())).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(formatCurrency(row["sub_total_final"].ToString())).FontSize(10);
                                });
                            }
                        });

                        // Total
                        col.Item().AlignRight().PaddingTop(10).Text("TOTAL: " + formatCurrency(orden_de_compra.Rows[0]["total_final"].ToString()))
                            .Bold().FontSize(12).FontColor("#006D77");

                        // Espacio para firma
                        col.Item().PaddingTop(30).Column(firma =>
                        {
                            firma.Item().Text("Entrega conforme / firma: _____________________________").FontSize(10);
                            firma.Item().Text("Recibe conforme / firma: _____________________________").FontSize(10);
                        });
                    });

                    // Pie de página con numeración y logo pequeño
                    page.Footer().Row(row =>
                    {

                        // Numeración de páginas en el centro
                        row.RelativeItem().AlignCenter().Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                        // Coloca el logo pequeño a la derecha
                        row.ConstantItem(50).Container().AlignRight().Element(container =>
                        {
                            container.Image(logo).FitArea();
                        });
                    });

                });
            }).GeneratePdf(rutaDestino);
        }

        public void GenerarPDF_ResumenPedidos(string rutaDestino, byte[] logo, byte[] logo_negocio, DataTable negocio, DataTable resumen)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(document =>
            {
                document.Page(page =>
                {  
                    // tamaño A1 + orientación Landscape
                    page.Size(PageSizes.A4.Landscape());

                    page.Margin(30);

                    // Encabezado
                    page.Header().ShowOnce().Row(row =>
                    {
                        // Logo
                        row.ConstantItem(120).Container().AlignCenter().Element(container =>
                        {
                            container.Image(logo_negocio).FitArea();
                        });

                        // Información de la orden
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().AlignCenter().Text("Resumen de Pedido")
                                .FontSize(18).Bold().FontColor("#3A3A3A");

                            col.Item().AlignCenter().Text("Negocio: " + negocio.Rows[0]["nombre"].ToString())
                                .FontSize(12).FontColor("#4F4F4F");
                        });


                    });

                    // Contenido de la orden de compra
                    page.Content().PaddingVertical(20).Column(col =>
                    {

                        col.Item().LineHorizontal(1).LineColor("#E6E6E6");

                        // Tabla de detalles de la orden
                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                for (int columna = resumen.Columns["total"].Ordinal + 1; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    columns.RelativeColumn();
                                }
                            });

                            // Encabezado de la tabla
                            tabla.Header(header =>
                            {
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Total").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Producto").Bold().FontSize(10);
                                });
                                header.Cell().Element(container =>
                                {
                                    container.Background("#E6F4F4").Padding(8).AlignCenter().Text("Presentacion").Bold().FontSize(10);
                                });

                                for (int columna = resumen.Columns["total"].Ordinal + 1; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    string cliente = resumen.Columns[columna].ColumnName;
                                    header.Cell().Element(container =>
                                    {
                                        container.Background("#E6F4F4").Padding(8).AlignCenter().Text(cliente).Bold().FontSize(10);
                                    });
                                }
                            });
                            
                            // Detalles de la orden
                            for (int fila = 0; fila <= resumen.Rows.Count-1; fila++)
                            {
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(resumen.Rows[fila]["total"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(resumen.Rows[fila]["nombre_producto"].ToString()).FontSize(10);
                                });
                                tabla.Cell().Element(container =>
                                {
                                    container.Padding(8).Text(resumen.Rows[fila]["presentacion"].ToString()).FontSize(10);
                                });
                                for (int columna = resumen.Columns["total"].Ordinal + 1; columna <= resumen.Columns.Count - 1; columna++)
                                {
                                    string cliente = resumen.Columns[columna].ColumnName;
                                    tabla.Cell().Element(container =>
                                    {
                                        container.Padding(8).Text(resumen.Rows[fila][cliente].ToString()).FontSize(10);
                                    });
                                }
                            }
                         });


                        // Espacio para firma
                        col.Item().PaddingTop(30).Column(firma =>
                        {
                            firma.Item().Text("Entrega conforme / firma: _____________________________").FontSize(10);
                            firma.Item().Text("Recibe conforme / firma: _____________________________").FontSize(10);
                        });
                    });

                    // Pie de página con numeración y logo pequeño
                    page.Footer().Row(row =>
                    {

                        // Numeración de páginas en el centro
                        row.RelativeItem().AlignCenter().Text(text =>
                        {
                            text.Span("Página ");
                            text.CurrentPageNumber();
                            text.Span(" de ");
                            text.TotalPages();
                        });
                        // Coloca el logo pequeño a la derecha
                        row.ConstantItem(50).Container().AlignRight().Element(container =>
                        {
                            container.Image(logo).FitArea();
                        });
                    });

                });
            }).GeneratePdf(rutaDestino);
        }

    }
}
