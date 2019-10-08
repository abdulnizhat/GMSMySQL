<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  EnableEventValidation="false" CodeFile="ImportExcel.aspx.cs" Inherits="ImportExcel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %>Import Excel Files (One time use for every import to.)</h3>


    <div class="row">
        <div class="col-md-12">
            <div class="col-md-5" id="divcust" runat="server" visible="false">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlcust" runat="server" CssClass="form-control" TabIndex="1"></asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlcust"
                                Display="Dynamic" ErrorMessage="Select Customer" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                ValueToCompare="--Select--"></asp:CompareValidator>
                            <asp:RequiredFieldValidator runat="server" ValidationGroup="a" ControlToValidate="ddlcust" SetFocusOnError="True"
                                Display="Dynamic" CssClass="text-danger" ErrorMessage="Customer Name field is required."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-5" runat="server">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Import To<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlSelectForm" runat="server" CssClass="form-control" TabIndex="2">
                                <asp:ListItem>--Select--</asp:ListItem>
                                <asp:ListItem>Customer</asp:ListItem>
                                <asp:ListItem>Employee</asp:ListItem>
                                <asp:ListItem>Supplier</asp:ListItem>
                                <asp:ListItem>Part Master</asp:ListItem>
                                <asp:ListItem>Gauge Master</asp:ListItem>
                                <asp:ListItem>Gauge Supplier</asp:ListItem>
                                <asp:ListItem>Gauge Part</asp:ListItem>                                
                                <asp:ListItem>Issue Status</asp:ListItem>
                                <asp:ListItem>Return Issue</asp:ListItem>
                                <asp:ListItem>Calibration Schedule</asp:ListItem>
                                <asp:ListItem>Calibration Transaction</asp:ListItem>
                               <%-- <asp:ListItem>MSA Transaction</asp:ListItem>--%>
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlSelectForm"
                                Display="Dynamic" ErrorMessage="Select search option" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                ValueToCompare="--Select--"></asp:CompareValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="col-md-5" runat="server">
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-4 control-label">Upload Excel File<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-8">
                            <asp:FileUpload ID="ExcelFileUpload" runat="server" TabIndex="15" CssClass="form-control" />
                            <asp:Label ID="Label1" Text="Excel file only." ForeColor="#cc3300" runat="server"></asp:Label>
                        </div>

                    </div>
                </div>
            </div>
            <div class="col-md-5" runat="server">
                <div class="form-group">
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="updd" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnImportExcelFile" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Button runat="server" ID="btnImportExcelFile" Text="Import Excel File"  ValidationGroup="a" OnClick="btnImportExcelFile_Click" CssClass="btn btn-primary" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

