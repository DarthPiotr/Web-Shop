using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop
{
    public partial class Cart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = Utils.GetUser(Session);
            // only first time
            if ( user != null && !IsPostBack)
            {
                BindData();
            }

        }

        protected void BtnConfirm_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("BtnConfirm_Click");
            ButtonItemId btn = sender as ButtonItemId;

            if (Session[Utils.EDITCART] == null ||
                (Session[Utils.EDITCART].ToString() != null && Session[Utils.EDITCART].ToString() != GetRowNum(btn))
               )
            {
                string rowNum = GetRowNum(btn);

                for (int i = 0; i < repTab.Items.Count; i++)
                {
                    if (i.ToString() == rowNum)
                    {
                        Debug.WriteLine("Line " + i + " will be edited.");
                        Label lab = Utils.GetControl<Label>(repTab, "labCount", i);
                        lab.Visible = false;
                        TextBox valChange = Utils.GetControl<TextBox>(repTab, "tbCount", i);
                        valChange.Text = lab.Text;
                        valChange.Visible = true;
                        Session[Utils.EDITCART] = rowNum;
                        btn.Text = "Zatwierdź";
                    }
                    else
                    {
                        Debug.WriteLine("Line " + i + " will be aborted.");
                        Label lab = Utils.GetControl<Label>(repTab, "labCount", i);
                        lab.Visible = true;
                        TextBox valChange = Utils.GetControl<TextBox>(repTab, "tbCount", i);
                        valChange.Visible = false;
                        Utils.GetControl<Button>(repTab, "btnConfirm", i).Text = "Zmień";
                    }
                }
            }
            else
            {
                string rowNum = Session[Utils.EDITCART].ToString();
                Session[Utils.EDITCART] = null;
                TextBox valChange = Utils.GetControl<TextBox>(repTab, "tbCount", rowNum);
                valChange.Visible = false;
                Label lab = Utils.GetControl<Label>(repTab, "labCount", rowNum);
                lab.Visible = true;
                btn.Text = "Zmień";
                int newCount = Convert.ToInt32(valChange.Text) < 1 ? 1 : Convert.ToInt32(valChange.Text);
                lab.Text = newCount.ToString();
                Debug.WriteLine("[DEBUG]: Entered Value is: " + valChange.Text + ". Able to update database");

                sqlGetCart.UpdateCommand = String.Format("UPDATE [CartItems] SET [Count] = {1} WHERE [ProductId] = {2} AND [UserId] = {0}", Utils.GetUser(Session).Id, newCount, btn.ItemId);
                Debug.WriteLine("[DEBUG]: Update: " + sqlGetCart.Update());
                BindData();
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            ButtonItemId btn = sender as ButtonItemId;
            sqlGetCart.DeleteCommand = String.Format("DELETE FROM [CartItems] WHERE [ProductId] = {1} AND [UserId] = {0}", Utils.GetUser(Session).Id, btn.ItemId);
            Debug.WriteLine("[DEBUG]: Delete form cart status: " + sqlGetCart.Delete());
            Session[Utils.EDITCART] = null;
            Response.Redirect("~/User/Cart.aspx");
        }
        private void BindData()
        {
            sqlGetCart.SelectCommand = String.Format("SELECT [Model], [Manufacturer], [Path], [Price], [ProductId], [Count] FROM [ProductsOneImage] INNER JOIN [CartItems] ON [CartItems].[ProductId] = [ProductsOneImage].[Id] WHERE [UserId] = {0}", Utils.GetUser(Session).Id);
            var dv = SQLHelper.SQLSelect(sqlGetCart);
            repTab.DataSource = dv;
            Debug.WriteLine("DataBind()");
            repTab.DataBind();

            double sum = 0;
            foreach (DataRowView x in dv)
                sum += Convert.ToDouble(x.Row["Price"]) * Convert.ToInt32(x.Row["Count"]);

            GetControlInRepeaterFooter<Label>("labSum").Text = String.Format("{0:N2} zł", sum);
        }

        protected T GetControlInRepeaterFooter<T>(string name) where T : Control
        {
            return (T)repTab.Controls[repTab.Controls.Count - 1].Controls[0].FindControl(name);
        }
        protected string GetRowNum(Control c)
        {
            return c.ClientID.Substring(c.ClientID.LastIndexOf('_') + 1, c.ClientID.Length - 1 - c.ClientID.LastIndexOf('_'));
        }
        protected void BtnMakeOrder_Click(object sender, EventArgs e)
        {
            if (SQLHelper.CartCount(Utils.GetUser(Session).Id) < 1)
                Utils.MessageBox("Koszyk jest aktualnie pusty. Dodaj przedmioty do koszyka, aby złożyć zamówienie.", this);
            else
            {
                string query = string.Format("EXECUTE procAddOrder {0}", Utils.GetUser(Session).Id);
                Debug.WriteLine("[DEBUG]: query" + query + "returned: " + SQLHelper.MakeQuery(query));
                Session[Utils.EDITCART] = null;
                Response.Redirect("~/User/Orders.aspx");
            }
        }

        protected void btnMakeOrder_Load(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (!Utils.GetUser(Session).Active)
            {
                btn.Enabled = false;
                btn.Visible = false;
                btn.ToolTip = "Potwierdź email aby dokonać zakupu.";
            }

        }
    }
}