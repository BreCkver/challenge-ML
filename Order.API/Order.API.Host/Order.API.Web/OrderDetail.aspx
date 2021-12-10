<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderDetail.aspx.cs" Inherits="Order.API.Web.OrderDetail" %>

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
        <asp:Label ID="lblError" CssClass="textCenter" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblOrderName" CssClass="textCenter" runat="server"></asp:Label><br />
    <asp:Panel id="pnlHeader" runat="server">
        <br />
        <asp:LinkButton ID="lnkNewButton" CssClass="textCenter" runat="server" OnClick="OrderNew">Agregar Libro</asp:LinkButton>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lnkOrdersGo" CssClass="textCenter" runat="server" OnClick="GoOrders">Regresar</asp:LinkButton>

    </asp:Panel>
        
    </div>
    <br />
    <asp:Panel runat="server" ID="pnlOrderDetailList">
        <table id="tblOrderDetail" class="grid">
            <thead>
                <tr>
                    <th class="textCenter">Titulo</th>
                    <th class="textCenter">Autor</th>
                    <th class="textCenter">Identificador Externo</th>
                    <th class="textCenter">Editorial</th>
                    <th class="textCenter">Acci&oacute;n</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptOrderDetail">
                    <ItemTemplate>
                        <tr>
                            <asp:HiddenField runat="server" Value='<%#Eval("Identifier")%>' ID="hfIdentifier" />
                            <td>
                                <asp:Label runat="server" ID="lblTitle" Text='<%#Eval("Title")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblAuthors" Text='<%#Eval("Author") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblIdExternal" Text='<%#Eval("ExternalIdentifier")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblPublisher" Text='<%#Eval("Publisher")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Button Text="Ver Detalle" runat="server" OnClick="ProductDetail" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button Text="Eliminar" runat="server" OnClick="ProductUpdated" />

                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlSearch">
        <div class="textCenter">
            <span>Palabra Clave:</span>
            <asp:TextBox ID="txtWordKey" runat="server"></asp:TextBox>
            <br />
            <span>Titulo: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
            <br />
            <span>Author: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <asp:TextBox ID="txtAuthor" runat="server"></asp:TextBox>
            <br />
            <span>Editorial: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <asp:TextBox ID="txtPublisher" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnBusar" runat="server" Text="Buscar" OnClick="SearchBook" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnRegresarDetail" runat="server" Text="Regresar" OnClick="GoBack" />

            <table id="NewBooks" class="grid">
                <thead>
                    <tr>
                        <th class="textCenter">Titulo</th>
                        <th class="textCenter">Autor</th>
                        <th class="textCenter">Identificador Externo</th>
                        <th class="textCenter">Editorial</th>
                        <th class="textCenter">Acci&oacute;n</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rptNewBooks">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblTitle" Text='<%#Eval("Title")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblAuthors" Text='<%#Eval("Author") %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblIdExternal" Text='<%#Eval("ExternalIdentifier")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblPublisher" Text='<%#Eval("Publisher")%>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Button Text="Agregar" ID="btnAddNewBook" runat="server" OnClick="AddNewBook" />

                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </asp:Panel>

</asp:Content>
