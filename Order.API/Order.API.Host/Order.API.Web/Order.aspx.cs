using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Order.API.Web.Business;
using Order.API.Web.Entities;

namespace Order.API.Web
{
    public partial class Order : System.Web.UI.Page
    {
        public UserDTO UserMaster { get { return (UserDTO)HttpContext.Current.Session["Usuario"]; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (HttpContext.Current.Session["Usuario"] == null)
                    {
                        Response.Redirect("About.aspx", true);
                    }
                    else
                    {
                        HidePanels();
                        GetOrdersByUser();
                    }
                }
                lblError.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }            
        }

        private void GetOrdersByUser()
        {
            try
            {
                pnlOrderList.Visible = true;
                var request = new WishListRequest
                {
                    User = new UserDTO { Identifier = UserMaster.Identifier },
                };
                var response = new OrderExecute(UserMaster.Token).ExecuteGet(request, "order/user");
                if (response != null && response.Success)
                {
                    var orders = response.Data.WishLists;
                    rptOrders.DataSource = orders;
                    rptOrders.DataBind();
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

        protected void OrderUpdated(object sender, EventArgs e)
        {
            try 
            {
                RepeaterItem item = (sender as Button).NamingContainer as RepeaterItem;
                string orderId = (item.FindControl("hfIdentifier") as HiddenField).Value;
                string messageSucess = "La orden: " + (item.FindControl("lblOrderName") as Label).Text + " fue eliminida";
                var request = new WishListRequest
                {
                    User = new UserDTO { Identifier = UserMaster.Identifier },
                    WishList = new OrderDTO { Identifier = Convert.ToInt32(orderId), Status = 3 }
                };
                var response = new OrderExecute(UserMaster.Token).ExecutePut(request, "order/action/delete");
                if (response != null && response.Success)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + messageSucess + "');", true);
                    GetOrdersByUser();
                }
                else
                {
                    lblError.Text = string.Join(",", response.Meta.Messages?.Select(ms => ms.Text));
                }
            }
            catch(Exception ex)
            {
                lblError.Text = ex.Message;
            }            
        }
        protected void CrearOrder(object sender, EventArgs e)
        {
            var request = new WishListRequest
            {
                User = new UserDTO { Identifier = UserMaster.Identifier },
                WishList = new OrderDTO { Name = txtNewOrder.Text, Status = 1 }
            };
            try
            {
                var response = new OrderExecute(UserMaster.Token).ExecutePost(request, "order");
                if (response != null && response.Success)
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Se creo correctamente el wishlist: {txtNewOrder.Text}');", true);
                    txtNewOrder.Text = string.Empty;
                    lblWishListNew.Visible = true;
                    HidePanels();
                    GetOrdersByUser();
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

        private void HidePanels()
        {
            pnlOrderNew.Visible = false;
            pnlOrderList.Visible = false;
        }

        protected void OrderNew(object sender, EventArgs e)
        {
            HidePanels();
            lblWishListNew.Visible = false;
            pnlOrderNew.Visible = true;
        }

        protected void ViewDetail(object sender, EventArgs e)
        {
            try
            {
                RepeaterItem item = (sender as Button).NamingContainer as RepeaterItem;
                string orderId = (item.FindControl("hfIdentifier") as HiddenField).Value;
                string name = (item.FindControl("lblOrderName") as Label).Text;
                Response.Redirect($"OrderDetail.aspx?orderId={orderId}&name={name}", true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}