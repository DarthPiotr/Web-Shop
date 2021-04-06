using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Data;

namespace WebShop
{
    public partial class OrderDetails : System.Web.UI.Page
    {
        protected bool CanLoad { private set; get; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            // only first time
            if (Utils.GetUser(Session) != null && !IsPostBack)
                CanLoad = BindData();
        }

        private bool BindData()
        {
            sqlGetOrder.SelectCommand = String.Format("SELECT [Orders].[Id] AS [OrderId], [UserId], [OrderItems].[ProductId], [Model], [Manufacturer], [Price], [Count], [Path] FROM[Orders] INNER JOIN ([OrderItems] INNER JOIN [ProductsOneImage] ON [OrderItems].[ProductId] = [ProductsOneImage].[Id]) ON[Orders].[Id] = [OrderItems].[OrdersId] WHERE[OrdersId] = {0}", Request[UriQuery.ORDER]);
            var dv = SQLHelper.SQLSelect(sqlGetOrder);

            if (dv.Count > 0)
                if (!Utils.GetUser(Session).IsAdmin && dv[0]["UserId"].ToString() != Utils.GetUser(Session).Id.ToString())
                    return false;


            repTab.DataSource = dv;
            Debug.WriteLine("DataBind()");
            repTab.DataBind();

            GetControlInRepeaterHeader<Label>("labOrderName").Text = "Zamówienie " + dv[0]["OrderId"];

            double sum = 0;
            foreach (DataRowView x in dv)
                sum += Convert.ToDouble(x.Row["Price"]) * Convert.ToInt32(x.Row["Count"]);

            GetControlInRepeaterFooter<Label>("labSum").Text = String.Format("{0:N2} zł", sum);
            return true;
        }
        protected T GetControlInRepeaterFooter<T>(string name) where T : Control
        {
            return (T)repTab.Controls[repTab.Controls.Count - 1].Controls[0].FindControl(name);
        }

        protected T GetControlInRepeaterHeader<T>(string name) where T : Control
        {
            return (T)repTab.Controls[0].Controls[0].FindControl(name);
        }
    }
}