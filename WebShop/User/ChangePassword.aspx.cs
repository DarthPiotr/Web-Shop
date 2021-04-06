using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebShop
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User u = Utils.GetUser(Session);
            if (u != null)
            {
                header.InnerHtml = "Zmiana hasła dla konta " + u.Username + " ( <strong>" + u.Email + "</strong> )";
            }
        }

        protected void BtnChange_Click(object sender, EventArgs e)
        {
            User u = Utils.GetUser(Session);
            string email = u != null ? u.Email : tbEmail.Text;
            bool canProceed = true;
            string hashPasswd = Hashing.Hash(tbPasswd1.Text);

            if (String.IsNullOrEmpty(email)) // czy wypełniono email
            {
                SetState(checkEmail, State.Wrong);
                canProceed = false;
            }
            if (String.IsNullOrEmpty(tbPasswd1.Text)) // czy wypełniono pierwsze hasło
            {
                SetState(checkPasswd1, State.Wrong);
                canProceed = false;
            }
            if (String.IsNullOrEmpty(tbPasswd2.Text)) // czy wypełniono drugie hasło
            {
                SetState(checkPasswd2, State.Wrong);
                canProceed = false;
            }

            if (canProceed) // jeśli tak to drugi etap weryfikacji
            {
                // Pobranie hasła do danego maila
                sqlEmailPassword.CancelSelectOnNullParameter = false;
                sqlEmailPassword.SelectCommand = "SELECT [Email], [Password] FROM [Users] WHERE ([Email] = '" + email + "')";
                DataView dv =SQLHelper.SQLSelect(sqlEmailPassword);

                if (dv.Count == 0) // jeśli nie ma adresu
                {
                    SetState(checkEmail, State.Wrong);
                    labCheckEmail.InnerText = "Nie odnaleziono adresu w bazie danych";
                    canProceed = false;
                }

                if(tbPasswd1.Text != tbPasswd2.Text) // jeśli hasła są różne
                {
                    SetState(checkPasswd1, State.Wrong);
                    SetState(checkPasswd2, State.Wrong);
                    labCheckPasswd.InnerText = "Hasła nie są zgodne.";
                    canProceed = false;
                }
                else if(tbPasswd2.Text.Length < 8) // jeśli hasło za krótkie
                {
                    SetState(checkPasswd1, State.Wrong);
                    SetState(checkPasswd2, State.Wrong);
                    labCheckPasswd.InnerText = "Hasło musi mieć minimum 8 znaków.";
                    canProceed = false;
                }
                else if(Hashing.ComparePasswords(tbPasswd1.Text, dv[0][1].ToString())) // jeśli takie jak stare hasło
                {
                    SetState(checkPasswd1, State.Wrong);
                    SetState(checkPasswd2, State.Wrong);
                    labCheckPasswd.InnerText = "Nowe hasło musi być różne od starego hasła.";
                    canProceed = false;
                }

                if (canProceed)
                {
                    // aktualizacja hasła w bazie
                    sqlEmailPassword.UpdateCommand = "UPDATE [Users] SET [Password] = '" + hashPasswd + "' WHERE [Email] = '" + email + "'";
                    sqlEmailPassword.Update();

                    Session[Utils.PASSWDCHANGED] = tbEmail.Text;
                    if (Utils.GetUser(Session) != null)
                        Response.Redirect("UserInfo.aspx"); // powrót na stronę usera jeśli zalogowany
                    else
                        Response.Redirect("LoginChange.aspx"); // na stronę informacyjną jeśli nie
                }
            }

        }


        private enum State { Undefined, Right, Wrong }
        private void SetState(HtmlGenericControl node, State state = State.Undefined)
        {
            switch (state)
            {
                case State.Undefined:
                case State.Right:
                    node.Attributes["class"] = "";
                    break;
                /*case State.Right:
                    node.Attributes["class"] = "glyphicon glyphicon-ok-circle";
                    break;*/
                case State.Wrong:
                    node.Attributes["class"] = "glyphicon glyphicon-remove-circle";
                    break;
            }
        }
    }
}