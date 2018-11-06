Option Strict Off
Option Explicit On
Friend Class ItemOnHandPrompt
    Inherits System.Windows.Forms.Form

    Dim gsItemDescription As String
    Dim giItemKey As Integer

    Private Sub CancelButton_Renamed_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles CancelButton_Renamed.Click
        Me.Close()
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        ShowItemLevel()
    End Sub

    Private Sub GetItemInfo()
        Dim rs As DAO.Recordset = Nothing

        rs = SQLOpenRecordSet("SELECT I.Item_Key,I.Item_Description " & _
                              "FROM Item I " & _
                              "JOIN ItemIdentifier II ON II.Item_Key = I.Item_Key " & _
                              "WHERE II.Identifier = '" & txtIdentifier.Text & "' AND I.Deleted_Item = 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        If rs.RecordCount = 0 Then
            giItemKey = -1
            Exit Sub
        End If

        giItemKey = rs.Fields(0).Value
        gsItemDescription = rs.Fields(1).Value & ""

        rs.Close()
        rs = Nothing
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        Dim itemSearch As New frmItemSearch

        itemSearch.ShowDialog()
        itemSearch.Close()

        If glItemID <> 0 Then
            txtIdentifier.Text = itemSearch.ItemIdentifier.ToString
            ShowItemLevel()
        End If

        itemSearch.Dispose()
    End Sub

    Private Sub ShowItemLevel()
        GetItemInfo()

        glItemID = giItemKey

        If glItemID = -1 Then
            MsgBox("Identifier " & txtIdentifier.Text & " does not exist.", MsgBoxStyle.Critical, "Item On Hand Prompt")
            txtIdentifier.SelectionStart = 0
            txtIdentifier.SelectionLength = txtIdentifier.Text.Length
            txtIdentifier.Focus()
        Else
            Dim fItemOnHand As New frmItemOnHand
            fItemOnHand.Text = ResourcesItemHosting.GetString("CurrentUnits") & " " & gsItemDescription
            fItemOnHand.ShowDialog()
            fItemOnHand.Close()
            fItemOnHand.Dispose()
        End If
    End Sub
End Class