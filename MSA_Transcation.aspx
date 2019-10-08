<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MSA_Transcation.aspx.cs" Inherits="MSA_Transcation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

   <script type="text/javascript" src="js/searchabledropdown/jquery-1.9.1-jquery.min.js"></script>
     <link href="js/searchabledropdown/3.3.7-css-bootstrap.min.css" rel="stylesheet" />
     <script type="text/javascript" src="js/searchabledropdown/1.12.2-js-select.min.js"></script>
     <link href="js/searchabledropdown/1.12.2-css-select.min.css" rel="stylesheet" />
   
    <%--For Calendar--%>
    <link rel="stylesheet" href="css/jquery-ui.css" type="text/css" media="all" />
    <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <style>
        .dropdownCustom{
            width:225px !important;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#<%= txtMSADate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        function onlyNos(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            catch (err) {
                alert(err.Description);
            }
        }
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete customer record?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <h3><%: Title %>MSA Transaction</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <Triggers>

                                        <asp:PostBackTrigger ControlID="btnAddMSATransaction" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="col-md-1">
                                        <asp:Button runat="server" ID="btnAddMSATransaction" Text="Add New" OnClick="btnAddMSATransaction_Click" CssClass="btn btn-primary" />
                                    </div>
                                        <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                     <asp:ListItem>Gauge Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>                                                  
                                                    <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Type-Wise</asp:ListItem>                                                    
                                                   
                                                    </asp:DropDownList>
                                              
                                            </div>

                                        </div>
                                        <div class="col-md-5" runat="server" id="divSearchBy">
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
                                        <div class="col-md-2">
                                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" ValidationGroup="a" Visible="false" CssClass="btn btn-primary" />
                                        </div>
                                             </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:Label runat="server" Font-Bold="true" ID="Label2" Text="MSA Due List"></asp:Label>
                                    </div>
                                </div>
                                <asp:Panel ID="Panel3" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:Panel ID="Panel4" runat="server" Width="100%" ScrollBars="Auto" Style="max-height: 350px">
                                        <asp:GridView ID="grdMsaDue" runat="server" Width="100%"
                                            AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdMsaDue_PageIndexChanging"
                                            CssClass="table table-bordered table-responsive" PageSize="10">
                                            <HeaderStyle CssClass="header" />
                                            <Columns>
                                                <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                                <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                                <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                                 <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                            <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                                                <asp:BoundField DataField="next_due_date" HeaderText="MSA Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="updmsa" runat="server">
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="btnReTransactionMSA" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnReTransactionMSA" runat="server"
                                                                    CommandArgument='<%# Eval("gauge_id")%>' class="btn btn-sm btn-info" OnCommand="btnReTransactionMSA_Command"
                                                                    Text="Transaction" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:Label runat="server" Font-Bold="true" ID="Label3" Text="MSA Transactions List"></asp:Label>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="grdMSATran" runat="server" Width="100%" AutoGenerateColumns="false"
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false"
                                        CssClass="table table-bordered table-responsive" PageSize="10"
                                        AllowPaging="true" OnPageIndexChanging="grdMSATran_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="msa_transaction_id" HeaderText="MSA Tran. Id" />
                                            <asp:BoundField DataField="msa_date" HeaderText="MSA Date" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="calibration_schedule_id" HeaderText="Scheduled Id" />
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                            <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                             <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                             <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                                            <asp:BoundField DataField="msa_report_no" HeaderText="MSA Report No." />
                                            <asp:BoundField DataField="msa_hours" HeaderText="MSA Hrs" />
                                            <asp:BoundField DataField="msa_status" HeaderText="MSA Status" />

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


                                           <%-- <asp:BoundField DataField="bias" HeaderText="Bias" />
                                            <asp:BoundField DataField="linearity" HeaderText="Linearity" />
                                            <asp:BoundField DataField="stability" HeaderText="Stability" />--%>
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updaction" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnEditMSATran" />
                                                             <asp:PostBackTrigger ControlID="LnkApproved" />
                                                            </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnEditMSATran" runat="server" OnClick="btnEditMSATran_Click"
                                                                CommandArgument='<%# Eval("msa_transaction_id")%>' class="btn btn-sm btn-info"
                                                                title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                            &nbsp;
                                                             <asp:Button ID="LnkApproved" runat="server"
                                                                CommandArgument='<%# Eval("msa_transaction_id")%>' class="btn btn-sm btn-info" OnCommand="LnkApproved_Command"
                                                                Text="Approve" />
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
        </asp:View>
        <asp:View ID="View2" runat="server">
            <div class="row">
                <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
                    <div class="col-md-12">
                        <br />
                        <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                            <div class="col-md-4">
                                <div class="form-horizontal">
                                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                        <p class="text-danger">
                                            <asp:Literal runat="server" ID="FailureText" />
                                        </p>
                                    </asp:PlaceHolder>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">MSA Transaction Id</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtMSATranID" ReadOnly="true" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtMSATranID"
                                                CssClass="text-danger" ErrorMessage="MSA Transaction ID field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="ddlGauge" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlGauge" TabIndex="1" runat="server" OnSelectedIndexChanged="ddlGauge_SelectedIndexChanged" AutoPostBack="true" class="selectpicker dropdownCustom" data-live-search-style=""
                                    data-live-search="true">
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlGauge" Display="Dynamic" ForeColor="Maroon"
                                                        ErrorMessage="Select Gauge." SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">MSA Date<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtMSADate" TabIndex="2" CssClass="form-control" placeHolder="DD/MM/YYYY" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" SetFocusOnError="true" Display="Dynamic" runat="server"
                                                ControlToValidate="txtMSADate"
                                                CssClass="text-danger" ErrorMessage="MSA Date field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Status<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="3" CssClass="form-control">
                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">PASSED</asp:ListItem>
                                                <asp:ListItem Value="2">FAILED</asp:ListItem>
                                                <asp:ListItem Value="3">REPAIR</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlStatus" Display="Dynamic" ForeColor="Maroon"
                                                ErrorMessage="Select Status." SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="0"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">MSA Hours<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtMSAHours" TabIndex="5" onkeypress="return onlyNos(event,this);" MaxLength="10" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtMSAHours"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="MSA Hours field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Temprature<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtTemprature" TabIndex="10" CssClass="form-control" MaxLength="10" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtTemprature"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Temprature field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Humidity<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtHumidity" TabIndex="11" MaxLength="10" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtHumidity"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Humidity field is required." />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Pressure<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtPressure" TabIndex="12" MaxLength="10" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtPressure"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Pressure field is required." />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Other</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtOther" TabIndex="13" CssClass="form-control" MaxLength="149" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">MSA Report No.<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtMSAReportNo" TabIndex="14" CssClass="form-control" MaxLength="50" />
                                            <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtMSAReportNo"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="MSA Report No field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Bias</asp:Label>
                                        <div class="col-md-8">
                                            <asp:FileUpload ID="FileUploadBias" runat="server" TabIndex="15" CssClass="form-control" />
                                            <asp:Label ID="lblFileBias" runat="server"></asp:Label>
                                            <br />
                                            <asp:Label ID="Label5" Text="pdf and excel file only" ForeColor="#cc3300" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Linearity</asp:Label>
                                        <div class="col-md-8">
                                            <asp:FileUpload ID="FileUploadLinearity" runat="server" TabIndex="15" CssClass="form-control" />
                                            <asp:Label ID="lblFileLinearity" runat="server"></asp:Label>
                                            <br />
                                            <asp:Label ID="Label7" Text="pdf and excel file only" ForeColor="#cc3300" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Stability</asp:Label>
                                        <div class="col-md-8">
                                            <asp:FileUpload ID="FileUploadStability" runat="server" TabIndex="15" CssClass="form-control" />
                                            <asp:Label ID="lblFileStability" runat="server"></asp:Label>
                                            <br />
                                            <asp:Label ID="Label9" Text="pdf and excel file only" ForeColor="#cc3300" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">R&R</asp:Label>
                                        <div class="col-md-8">
                                            <asp:FileUpload ID="FileUploadRR" runat="server" TabIndex="15" CssClass="form-control" />
                                            <asp:Label ID="lblFileRR" runat="server"></asp:Label>
                                            <br />
                                            <asp:Label ID="Label1" Text="pdf and excel file only" ForeColor="#cc3300" runat="server"></asp:Label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-4" runat="server" id="divDisplayData1" visible="false">
                                <div class="form-horizontal">
                                    <asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
                                        <p class="text-danger">
                                            <asp:Literal runat="server" ID="Literal1" />
                                        </p>
                                    </asp:PlaceHolder>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Schedule Id</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtschduleId" ReadOnly="true" CssClass="form-control" />
                                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtGaugeId"
                                        CssClass="text-danger" ErrorMessage="Gauge ID field is required." />--%>
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group" runat="server">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Id</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtGaugeId" ReadOnly="true" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtGaugeId"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Gauge ID field is required." />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Type</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtType" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div runat="server" id="divGoTolPulus">
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Go Tollerance (+)</asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" ID="txtGoTollerancePlus" ReadOnly="true" CssClass="form-control" />
                                                <br />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">No Go Tollerance (+)</asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" ID="txtNoGoTollerancePlus" ReadOnly="true" CssClass="form-control" />
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divLeastCount">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Least Count</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtLeastCount" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Store Location</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtStoreLocation" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Purchase Cost</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtPurchaseCost" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Service Date</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtServiceDate" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divResolution">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Resolution</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtResolution" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Calibratted By</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtCalibrattedBy" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Calibration Frequency</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtCalibrationFrequency" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Last Calibration Date</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtLastCalibrationDate" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <%-- <div class="form-group" >
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Projected Calib. Schedule</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtProjectedCalibrationSchedule" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>--%>

                                    <div class="form-group" id="divPermissableError1" runat="server">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Permissible Error1</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtPermisableError1" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4" runat="server" id="divDisplayData2" visible="false">
                                <div class="form-horizontal">
                                    <asp:PlaceHolder runat="server" ID="PlaceHolder2" Visible="false">
                                        <p class="text-danger">
                                            <asp:Literal runat="server" ID="Literal2" />
                                        </p>
                                    </asp:PlaceHolder>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Name</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtGaugeName" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <%--<div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Operation</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtOperation" ReadOnly="true" CssClass="form-control" />
                                    <br />
                                   </div>
                            </div>--%>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Manufacture Id</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtManufactureID" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divSize">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Size</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtSize" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <asp:Label ID="lblMSATranId" runat="server" Visible="false"></asp:Label>
                                    <div class="form-group" runat="server" id="divRange" visible="false">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Range</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtRange" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div runat="server" id="divNoGoPlusminus">
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Go Tollerance(-)</asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" ID="txtGoTolleranceMinus" ReadOnly="true" CssClass="form-control" />
                                                <br />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">No Go Tollerance(-)</asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" ID="txtNoGoTolleranceMinus" ReadOnly="true" CssClass="form-control" />
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGoWearLimit">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Go Were Limit</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtGoWereLimit" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Current Location</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtCurrentLocation" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Purchase Date</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtPurchaseDate" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Retairment Date</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtRetairementDate" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Initial Time Used</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtInitialTimeUsed" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Last Calibratted By</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtLastCalibratedBy" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Frequency Type</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtFrequencyType" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Calibration Hours</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtCalibrationHours" ReadOnly="true" MaxLength="10" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Next Due Date</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtNextDueDate" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                    <div class="form-group" id="divPermissableError2" runat="server">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Permissible Error2</asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtPermisableError2" ReadOnly="true" CssClass="form-control" />
                                            <br />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-offset-5 col-md-6" style="margin-bottom: 10px">
                                <div class="col-md-2">
                                    <asp:UpdatePanel ID="updd" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSaveMSATransaction" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnSaveMSATransaction" ValidationGroup="c" TabIndex="17" OnClick="btnSaveMSATransaction_Click" Text="Save" CssClass="btn btn-primary" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                                <div class="col-md-3">
                                    <asp:UpdatePanel ID="btnMsaUpd" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnCloseMSATransaction" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:Button runat="server" ID="btnCloseMSATransaction" CausesValidation="false" TabIndex="18" Text="Close" OnClick="btnCloseMSATransaction_Click" CssClass="btn btn-primary" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
        </asp:View>
    </asp:MultiView>
      <div class="col-md-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <table id="tblPaymentDetails" runat="server" visible="false">
                        <tr>
                            <td align="center">
                                <asp:Panel ID="Panel5" CssClass="panel_popup" runat="server" BackColor="#CCCCCC"
                                    ForeColor="#333" BorderColor="Brown" Height="220px" Width="450px" ScrollBars="Auto">
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <table border="0" cellpadding="2" cellspacing="2" width="65%">
                                        <tr>
                                            <td style="color: #CC3300; font-size: 20px; font-weight: bold" colspan="4" align="center">MSA Transaction Approved
                                            </td>
                                            <td align="right">
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Close.jpg" Width="31px" ToolTip="Close" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="color: #C0C0C0"></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTranId" runat="server" Visible="false"></asp:Label>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Remark 
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtremark" runat="server" MaxLength="200" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </td>

                                        </tr>
                                    </table>
                                    <br />
                                    <table style="align-items:center">

                                        <tr>
                                            <td>
                                                <asp:Button ID="btnSaveRemak" OnClick="btnSaveRemak_Click" runat="server" Text="Save" CssClass="btn btn-primary" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblmsg" runat="server" Text="Remark is required." Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <asp:Label ID="Label6" runat="server"></asp:Label>
                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panel5"
                                    TargetControlID="Label6" CancelControlID="ImageButton2">
                                </asp:ModalPopupExtender>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
</asp:Content>

