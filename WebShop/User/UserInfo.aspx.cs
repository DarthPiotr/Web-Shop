using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebShop
{
    public partial class UserInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Utils.GetUser(Session) != null)
            {
                header.InnerText = "Witaj " + Utils.GetUser(Session).Username + "!";
            }
            Debug.WriteLine("Request: " + Request["ver"]);
        }

        protected void BtnChange_Click(object sender, EventArgs e)
        {
            Session[Utils.EMAILCHANGING] = true;
            bool canProceed = true;

            if (String.IsNullOrEmpty(tbEmail.Text))
            {
                SetState(checkEmail, State.Wrong);
                canProceed = false;
            }

            labCheckPasswd.InnerText = "";
            if (String.IsNullOrEmpty(tbPasswd.Text))
            {
                SetState(checkPasswd, State.Wrong);
                labCheckPasswd.InnerText = "Wprowadź hasło";
                canProceed = false;
            }

            if (canProceed)
            {
                // czy email już istnieje w bazie
                sqlDeleteUpdate.SelectCommand = "SELECT [Id] FROM [Users] WHERE [Email] = '" + tbEmail.Text + "'";
                DataView dv =SQLHelper.SQLSelect(sqlDeleteUpdate);

                labCheckEmail.InnerText = "";
                if (dv.Count > 0)
                {
                    SetState(checkEmail, State.Wrong);
                    labCheckEmail.InnerText = "Podany adres email istnieje już w bazie danych.";
                }
                else
                {
                    // czy hasło do konta jest zgodne
                    int userId = Utils.GetUser(Session).Id;
                    sqlDeleteUpdate.SelectCommand = "SELECT [Password], [Active] FROM [Users] WHERE [Id] = " + userId;
                    dv =SQLHelper.SQLSelect(sqlDeleteUpdate);

                    if (Hashing.ComparePasswords(tbPasswd.Text, dv[0][0].ToString()))
                    {
                        // zaktualizuj maila i  wyłącz konto
                        sqlDeleteUpdate.UpdateCommand = "UPDATE [Users] SET [Email] = '" + tbEmail.Text + "', [Active] = 0 WHERE [Id] = " + userId;
                        sqlDeleteUpdate.Update();

                        string hash = Hashing.Hash(userId + DateTime.Now.ToString()); // generate new hash
                        if (!(bool)dv[0][1]) { // if inactive
                            sqlDeleteUpdate.UpdateCommand = "UPDATE [Verification] SET [Code] = '" + hash + "' WHERE [UserId] = " + userId;
                            sqlDeleteUpdate.Update(); // update database
                        }
                        else // if active account
                        {
                            sqlDeleteUpdate.InsertCommand = "INSERT INTO [Verification] VALUES (" + userId + ", '" + hash + "')";
                            sqlDeleteUpdate.Insert();
                        }
                        Mailing.SendEmail(tbEmail.Text, hash); // send new email

                        Utils.GetUser(Session).Email = tbEmail.Text;
                        Utils.GetUser(Session).Active = false;
                        Session[Utils.EMAILCHANGED] = true;
                        Session[Utils.EMAILCHANGING] = null;
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }
        }

        protected void DeleteAccount_Click(object Sender, EventArgs e)
        {
            sqlDeleteUpdate.DeleteCommand = "DELETE FROM [Users] WHERE Id = " + Utils.GetUser(Session).Id;
            sqlDeleteUpdate.Delete();

            sqlDeleteUpdate.DeleteCommand = "DELETE FROM [CartItems] WHERE [UserId] = " + Utils.GetUser(Session).Id;
            sqlDeleteUpdate.Delete();

            // remove cart with user
            /*SqlConnection newCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            newCon.Open();
            String st = String.Format(
                "if exists (select * from sys.tables where name='cart_{0}') drop table cart_{0}", Utils.GetUser(Session).Id);
            SqlCommand newCmd = new SqlCommand(st, newCon);
            newCmd.ExecuteNonQuery();
            newCon.Close();
            newCon.Dispose();*/

            System.Diagnostics.Debug.WriteLine("Logging out...");
            Session[Utils.REMOVEACCOUNT] = true;
            Response.Redirect("LoginChange.aspx");
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

        protected void resend_Click(object sender, EventArgs e)
        {
            Mailing.SendEmail("p.marciniak@vp.pl", "THISisFORverificationTEST");
            System.Diagnostics.Debug.WriteLine("Przycisk kliknięty!");
            var btn = sender as LinkButton;
            btn.Text = "Wysłano!";
            btn.Enabled = false;
            btn.Font.Underline = false;
        }
    }
}