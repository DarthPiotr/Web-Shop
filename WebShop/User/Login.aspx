<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebShop.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Logowanie</h1>
    </div>
    <div class="content">
        <div class="form-group">
            <asp:Label ID="labEmail" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox Cssclass="form-control" ID="tbEmail" runat="server" TextMode="Email"></asp:TextBox>
            <span runat="server" id="checkEmail" />
            <label runat="server" id="labCheckEmail" />
        </div>
        <div class="form-group">
            <asp:Label ID="labPasswd" runat="server" Text="Hasło:"></asp:Label>
            <asp:TextBox Cssclass="form-control" ID="tbPasswd" runat="server" TextMode="Password"></asp:TextBox>
            <span runat="server" id="checkPasswd" />
            <label runat="server" id="labCheckPasswd" />
        </div>
        <div class="form-group">
            <asp:Button Cssclass="btn btn-default" ID="btnRegister" runat="server" Text="Zaloguj!" OnClick="BtnRegister_Click" />
        </div>
        <div class="form-group">
            <a href="ChangePassword.aspx"><span class="glyphicon glyphicon-question-sign"></span>Zapomniałeś hasła?</a>
        </div>
    </div>

    <asp:SqlDataSource ID="sqlGetUserInfo" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT * FROM [Users] WHERE ([Email] = @Email)">
        <SelectParameters>
            <asp:ControlParameter ControlID="tbEmail" Name="Email" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
