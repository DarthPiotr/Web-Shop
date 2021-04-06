<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="WebShop.OrderDetails" %>

<%@ Register Assembly="WebShop" Namespace="WebShop" TagPrefix="webshop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="sqlGetOrder" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"></asp:SqlDataSource>
    <%if (CanLoad)
        { %>
    <asp:Repeater runat="server" ID="repTab">
        <HeaderTemplate>
            <h1>
                <asp:Label runat="server" ID="labOrderName" /></h1>
            <div class="content">
                <div class="row">
                    <div class="col-lg-12 well">
                        <table class="table tabe-hover cart">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>Nazwa</th>
                                    <th>Cena</th>
                                    <th>Ilość</th>
                                    <th>Wartość</th>
                                </tr>
                            </thead>
        </HeaderTemplate>
        <ItemTemplate>
                        <tr>
                            <!-- image -->
                            <td class="image">
                                <img height="120" src='../images/<%# DataBinder.Eval(Container.DataItem, "Path") %>' /></td>
                            <!-- product name -->
                            <td><a href="../Product.aspx?<%# WebShop.UriQuery.PRODUCT + "=" + DataBinder.Eval(Container.DataItem, "ProductId") %>"><%# DataBinder.Eval(Container.DataItem, "Manufacturer") %> <%# DataBinder.Eval(Container.DataItem, "Model") %>
                            </a></td>
                            <!-- price -->
                            <td class="number"><%# DataBinder.Eval(Container.DataItem, "Price", "{0:N2}") %> zł</td>
                            <!-- !! count changer !! -->
                            <td runat="server" id="change" class="number" enableviewstate="True">
                                <asp:Label runat="server" ID="labCount" Text='<%# DataBinder.Eval(Container.DataItem, "Count") %>'></asp:Label>
                                <asp:TextBox Visible="false" ID="tbCount" runat="server" TextMode="Number"></asp:TextBox>
                                szt.
                            </td>
                            <!-- value -->
                            <td class="number" runat="server" id="elemValue">
                                <%# String.Format("{0:N2}", Convert.ToDouble(DataBinder.Eval(Container.DataItem, "Price"))*Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Count"))) %>
                                            zł</td>
                        </tr>
        </ItemTemplate>
        <FooterTemplate>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td class="number"><b>Razem:</b></td>
                            <!-- count sum -->
                            <td class="number">
                                <asp:Label runat="server" ID="labSum"></asp:Label></td>
                        </tr>
                    </table>  
                </div>
                <div class="form-group">
                    <a href="Orders.aspx">< Powrót do listy zamówień</a>
                </div>
            </div>
        </div>
        </FooterTemplate>
    </asp:Repeater>
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
