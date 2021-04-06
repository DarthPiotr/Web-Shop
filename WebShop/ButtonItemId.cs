using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebShop
{
    [DefaultBindingProperty("ItemId")]
    [ToolboxData("<{0}:ButtonAddToCart runat=server></{0}:ButtonAddToCart  >")]
    public class ButtonItemId : Button
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string ItemId
        {
            get
            {
                if (ViewState["itemid"] == null) return String.Empty;
                return ViewState["itemid"].ToString();
            }
            set
            {
                ViewState["itemid"] = value;
            }
        }
    }
}