<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Order.API.Web.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .textCenter {
            text-align: center !important;
        }

        .div textLeft {
            text-align: center !important;
        }
    </style>
    <h2><%: Title %>.</h2>
    <h3>Order System, Challenge ML</h3>
    <p>Login.</p>
    <asp:Panel runat="server" ID="pnlAuth">
        <div class="textCenter">
            <div class="textLeft">
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="RegisterEnabled">Nuevo Usuario</asp:LinkButton>
                <br />
                <p>Usuario: </p>
                <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                <br />
                <p>Contraseña: </p>
                <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                <br /><asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="ValidateUser" />
                <br />
                <br />
                <asp:Label ID="lblError" runat="server"></asp:Label>

                <br />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlRegistro">
        <div class="textCenter">
            <br />
            <p>Usuario: </p>
            <asp:TextBox ID="txtNewUsuario" runat="server"></asp:TextBox>
            <br />
            <p>Contraseña: </p>
            <asp:TextBox ID="txtNewPassword" TextMode="Password" runat="server"></asp:TextBox>
            <br />
            <p>Confirme su contraseña: </p>
            <asp:TextBox ID="txtNewPasswordConfirm" TextMode="Password" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnNuevo" runat="server" Text="Registrar" OnClick="NewUser" />
            <asp:Button ID="btnBackLogin" runat="server" Text="Regresar" OnClick="GoBack" />
            <br />
            <br />
            <asp:Label ID="lblErrorNewUser" runat="server"></asp:Label>
            <br />
        </div>
    </asp:Panel>
</asp:Content>

