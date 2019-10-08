<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GaugeDetailsReportViewer.aspx.cs" Inherits="GaugeDetailsReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <div style="margin-left:250px">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" WaitMessageFont-Names="Verdana" AsyncRendering="False" SizeToReportContent="True"
            WaitMessageFont-Size="14pt" Height="650px" Width="730px" ShowRefreshButton="False">
            <LocalReport ReportPath="GaugeDetailsReport.rdlc" EnableExternalImages="True">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSetcust" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
             <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" TypeName="DataSetCustomerTableAdapters."></asp:ObjectDataSource>
             <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="GaugeDetailsReportDataSetTableAdapters."></asp:ObjectDataSource>
             <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetData" TypeName="DataSetCustomerTableAdapters.spCustomerRepTableAdapter"></asp:ObjectDataSource>
            
             </div>
       
    </div>
    </form>
</body>
</html>
