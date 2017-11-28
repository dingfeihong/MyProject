<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MyWeb.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script language="javascript"  type="text/javascript" src="/DatePicker/WdatePicker.js"></script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       
    <div>
        <table style="font: 8pt verdana;">
            <tr>
                <td colspan="2" style="background-color: #aaaadd; font: 8pt verdana;">增加订单：
                </td>
            </tr>            
            <tr>
                <td style="white-space: nowrap">预定日期:
                </td>
                <td>
                    <asp:TextBox ID="AScheTime" runat="server" onFocus="WdatePicker({minDate:'%y-%M-{%d+1} 10:00:00',disabledDates:['....-..-.. 2[0-4]\:00\:00','....-..-.. 0[0-9]\:00\:00'],dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" Width="136px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap">人数:
                </td>
                <td>
                    <asp:TextBox ID="APeopleNum" runat="server" Width="136px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap">下单人:
                </td>
                <td>
                    <asp:TextBox ID="APeople" runat="server" Width="136px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="white-space: nowrap">联系电话:
                </td>
                <td>
                    <asp:TextBox ID="Aphone" runat="server" Width="136px"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td colspan="2" style="padding-top: 15px; text-align: center">
                    <asp:Button ID="MakeOrder" runat="server" Text="预订" OnClick="Order" Width="136px" Height="17px" />
                </td>
            </tr>

        </table>      
    </div>
    <div>
        <asp:Button ID="Table" runat="server" Text="查看" OnClick="Table_Click" />
    </div>
    </form>
</body>
</html>
