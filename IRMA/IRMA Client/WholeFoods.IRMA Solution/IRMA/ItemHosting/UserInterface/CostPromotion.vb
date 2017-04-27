Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class CostPromotion

    Private _itemBO As ItemBO
    Private _vendorBO As VendorBO
    Private _storeBO As StoreBO
    Dim WithEvents _costPromoForm As CostPromotionDetail

#Region "properties"

    Public Property ItemBO() As ItemBO
        Get
            Return _itemBO
        End Get
        Set(ByVal value As ItemBO)
            _itemBO = value
        End Set
    End Property

    Public Property VendorBO() As VendorBO
        Get
            Return _vendorBO
        End Get
        Set(ByVal value As VendorBO)
            _vendorBO = value
        End Set
    End Property

    Public Property StoreBO() As StoreBO
        Get
            Return _storeBO
        End Get
        Set(ByVal value As StoreBO)
            _storeBO = value
        End Set
    End Property

#End Region

    Private Sub CostPromotion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToParent()

        LoadStaticData()
        BindData()
    End Sub

    ''' <summary>
    ''' sets label values that are read-only; data is passed from parent screens
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadStaticData()
        'display item unit data
        Dim packDesc As New StringBuilder
        packDesc.Append(Format(_itemBO.PackageDesc1, ResourcesIRMA.GetString("NumberFormatBigInteger")))
        packDesc.Append(" / ")
        packDesc.Append(Format(_itemBO.PackageDesc2, ResourcesIRMA.GetString("NumberFormatBigDecimal")))

        packDesc.Append(" ")
        packDesc.Append(_itemBO.PackageUnitName)

        Me.Label_StoreNameValue.Text = _storeBO.StoreName
        Me.Label_VendorValue.Text = _vendorBO.VendorName
        Me.Label_ItemValue.Text = _itemBO.ItemDescription
        Me.Label_PkgDescValue.Text = packDesc.ToString
    End Sub

    ''' <summary>
    ''' binds data to the grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        Dim gridData As DataTable = CostPromotionDAO.GetVendorDeals(_itemBO.Item_Key, _vendorBO.VendorID, _storeBO.StoreNo)
        Me.UltraGrid_VendorDeals.DataSource = gridData

        If UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns.Count > 0 Then
            'hide columns
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("CostPromoCodeTypeID").Hidden = True

            'sort columns in correct order
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("CostPromoDesc").Header.VisiblePosition = 0
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("VendorDealTypeDesc").Header.VisiblePosition = 1
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("CaseAmt").Header.VisiblePosition = 2
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("CaseAmtType").Header.VisiblePosition = 3
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("StartDate").Header.VisiblePosition = 4
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("EndDate").Header.VisiblePosition = 5

            'set column names
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("VendorDealTypeDesc").Header.Caption = ResourcesItemHosting.GetString("label_header_vendorDealType")

            'set extra column properties (for some reason are not being picked up by grid props)
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("VendorDealTypeDesc").CellActivation = Activation.NoEdit
            UltraGrid_VendorDeals.DisplayLayout.Bands(0).Columns("VendorDealTypeDesc").CellClickAction = CellClickAction.RowSelect

            If gridData.Rows.Count > 0 Then
                'default first row to be selected
                UltraGrid_VendorDeals.DisplayLayout.Rows(0).Selected = True
            End If
        End If
    End Sub

    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    Private Sub Button_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Delete.Click
        'validate that user has selected a cost promo to delete
        Dim selectedRow As UltraGridRow = GetSelectedRow()

        If selectedRow IsNot Nothing Then
            Dim extraMsgText As New StringBuilder
            extraMsgText.Append(Environment.NewLine)
            extraMsgText.Append(Environment.NewLine)
            extraMsgText.Append("       ")
            extraMsgText.Append(selectedRow.Cells("CostPromoDesc").Column.Header.Caption)
            extraMsgText.Append(": ")
            extraMsgText.Append(selectedRow.Cells("CostPromoDesc").Value.ToString)
            extraMsgText.Append(Environment.NewLine)
            extraMsgText.Append("       ")
            extraMsgText.Append(selectedRow.Cells("VendorDealTypeDesc").Column.Header.Caption)
            extraMsgText.Append(": ")
            extraMsgText.Append(selectedRow.Cells("VendorDealTypeDesc").Value.ToString)

            'ask user if they are sure they wish to delete
            Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesCommon.GetString("msg_deleteConfirmation"), extraMsgText.ToString), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                Dim promoDAO As New CostPromotionDAO
                Dim vendorDeal As New VendorDealBO

                vendorDeal.VendorDealHistoryID = CType(selectedRow.Cells("VendorDealHistoryID").Value, Integer)
                promoDAO.DeleteVendorDeal(vendorDeal)

                'refresh grid data
                BindData()
            End If
        Else
            'inform user to select a row
            MessageBox.Show(ResourcesCommon.GetString("msg_selectDeleteRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        'get selected row from grid
        Dim selectedRow As UltraGridRow = GetSelectedRow()

        If selectedRow IsNot Nothing Then
            _costPromoForm = New CostPromotionDetail
            _costPromoForm.IsAdd = False
            _costPromoForm.ItemBO = _itemBO
            _costPromoForm.VendorBO = _vendorBO
            _costPromoForm.StoreBO = _storeBO
            _costPromoForm.VendorDealBO = New VendorDealBO(selectedRow)

            _costPromoForm.ShowDialog()
            _costPromoForm.Dispose()
        Else
            'inform user to select a row
            MessageBox.Show(ResourcesCommon.GetString("msg_selectEditRow"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
    End Sub

    ''' <summary>
    ''' refresh data grid w/ new values from add/edit form
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _costPromoForm_UpdateCallingForm() Handles _costPromoForm.UpdateCallingForm
        BindData()
    End Sub

    Private Function GetSelectedRow() As UltraGridRow
        Dim selectedRowIndex As Integer = -1
        Dim selectedRow As UltraGridRow = Nothing

        If Me.UltraGrid_VendorDeals.Selected.Rows.Count = 0 Then
            If Me.UltraGrid_VendorDeals.ActiveCell IsNot Nothing Then
                selectedRowIndex = Me.UltraGrid_VendorDeals.ActiveCell.Row.Index
            End If
        Else
            selectedRowIndex = Me.UltraGrid_VendorDeals.ActiveRow.Index
        End If

        If selectedRowIndex > -1 Then
            selectedRow = Me.UltraGrid_VendorDeals.Rows(selectedRowIndex)
        End If

        Return selectedRow
    End Function

End Class