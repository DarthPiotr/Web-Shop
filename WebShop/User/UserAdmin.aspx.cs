using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        protected bool CanLoad { private set; get; } = false;

        string[] names = { "Name", "Lastname", "Username", "Email", "Type" };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utils.GetUser(Session) != null)
                if (Utils.GetUser(Session).IsAdmin)
                    CanLoad = true;

            if (!IsPostBack)
                BindData();
        }

        private void BindData()
        {
            sqlUsers.SelectCommand = "SELECT [Id], [Name], [Lastname], [Email], [UserName], [Type] FROM [Users]";
            var dv = SQLHelper.SQLSelect(sqlUsers);
            repTab.DataSource = dv;
            repTab.DataBind();
        }

        protected void BtnChange_Click(object sender, EventArgs e)
        {
            ButtonItemId btn = sender as ButtonItemId;
            string rowNum123 = GetRowNum(btn);

            if (Session[Utils.EDITUSER] == null ||
                (Session[Utils.EDITUSER].ToString() != null &&
                Session[Utils.EDITUSER].ToString() != GetRowNum(btn)))
            {
                string rowNum = GetRowNum(btn);
                Session[Utils.EDITUSER] = rowNum;

                for (int i = 0; i < repTab.Items.Count; i++)
                {
                    if(i.ToString() == rowNum)
                    {
                        foreach(string controlId in names)
                        {
                            Label lab = Utils.GetControl<Label>(repTab, "lab"+controlId, i);
                            lab.Visible = false;

                            if (controlId != "Type")
                            {
                                TextBox tb = Utils.GetControl<TextBox>(repTab, "tb" + controlId, i);
                                tb.Text = lab.Text;
                                tb.Visible = true;
                            }
                            else
                            {
                                DropDownList dd = Utils.GetControl<DropDownList>(repTab, "dd" + controlId, i);
                                dd.SelectedIndex = (lab.Text == "user") ? 0 : 1;
                                dd.Visible = true;
                            }
                        }

                        Utils.GetControl<Button>(repTab, "btnChange", i).Text = "Zatwierdź";
                    }
                    else
                    {
                        foreach (string controlId in names)
                        {
                            Label lab = Utils.GetControl<Label>(repTab, "lab" + controlId, i);
                            lab.Visible = true;

                            if (controlId != "Type")
                            {
                                TextBox tb = Utils.GetControl<TextBox>(repTab, "tb" + controlId, i);
                                tb.Visible = false;
                            }
                            else
                            {
                                DropDownList dd = Utils.GetControl<DropDownList>(repTab, "dd" + controlId, i);
                                dd.Visible = false;
                            }
                        }
                        Utils.GetControl<Button>(repTab, "btnChange", i).Text = "Zmień";
                    }
                }
            }
            else
            {
                string rowNum = Session[Utils.EDITUSER].ToString();
                Session[Utils.EDITUSER] = null;

                string[] values = new string[5];

                int i = 0;
                foreach (string controlId in names)
                {
                    Label lab = Utils.GetControl<Label>(repTab, "lab" + controlId, rowNum);
                    lab.Visible = true;

                    if (controlId != "Type")
                    {
                        TextBox tb = Utils.GetControl<TextBox>(repTab, "tb"+controlId, rowNum);
                        values[i++] = tb.Text;
                        tb.Visible = false;
                    }
                    else
                    {
                        DropDownList dd = Utils.GetControl<DropDownList>(repTab, "dd"+controlId, rowNum);
                        values[i++] = dd.SelectedItem.Value;
                        dd.Visible = false;
                    }
                }

                Debug.WriteLine(string.Format("NEW USER DATA: {0}, {1}, {2}, {3}, {4}", values[0], values[1], values[2], values[3], values[4]));

                sqlUsers.UpdateCommand = string.Format("UPDATE [Users] SET [Name] = '{0}', [Lastname] = '{1}', [Username] = '{2}', [Email] = '{3}', [Type] = '{4}' WHERE [Id] = {5}", values[0], values[1], values[2], values[3], values[4], btn.ItemId);
                try
                {
                    sqlUsers.Update();
                }
                catch (Exception ex)
                {
                    Utils.MessageBox("Nieprawidłowe dane. Operacja nie może zostać ukończona. Sprawdź, czy inne konto nie posiada już tego adresu email. \n"+ex.Message, this);
                }

                BindData();
            }
        }
        protected string GetRowNum(Control c)
        {
            return c.ClientID.Substring(c.ClientID.LastIndexOf('_') + 1, c.ClientID.Length - 1 - c.ClientID.LastIndexOf('_'));
        }
    }
}