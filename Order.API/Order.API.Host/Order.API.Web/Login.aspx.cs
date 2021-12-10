using Newtonsoft.Json;
using Order.API.Web.Business;
using Order.API.Web.Entities;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Order.API.Web
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    pnlAuth.Visible = true;
                    pnlRegistro.Visible = false;
                }
                lblError.Text = string.Empty;
                lblErrorNewUser.Text = string.Empty;

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void ValidateUser(object sender, EventArgs e)
        {
            var userDTO = new UserDTO
            {
                UserName = txtUser.Text,
                Password = txtPassword.Text
            };
            try
            {
                var response = new UserExecute().ExecutePost(userDTO, "user/authenticate");
                if (response != null && response.Success)
                {
                    Session.Add("Usuario", response.Data.User);
                    Response.Redirect("Order.aspx", true);
                }
                else
                {
                    lblError.Text = string.Join(",", response.Meta.Messages?.Select(ms => ms.Text));
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void NewUser(object sender, EventArgs e)
        {
            var userDTO = new UserDTO
            {
                UserName = txtNewUsuario.Text,
                Password = txtNewPassword.Text,
                PasswordConfirm = txtNewPasswordConfirm.Text,
                StatusIdentifier = 1
            };
            try
            {
                var response = new UserExecute().ExecutePost(userDTO, "user");
                if (response != null && response.Success)
                {
                    txtUser.Text = userDTO.UserName;
                    pnlAuth.Visible = true;
                    pnlRegistro.Visible = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Se registro correctamente el usuario: {userDTO.UserName}');", true);
                }
                else
                {
                    lblErrorNewUser.Text = string.Join(",", response.Meta.Messages?.Select(ms => ms.Text));
                }

            }
            catch (Exception ex)
            {
                lblErrorNewUser.Text = ex.Message;
            }
        }

        protected void RegisterEnabled(object sender, EventArgs e)
        {
            pnlAuth.Visible = false;
            pnlRegistro.Visible = true;
        }

        protected void GoBack(object sender, EventArgs e)
        {
            pnlAuth.Visible = true;
            pnlRegistro.Visible = false;
        }
    }
}