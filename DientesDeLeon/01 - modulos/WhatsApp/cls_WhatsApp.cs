using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;


namespace _01___modulos.WhatsApp
{
    public class cls_WhatsApp
    {
        // Coloca aquí tus credenciales reales de Twilio
        private const string accountSid = "AC482204a2cf675ba0811fb649771ecd2e";     // ejemplo: ACxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        private const string authToken = "b3849433cae2316ce974044e1d81f6e5";       // ejemplo: yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy

        // Este número debe estar habilitado por Meta y aprobado en Twilio para uso comercial
        private const string fromNumber = "whatsapp:+5491134060253";

        public void EnviarMensaje(string numeroDestino, string mensaje)
        {
            try
            {
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    from: new PhoneNumber(fromNumber),
                    to: new PhoneNumber($"whatsapp:{numeroDestino}"),
                    body: mensaje
                );

                Console.WriteLine($"Mensaje enviado correctamente. SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
            }
        }

        public async Task enviarPedidoAsync(string proveedor, string cliente, string id, string telefono)
        {
            try
            {
                // Inicializar Twilio con las credenciales
                TwilioClient.Init(accountSid, authToken);

                // SID de la plantilla aprobada en Twilio
                string contentSid = "HX951456b48eff8f133896f35974afeba2"; // Reemplaza con el SID de tu plantilla aprobada

                // Crear los parámetros que se usarán en la plantilla
                var contentVariables = new Dictionary<string, object>()
        {
            { "1", proveedor },
            { "2", cliente },
            { "3", id }
        };

                // Enviar el mensaje usando la plantilla de forma asincrónica
                var message = await MessageResource.CreateAsync(
                    contentSid: contentSid,
                    to: new Twilio.Types.PhoneNumber($"whatsapp:{telefono}"),
                    from: new Twilio.Types.PhoneNumber(fromNumber),
                    contentVariables: JsonConvert.SerializeObject(contentVariables, Formatting.Indented)
                );

                // Imprimir la respuesta
                Console.WriteLine($"Mensaje enviado correctamente. SID: {message.Sid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
            }
        }

    }
}
