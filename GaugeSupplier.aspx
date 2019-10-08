<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GaugeSupplier.aspx.cs" Inherits="GaugeSupplier" %>

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
            var chk = '<%= Session["dleteGageSuppLink"].ToString() %>';
             if (chk == "YES") {
                 var confirm_value = document.createElement("INPUT");
                 confirm_value.type = "hidden";
                 confirm_value.name = "confirm_value";
                 if (confirm("Do you want to delete Gauge Supplier link?")) {
                     confirm_value.value = "Yes";
                 } else {
                     confirm_value.value = "No";
                 }
                 document.forms[0].appendChild(confirm_value);
             }
        }
 </script>
    
    <h3><%: Title %>Gauge Supplier</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            
                             <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <Triggers>

                                        <asp:PostBackTrigger ControlID="btnAddGaugeSupplier" />
                                    </Triggers>
                                    <ContentTemplate>
                            <div class="col-md-12">
                                <asp:Button runat="server" ID="btnAddGaugeSupplier" Text="Add New" OnClick="btnAddGaugeSupplier_Click" CssClass="btn btn-primary" />
                            </div>
                              <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>
                                                     <asp:ListItem>Supplier Name-Wise</asp:ListItem> 
                                                     <asp:ListItem>Gauge Type-Wise</asp:ListItem>                                                                                                     
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
                                 <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdGaugeSupplier" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdGaugeSupplier_PageIndexChanging">
                                    <HeaderStyle CssClass="header" />
                                     <Columns>
                                        
                                        <asp:BoundField DataField="gauge_supplier_link_id" HeaderText="Gauge Supplier Link Id" />
                                        <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                        <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                         <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                         <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                          <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                                        <asp:BoundField DataField="gauge_type" HeaderText="Gauge Type" />
                                        <asp:BoundField DataField="supplier_name" HeaderText="Supplier" />
                                        <asp:BoundField DataField="link_status" HeaderText="Status" />
                                        
                                        <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                        <asp:BoundField DataField="employee_name" HeaderText="Created By" />
                                        <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditPart" runat="server" CommandName="Upd" OnClick="btnEditPart_Click"
                                                    CommandArgument='<%# Eval("gauge_supplier_link_id")%>' class="btn btn-sm btn-info"
                                                    title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                &nbsp;
                                                <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click"
                                                    CommandArgument='<%# Eval("gauge_supplier_link_id")%>' class="btn btn-sm btn-info" OnClientClick = "Confirm()"
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
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Supplier Link Id</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtGaugeSupLinkID" ReadOnly="true" CssClass="form-control" />
                                   </div>
                            </div>
                            <div class="form-group">
                                  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <Triggers>

                                        <asp:PostBackTrigger ControlID="ddlGauge" />
                                    </Triggers>
                                    <ContentTemplate>
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8" >
                                  
                                    <asp:DropDownList ID="ddlGauge" runat="server" TabIndex="2"
                                       AutoPostBack="true"   OnSelectedIndexChanged="ddlGauge_SelectedIndexChanged"  class="selectpicker dropdownCustom" data-live-search-style=""
                                    data-live-search="true" 
                                         >
                                    </asp:DropDownList>
                                     <asp:CompareValidator ID="CompareValidator3" SetFocusOnError="true" runat="server" ControlToValidate="ddlGauge" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Gauge" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                             
                                         </div>
                                        </ContentTemplate>
                                      </asp:UpdatePanel>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Type</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtType" TabIndex="4" ReadOnly="true" CssClass="form-control" />
                             </div>
                            </div>                          

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Store Location</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtStoreLocation" TabIndex="6" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Purchase Cost</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtPurchaseCost" TabIndex="8" ReadOnly="true" CssClass="form-control" />
                              </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Service Date</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtServiceDate" TabIndex="10" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="divPemisablerror1">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Permisable Error1</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtPermisableError1" TabIndex="12" ReadOnly="true" TextMode="MultiLine" CssClass="form-control" />
                               </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Manufacture Id</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtManufactureID" TabIndex="1" ReadOnly="true" CssClass="form-control" />
                                 </div>
                            </div>
                           
                          
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal1" />
                                </p>
                            </asp:PlaceHolder>

                            
                            
                            <div class="form-group" runat="server" id="divSize">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Size</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtSize" TabIndex="3" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>

                            <div class="form-group" runat="server" id="divRange" visible="false">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Range</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtRange" TabIndex="5" ReadOnly="true" CssClass="form-control" />
                                 </div>
                            </div>
                            <asp:Label ID="lblGaugeSupplierID" runat="server" Visible="false"></asp:Label>
                            
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Current Location</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtCurrentLocation" TabIndex="7" ReadOnly="true" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Purchase Date</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtPurchaseDate" TabIndex="9" ReadOnly="true" CssClass="form-control" />
                                 </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Retairment Date</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtRetairementDate" TabIndex="11" ReadOnly="true" CssClass="form-control" />
                               </div>
                            </div>

                            <div class="form-group" runat="server" id="divPemisablerror2">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Permisable Error2</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtPermisableError2" TabIndex="13" ReadOnly="true" TextMode="MultiLine" CssClass="form-control" />
                                 </div>
                            </div>
                             <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Supplier<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlSuplier" runat="server" CssClass="form-control" TabIndex="14">
                                    </asp:DropDownList>
                                     <asp:CompareValidator ID="CompareValidator1" SetFocusOnError="true" runat="server" ControlToValidate="ddlSuplier" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Supplier" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                              <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Status<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="15">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">ISSUED</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator2" SetFocusOnError="true" runat="server" ControlToValidate="ddlStatus" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Status" Operator="NotEqual" ValidationGroup="c" ValueToCompare="0"></asp:CompareValidator>
                                </div>
                            </div>
                          
                        </div>
                    </div>

                    <div class="col-md-offset-5 col-md-6">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSaveGaugeSupplier" TabIndex="16"  ValidationGroup="c" OnClick="btnSaveGaugeSupplier_Click" Text="Save" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnClloseGaugeSupplier" TabIndex="17" CausesValidation="false" Text="Close" OnClick="btnClloseGaugeSupplier_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>

</asp:Content>

