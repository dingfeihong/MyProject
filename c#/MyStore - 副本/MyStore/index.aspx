<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="MyStore.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" margin:0px auto; height:100px; "></div>
    <div style=" margin:0px auto; height:300px; "> 
        <div style="float:left; width:35%; height:100px;"></div>
        <div style="float:left; width:60%; height:200px; ">   
            <table>
                <tr>
                    <td>
                        用户名：<asp:TextBox ID="Cuser" runat="server" Width="130px" ></asp:TextBox>                
                    </td>
                </tr>
                <tr>
                    <td>
                        密码&nbsp：<asp:TextBox ID="Cpassword" runat="server" Width="130px" TextMode="Password" ></asp:TextBox>                
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="Clog_in" runat="server" Text="登录" OnClick="Clog_in_Click" />
                        <asp:Button ID="Creg" runat="server" Text="注册" OnClick="CRegister" />
                        <asp:Label ID="Csign" runat="server" Text=" "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>默认用户 账户：user  密码：123</td>            
                </tr>
                <tr>
                    <td>默认用户 账户：user  密码：123</td>
                </tr>
            </table>
        </div>
        <div style="float:left; width:20%; height:100px;"></div>  
    </div>
    <div style=" margin:0px auto; height:50px; "></div>
 
  
    </form>
</body>
</html>
