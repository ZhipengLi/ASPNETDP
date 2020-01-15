<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="ASPPatterns.Chap6.EventTickets.WebShop.Checkout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>


            <h2>Checkout</h2>
            In your basket you have:
            <p>
                <asp:Label ID="lblBasketContents" runat="server" Text="" />
            </p>
            <asp:Button ID="btnPlaceOrder" runat="server"
                Text="Place Order" OnClick="btnPlaceOrder_Click" />
            <br />
            <small>Click the “Place Order" button again and the Ticket Id willalways return the same due to the use of the Idempotent Pattern.</small>
            <p>
                <asp:Label ID="lblThankYou" runat="server" Text=""></asp:Label>
            <p/>
        </div>
    </form>
</body>
</html>
