Imports WholeFoods.Utility.DataAccess

Public Class Form_AddMenu
    Dim m_sMenuName As String = ""
    Dim m_bIsMenuVisible As Boolean = False

    Public Property MenuName() As String
        Get
            MenuName = m_sMenuName
        End Get
        Set(ByVal value As String)
            m_sMenuName = value
        End Set
    End Property

    Public Property IsMenuVisible() As Boolean
        Get
            IsMenuVisible = m_bIsMenuVisible
        End Get
        Set(ByVal value As Boolean)
            m_bIsMenuVisible = value
        End Set
    End Property

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If Trim(Textbox_MenuName.Text) = "" Then
            MsgBox("Menu Name cannot be blank.", MsgBoxStyle.Critical, "Add Menu")
            Exit Sub
        End If

        If Not MenuExists() Then
            Me.MenuName = Trim(Textbox_MenuName.Text)
            Me.IsMenuVisible = CheckBox_IsVisible.Checked
            Me.Close()
        Else
            MsgBox("Menu """ & Textbox_MenuName.Text & """ cannot be added because it already exists.", MsgBoxStyle.Critical, "Add Menu")
        End If
    End Sub

    Private Function MenuExists() As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim sResult As String = ""

        sResult = CType(factory.ExecuteScalar("SELECT MenuAccessID FROM MenuAccess WHERE MenuName = '" & Textbox_MenuName.Text & "'"), String)
        Return CBool(sResult <> "")
    End Function
End Class