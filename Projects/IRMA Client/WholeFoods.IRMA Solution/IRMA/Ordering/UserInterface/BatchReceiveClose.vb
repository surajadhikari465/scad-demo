Option Strict Off
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports BatchReceiveCloseDAO
Imports WholeFoods.IRMA.Ordering.DataAccess.OrderingDAO
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmBatchReceiveClose

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Const iALL As Short = 0
    Private Const iNONE As Short = 1

    Private Sub frmBatchReceiveClose_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadFacility()
        LoadSubteam()
        If cmbFacility.Items.Count > 1 Then
            cmbFacility.SelectedValue = -1
        End If
        cmbSubteam.SelectedValue = -1
        dtpStartDate.Value = Today()
        dtpEndDate.Value = Today()
        cmdSelectAll.Image = imgIcons.Images.Item(iALL)
    End Sub

    Private Sub cmdEditItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        frmReceivingList.ShowDialog()
        frmReceivingList.Close()
        frmReceivingList.Dispose()

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

        Dim b As New BatchReceiveCloseBO

        b.BRC_Vendor_ID = CInt(cmbFacility.SelectedValue)
        b.BRC_Subteam_No = CInt(cmbSubteam.SelectedValue)
        b.BRC_StartDate = CDate(dtpStartDate.Value)
        b.BRC_EndDate = CDate(dtpEndDate.Value)

        Dim missingFields As String = ""

        If cmbFacility.SelectedValue = Nothing Then
            missingFields = "Choose a Facility " & vbCrLf
        End If

        If cmbSubteam.SelectedValue = Nothing Then
            missingFields = missingFields & "Choose a Subteam"
        End If

        If missingFields <> "" Then
            MsgBox(missingFields, MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        LoadDataGrid(b)

    End Sub

    Private Sub LoadFacility()

        cmbFacility.DataSource = BatchReceiveCloseDAO.BRC_GetFacilities.Tables(0)
        cmbFacility.DisplayMember = "CompanyName"
        cmbFacility.ValueMember = "Vendor_ID"

    End Sub

    Private Sub LoadSubteam()

        cmbSubteam.DataSource = BatchReceiveCloseDAO.GetSubteams.Tables(0)
        cmbSubteam.DisplayMember = "SubTeam_Name"
        cmbSubteam.ValueMember = "SubTeam_No"

    End Sub

    Private Sub LoadDataGrid(ByVal b As BatchReceiveCloseBO)

        Dim d As New BatchReceiveCloseDAO
        With ugOrderList
            .DataSource = d.BRC_GetOrders(b)
        End With

        If (ugOrderList.Rows.Count = 0) Then
            MsgBox("No orders found", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

    End Sub


    Private Sub cmdReceiveAndClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReceiveAndClose.Click

        Dim ol As New ArrayList
        Dim r As UltraGridRow
        For Each r In ugOrderList.Rows.All
            If CBool(r.Cells("Select").Value) Then
                ol.Add(r.Cells("OrderHeader_ID").Value)
            End If
        Next

        If (ol.Count = 0) Then
            MsgBox("No orders selected", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Dim o As Integer

        Dim d As New BatchReceiveCloseDAO

        For Each o In ol
            'receive each selected order
            d.BRC_ReceiveOrder(o, giUserID)

            'close each selected order
            CloseOrder(o)
        Next

        ugOrderList.DataSource = Nothing

    End Sub

    Private Sub ugOrderList_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugOrderList.InitializeLayout
        With ugOrderList
            If .DisplayLayout.Bands(0).Columns.Exists("Select") = False Then
                .DisplayLayout.Bands(0).Columns.Insert(0, "Select").DataType = GetType(Boolean)
            End If

            .DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
            .DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
            .DisplayLayout.Bands(0).Columns("Select").CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
            .DisplayLayout.Bands(0).Columns("Select").CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit
            .DisplayLayout.Bands(0).Columns("OrderHeader_ID").Header.Caption = "PO Number"
            .DisplayLayout.Bands(0).Columns("Subteam_Name").Header.Caption = "Subteam"
            .DisplayLayout.Bands(0).Columns("Store_Name").Header.Caption = "Store"
            .DisplayLayout.Bands(0).Columns("OrderedCost").Header.Caption = "Ordered Cost"
            .DisplayLayout.Bands(0).Columns("Expected_Date").Header.Caption = "Expected Date"
        End With
    End Sub

    Private Sub ugOrderList_DoubleClickRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugOrderList.DoubleClickRow

        glOrderHeaderID = CInt(e.Row.Cells("OrderHeader_ID").Value)

        frmReceivingList.ShowDialog()
        frmReceivingList.Close()
        frmReceivingList.Dispose()

    End Sub

    Private Sub cmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectAll.Click
        logger.Debug("cmdSelectAll_Click Entry")

        Dim r As UltraGridRow

        If cmdSelectAll.Tag = "Select" And ugOrderList.Rows.Count > 0 Then
            cmdSelectAll.Tag = "Deselect"
            cmdSelectAll.Image = imgIcons.Images.Item(iNONE)
            ToolTip1.SetToolTip(cmdSelectAll, "Deselect All")

            ' highlight all the rows
            For Each r In ugOrderList.Rows.All
                r.Cells("Select").Value = True
            Next
        Else
            cmdSelectAll.Tag = "Select"
            cmdSelectAll.Image = imgIcons.Images.Item(iALL)
            ToolTip1.SetToolTip(cmdSelectAll, "Select All")

            ' unhighlight all the rows
            For Each r In ugOrderList.Rows.All
                r.Cells("Select").Value = False
            Next
        End If

        logger.Debug("cmdSelectAll_Click Exit")
    End Sub

End Class