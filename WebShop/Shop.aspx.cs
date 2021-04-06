using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;
using System.Data;
using System.Web.UI.HtmlControls;

namespace WebShop
{
    public partial class Shop : System.Web.UI.Page
    {
        protected int PageNum { get; set; }
        protected int ProductsPerPage { get; set; }

        protected DataView ProductsList { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            // fill list manufacturers
            sqlGetFilterData.SelectCommand = SQLFilterQuery.ManufacturerCount;
            FillOptionsList(cbManufacturer,SQLHelper.SQLSelect(sqlGetFilterData));

            // fill screen sizes
            sqlGetFilterData.SelectCommand = SQLFilterQuery.ScreenSizeCount;
            var screens =SQLHelper.SQLSelect(sqlGetFilterData);
            int index;
            for (int i = 0; i < screens.Count; i++)
            {
                index = Convert.ToInt32(screens[i][0]);
                cbScreen.Items[index].Text = cbScreen.Items[index].Text.Replace("(0)", "(" + screens[i][1] + ")");
            }

            //fill list Operating System
            sqlGetFilterData.SelectCommand = SQLFilterQuery.OSCount;
            FillOptionsList(cbOperatingSystem,SQLHelper.SQLSelect(sqlGetFilterData));

            //fill list Memory
            sqlGetFilterData.SelectCommand = SQLFilterQuery.MemoryCount;
            FillOptionsList(cbMemory,SQLHelper.SQLSelect(sqlGetFilterData), "GB");

            //fill list RAM
            sqlGetFilterData.SelectCommand = SQLFilterQuery.RAMCount;
            FillOptionsList(cbRAM,SQLHelper.SQLSelect(sqlGetFilterData), "GB");

            //fill list Processor
            sqlGetFilterData.SelectCommand = SQLFilterQuery.ProcessorCount;
            FillOptionsList(cbProcessor,SQLHelper.SQLSelect(sqlGetFilterData));

            //fill list Cores
            sqlGetFilterData.SelectCommand = SQLFilterQuery.CoresCount;
            FillOptionsList(cbCores,SQLHelper.SQLSelect(sqlGetFilterData));

            //fill list Clock
            sqlGetFilterData.SelectCommand = SQLFilterQuery.ClockCount;
            FillOptionsList(cbClock,SQLHelper.SQLSelect(sqlGetFilterData), "GHz");

            //fill list Camera
            sqlGetFilterData.SelectCommand = SQLFilterQuery.CameraCount;
            FillOptionsList(cbCamera,SQLHelper.SQLSelect(sqlGetFilterData), "MPix");

            //fill list SDCard
            sqlGetFilterData.SelectCommand = SQLFilterQuery.SDCardCount;
            FillOptionsList(cbSDCard,SQLHelper.SQLSelect(sqlGetFilterData), "Tak", "Nie");

            //fill list DualSIM
            sqlGetFilterData.SelectCommand = SQLFilterQuery.DualSimCount;
            FillOptionsList(cbDualSim,SQLHelper.SQLSelect(sqlGetFilterData), "Tak", "Nie");

            // page changed
            if (Session[Utils.PAGE] != null)
                PageNum = (int)Session[Utils.PAGE];
            else
                Session[Utils.PAGE] = PageNum = 1;

            // products per page changed
            if (Session[Utils.PRODUCTSPERPAGE] == null) // page fist load, set default values
            {
                Session[Utils.PRODUCTSPERPAGE] = ProductsPerPage = 20;
                ddPagination.SelectedIndex = 3;
            }
            // dropdown caused postback, set new values
            else if (Utils.GetControlThatCausedPostBack(this)?.ID == "ddPagination")
            {
                Session[Utils.PRODUCTSPERPAGE] = ProductsPerPage = Convert.ToInt32(ddPagination.SelectedItem.Value);
                Session[Utils.PAGE] = PageNum = 1; // back to fist page
            }
            // some action happened, restore values from session
            else
            {
                ProductsPerPage = Convert.ToInt32(Session[Utils.PRODUCTSPERPAGE]);
                for (int i = 0; i < ddPagination.Items.Count; i++)
                    if (ddPagination.Items[i].Value == Session[Utils.PRODUCTSPERPAGE].ToString())
                    {
                        ddPagination.SelectedIndex = i;
                        break;
                    }
            }

            Debug.WriteLine("[DEBUG]: Element that caused postback: " + Utils.GetControlThatCausedPostBack(this)?.ID);

            AddQueryConditionsPaged();
            ProductsList =SQLHelper.SQLSelect(sqlGetProducts);
            repeater.DataSource = ProductsList;
            repeater.DataBind();

            AddPagination();
        }

