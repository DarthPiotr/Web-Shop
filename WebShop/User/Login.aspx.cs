using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data.SqlClient;

namespace WebShop
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            bool canProceed = true;
            // Email
            labCheckEmail.InnerText = "";
            if (String.IsNullOrWhiteSpace(tbEmail.Text))
            {
                SetState(checkEmail, State.Wrong);
                canProceed = false;
            }
            if (String.IsNullOrWhiteSpace(tbPasswd.Text))
            {
                SetState(checkPasswd, State.Wrong);
                canProceed = false;
            }

            if (canProceed)
            {
                DataView dv = SQLHelper.SQLSelect(sqlGetUserInfo);

                if (dv.Count == 0)
                    checkEmail.InnerText = "Nieprawidłowy email lub hasło.";
                else if (!Hashing.ComparePasswords(tbPasswd.Text, dv[0][4].ToString())){
                    checkEmail.InnerText = "Nieprawidłowy email lub hasło.";
                }
                else
                {
                    User u = new User((int)dv[0]["Id"], dv[0]["Name"].ToString(), dv[0]["Lastname"].ToString(), dv[0]["Username"].ToString(), dv[0]["Email"].ToString(), dv[0]["Type"].ToString(), (bool)dv[0]["Active"]);
                    Session[Utils.USERDATA] = u;

                    // create cart for new user
                    /*SqlConnection newCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    newCon.Open();
                    String st =String.Format(
                        "if not exists (select * from sys.tables where name='cart_{0}') create table cart_{0} ( [Id] int identity(1,1) not null, [ItemId] varchar(64) not null, [Count] int not null default 1, UNIQUE NONCLUSTERED ([ItemId] ASC) )", u.Id);
                    SqlCommand newCmd = new SqlCommand(st, newCon);
                    newCmd.ExecuteNonQuery();
                    newCon.Close();
                    newCon.Dispose();*/


                    /*ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Zalogowano jako "+u.Username+"')", true);*/

                    Response.Redirect("LoginChange.aspx");
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