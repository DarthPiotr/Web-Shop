using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace WebShop
{
    public partial class Register1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            bool canProceed = true;

            // Imię
            if (String.IsNullOrWhiteSpace(tbName.Text))
            {
                SetState(checkName, State.Wrong);
                canProceed = false;
            }
            else
                SetState(checkName, State.Right);

            // Nazwisko
            if (String.IsNullOrWhiteSpace(tbLastname.Text))
            {
                SetState(checkLastname, State.Wrong);
                canProceed = false;
            }
            else
                SetState(checkLastname, State.Right);

            // Email
            labCheckEmail.InnerText = "";
            if (String.IsNullOrWhiteSpace(tbEmail.Text))
            {
                SetState(checkEmail, State.Wrong);
                canProceed = false;
            }
            else
            {
                DataView dv =SQLHelper.SQLSelect(SqlShopDatabaseSelectEmail);
                if(dv.Count == 0)
                    SetState(checkEmail, State.Right);
                else
                {
                    SetState(checkEmail, State.Wrong);
                    labCheckEmail.InnerText = "Na ten adres już założono konto.";
                    canProceed = false;
                }
            }

            // Hasło
            bool passwordsReady = true;
            labCheckPasswd.InnerText = "";
            if (String.IsNullOrEmpty(tbPasswd1.Text))
            {
                SetState(checkPasswd1, State.Wrong);
                canProceed = passwordsReady = false;
            }
            if (String.IsNullOrEmpty(tbPasswd2.Text))
            {
                SetState(checkPasswd2, State.Wrong);
                canProceed = passwordsReady = false;
            }
            if (passwordsReady)
            {
                if (tbPasswd1.Text != tbPasswd2.Text)
                {
                    SetState(checkPasswd1, State.Wrong);
                    SetState(checkPasswd2, State.Wrong);
                    canProceed = passwordsReady = false;
                    labCheckPasswd.InnerText = "Hasła nie są zgodne.";
                }
                else if (tbPasswd1.Text.Length < 8)
                {
                    SetState(checkPasswd1, State.Wrong);
                    SetState(checkPasswd2, State.Wrong);
                    canProceed = passwordsReady = false;
                    labCheckPasswd.InnerText = "Hasło powinno mieć przynajmiej 8 znaków.";
                }
                else
                {
                    SetState(checkPasswd1, State.Right);
                    SetState(checkPasswd2, State.Right);
                }
            }
            Debug.WriteLine("");

            if (!IsReCaptchValid())
            {
                Debug.WriteLine("reCAPTCHA nie jest prawidłowa.");
                canProceed = false;
            }
            else
                Debug.WriteLine("reCAPTCHA Prawidłowa.");

            if (canProceed)
            {
                tbPasswd1.Text = Hashing.Hash(tbPasswd1.Text);
                int result = sqlShopDatabaseInsert.Insert();                
                
                if (result > 0)
                {
                    DataView dv = SQLHelper.SQLSelect(SqlInsertVerification); // get new user ID
                    string verificationString = Hashing.Hash(dv[0]["Id"].ToString() + DateTime.Now.ToString()); // hash id to get verification key
                    SqlInsertVerification.InsertCommand =
                        String.Format("INSERT INTO [Verification] VALUES ({0}, '{1}')", dv[0]["Id"], verificationString); // prepare insert key to table
                    Debug.WriteLine(SqlInsertVerification.InsertCommand);
                    SqlInsertVerification.Insert(); // insert
                    Mailing.SendEmail(tbEmail.Text, verificationString); // send email

                    Session[Utils.FIRSTREGISTER] = true;
                    Response.Redirect("LoginChange.aspx");
                }
                else
                {
                    Utils.MessageBox("Konto nie mogło zostać założone.", this);
                }
            }
        }

        public bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = "6LdK2qYUAAAAABQWs-YGnaP5eBr4CXYw_piXNU74";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
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