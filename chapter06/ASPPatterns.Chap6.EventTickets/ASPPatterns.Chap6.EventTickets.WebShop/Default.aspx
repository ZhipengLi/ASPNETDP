<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASPPatterns.Chap6.EventTickets.WebShop._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <h2>Basket</h2>
        I want
        <asp:TextBox ID="txtNoOfTickets" runat="server" Width="43px" />
        tickets to see
        <asp:DropDownList ID="ddlEvents" runat="server">
            <asp:ListItem Value="2de874d0-00b7-4c86-9925-c7f2c243151c">Portsmouth vs Southampton</asp:ListItem>
        </asp:DropDownList>
        <p>
            <asp:Button
                ID="btnReserveTickets" runat="server"
                Text="Reserve & Checkout" OnClick="btnReserveTickets_Click" />
            <br />
            <small>"Reserve &amp; Checkout" Reserves the Tickets for you as part of the Reservation Pattern.</small>
        </p>
</asp:Content>
