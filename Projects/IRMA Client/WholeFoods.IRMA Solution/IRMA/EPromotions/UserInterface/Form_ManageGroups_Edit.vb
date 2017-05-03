Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Public Class Form_ManageGroups_Edit

    Public Enum EditGroup_FormMode
        [New]
        Edit
    End Enum
    Private _ItemGroup As ItemGroupBO = Nothing
    Private _FormMode As EditGroup_FormMode
    Private _IsLoading As Boolean = True
    Private _GroupNameChanged As Boolean = False
    Private _DeletedGroupIds As String = String.Empty

    Private Sub Form_ManageGroups_Edit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If _FormMode = EditGroup_FormMode.Edit Then
            Me.Text = "Edit Group"
            'load group info.
            TextBox_GroupName.Text = _ItemGroup.GroupName

            If _ItemGroup.GroupLogic = ItemGroup_GroupLogic.And Then
                RadioButton_AND.Checked = True
            Else
                RadioButton_OR.Checked = True
            End If
        Else
            Me.Text = "Create New Group"
        End If
        _IsLoading = False
    End Sub

    Public Function CreateNewGroup(ByRef DeletedGroupIds As String) As ItemGroupBO
        _FormMode = EditGroup_FormMode.[New]
        _DeletedGroupIds = DeletedGroupIds
        Me.ShowDialog(MyBase.ParentForm)
        Return _ItemGroup
    End Function

    Public Function EditGroup(ByVal ItemGroup As ItemGroupBO) As ItemGroupBO
        _FormMode = EditGroup_FormMode.Edit
        _ItemGroup = ItemGroup
        Me.ShowDialog(MyBase.ParentForm)
        Return _ItemGroup
    End Function

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        _ItemGroup = Nothing
        Me.Close()
    End Sub

    Private Sub TextBox_GroupName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_GroupName.TextChanged
        If Not _IsLoading Then
            If _FormMode = EditGroup_FormMode.Edit Then
                _ItemGroup.MarkDirty()
                _GroupNameChanged = True
            End If
        End If
    End Sub

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        Dim GroupDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO
        Dim ItemGroup As ItemGroupBO = New ItemGroupBO
        Dim DAO As ItemGroupDAO = New ItemGroupDAO
        If _FormMode = EditGroup_FormMode.[New] Then
            _ItemGroup = New ItemGroupBO
        End If

        _ItemGroup.GroupName = TextBox_GroupName.Text

        If RadioButton_AND.Checked Then
            _ItemGroup.GroupLogic = ItemGroup_GroupLogic.And
        Else
            _ItemGroup.GroupLogic = ItemGroup_GroupLogic.Or
        End If

        If _FormMode = EditGroup_FormMode.[New] Then

            If ItemGroup.ValidateItemGroup(_ItemGroup, _DeletedGroupIds) Then
                _ItemGroup.GroupID = GroupDAO.CreateNewGroup(_ItemGroup.GroupName, _ItemGroup.GroupLogic)
                Me.Close()
            End If

        Else

            If _GroupNameChanged Then
                If ItemGroup.ValidateItemGroup(_ItemGroup, _DeletedGroupIds) Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If


    End Sub
End Class