<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="WebShop.Shop" %>
<%@ Register Assembly="WebShop"  Namespace="WebShop"  TagPrefix="webshop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:SqlDataSource ID="sqlGetProducts" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT COUNT(*) FROM [ProductsOneImage]"></asp:SqlDataSource>
    <!-- BASIC QUERY
        SELECT [Model], [Manufacturer], [Memory], [RAM], [ScreenSize], [Camera], [Path], [Price], [Id] FROM [ProductsOneImage]
        -->
    <asp:SqlDataSource ID="sqlGetFilterData" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Manufacturer], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [Manufacturer]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCart" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"></asp:SqlDataSource>

    <div class="content">
        <div class="row">
            <div class="col-md-12">
                <h1>Sklep z telefonami</h1>
            </div>
        </div>
        <div class="row" style="margin-top: 30px;">
            <div class="col-md-3">
                <div class="filtering well">
                    <h3>Filtrowanie</h3>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#price" aria-expanded="true">Cena</a>
                        <div class="collapse in" id="price">
                            <hr />
                            <label>Od:</label>
                            <asp:TextBox runat="server" ID="tbPriceMin" class="form-control" TextMode="Number"></asp:TextBox>
                            <label>Do:</label>
                            <asp:TextBox runat="server" ID="tbPriceMax" class="form-control" TextMode="Number"></asp:TextBox>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#manufacturer" aria-expanded="true">Producent</a>
                        <div class="collapse in" id="manufacturer">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbManufacturer"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#screen">Przekątna ekranu</a>
                        <div class="collapse" id="screen">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbScreen">
                                <asp:ListItem>Do 4&quot; (0)</asp:ListItem>
                                <asp:ListItem>4&quot; - 4,5&quot; (0)</asp:ListItem>
                                <asp:ListItem>4,5&quot; - 5&quot; (0)</asp:ListItem>
                                <asp:ListItem>5&quot; - 5,5&quot; (0)</asp:ListItem>
                                <asp:ListItem>5,5&quot; - 6&quot; (0)</asp:ListItem>
                                <asp:ListItem>Ponad 6&quot; (0)</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#os">System operacyjny</a>
                        <div class="collapse" id="os">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbOperatingSystem"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#memory">Pamięć wbudowana</a>
                        <div class="collapse" id="memory">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbMemory"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#ram">Pamięć operacyjna</a>
                        <div class="collapse" id="ram">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbRAM"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#processor">Procesor</a>
                        <div class="collapse" id="processor">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbProcessor"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#cores">Liczba rdzeni</a>
                        <div class="collapse" id="cores">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbCores"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#clock">Zegar procesora</a>
                        <div class="collapse" id="clock">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbClock"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#camera">Aparat</a>
                        <div class="collapse" id="camera">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbCamera"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#sdcard">Obsługa kart pamięci</a>
                        <div class="collapse" id="sdcard">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbSDCard"></asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="filter">
                        <a data-toggle="collapse" data-target="#dualsim">Dual SIM</a>
                        <div class="collapse" id="dualsim">
                            <hr />
                            <asp:CheckBoxList runat="server" ID="cbDualSim"></asp:CheckBoxList>
                        </div>
                    </div>

                    <asp:Button runat="server" ID="btnFilter" Text="Filtruj" OnClick="BtnFilter_Click" />
                </div>

                <script>
                    // prices
                    $('#MainContent_tbPriceMin').val(getUrlParameter('<%= UriQuery.MINPRICE%>'));
                    $('#MainContent_tbPriceMax').val(getUrlParameter('<%= UriQuery.MAXPRICE%>'));

                    // manufacturers
                    var list = getUrlParameter('<%= UriQuery.MANUFACTURERS %>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbManufacturer.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbManufacturer.Items[i].Value%>', 'cbManufacturer', <%=i%>)
                    <%}%>
                    }

                    // operating system
                    list = getUrlParameter('<%= UriQuery.OS %>');
                    if (list != undefined) {
                        list = list.replace(/\+/g, " ");
                    <%for (int i = 0; i < cbOperatingSystem.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbOperatingSystem.Items[i].Value%>', 'cbOperatingSystem', <%=i%>)
                    <%}%>
                    }

                    // memory
                    list = getUrlParameter('<%= UriQuery.MEMORY%>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbMemory.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbMemory.Items[i].Value%>', 'cbMemory', <%=i%>)
                    <%}%>
                    }

                    // ram
                    list = getUrlParameter('<%= UriQuery.RAM%>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbRAM.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbRAM.Items[i].Value%>', 'cbRAM', <%=i%>)
                    <%}%>
                    }

                    // processor
                    list = getUrlParameter('<%= WebShop.UriQuery.PROCESSOR%>');
                    if (list != undefined) {
                        list = list.replace(/\+/g, " ");
                    <%for (int i = 0; i < cbProcessor.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbProcessor.Items[i].Value%>', 'cbProcessor', <%=i%>)
                    <%}%>
                    }

                    // cores
                    list = getUrlParameter('<%= WebShop.UriQuery.CORES%>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbCores.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbCores.Items[i].Value%>', 'cbCores', <%=i%>)
                    <%}%>
                    }

                    // clock
                    list = getUrlParameter('<%= WebShop.UriQuery.CLOCK%>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbClock.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbClock.Items[i].Value%>', 'cbClock', <%=i%>)
                    <%}%>
                    }

                    // camera
                    list = getUrlParameter('<%= WebShop.UriQuery.CAMERA%>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbCamera.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbCamera.Items[i].Value%>', 'cbCamera', <%=i%>)
                    <%}%>
                    }

                    // SD
                    list = getUrlParameter('<%= WebShop.UriQuery.SDCARD%>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbSDCard.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbSDCard.Items[i].Value%>', 'cbSDCard', <%=i%>)
                    <%}%>
                    }

                    // DualSim
                    list = getUrlParameter('<%= WebShop.UriQuery.DUALSIM%>');
                    if (list != undefined) {
                    <%for (int i = 0; i < cbDualSim.Items.Count; i++)
                    { %>
                        setCheckbox('<%=cbDualSim.Items[i].Value%>', 'cbDualSim', <%=i%>)
                    <%}%>
                    }

                    // screen sizes
                    list = getUrlParameter('<%= WebShop.UriQuery.SCREENSIZE %>');
                    try {
                        for (var i = 0; i < <%= cbScreen.Items.Count%>; i++) {
                            if (list[i] == '1')
                                $('#MainContent_cbScreen_' + i).attr("checked", "checked");
                        }
                    } catch (e) { }

                </script>

            </div>
            <div class="col-md-9">
                <div class="well">
                    <div class="form-group" style="margin: 0px; vertical-align: middle;">
                        <label>Sortowanie: </label>
                        <asp:DropDownList runat="server" ID="ddSort" AutoPostBack="True">
                            <asp:ListItem Value="0">Cena rosnąco</asp:ListItem>
                            <asp:ListItem Value="1">Cena malejąco</asp:ListItem>
                            <asp:ListItem Value="2">Nazwa rosnąco</asp:ListItem>
                            <asp:ListItem Value="3">Nazwa malejąco</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;
                        <label>Wyników na stronie: </label>
                        <asp:DropDownList runat="server" ID="ddPagination" AutoPostBack="True">
                            <asp:ListItem Value="2">2</asp:ListItem>
                            <asp:ListItem Value="3">3</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                        </asp:DropDownList>
                        <%if (Request.Url.Query.Length > 0)
                            {%>
                        &nbsp;<asp:LinkButton runat="server" ID="lbClearFilter" OnClick="LbClearFilter_Click" Text="Wyczyść filtry"></asp:LinkButton>
                        <%} %>
                    </div>
                </div>

                <div style="text-align: center;">
                    <ul runat="server" class="pagination" id="paginationListUpper">
                    </ul>
                </div>

                <%  
                    if (ProductsList.Count == 0)
                    {%>
                <h2>Nie znaleniono produktów spełniających podane kryteria.</h2>

                <%  }
                    else
                    {%>

                <asp:Repeater ID="repeater" runat="server">
                    <ItemTemplate>

                        <div class="product well">
                            <div class="col-md-3">
                                <img src="images/<%# DataBinder.Eval(Container.DataItem, "Path") %>" style="max-width: 100%" />
                            </div>
                            <div class="col-md-9">
                                <h2>
                                    <a href="Product.aspx?<%# WebShop.UriQuery.PRODUCT + "=" + DataBinder.Eval(Container.DataItem, "Id") %>">
                                        <%# DataBinder.Eval(Container.DataItem, "Manufacturer") %> <%# DataBinder.Eval(Container.DataItem, "Model") %>
                                    </a>
                                </h2>
                                <h3><%# DataBinder.Eval(Container.DataItem, "Price") %> zł</h3>
                                <ul>
                                    <li>Pamięć wbudowana: <b><%# DataBinder.Eval(Container.DataItem, "Memory") %> GB</b></li>
                                    <li>Pamięć RAM: <b><%# DataBinder.Eval(Container.DataItem, "RAM") %> GB</b></li>
                                    <li>Przekątna ekranu: <b><%# DataBinder.Eval(Container.DataItem, "ScreenSize") %>"</b></li>
                                    <li>Aparat: <b><%# DataBinder.Eval(Container.DataItem, "Camera") %> MPix</b></li>
                                </ul>
                                <webshop:ButtonItemId runat="server" ID="btnAddCart" Text="Dodaj do koszyka" OnClick="BtnAdd_Click" ItemId='<%# DataBinder.Eval(Container.DataItem, "Id") %>'/>
                            </div>
                        </div>

                    </ItemTemplate>
                </asp:Repeater>
                <%  }%>

                <div style="text-align: center;">
                    <ul runat="server" class="pagination" id="paginationListLower">
                    </ul>
                </div>

                <script>
                    addListNodes('#MainContent_paginationListUpper', <%= Session[WebShop.Utils.PAGE].ToString() %>);
                    addListNodes('#MainContent_paginationListLower', <%= Session[WebShop.Utils.PAGE].ToString() %>);
                </script>
            </div>
        </div>
    </div>
</asp:Content>
