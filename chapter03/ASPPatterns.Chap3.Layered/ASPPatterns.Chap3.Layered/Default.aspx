<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASPPatterns.Chap3.Layered.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="ddlCustomerType" runat="server" AutoPostBack="True">
                <asp:ListItem Value="0">Standard</asp:ListItem>
                <asp:ListItem Value="1">Trade</asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
        </div>
        <asp:Repeater ID="rptProducts" runat="server">
            <HeaderTemplate>
                <table>
                    <tr>
                        <td>Name</td>
                        <td>RRP</td>
                        <td>Selling Price</td>
                        <td>Discount</td>
                        <td>Savings</td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <hr />
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Name") %></td>
                    <td><%# Eval("RRP")%></td>
                    <td><%# Eval("SellingPrice") %></td>
                    <td><%# Eval("Discount") %></td>
                    <td><%# Eval("Savings") %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
