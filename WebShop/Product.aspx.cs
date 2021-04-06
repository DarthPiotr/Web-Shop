using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop
{
    public partial class Product : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnAdd.ItemId = Request[UriQuery.PRODUCT];
        }

        protected DataView GetProductData()
        {
            if (Request[UriQuery.PRODUCT] == null) return null;

            sqlGetProductData.SelectCommand = "SELECT [Model], [Manufacturer], [OS], [ScreenSize], [Memory], [RAM], [Processor], [Cores], [Clock], [Camera], [SDCard], [DualSIM], [Price], [Id] FROM[Products] WHERE [Id] = '" + Request[UriQuery.PRODUCT] + "'";
            var dv =SQLHelper.SQLSelect(sqlGetProductData);
            btnAdd.ItemId = dv[0][13].ToString();
            return dv;
        }

        protected DataView GetProductPhotos()
        {
            if (Request[UriQuery.PRODUCT] == null) return null;

            sqlGetProductData.SelectCommand = "SELECT [Path] FROM [Photos] WHERE [ProductId] = '" + Request[UriQuery.PRODUCT] + "' ORDER BY [Main] DESC , [Path]";
            return SQLHelper.SQLSelect(sqlGetProductData);
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Utils.GetUser(Session) != null) // if logged in
            {
                int id = Utils.GetUser(Session).Id;
                ButtonItemId btn = sender as ButtonItemId;
                

                // check if product already in cart
                sqlGetProductData.SelectCommand = String.Format("SELECT [ProductId] FROM [CartItems] WHERE [ProductId] = {1} AND [UserId] = {0}", id, btn.ItemId);
                var data =SQLHelper.SQLSelect(sqlGetProductData);

                if (data.Count == 0)
                {
                    sqlGetProductData.InsertCommand = String.Format("INSERT INTO [CartItems] ([ProductId], [UserId], [Count]) VALUES ({1}, {0}, 1)", id, btn.ItemId);
                    System.Diagnostics.Debug.WriteLine("[DEBUG] Inserting result: " + sqlGetProductData.Insert());
                }
                else
                    Response.Redirect("~/User/Cart.aspx"); // item already in cart
            }
            else
                Response.Redirect("~/User/Login.aspx"); // not logged
        }
    }
}