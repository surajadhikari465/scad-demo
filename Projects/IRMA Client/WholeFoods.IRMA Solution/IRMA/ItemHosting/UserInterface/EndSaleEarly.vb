Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Pricing.DataAccess

Public Class EndSaleEarly

    Private _itemBO As ItemBO
    Private _storeBO As StoreBO
    Private _regPrice As Decimal
    Private _regMultiple As Integer

    Private _userLockDate As String

#Region "properties"

    Public Property ItemBO() As ItemBO
        Get
            Return _itemBO
        End Get
        Set(ByVal value As ItemBO)
            _itemBO = value
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

    Public Property RegPrice() As Decimal
        Get
            Return _regPrice
        End Get
        Set(ByVal value As Decimal)
            _regPrice = value
        End Set
    End Property

    Public Property RegMultiple() As Integer
        Get
            Return _regMultiple
        End Get
        Set(ByVal value As Integer)
            _regMultiple = value
        End Set
    End Property

#End Region

    Private Sub EndSaleEarly_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()
        LockItem()
        BindData()
    End Sub

    ''' <summary>
    ''' put a LOCK on this item for the current user
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LockItem()
        Dim itemDAO As New ItemDAO
        Dim dsItemLock As DataSet = itemDAO.LockItem(_itemBO.Item_Key, giUserID)

        If CType(dsItemLock.Tables(0).Rows(0)("User_ID"), Integer) <> giUserID Then
            MsgBox(String.Format(ResourcesItemHosting.GetString("ItemLocked"), dsItemLock.Tables(0).Rows(0)("FullName"), dsItemLock.Tables(0).Rows(0)("User_ID_Date")), MsgBoxStyle.Critical, Me.Text)
        Else
            _userLockDate = dsItemLock.Tables(0).Rows(0)("User_ID_Date").ToString
        End If
    End Sub

    ''' <summary>
    ''' bind data to the grid with batch data; load default regular price value for item
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        'load processed sale data to grid
        Me.UltraGrid_SaleData.DataSource = AdjustSaleDataDAO.GetCurrentProcessedSales(_itemBO.Item_Key, _storeBO.StoreNo)

        'select the last row for the user, as it is the sale that is currently residing on the POS system
        If UltraGrid_SaleData.Rows.Count > 0 Then
            'make row appear as if it's selected; the user can't actually select rows because this feature has been disabled
            UltraGrid_SaleData.Rows(UltraGrid_SaleData.Rows.Count - 1).Appearance.BackColor = Color.RoyalBlue
            UltraGrid_SaleData.Rows(UltraGrid_SaleData.Rows.Count - 1).Appearance.ForeColor = Color.White
        End If

        SetItemRegularPriceInfo()
    End Sub

    ''' <summary>
    ''' sets the current regular price and current multiple for the selected item
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetItemRegularPriceInfo()
        txtMultiple.Text = _regMultiple.ToString
        txtPOSPrice.Text = String.Format(_regPrice.ToString, "##0.00")
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean
        Dim adjustSaleData As New AdjustSaleDataBO
        Dim statusList As ArrayList
        Dim currentStatus As AdjustSaleDataStatus
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim storeList As New StringBuilder

        'set non-UI values required for save
        adjustSaleData.ItemKey = _itemBO.Item_Key
        adjustSaleData.StoreList = _storeBO.StoreNo.ToString
        adjustSaleData.UserID = giUserID

        adjustSaleData.UserIDDate = _userLockDate

        'get user entries from form        
        adjustSaleData.StartDate = CType(Me.dtpStartDate.Value, Date)
        adjustSaleData.POSPrice = CType(Me.txtPOSPrice.Value, Decimal)
        adjustSaleData.SetPrice(adjustSaleData.POSPrice)
        If AdjustSaleDataBO.IsIntegerValue(Me.txtMultiple.Text) Then
            adjustSaleData.RegMultiple = CType(Me.txtMultiple.Text, Integer)
        End If

        'get selected sale to end early (will be max row in the grid)
        If UltraGrid_SaleData.Rows.Count > 0 Then
            'setup store info
            adjustSaleData.PriceBatchDetailIdToEndEarly = CType(UltraGrid_SaleData.Rows(UltraGrid_SaleData.Rows.Count - 1).Cells("ID").Value, Integer)

            'get the row's Sale_End_Date
            adjustSaleData.PreviousSaleEndDate = CType(UltraGrid_SaleData.Rows(UltraGrid_SaleData.Rows.Count - 1).Cells("Sale End").Value, Date)
        End If

        'validate current set of data
        statusList = adjustSaleData.ValidateEndSaleEarly

        'loop through possible validation erorrs and build message string containing all errors
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, AdjustSaleDataStatus)

            Select Case currentStatus
                Case AdjustSaleDataStatus.Error_Required_SelectSale
                    message.Append(ResourcesItemHosting.GetString("msg_error_SelectSaleToEndEarly"))
                    message.Append(Environment.NewLine)
                Case AdjustSaleDataStatus.Error_StartDate_PastDate
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_startDateInFuture"), Me.Label_StartDate.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case AdjustSaleDataStatus.Error_StartDate_AfterSaleEndDate
                    message.Append(String.Format(ResourcesItemHosting.GetString("msg_validation_startDateAfterSaleEndDate"), Me.Label_StartDate.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
                Case AdjustSaleDataStatus.Error_Invalid_RegularMultiple
                    message.Append(ResourcesItemHosting.GetString("RegularMultipleNotZero"))
                    message.Append(Environment.NewLine)
                Case AdjustSaleDataStatus.Error_Invalid_RegularPrice
                    message.Append(String.Format(ResourcesCommon.GetString("msg_validation_greaterThanZero"), Me.Label_POSPrice.Text.Replace(":", "")))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If message.Length <= 0 Then
            Dim result As DialogResult

            'perform final validations
            '1. Check to see if there are any pending price change batches for the Item/Store selected in the Building state.  
            '   If there are, the user will be warned that the pending changes will be removed from the batch.
            Dim batchesInBuildingStatus As Boolean = AdjustSaleDataDAO.CheckForPendingBatches(adjustSaleData, BatchStatus.Building)

            If batchesInBuildingStatus Then
                MessageBox.Show(ResourcesItemHosting.GetString("msg_warning_CancelSales_BatchesInBuildingStatus"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            '2. Check to see if there are any pending price change batches for the Item/Store selected in the Packaged, Ready, Printed, or Sent states.  
            '   If there are, the user will receive an error that they cannot cancel the sale for the store until these batches are Processed. 
            Dim batchesInPendingStatus As Boolean = AdjustSaleDataDAO.CheckForPendingBatches(adjustSaleData, BatchStatus.AllButProcessedAndBuilding)

            If batchesInPendingStatus Then
                'the user can not proceed with saving this change
                MessageBox.Show(String.Format(ResourcesItemHosting.GetString("msg_error_AdjustSale_UnprocessedBatches"), ResourcesItemHosting.GetString("msg_text_EndSaleEarly")), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                success = True
            Else
                'the user can proceed
                'prompt user with final warning that they are about to cancel all sales for the selected stores
                result = MessageBox.Show(ResourcesItemHosting.GetString("msg_confirm_EndSaleEarly"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If result = Windows.Forms.DialogResult.Yes Then
                    'save data
                    Dim adjustSaleDAO As New AdjustSaleDataDAO

                    Try
                        Dim saveStatus As Integer = adjustSaleDAO.EndSaleEarly(adjustSaleData)
                        If saveStatus <> 0 Then  ' 0 is the VALID code
                            ' A validation error was encountered during the save.  Let the user know and exit processing.
                            ' Make sure it wasn't just a warning.
                            If ValidationDAO.IsErrorCode(saveStatus) Then
                                success = False
                                Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(saveStatus)
                                MsgBox(String.Format(ResourcesPricing.GetString("UnknownPriceChgError"), vbCrLf, saveStatus, validationCode.ValidationCodeDesc), MsgBoxStyle.Critical, Me.Text)
                            Else
                                success = True
                            End If
                        Else
                            success = True
                        End If
                    Catch ex As Exception
                        success = False
                        MessageBox.Show(ResourcesCommon.GetString("msg_dbError"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                Else
                    success = True
                End If
            End If
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Return success
    End Function

#Region "Event Handlers"

    ''' <summary>
    ''' exit form
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Exit.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' save data
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ApplyChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ApplyChanges.Click
        'validate that user has sale data available to end early
        If UltraGrid_SaleData.Rows.Count = 0 Then
            MessageBox.Show(ResourcesItemHosting.GetString("msg_warning_EndSaleEarlyNotPossible"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            'save changes
            If ApplyChanges() Then
                'close this form is successful in saving
                Me.Hide()
            End If
        End If
    End Sub

#End Region

End Class