<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalibrationDueStatusReportViewer.aspx.cs" Inherits="CalibrationDueStatusReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div style="margin-left:250px">
    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" WaitMessageFont-Names="Verdana" Margin="0"
            WaitMessageFont-Size="14pt" Height="443px" Width="720px" ShowRefreshButton="False">
        <LocalReport ReportPath="CalibrationDueStatusReport.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
            </DataSources>
        </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="CalibDueStatusReportDataSetTableAdapters."></asp:ObjectDataSource>
    </div>
    </form>
</body>

</html>
