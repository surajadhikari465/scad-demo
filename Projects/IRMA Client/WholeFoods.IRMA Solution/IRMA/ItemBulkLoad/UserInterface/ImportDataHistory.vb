Option Strict Off
Option Explicit On

Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
Imports WholeFoods.IRMA.ItemBulkLoad.DataAccess


Public Class ImportDataHistory
    Private mdt As DataTable
    Private mdv As DataView

    Private itemUploadSearchBO As ItemUploadSearchBO

    Private Sub ImportDataHistory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CenterForm(Me)
        BindImportTypeComboBox()
        BindUsersComboBox()

        itemUploadSearchBO = New ItemUploadSearchBO()
    End Sub

    Private Sub BindImportTypeComboBox()

        Dim itemBulkLoadDAO As New ItemMaintenanceBulkLoadDAO

        ImportTypeComboBox.DataSource = itemBulkLoadDAO.GetImportTypeList()
        ImportTypeComboBox.DisplayMember = "Description"
        ImportTypeComboBox.ValueMember = "ItemUploadTypeID"
        ' set to Item and lock down
        ImportTypeComboBox.SelectedIndex = 0
        ImportTypeComboBox.Enabled = False
    End Sub


    Private Sub BindUsersComboBox()

        Dim itemBulkLoadDAO As New ItemMaintenanceBulkLoadDAO

        UserComboBox.DataSource = itemBulkLoadDAO.GetItemAdminUserList()
        UserComboBox.DisplayMember = "UserName"
        UserComboBox.ValueMember = "UserID"
        UserComboBox.SelectedIndex = -1

    End Sub


    Public Function GetImportTypes() As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataReader("GetItemUploadTypes")
    End Function


    Public Function GetItemAdminUsers() As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataReader("GetItemAdminUsers")
    End Function
    Private Sub ItemLoadDetailButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemLoadDetailButton.Click
        If ugrdList.Selected.Rows.Count <> 1 Then
            MsgBox(ResourcesIRMA.GetString("SelectSingleRow"), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        Else
            Dim headerBO As New ItemUploadHeaderBO()
            headerBO.ItemUploadHeaderID = ugrdList.Selected.Rows(0).Cells("ItemUploadHeaderID").Value
            If ugrdList.Selected.Rows(0).Cells("ItemsProcessedCount").Value Is DBNull.Value Then
                headerBO.ItemProcessedCount = 0
            Else
                headerBO.ItemProcessedCount = ugrdList.Selected.Rows(0).Cells("ItemsProcessedCount").Value
            End If
            If ugrdList.Selected.Rows(0).Cells("ItemsLoadedCount").Value Is DBNull.Value Then
                headerBO.ItemLoadedCount = 0
            Else
                headerBO.ItemLoadedCount = ugrdList.Selected.Rows(0).Cells("ItemsLoadedCount").Value
            End If
            If ugrdList.Selected.Rows(0).Cells("ErrorsCount").Value Is DBNull.Value Then
                headerBO.ErrorsCount = 0
            Else
                headerBO.ErrorsCount = ugrdList.Selected.Rows(0).Cells("ErrorsCount").Value
            End If
            headerBO.UploadDate = ugrdList.Selected.Rows(0).Cells("UploadedDateTime").Value
            ImportDataHistoryDetails.DisplayHeaderInfo(headerBO)
            ImportDataHistoryDetails.ShowDialog()
        End If

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()

    End Sub



    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim oldCursor As Cursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        If Not ItemLoadHeaderIDUNE.Value Is DBNull.Value Then
            itemUploadSearchBO.ItemUploadHeaderID = CInt(ItemLoadHeaderIDUNE.Value)
        End If
        itemUploadSearchBO.ItemUploadTypeID = CType(ImportTypeComboBox.SelectedItem, ImportTypeBO).ItemUploadTypeID
        If UserComboBox.SelectedIndex >= 0 Then
            itemUploadSearchBO.UserID = CType(UserComboBox.SelectedItem, ItemAdminUserBO).UserID
        Else
            itemUploadSearchBO.UserID = 0
        End If
        itemUploadSearchBO.CreateDate = CDate(CreateDateUDTE.Value)

        mdt = itemUploadSearchBO.Search()

        'Setup a column that you would like to sort on initially.
        mdt.AcceptChanges()
        mdv = New System.Data.DataView(mdt)

        'mdv.Sort = "Store_Name"
        ugrdList.DataSource = mdv

        Me.Cursor = oldCursor

        'This may or may not be required.
        If mdt.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdList.Rows(0).Selected = True
        Else
            MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub UserComboBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles UserComboBox.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)
        If KeyAscii = 8 Then
            UserComboBox.SelectedIndex = -1
        End If
        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub CreateDateUDTE_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateDateUDTE.ValueChanged
        If CreateDateUDTE.Value = Nothing Then CreateDateUDTE.Value = Now.Date

    End Sub
End Class