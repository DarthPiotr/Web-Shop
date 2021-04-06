using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace WebShop
{
    public partial class OrdersAdmin : System.Web.UI.Page
    {
        protected bool CanLoad { private set;  get; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetUser(Session) != null)
                CanLoad = Utils.GetUser(Session).IsAdmin;

            if (CanLoad)
                BindData();
        }

        private void BindData()
        {
            StringBuilder whereClause = new StringBuilder();
            bool hasOption = false;

            if (cbRealised.Checked) // zrealizowane
            {
                whereClause.Append(" WHERE [DateExecuted] IS NOT NULL");
                hasOption = true;
            }
            if (cbToRealization.Checked) // do realizacji
            {
                whereClause.Append(hasOption ? " OR" : " WHERE");
                whereClause.Append(" ([DateAccepted] IS NOT NULL AND [DateExecuted] IS NULL)");
                hasOption = true;
            }
            if (cbToConfirm.Checked) // do akceptacji
            {
                whereClause.Append(hasOption ? " OR" : " WHERE");
                whereClause.Append(" [DateAccepted] IS NULL");
                hasOption = true;
            }

            if (!hasOption)
                whereClause.Append(" WHERE [DateMade] IS NULL");

            Debug.WriteLine("Query: " + "SELECT * FROM [Orders]" + whereClause.ToString());
            sqlOrders.SelectCommand = "SELECT * FROM [Orders]" + whereClause.ToString();
            var dv = SQLHelper.SQLSelect(sqlOrders);
            repTab.DataSource = dv;
            repTab.DataBind();
        }

        protected void RepTab_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label l = (Label)e.Item.FindControl("labStatus");

                Debug.WriteLine("DateExecuted: '" + DataBinder.Eval(e.Item.DataItem, "DateExecuted") + "'");
                Debug.WriteLine("DateAccepted: '" + DataBinder.Eval(e.Item.DataItem, "DateAccepted") + "'");
                Debug.WriteLine("DateMade: '" + DataBinder.Eval(e.Item.DataItem, "DateMade") + "'");

                Button btn = ((Button)e.Item.FindControl("btnAct"));

                if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "DateExecuted").ToString()))
                {
                    l.Text = "Zrealizowane";
                    l.ForeColor = Color.Green;
                }
                else if (!String.IsNullOrEmpty(DataBinder.Eval(e.Item.DataItem, "DateAccepted").ToString()))
                {
                    btn.Visible = true;
                    btn.Text = "Zrealizuj";
                    l.Text = "W trakcie realizacji";
                    l.ForeColor = Color.Orange;
                }
                else
                {
                    l.Text = "Oczekuje na zatwierdzenie";
                    btn.Visible = true;
                    btn.Text = "Zatwierdź";
                    l.ForeColor = Color.Red;
                }
            }
        }

        protected void BtnAct_Click(object sender, EventArgs e)
        {
            ButtonItemId btn = sender as ButtonItemId;

            switch (btn.Text)
            {
                case "Zatwierdź":

                    sqlOrders.UpdateCommand = string.Format("UPDATE [Orders] SET [DateAccepted] = GETDATE() WHERE [Id] = {0}", btn.ItemId);
                    Debug.WriteLine("[DEBUG]: Update result: " + sqlOrders.Update());
                    //DataBind();
                    Response.Redirect(Request.Url.AbsoluteUri);
                    break;

                case "Zrealizuj":

                    sqlOrders.UpdateCommand = string.Format("UPDATE [Orders] SET [DateExecuted] = GETDATE() WHERE [Id] = {0}", btn.ItemId);
                    Debug.WriteLine("[DEBUG]: Update result: " + sqlOrders.Update());
                    //DataBind();
                    Response.Redirect(Request.Url.AbsoluteUri);
                    break;
            }
        }
        protected void BtnDel_Click(object sender, EventArgs e) { }
    }
}