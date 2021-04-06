<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserAdmin.aspx.cs" Inherits="WebShop.UserAdmin" %>
<%@ Register Assembly="WebShop"  Namespace="WebShop"  TagPrefix="webshop" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:SqlDataSource ID="sqlUsers" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" />    <% if (CanLoad)
        { %>
    
    <h1>Użytkownicy</h1>
    <div class="content">
        <div class="row">
            <div class="col-lg-12 well">
                <asp:Repeater runat="server" ID="repTab">
                    <HeaderTemplate>
                        <table class="table tabe-hover cart">
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Imię</th>
                                    <th>Nazwisko</th>
                                    <th>Login</th>
                                    <th>Email</th>
                                    <th>Typ konta</th>
                                    <th>Akcja</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("Id") %>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="labName" Text='<%# Eval("Name") %>' />
                                <asp:TextBox runat="server" ID="tbName" Visible="false" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="labLastname" Text='<%# Eval("Lastname") %>' />
                                <asp:TextBox runat="server" ID="tbLastname" Visible="false" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="labUsername" Text='<%# Eval("Username") %>' />
                                <asp:TextBox runat="server" ID="tbUsername" Visible="false" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="labEmail" Text='<%# Eval("Email") %>' />
                                <asp:TextBox TextMode="Email" runat="server" ID="tbEmail" Visible="false" />
                            </td>
                            <td>
                                <asp:Label runat="server" ID="labType" Text='<%# Eval("Type") %>' />
                                <asp:DropDownList runat="server" ID="ddType" Visible="false">
                                    <asp:ListItem Value="user">Użytkownik</asp:ListItem>
                                    <asp:ListItem Value="admin">Administrator</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <webshop:ButtonItemId OnClick="BtnChange_Click" Text="Zmień" runat="server" ID="btnChange" ItemId='<%# Eval("Id") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <% }
    else
    {  %>
    <div class="jumbotron">
        <p>
            Aby korzystać z tej strony, <a href="Login.aspx">zaloguj się</a>.
        </p>
    </div>
    <%} %>

</asp:Content>
