<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="WebShop.Product" %>
<%@ Register Assembly="WebShop"  Namespace="WebShop"  TagPrefix="webshop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:SqlDataSource ID="sqlGetProductData" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT 
        [Model],        /*0*/
        [Manufacturer], 
        [OS],           /*2*/
        [ScreenSize], 
        [Memory],       /*4*/
        [RAM], 
        [Processor],    /*6*/
        [Cores], 
        [Clock],        /*8*/
        [Camera], 
        [SDCard],       /*10*/
        [DualSIM], 
        [Price],        /*12*/
        [Id]
        FROM [Products]"></asp:SqlDataSource>

    <div class="content">

        <%
            var product = GetProductData();
            if (product?.Count > 0)
            {
                var photos = GetProductPhotos();
        %>
        <div class="jumbotron">
            <h1><%= product[0][1] + " " + product[0][0] %></h1>
        </div>

        <div style="margin-top: 40px;" class="row">
            <div class="col-md-4">

                <div id="carousel" class="carousel slide" data-ride="carousel" data-interval="false">
                    <!-- Indicators -->
                    <ol class="carousel-indicators">

                        <% for (int i = 0; i < photos.Count; i++)
                           {%>
                        <li data-target="#carousel" 
                            data-slide-to="<%=i %>" 
                            <%if (i == 0) {%> 
                            class="active" 
                            <%} %> >
                        </li>
                        <% } %>
                    </ol>

                    <!-- Wrapper for slides -->
                    <div class="carousel-inner">

                        <% for (int i = 0; i < photos.Count; i++)
                           {%>
                        <div class="item<%= i==0 ? " active" : "" %>">
                            <img src="<%="images/"+photos[i][0] %>" style="width: 100%;">
                        </div>
                        <% } %>
                    </div>

                    <!-- Left and right controls -->
                    <a class="left carousel-control" href="#carousel" data-slide="prev">
                        <span class="glyphicon glyphicon-chevron-left"></span>
                        <span class="sr-only">Previous</span>
                    </a>
                    <a class="right carousel-control" href="#carousel" data-slide="next">
                        <span class="glyphicon glyphicon-chevron-right"></span>
                        <span class="sr-only">Next</span>
                    </a>
                </div>

            </div>
            <div class="col-md-8">
                <div class="content well">
                    <div class="row">
                        <div class="col-md-8" id="basicinfo">
                            <h2>Wybrane parametry</h2>
                            <hr />
                            <ul>
                                <li>Pamięć wbudowana: <b><%=product[0][4].ToString() %> GB</b></li>
                                <li>Pamięć RAM: <b><%=product[0][5].ToString() %> GB</b></li>
                                <li>Przekątna ekranu: <b><%=product[0][3].ToString() %>"</b></li>
                                <li>Aparat: <b><%=product[0][9].ToString() %> MPix</b></li>
                            </ul>
                            <div class="alert alert-info">
                                <h4>Producent poleca</h4>
                                <p>Niezawodny telefon, warty swojej ceny!</p>
                            </div>
                        </div>
                        <div class="col-md-4 well" id="priceinfo">
                            <h2><%= product[0][12] %> zł</h2>
                            <p><b>Darmowa dostawa!</b> Dostawa lub odbiór w ciągu najbliższych 3 dni roboczych.</p>
                            <webshop:ButtonItemId class="button" runat="server" ID="btnAdd" OnClick="BtnAdd_Click" Text="Dodaj do koszyka" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 well" id="specs">
                    <h3>PRODUKT</h3>
                    <table class="table table-striped">
                        <tr>
                            <td>Producent</td>
                            <td><%= product[0][1] %></td>
                        </tr>
                        <tr>
                            <td>Model</td>
                            <td><%= product[0][0] %></td>
                        </tr>
                    </table>
                    <h3>Techniczne</h3>
                    <table class="table table-striped">
                        <tr>
                            <td>System operacyjny</td>
                            <td><%= product[0][2] %></td>
                        </tr>
                        <tr>
                            <td>Procesor</td>
                            <td><%= product[0][6] %></td>
                        </tr>
                        <tr>
                            <td>Pamięć RAM</td>
                            <td><%= product[0][5] %> GB</td>
                        </tr>
                        <tr>
                            <td>Pamięć wbudowana</td>
                            <td><%= product[0][4] %> GB</td>
                        </tr>
                        <tr>
                            <td>Obsługa kart pamięci</td>
                            <td><%= product[0][10].ToString() == "True" ? "Tak" : "Nie" %></td>
                        </tr>
                        <tr>
                            <td>Dual SIM</td>
                            <td><%= product[0][11].ToString() == "True" ? "Tak" : "Nie" %></td>
                        </tr>
                    </table>
                    <h3>Wyświetlacz</h3>
                    <table class="table table-striped">
                        <tr>
                            <td>Przekątna ekranu</td>
                            <td><%= product[0][3] %> "</td>
                        </tr>
                    </table>
                    <h3>Aparat</h3>
                    <table class="table table-striped">
                        <tr>
                            <td>Aparat główny</td>
                            <td><%= product[0][9] %> MPix</td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <%}
            else
            {
        %>
        <div style="margin-top: 40px;" class="row">
            <div class="col-md-12">
                <h1>Produkt nie został znaleziony w bazie danych</h1>
            </div>
        </div>
        <%} %>
    </div>
</asp:Content>
