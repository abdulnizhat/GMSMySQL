<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Register Assembly="System.Web.DataVisualization,Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        .buttonstyle {
            width: 230px !important;
            height: 80px !important;
            white-space: normal !important;
            font-family:'Arial Rounded MT' !important;
        }
    </style>
    <h3><%: Title %>Dashboard</h3>
    <div class="col-md-12">
        <div class="row">
            <div class="form-horizontal">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Label runat="server" Font-Bold="true" ID="lbltitleofGaugeDueStat" Text="Gauge Due List"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto" Height="350px">
                                    <asp:GridView ID="grdDueStatus" runat="server" Width="100%"
                                        AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdDueStatus_PageIndexChanging"
                                        CssClass="table table-bordered table-responsive" PageSize="10">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                            <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                            <asp:BoundField DataField="next_due_date" HeaderText="Next Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                        <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Label runat="server" Font-Bold="true" ID="Label1" Text="MSA Due List"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:Panel ID="Panel3" runat="server" Width="100%" ScrollBars="Auto" Height="350px">
                                    <asp:GridView ID="grdMsaDue" runat="server" Width="100%"
                                        AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdMsaDue_PageIndexChanging"
                                        CssClass="table table-bordered table-responsive" PageSize="10">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                            <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                            <asp:BoundField DataField="next_due_date" HeaderText="MSA Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                        <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    </div>

    <div class="col-md-12">
        <div class="row">
            <div class="form-horizontal">
                <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Label runat="server" Font-Bold="true" ID="Label2" Text="Issued Pending Gauge List"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel4" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:Panel ID="Panel5" runat="server" Width="100%" ScrollBars="Auto" Height="350px">
                                    <asp:GridView ID="grdIssue" runat="server" Width="100%"
                                        AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdIssue_PageIndexChanging"
                                        CssClass="table table-bordered table-responsive" PageSize="10">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="issued_id" HeaderText="Issued Id" />
                                            <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                            <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                            <asp:BoundField DataField="issued_to_type" HeaderText="Issued To" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" />
                                            <asp:BoundField DataField="issued_date" HeaderText="Issued Date" DataFormatString="{0:dd/MM/yyyy}" />
                                        </Columns>
                                        <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
              <div class="col-md-6">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Label runat="server" Font-Bold="true" ID="Label5" Text="GMS Executive Report"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                         <asp:Panel ID="Panel10" runat="server" Width="100%">
                                <table style="width: 100%; margin: 0px; border:thick; border-color:blue" class="table table-bordered table-responsive">

                                    <tr class="header cssForExcluiveReportHeader">
                                        <td style="width: 180px" align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman'; font-weight:bold" Text="Details for"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 150px">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Total Count"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 160px">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Last Month"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 170px">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Current Month"></asp:Label>
                                        </td>
                                        <td style="width: 130px">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Year Till Date"></asp:Label>
                                        </td>

                                    </tr>
                                     </table>
                             </asp:Panel>
                             <asp:Panel ID="Panel9" runat="server" Width="100%" Height="350px" ScrollBars="Auto">
                                    <table style="width: 100%; margin: 0px; border:thick; border-color:blue" class="table table-bordered table-responsive">

                                     <tr class="cssForRow2Exv">
                                        <td align="center" style="width: 155px">
                                            <asp:Label runat="server" ID="lblcalibdue" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 175px">
                                            <asp:Label runat="server" ID="lblcalibduecount" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 160px">
                                            <asp:Label runat="server" ID="lblVerificationduecountlmonth" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 170px">
                                            <asp:Label runat="server" ID="lblVerificationduecountcmonth" Style="align-items: center; font-family: 'Times New Roman';" Text="----"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>

                                    </tr>
                                     <tr class="cssForExcluiveReport">
                                        <td align="center" style="width: 150px">
                                            <asp:Label runat="server" ID="lblverificationcom" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 150px">
                                            <asp:Label runat="server" ID="lblverificationcomcount" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 160px">
                                            <asp:Label runat="server" ID="lblverificationcomlmonth" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 170px">
                                            <asp:Label runat="server" ID="lblverificationcomcmonth" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>

                                    </tr>
                                     <tr class="cssForRow2Exv">
                                        <td align="center">
                                          
                                            <asp:Label runat="server" ID="lblMSADue" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblMSADuecount" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblvalidationDuecountlmonth" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblvalidationDuecountcmonth" Style="align-items: center; font-family: 'Times New Roman';" Text="----"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>

                                    </tr>
                                     <tr class="cssForExcluiveReport">
                                        <td align="center">
                                          
                                            <asp:Label runat="server" ID="lblvalidationcom" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblvalidationcomcount" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblvalidationcomcountlmonth" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblvalidationcomcountcmonth" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>

                                    </tr>
                                     <tr class="cssForRow2Exv">
                                        <td align="center">
                                            <asp:Label runat="server" ID="lbldueforreturn" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lbldueforreturncount" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';" Text="----"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';" Text="---"></asp:Label>
                                        </td>

                                    </tr>
                                     <tr class="cssForExcluiveReport">
                                        <td align="center">
                                          
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Total Purchase Cost of Gauge"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="---"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblPCostlastMonth" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  ID="lblPCostcurrentMonth" Style="align-items: center; font-family: 'Times New Roman';" ></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  ID="lblPCostYTD" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>

                                    </tr>
                                     <tr class="cssForRow2Exv">
                                        <td align="center">
                                          
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Total Purchase Cost of Return Gauge"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="---"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblRPCostlastMonth" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  ID="lblRPCostcurrentMonth" Style="align-items: center; font-family: 'Times New Roman';" ></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  ID="lblRPCostYTD" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>

                                    </tr>

                                      <tr class="cssForExcluiveReport">
                                        <td align="center">
                                          
                                            <asp:Label runat="server" Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="Total Calibration Cost"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  Style="align-items: center; font-family: 'Times New Roman';font-weight:bold" Text="---"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblcalibcostLastM" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  ID="lblcalibcostcurrentM" Style="align-items: center; font-family: 'Times New Roman';" ></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label runat="server"  ID="lblcalibcostYTD" Style="align-items: center; font-family: 'Times New Roman';"></asp:Label>
                                        </td>

                                    </tr>
                                   
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="col-md-12">
        <div class="row">
            <div class="form-horizontal">
                <div class="col-md-12">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Label runat="server" Font-Bold="true" ID="Label3" Text="Schedule Vs Calibration From Last year To Uptodate"></asp:Label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <div style="text-align: left" runat="server" id="chartDiv">
                                <asp:Panel ID="Panel6" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:Chart ID="Chart1" runat="server" BorderDashStyle="Solid"
                                        BackSecondaryColor="White" BackGradientStyle="TopBottom" BorderWidth="2px" BackColor="211, 223, 240"
                                        BorderColor="#1A3B69" Width="1000px" Height="380px" Palette="None">
                                        <Titles>
                                            <asp:Title Text="Scheduled VS Calibration Report" Font="Microsoft Sans Serif, 10pt, style=Bold" />
                                        </Titles>
                                        <Legends>
                                            <asp:Legend
                                                BackColor="Transparent"
                                                ForeColor="#000000"
                                                BorderColor="#1A3B69"
                                                Font="Bold">
                                            </asp:Legend>
                                        </Legends>
                                        <Series>
                                            <asp:Series Name="Series1" IsValueShownAsLabel="True" LegendText="Calibration Scheduled" Color="#f4f124" ToolTip="#VALY">
                                            </asp:Series>
                                            <asp:Series Name="Series2" IsValueShownAsLabel="True" LegendText="Calibration Transaction" Color="#1ccc10" ToolTip="#VALY">
                                            </asp:Series>
                                        </Series>

                                        <ChartAreas>
                                            <asp:ChartArea Name="ChartArea1">
                                                <AxisX LineColor="DarkCyan" Interval="1"></AxisX>
                                                <AxisY LineColor="DarkMagenta"></AxisY>
                                            </asp:ChartArea>
                                        </ChartAreas>
                                        <%--<Titles>
                                        <asp:Title Name="Title1" Text="Bar Chart">
                                        </asp:Title>
                                    </Titles>--%>
                                    </asp:Chart>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </div>
    </div>

    <div runat="server" visible="false">
        <div class="form-group">
            <div class="col-md-12">
               
                <asp:Table ID="Table1" runat="server" Width="1000px">
                    <asp:TableRow runat="server">
                        <asp:TableCell ID="tablecell" runat="server">
                  
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                
                <asp:Label ID="lblcustmailid" runat="server" Visible="false"></asp:Label>
                <asp:Panel ID="Panel7" runat="server" Width="100%" ScrollBars="Auto">
                    <asp:Panel ID="Panel8" runat="server" Width="100%" ScrollBars="Auto" Height="350px">
                        <asp:GridView ID="grdsendCustomerGaugeStatus" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive">
                            <HeaderStyle CssClass="header" />
                            <Columns>
                                <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                <asp:BoundField DataField="current_location" HeaderText="Current Location" />
                                <asp:BoundField DataField="size_range" HeaderText="Size/Range" />
                                <asp:BoundField DataField="next_due_date" HeaderText="Next Due Date" DataFormatString="{0:dd/MM/yyyy}" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>