        /// <summary>
        /// fill list of options. Option does not requre special suffix or translation
        /// </summary>
        private void FillOptionsList(CheckBoxList cbl, DataView dv)
        {
            if (cbl.Items.Count == 0)
                for (int i = 0; i < dv.Count; i++)
                    cbl.Items.Add(
                        new ListItem(" " + dv[i][0] + " (" + dv[i][1] + ")", dv[i][0].ToString())
                        );
        }
        /// <summary>
        /// fill list of options. Option requre special suffix
        /// </summary>
        private void FillOptionsList(CheckBoxList cbl, DataView dv, string suffix)
        {
            if (cbl.Items.Count == 0)
                for (int i = 0; i < dv.Count; i++)
                    cbl.Items.Add(
                        new ListItem(" " + dv[i][0] + " " + suffix + " (" + dv[i][1] + ")", dv[i][0].ToString())
                        );
        }
        /// <summary>
        /// fill list of options. Option type true/false
        /// </summary>
        private void FillOptionsList(CheckBoxList cbl, DataView dv, string sTrue, string sFalse)
        {
            if (cbl.Items.Count == 0)
                for (int i = 0; i < dv.Count; i++)
                    cbl.Items.Add(
                        new ListItem(" " + (i == 0 ? sTrue : sFalse) + " (" + dv[i][1] + ")", dv[i][0].ToString())
                        );
        }

        private void DebugQuery(string uriquery) =>
            Debug.WriteLine(String.Format("[QUERYBUILDER] Adding {0}: {1}", uriquery, Request[uriquery]));

        /// <summary>
        /// Builds SQL Query. Called when page is being loaded.
        /// </summary>
        protected void AddQueryConditionsPaged()
        {
            StringBuilder whereClause = GetWherePart();
            StringBuilder orderByClause = GetOrderbyPart();

            int minRow = (PageNum - 1) * ProductsPerPage + 1,
                maxRow = PageNum * ProductsPerPage;
            StringBuilder query = new StringBuilder();
            query.AppendFormat("SELECT [Model], [Manufacturer], [Memory], [RAM], [ScreenSize], [Camera], [Path], [Price], [Id] FROM (SELECT ROW_NUMBER() OVER({1}) AS RowNum, * FROM [ProductsOneImage] {0}) AS RowConstrainedResult WHERE RowNum >= {2} AND RowNum <= {3} ORDER BY RowNum",
                whereClause.ToString(),
                orderByClause.ToString(),
                minRow,
                maxRow);

            // apply query
            sqlGetProducts.SelectCommand = query.ToString();
            Debug.WriteLine("[Debug]: Current select query: " + sqlGetProducts.SelectCommand);
        }
        /// <summary>
        /// Builds SQL Query. Called to count pagination
        /// </summary>
        protected void AddQueryConditionsCount()
        {
            StringBuilder whereClause = GetWherePart();

            StringBuilder query = new StringBuilder();
            query.AppendFormat("SELECT COUNT(*) FROM [ProductsOneImage] {0}", whereClause.ToString());

            sqlGetProducts.SelectCommand = query.ToString();
            Debug.WriteLine("[Debug]: Current select query: " + sqlGetProducts.SelectCommand);
        }

