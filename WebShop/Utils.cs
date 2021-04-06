using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop
{
    public static class Utils
    {
        public static readonly string USERDATA = "userdata";
        public static readonly string FIRSTREGISTER = "firstRegister";
        public static readonly string REMOVEACCOUNT = "removeAccount";
        public static readonly string PASSWDCHANGED = "passwdChanged";
        public static readonly string EMAILCHANGING = "emailChanging";
        public static readonly string EMAILCHANGED = "emailChanged";
        public static readonly string FILTERSTRING = "filterString";
        public static readonly string PAGE = "page";
        public static readonly string PRODUCTSPERPAGE = "productsperpage";
        public static readonly string EDITCART = "editcart";
        public static readonly string EDITUSER = "edituser";

        public static User GetUser(HttpSessionState Session) =>
            (User)Session[Utils.USERDATA];
        public static void MessageBox(string message, Control c) =>
            ScriptManager.RegisterClientScriptBlock(c, c.GetType(), "alertMessage", "alert('" + message + "')", true);
        
        public static Control GetControlThatCausedPostBack(Page page)
        {
            //initialize a control and set it to null
            Control ctrl = null;

            //get the event target name and find the control
            string ctrlName = page.Request.Params.Get("__EVENTTARGET");
            if (!String.IsNullOrEmpty(ctrlName))
                ctrl = page.FindControl(ctrlName);

            //return the control to the calling method
            return ctrl;
        }
        public static T GetControl<T>(Repeater r, string name, string rowNum) where T : Control => 
            GetControl<T>(r, name, Convert.ToInt32(rowNum));
        public static T GetControl<T>(Repeater r, string name, int rowNum) where T : Control =>
            (T)r.Items[rowNum].FindControl(name);
    }

    public static class SQLHelper
    {
        public static int CartCount(int userID)
        {
            SqlDataSource sqlCartCount = new SqlDataSource
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString,
                SelectCommand = String.Format("SELECT COUNT(*) AS [Count] FROM [CartItems] WHERE [UserId] = {0}", userID)
            };
            SQLSelect(sqlCartCount);
            var dv = SQLSelect(sqlCartCount);
            return Convert.ToInt32(dv[0]["Count"]);
        }
        public static DataView SQLSelect(SqlDataSource sqlds) =>
            (DataView)sqlds.Select(DataSourceSelectArguments.Empty);

        public static int MakeQuery(string query)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            conn.Open();
            SqlCommand newCmd = new SqlCommand(query, conn);
            int exit = newCmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return exit;
        }
    }

    public static class UriQuery
    {
        public static readonly string MINPRICE = "minprice";
        public static readonly string MAXPRICE = "maxprice";
        public static readonly string MANUFACTURERS = "manufacturers";
        public static readonly string SCREENSIZE = "screensize";
        public static readonly string OS = "os";
        public static readonly string MEMORY = "memory";
        public static readonly string RAM = "ram";
        public static readonly string PROCESSOR = "processor";
        public static readonly string CORES = "cores";
        public static readonly string CLOCK = "clock";
        public static readonly string CAMERA = "camera";
        public static readonly string SDCARD = "sdcard";
        public static readonly string DUALSIM = "dualsim";
        public static readonly string PRODUCT = "product";
        public static readonly string ORDER = "order";

        public static bool HasActivationString(HttpRequest request)
        {
            System.Diagnostics.Debug.WriteLine("Sprawdzanie czy jest próba aktywacji");
            return request.QueryString["ver"] != null;
        }
        public static bool ValidActivation(HttpRequest request)
        {
            System.Diagnostics.Debug.WriteLine("Sprawdzanie czy kod jest prawidłowy");
            if (HasActivationString(request))
            {
                string ver = request.QueryString["ver"];
                ver = ver.Replace(' ', '+');
                SqlDataSource sql = new SqlDataSource
                {
                    ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\pmarc\\Documents\\Visual Studio 2017\\Projects\\WebShop\\WebShop\\App_Data\\ShopDB.mdf\";Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework",
                    SelectCommand = "SELECT [UserId] FROM [Verification] WHERE [Code] = '"+ver+"'"
                };
                System.Diagnostics.Debug.WriteLine(sql.SelectCommand);
                var dv = SQLHelper.SQLSelect(sql);
                if (dv.Count > 0)
                {
                    sql.UpdateCommand = "UPDATE [Users] SET [Active] = 1 WHERE [Id] = " + dv[0]["UserId"];
                    sql.Update();
                    sql.DeleteCommand = "DELETE FROM [Verification] WHERE [UserId] = " + dv[0]["UserId"];
                    sql.Delete();
                    return true;
                }
                System.Diagnostics.Debug.WriteLine("Wykonano zapytanie");
            }
            return false;
        }
    }

    public static class SQLFilterQuery
    {
        public static readonly string ManufacturerCount = "SELECT [Manufacturer], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [Manufacturer]";
        public static readonly string MemoryCount = "SELECT [Memory], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [Memory]";
        public static readonly string RAMCount = "SELECT [RAM], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [RAM]";
        public static readonly string ProcessorCount = "SELECT [Processor], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [Processor]";
        public static readonly string CoresCount = "SELECT [Cores], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [Cores]";
        public static readonly string ClockCount = "SELECT [Clock], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [Clock]";
        public static readonly string CameraCount = "SELECT [Camera], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [Camera]";
        public static readonly string OSCount = "SELECT [OS], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [OS]";
        public static readonly string ScreenSizeCount = "SELECT [size group], [number of occurences] FROM [ScreenViewCount] ORDER BY [size group]";
        public static readonly string SDCardCount = "SELECT [SDCard], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [SDCard] ORDER BY [SDCard] DESC";
        public static readonly string DualSimCount = "SELECT [DualSIM], COUNT([Id]) AS [Count] FROM [Products] GROUP BY [DualSIM] ORDER BY [DualSIM] DESC";
    }
}