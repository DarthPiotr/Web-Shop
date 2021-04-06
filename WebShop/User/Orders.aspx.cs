using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data.SqlClient;

namespace WebShop
{
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Utils.GetUser(Session) != null)
            {
                sqlOrders.SelectCommand = String.Format("SELECT * FROM [Orders] WHERE [UserId] = {0}", Utils.GetUser(Session).Id);
                var dv = SQLHelper.SQLSelect(sqlOrders);
                repTab.DataSource = dv;
                repTab.DataBind();
            }
        }

        protected void RepTab_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label l = (Label)e.Item.FindControl("labStatus");

                Debug.WriteLine("DateExecuted: '" + DataBinder.Eval(e.Item.DataItem, "DateExecuted") + "'");
                Debug.WriteLine("DateAccepted: '" + DataBinder.Eval(e.Item.DataItem, "DateAccepted") + "'");
                Debug.WriteLine("DateMade: '" + DataBinder.Eval(e.Item.DataItem, "DateMade") + "'");
                
                if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "DateExecuted").ToString()))
                    l.Text = "Zrealizowane";
                else if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "DateAccepted").ToString()))
                    l.Text = "W trakcie realizacji";
                else
                {
                    l.Text = "Oczekuje na zatwierdzenie";
                    ((Button)e.Item.FindControl("btnDel")).Visible = true;
                }
            }
        }

        protected void BtnDel_Click(object sender, EventArgs e)
        {
            ButtonItemId btn = sender as ButtonItemId;
            string query = string.Format("EXECUTE procRemoveOrder {0}", btn.ItemId);
            Debug.WriteLine("[DEBUG]: query" + query + "returned: " + SQLHelper.MakeQuery(query));
            Response.Redirect("~/User/Orders.aspx");
        }
    }
}