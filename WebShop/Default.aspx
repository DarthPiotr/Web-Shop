<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebShop._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>AliPhonExpress</h1>
    </div>
    <div class="content">
        <div class="row">
            <div class="col-lg-6">
                <h2>Witaj!</h2>
                <p>To jest sklep internetowy stworzony na potrzeby projektu na przedmiot Systemy baz danych.</p>
                <a href="Shop.aspx">Przejdź do sklepu >></a>
            </div>
            <div class="col-lg-6">
                <h3>Użyte technologie</h3>
                <ul>
                    <li>ASP.NET</li>
                    <li>C#</li>
                    <li>HTML</li>
                    <li>CSS</li>
                    <li>JavaScript</li>
                    <li>jQuery</li>
                    <li>Bootstrap</li>
                    <li>T-SQL</li>
                </ul>
            </div>
        </div>
         <div class="row">
            <div class="col-lg-12 alert-info">
                <h2><strong>NOWOŚCI!!!</strong></h2>
                <ul>
                    <li>Potwierdzanie rejestracji przez email</li>
                    <li>Konieczność potwierdzania rejestracji aby złożyć zamówienie</li>
                    <li>Generowanie unikalnego klucza do potwierdzenia konta</li>
                    <li>Zmiana adresu email przed potwierdzeniem rejestracji tworzy nowy kod i wysyła na nowego maila. Stary kod jest nieaktualny.</li>
                    <li>Deaktywacja konta przy zmianie adresu email</li>
                    <li>Aktualizacja stanu aktywacji konta przy zmianie adresu email</li>
                </ul>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-7">
                <h2>O aplikacji</h2>
                <p>Strona zawiera następujące funkcjonalności:</p>
                <ul style="list-style-type: none">
                    <li><span class="glyphicon glyphicon-search"></span> Wyszukiwanie
                        <ul style="list-style-type: none">
                            <li class="ok"> Filtrowanie wyników według wszystkich kryteriów w bazie danych</li>
                            <li class="ok"> Ilość produktów dla danego kryterium filtrowania</li>
                            <li class="ok"> Przekazywanie parametrów filtrowania przez adres (GET)</li>
                            <li class="ok"> Przekazywanie id produktu do strony ze szczegółami przez adres (GET)</li>
                            <li class="ok"> Ilość pokazywanych wyników na stronie</li>
                            <li class="ok"> Sortowanie według ceny oraz nazwy</li>
                            <li class="ok"> Szczegóły produktu</li>
                            <li class="ok"> Dodawanie produktu do koszyka jako użytkownik</li>
                            <li class="ok"> Automatyczne tworzenie zapytań SQL w zależności od wybranych filtrów</li>
                        </ul>
                    </li>
                    <li><span class="glyphicon glyphicon-user"></span> Konta i zarządzanie
                        <ul style="list-style-type: none">
                            <li class="ok"> Zakładanie i usuwanie konta</li>
                            <li class="ok"> Sprawdzanie unikalności adresu email</li>
                            <li class="ok"> Minimum 8-znakowe hasło</li>
                            <li class="ok"> Szyfrowanie hasła i zapis w takiej postaci w bazie danych</li>
                            <li class="ok"> Możliwość zmiany hasła i adresu email przypisanego do konta</li>
                            <li class="ok"> Konto użytkownika i administratora</li>
                            <li class="ok"> Administrator z możliwością usunięcia lub zmiany dowolnego konta: uprawnień oraz wszystkich danych</li>
                            <li class="ok"> Administrator z możliwością zarządzania zakupami</li>
                        </ul>
                    </li>
                    <li><span class="glyphicon glyphicon-shopping-cart"></span> System Zakupów
                        <ul style="list-style-type: none">
                            <li class="ok"> Koszyk użytkownika</li>
                            <li class="ok"> Zmiana ilości wybranych produktów</li>
                            <li class="ok"> Usuwanie przedmiotów z koszyka</li>
                            <li class="ok"> Zamawianie całego koszyka</li>
                            <li class="ok"> Podział realizacji zamówienia na trzy etapy:
                                <ul>
                                    <li><em>Oczekuje na zatwierdzenie</em> - użytkownik może anulować zlecenie; oczekuje na zatwierdzenie przez administratora</li>
                                    <li><em>W trakcie realizacji</em> - zatwierdzone przez administratora, tylko administrator może je usunąć. Czeka na zrealizowanie przez administratora</li>
                                    <li><em>Zrealizowane</em> - zrealizowane przez administratora. Administrator może je usunąć</li>
                                </ul>
                            </li>
                            <li class="ok"> Lista zamówień użytkownika</li>
                            <li class="ok"> Szczegóły zamówienia</li>
                            <li class="ok"> Lista zamówień wszystkich użytkowników widoczna dla administratora</li>
                            <li class="ok"> Filtrowanie listy według statusu zamówienia</li>
                        </ul>
                    </li>
                    <li><span class="glyphicon glyphicon-hdd"></span> Baza danych i zabezpieczenia
                        <ul style="list-style-type: none">
                            <li class="ok"> Archiwizacja usuniętych użytkowników dzięki wyzwalaczowi</li>
                            <li class="ok"> Procedury ułatwiające zarządzanie zamówieniami</li>
                            <li class="ok"> Widoki ułatwiające wybór informacji o produkcie i zdjęcia produktu</li>
                            <li class="ok"> Widok wybierający ilość produktów w danych zakresach wielkości ekranu</li>
                            <li class="ok"> Użytkownik musi posiadać unikalny adres email</li>
                            <li class="ok"> Sprawdzanie, czy email nie istnieje już w bazie danych przy zmianie adresu email</li>
                            <li class="ok"> Rejestracja zabezpieczona przez reCAPTCHA</li>
                            <li class="ok"> Sprawdzanie, czy nowe hasło różni się od nowego przy zmianie hasła</li>
                            <li class="ok"> Zabezpieczenie stron użytkownika przed wejściem osoby niezalogowanej</li>
                            <li class="ok"> Zabezpieczenie stron administratora przed wejściem przez osoby niezalogowane lub bez uprawnień administratora</li>
                        </ul>
                    </li>
                </ul>
            </div>
            <div class="col-lg-5">
                <h3>Możliwości rozwoju</h3>
                <p>Poniżej znajduje się lista pomysłów na funkcje, które można dodać lub nie starczyło mi czasu na ich realizację</p>
                <ul>
                    <li>Więcej produktów</li>
                    <li>Więcej atrybutów produktów - więcej opcji filtrowania</li>
                    <li>Wyszukiwanie produktów po nazwie</li>
                    <li>Menu administratora do dodawania i modyfikacji produktów</li>
                    <li>Dodawanie kolejnych zdjęć do produktów przez administratora</li>
                    <li>Tworzenie raportu z realizacji zamówień przez administratora (do tego potrzebna jest archiwizacja użytkowników)</li>
                    <li>Dodanie oceniania i komentowania produktów, sortowanie według oceny</li>
                    <li>Dodanie licznika odwiedzeń i kupna produktów, sortowanie według popularności, promowanie bestsellerów</li>
                    <li>Dodanie informacji o ilości produktów w magazynie, aktualizacja informacji przy zakupie</li>
                    <li>Zamówienia specjalne, kiedy produktu nie ma w magazynie</li>
                    <li>Więcej opcji filtrowania zamówień w menu administratora</li>
                    <li>Możliwość czyszczenia koszyka</li>
                    <li>Zniżki dla dużych zamówień i stałych klientów</li>
                    <li>Okresowe promocje</li>
                    <li>Filtrowanie i sortowanie zamówień w menu użytkownika</li>
                    <li>Filtrowanie i sortowanie produktów w koszyku i w szczegółach zamówienia</li>
                    <li>Refactoring i porządkowanie kodu</li>
                    <li>Dodanie linków do mediów społecznościowych</li>
                </ul>
            </div>
        </div>
    </div>

</asp:Content>