        /// <summary>
        /// Get WHERE part of SQL query based on flter options
        /// </summary>
        private StringBuilder GetWherePart()
        {
            // Extend sql select command
            StringBuilder whereClause = new StringBuilder();
            bool conditionExists = false;

            // add minimum price condition
            if (Request[UriQuery.MINPRICE] != null)
            {
                DebugQuery(UriQuery.MINPRICE);
                whereClause.AppendFormat(" WHERE [Price] >= {0}", Request[UriQuery.MINPRICE]);
                conditionExists = true;
            }

            // add maximum price condition
            if (Request[UriQuery.MAXPRICE] != null)
            {
                DebugQuery(UriQuery.MAXPRICE);
                if (conditionExists) whereClause.Append(" AND");
                else whereClause.Append(" WHERE");
                whereClause.AppendFormat(" [Price] <= {0}", Request[UriQuery.MAXPRICE]);
                conditionExists = true;
            }

            AddSQLFromUriQuery(whereClause, UriQuery.MANUFACTURERS, "Manufacturer", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.OS, "OS", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.MEMORY, "Memory", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.RAM, "RAM", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.PROCESSOR, "Processor", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.CORES, "Cores", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.CLOCK, "Clock", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.CAMERA, "Camera", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.SDCARD, "SDCard", ref conditionExists);
            AddSQLFromUriQuery(whereClause, UriQuery.DUALSIM, "DualSIM", ref conditionExists);

            // screen size
            if (Request[UriQuery.SCREENSIZE] != null)
            {
                DebugQuery(UriQuery.SCREENSIZE);

                //if price or manufacturer specified
                if (conditionExists)
                    whereClause.Append(" AND (");
                // there is no condition yet
                else whereClause.Append(" WHERE (");

                bool sizeExists = false;
                var nfi = new NumberFormatInfo() { NumberDecimalSeparator = "." };
                for (int i = 0; i < cbScreen.Items.Count; i++)
                {
                    if (Request[UriQuery.SCREENSIZE][i] == '1')
                    {
                        if (i == 0)
                        {
                            whereClause.Append("([ScreenSize] <= 4)");
                            sizeExists = true;
                        }
                        else if (i == cbScreen.Items.Count - 1)
                        {
                            if (sizeExists) whereClause.Append(" OR ");
                            whereClause.AppendFormat("([ScreenSize] >= {0})", (3.5 + i * 0.5).ToString(nfi));
                            sizeExists = true;
                        }
                        else
                        {
                            if (sizeExists) whereClause.Append(" OR ");

                            whereClause.AppendFormat(
                                "([ScreenSize] >= {0} AND [ScreenSize] <= {1})",
                                // minimum value
                                (3.5 + i * 0.5).ToString(nfi),
                                // maximum value
                                (4 + i * 0.5).ToString(nfi)
                                );
                            sizeExists = true;
                        }
                    }
                }
                whereClause.Append(")");
            }
            return whereClause;
        }
        /// <summary>
        /// Get ORDER BY part of SQL query based on flter options
        /// </summary>
        private StringBuilder GetOrderbyPart()
        {
            StringBuilder orderByClause = new StringBuilder();
            // sorting
            switch (ddSort.SelectedIndex)
            {
                case 0: // cena rosnąco
                    orderByClause.Append(" ORDER BY [Price] ASC ");
                    break;
                case 1: // cena malejąco
                    orderByClause.Append(" ORDER BY [Price] DESC ");
                    break;
                case 2: // nazwa rosnąco
                    orderByClause.Append(" ORDER BY [Manufacturer] ASC, [Model] ASC ");
                    break;
                case 3: // nazwa malejąco
                    orderByClause.Append(" ORDER BY [Manufacturer] DESC, [Model] DESC ");
                    break;

            }
            return orderByClause;
        }

