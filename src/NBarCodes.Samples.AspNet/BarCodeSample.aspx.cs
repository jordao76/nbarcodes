using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace NBarCodes.Samples.AspNet
{
    public partial class BarCodeSample : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.DataSource = new EnumConverter(typeof(BarCodeType)).GetStandardValues();
                ddlType.DataBind();
                ddlType.SelectedIndex = (int)BarCodeType.Code128;
            }

        }

        private void GenerateBarCode()
        {
            BarCodeControl1.Type = (BarCodeType)Enum.Parse(typeof(BarCodeType), ddlType.SelectedValue);
            BarCodeControl1.Data = txtValue.Text;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            GenerateBarCode();
        }
    }
}