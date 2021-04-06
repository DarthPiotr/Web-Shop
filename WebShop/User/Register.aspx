<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebShop.Register1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Rejestracja</h1>
    </div>
    <div class="content">
        <div class="form-group">
            <asp:Label ID="labName" runat="server" Text="Imię:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbName" runat="server"></asp:TextBox>
            <span runat="server" id="checkName" />
        </div>
        <div class="form-group">
            <asp:Label ID="labLastname" runat="server" Text="Nazwisko:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbLastname" runat="server"></asp:TextBox>
            <span runat="server" id="checkLastname" />
        </div>
        <div class="form-group">
            <asp:Label ID="labEmail" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbEmail" runat="server" TextMode="Email"></asp:TextBox>
            <span runat="server" id="checkEmail" />
            <label runat="server" id="labCheckEmail" />
        </div>
        <div class="form-group">
            <asp:Label ID="labUsername" runat="server" Text="Nick:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbUsername" runat="server"></asp:TextBox>
            <span runat="server" id="checkUsername" />
        </div>
        <div class="form-group">
            <asp:Label ID="labPasswd1" runat="server" Text="Hasło:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbPasswd1" runat="server" TextMode="Password"></asp:TextBox>
            <span runat="server" id="checkPasswd1" />
            <label runat="server" id="labCheckPasswd" />
        </div>
        <div class="form-group">
            <asp:Label ID="labPasswd2" runat="server" Text="Powtórz hasło:"></asp:Label>
            <asp:TextBox class="form-control" ID="tbPasswd2" runat="server" TextMode="Password"></asp:TextBox>
            <span runat="server" id="checkPasswd2" />
        </div>
        <div class="form-group">
            <div id="recaptcha" class="g-recaptcha" data-type="image" data-sitekey="6LdK2qYUAAAAAJ-UAjw9T-Gn3xy62wWVudxIBwD1">

            </div>
                <label type="hidden" id="lblMessage" runat="server" clientidmode="static"></label>
            
            <script src="https://www.google.com/recaptcha/api.js?onload=renderRecaptcha&render=explicit"" async defer></script>
            <script>
                var your_site_key = '6LdK2qYUAAAAAJ-UAjw9T-Gn3xy62wWVudxIBwD1';
                var renderRecaptcha = function () {
                    grecaptcha.render('recaptcha', {
                        'sitekey': your_site_key,
                        'callback': reCaptchaCallback,
                        theme: 'light', //light or dark    
                        type: 'image',// image or audio    
                        size: 'normal'//normal or compact    
                    });
                };

                var reCaptchaCallback = function (response) {
                    if (response !== '') {
                        jQuery('#lblMessage').html('reCAPTCHA OK');
                    }
                };

                $('#MainContent_btnRegister').click(function (e) {
                    var message = 'Wypełnij reCAPTCHA';
                    if (typeof (grecaptcha) != 'undefined') {
                        var response = grecaptcha.getResponse();
                        (response.length === 0) ? (message = 'reCAPTCHA nieprawidłowa') : (message = 'reCAPTCHA OK');
                    }
                    jQuery('#lblMessage').html(message);
                    jQuery('#lblMessage').css('color', (message.toLowerCase() == 'success!') ? "green" : "red");
                });
            </script>
        </div>
        <div>
            <asp:Button class="btn btn-default" ID="btnRegister" runat="server" Text="Zarejestruj!" OnClick="BtnRegister_Click" />
        </div>
    </div>
    <asp:SqlDataSource ID="sqlShopDatabaseInsert"
        runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        InsertCommand="INSERT INTO [Users] ([Name], [Lastname], [Email], [Password], [Username]) VALUES (@Name, @Lastname, @Email, @Password, @Username)">
        <InsertParameters>
            <asp:ControlParameter Name="Name" Type="String" ControlID="tbName" />
            <asp:ControlParameter Name="Lastname" Type="String" ControlID="tbLastname" />
            <asp:ControlParameter Name="Email" Type="String" ControlID="tbEmail" />
            <asp:ControlParameter Name="Password" Type="String" ControlID="tbPasswd1" />
            <asp:ControlParameter Name="Username" Type="String" ControlID="tbUsername" />
        </InsertParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlShopDatabaseSelectEmail"
        runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [Email] FROM [Users] WHERE ([Email] = @Email)">
        <SelectParameters>
            <asp:ControlParameter ControlID="tbEmail" Name="Email" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlInsertVerification"
        runat="server"
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand="SELECT [Id] FROM [Users] WHERE ([Email] = @Email)" >
        <SelectParameters>
            <asp:ControlParameter ControlID="tbEmail" Name="Email" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>
