<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="MyStore.Manage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="/DatePicker/WdatePicker.js"></script>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 491px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="return" runat="server" BorderWidth="0" Text="退出" OnClick="btn_return" />
        <asp:Button ID="btnBook" runat="server" BorderWidth="0" Text="书籍管理" OnClick="btn_Click" />  
        <asp:Button ID="btnOrder" runat="server" BorderWidth="0" Text="订单管理" OnClick="btn_Click" />  
        <asp:Button ID="btnAccount" runat="server" BorderWidth="0" Text="账户管理" OnClick="btn_Click" />  
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="Vbook"  runat="server">
                <table>
                    <tr>
                        <td class="auto-style1">
                            <asp:DataGrid ID="BookGrid" runat="server"
                                Width="728px"
                                CellPadding="4"
                                Font-Name="Verdana" Font-Size="8pt"
                                HeaderStylr-BackColor="#aaaadd"
                                OnEditCommand="EditBook"
                                OnCancelCommand="Cancel"
                                OnUpdateCommand="UpdateBook"
                                OnDeleteCommand="DeleteBook"
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
                                     <asp:EditCommandColumn EditText="编辑" CancelText="取消" UpdateText="更新" ItemStyle-Wrap="false">
                                        <HeaderStyle Width="60px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    </asp:EditCommandColumn>

                                    <asp:ButtonColumn Text="删除" CommandName="Delete" ItemStyle-Wrap="false">
                                        <HeaderStyle Width="30px" />
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:ButtonColumn>
                                  
                                    <asp:TemplateColumn HeaderText="图书编号">
                                        <HeaderStyle Width="60px" />
                                        <ItemTemplate>
                                            <asp:Label ID="OrderID" runat="server" Text='<%# Eval("图书编号") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn HeaderText="书名" DataField="书名"/>                                      
                                    <asp:BoundColumn HeaderText="作者" DataField="作者"/>     
                                    <asp:BoundColumn HeaderText="数量" DataField="数量"/>                                       

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
                        <td class="auto-style1">                            
                            <asp:TextBox ID="CInfo" runat="server" Height="14px" Width="116px">书名/图书编号/作者</asp:TextBox>
                            <asp:Button ID="CSearch" runat="server" Text="查找" OnClick="SearchBook" />
                            <asp:Button ID="CReset" runat="server" Text="重置" OnClick="Reset" />
                        </td>
                    </tr>

                    <tr>
                        <td class="auto-style1">
                            书名：<asp:TextBox ID="RBookName" runat="server" Height="12px" Width="55px"></asp:TextBox>
                            作者：<asp:TextBox ID="RBookWriter" runat="server" Height="12px" Width="55px"></asp:TextBox>
                            数量：<asp:TextBox ID="RBookNum" runat="server" Height="12px" Width="55px"></asp:TextBox>
                            <asp:Button ID="RAddBook" runat="server" Text="添加" OnClick="AddBook" />
                        </td>
                    </tr>
                </table>                
            </asp:View>
            <asp:View ID="Vorder" runat="server">
                 <table>
                    <tr>
                        <td>
                            <asp:DataGrid ID="OrderGrid" runat="server"
                                Width="728px"
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
                                OnPageIndexChanged="OrderGrid_PageIndexChanged" PageSize="20">
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

                                    <asp:TemplateColumn HeaderText="购书时间">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="OrderTime" runat="server" onFocus="WdatePicker({minDate:'%y-%M-{%d+1} 08:00:00',dateFmt:'yyyy-MM-dd HH:mm:ss',alwaysUseStartDate:true})" Height="12px" Width="120px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="85px" />
                                        <ItemTemplate>
                                            <asp:Label ID="_OrderTime" runat="server" Text='<%# Eval("购书时间") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateColumn>
                 
                                     <asp:TemplateColumn HeaderText="购书人">
                                        <HeaderStyle Width="70px" />
                                        <ItemTemplate>
                                            <asp:Label ID="OCustomer" runat="server" Text='<%# Eval("购书人") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateColumn>
                                    
                                    <asp:TemplateColumn HeaderText="图书编号">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="OBookNum" runat="server" Width="60px" HeaderText="图书编号">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="60px" />
                                        <ItemTemplate>
                                            <asp:Label ID="_OBookNum" runat="server" Text='<%# Eval("图书编号") %>'></asp:Label>
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
                    </tr>
                    <tr>
                        <td>                            
                            <asp:TextBox ID="SOrderInfo" runat="server" Height="14px" Width="116px">单号/图书编号/购书人</asp:TextBox>
                            <asp:TextBox ID="SOrderTime" runat="server" Height="14px" Width="68px" onFocus="WdatePicker({errDealMode:2})">购书日期</asp:TextBox>
                            <asp:Button ID="SSearchOrder" runat="server" Text="查找" OnClick="SearchOrder" />
                            <asp:Button ID="Sreset" runat="server" Text="重置" OnClick="Reset" />
                        </td>
                    </tr>
                </table>
                
            </asp:View>
            <asp:View ID="Vaccount" runat="server">
                 <table>
                    <tr>
                        <td class="auto-style1">
                            <asp:DataGrid ID="AccountGrid" runat="server"
                                Width="728px"
                                CellPadding="4"
                                Font-Name="Verdana" Font-Size="8pt"
                                HeaderStylr-BackColor="#aaaadd"
                                OnEditCommand="EditAccount"
                                OnCancelCommand="Cancel"
                                OnUpdateCommand="UpdateAccount"
                                OnDeleteCommand="DeleteAccount"
                                DataKeyField="用户名"
                                AutoGenerateColumns="False"
                                MaintainState="false"
                                Font-Names="Verdana"
                                ForeColor="#333333"
                                GridLines="None"
                                AllowPaging="True"
                                AllowSorting="True"
                                OnPageIndexChanged="AccountGrid_PageIndexChanged" PageSize="20">
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
                                  
                                    <asp:TemplateColumn HeaderText="用户名">
                                        <HeaderStyle Width="60px" />
                                        <ItemTemplate>
                                            <asp:Label ID="Account" runat="server" Text='<%# Eval("用户名") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateColumn>

                                    <asp:BoundColumn HeaderText="密码" DataField="密码">  
                                        <HeaderStyle Width="60px" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
                                     </asp:BoundColumn>

                                    <asp:BoundColumn HeaderText="联系电话" DataField="联系电话">                                      
                                        <HeaderStyle Width="60px" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" />
                                     </asp:BoundColumn>

                                     <asp:TemplateColumn HeaderText="账户类型">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="AccountType" runat="server" Width="80px" HeaderText="账户类型">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="80px" />
                                        <ItemTemplate>
                                            <asp:Label ID="_AccountType" runat="server" Text='<%# Eval("账户类型") %>'></asp:Label>
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
                    </tr>
                    <tr>
                        <td class="auto-style1">                            
                            <asp:TextBox ID="SAInfo" runat="server" Height="14px" Width="70px">用户名</asp:TextBox>
                            <asp:DropDownList ID="AccountType" runat="server" Width="79px">
                                <asp:ListItem>全部类型</asp:ListItem>
                                <asp:ListItem>管理员</asp:ListItem>
                                <asp:ListItem>用户</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="Button1" runat="server" Text="查找" OnClick="SearchAccount" />
                            <asp:Button ID="Button2" runat="server" Text="重置" OnClick="Reset" />
                        </td>
                    </tr>

                    <tr>
                        <td class="auto-style1">
                            用户名：<asp:TextBox ID="Ruser" runat="server" Height="12px" Width="55px"></asp:TextBox>
                            密码：<asp:TextBox ID="Rpsd" runat="server" Height="12px" Width="55px"></asp:TextBox>
                            联系方式：<asp:TextBox ID="Rphone" runat="server" Height="12px" Width="55px"></asp:TextBox>
                            账户类型：<asp:DropDownList  ID="Rtype" runat="server" Width="65px">
                                      <asp:ListItem>管理员</asp:ListItem><asp:ListItem>用户</asp:ListItem>
                                      </asp:DropDownList>
                            <asp:Button ID="Register" runat="server" Text="注册" OnClick="RegisterAccount" />
                        </td>
                    </tr>
                </table>   
            </asp:View>
        </asp:MultiView>
    </div>
    </form>
</body>
</html>
