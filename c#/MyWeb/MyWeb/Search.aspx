<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="MyWeb.Search" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="/DatePicker/WdatePicker.js"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .editcolumn input {
            width: 70px;
            height: 14px;
        }

        .auto-style1 {
            width: 82px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>          
            <asp:Button ID="btnIntroduction" runat="server" BorderWidth="0" Text="订单表" OnClick="btnIntroduction_Click" />
            <asp:Button ID="btnWelcome" runat="server" BorderWidth="0" Text="餐桌表" OnClick="btnIntroduction_Click" />
            <table style="border: 1px ridge #0000FF; width: 100%;">
                <tr>
                    <td>
                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View1" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DataGrid ID="odg" runat="server"
                                                Width="752px"
                                                CellPadding="4"
                                                Font-Name="Verdana" Font-Size="8pt"
                                                HeaderStylr-BackColor="#aaaadd"
                                                OnEditCommand="EditOrder"
                                                OnCancelCommand="Cancel"
                                                OnUpdateCommand="UpdateOrder"
                                                OnDeleteCommand="DeleteOrder"
                                                DataKeyField="单号"
                                                AutoGenerateColumns="False"
                                                MaintainState="false"
                                                Font-Names="Verdana"
                                                ForeColor="#333333"
                                                GridLines="None"
                                                AllowPaging="True"
                                                AllowSorting="True"
                                                OnPageIndexChanged="odg_PageIndexChanged">
                                                <AlternatingItemStyle BackColor="White" />
                                                <Columns>
                                                    <asp:EditCommandColumn EditText="编辑" CancelText="取消" UpdateText="更新" ItemStyle-Wrap="false">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:EditCommandColumn>

                                                    <asp:ButtonColumn Text="删除" CommandName="Delete" ItemStyle-Wrap="false">
                                                        <HeaderStyle Width="30px" />
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:ButtonColumn>

                                                    <asp:TemplateColumn HeaderText="单号">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="OrderID" runat="server" Text='<%# Eval("单号") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="下单时间">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="OrderTime" runat="server" onfocus="WdatePicker()" Height="12px" Width="120px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="85px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="_OrderTime" runat="server" Text='<%# Eval("下单时间") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="预定时间">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="ScheTime" runat="server" onFocus="WdatePicker({minDate:'%y-%M-{%d+1} 08:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" Height="12px" Width="90px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="85px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="_ScheTime" runat="server" Text='<%# Eval("预定时间") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="人数">
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="PeopleNum" runat="server" onfocus="WdatePicker()" Height="12px" Width="25px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="25px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="_PeopleNum" runat="server" Text='<%# Eval("人数") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateColumn>

                                                    <asp:TemplateColumn HeaderText="订单状态">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="OrderState" runat="server" Width="70px" HeaderText="订单状态">
                                                                <asp:ListItem>未完成</asp:ListItem>
                                                                <asp:ListItem>已完成</asp:ListItem>
                                                                <asp:ListItem>已撤销</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="_OrderState" runat="server" Text='<%# Eval("订单状态") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateColumn>

                                                    <asp:BoundColumn HeaderText="下单人" DataField="下单人">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundColumn>

                                                    <asp:BoundColumn HeaderText="联系电话" DataField="联系电话">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundColumn>

                                                    <asp:TemplateColumn HeaderText="桌号">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="OTableNum" runat="server" Width="40px" HeaderText="桌号">
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                        <HeaderStyle Width="40px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="_OTableNum" runat="server" Text='<%# Eval("桌号") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateColumn>

                                                </Columns>
                                                <EditItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
                                                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" NextPageText="下一页&amp;gt " PrevPageText="&amp;lt上一页" Mode="NumericPages" />
                                                <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                            </asp:DataGrid>
                                        </td>
                                        <td>
                                            <table style="font: 8pt verdana;">
                                                <tr>
                                                    <td colspan="2" style="background-color: #aaaadd; font: 8pt verdana;">增加订单：
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap">下单日期:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AOrderTime" runat="server" onfocus="WdatePicker()" Width="136px"></asp:TextBox>
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
                                                    <td style="white-space: nowrap">桌号:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ATableNum" runat="server" Width="136px"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-top: 15px; text-align: center">
                                                        <asp:Button ID="MakeOrder" runat="server" Text="预订" OnClick="Order" Width="136px" Height="17px" />
                                                    </td>
                                                </tr>

                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="SOInfo" runat="server" Height="14px" Width="107px">单号/下单人/手机</asp:TextBox>
                                                <asp:DropDownList ID="SOTableNum" runat="server" Width="65px">
                                                    <asp:ListItem>桌号</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:TextBox ID="SOScheTime" runat="server" Height="14px" Width="94px" onFocus="WdatePicker({errDealMode:2})">预订日期</asp:TextBox>
                                                <asp:TextBox ID="SOOrderTime" runat="server" Height="14px" Width="94px" onFocus="WdatePicker({errDealMode:2})">下单日期</asp:TextBox>
                                                <asp:DropDownList ID="OrderState" runat="server" Width="79px">
                                                    <asp:ListItem>订单状态</asp:ListItem>
                                                    <asp:ListItem>已完成</asp:ListItem>
                                                    <asp:ListItem>未完成</asp:ListItem>
                                                    <asp:ListItem>已取消</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Button ID="BSearchOrder" runat="server" Text="查找" OnClick="SearchOrder" />
                                                <asp:Button ID="ResetOrder" runat="server" Text="重置" OnClick="Reset" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:DataGrid ID="tdg" runat="server"
                                                Width="752px"
                                                CellPadding="4"
                                                Font-Name="Verdana" Font-Size="8pt"
                                                HeaderStylr-BackColor="#aaaadd"
                                                OnEditCommand="EditTable"
                                                OnCancelCommand="Cancel"
                                                OnUpdateCommand="UpdateTable"
                                                OnDeleteCommand="DeleteTable"
                                                DataKeyField="桌号"
                                                AutoGenerateColumns="False"
                                                MaintainState="false"
                                                Font-Names="Verdana"
                                                ForeColor="#333333"
                                                GridLines="None"
                                                AllowPaging="True"
                                                AllowSorting="True"
                                                OnPageIndexChanged="tdg_PageIndexChanged">
                                                <AlternatingItemStyle BackColor="White" />
                                                <Columns>

                                                    <asp:EditCommandColumn EditText="编辑" CancelText="取消" UpdateText="更新" ItemStyle-Wrap="false">
                                                        <HeaderStyle Width="30px" />
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    </asp:EditCommandColumn>

                                                    <asp:ButtonColumn Text="删除" CommandName="Delete" ItemStyle-Wrap="false">
                                                        <HeaderStyle Width="30px" />
                                                        <ItemStyle Wrap="False"></ItemStyle>
                                                    </asp:ButtonColumn>

                                                    <asp:TemplateColumn HeaderText="桌号">
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="TTableNum" runat="server" Text='<%# Eval("桌号") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:TemplateColumn>

                                                    <asp:BoundColumn HeaderText="名字" DataField="名字">
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                                    </asp:BoundColumn>

                                                    <asp:BoundColumn HeaderText="可容人数" DataField="可容人数">
                                                        <HeaderStyle Width="50px" />
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
                                        <td>
                                            <table style="font: 8pt verdana;">
                                                <tr>
                                                    <td colspan="2" style="background-color: #aaaadd; font: 8pt verdana;">增加新桌子：
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap">名字:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tableName" runat="server" Width="136px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="white-space: nowrap">可容人数:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="tableVolume" runat="server" Width="136px"></asp:TextBox>
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <td colspan="2" style="padding-top: 15px; text-align: center">
                                                        <asp:Button ID="AddDTable" runat="server" Text="添加" OnClick="AddTable" Width="136px" />
                                                    </td>
                                                </tr>                                                
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="STInfo" runat="server" Height="14px" Width="66px">桌号/名字</asp:TextBox>
                                                <asp:Button ID="BSearchTable" runat="server" Text="查找" OnClick="SearchTable" />
                                                <asp:Button ID="ResetTable" runat="server" Text="重置" OnClick="Reset" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                            </asp:View>
                        </asp:MultiView>
                    </td>

                </tr>
            </table>
        </div>
        <br />
        <asp:Label ID="sign" runat="server" Text="1" Visible="false"></asp:Label>
    </form>
</body>
</html>
