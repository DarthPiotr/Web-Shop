<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="WebShop.UserInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if (UriQuery.HasActivationString(Request))
        {
            if (UriQuery.ValidActivation(Request))
            {%>
                <div class="alert alert-success alert-dismissible">
                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                    <p><strong>Aktywacja konta przebiegła pomyślnie.</strong></p>
                    <p>Może być wymagane ponowne logowanie.</p>
                </div>
        <%  }
            else
            {%>
                <div class="alert alert-danger alert-dismissible">
                    <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                    <p><strong>Błąd podczas aktywacji konta.</strong></p>
                    <p>Być może link nie jest już aktywny lub konto zostało przeniesione na inny adres.</p>
                </div>
         <% }

       }%> 
    
    <%if (WebShop.Utils.GetUser(Session) != null)
        { %>
    <div class="jumbotron">
        <h1 runat="server" id="header"></h1>
    </div>
    <div class="content">
        <% if (Session[WebShop.Utils.PASSWDCHANGED] != null)
            { %>
        <div class="alert alert-success alert-dismissible">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <p><strong>Sukces!</strong> Hasło do konta zostało zmienione.</p>
        </div>
        <%  Session[WebShop.Utils.PASSWDCHANGED] = null;
            } %>

        <% if (Session[WebShop.Utils.EMAILCHANGED] != null)
            { %>
        <div class="alert alert-success alert-dismissible">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <p><strong>Sukces!</strong> Email został zmieniony.</p>
        </div>
        <div class="alert alert-warning alert-dismissible">
            <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
            <p><strong>Uwaga!</strong> Na podany adres została wysłana wiadomość z potwierdzeniem zmiany adresu. Aktywuj nowy adres.</p>
        </div>
        <%  Session[WebShop.Utils.EMAILCHANGED] = null;
            }%>
        <% if (!Utils.GetUser(Session).Active)
            {  %>
        <div class="alert alert-warning">
            <p><strong>Uwaga!</strong> Aktywuj konto aby uzyskać dostęp do wszystkich możliwości.
            <asp:LinkButton runat="server" ID="resend" OnClick="resend_Click" Text="Wyślij jeszcze raz">Wyślij jeszcze raz</asp:LinkButton></p>
        </div>
        <%  }%>
        <div class="row text-center">
            <div class="col-md-6 well">
                <span class="glyphicon glyphicon-shopping-cart" style="font-size: 70px;"></span>
                <p><a href="Cart.aspx" style="font-size: 30px;">Koszyk</a></p>
            </div>
            <div class="col-md-6 well">
                <span class="glyphicon glyphicon-list-alt" style="font-size: 70px;"></span>
                <p><a href="Orders.aspx" style="font-size: 30px;">Twoje zamówienia</a></p>
            </div>
        </div>
        <div class="row text-center">
            <div class="col-md-4 well">
                <span class="glyphicon glyphicon-question-sign" style="font-size: 70px;"></span>
                <p><a href="ChangePassword.aspx" style="font-size: 30px;">Zmiana hasła</a></p>
            </div>
            <div class="col-md-4 well">
                <span class="glyphicon glyphicon-envelope" style="font-size: 70px;"></span>
                <p><a href="#emailchange" data-toggle="collapse" style="font-size: 30px;">Zmiana adresu email</a></p>
            </div>
            <div class="col-md-4 well">
                <span class="glyphicon glyphicon-trash" style="font-size: 70px;"></span>
                <p><a data-toggle="modal" data-target="#modDel" href="#" style="font-size: 30px;">Usuwanie konta</a></p>
            </div>

            <!-- Modal -->
            <div class="modal fade" id="modDel" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Usuwanie konta</h4>
                        </div>
                        <div class="modal-body">
                            <div class="alert alert-danger">
                                <p>Czy na pewno chcesz usunąć konto? <strong>Tej operacji nie można cofnąć.</strong></p>
                                <p>Po usunięciu konta nastąpi wylogowanie.</p>
                            </div>
                            <div class="alert alert-warning">
                                <p>Jeśli składałeś zamówienia, część twoich danych zostanie zarchiwizowana i może być wykorzystana w dokumentacji Serwisu. Aby trwale usunąć dane, skontaktuj się z Działem Obsługi Klienta.</p>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button runat="server" onserverclick="DeleteAccount_Click" type="button" class="btn btn-default" data-dismiss="modal">Tak</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Nie</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <!-- Zmiana hasła -->
        <div class="row">
            <div id="emailchange" 
                class="collapse 
                <% if (Session[WebShop.Utils.EMAILCHANGING] != null) { %> in <%} Session[WebShop.Utils.EMAILCHANGING] = null;%> col-md-12">
                <h2>Zmaina adresu email</h2>
                <div class="form-group">
                    <asp:Label ID="labEmail" runat="server" Text="Nowy email:"></asp:Label>
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
                    <asp:Button Cssclass="btn btn-default" ID="btnRegister" runat="server" Text="Zmień adres email" OnClick="BtnChange_Click" />
                </div>
            </div>
        </div>
    </div>

    <asp:SqlDataSource ID="sqlDeleteUpdate" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"></asp:SqlDataSource>

     <%     if (WebShop.Utils.GetUser(Session).IsAdmin)
            {
            %>
    <h2>Narzędzia administratora</h2>
        <div class="row text-center">
            <div class="col-lg-6 well">
                <span class="glyphicon glyphicon-user" style="font-size: 70px;"></span>
                <p><a href="UserAdmin.aspx" style="font-size: 30px;">Użytkownicy</a></p>
            </div>
            <!--
            <div class="col-lg-4 well">
                <span class="glyphicon glyphicon-phone" style="font-size: 70px;"></span>
                <p><a href="#" style="font-size: 30px;">Produkty</a></p>
            </div>-->
            <div class="col-lg-6 well">
                <span class="glyphicon glyphicon-check" style="font-size: 70px;"></span>
                <p><a href="OrdersAdmin.aspx" style="font-size: 30px;">Zamówienia</a></p>
            </div>
        </div>
        <%} %>



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
