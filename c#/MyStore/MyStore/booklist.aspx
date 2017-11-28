<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="booklist.aspx.cs" Inherits="MyStore.booklist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>    
</head>
<body>
    <form  runat="server">      
    <div>
     <table>
        <tr>
            <td>
                <asp:Button ID="return" runat="server" BorderWidth="0" Text="退出" OnClick="btn_return" />
                <asp:DataGrid ID="BookGrid" runat="server"
                    Width="728px"
                    CellPadding="4"
                    Font-Name="Verdana" Font-Size="8pt"
                    HeaderStylr-BackColor="#aaaadd"
                    OnEditCommand="Buy"
                    DataKeyField="图书编号"
                    AutoGenerateColumns="False"
                    MaintainState="false"
                    Font-Names="Verdana"
                    ForeColor="#333333"
                    GridLines="None"
                    AllowPaging="True"
                    AllowSorting="True"
                    OnPageIndexChanged="BookGrid_PageIndexChanged" PageSize="20">
                    <AlternatingItemStyle BackColor="White" />
                    <Columns> 
                        <asp:EditCommandColumn EditText="购买"  ItemStyle-Wrap="false"> 
                            <HeaderStyle Width="30px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        </asp:EditCommandColumn>

                        <asp:BoundColumn HeaderText="图书编号" DataField="图书编号">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundColumn>

                        <asp:BoundColumn HeaderText="书名" DataField="书名">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundColumn>
                 
                        <asp:BoundColumn HeaderText="作者" DataField="作者">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundColumn>

                        <asp:BoundColumn HeaderText="数量" DataField="数量">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                        </asp:BoundColumn>

                    </Columns>
                    <EditItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" NextPageText="下一页&amp;gt " PrevPageText="&amp;lt上一页" Mode="NumericPages" />
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                </asp:DataGrid>
            </td>
            
        </tr>
        <tr>
            <td>
                <div>
                    <asp:TextBox ID="CInfo" runat="server" Height="14px" Width="116px">书名/图书编号/作者</asp:TextBox>
                    <asp:Button ID="CSearch" runat="server" Text="查找" OnClick="Search" />
                    <asp:Button ID="CReset" runat="server" Text="重置" OnClick="Reset" />
                </div>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
