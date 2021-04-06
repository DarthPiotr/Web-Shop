using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetUser(Session) != null)
                logName.InnerHtml = "<span class=\"glyphicon glyphicon-user\"></span> " + Utils.GetUser(Session).Username;

            if (Request.Url.AbsoluteUri.IndexOf("/Cart") == -1)
                Session[Utils.EDITCART] = null;

            if (Request.Url.AbsoluteUri.IndexOf("/UserAdmin") == -1)
                Session[Utils.EDITUSER] = null;
        }
        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Logging out...");
            Session[Utils.USERDATA] = null;
            Response.Redirect("~/User/LoginChange.aspx");
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Session[Utils.USERDATA] = new User(13, "Piotr", "Marciniak", "darthpiotr", "p.marciniak@vp.pl", "admin", true);
            Response.Redirect("~/User/LoginChange.aspx");
        }
    }
}