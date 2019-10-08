<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MailSetting.aspx.cs" Inherits="MailSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <h3><%: Title %> Mail Setting</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-10">
                                <asp:Button runat="server" Visible="false" ID="btnAddMailSetting" Text="Add New" OnClick="btnAddMailSetting_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="grdmailSetting" runat="server" HeaderStyle-Wrap="false" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive" PageSize="10" OnPageIndexChanging="grdmailSetting_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>

                                            <asp:BoundField DataField="mail_setting_id" HeaderText="Id" Visible="false" />
                                            <asp:BoundField DataField="email_id_from" HeaderText="Sender Email Id" />
                                            <asp:BoundField DataField="port" HeaderText="Sender Smtp Port" />
                                            <asp:BoundField DataField="specified_emai_to_send" HeaderText="Specified Email Id To Send" ItemStyle-Wrap="true" />
                                            <asp:BoundField DataField="status" HeaderText="Status" />
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditMailSetting" runat="server" OnClick="btnEditMailSetting_Click"
                                                       CommandArgument='<%# Eval("mail_setting_id")%>' class="btn btn-sm btn-info" title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
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
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Sender Email Id<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TabIndex="1" />
                                <asp:Label ID="lblMailSettingId" runat="server" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Email Id field is required." ValidationGroup="a" />
                            </div>
                        </div>
                         <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Sender Email Id Credential<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtpassword" CssClass="form-control" TabIndex="2" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtpassword" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Sender credential field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Sender Port No.<b style="color: Red">*</b></asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="txtPort" CssClass="form-control" TabIndex="3" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPort" SetFocusOnError="true"
                                    CssClass="text-danger" ErrorMessage="Sender port number field is required." ValidationGroup="a" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Specify Email Id To Send</asp:Label>
                            <div class="col-md-4">
                                <asp:TextBox runat="server" ID="txtSpecifyMailId" CssClass="form-control" TabIndex="2" />
                            </div>
                            <div style="float: left;">
                                <asp:UpdatePanel ID="updAddemai" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>

                        <div class="form-group" style="max-height: 200px">
                            <div class="col-md-offset-2 col-md-4">
                                <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="grdAddEmailIdToSend" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true"
                                        CssClass="table table-bordered table-responsive">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>

                                            <asp:BoundField DataField="mailid" HeaderText="Email Id" />
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="75px">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updAddemai" runat="server">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnDeleteEmailId" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnDeleteEmailId" runat="server" OnClick="btnDeleteEmailId_Click"
                                                                CommandArgument='<%# Eval("mailid")%>' class="btn btn-sm btn-info" title="Delete"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-md-2 control-label">Status</asp:Label>
                            <div class="col-md-10">
                                <asp:RadioButtonList ID="rd_status" runat="server" RepeatDirection="Horizontal" TabIndex="3">
                                    <asp:ListItem Selected="True" Text="Active" Value="0">
                                    </asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="1">
                                    </asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                        </div>

                        <div class="col-md-offset-2 col-md-6">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveMailSetting" OnClick="btnSaveMailSetting_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" TabIndex="3" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnCloseSaveMailSetting" Text="Close" OnClick="btnCloseSaveMailSetting_Click" CssClass="btn btn-primary" TabIndex="4" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>

    </asp:MultiView>
</asp:Content>

