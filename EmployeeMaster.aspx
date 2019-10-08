<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EmployeeMaster.aspx.cs" Inherits="EmployeeMaster" %>

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
            var chk = '<%= Session["dleteempLink"].ToString() %>';
            if (chk == "YES") {
                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm("Do you want to delete employee record?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            }
        }
    </script>
    <h3><%: Title %>Employee Master</h3>
    <style>
        .space-left {
            margin-left: 10px;
        }
    </style>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-1">
                                <asp:Button runat="server" ID="btnAddEmployee" Text="Add New" OnClick="btnAddEmployee_Click" CssClass="btn btn-primary" />
                            </div>
                            <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Employee Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Mobile No.-Wise</asp:ListItem>                                                                                                      
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
                                    <asp:GridView ID="grdEmployee" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdEmployee_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>                                           
                                            <asp:BoundField DataField="employee_id" HeaderText="EMP Id" />
                                            <asp:BoundField DataField="employee_name" HeaderText="Employee" />
                                            <asp:BoundField DataField="mobile_no" HeaderText="Mobile No." />
                                            <asp:BoundField DataField="email" HeaderText="Email Id" />
                                            <asp:TemplateField HeaderText="Address">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtadd" BackColor="#ffffad" runat="server" ReadOnly="true" TextMode="MultiLine" Text='<%# Bind("Address") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="department_name" HeaderText="Department" />
                                            <asp:BoundField DataField="designation_name" HeaderText="Designation" />
                                            <asp:BoundField DataField="branch_name" HeaderText="Branch" />
                                            <asp:BoundField DataField="Customer_name" HeaderText="Customer" />
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditEmployee" runat="server" OnClick="btnEditEmployee_Click"
                                                        CommandArgument='<%# Eval("employee_id")%>' class="btn btn-sm btn-info"
                                                        title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                    &nbsp;
                                                <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="Confirm()" OnClick="lnkDelete_Click"
                                                    CommandArgument='<%# Eval("employee_id")%>' class="btn btn-sm btn-info"
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
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Is Supplier</asp:Label>
                                <div class="col-md-9">
                                    <asp:RadioButtonList ID="rbtIsSupplier" runat="server"
                                        AutoPostBack="True" OnSelectedIndexChanged="rbtIsSupplier_SelectedIndexChanged"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True">No</asp:ListItem>
                                        <asp:ListItem class="space-left">Yes</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Employee Id</asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" ID="txtEmployeeId" ReadOnly="true" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmployeeId" SetFocusOnError="true"
                                        CssClass="text-danger" ValidationGroup="e" Display="Dynamic" ErrorMessage="Employee Id field is required." />
                                </div>
                            </div>
                            <asp:Label ID="lblEmpID" runat="server" Visible="false"></asp:Label>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Employee Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" ID="txtEmployeeName" TabIndex="1" CssClass="form-control" MaxLength="45" />
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="e" Display="Dynamic" ControlToValidate="txtEmployeeName"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Employee Name field is required." />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Branch Name<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlBranch" TabIndex="2" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ddlBranch"
                                        Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select Branch" Operator="NotEqual"
                                        ValidationGroup="e" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Department<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlDepartment" TabIndex="3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlDepartment" Display="Dynamic" ForeColor="Maroon"
                                        ErrorMessage="Select Department" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="e" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Designation<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlDesignation" TabIndex="4" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="ddlDesignation"
                                        Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select Designation" Operator="NotEqual"
                                        ValidationGroup="e" ValueToCompare="--Select--"></asp:CompareValidator>


                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Mobile Number<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" TabIndex="5" ID="txtMobileNo" onkeypress="return onlyNos(event,this);" CssClass="form-control" MaxLength="10" />
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="e" Display="Dynamic" ControlToValidate="txtMobileNo"
                                        CssClass="text-danger" ErrorMessage="Mobile Number field is required." SetFocusOnError="true" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ErrorMessage="Enter valid mobile number." ValidationGroup="e" SetFocusOnError="true"
                                        ControlToValidate="txtMobileNo" Display="Dynamic" ForeColor="Maroon"
                                        ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
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



                            <%-- <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Role<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlRole"
                                        CssClass="text-danger" ErrorMessage="Role field is required." />
                                </div>
                            </div>--%>

                            <div class="form-group" runat="server" id="divSupplier" visible="false">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Supplier</asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlSupplier" AutoPostBack="true" TabIndex="2" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator8" runat="server" ControlToValidate="ddlSupplier"
                                        Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select Supplier" Operator="NotEqual"
                                        ValidationGroup="e" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Email Id<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" ID="txtEmail" TabIndex="6" CssClass="form-control" MaxLength="49" />
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="e" Display="Dynamic" ControlToValidate="txtEmail"
                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Email ID field is required." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                        Display="Dynamic" ForeColor="Maroon" ErrorMessage="Enter valid email id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        ValidationGroup="e" SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    <%--<asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                    </asp:ValidatorCalloutExtender>--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Country<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlCountry" TabIndex="7" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="ddlCountry"
                                        Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select Country" Operator="NotEqual"
                                        ValidationGroup="e" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">State<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlState" TabIndex="8" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ddlState"
                                        Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select State" Operator="NotEqual"
                                        ValidationGroup="e" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">City<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:DropDownList ID="ddlCity" TabIndex="9" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToValidate="ddlCity"
                                        Display="Dynamic" ForeColor="Maroon" SetFocusOnError="true"
                                        ErrorMessage="Select City" Operator="NotEqual"
                                        ValidationGroup="e" ValueToCompare="--Select--"></asp:CompareValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" CssClass="col-md-3 control-label">Address<b style="color: Red">*</b></asp:Label>
                                <div class="col-md-9">
                                    <asp:TextBox runat="server" TabIndex="10" ID="txtAddress" TextMode="MultiLine" MaxLength="199" CssClass="form-control" />
                                    <asp:RequiredFieldValidator runat="server" ValidationGroup="e" Display="Dynamic" ControlToValidate="txtAddress"
                                        CssClass="text-danger" ErrorMessage="Address field is required." SetFocusOnError="true" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-offset-5 col-md-6">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSaveEmployee" TabIndex="11" ValidationGroup="e" OnClick="btnSaveEmployee_Click" Text="Save" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnCloseEmployee" TabIndex="12" Text="Close" CausesValidation="false" OnClick="btnCloseEmployee_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>


            </div>

        </asp:View>
    </asp:MultiView>

</asp:Content>

