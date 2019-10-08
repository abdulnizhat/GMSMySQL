<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GaugePart.aspx.cs" Inherits="GaugePart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

     <script type="text/javascript" src="js/searchabledropdown/jquery-1.9.1-jquery.min.js"></script>
     <link href="js/searchabledropdown/3.3.7-css-bootstrap.min.css" rel="stylesheet" />
     <script type="text/javascript" src="js/searchabledropdown/1.12.2-js-select.min.js"></script>
     <link href="js/searchabledropdown/1.12.2-css-select.min.css" rel="stylesheet" />
   
  
    <style>
        .dropdownCustom{
            width:280px !important;
        }
    </style>

     <script type="text/javascript">
         function Confirm() {
             var chk = '<%= Session["dleteGagePartLink"].ToString() %>';
             if (chk == "YES") {
                 var confirm_value = document.createElement("INPUT");
                 confirm_value.type = "hidden";
                 confirm_value.name = "confirm_value";
                 if (confirm("Do you want to delete Gauge Part link?")) {
                     confirm_value.value = "Yes";
                 } else {
                     confirm_value.value = "No";
                 }
                 document.forms[0].appendChild(confirm_value);
             }
         }
        </script>
    <h3><%: Title %>Gauge Part</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                             <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <Triggers>

                                        <asp:PostBackTrigger ControlID="btnAddGaugePart" />
                                    </Triggers>
                                    <ContentTemplate>
                            <div class="col-md-12">
                                <asp:Button runat="server" ID="btnAddGaugePart" Text="Add New" OnClick="btnAddGaugePart_Click" CssClass="btn btn-primary" />
                            </div>
                             <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>
                                                     <asp:ListItem>Part No.-Wise</asp:ListItem> 
                                                     <asp:ListItem>Part Name-Wise</asp:ListItem>                                                                                                     
                                                </asp:DropDownList>
                                                
                                            </div>

                                        </div>
                                        <div class="col-md-5" runat="server" id="divSearchBy">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="lblName" Visible="false"></asp:Label>
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="searchBy" Visible="false"></asp:Label>
                                            <div class="col-md-6">
                                           
                                                <div runat="server" id="divtxtsearch"  Visible="false">
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
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Width="100%">
                                    <asp:GridView ID="grdGaugePart" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdGaugePart_PageIndexChanging">
                                       <HeaderStyle CssClass="header" />
                                          <Columns>
                                           
                                            <asp:BoundField DataField="gauge_part_link_id" HeaderText="G.P Link Id" />
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                             <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                              <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                              <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                                            <asp:BoundField DataField="gauge_type" HeaderText="Gauge Type" />
                                            <asp:BoundField DataField="part_id" HeaderText="Part Id" />
                                            <asp:BoundField DataField="part_number" HeaderText="Part No" />
                                            <asp:BoundField DataField="part_name" HeaderText="Part Name" />
                                            <asp:BoundField DataField="part_linked_Status" HeaderText="Status" />
                                            <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                            <%-- <asp:BoundField DataField="employee_name" HeaderText="Created By" />--%>
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditGaugePart" runat="server" OnClick="btnEditGaugePart_Click"
                                                        CommandArgument='<%# Eval("gauge_part_link_id")%>' class="btn btn-sm btn-info"
                                                        title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick = "Confirm()"
                                                        CommandArgument='<%# Eval("gauge_part_link_id")%>' class="btn btn-sm btn-info"
                                                        title="Delete record"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
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
                <div class="col-md-12">
                   <br />
                     <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="FailureText" />
                                </p>
                            </asp:PlaceHolder>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Part Link Id</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtLinkID" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <Triggers>

                                        <asp:PostBackTrigger ControlID="ddlGaugeID" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Name<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:DropDownList ID="ddlGaugeID" runat="server" TabIndex="1" OnSelectedIndexChanged="ddlGaugeID_SelectedIndexChanged" AutoPostBack="true" class="selectpicker dropdownCustom" data-live-search-style=""
                                                data-live-search="true">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlGaugeID" Display="Dynamic" ForeColor="Maroon"
                                                ErrorMessage="Select Gauge" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                            

                             <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-4 control-label">Part Number<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-4">
                                <asp:ListBox ID="ddlPartNumber" runat="server" SelectionMode="Multiple" TabIndex="2" class="selectpicker dropdownCustom" data-live-search-style=""
                                                data-live-search="true"
                                    OnSelectedIndexChanged="ddlPartNumber_SelectedIndexChanged" AutoPostBack="false"></asp:ListBox>
                                <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlPartNumber" Display="Dynamic" ForeColor="Maroon"
                                    ErrorMessage="Select Part Number" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>--%>
                            </div>
                                 </div>
                          

                             <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Status</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtStatus" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="col-md-6" style="margin-left: -20px">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal1" />
                                </p>
                            </asp:PlaceHolder>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Manufacture Id</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtmanufactureID" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Type</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtType" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>
                              <div class="col-md-1">
                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="lnkallAddPartNumber" />
                                    <asp:PostBackTrigger ControlID="lnkRemovePart" />
                                </Triggers>
                                <ContentTemplate>
                                <asp:LinkButton ID="lnkallAddPartNumber" runat="server" OnClick="lnkallAddPartNumber_Click" Text=">" ForeColor="Black" Font-Bold="true" Font-Size="XX-Large" ToolTip="Add Part Number"></asp:LinkButton>
                                <br />
                                <asp:LinkButton ID="lnkRemovePart" runat="server" OnClick="lnkRemovePart_Click" Text="<" ForeColor="Black" Font-Bold="true" Font-Size="XX-Large" ToolTip="Remove Selected Part Number"></asp:LinkButton>
                                    </ContentTemplate>
                            </asp:UpdatePanel>
                            </div>
                             <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Selected  Part Number<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-7">
                                    <asp:ListBox ID="listSelectedPart" runat="server" SelectionMode="Multiple" TabIndex="2"
                                        CssClass="form-control"></asp:ListBox>
                                     <asp:RequiredFieldValidator ValidationGroup="c" SetFocusOnError="true" runat="server" ControlToValidate="listSelectedPart"
                                            CssClass="text-danger" ErrorMessage="Select Part Number" />
                                   <%-- <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="listSelectedPart" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Part Number" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>--%>
                                </div>

                                 <asp:HiddenField ID="HiddenFieldPartId" runat="server" />

                            </div>
                          




                            <asp:Label ID="lblgaugePartLinkId" runat="server" Visible="false"></asp:Label>
                            <div class="form-group" runat="server" visible="false">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Part Id</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtPartId" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group" runat="server" visible="false">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Part Name</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtPartName" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group" runat="server" visible="false">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Operation</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtOperation" ReadOnly="true" CssClass="form-control" />
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="col-md-offset-5 col-md-6">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSaveGaugePart" TabIndex="3" ValidationGroup="c" OnClick="btnSaveGaugePart_Click" Text="Save" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnClloseGaugePart" TabIndex="4" CausesValidation="false" Text="Close" OnClick="btnClloseGaugePart_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>

</asp:Content>