        /// <summary>
        /// Adds query conditions from check box list type options
        /// </summary>
        private void AddSQLFromUriQuery(StringBuilder queryBuilder, string uriQueryName, string columnName, ref bool conditionExists)
        {
            if (Request[uriQueryName] != null)
            {
                DebugQuery(uriQueryName);

                // if there is price condition specified
                if (conditionExists)
                    queryBuilder.Append(" AND [" + columnName + "] IN (");
                // there is no price
                else queryBuilder.Append(" WHERE [" + columnName + "] IN (");

                string paramsList = Request[uriQueryName];
                while (paramsList.IndexOf(",") != -1)
                {
                    queryBuilder.AppendFormat("'{0}', ", paramsList.Substring(0, paramsList.IndexOf(",")));
                    paramsList = paramsList.Remove(0, paramsList.IndexOf(",") + 1);
                }
                queryBuilder.AppendFormat("'{0}')", paramsList);
                conditionExists = true;
            }
        }

        /// <summary>
        /// Sets the value of uri query
        /// </summary>
        protected void BtnFilter_Click(object sender, EventArgs e)
        {
            // build a uri with filter parameters
            int minPrice = 0;
            int maxPrice = 0;

            // start with no parameters
            var uriBuilder = new UriBuilder(
                Request.Url.Query.Length > 0 ? Request.Url.AbsoluteUri.Replace(Request.Url.Query, "") : Request.Url.AbsoluteUri
                );
            var paramValues = HttpUtility.ParseQueryString(uriBuilder.Query);

            try // minimum price
            {
                minPrice = Convert.ToInt32(tbPriceMin.Text);
                if (minPrice >= 0)
                    paramValues.Add(UriQuery.MINPRICE, minPrice.ToString());
            }
            catch
            { Debug.WriteLine("Could not convert textbox to number."); }

            try // maximum price
            {
                maxPrice = Convert.ToInt32(tbPriceMax.Text);
                if (maxPrice >= minPrice)
                    paramValues.Add(UriQuery.MAXPRICE, maxPrice.ToString());
            }
            catch
            { Debug.WriteLine("Could not convert textbox to number."); }


            AddParametersFromOptions(paramValues, cbManufacturer, UriQuery.MANUFACTURERS);
            AddParametersFromOptions(paramValues, cbOperatingSystem, UriQuery.OS);
            AddParametersFromOptions(paramValues, cbMemory, UriQuery.MEMORY);
            AddParametersFromOptions(paramValues, cbRAM, UriQuery.RAM);
            AddParametersFromOptions(paramValues, cbProcessor, UriQuery.PROCESSOR);
            AddParametersFromOptions(paramValues, cbCores, UriQuery.CORES);
            AddParametersFromOptions(paramValues, cbClock, UriQuery.CLOCK);
            AddParametersFromOptions(paramValues, cbCamera, UriQuery.CAMERA);
            AddParametersFromOptions(paramValues, cbSDCard, UriQuery.SDCARD);
            AddParametersFromOptions(paramValues, cbDualSim, UriQuery.DUALSIM);

            // screen size
            StringBuilder sb = new StringBuilder();
            bool screenSelected = false;
            foreach (ListItem item in cbScreen.Items)
            {
                sb.Append(item.Selected ? "1" : "0");
                if (item.Selected) screenSelected = true;
            }
            if (screenSelected)
                paramValues.Add(UriQuery.SCREENSIZE, sb.ToString());

            // Build query string
            uriBuilder.Query = paramValues.ToString();
            // Redirect with query string
            Debug.WriteLine("[DEBUG]: Uri query string: " + uriBuilder.Uri.AbsoluteUri);
            Session[Utils.PAGE] = 1;
            Response.Redirect(uriBuilder.Uri.AbsoluteUri);
        }

