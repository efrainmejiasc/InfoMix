using System;
using System.IO;
using System.Web.Services.Protocols;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace consumo
{
    
    public class TraceExtension : SoapExtension
    {
        Stream oldStream;
        Stream newStream;
        string filename;

        private static XmlDocument xmlRequest;
        /// <summary>
        /// Gets the outgoing XML request sent to PayPal
        /// </summary>
        public static XmlDocument XmlRequest
        {
            get { return xmlRequest; }
        }

        private static XmlDocument xmlResponse;
        /// <summary>
        /// Gets the incoming XML response sent from PayPal
        /// </summary>
        public static XmlDocument XmlResponse
        {
            get { return xmlResponse; }
           
        }


        // Save the Stream representing the SOAP request or SOAP response into
        // a local memory buffer.
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        // When the SOAP extension is accessed for the first time, the XML Web
        // service method it is applied to is accessed to store the file
        // name passed in, using the corresponding SoapExtensionAttribute.
        
        public override object GetInitializer(LogicalMethodInfo methodInfo,
                                           SoapExtensionAttribute attribute)
        {
            return ((TraceExtensionAttribute)attribute).Filename;
            //return "";
        }

        // The SOAP extension was configured to run using a configuration file
        // instead of an attribute applied to a specific XML Web service
        // method.
        public override object GetInitializer(Type WebServiceType)
        {
           
            //string ruta = @"C:\";
            string ruta = System.Web.HttpContext.Current.Server.MapPath(@"Log\");
            string fecha = string.Format("{0:dd-MM-yyyy}", DateTime.Now);
            //return ruta + WebServiceType.FullName +".log";
            return String.Format("{0}{1}{2}.log", ruta, WebServiceType.FullName,fecha); 
        }

        // Receive the file name stored by GetInitializer and store it in a
        // member variable for this specific instance.
        public override void Initialize(object initializer)
        {
            filename = (string)initializer;
        }

        //  If the SoapMessageStage is such that the SoapRequest or
        //  SoapResponse is still in the SOAP format to be sent or received,
        //  save it out to a file.
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:

                    break;
                case SoapMessageStage.AfterSerialize:

                    xmlRequest = GetSoapEnvelope(newStream);

                    string xmlStr = XmlRequest.InnerXml;

                    string usuario = System.Configuration.ConfigurationManager.AppSettings["usuarioSoap"];
                    string clave = System.Configuration.ConfigurationManager.AppSettings["claveSoap"];
                    string header = string.Empty;

                    if (xmlStr.IndexOf("SolicitudDeEstadoDeTransacciones", StringComparison.Ordinal) >= 0)
                    {
                        header = "<soap:Header xmlns:wsa=\"http://www.w3.org/2005/08/addressing\">";
                        header += "<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">";
                        header += "<wsse:UsernameToken wsu:Id=\"UsernameToken-6\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">";
                        header += String.Format("<wsse:Username>{0}</wsse:Username>", usuario);
                        header += String.Format("<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{0}</wsse:Password>", clave);
                        header += "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">G6CESHhP3gViT7AsaaFKPQ==</wsse:Nonce>";
                        header += "<wsu:Created>2014-06-03T14:28:31.280Z</wsu:Created></wsse:UsernameToken></wsse:Security>";
                        header += "<wsa:Action>http://dipres.gob.cl/ejecucion/comun/estadoDeTransaccion/servicio/1/0/EstadoDeTransaccion/obtenerEstadoDeTransaccionesRequest</wsa:Action>";
                        header += "<wsa:MessageID>75640248</wsa:MessageID></soap:Header><soap:Body>";
                    }
                    else if (xmlStr.IndexOf("SolicitudDeObtencionDeTransaccionContable", StringComparison.Ordinal) >= 0)
                    {
                        header = "<soap:Header xmlns:wsa=\"http://www.w3.org/2005/08/addressing\">";
                        header += "<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">";
                        header += "<wsse:UsernameToken wsu:Id=\"UsernameToken-3\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">";
                        header += String.Format("<wsse:Username>{0}</wsse:Username>", usuario);
                        header += String.Format("<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{0}</wsse:Password>", clave);
                        header += "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">3qRG9XXbpd3R7ff45t+1ZA==</wsse:Nonce>";
                        header += "<wsu:Created>2014-06-05T16:02:48.025Z</wsu:Created></wsse:UsernameToken></wsse:Security>";
                        header += "<wsa:Action>http://dipres.gob.cl/ejecucion/contabilidad/servicio/1/0/Contabilidad/registrarTransaccionesContablesRequest</wsa:Action>";
                        header += "<wsa:MessageID>615282102</wsa:MessageID></soap:Header><soap:Body>";
                    }
                    else
                    {
                        string fecha = "";
                        //fecha = string.Format("{0:yy-MM-dd hh:mm:ss}", DateTime.Now); //fecha actual para id de envío
                        //fecha = fecha.Replace("-", "").Replace(":", "").Replace(" ", "").Trim();
                        string Id = "";//fecha + aleatorio;

                        int posini = xmlStr.LastIndexOf("<id xmlns=\"http://dipres.gob.cl/ejecucion/contabilidad/esquema/1/0/\">", StringComparison.Ordinal);
                       
                        for (int i = (posini + 69); xmlStr.Length > i; i++)
                        {
                            if (xmlStr[i] == '<')
                            {
                                break;
                            }
                            else
                            {
                                Id = Id + xmlStr[i];
                            }
                        }

                        posini = xmlStr.LastIndexOf("<titulo xmlns=\"http://dipres.gob.cl/ejecucion/contabilidad/esquema/1/0/\">", StringComparison.Ordinal);

                        for (int i = (posini + 76); xmlStr.Length > i; i++)
                        {
                            if (xmlStr[i] == '_')
                            {
                                break;
                            }
                            else
                            {
                                //if (xmlStr[i] != '-')
                                //{
                                    fecha = fecha + xmlStr[i];
                                //}
                            }
                        }

                        try
                        {
                            fecha = string.Format("{0:yyyy-MM-dd}", DateTime.Parse(fecha)).Replace("-", "");
                        }
                        catch (Exception)
                        {
                            fecha = fecha.Replace("-", "");
                        }
                        Random r = new Random();
                        string aleatorio = r.Next(99).ToString();
                        Id =aleatorio + fecha + Id;

                        header = "<soap:Header xmlns:wsa=\"http://www.w3.org/2005/08/addressing\">";
                        header +="<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">";
                        header +="<wsse:UsernameToken wsu:Id=\"UsernameToken-3\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">";
                        header += String.Format("<wsse:Username>{0}</wsse:Username>", usuario);
                        header +=String.Format("<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{0}</wsse:Password>",clave);
                        header +="<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">3qRG9XXbpd3R7ff45t+1ZA==</wsse:Nonce>";
                        header +="<wsu:Created>2014-06-05T16:02:48.025Z</wsu:Created></wsse:UsernameToken></wsse:Security>";
                        header +="<wsa:Action>http://dipres.gob.cl/ejecucion/contabilidad/servicio/1/0/Contabilidad/registrarTransaccionesContablesRequest</wsa:Action>";
                        //header += "<wsa:MessageID>615282106</wsa:MessageID></soap:Header><soap:Body>";
                        header += String.Format("<wsa:MessageID>{0}</wsa:MessageID></soap:Header><soap:Body>", Id);
                    }

                    xmlStr = xmlStr.Replace("<soap:Body>", header);
                   
                    var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr));

                    newStream = stream;
                    WriteOutput(message);

                    break;
                case SoapMessageStage.BeforeDeserialize:
                   
                    /*CODIGO PARA MODIFICAR LA RESPUESTA ENTREGADA POR EL WS*/
                    //Copy(oldStream, newStream);
                    //xmlResponse = GetSoapEnvelope(newStream);
                    
                    //string xmlStrResponse = xmlResponse.InnerXml;

                    //if (xmlStrResponse.IndexOf("RespuestaDeEstadoDeTransacciones", StringComparison.Ordinal) >= 0)
                    //{
                    //    xmlStrResponse = xmlStrResponse.Replace("<ns0:folio>No existe Folio</ns0:folio>", "<ns0:folio>0</ns0:folio>");
                    //    xmlStrResponse = xmlStrResponse.Replace("<ns0:id>No existe Identificador de Transferencia</ns0:id>", "<ns0:id>0</ns0:id>");
                    //}

                    //var streammod = new MemoryStream(Encoding.UTF8.GetBytes(xmlStrResponse));
                    
                    //oldStream = streammod;
                    WriteInput(message);

                    break;
                case SoapMessageStage.AfterDeserialize:


                    break;
                default:
                    throw new Exception("invalid stage");
            }
        }
      

        public void WriteOutput(SoapMessage message)
        {
            try
            {
                newStream.Position = 0;
                FileStream fs = new FileStream(filename, FileMode.Append,FileAccess.Write);
                StreamWriter w = new StreamWriter(fs);

                string soapString =(message is SoapServerMessage) ? "SoapResponse" : "SoapRequest";
                w.WriteLine(String.Format("-----{0} at {1}", soapString, DateTime.Now));
                //w.Flush();
                //Copy(newStream, fs);
                //w.WriteLine("***************************************************************************");
                //w.WriteLine("");
                //w.WriteLine("");
                //w.WriteLine("");
                //w.Close();
                //newStream.Position = 0;
                //Copy(newStream, oldStream);
                w.Flush();
                Copy(newStream, fs);
                w.Close();
                newStream.Position = 0;
                Copy(newStream, oldStream);
            }
            catch (Exception)
            {
               
            }
        }

        public void WriteInput(SoapMessage message)
        {
            try
            {
                if (message != null)
                {
                    Copy(oldStream, newStream);
                    FileStream fs = new FileStream(filename, FileMode.Append,
                                                   FileAccess.Write);
                    StreamWriter w = new StreamWriter(fs);
                    //w.WriteLine("");
                    //w.WriteLine("");
                    //w.WriteLine("");
                    //w.WriteLine("###########################################################################");
                    string soapString = (message is SoapServerMessage) ? "SoapRequest" : "SoapResponse";
                    w.WriteLine("-----" + soapString + " at " + DateTime.Now);
                    w.Flush();
                    newStream.Position = 0;
                    Copy(newStream, fs);
                    w.Close();
                    newStream.Position = 0;
                }
            }
            catch (Exception) { 
            
            }
        }

        private XmlDocument GetSoapEnvelope(Stream stream)
        {
            XmlDocument xml = new XmlDocument();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            xml.LoadXml(reader.ReadToEnd());
            stream.Position = 0;
            return xml;
        }

        void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }

        //private void CopyStream(Stream from, Stream to)
        //{
        //    TextReader reader = new StreamReader(from);
        //    TextWriter writer = new StreamWriter(to);
        //    writer.Write(reader.ReadToEnd());
        //    writer.Flush();
        //}


        public string Filename
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
            }
        }


    }


}

