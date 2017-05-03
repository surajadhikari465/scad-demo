Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports System.ComponentModel
Imports Infragistics.Win.UltraWinGrid

Public Class Form_ManageGroups
    Private _groupList As BindingList(Of ItemGroupBO)
    Private _CurrentOffer As PromotionOfferBO
    Private _CurrentOfferReadOnly As Boolean
    Private _flagReadOnly As Boolean
    Private Sub Form_ManageGroups_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PopulateGroupInfo()
    End Sub

    Private Sub PopulateGroupInfo()
        Dim dao As PromotionOfferDAO = New PromotionOfferDAO
        _groupList = dao.GetPromotionalGroupList(-1)
        ItemGroupBOBindingSource.DataSource = _groupList
        BindGrid()
        SetGridFilters()
    End Sub

    Sub New(ByVal CurrentOffer As PromotionOfferBO, ByVal IsReadOnly As Boolean)
        InitializeComponent()
        _CurrentOffer = CurrentOffer
        _CurrentOfferReadOnly = IsReadOnly
    End Sub
    Private Sub BindGrid()
        UltraGrid_Groups.DataSource = ItemGroupBOBindingSource
    End Sub

    Private Sub SetGridFilters()
        ' Make sure deleted Groups are not displayed in the grid
        With UltraGrid_Groups.DisplayLayout.Bands(0)
            .ColumnFilters("isDeleted").FilterConditions.Clear()
            .Columns("isDeleted").Hidden = True
            .Columns("isNew").Hidden = True
            .Columns("isDirty").Hidden = True
            .Columns("Loading").Hidden = True
            .Columns("GroupId").Hidden = True
            .Columns("CreateDate").Hidden = True
            .Columns("ModifiedDate").Hidden = True
            .Columns("UserId").Hidden = True
            .Columns("EntityState").Hidden = True
            .Columns("isDeleted").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
            .ColumnFilters("isDeleted").FilterConditions.Add(FilterComparisionOperator.NotEquals, True)
        End With
    End Sub

    Private Sub UpdateButtonState()
        Dim GroupDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO
        Dim IsEditing As Boolean
        Dim EditedBy As String
        Label_Message.Visible = False
        'Button_Edit.Enabled = (CType(ItemGroupBOBindingSource.Current, ItemGroupBO).PendingPromotionCount = 0)
        Button_Remove.Enabled = (CType(ItemGroupBOBindingSource.Current, ItemGroupBO).PendingPromotionCount = 0)

        If Button_Remove.Enabled Then
            ' if there are no pending promotions, make sure no on else is editing this Group
            EditedBy = GroupDAO.GetGroupEditStatus(CType(ItemGroupBOBindingSource.Current, ItemGroupBO).GroupID)
            If EditedBy <> "" Then IsEditing = True Else IsEditing = False
            Button_Remove.Enabled = Not IsEditing
            Label_Message.Text = String.Format("* This group is currently being edited by {0} and is set as read-only.", EditedBy)
            Label_Message.Visible = IsEditing
            ' if the user has the correct permissions, enable the unlock button.
            Button_Unlock.Enabled = (IsEditing And (gbLockAdministrator Or gbSuperUser))
        End If

        If _CurrentOfferReadOnly And Button_Remove.Enabled Then
            If GroupDAO.IsGroupInCurrentPromotion(CType(ItemGroupBOBindingSource.Current, ItemGroupBO).GroupID, _CurrentOffer.PromotionOfferID) Then
                Button_Remove.Enabled = False
                _flagReadOnly = True
            Else
                Button_Remove.Enabled = True
                _flagReadOnly = False
            End If

        End If

    End Sub

    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Dim frm As Form_ManageGroups_Edit = New Form_ManageGroups_Edit
        Dim ItemGroup As ItemGroupBO = Nothing
        Dim DeletedGroupIds As String = String.Empty

        ' get list of groupids that have been marked as deleted.
        For Each group As ItemGroupBO In ItemGroupBOBindingSource
            If group.isDeleted Then
                DeletedGroupIds += group.GroupID & ","
            End If
        Next
        ' remove trailing comma
        If DeletedGroupIds.Length > 0 Then
            DeletedGroupIds = DeletedGroupIds.Remove(DeletedGroupIds.Length - 1, 1)
        End If

        ItemGroup = frm.CreateNewGroup(DeletedGroupIds)
        If Not ItemGroup Is Nothing Then
            ItemGroup.MarkNew()
            _groupList.Add(ItemGroup)
        End If
    End Sub

    Private Sub Button_Remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Remove.Click
        Dim ItemGroup As ItemGroupBO
        If Not UltraGrid_Groups.ActiveRow Is Nothing Then

            ItemGroup = CType(ItemGroupBOBindingSource.Current, ItemGroupBO)
            If ItemGroup.PromotionCount = 0 Then
                If MessageBox.Show("Do you really want to delete this Item Group?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                    ItemGroup.MarkDeleted()
                End If

            Else
                MessageBox.Show("This Group must be removed from all promotions before it can be deleted.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
            BindGrid()
            SetGridFilters()
        End If

    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        Dim frm As Form_EditItemGroup
        Dim result As Boolean = False
        Dim CurrentItemGroup As ItemGroupBO
        Dim DeletedGroupIds As String = String.Empty
        Dim IsEdited As Boolean = False
        Dim EditedBy As String
        Dim GroupDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO



        If Not UltraGrid_Groups.ActiveRow Is Nothing Then

            For Each Group As ItemGroupBO In ItemGroupBOBindingSource
                If Group.isDeleted Then
                    DeletedGroupIds += Group.GroupID & ","
                End If
            Next
            If DeletedGroupIds.Length > 0 Then
                DeletedGroupIds = DeletedGroupIds.Remove(DeletedGroupIds.Length - 1, 1)
            End If


            CurrentItemGroup = CType(ItemGroupBOBindingSource.Current, ItemGroupBO)
            EditedBy = GroupDAO.GetGroupEditStatus(CurrentItemGroup.GroupID)
            If EditedBy <> "" Then IsEdited = True Else IsEdited = False
            If IsEdited Then
                MsgBox(String.Format("This group is being edited by {0}. This screen will be read-only.", EditedBy))
            End If


            frm = New Form_EditItemGroup(CurrentItemGroup)
            result = frm.EditGroup(DeletedGroupIds, ((CurrentItemGroup.PendingPromotionCount <> 0) Or IsEdited Or _flagReadOnly))
            If result Then

                PopulateGroupInfo()

                ' make sure any groups that were marked as deleted but not committed to the database are 
                ' still marked as deleted after the grid is refreshed.
                For Each DeletedGroup As String In Split(DeletedGroupIds, ",")
                    For Each Group As ItemGroupBO In ItemGroupBOBindingSource
                        If Group.GroupID.ToString = DeletedGroup Then
                            Group.MarkDeleted()
                        End If
                    Next
                Next

            End If

        End If
    End Sub

    Private Sub SaveChanges()

        Dim GroupDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO
        For Each ItemGroup As ItemGroupBO In ItemGroupBOBindingSource
            If ItemGroup.IsDirty Then

            End If

            'If ItemGroup.IsNew Then
            ' GroupDAO.CreateNewGroup(ItemGroup.GroupName, ItemGroup.GroupLogic)
            ' End If

            If ItemGroup.isDeleted Then
                If ItemGroup.GroupID > 0 Then
                    GroupDAO.DeleteItemGroup(ItemGroup.GroupID)
                End If
            End If
        Next

        Me.Close()

    End Sub

    Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click
        SaveChanges()
    End Sub

    Private Sub ItemGroupBOBindingSource_CurrentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ItemGroupBOBindingSource.CurrentChanged
        UpdateButtonState()
    End Sub

    Private Sub Button_Unlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Unlock.Click
        Dim GroupDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO
        If Not UltraGrid_Groups.ActiveRow Is Nothing Then
            GroupDAO.UnlockGroup(CType(ItemGroupBOBindingSource.Current, ItemGroupBO).GroupID)
            UpdateButtonState()
        End If
    End Sub
End Class