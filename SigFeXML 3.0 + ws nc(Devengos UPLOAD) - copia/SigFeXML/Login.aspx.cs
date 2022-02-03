using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigFeXML
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {            
                this.Form.DefaultButton = btnIngresar.UniqueID;
            }          
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (txtPass.Text != string.Empty && txtUser.Text != string.Empty)
            {

                bool res = false;
                string Usuario = "";
                string Pass = "";

                Usuario = txtUser.Text;
                Pass = txtPass.Text;


                if (Usuario == "PruebaXml" && Pass == "1234")
                {
                    Session.Add("UsuarioLogueado", Usuario);
                    res = true;
                }
                else if (Usuario == "PruebaXml2" && Pass == "1234")
                {
                    Session.Add("UsuarioLogueado", Usuario);
                    res = true;
                }
                else if (Usuario == "Usua_1604001" && Pass == "usuario1234")
                {
                    Session.Add("UsuarioLogueado", Usuario);
                    res = true;
                }
                else if (Usuario == "cmunozca" && Pass == "andre8522")
                {
                    Session.Add("UsuarioLogueado", Usuario);
                    res = true;
                }                

                if (res)
                {
                    Response.Redirect("XML.aspx");
                }
                else
                {
                    lblMensaje.Text = "Error con las credenciales, revisar";
                    PopupMensaje.Show();
                }
            }
            else
            {
                if (txtUser.Text == string.Empty)
                {
                    lblMensaje.Text = "Debe ingresar usuario";
                    PopupMensaje.Show();
                }
                else if(txtPass.Text == string.Empty)
                {
                    lblMensaje.Text = "Debe ingresar contraseña";
                    PopupMensaje.Show();
                }            
            
            }

        }

        protected void BtnPAceptar_Click(object sender, EventArgs e)
        {
            PopupMensaje.Hide();
        }
    }
}