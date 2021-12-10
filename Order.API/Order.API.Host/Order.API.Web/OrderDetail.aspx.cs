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
    public partial class OrderDetail : System.Web.UI.Page
    {
        public UserDTO UserMaster { get { return (UserDTO)HttpContext.Current.Session["Usuario"]; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (HttpContext.Current.Session["Usuario"] == null || Request.QueryString["orderId"] == null || Request.QueryString["name"] == null)
                    {
                        Response.Redirect("Login.aspx", true);
                    }
                    else
                    {
                        lblOrderName.Text = "Nombre de orden: " + Request.QueryString["name"];
                        hfOrderId.Value = Request.QueryString["orderId"];
                        HidePanels();
                        GetDetailByOrder();
                    }
                }
                lblError.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void GetDetailByOrder()
        {
            try
            {
                pnlOrderDetailList.Visible = true;
                var orderId = Convert.ToInt32(hfOrderId.Value);
                var request = new WishListDetailRequest
                {
                    User = new UserDTO { Identifier = UserMaster.Identifier },
                    WishList = new WishListDTO { Identifier = orderId }
                };
                var response = new OrderDetailExecute(UserMaster.Token).ExecuteGet(request, "order/user/detail");
                if (response != null && response.Success)
                {
                    var orders = response.Data.WishList.BookList;
                    rptOrderDetail.DataSource = orders;
                    rptOrderDetail.DataBind();
                }
                else
                {
                    lblError.Text = string.Join(",", response.Meta.Messages.Select(ms => ms.Text));
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        protected void ProductUpdated(object sender, EventArgs e)
        {
            try
            {
                RepeaterItem item = (sender as Button).NamingContainer as RepeaterItem;
                string productId = (item.FindControl("hfIdentifier") as HiddenField).Value;
                string messageSucess = "El libro con titulo: " + (item.FindControl("lblTitle") as Label).Text + " fue eliminido";
                var request = new WishListDetailRequest
                {
                    User = new UserDTO { Identifier = UserMaster.Identifier },
                    WishList = new WishListDTO
                    {
                        Identifier = Convert.ToInt32(hfOrderId.Value),
                        BookList = new List<BookDTO>
                    {
                       new BookDTO
                       {
                           Identifier = Convert.ToInt32(productId),
                           Status = 11
                       }
                    }
                    }
                };
                var response = new OrderDetailExecute(UserMaster.Token).ExecutePut(request, "order/detail/action/delete");
                if (response != null && response.Success)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + messageSucess + "');", true);
                    GetDetailByOrder();
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
            pnlOrderDetailList.Visible = false;
            pnlSearch.Visible = false;
        }

        /// <summary>
        /// Metodo para mostrar la imagen del libro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ProductDetail(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as Button).NamingContainer as RepeaterItem;
            string orderId = (item.FindControl("hfIdentifier") as HiddenField).Value;
            string name = (item.FindControl("lblOrderName") as Label).Text;
        }

        protected void OrderNew(object sender, EventArgs e)
        {
            HidePanels();
            pnlSearch.Visible = true;
            LimpiarControlesBusqueda();
            lnkNewButton.Visible = false;
        }

        protected void GoOrders(object sender, EventArgs e)
        {
            Response.Redirect("Order.aspx", true);

        }

        protected void SearchBook(object sender, EventArgs e)
        {
            try
            {
                var request = new BookFilterRequest
                {
                    User = new UserDTO { Identifier = UserMaster.Identifier, Token = UserMaster.Token },
                    Book = new BookDTO
                    {
                        Keyword = txtWordKey.Text,
                        Authors = new List<string> { txtAuthor.Text },
                        Title = txtTitle.Text,
                        Publisher = txtPublisher.Text
                    }
                };
                var response = new OrderDetailExecute(UserMaster.Token).ExecutePost(request, "order/detail/search");
                if (response != null && response.Success)
                {
                    rptNewBooks.DataSource = response.Data.BookList;
                    rptNewBooks.DataBind();
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

        protected void AddNewBook(object sender, EventArgs e)
        {
            try
            {
                RepeaterItem item = (sender as Button).NamingContainer as RepeaterItem;
                string title = (item.FindControl("lblTitle") as Label).Text;
                string authors = (item.FindControl("lblAuthors") as Label).Text;
                string externalId = (item.FindControl("lblIdExternal") as Label).Text;
                string publisher = (item.FindControl("lblPublisher") as Label).Text;

                string messageSucess = $"El libro con titulo: {title} fue agregado a";
                var request = new WishListDetailRequest
                {
                    User = new UserDTO { Identifier = UserMaster.Identifier },
                    WishList = new WishListDTO
                    {
                        Identifier = Convert.ToInt32(hfOrderId.Value),
                        BookList = new List<BookDTO>
                    {
                       new BookDTO
                       {
                           Authors = new List<string> { authors },
                           ExternalIdentifier = externalId,
                           Publisher = publisher,
                           Title = title,
                           Status = 10
                       }
                    }
                    }
                };

                var response = new OrderDetailExecute(UserMaster.Token).ExecutePost(request, "order/detail");
                if (response != null && response.Success)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + messageSucess + "');", true);
                    (item.FindControl("btnAddNewBook") as Button).Enabled = false;
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
        protected void GoBack(object sender, EventArgs e)
        {
            HidePanels();
            lnkNewButton.Visible = true;
            GetDetailByOrder();
        }

        private void LimpiarControlesBusqueda()
        {
            txtWordKey.Text = string.Empty;
            txtAuthor.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtPublisher.Text = string.Empty;
            rptNewBooks.DataSource = new List<BookDTO>();
            rptNewBooks.DataBind();
        }

    }
}