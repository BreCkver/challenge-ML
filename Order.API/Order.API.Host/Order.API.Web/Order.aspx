<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="Order.API.Web.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .textCenter {
            text-align: center !important;
        }

        table.grid {
            margin: auto;
            width: 600px;
            border-collapse: collapse;
            border: 1px solid #fff; /*for older IE*/
            border-style: hidden;
        }

            table.grid thead th {
                padding: 8px;
                background-color: #fde9d9;
                font-size: large;
            }

            table.grid th, table.grid td {
                padding: 3px;
                border-width: 1px;
                border-style: solid;
                border-color: #f79646 #ccc;
            }

            table.grid td {
                text-align: left;
            }
    </style>

    <br />
    <br />
    <asp:HiddenField runat="server" Value="" ID="hfOrderId" />
    <div class="textCenter">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <br />
        <asp:LinkButton ID="lblWishListNew" runat="server" OnClick="OrderNew">Nueva WishList</asp:LinkButton>
        <br />
        <br />
    </div>
    <asp:Panel runat="server" ID="pnlOrderList">
        <table class="grid">
            <thead>
                <tr>
                    <th class="textCenter">Nombre</th>
                    <th class="textCenter">Acci&oacute;n</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptOrders">
                    <ItemTemplate>
                        <tr>
                            <asp:HiddenField runat="server" Value='<%#Eval("Identifier")%>' ID="hfIdentifier" />
                            <td>
                                <asp:Label runat="server" ID="lblOrderName" Text='<%#Eval("Name")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Button Text="Eliminar" runat="server" OnClick="OrderUpdated" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button Text="Ver Detalle" runat="server" OnClick="ViewDetail" />
                            </td>
                        </tr>

                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlOrderNew">
        <div class="textCenter">
            <br />
            <p>Nombre: </p>
            <asp:TextBox ID="txtNewOrder" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnCrear" runat="server" Text="Crear" OnClick="CrearOrder" />
            <br />
        </div>
    </asp:Panel>
</asp:Content>
