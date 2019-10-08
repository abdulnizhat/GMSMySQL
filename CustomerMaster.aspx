<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerMaster.aspx.cs" Inherits="CustomerMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">

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
            var chk = '<%= Session["dletecustLink"].ToString() %>';
            if (chk == "YES") {
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
        }

    </script>
    <h3><%: Title %>Customer Master</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Button runat="server" ID="btnAddnewCustomer" Text="Add New" CssClass="btn btn-primary" OnClick="btnAddnewCustomer_Click" />
                            </div>
                            <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Customer Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Contact No.-Wise</asp:ListItem>
                                                     <asp:ListItem>Branch Name-Wise</asp:ListItem>                                                                                                      
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="ddlsortby"
                                                    Display="Dynamic" ErrorMessage="Select search option" Operator="NotEqual" ValidationGroup="a" CssClass="text-danger"
                                                    ValueToCompare="--Select--"></asp:CompareValidator>
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
                        </div>
                        <div class="form-group">

                            <div class="col-md-12">
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="grdCustomer" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdCustomer_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="customer_id" HeaderText="Customer Id" />
                                            <asp:BoundField DataField="customer_name" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="branch_name" HeaderText="Branch" />
                                            <asp:BoundField DataField="mobile_no" HeaderText="Contact No" />
                                            <asp:BoundField DataField="email" HeaderText="Email Id" />
                                            <asp:BoundField DataField="owner" HeaderText="Owner" />
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtadd" BackColor="#ffffad" runat="server" TextMode="MultiLine" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updaction" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnEditCustomer" />
                                                            <asp:PostBackTrigger ControlID="LnkBtnDelete" />
                                                        </Triggers>
                                                        <ContentTemplate>


                                                            <asp:LinkButton ID="btnEditCustomer" OnClick="btnEditCustomer_Click" runat="server" CommandArgument='<%# Eval("customer_id")%>' class="btn btn-sm btn-info" title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                            &nbsp;
                                                <asp:LinkButton ID="LnkBtnDelete" OnClientClick="Confirm()" OnClick="LnkBtnDelete_Click" runat="server" CommandArgument='<%# Eval("customer_id")%>' class="btn btn-sm btn-info" title="Delete"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
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
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Customer ID</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtcustomerId" CssClass="form-control" ReadOnly="true" />
                                    <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtcustomerId" SetFocusOnError="true"
                                        CssClass="text-danger" ErrorMessage="Customer ID field is required." />
                                </div>
                            </div>
                            <asp:Label ID="lblCustomerId" runat="server" Visible="false"></asp:Label>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Customer Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="1" ID="txtcustomerName" CssClass="form-control" MaxLength="50" />
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="c" SetFocusOnError="true"
                                        ControlToValidate="txtcustomerName" Display="Dynamic" CssClass="text-danger" ErrorMessage="Customer Name field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Branch Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlBranch" runat="server" TabIndex="2" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlBranch" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Branch" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--" SetFocusOnError="true"></asp:CompareValidator>

                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Country Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlCountry" TabIndex="3" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="cmp7" runat="server" ControlToValidate="ddlCountry" Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select Country" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>

                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">State Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlState" runat="server" TabIndex="4" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server"
                                        ControlToValidate="ddlState" Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select State" Operator="NotEqual" ValidationGroup="c"
                                        ValueToCompare="--Select--"></asp:CompareValidator>

                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">City Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="ddlcity" TabIndex="5" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlcity" Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select City" Operator="NotEqual" ValidationGroup="c" ValueToCompare="--Select--"></asp:CompareValidator>

                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Contact Number<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtMobile" MaxLength="15" TabIndex="6"  CssClass="form-control" />
                                    <%--<asp:FilteredTextBoxExtender ID="txtMobile_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" TargetControlID="txtMobile" ValidChars="0123456789">
                                    </asp:FilteredTextBoxExtender>--%>
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="c" ControlToValidate="txtMobile" SetFocusOnError="True"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Contact Number field is required." />
                                   <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ErrorMessage="Enter valid mobile number." ValidationGroup="c" SetFocusOnError="true"
                                        ControlToValidate="txtMobile" Display="Dynamic" ForeColor="Maroon"
                                        ValidationExpression="\d{10}"></asp:RegularExpressionValidator>--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Email<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="7" ID="txtEmail" MaxLength="49" CssClass="form-control" />
                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ControlToValidate="txtEmail" SetFocusOnError="true"
                                        CssClass="text-danger" ValidationGroup="c" ErrorMessage="Email field is required." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                        Display="Dynamic" ForeColor="Maroon" ErrorMessage="Enter valid email id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="c" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                    </asp:ValidatorCalloutExtender>

                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Phone1</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="8" ID="txtPhone1" MaxLength="11" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Maroon" ControlToValidate="txtPhone1" ID="RegularExpressionValidator4"
                                        ValidationExpression="^[\s\S]{6,11}$" runat="server" ValidationGroup="c" SetFocusOnError="true"
                                        ErrorMessage="Minimum 6 and Maximum 11 characters required."></asp:RegularExpressionValidator>

                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Phone2</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="9" ID="txtPhone2" MaxLength="11" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ControlToValidate="txtPhone2" ID="RegularExpressionValidator6"
                                        ValidationExpression="^[\s\S]{6,11}$" runat="server" ValidationGroup="c"
                                        ErrorMessage="Minimum 6 and Maximum 11 characters required."></asp:RegularExpressionValidator>
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


                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Address1<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtAddress1" TabIndex="10" TextMode="MultiLine"
                                        CssClass="form-control" MaxLength="199" />
                                    <asp:RequiredFieldValidator ValidationGroup="c" Display="Dynamic" runat="server" ControlToValidate="txtAddress1" CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Address1 field is required." />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Address2</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="11" ID="txtAddress2" MaxLength="199" TextMode="MultiLine" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Fax Number</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="12" ID="txtFaxNumber" MaxLength="11" onkeypress="return onlyNos(event,this);" CssClass="form-control" />
                                    <asp:FilteredTextBoxExtender ID="txtFaxNumber_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" TargetControlID="txtFaxNumber" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
                                    <asp:RegularExpressionValidator Display="Dynamic" ForeColor="Maroon" ControlToValidate="txtFaxNumber" ID="RegularExpressionValidator5"
                                        ValidationExpression="^[\s\S]{6,11}$" runat="server" ValidationGroup="c" SetFocusOnError="true"
                                        ErrorMessage="Minimum 6 and Maximum 11 characters required."></asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Owner Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="13" ID="txtOwnerName" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="c" ControlToValidate="txtOwnerName"
                                        Display="Dynamic" SetFocusOnError="true" CssClass="text-danger" ErrorMessage="Owner Name field is required." />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Pin Code.<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="14" onkeypress="return onlyNos(event,this);" ID="txtpin" CssClass="form-control" MaxLength="6" />
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="c" ControlToValidate="txtpin" SetFocusOnError="True"
                                        Display="Dynamic" CssClass="text-danger" ErrorMessage="Pin Code field is required." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                        ErrorMessage="Enter valid Pin number." ValidationGroup="c" SetFocusOnError="true"
                                        ControlToValidate="txtpin" Display="Dynamic" ForeColor="Maroon"
                                        ValidationExpression="^[\s\S]{6,11}$"></asp:RegularExpressionValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Website</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="15" ID="txtWebsite" CssClass="form-control" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic"
                                        ControlToValidate="txtWebsite" ErrorMessage="Enter valid website format" ForeColor="Maroon"
                                        SetFocusOnError="True" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" ValidationGroup="c"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">License Number</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="16" ID="txtLicenseNo" CssClass="form-control" />
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtLicenseNo" CssClass="text-danger" ErrorMessage="License Number field is required." />--%>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Agent Name</asp:Label>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" TabIndex="16" ID="txtagent" CssClass="form-control" />
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="txtLicenseNo" CssClass="text-danger" ErrorMessage="License Number field is required." />--%>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-4 control-label">Logo Upload</asp:Label>
                                <div class="col-md-8">
                                    <asp:FileUpload ID="fileuploadlogo" runat="server" TabIndex="17" CssClass="form-control" />
                                    <asp:Label ID="lblImageName" runat="server"></asp:Label>
                                    <br />
                                    <asp:Label ID="Label1" Text="jpeg, png, gif, bmp, jpg only" ForeColor="#cc3300" runat="server"></asp:Label>
                                </div>
                            </div>
                             <%--<div class="form-group">
                                <div class="col-md-offset-5 col-md-12" style="column-span:all">
                                    <asp:Image ID="imglogo" runat="server" CssClass="form-control" Height="60px" Width="60px" />
                                </div>
                            </div>--%>
                        </div>
                    </div>

                    <div class="col-md-offset-5 col-md-6" style="margin-bottom: 10px">
                        <div class="col-md-2">
                            <asp:UpdatePanel ID="updd" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSaveCustomer" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button runat="server" ID="btnSaveCustomer" TabIndex="18" ValidationGroup="c" OnClick="btnSaveCustomer_Click" Text="Save" CssClass="btn btn-primary" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnCloseCustomer" TabIndex="19" Text="Close" CausesValidation="false" OnClick="btnCloseCustomer_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>

                </div>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
