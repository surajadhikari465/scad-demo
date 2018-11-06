Imports WholeFoods.IRMA.Ordering.BusinessLogic.FairShareAllocationBO

Friend Class frmAllocationReport
    Inherits System.Windows.Forms.Form

#Region " Private Fields"

    Private m_bCancelled As Boolean
    Private iStoreNo As Integer
    Private sStoreName As String
    Private iSubteamNo As Integer

#End Region

#Region " Properties"

    Public Property WarehouseSentOption() As WarehouseStatus
        Get
            If Me.optWarehouseSent(0).Checked Then
                Return WarehouseStatus.All
            ElseIf Me.optWarehouseSent(1).Checked Then
                Return WarehouseStatus.Sent
            ElseIf Me.optWarehouseSent(2).Checked Then
                Return WarehouseStatus.NotSent
            End If
        End Get
        Set(ByVal value As WarehouseStatus)
            Select Case value
                Case WarehouseStatus.All
                    Me.optWarehouseSent(0).Checked = True
                Case WarehouseStatus.Sent
                    Me.optWarehouseSent(1).Checked = True
                Case WarehouseStatus.NotSent
                    Me.optWarehouseSent(2).Checked = True
            End Select
        End Set
    End Property

    Public Property IncludeExpectedDate() As Boolean
        Get
            Return Me.checkIncludeWOO.Checked
        End Get
        Set(ByVal value As Boolean)
            Me.grpPOExpectedDate.Enabled = value
            Me.checkIncludeWOO.Checked = value
        End Set
    End Property

    Public Property ShipDate() As Date
        Get
            Return Me.dtShipStart.Value
        End Get
        Set(ByVal value As Date)
            Me.dtShipStart.Value = value
        End Set
    End Property

    Public Property ExpectedStartDate() As Date
        Get
            Return Me.dtWOOStart.Value
        End Get
        Set(ByVal value As Date)
            Me.dtWOOStart.Value = value
        End Set
    End Property

    Public Property ExpectedEndDate() As Date
        Get
            Return Me.dtWOOEnd.Value
        End Get
        Set(ByVal value As Date)
            Me.dtWOOEnd.Value = value
        End Set
    End Property

    Public Property PreOrderOption() As PreOrder
        Get
            If Me.optPreOrder(0).Checked Then
                Return PreOrder.All
            ElseIf Me.optPreOrder(1).Checked Then
                Return PreOrder.PreOrder
            ElseIf Me.optPreOrder(2).Checked Then
                Return PreOrder.NonPreOrder
            End If
        End Get
        Set(ByVal value As PreOrder)
            Select Case value
                Case PreOrder.All
                    Me.optPreOrder(0).Checked = True
                Case PreOrder.PreOrder
                    Me.optPreOrder(1).Checked = True
                Case PreOrder.NonPreOrder
                    Me.optPreOrder(2).Checked = True
            End Select
        End Set
    End Property

    Public Property BOHOption() As BOH
        Get
            If Me.optBOH(0).Checked Then
                Return BOH.All
            ElseIf Me.optBOH(1).Checked Then
                Return BOH.GreaterThanZero
            ElseIf Me.optBOH(2).Checked Then
                Return BOH.LessThanEqualZero
            ElseIf Me.optBOH(3).Checked Then
                Return BOH.LessThanZero
            ElseIf Me.optBOH(4).Checked Then
                Return BOH.GreaterThanEqualZero
            End If
        End Get
        Set(ByVal value As BOH)
            Select Case value
                Case BOH.All
                    Me.optBOH(0).Checked = True
                Case BOH.GreaterThanZero
                    Me.optBOH(1).Checked = True
                Case BOH.LessThanEqualZero
                    Me.optBOH(2).Checked = True
                Case BOH.LessThanZero
                    Me.optBOH(3).Checked = True
                Case BOH.GreaterThanEqualZero
                    Me.optBOH(4).Checked = True
            End Select
        End Set
    End Property

    Public Property OrderSubteamTypeOption() As OrderSubteamType
        Get
            If Me.optAllOrders.Checked Then
                Return OrderSubteamType.All
            ElseIf Me.optNonRetail.Checked Then
                Return OrderSubteamType.NonRetail
            ElseIf Me.optRetail.Checked Then
                Return OrderSubteamType.Retail
            End If
        End Get
        Set(ByVal value As OrderSubteamType)
            Select Case value
                Case OrderSubteamType.All
                    Me.optAllOrders.Checked = True
                Case OrderSubteamType.NonRetail
                    Me.optNonRetail.Checked = True
                Case OrderSubteamType.Retail
                    Me.optRetail.Checked = True
            End Select
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return iStoreNo
        End Get
        Set(ByVal value As Integer)
            iStoreNo = value
        End Set
    End Property

    Public Property StoreName() As String
        Get
            Return Me.sStoreName
        End Get
        Set(ByVal value As String)
            Me.sStoreName = value
        End Set
    End Property

    Public Property SubteamNo() As Integer
        Get
            Return Me.iSubteamNo
        End Get
        Set(ByVal value As Integer)
            Me.iSubteamNo = value
        End Set
    End Property

#End Region

#Region " Private Methods"

    Private Sub frmAllocationReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.dtShipStart.Value = Date.Today.AddDays(1).ToShortDateString

    End Sub

    Private Sub cmdRunReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRunReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        ' Report Name.
        Dim filename As String = String.Empty

        If Me.optDetailReport.Checked Then
            filename = "AllocationReportDetail"
        Else
            filename = "AllocationReportSummary"
        End If

        sReportURL.Append(filename)

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '-----------------------------------------------------------------
        ' Add all Report Parameters
        '-------------------------------------------------------------------------

        sReportURL.Append("&Store_No=" & Me.StoreNo)
        sReportURL.Append("&Transfers=" & CInt(Me.OrderSubteamTypeOption))
        sReportURL.Append("&SubTeam_No=" & Me.SubteamNo)
        sReportURL.Append("&Pre_Order=" & CInt(Me.PreOrderOption))
        sReportURL.Append("&BOH=" & CInt(Me.BOHOption))
        sReportURL.Append("&IncludeWOO=" & Me.IncludeExpectedDate)
        sReportURL.Append("&ShipDate=" & Me.ShipDate.ToShortDateString)
        sReportURL.Append("&ExpectedDateStart=" & Me.ExpectedStartDate.ToShortDateString)
        sReportURL.Append("&ExpectedDateEnd=" & Me.ExpectedEndDate.ToShortDateString)
        sReportURL.Append("&WarehouseSent=" & CInt(Me.WarehouseSentOption))
        sReportURL.Append("&Store_Name=" & Me.StoreName)

        Try
            Call ReportingServicesReport(sReportURL.ToString)
        Catch ex As Exception
            MessageBox.Show("An error ocurred attempting to run the report: " & Environment.NewLine & ex.InnerException.Message, "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Me.Close()

    End Sub

    Private Sub checkIncludeWOO_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles checkIncludeWOO.CheckedChanged

        Me.grpPOExpectedDate.Enabled = Me.checkIncludeWOO.Checked

    End Sub

#End Region

End Class