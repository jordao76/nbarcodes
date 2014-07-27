<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarCodeSample.aspx.cs" Inherits="NBarCodes.Samples.AspNet.BarCodeSample" %>
<%@ Register TagPrefix="nbc" Namespace="NBarCodes.WebUI" Assembly="NBarCodes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NBarCodes Sample Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <div>
            <asp:TextBox id="txtValue" runat="server" Width="300" Text="NBarCodes"></asp:TextBox>
            <asp:RequiredFieldValidator id="vlValidator" runat="server" ErrorMessage="*" ControlToValidate="txtValue"></asp:RequiredFieldValidator>
        </div>

        <div>
		    <asp:DropDownList id="ddlType" runat="server"></asp:DropDownList>
        </div>

        <div>
		    <asp:Button id="btnSubmit" runat="server" Text="Generate" 
                EnableViewState="False" BorderColor="#CCCCCC"></asp:Button>
        </div>

        <div>
            <nbc:BarCodeControl id="BarCodeControl1" runat="server" 
                BackColor="LightSteelBlue" BarColor="Orange" Font="Comic Sans MS, 15pt, style=Bold"
                FontColor="ForestGreen"></nbc:BarCodeControl>
        </div>
    
    </div>
    </form>
</body>
</html>
