<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="Default3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
 <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
    <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Store Location<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                           <asp:GridView ID="grdGauge" runat="server" Width="100%" AutoGenerateColumns="false"
                                        CssClass="table table-bordered table-responsive" >
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="employee_name" HeaderText="Employee Name" />
                                            <asp:BoundField DataField="mobile_no" HeaderText="Mobile No" />
                                          
                                        </Columns>
                                        <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                    </asp:GridView>
                                      <asp:Table ID="Table1" runat="server" Width="1000px">
            <asp:TableRow runat="server">
                <asp:TableCell ID="tablecell" runat="server">
                  
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
                                    </div>
                                </div>
                                </div>
                            </div>
                         </div>
                    </div>
         </div>
    </form>
</body>
</html>
