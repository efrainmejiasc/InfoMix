using System;
using System.Web.Services.Protocols;




namespace consumo
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TraceExtensionAttribute : SoapExtensionAttribute
    {

        private string myFilename = System.Web.HttpContext.Current.Server.MapPath(@"Log");
        private int myPriority;


        // Set the name of the log file were SOAP messages will be stored. 
        public TraceExtensionAttribute()
            : base()
        {
            
            //myFilename = "C:\\logClient.txt";
            myFilename = System.Web.HttpContext.Current.Server.MapPath(@"Log\logClient.txt");
        }

        // Return the type of 'TraceExtension' class. 
        public override Type ExtensionType
        {
            get
            {
                return typeof(TraceExtension);
            }
        }

        // User can set priority of the 'SoapExtension'. 
        public override int Priority
        {
            get
            {
                return myPriority;
            }
            set
            {
                myPriority = value;
            }
        }

        public string Filename
        {
            get
            {
                return myFilename;
            }
            set
            {
                myFilename = value;
            }
        }
    }
}
