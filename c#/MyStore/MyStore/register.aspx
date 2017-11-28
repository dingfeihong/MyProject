<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="MyStore.register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td> 
                     用户名&nbsp：<asp:TextBox ID="Ruser" runat="server" Height="12px" Width="55px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                     密码&nbsp&nbsp：<asp:TextBox ID="Rpsd" runat="server" Height="12px" Width="55px"></asp:TextBox>                          
                </td>
            </tr>
            <tr>
                <td>
                     联系方式：<asp:TextBox ID="Rphone" runat="server" Height="12px" Width="55px"></asp:TextBox>                      
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="注册" OnClick="Register" />
                    <asp:Button ID="Button2" runat="server" Text="返回" OnClick="btn_return" />
                </td>
            </tr>
        </table>
    
                           
    </div>
    </form>
</body>
</html>
