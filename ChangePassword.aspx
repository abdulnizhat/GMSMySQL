<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <h3><%: Title %>Country Master</h3>
       <div class="col-md-12">
         <div class="form-horizontal">
                  
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Current Password<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                             <asp:TextBox ID="txtcurrentpassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rq1" runat="server" ForeColor="Red"
                                                                    ControlToValidate="txtcurrentpassword" Display="Dynamic"  CssClass="text-danger"
                                                                    ErrorMessage="Current Password field is required." SetFocusOnError="True"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server"  CssClass="col-md-2 control-label">New Password<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                           <asp:TextBox ID="txtnewpassword" runat="server" TextMode="Password" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                                               
                                                                <asp:RequiredFieldValidator ID="rq2" runat="server" ForeColor="Red"
                                                                    ControlToValidate="txtnewpassword" Display="Dynamic"  CssClass="text-danger"
                                                                    ErrorMessage="New Password field is required." SetFocusOnError="True" ValidationGroup="a"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label runat="server"  CssClass="col-md-2 control-label">Confirm Password<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                           <asp:TextBox ID="txtconfpassword" runat="server" TextMode="Password" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                                                
                                                                <asp:RequiredFieldValidator ID="rq3" runat="server" ForeColor="Red"
                                                                    ControlToValidate="txtconfpassword" Display="Dynamic"  CssClass="text-danger"
                                                                    ErrorMessage="Confirm Password field is required." SetFocusOnError="True"
                                                                    ValidationGroup="a"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-offset-2 col-md-6">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Update" CssClass="btn btn-primary" ValidationGroup="a"/>
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnClose" Text="Reset" OnClick="btnClose_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
         </div>

        
   
</asp:Content>

