using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop
{
    public partial class LoginSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Przekierowanie z pomyślnej rejestracji
            if (Session[Utils.FIRSTREGISTER] != null)
            {
                Session[Utils.FIRSTREGISTER] = null;
                header.InnerText = "Rejestracja przebiegła pomyślnie";
                info.InnerHtml = "Teraz możesz się <a href=\"Login.aspx\">zalogować</a>";
                Session[Utils.USERDATA] = null;
                return;
            }

            // Usuwanie konta
            if (Session[Utils.REMOVEACCOUNT] != null)
            {
                Session[Utils.REMOVEACCOUNT] = null;
                header.InnerHtml = "Usunięto konto " + Utils.GetUser(Session).Username + " ( " + Utils.GetUser(Session).Email + " )";
                info.InnerHtml = "Przykro nam, że nas opuszczasz :(";
                Session[Utils.USERDATA] = null;
                return;
            }

            // Zmiana hasła
            if (Session[Utils.PASSWDCHANGED] != null)
            {
                header.InnerHtml = "Hasło do konta " + Session[Utils.PASSWDCHANGED].ToString() + "  zostało zmienione.";
                info.InnerHtml = "Teraz możesz się <a href=\"Login.aspx\">zalogować</a>";
                Session[Utils.PASSWDCHANGED] = null;
                Session[Utils.USERDATA] = null;
                return;
            }


            User u = Utils.GetUser(Session);
            // Wylogowanie
            if (u == null)
            {
                header.InnerText = "Wylogowano pomyślnie";
                info.InnerText = "Zapraszamy ponownie";
            }
            // Zalogowanie
            else
            {
                header.InnerText = "Logowanie zakończone sukcesem";
                info.InnerHtml = "Zalogowano jako " + u.Name + " " + u.Lastname + " ( <b>" + u.Username + "</b> ). <a href=\"UserInfo.aspx\"> Panel &raquo;</a>";
            }
        }
    }
}