        /// <summary>
        /// Addp pagination and calculates pages count
        /// </summary>
        protected void AddPagination()
        {
            Debug.WriteLine("[DEBUG]: Adding Pagination");
            AddQueryConditionsCount();
            var prodlist = SQLHelper.SQLSelect(sqlGetProducts);
            int pages = (int)Math.Ceiling((Convert.ToDouble(prodlist[0][0]) / ProductsPerPage));

            if (pages > 1)
                for (int i = 1; i <= pages; i++)
                {
                    LinkButton upper = new LinkButton
                    {
                        ID = "pageUpper" + i,
                        Text = i.ToString()
                    };
                    LinkButton lower = new LinkButton
                    {
                        ID = "pageLower" + i,
                        Text = i.ToString()
                    };
                    upper.Click += new EventHandler(Pagination_ServerClick);
                    lower.Click += new EventHandler(Pagination_ServerClick);
                    paginationListUpper.Controls.Add(upper);
                    paginationListLower.Controls.Add(lower);
                }
        }

        /// <summary>
        /// Automated adding parameters to uri query
        /// </summary>
        /// <param name="parameters">Parameters list</param>
        /// <param name="cbl">Checkbox list connected with category</param>
        /// <param name="uriQueryName">Param name in uri</param>
        private void AddParametersFromOptions(NameValueCollection parameters, CheckBoxList cbl, string uriQueryName)
        {
            Debug.WriteLine("[DEBUG]: Adding " + uriQueryName + " to the uri query string");
            Debug.WriteLine("[DEBUG]: Current query: " + parameters.ToString());
            bool selected = false; // clear flag
            StringBuilder sb = new StringBuilder();
            foreach (ListItem item in cbl.Items)
            {
                // if item selected
                if (item.Selected)
                {
                    Debug.WriteLine("[DEBUG]: Found selected item: " + item.Value);
                    if (selected) // if flag toggled, add comma
                        sb.Append(",");
                    sb.Append(item.Value); // add item to parameter list
                    selected = true; // toggle flag 
                }
            }
            if (selected)
            {
                parameters.Add(uriQueryName, sb.ToString());
                Debug.WriteLine("[DEBUG]: Added: " + sb.ToString());
            }
        }

        /// <summary>
        /// Handler for page change via pagination
        /// </summary>
        public void Pagination_ServerClick(object sender, EventArgs e)
        {
            LinkButton lb = sender as LinkButton;
            Session[Utils.PAGE] = PageNum = Convert.ToInt32(lb.Text);
            Debug.WriteLine("[DEBUG]: Page is now: " + PageNum);
            Response.Redirect(Request.Url.AbsoluteUri);
        }
        /// <summary>
        /// Clears filter on button click
        /// </summary>
        protected void LbClearFilter_Click(object sender, EventArgs e)
        {
            // redirect to clear shop
            Session[Utils.PAGE] = 1;
            Response.Redirect(Request.Url.Query.Length > 0 ? Request.Url.AbsoluteUri.Replace(Request.Url.Query, "") : Request.Url.AbsoluteUri);
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            if (Utils.GetUser(Session) != null) // if logged in
            {
                int id = Utils.GetUser(Session).Id;
                ButtonItemId btn = sender as ButtonItemId;

                // check if product already in cart
                sqlCart.SelectCommand = String.Format("SELECT [ProductId] FROM [CartItems] WHERE [ProductId] = {1} AND [UserId] = {0}", id, btn.ItemId);
                var data =SQLHelper.SQLSelect(sqlCart);

                if (data.Count == 0)
                {
                    sqlCart.InsertCommand = String.Format("INSERT INTO [CartItems] ([ProductId], [UserId], [Count]) VALUES ({1}, {0}, 1)", id, btn.ItemId);
                    Debug.WriteLine("[DEBUG] Inserting result: " + sqlCart.Insert());
                }
                else
                    Response.Redirect("~/User/Cart.aspx"); // item already in cart
            }
            else
                Response.Redirect("~/User/Login.aspx"); // not logged
        }
    }
}