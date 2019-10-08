<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="MSAHistoryReport.aspx.cs" Inherits="MSAHistoryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
     <h3><%: Title %>MSA History</h3>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-6" id="divcust" runat="server" visible="false">
                <div class="form-horizontal">

                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlcust" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlcust_SelectedIndexChanged" CssClass="form-control" TabIndex="1"></asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlcust"
                             Display="Dynamic"  ValidationGroup="a" CssClass="text-danger" ErrorMessage="Customer Name field is required." />
                        </div>
                    </div>

                </div>
            </div>
            
        </div>
         
         </div>
    
       <div class="row">
        
    <div class="col-md-12" id="divgauge" runat="server">
                <div class="form-horizontal">
  <div class="form-group">
                   <div class="col-md-6">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>All</asp:ListItem>
                                                     <asp:ListItem>Gauge Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>                                                  
                                                    <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Type-Wise</asp:ListItem>                                                    
                                                   
                                                    </asp:DropDownList>
                                              
                                            </div>

                                        </div>
                                        <div class="col-md-6" runat="server" id="divSearchBy">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="lblName" Visible="false"></asp:Label>
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="searchBy" Visible="false"></asp:Label>
                                            <div class="col-md-6">
                                                
                                                <div runat="server" id="divtxtsearch" visible="false">
                                                    <asp:TextBox ID="txtsearchValue" runat="server" ValidationGroup="a" TabIndex="3" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtsearchValue"
                                                        CssClass="text-danger" ErrorMessage="Required search value." />
                                                </div>
                                            </div>
                                        </div>
      </div>
                </div>
            </div>
             </div>
         
       
        <div class="row">
         <div class="col-md-12">
         <div class="col-md-offset-2 col-md-1"">
               <asp:Button runat="server" ID="btnShowMSAHistory" ValidationGroup="a" 
                   OnClick="btnShowMSAHistory_Click" Text="Show" CssClass="btn btn-primary" TabIndex="3"/>
             <br />
             </div>
             <div class="col-md-2">
                    <asp:UpdatePanel ID="updexport" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExportToExcel" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Button runat="server" ID="btnExportToExcel" Text="Export To Excel" OnClick="btnExportToExcel_Click" CssClass="btn btn-primary" TabIndex="4" ValidationGroup="a" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </div>
        </div>
        </div>
       <div class="row">
          <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                           <asp:GridView ID="grdMSATranReport" runat="server" Width="100%" AutoGenerateColumns="false"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" 
                                        CssClass="table table-bordered table-responsive" PageSize="10"
                                         AllowPaging="true" OnPageIndexChanging="grdMSATranReport_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="msa_transaction_id" HeaderText="MSA Tran. Id" />
                                            <asp:BoundField DataField="msa_date" HeaderText="MSA Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="calibration_schedule_id" HeaderText="Scheduled Id" />
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                            <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                            <asp:BoundField DataField="msa_report_no" HeaderText="MSA Report No." />
                                            <asp:BoundField DataField="msa_hours" HeaderText="MSA Hrs" />
                                            <asp:BoundField DataField="msa_status" HeaderText="MSA Status" />
                                             <asp:BoundField DataField="is_approved" HeaderText="Approved Status" />
                                            <asp:BoundField DataField="bias" HeaderText="Bias" />
                                            <asp:BoundField DataField="linearity" HeaderText="Linearity" />
                                            <asp:BoundField DataField="stability" HeaderText="Stability" />
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updaction" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnPrintMSATran" />
                                                            <asp:PostBackTrigger ControlID="LnkDownLoadDocument" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                    <asp:LinkButton ID="btnPrintMSATran" runat="server" OnClick="btnPrintMSATran_Click"
                                                        CommandArgument='<%# Eval("gauge_id")%>' class="btn btn-sm btn-info"
                                                        title="Print"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
                                                  
                                                            </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bias" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updactionBias" runat="server">
                                                        <Triggers>
                                                          <asp:PostBackTrigger ControlID="LnkDownLoadDocument" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="LnkDownLoadDocument" runat="server" OnClick="LnkDownLoadDocument_Click"
                                                                CommandArgument='<%# Eval("msa_transaction_id")%>' class="btn btn-sm btn-info"
                                                                title="Download Bias"><i class="glyphicon glyphicon-download"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Linearity" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updactionLinearits" runat="server">
                                                        <Triggers>
                                                          <asp:PostBackTrigger ControlID="LnkDownLoadLinearity" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="LnkDownLoadLinearity" runat="server" OnClick="LnkDownLoadLinearity_Click"
                                                                CommandArgument='<%# Eval("msa_transaction_id")%>' class="btn btn-sm btn-info"
                                                                title="Download Linearity"><i class="glyphicon glyphicon-download"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Stability" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updactionStability" runat="server">
                                                        <Triggers>
                                                          <asp:PostBackTrigger ControlID="LnkDownLoadStability" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="LnkDownLoadStability" runat="server" OnClick="LnkDownLoadStability_Click"
                                                                CommandArgument='<%# Eval("msa_transaction_id")%>' class="btn btn-sm btn-info"
                                                                title="Download Stability"><i class="glyphicon glyphicon-download"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="R&R" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updactionRR" runat="server">
                                                        <Triggers>
                                                          <asp:PostBackTrigger ControlID="LnkDownLoadRR" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="LnkDownLoadRR" runat="server" OnClick="LnkDownLoadRR_Click"
                                                                CommandArgument='<%# Eval("msa_transaction_id")%>' class="btn btn-sm btn-info"
                                                                title="Download R&R"><i class="glyphicon glyphicon-download"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
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
</asp:Content>

