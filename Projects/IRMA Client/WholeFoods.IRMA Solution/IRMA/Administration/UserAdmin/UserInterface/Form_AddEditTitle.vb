Imports WholeFoods.Utility.DataAccess

Public Class Form_AddEditTitle
    Dim m_iTitleId As Integer = 0
    Dim m_sTitleCode As String = ""
    Dim m_sTitleDesc As String = ""
    Dim m_bIsEdit As Boolean = False

    Public Property IsEdit() As Boolean
        Get
            IsEdit = m_bIsEdit
        End Get
        Set(ByVal value As Boolean)
            m_bIsEdit = value
        End Set
    End Property

    Public Property TitleId() As Integer
        Get
            TitleId = m_iTitleId
        End Get
        Set(ByVal value As Integer)
            m_iTitleId = value
        End Set
    End Property

    Public Property TitleDesc() As String
        Get
            TitleDesc = m_sTitleDesc
        End Get
        Set(ByVal value As String)
            m_sTitleDesc = value
        End Set
    End Property
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If Trim(txtTitleDescription.Text) = "" Then
            MsgBox("Title Description cannot be blank.", MsgBoxStyle.Critical, "Add Title")
            Exit Sub
        End If

        If Not TitleExists() Then
            Me.TitleDesc = Trim(txtTitleDescription.Text)
            Me.Close()
        Else
            MsgBox("Title """ & txtTitleDescription.Text & """ cannot be added because it already exists.", MsgBoxStyle.Critical, "Add Title")
        End If
    End Sub

    Private Sub Form_AddEditTitle_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsEdit Then
            txtTitleDescription.Text = Me.TitleDesc.ToString
        End If
    End Sub

    Private Function TitleExists() As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim sResult As String = ""

        sResult = CType(factory.ExecuteScalar("SELECT Title_ID FROM Title WHERE Title_Desc = '" & Replace(txtTitleDescription.Text, "'", "''") & "'"), String)
        Return CBool(sResult <> "")
    End Function
End Class