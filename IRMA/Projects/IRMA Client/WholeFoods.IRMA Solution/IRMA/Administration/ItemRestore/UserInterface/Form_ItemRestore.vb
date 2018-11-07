Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Public Class Form_ItemRestore

    ' ----------------------------------------------------------------------------
    ' Revision History
    ' ----------------------------------------------------------------------------
    ' 8/18/10             Tom Lux               TFS 13138        Updated Form_ItemRestore_RestoreItem method to include validation, BO updates, and flow improvements.

#Region "Private Members"

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Form Events"

    Private Sub Form_ItemRestore_RestoreItem(ByVal sender As Object, ByVal e As System.EventArgs) Handles RestoreButton.Click
        Dim identifier As String = Me.Identifier.Text.Trim()

        ' Make sure the business rules are satisfied before attempting restore.
        Try
            ItemRestoreBO.validateRestore(identifier)
        Catch ex As Exception
            logger.Error(ex.Message)
            ErrorDialog.HandleError("Restore Deleted Item", ex, ErrorDialog.NotificationTypes.DialogOnly, "")
            Me.Identifier.SelectAll()
            Me.Identifier.Focus()
            Exit Sub
        End Try

        ' Attempt restore.
        Try
            If ItemRestoreBO.Restore(identifier) Then
                Dim msg As String = "Item '" & identifier & "' has been restored successfully."
                MessageBox.Show(msg, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                logger.Info(msg)
                Me.Identifier.Clear()
                Me.Identifier.Focus()
            Else
                MessageBox.Show("No items were restored during the restore attempt." & vbCrLf & "Check the item information and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                logger.Error("ItemRestoreBO.Restore() for item '" & identifier & "' returned FALSE (no rows affected by update query).")
                Me.Identifier.SelectAll()
                Me.Identifier.Focus()
            End If
        Catch ex As Exception
            logger.Error(ex.Message)
            ErrorDialog.HandleError("Restore Deleted Item", ex, ErrorDialog.NotificationTypes.DialogOnly, "")
            Exit Sub
        End Try

        glItemID = UploadRowDAO.Instance.GetItemKeyByIdentifier(identifier)

        Using itemInfoForm As New frmItem
            itemInfoForm.ShowDialog()
        End Using

    End Sub

    Private Sub Form_ItemRestore_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Identifier.Focus()
    End Sub

#End Region

End Class