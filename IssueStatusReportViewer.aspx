<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IssueStatusReportViewer.aspx.cs" Inherits="IssueStatusReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-left:250px">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"
                Font-Size="8pt" ShowRefreshButton="False"
                WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="650px" Width="903px">
                <LocalReport ReportPath="IssueStatusReport.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
