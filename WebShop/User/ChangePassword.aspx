<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="WebShop.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Zmiana hasła</h1>
    </div>
    <div class="content>">
        <%if (WebShop.Utils.GetUser(Session) == null)
            { %>
        <div class="form-group">
            <asp:Label ID="labName" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbEmail" runat="server" TextMode="Email"></asp:TextBox>
            <span runat="server" id="checkEmail" />
            <label runat="server" id="labCheckEmail" />
        </div>

        <%}
            else
            {%>
        <div class="form-group">
            <div class="alert alert-warning" runat="server" id="header"></div>
        </div>

        <%} %>

        <div class="form-group">
            <asp:Label ID="labPasswd1" runat="server" Text="Nowe hasło:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbPasswd1" runat="server" TextMode="Password"></asp:TextBox>
            <span runat="server" id="checkPasswd1" />
        </div>
        <div class="form-group">
            <asp:Label ID="labPasswd2" runat="server" Text="Powtórz hasło:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbPasswd2" runat="server" TextMode="Password"></asp:TextBox>
            <span runat="server" id="checkPasswd2" />
            <label runat="server" id="labCheckPasswd" />
        </div>
        <div>
            <asp:Button class="btn btn-default" ID="btnChange" runat="server" Text="Zmień hasło" OnClick="BtnChange_Click" />
            
        </div>
        
        <asp:SqlDataSource ID="sqlEmailPassword" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Email], [Password] FROM [Users] WHERE ([Email] = @Email2)">
            <SelectParameters>
                <asp:ControlParameter ControlID="tbEmail" Name="Email2" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
