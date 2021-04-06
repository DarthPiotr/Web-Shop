<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdersAdmin.aspx.cs" Inherits="WebShop.OrdersAdmin" %>
<%@ Register Assembly="WebShop"  Namespace="WebShop"  TagPrefix="webshop" %>
<asp:Content ContentPlaceHolderID="headerFiles" runat="server">
    <link rel="stylesheet" href="../Content/InputStyle.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<asp:SqlDataSource ID="sqlOrders" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" />

     <%if (CanLoad)
        { %>
    <h1>Wszystkie zamówienia</h1>
    <div class="content">
        <div class="row well">
            <div class="col-lg-12">
                 <div class="form-group" style="margin: 0px; vertical-align: middle;">
                     <label>Pokaż:</label>
                     <asp:CheckBox ForeColor="Green" runat="server" ID="cbRealised" Text="Zrealizowane" Checked="true" />
                     <asp:CheckBox ForeColor="Orange" runat="server" ID="cbToRealization" Text="Do realizacji" Checked="true" />
                     <asp:CheckBox ForeColor="Red" runat="server" ID="cbToConfirm" Text="Do zatwierdzenia" Checked="true" />
                 </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 well">
                <asp:Repeater OnItemDataBound="RepTab_ItemDataBound" runat="server" ID="repTab">
                    <HeaderTemplate>
                        <table class="table tabe-hover cart">
                            <thead>
                                <tr>
                                    <th>Numer zamówienia</th>
                                    <th>ID użytkownika</th>
                                    <th>Wartość</th>
                                    <th>Data złożenia</th>
                                    <th>Data Przyjęcia</th>
                                    <th>Data Realizacji</th>
                                    <th>Status</th>
                                    <th colspan="2">Akcja</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <!-- Order ID -->
                            <td>
                                <a href='OrderDetails.aspx?<%# UriQuery.ORDER + "=" + Eval("Id") %>'>Zamówienie <%# Eval("Id") %></a>
                            </td>
                            <!-- User ID -->
                            <td>
                               <%# Eval("UserId") %>
                            </td>
                            <!-- Value -->
                            <td class="number">
                               <%# DataBinder.Eval(Container.DataItem, "Value", "{0:N2}") %>&nbsp;zł
                            </td>
                            <!-- Date made -->
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "DateMade", "{0:dddd, dd MMMM yyyy HH:mm:ss}") %>
                            </td>
                            <!-- Date accepted -->
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "DateAccepted", "{0:dddd, dd MMMM yyyy HH:mm:ss}") %>
                            </td>
                            <!-- Date executed -->
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "DateExecuted", "{0:dddd, dd MMMM yyyy HH:mm:ss}") %>
                            </td>
                            <!-- Status -->
                            <td>
                                <asp:Label runat="server" ID="labStatus" />
                            </td>
                            <!-- Action button -->
                            <td>
                                <webshop:ButtonItemId ItemId='<%# Eval("Id") %>' runat="server" ID="btnAct" Visible="false" OnClick="BtnAct_Click" />
                            </td>
                            <!-- Delete button -->
                            <td>
                                <webshop:ButtonItemId ItemId='<%# Eval("Id") %>' runat="server" ID="btnDel" Text="Usuń" OnClick="BtnDel_Click" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div class="form-group">
                <a href="UserInfo.aspx">< Powrót do panelu</a>
            </div>
        </div>
    </div>
    <%}
        else
        { %>
    <div class="jumbotron">
        <h1>Nie zalogowano</h1>
        <p>
            Aby korzystać z tej strony, <a href="Login.aspx">zaloguj się</a>.
        </p>
    </div>
    <%} %>

</asp:Content>
