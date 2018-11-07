Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports System.ComponentModel   ' Need for BindingList 
Imports WholeFoods.Utility
Imports Infragistics.Win.UltraWinGrid

Public Class Form_EditItemGroup


    Private _PromotionOfferMember As PromotionOfferMemberBO
    Private _ItemGroup As ItemGroupBO
    Private _RefreshStatus As Boolean = False
    Private _frmItemSearch As frmItemSearch
    Private _GroupItems As BindingList(Of ItemGroupMemberBO)
    Private _IsLoading As Boolean = True
    Private _GroupNameChanged As Boolean = False
    Private _DeletedGroupIds As String = String.Empty
    Private _PromotionalOfferPriceBatchDetail As PromotionalOfferPriceBatchDetailBO
    Private _flagReadOnlyGroup As Boolean

    Public Sub New(ByVal PromotionOfferMember As PromotionOfferMemberBO)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _PromotionOfferMember = PromotionOfferMember


    End Sub
    Public Sub New(ByRef ItemGroup As ItemGroupBO)
        Dim PO As PromotionOfferMemberBO = New PromotionOfferMemberBO
        InitializeComponent()
        PO.Loading = True
        PO.GroupID = ItemGroup.GroupID
        PO.GroupName = ItemGroup.GroupName
        PO.Loading = False
        _PromotionOfferMember = PO

    End Sub


    Public Function EditGroup(ByVal DeletedGroupIds As String, ByVal _ReadOnlyGroup As Boolean) As Boolean
        Dim retval As Boolean = True
        Dim GroupDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO

        _flagReadOnlyGroup = _ReadOnlyGroup
        If _flagReadOnlyGroup Then
            TextBox_GroupName.Enabled = False
            Button_Save.Enabled = False
            Button_AddItem.Enabled = False
            Button_RemoveItem.Enabled = False
            rdoAnd.Enabled = False
            rdoOr.Enabled = False
        Else
            TextBox_GroupName.Enabled = True
            Button_Save.Enabled = True
            Button_AddItem.Enabled = True
            Button_RemoveItem.Enabled = True
            rdoAnd.Enabled = True
            rdoOr.Enabled = True
            GroupDAO.SetGroupEditStatus(_PromotionOfferMember.GroupID, True)

        End If



        _DeletedGroupIds = DeletedGroupIds

        ' create new Price Batch detail management object
        _PromotionalOfferPriceBatchDetail = New PromotionalOfferPriceBatchDetailBO
        _PromotionalOfferPriceBatchDetail.PreEditInitialization(_PromotionOfferMember.GroupID, PromotionalOfferPriceBatchDetailBO.InitializeType.GroupOwners)


        Me.ShowDialog(MyBase.ParentForm)
        Return retval
    End Function
    Private Sub Form_EditItemGroup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _ItemGroup = getItemGroupInfo(_PromotionOfferMember)
        _ItemGroup.MarkClean()
        PopulateGroupName(_ItemGroup)
        PopulateGroupItems(_ItemGroup)
        PopulateGroupLogic(_ItemGroup)

        With UltraGrid_GroupItems
            If .Rows.Count > 0 Then
                .ActiveRow = .Rows(0)
                .Selected.Rows.Add(.Rows(0))
            End If
        End With

  


        _IsLoading = False
    End Sub

    Private Function getItemGroupInfo(ByRef _PromotionalOfferMember As PromotionOfferMemberBO) As ItemGroupBO
        Dim dao As PromotionOfferDAO = New PromotionOfferDAO
        Dim groupList As BindingList(Of ItemGroupBO)
        groupList = dao.GetPromotionalGroupList(_PromotionalOfferMember.GroupID)
        Return groupList.Item(0)
    End Function

    Private Sub PopulateGroupItems(ByVal ItemGroup As ItemGroupBO)
        Dim groupItemDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO
        _GroupItems = groupItemDAO.GetGroupItems(ItemGroup.GroupID)
        GroupItemsBindingSource.DataSource = _GroupItems

        BindGrid()
        FormatGrid()

    End Sub

    Private Sub BindGrid()
        'GroupItemsBindingSource.Filter = "IsDeleted = False"
        UltraGrid_GroupItems.DataSource = GroupItemsBindingSource
    End Sub
    Private Sub PopulateGroupName(ByVal ItemGroup As ItemGroupBO)
        TextBox_GroupName.Text = ItemGroup.GroupName
    End Sub

    Private Sub PopulateGroupLogic(ByVal ItemGroup As ItemGroupBO)
        If ItemGroup.GroupLogic = ItemGroup_GroupLogic.And Then
            rdoAnd.Checked = True
        Else
            rdoOr.Checked = True
        End If
    End Sub

    Private Sub Button_AddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddItem.Click


        _frmItemSearch = New frmItemSearch()
        AddHandler _frmItemSearch.ItemSelected, AddressOf HandleItemSelected
        _frmItemSearch.ShowDialog()
        RemoveHandler _frmItemSearch.ItemSelected, AddressOf HandleItemSelected
        _frmItemSearch.Dispose()
        _ItemGroup.MarkDirty()


    End Sub

    Private Sub HandleItemSelected(ByVal itemSearch As ItemSearchBO)
        Dim GroupMember As ItemGroupMemberBO = New ItemGroupMemberBO
        Dim DAO As ItemGroupMemberDAO = New ItemGroupMemberDAO

        GroupMember.ItemKey = itemSearch.ItemKey
        GroupMember.ItemDesc = itemSearch.ItemDesc
        GroupMember.ItemIdentifier = itemSearch.ItemIdentifier

        GroupMember.GroupId = _ItemGroup.GroupID
        GroupMember.ModifiedDate = DateTime.Now
        GroupMember.MarkNew()

        ' dont allow addition of duplicate items.
        For Each item As ItemGroupMemberBO In _GroupItems
            If item.ItemKey = GroupMember.ItemKey Then Exit Sub
        Next
        _GroupItems.Add(GroupMember)

    End Sub

    Private Sub FormatGrid()
        With UltraGrid_GroupItems.DisplayLayout.Bands(0)
            .Columns("isDeleted").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
            .ColumnFilters("isDeleted").FilterConditions.Add(FilterComparisionOperator.NotEquals, True)

            .Columns("Status").AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
            .ColumnFilters("Status").FilterConditions.Add(FilterComparisionOperator.NotEquals, "Delete")

        End With

    End Sub

    Private Sub Button_RemoveItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RemoveItem.Click
        If UltraGrid_GroupItems.Selected.Rows.Count > 0 Then
            If MessageBox.Show("Do you really want to delete this Item?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                CType(GroupItemsBindingSource.Current, ItemGroupMemberBO).MarkDeleted()
                _ItemGroup.MarkDirty()
                BindGrid()
                FormatGrid()
            End If
        End If
    End Sub

    Private Sub rdoAnd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoAnd.CheckedChanged
        If Not _IsLoading Then
            _ItemGroup.MarkDirty()
        End If
    End Sub

    Private Sub rdoOr_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoOr.CheckedChanged
        If Not _IsLoading Then
            _ItemGroup.MarkDirty()
        End If

    End Sub

    Private Sub TextBox_GroupName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_GroupName.TextChanged
        If Not _IsLoading Then
            _ItemGroup.MarkDirty()
            _GroupNameChanged = True
        End If
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Dim ItemDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO
        If Not _flagReadOnlyGroup Then
            ItemDAO.SetGroupEditStatus(_ItemGroup.GroupID, False)
        End If
        Me.Close()
    End Sub

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click

        SaveAllChanges()
    End Sub

    Private Sub SaveAllChanges()
        Dim GroupBO As ItemGroupBO = New ItemGroupBO
        Dim ItemDAO As ItemGroupMemberDAO = New ItemGroupMemberDAO
        Dim ItemBO As ItemGroupMemberBO = New ItemGroupMemberBO
        Dim ValidGroupName As Boolean = False
        Dim result As Boolean = True


        If rdoAnd.Checked Then
            _ItemGroup.GroupLogic = ItemGroup_GroupLogic.And
        Else
            _ItemGroup.GroupLogic = ItemGroup_GroupLogic.Or
        End If


        If _ItemGroup.IsDirty Then
            'Group Name
            If _GroupNameChanged Then
                _ItemGroup.GroupName = TextBox_GroupName.Text
                ValidGroupName = GroupBO.ValidateItemGroup(_ItemGroup, _DeletedGroupIds)
            Else
                ValidGroupName = True
            End If
            If ValidGroupName Then
                'save changes to group informaiton. 
                result = ItemDAO.InsertOrUpdateGroupData(_ItemGroup.GroupID = 0, _ItemGroup)
            Else
                result = False
            End If
        End If



        'Group Items 
        For Each item As ItemGroupMemberBO In GroupItemsBindingSource
            If item.IsDirty Then
                If item.isDeleted Then
                    ItemDAO.RemoveItemFromGroup(_PromotionOfferMember.OfferID, _ItemGroup.GroupID, item.ItemKey)
                End If

                If item.IsNew Then
                    ItemDAO.AddItemToGroup(_PromotionOfferMember.OfferID, _ItemGroup.GroupID, item.ItemKey)
                End If
            End If
        Next

        ' Create Price Batch Detail records to reflect current changes in all offers that use this group
        If result Then

            If _PromotionalOfferPriceBatchDetail.MaintainPriceBatchDetail() Then
                MessageBox.Show("Price Batch Detail records could not be built.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        End If

        If result Then
            Me.Close()
            ItemDAO.SetGroupEditStatus(_ItemGroup.GroupID, False)
        End If

    End Sub

    ''' <summary>
    ''' Create Price Batch Detail records to reflect current changes in all offers that use this group
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MaintainPriceBatchDetail() As Boolean

        ' Build list of offers to process
        Dim listOfferID As ArrayList = Nothing


        ' Build list of all stores 
        Dim listStoreId As ArrayList = Nothing


        ' Process each Offer for each possible store
        Dim listPBDItem As ArrayList = Nothing 'list of items in offer as defined in PriceBatchDetail file
        Dim batchdetailDAO As New PriceBatchDetailDAO

        For Each offerID As Integer In listOfferID
            For Each StoreID As Integer In listStoreId
                'listPBDItem = batchdetailDAO.GetPriceBatchDetailPromoDefiniton(StoreID, offerID)

            Next
        Next

    End Function
End Class
