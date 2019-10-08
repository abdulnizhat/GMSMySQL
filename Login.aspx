<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/uts_styles.css" rel="stylesheet" />
    
     <script>history.go(1)</script>
</head>
<body>
    <form id="frmlogin" runat="server">
        <div class="containerlogin">
            <div class="hdr_logo">
                <a href="http://www.aarushqualityservices.com/">Gauge Management System
                </a>&nbsp;<a href="http://www.aarushqualityservices.com/"></a>
                <br/>
                <a href="http://www.aarushqualityservices.com/">Home
                </a>&nbsp;<a href="http://www.aarushqualityservices.com/"></a>
            </div>

            <div class="mdl_cnt">
                <div class="logintext">
                    <img src="Images/photo1.jpg" width="400" height="270" />
                </div>
                <div class="logincntrl">
                    <div class="login_item">
                        Customer Name<br />

                        <asp:DropDownList ID="ddlCustomer" runat="server" 
                            CssClass="txtbx_style" Width="222px" TabIndex="1">
                        </asp:DropDownList>
                    </div>
                    <div class="login_item">
                        User ID<br />
                        <label>
                            <asp:TextBox ID="txtusername" MaxLength="20" TabIndex="2" runat="server" CssClass="txtbx_style"
                                 Width="217px"></asp:TextBox></label>
                    </div>
                    <div class="login_item">
                        Password<br />
                        <label>
                            <asp:TextBox ID="txtpassword" MaxLength="20" TabIndex="3" runat="server" CssClass="txtbx_style"
                                 TextMode="Password" Width="217px"></asp:TextBox></label>
                    </div>

                    <div style="float: right; margin-right: 30px; margin-top: 10px;">
                        <asp:ImageButton ID="btnlogin" runat="server" OnClick="btnlogin_Click" TabIndex="4" ImageUrl="~/Images/lg_btn.png" />
                    </div>

                    <div class="login_item">
                        <asp:Label ID="lblerror" runat="server" Text="Label" Height="27px" Width="128px" Visible="false"></asp:Label>
                    </div>

                </div>
            </div>

            <div class="mdl_cnt2">&nbsp;</div>
            <div class="login_btm">
                <%--<div style="float: left;"><a href="http://www.aarushqualityservices.com/" target="_blank">
                    <img src="Images/logo_dsahil_w62px.png" alt="Dsahil_Industries" border="none" align="left" /></a>
                </div>--%>
                <div class="uts_cprt">
                    System by : <a href="http://www.aarushqualityservices.com/" target="_blank">Aarush Quality Services</a> &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; 
		&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; 
		&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
			&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
				&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
					&nbsp;&nbsp;&nbsp;&nbsp;
		 © 2017-2018 All Rights Reserved.
                </div>
            </div>

        </div>

    </form>
</body>
</html>
