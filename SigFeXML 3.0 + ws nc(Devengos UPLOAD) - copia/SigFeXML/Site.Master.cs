using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigFeXML
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lbnSalir_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void lbnMantenedor_Click(object sender, EventArgs e)
        {
            Response.Redirect("Man_Requerimientos.aspx");
        }

        protected void lbnXML_Click(object sender, EventArgs e)
        {
            Response.Redirect("XML.aspx");
        }
    }
}
