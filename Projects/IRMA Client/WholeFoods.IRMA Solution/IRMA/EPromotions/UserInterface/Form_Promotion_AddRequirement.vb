Imports WholeFoods.Utility
Imports System.ComponentModel
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.EPromotions.BusinessLogic

Public Class Form_Promotion_AddRequirement

    Private _CurrentPromotion As PromotionOfferBO
    Private _Logic As PromotionOfferMemberJoinLogic
    Private _Purpose As PromotionOfferMemberPurpose
    Private _SelectedPromotionOfferMember As PromotionOfferMemberBO
    Private _GroupIdsToExclude As ArrayList = Nothing




    Sub New(ByVal Promotion As PromotionOfferBO, ByVal Logic As PromotionOfferMemberJoinLogic, ByVal Purpose As PromotionOfferMemberPurpose, Optional ByVal GroupIdsToExclude As ArrayList = Nothing)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _CurrentPromotion = Promotion
        _Logic = Logic
        _Purpose = Purpose
        If Not GroupIdsToExclude Is Nothing Then
            _GroupIdsToExclude = GroupIdsToExclude
        
        End If




    End Sub

    Private Sub Form_Promotion_AddRequirement_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadAvailableGroups()
    End Sub

    Private Sub LoadAvailableGroups()
        Dim DAO As PromotionOfferDAO = New PromotionOfferDAO()
        Dim AvailableGroups As BindingList(Of ItemGroupBO)
        AvailableGroups = DAO.GetPromotionalGroupList(-1)
        RemoveExcludedGroups(_GroupIdsToExclude, AvailableGroups)


        With ListBox_AvailableGroups
            .DataSource = AvailableGroups
            .DisplayMember = "GroupName"
            .ValueMember = "GroupId"
        End With


    End Sub
    Private Sub RemoveExcludedGroups(ByRef GroupIdsToExclude As ArrayList, ByRef AvailableGroups As BindingList(Of ItemGroupBO))
        Dim GroupsToRemove As ArrayList = New ArrayList

        For Each ExcludeGroupId As Integer In GroupIdsToExclude
            For Each Group As ItemGroupBO In AvailableGroups
                If Group.GroupID = ExcludeGroupId Then
                    GroupsToRemove.Add(Group)
                End If
            Next
        Next

        For Each Group As ItemGroupBO In GroupsToRemove
            AvailableGroups.Remove(Group)
        Next
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        _SelectedPromotionOfferMember = Nothing
        Me.Close()
    End Sub

    Public Function AddGroup() As PromotionOfferMemberBO
        Me.ShowDialog(MyBase.ParentForm)
        Return _SelectedPromotionOfferMember
    End Function
    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        If ListBox_AvailableGroups.SelectedItems.Count > 0 Then
            _SelectedPromotionOfferMember = GeneratePromotionOfferMember(CType(ListBox_AvailableGroups.SelectedItem, ItemGroupBO))
        Else
            _SelectedPromotionOfferMember = Nothing
        End If
        Me.Close()
    End Sub

    Private Function GeneratePromotionOfferMember(ByVal Item As ItemGroupBO) As PromotionOfferMemberBO
        Dim DAO As PromotionOfferDAO = New PromotionOfferDAO()
        Dim PromotionallOfferMember As PromotionOfferMemberBO = New PromotionOfferMemberBO()

        PromotionallOfferMember.OfferID = _CurrentPromotion.PromotionOfferID
        PromotionallOfferMember.CreateDate = DateTime.Now
        PromotionallOfferMember.ModifiedDate = DateTime.Now
        PromotionallOfferMember.Modified = DateTime.Now
        PromotionallOfferMember.Purpose = PromotionOfferMemberPurpose.Requirement
        PromotionallOfferMember.JoinLogic = _Logic
        PromotionallOfferMember.Quantity = CInt(TextBox_Quantity.Text)
        PromotionallOfferMember.GroupID = Item.GroupID
        PromotionallOfferMember.GroupName = Item.GroupName
        PromotionallOfferMember.MarkNew()

        Return PromotionallOfferMember

    End Function

    Private Sub TextBox_Quantity_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox_Quantity.Validating
        Dim value As Integer
        Try
            value = Int32.Parse(TextBox_Quantity.Text.Trim())
        Catch ex As Exception
            e.Cancel = True
            MessageBox.Show("You must enter an integer value greater than 0.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

        If value = 0 Then TextBox_Quantity.Text = "1"
    End Sub
End Class