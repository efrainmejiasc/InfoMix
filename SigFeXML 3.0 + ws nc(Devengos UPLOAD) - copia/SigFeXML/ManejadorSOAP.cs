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
        Stream _oldStream;
        Stream _newStream;
        string _filename;

        private static XmlDocument _xmlRequest;
        /// <summary>
        /// Gets the outgoing XML request sent to PayPal
        /// </summary>
        public static XmlDocument XmlRequest
        {
            get { return _xmlRequest; }
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
            _oldStream = stream;
            _newStream = new MemoryStream();
            return _newStream;
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
        public override object GetInitializer(Type webServiceType)
        {
           
            //string ruta = @"C:\Users\pto\Desktop";
            string ruta = System.Web.HttpContext.Current.Server.MapPath(@"Log\");
            //return ruta + WebServiceType.FullName +".log";
            return ruta + webServiceType.FullName + ".log"; 
        }

        // Receive the file name stored by GetInitializer and store it in a
        // member variable for this specific instance.
        public override void Initialize(object initializer)
        {
            _filename = (string)initializer;
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

                        _xmlRequest = GetSoapEnvelope(_newStream);
 
                        string xmlStr = XmlRequest.InnerXml;
                        string usuario = System.Configuration.ConfigurationManager.AppSettings["usuarioSoap"].ToString();
                        string clave = System.Configuration.ConfigurationManager.AppSettings["claveSoap"].ToString();

                        string header = string.Empty;

                        header = "<soap:Header xmlns:wsa=\"http://www.w3.org/2005/08/addressing\">";
                        header += "<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">";
                        header += "<wsse:UsernameToken wsu:Id=\"UsernameToken-1\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">";
                        header += String.Format("<wsse:Username>{0}</wsse:Username>", usuario);
                        header += String.Format("<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{0}</wsse:Password>", clave);
                        header += "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">cJ1qRr/wR1tCqAjxHm7URg==</wsse:Nonce>";
                        header += "<wsu:Created>2014-01-02T14:49:03.135Z</wsu:Created></wsse:UsernameToken></wsse:Security>";
                        header += "<wsa:Action>http://dipres.gob.cl/ejecucion/devengo/servicio/1/0/Devengo/registrarDevengosRequest</wsa:Action>";
                        header += "<wsa:MessageID>56778601</wsa:MessageID></soap:Header><soap:Body>";

                        xmlStr = xmlStr.Replace("<soap:Body>", header);

                        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr));
                       
                        _newStream = stream;
                    WriteOutput(message);

                    break;
                case SoapMessageStage.BeforeDeserialize:                
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
                _newStream.Position = 0;
                FileStream fs = new FileStream(_filename, FileMode.Append,FileAccess.Write);
                StreamWriter w = new StreamWriter(fs);

                string soapString =(message is SoapServerMessage) ? "SoapResponse" : "SoapRequest"; 
                w.WriteLine("-----" + soapString + " at " + DateTime.Now);
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
                Copy(_newStream, fs);
                w.Close();
                _newStream.Position = 0;
                Copy(_newStream, _oldStream);
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
                    Copy(_oldStream, _newStream);
                    FileStream fs = new FileStream(_filename, FileMode.Append,
                                                   FileAccess.Write);
                    StreamWriter w = new StreamWriter(fs);
                    //w.WriteLine("");
                    //w.WriteLine("");
                    //w.WriteLine("");
                    //w.WriteLine("###########################################################################");
                    string soapString = (message is SoapServerMessage) ? "SoapRequest" : "SoapResponse";
                    w.WriteLine("-----" + soapString + " at " + DateTime.Now);
                    w.Flush();
                    _newStream.Position = 0;
                    Copy(_newStream, fs);
                    w.Close();
                    _newStream.Position = 0;
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

        public string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }


    }


}

