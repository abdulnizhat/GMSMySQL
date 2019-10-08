<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BranchMaster.aspx.cs" Inherits="BranchMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

     <h3><%: Title %>Branch Master</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
        <div class="col-md-12">
         <div class="form-horizontal">
             <div class="form-group">
                        <div class="col-md-10">
                            <asp:Button runat="server" ID="btnAddnewBranch" Text="Add New" OnClick="btnAddnewBranch_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                  <div class="form-group">
                       <div class="col-md-10">
                            <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                       <asp:GridView ID="grdBranch" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdBranch_PageIndexChanging">
                            <HeaderStyle CssClass="header" />    
                           <Columns>
                                   <asp:BoundField DataField="branch_id" HeaderText="Branch Id" />
                                        <asp:BoundField DataField="branch_name" HeaderText="Branch Name" />
                                        <asp:BoundField DataField="branch_code" HeaderText="Branch Code" />
                                       <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                              <asp:LinkButton id="btnEdit" runat="server" OnClick="btnEdit_Click" CommandArgument='<%# Eval("branch_id")%>' class="btn btn-sm btn-info" title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                               </Columns>
                           <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                           </asp:GridView>
                                </asp:Panel>
                        </div>
                    </div>
                    
                    
                    
                </div>
         </div>

        
    </div>
        </asp:View>

        <asp:View ID="View2" runat="server">
         <div class="row">
        <div class="col-md-offset-2 col-md-12">
         <div class="form-horizontal">
                   <%-- <h4>Branch Master</h4>--%>
                    <br />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
              <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Branch Id</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtBranchId" ReadOnly="true" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBranchId" ValidationGroup="v"
                                CssClass="text-danger" ErrorMessage="Branch ID field is required." />
                        </div>
                    </div>
             <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Branch Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtBranch" TabIndex="1" CssClass="form-control" MaxLength="45" />
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" ControlToValidate="txtBranch" ValidationGroup="v"
                                CssClass="text-danger" ErrorMessage="Branch Name field is required." />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server"  CssClass="col-md-2 control-label">Branch Code<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtBranchCode" TabIndex="2" CssClass="form-control" MaxLength="18" />
                            <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" ValidationGroup="v" ControlToValidate="txtBranchCode" CssClass="text-danger" ErrorMessage="Branch Code field is required." />
                        </div>
                    </div>
                    
                    <div class="col-md-offset-2 col-md-6">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSaveBranch" TabIndex="3" OnClick="btnSaveBranch_Click" Text="Save" ValidationGroup="v" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnClose" TabIndex="4" OnClick="btnClose_Click" Text="Close" CssClass="btn btn-primary" CausesValidation="False" />
                        </div>
                    </div>
                </div>
         </div>

        
    </div>

    </asp:View>
    </asp:MultiView>
</asp:Content>

