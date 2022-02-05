using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Negocio;
using Datos;

namespace RegistroTransacciones
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.DefaultButton = btnIngresar.UniqueID;

            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (txtPass.Text != string.Empty)
            {
                bool res = false;
                //string Usuario = "";
                //string Pass = "";
                DataSet ds = new DataSet();
                Bsn_Usuario bsn = new Bsn_Usuario();
                Dto_Usuario dto = new Dto_Usuario();
                //Usuario = txtUser.Text;
                try
                {
                     Response.Redirect("Default.aspx");

                    ds = bsn.BuscarUsuario(txtPass.Text.Trim().ToUpper());


                    if (ds != null && ds.Tables[0].Columns.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        dto.Pass = ds.Tables[0].Rows[0]["password"].ToString().ToLower().Trim();

                        if (dto.Pass == txtPass.Text.Trim().ToLower())
                        {
                            dto.User = ds.Tables[0].Rows[0]["nombre_user"].ToString();
                            dto.Rut = int.Parse(ds.Tables[0].Rows[0]["rut_user"].ToString());
                            dto.Dv = ds.Tables[0].Rows[0]["dv_user"].ToString();
                            dto.Apellido = ds.Tables[0].Rows[0]["apellido_user"].ToString();
                            dto.Iniciales = ds.Tables[0].Rows[0]["ini_user"].ToString();
                            dto.Cargo = ds.Tables[0].Rows[0]["cargo_user"].ToString();

                            res = true;
                        }

                        if (res)
                        {
                            Session.Add("UsuarioLogueado", dto);
                            Session.Timeout = 120;
                            Response.Redirect("Default.aspx");
                            //Response.Redirect("Cierre_Diario.aspx");
                        }
                        else
                        {

                            PopupMensaje.Show();
                            lblMensaje.Text = "Error en datos ingresados";
                        }
                    }
                    else
                    {
                        PopupMensaje.Show();
                        lblMensaje.Text = "Clave incorrecta";
                    }
                }
                catch (Exception ex)
                {

                    PopupMensaje.Show();
                    lblMensaje.Text = "Error: "+ex.ToString();
                }

                //if (Usuario == "Prueba" && Pass == "1234")
                //{
                //    Session.Add("UsuarioLogueado", Usuario);
                //    res = true;
                //}

                //if (res)
                //{
                //    Response.Redirect("Default.aspx");
                //}
                //else
                //{
                //    lblMensaje.Text = "Error con las credenciales, revisar";
                //    PopupMensaje.Show();
                //}
            }
            else
            {
                //if (txtUser.Text == string.Empty)
                //{
                //    lblMensaje.Text = "Debe ingresar usuario";
                //    PopupMensaje.Show();
                //}
                //else if (txtPass.Text == string.Empty)
                //{
                    lblMensaje.Text = "Debe ingresar contraseña";
                    PopupMensaje.Show();
                //}
            }
        }

        protected void BtnPAceptar_Click(object sender, EventArgs e)
        {
            PopupMensaje.Hide();
        }
    }
}