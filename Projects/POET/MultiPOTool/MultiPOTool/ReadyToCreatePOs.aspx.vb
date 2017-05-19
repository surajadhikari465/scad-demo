Imports Infragistics.WebUI.UltraWebGrid
Imports Infragistics.Documents.Report
Imports MultiPOTool.BOValidatedPOs
Imports Infragistics.Web.UI.GridControls
Imports Infragistics.Shared
Imports Infragistics.WebUI

Partial Public Class ReadyToCreatePOs
    Inherits System.Web.UI.Page

    Private ReadOnly Property PushPOs() As List(Of Boolean)
        Get
            If Me.ViewState("PushPOs") Is Nothing Then
                Me.ViewState("PushPOs") = New List(Of Boolean)()
            End If
            Return CType(Me.ViewState("PushPOs"), List(Of Boolean))
        End Get
    End Property

    Private ReadOnly Property DeletePOs() As List(Of Boolean)
        Get
            If Me.ViewState("DeletePOs") Is Nothing Then
                Me.ViewState("DeletePOs") = New List(Of Boolean)()
            End If
            Return CType(Me.ViewState("DeletePOs"), List(Of Boolean))
        End Get
    End Property

    Protected Sub SaveCheckBoxState()

        Me.PushPOs.Clear()
        Me.DeletePOs.Clear()

        For Each row As UltraGridRow In uwgPOsReadyToPush.Rows
            Dim chkPushSelect As Boolean = CType(row.Cells(0).Value, Boolean)
            Me.PushPOs.Add(chkPushSelect)

            Dim chkDeleteSelect As Boolean = CType(row.Cells(13).Value, Boolean)
            Me.DeletePOs.Add(chkDeleteSelect)
        Next

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Session("POPushAll") = False
            Session("DeleteAll") = False

            Session("CurrentUser") = Session("UserID")

            Me.ViewState.Clear()

            Me.btnDeletePOs.Attributes.Add("onclick", "return confirm('You are about to delete some POs - Are you sure?');")

            Me.btnPushToIRMA.Attributes.Add("onclick", "return confirm('You are about to push the selected POs to IRMA - Are you sure?');")


            If Session("UserID") Is Nothing Then
                lblPOsReadyToPush.Text = "There isn't anyone logged in"
                ddlUsers.Visible = False
            Else
                lblPOsReadyToPush.Text = "POs that are ready to be pushed to IRMA for " + Session.Item("UserName").ToString
                Me.ddlUsers.SelectedValue = Session("UserID")
                Me.lblUserID.Text = Session("UserID")
                Me.ddlUsers.Visible = True
            End If

            Dim ds As New DataSet
            ds = GetPOsReadyToPushByUserByVal(Session("UserID"))

            If ds.Tables(0).Rows.Count > 0 Then
                uwgPOsReadyToPush.Visible = True
                trExport.Visible = True
                trPushDelete.Visible = True
            Else
                uwgPOsReadyToPush.Visible = False
                trExport.Visible = False
                trPushDelete.Visible = False

                ErrorLabel.Text = "No purchase orders are ready to push to IRMA."
            End If
        End If
    End Sub

    Protected Sub dsPOsReadyToPush_Selecting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles dsPOsReadyToPush.Selecting

        'e.InputParameters("UserID") = Session("UserID")
        e.InputParameters("UserID") = Session("CurrentUser")

        If Session("UserID") Is Nothing Then
            lblPOsReadyToPush.Text = "There isn't anyone logged in"
        Else
            lblPOsReadyToPush.Text = "POs that are ready to be pushed to IRMA for " + Session.Item("UserName").ToString
        End If

        If ddlPaging.SelectedValue.ToString() = "all" Then
            uwgPOsReadyToPush.DisplayLayout.Pager.AllowPaging = False
        Else
            uwgPOsReadyToPush.DisplayLayout.Pager.AllowPaging = True
            uwgPOsReadyToPush.DisplayLayout.Pager.PageSize = CInt(ddlPaging.SelectedValue)
        End If

    End Sub

    Private Sub uwgPOsReadyToPush_DblClick(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.ClickEventArgs) Handles uwgPOsReadyToPush.DblClick

        Dim rowEnum As UltraGridRowsEnumerator = uwgPOsReadyToPush.Bands(0).GetRowsEnumerator()
        Dim dr As New UltraGridRow

        If e.Column.Header.Key = "PushPO" Then
            Try
                rowEnum.Reset()
                While rowEnum.MoveNext
                    dr = CType(rowEnum.Current(), UltraGridRow)
                    If CType(Session("POPushAll"), Boolean) = True Then
                        dr.Cells(0).Value = False
                    Else
                        dr.Cells(0).Value = True
                    End If
                End While
                If CType(Session("POPushAll"), Boolean) = True Then
                    Session("POPushAll") = False
                Else
                    Session("POPushAll") = True
                End If

                SaveCheckBoxState()

            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try

        ElseIf e.Column.Header.Key = "DelPO" Then
            Try
                rowEnum.Reset()
                While rowEnum.MoveNext
                    dr = CType(rowEnum.Current(), UltraGridRow)
                    If CType(Session("DeleteAll"), Boolean) = True Then
                        dr.Cells(13).Value = False
                    Else
                        dr.Cells(13).Value = True
                    End If
                End While
                If CType(Session("DeleteAll"), Boolean) = True Then
                    Session("DeleteAll") = False
                Else
                    Session("DeleteAll") = True
                End If

                SaveCheckBoxState()

            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try
        End If

    End Sub

    Protected Sub uwgPOsReadyToPush_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles uwgPOsReadyToPush.InitializeLayout
        e.Layout.Pager.AllowPaging = True
        e.Layout.Pager.PageSize = 25
        e.Layout.Pager.PagerStyle.Font.Size = 16

        If e.Layout.Bands(0).Columns.IndexOf("PushPO") = -1 Then
            Dim colPush As New UltraGridColumn(True)
            colPush.Key = "PushPO"
            colPush.Header.Caption = "Push PO"
            colPush.Type = ColumnType.CheckBox
            colPush.Header.ClickAction = HeaderClickAction.Select
            e.Layout.Bands(0).Columns.Insert(0, colPush)
        End If

        If e.Layout.Bands(0).Columns.IndexOf("DelPO") = -1 Then
            Dim colDelete As New UltraGridColumn(True)
            colDelete.Key = "DelPO"
            colDelete.Header.Caption = "Delete PO"
            colDelete.Type = ColumnType.CheckBox
            colDelete.Header.ClickAction = HeaderClickAction.Select
            e.Layout.Bands(0).Columns.Insert(13, colDelete)
        End If

        With e.Layout.Bands(0).Columns.FromKey("UploadSessionHistoryID")
            .Header.Caption = "Session ID"
            .Width = 75
            .Key = "UploadSessionHistoryID"
            .Hidden = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("POHeaderID")
            .Hidden = True
            .Key = "POHeaderID"
        End With
        With e.Layout.Bands(0).Columns.FromKey("RegionName")
            .Header.Caption = "Region"
            .Width = 75
            .Hidden = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("PONumber")
            .Header.Caption = "PO Number"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("BusinessUnit")
            .Header.Caption = "Business Unit"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("StoreAbbr")
            .Header.Caption = "Store"
            .Width = 75
        End With
        With e.Layout.Bands(0).Columns.FromKey("Subteam")
            .Header.Caption = "Subteam"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("VendorName")
            .Header.Caption = "Vendor"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("VendorPSNumber")
            .Header.Caption = "Vendor PS #"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("OrderItemCount")
            .Header.Caption = "Item Count"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("TotalPOCost")
            .Header.Caption = "Total PO Cost"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("ExpectedDate")
            .Header.Caption = "Expected Date"
            If Session("regionName") = "United Kingdom" Then
                .Format = Format("dd/MM/yyyy")
            Else
                .Format = Format("MM/dd/yyyy")
            End If

            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("AutoPushDate")
            .Header.Caption = "Auto Push Date"

            If Session("regionName") = "United Kingdom" Then
                .Format = Format("dd/MM/yyyy")
            Else
                .Format = Format("MM/dd/yyyy")
            End If

            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("Notes")
            .Header.Caption = "Notes"
            .Width = 300
            .Hidden = False
        End With

        uwgPOsReadyToPush.DataKeyField = "POHeaderID"
    End Sub

    Protected Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Select Case ddlExportFormat.SelectedValue.ToString()
            Case "PDF"
                udeExportReadyToCreate.Format = FileFormat.PDF
                udeExportReadyToCreate.DownloadName = "POsToPush.pdf"
                udeExportReadyToCreate.Export(uwgPOsReadyToPush)

            Case "Text"
                udeExportReadyToCreate.Format = FileFormat.PlainText
                udeExportReadyToCreate.DownloadName = "POsToPush.txt"
                udeExportReadyToCreate.Export(uwgPOsReadyToPush)

            Case "Excel"
                Dim currentPaging As String
                currentPaging = ddlPaging.SelectedValue.ToString()
                ddlPaging.SelectedValue = "all"
                uwgPOsReadyToPush.DataBind()
                udeExcelExportReadyToCreate.DownloadName = "POsToPush.xls"
                udeExcelExportReadyToCreate.Export(uwgPOsReadyToPush)
                ddlPaging.SelectedValue = currentPaging
                uwgPOsReadyToPush.DataBind()
        End Select
    End Sub

    Protected Sub btnDeletePOs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeletePOs.Click

        Try
            Dim rowEnum As UltraGridRowsEnumerator = uwgPOsReadyToPush.Bands(0).GetRowsEnumerator()
            Dim dal As New DAOValidatedPOs
            Dim dr As New UltraGridRow

            rowEnum.Reset()
            While rowEnum.MoveNext

                dr = CType(rowEnum.Current(), UltraGridRow)

                If CType(dr.Cells(13).Value, Boolean) = True Then
                    dal.DeletePOs(CType(Session("UserID"), Integer), CInt(dr.DataKey))
                    dr.Cells(13).AllowEditing = AllowEditing.No
                    dr.Cells(0).AllowEditing = AllowEditing.No
                    ErrorLabel.Visible = True
                    ErrorLabel.ForeColor = Drawing.Color.Green
                    ErrorLabel.Text = "Selected POs Deleted"
                End If
            End While

        Catch ex As Exception
            ErrorLabel.Visible = True
            ErrorLabel.ForeColor = Drawing.Color.Red
            ErrorLabel.Text = "Error Deleting Selected POs"
            Debug.WriteLine(ex.Message)
            Debug.WriteLine(ex.StackTrace)
        End Try
    End Sub

    Protected Sub btnPushToIRMA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPushToIRMA.Click

        Try
            Dim rowEnum As UltraGridRowsEnumerator = uwgPOsReadyToPush.Bands(0).GetRowsEnumerator()
            Dim dal As New DAOValidatedPOs
            Dim dr As New UltraGridRow

            rowEnum.Reset()
            While rowEnum.MoveNext

                dr = CType(rowEnum.Current(), UltraGridRow)

                If CType(dr.Cells(0).Value, Boolean) = True Then

                    dal.InsertPushQueue(CInt(dr.DataKey), Session("UserID"))

                    dr.Cells(0).AllowEditing = AllowEditing.No
                    dr.Cells(13).AllowEditing = AllowEditing.No
                    ErrorLabel.Visible = True
                    ErrorLabel.ForeColor = Drawing.Color.Green
                    ErrorLabel.Text = "Selected POs Pushed To IRMA"
                End If
            End While

        Catch ex As Exception
            ErrorLabel.Visible = True
            ErrorLabel.ForeColor = Drawing.Color.Red
            ErrorLabel.Text = "Error Pushing Selected POs To IRMA"
            Debug.WriteLine(ex.Message)
            Debug.WriteLine(ex.StackTrace)
        End Try

    End Sub

    Protected Sub uwgPOsReadyToPush_InitializeRow(sender As Object, e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles uwgPOsReadyToPush.InitializeRow

        Try
            If e.Row Is Nothing Then Exit Sub

            Dim row As UltraGridRow = e.Row

            If Not Me.ViewState("PushPOs") Is Nothing And row.Index < Me.PushPOs.Count Then
                row.Cells(0).Value = Me.PushPOs(row.Index)
            End If

            If Not Me.ViewState("DeletePOs") Is Nothing And row.Index < Me.DeletePOs.Count Then
                row.Cells(13).Value = Me.DeletePOs(row.Index)
            End If

            If lblUserID.Text <> Session("UserID") Then
                uwgPOsReadyToPush.Columns("13").AllowUpdate = AllowUpdate.No
                btnDeletePOs.Enabled = False
            Else
                uwgPOsReadyToPush.Columns("13").AllowUpdate = AllowUpdate.Yes
                btnDeletePOs.Enabled = True
            End If

        Catch ex As Exception
            ErrorLabel.Visible = True
            ErrorLabel.ForeColor = Drawing.Color.Red
            ErrorLabel.Text = "Error in loading grid...[Row=" + IIf(IsNothing(e.Row.Index), "Null", e.Row.Index.ToString) + "] [Pushed=" + PushPOs.Count.ToString + "] [Deleted=" + DeletePOs.Count.ToString + "]"
            Debug.WriteLine(ex.Message)
            Debug.WriteLine(ex.StackTrace)

        End Try
    End Sub

    Protected Sub uwgPOsReadyToPush_UpdateCell(sender As Object, e As Infragistics.WebUI.UltraWebGrid.CellEventArgs) Handles uwgPOsReadyToPush.UpdateCell
        SaveCheckBoxState()
    End Sub

    Private Sub ddlUsers_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlUsers.SelectedIndexChanged
        Me.lblUserID.Text = Me.ddlUsers.SelectedValue
        Me.lblPOsReadyToPush.Text = "POs that are ready to be pushed to IRMA for " + Me.ddlUsers.SelectedItem.Text

        Session("CurrentUser") = lblUserID.Text

        If (Session("CurrentUser") <> Session("UserID")) Then
            SaveCheckBoxState()
        End If

        Dim ds As New DataSet
        ds = GetPOsReadyToPushByUserByVal(Me.lblUserID.Text)

        uwgPOsReadyToPush.DataSource = ds

        If ds.Tables(0).Rows.Count > 0 Then
            uwgPOsReadyToPush.Visible = True
            trExport.Visible = True
            trPushDelete.Visible = True
        Else
            uwgPOsReadyToPush.Visible = False
            trExport.Visible = False
            trPushDelete.Visible = False

            ErrorLabel.Text = "No purchase orders are ready to push to IRMA."
        End If
    End Sub
End Class
