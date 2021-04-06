<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="WebShop.Orders" %>
<%@ Register Assembly="WebShop"  Namespace="WebShop"  TagPrefix="webshop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="sqlOrders" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" />

    <%if (Utils.GetUser(Session) != null)
        { %>
    <h1>Twoje zamówienia</h1>
    <div class="content">
        <div class="row">
            <div class="col-lg-12 well">
                <asp:Repeater OnItemDataBound="RepTab_ItemDataBound" runat="server" ID="repTab">
                    <HeaderTemplate>
                        <table class="table tabe-hover cart">
                            <thead>
                                <tr>
                                    <th>Numer zamówienia</th>
                                    <th>Wartość</th>
                                    <th>Data złożenia</th>
                                    <th>Status</th>
                                    <th>Akcja</th>
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
                            <!-- Value -->
                            <td class="number">
                               <%# DataBinder.Eval(Container.DataItem, "Value", "{0:N2}") %> zł
                            </td>
                            <!-- Date -->
                            <td>
                                <%# DataBinder.Eval(Container.DataItem, "DateMade") %>
                            </td>
                            <!-- Status -->
                            <td>
                                <asp:Label runat="server" ID="labStatus" />
                            </td>
                            <!-- Action button -->
                            <td>
                                <webshop:ButtonItemId ItemId='<%# Eval("Id") %>' runat="server" ID="btnDel" Text="Anuluj" Visible="false" OnClick="BtnDel_Click" />
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
