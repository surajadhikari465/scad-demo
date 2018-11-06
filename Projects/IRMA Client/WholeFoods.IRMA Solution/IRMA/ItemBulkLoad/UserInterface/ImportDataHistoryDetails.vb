Option Strict Off
Option Explicit On
Imports Infragistics.Win
Imports WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
Imports WholeFoods.IRMA.ItemBulkLoad.DataAccess

Public Class ImportDataHistoryDetails
    Private mdt As DataTable
    Private mdv As DataView

    Private currentHeaderBO As ItemUploadHeaderBO

    Public Sub DisplayHeaderInfo(ByVal headerBO As ItemUploadHeaderBO)
        currentHeaderBO = headerBO
        HeaderInfoLabel.Text = currentHeaderBO.InfoToString()
    End Sub

    Private Sub ImportDataHistoryDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False

        CenterForm(Me)
        LoadDetails()

        Me.Enabled = True
        Me.Cursor = oldCursor
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    ' Sub routine to setup the data table to be used in the grid
    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New System.Data.DataTable(ResourcesItemBulkLoad.GetString("Screen_Title"))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("SubTeamName_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("Identifier_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("POSDescription_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("ItemDescription_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("TaxClass_Column"), GetType(Integer)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("FoodStamps_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("Discountable_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("Discontinued_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("NationalClass_Column"), GetType(String)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("ItemUploadDetailID_Column"), GetType(Integer)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("SubTeamNo_Column"), GetType(Integer)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("ItemIdentifierValid_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("SubTeamAllowed_Column"), GetType(Boolean)))
        mdt.Columns.Add(New DataColumn(ResourcesItemBulkLoad.GetString("Item_Key"), GetType(Integer)))

    End Sub

    Private Sub LoadDetails()
        Dim row As DataRow
        Dim obj As New ItemBulkLoadDisplayBO
        Dim bulkDAO As New ItemBulkLoadDisplayDAO

        obj.ItemUploadHeaderID = currentHeaderBO.ItemUploadHeaderID
        obj.ValidItems = bulkDAO.GetValidItems(obj.ItemUploadHeaderID)

        If Not obj.ValidItems Is Nothing Then

            '' Create the data table
            Call SetupDataTable()

            For Each oneInst As ItemUploadDetailBO In obj.ValidItems
                row = mdt.NewRow

                ' Populate the columns with the respective values
                If oneInst.SubTeamName Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("SubTeamName_Column")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("SubTeamName_Column")) = oneInst.SubTeamName
                End If

                If oneInst.ItemIdentifier Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("Identifier_Column")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("Identifier_Column")) = oneInst.ItemIdentifier
                End If

                If Trim(oneInst.PosDescription) Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("POSDescription_Column")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("POSDescription_Column")) = Trim(oneInst.PosDescription)
                End If

                If Trim(oneInst.Description) Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("ItemDescription_Column")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("ItemDescription_Column")) = Trim(oneInst.Description)
                End If

                If oneInst.TaxClassID Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("TaxClass_Column")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("TaxClass_Column")) = oneInst.TaxClassID
                End If

                If oneInst.FoodStamps Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("FoodStamps_Column")) = DBNull.Value
                ElseIf Trim(oneInst.FoodStamps) = "0" Or Trim(oneInst.FoodStamps) = "1" Then
                    row(ResourcesItemBulkLoad.GetString("FoodStamps_Column")) = CBool(oneInst.FoodStamps)
                End If

                If oneInst.RestrictedHours Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column")) = DBNull.Value
                ElseIf Trim(oneInst.RestrictedHours) = "0" Or Trim(oneInst.RestrictedHours) = "1" Then
                    row(ResourcesItemBulkLoad.GetString("RestrictedHrs_Column")) = CBool(oneInst.RestrictedHours)
                End If

                If oneInst.EmployeeDiscountable Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("Discountable_Column")) = DBNull.Value
                ElseIf Trim(oneInst.EmployeeDiscountable) = "0" Or Trim(oneInst.EmployeeDiscountable) = "1" Then
                    row(ResourcesItemBulkLoad.GetString("Discountable_Column")) = CBool(oneInst.EmployeeDiscountable)
                End If

                If oneInst.Discontinued Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("Discontinued_Column")) = DBNull.Value
                ElseIf Trim(oneInst.Discontinued) = "0" Or Trim(oneInst.Discontinued) = "1" Then
                    row(ResourcesItemBulkLoad.GetString("Discontinued_Column")) = CBool(oneInst.Discontinued)
                End If

                If oneInst.NationalClassID Is Nothing Then
                    row(ResourcesItemBulkLoad.GetString("NationalClass_Column")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("NationalClass_Column")) = oneInst.NationalClassID
                End If

                row(ResourcesItemBulkLoad.GetString("ItemUploadDetailID_Column")) = oneInst.ItemUploadDetail_ID

                If oneInst.SubTeamNo = Nothing Then
                    row(ResourcesItemBulkLoad.GetString("SubTeamNo_Column")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("SubTeamNo_Column")) = oneInst.SubTeamNo
                End If

                row(ResourcesItemBulkLoad.GetString("ItemIdentifierValid_Column")) = oneInst.ItemIdentifierValid

                If oneInst.ItemIdentifierValid = 0 Then
                    row.RowError = "1"
                End If

                row(ResourcesItemBulkLoad.GetString("SubTeamAllowed_Column")) = oneInst.SubTeamAllowed

                If oneInst.ItemIdentifierValid = 1 And oneInst.SubTeamAllowed = 0 Then
                    ' We've already caught the invalid identifier (which will have an invalid subteam)
                    '  So we use this to only report disallowed subteam
                    row.RowError = "2"
                End If

                If oneInst.ItemIdentifierValid = 1 And oneInst.SubTeamAllowed = 1 And oneInst.Uploaded = 0 Then
                    ' We've already caught the invalid identifier and disallowed subteam
                    '  So we use this to only report more 'trivial' errors
                    row.RowError = "3"
                End If

                If oneInst.Item_Key = Nothing Then
                    row(ResourcesItemBulkLoad.GetString("Item_Key")) = DBNull.Value
                Else
                    row(ResourcesItemBulkLoad.GetString("Item_Key")) = oneInst.Item_Key
                End If

                mdt.Rows.Add(row)
            Next

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)

            'mdv.Sort = "Store_Name"
            ugrdList.DataSource = mdv

            ' Set the settings on the grid to support error display
            ugrdList.DisplayLayout.Override.SupportDataErrorInfo = UltraWinGrid.SupportDataErrorInfo.RowsAndCells
            ugrdList.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True

            ' Based on validation, change the back colors and the tool tip text
            ValidateSpreadsheetData()

            'This may or may not be required.
            If mdt.Rows.Count > 0 Then
                'Set the first item to selected.
                ugrdList.Rows(0).Selected = True
            Else
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            End If
        Else
            MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End If

    End Sub
    Private Sub ValidateSpreadsheetData()
        Dim i As Integer
        Dim k As Integer
        Dim errorDisplay As String = String.Empty


        k = 0
        For i = 0 To mdt.Rows.Count - 1
            If mdt.Rows(i).HasErrors Then
                Dim errorText As String
                Dim errors() As String
                Dim s As String

                errorText = mdt.Rows(i).RowError  'set the string from the input
                errors = errorText.Split(",")
                For Each s In errors
                    If s = "1" Then
                        ugrdList.Rows(i).Appearance.BackColor = Color.Red
                        errorDisplay = "Identifier " & Trim(ugrdList.Rows(i).Cells(1).Value.ToString()) & " does not exist!"
                    ElseIf s = "2" Then
                        ugrdList.Rows(i).Appearance.BackColor = Color.Orange
                        errorDisplay = ResourcesItemBulkLoad.GetString("SubTeam_NotAllowed")
                    ElseIf s = "3" Then
                        ugrdList.Rows(i).Appearance.BackColor = Color.Yellow
                        errorDisplay = ResourcesItemBulkLoad.GetString("ItemNotUploaded")
                    End If

                Next s
                mdt.Rows(i).RowError = errorDisplay
                ugrdList.Rows(i).Activation = UltraWinGrid.Activation.NoEdit
            Else
                k = k + 1
            End If
        Next
    End Sub
    Private Sub ugrdList_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrdList.InitializeLayout
        ugrdList.DisplayLayout.AutoFitStyle = UltraWinGrid.AutoFitStyle.ResizeAllColumns
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub cmdItemEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdItemEdit.Click
        If ugrdList.Selected.Rows.Count = 1 Then

            Me.Enabled = False
            If Not (ugrdList.Selected.Rows(0).Cells("Item_Key").Value Is DBNull.Value) Then
                glItemID = ugrdList.Selected.Rows(0).Cells("Item_Key").Value

                frmItem.ShowDialog()
                frmItem.Close()

            Else
                MsgBox("You must select a valid item.", MsgBoxStyle.Critical, Me.Text)
            End If
            Me.Enabled = True
        Else
            MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Critical, Me.Text)
        End If

    End Sub
End Class