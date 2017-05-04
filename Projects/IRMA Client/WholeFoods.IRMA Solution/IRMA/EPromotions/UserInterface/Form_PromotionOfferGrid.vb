Imports System.ComponentModel   ' Need for BindingList 
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Resources
Imports System.text
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility
Imports Infragistics.Win.UltraWinGrid

Public Class Form_PromotionOfferGrid

#Region "Form Events"
    Private Sub Form_PromotionOfferGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        BindPromotionOfferData()
        InitializeGrid()
        With UltraGrid_PromotionOffers
            If .Rows.Count > 0 Then
                .ActiveRow = .Rows(0)
                .Selected.Rows.Add(.Rows(0))
            End If
        End With

        'Load Title bar caption
        Me.Text = ResourcesEPromotions.GetString("label_titlebar_PromotionalOfferGrid")

        Dim pricingmethodDAO As New PricingMethodDAO
        Dim pricingmethodList As BindingList(Of PricingMethodBO) = pricingmethodDAO.GetPricingMethodList(, True)

        If pricingmethodList.Count = 0 Then
            MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_noEditorEnabledPriceMethods")), Me.Text, MessageBoxButtons.OK)
            Me.Close()
        End If

    End Sub

    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Dim result As Integer

        result = AddOrEditOffer()

        '' If there is an active row, make sure it is also selected
        'If UltraGrid_PromotionOffers.ActiveRow Is Nothing Then

        'Else
        '    If UltraGrid_PromotionOffers.ActiveRow.Selected = False Then
        '        UltraGrid_PromotionOffers.ActiveRow.Selected = True
        '    End If

        'End If

        ' rebind grid data
        BindPromotionOfferData()

    End Sub

    Private Sub Button_Edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        Dim result As Integer

        With UltraGrid_PromotionOffers
            If .Selected.Rows.Count = 0 Then
                .Selected.Rows.Add(.Rows(0))
            End If


            If .Rows.Count > 0 Then
                result = AddOrEditOffer(CInt(.Selected.Rows(0).Cells("PromotionOfferID").Value))
            End If
        End With

    End Sub


    Private Sub UltraGrid_PromotionOffers_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles UltraGrid_PromotionOffers.AfterSelectChange

        ' enable Edit button if row is selected
        UpdateButtonState()

    End Sub

    Private Sub Button_Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Delete.Click


        If Not UltraGrid_PromotionOffers.ActiveRow Is Nothing Then
            DeleteOffer(CInt(UltraGrid_PromotionOffers.Selected.Rows(0).Cells("PromotionOfferID").Value))
        End If

    End Sub

    Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click
        Me.Close()
    End Sub
#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Populate Promotional Offer Binding Source 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindPromotionOfferData()
        Dim offerDAO As New PromotionOfferDAO
        Dim offerList As PromotionOfferBOList = offerDAO.GetPromotionalOfferList
        Dim offerBindingSource As New BindingSource

        BindingSource_PromotionOffer.DataSource = offerList

    End Sub

    ''' <summary>
    ''' Initialize Grid layout
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeGrid()

        With Me.UltraGrid_PromotionOffers
            .DataSource = Me.BindingSource_PromotionOffer
            .DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False

            ' Hide undesireable columns
            With .DisplayLayout.Bands(0)

                For Each gridColumn As Infragistics.Win.UltraWinGrid.UltraGridColumn In .Columns

                    Select Case gridColumn.Key.ToUpper
                        Case "DESC"
                            gridColumn.Hidden = False
                        Case "STARTDATE", "ENDDATE"
                            gridColumn.Hidden = False
                            gridColumn.Format = "M/dd/yyyy"
                            'gridColumn.Format = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
                            gridColumn.MinWidth = 4
                            gridColumn.Width = 4
                        Case Else
                            gridColumn.Hidden = True
                    End Select

                Next

            End With

            ' select first row, if present
            '   .Selected.Rows.Clear()
            '   .ActiveRow = Nothing
            UpdateButtonState()

            'If BindingSource_PromotionOffer.Count > 0 Then
            '    .Rows(0).Selected = True
            '    .ActiveRow = .Rows(0)
            '    UpdateButtonState()
            'End If
        End With

    End Sub

    ''' <summary>
    ''' Launch form to either add or edit a Promotional Offer 
    ''' </summary>
    ''' <remarks></remarks>
    Private Function AddOrEditOffer(Optional ByVal offerID As Integer = 0) As Integer
        Dim formOfferDetail As Form_PromotionOfferEditor
        'Dim idx As Integer

        formOfferDetail = New Form_PromotionOfferEditor

        With formOfferDetail
            If offerID > 0 Then
                .EditOffer(offerID)

                ' refresh grid with updated information
                BindPromotionOfferData()
            Else
                ' Call new offer routine - returns ID of new record or 0 if operation was canceled
                offerID = .NewOffer()
                BindPromotionOfferData()
                If offerID > 0 Then
                    'refresh grid data and select newly added offer

                    'idx = CType(UltraGrid_PromotionOffers.DataSource, BindingSource).Find("PromotionOfferId", offerID)

                End If
            End If

            .Dispose()
        End With
        With UltraGrid_PromotionOffers
            If .Rows.Count > 0 Then
                .ActiveRow = .Rows(0)
                .Selected.Rows.Add(.Rows(0))
            End If
        End With
        'UltraGrid_PromotionOffers.ActiveRow = Nothing
        UpdateButtonState()
    End Function

    ''' <summary>
    ''' delete the selected Promotional Offer and it's Members
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteOffer(ByVal offerID As Integer)
        Dim offerDAO As New PromotionOfferDAO
        Dim selectedRowIndex As Integer

        Try
            'get selected row
            If Me.UltraGrid_PromotionOffers.Selected.Rows.Count = 0 Then
                selectedRowIndex = Me.UltraGrid_PromotionOffers.ActiveCell.Row.Index
            Else
                selectedRowIndex = Me.UltraGrid_PromotionOffers.ActiveRow.Index
            End If
        Catch ex As Exception
            'user is trying to delete AddRow - prompt to select a valid row
            MessageBox.Show(ResourcesEPromotions.GetString("msg_selectDeleteRow"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            Return
        End Try

        'delete row if user has selected a row that is not the NewRow
        Dim currentRow As UltraGridRow = Me.UltraGrid_PromotionOffers.Rows(selectedRowIndex)
        Dim offerBO As New PromotionOfferBO(currentRow)

        ' get unbatched and unprocecess price batch detail counts for this offer
        Dim flagUnbatchedDetails As Boolean = False
        Dim flagUnprocessedDetails As Boolean = False
        Dim tableStores As DataTable
        Dim pricebatchDAO As New PriceBatchDetailDAO
        Dim StoreList As New StringBuilder
        Dim countActiveStores As Integer = 0

        flagUnprocessedDetails = offerBO.BatchPending(offerID)
        flagUnbatchedDetails = pricebatchDAO.GetUnbatchedPriceDetails(offerID) > 0
        tableStores = offerDAO.GetStoresByPromotionId(offerID)

        ' see if stores are associated
        For Each dr As DataRow In tableStores.Rows
            If CBool(dr("IsAssigned")) Then
                StoreList.Append(CType(dr("Store_name"), String).TrimEnd)
                StoreList.Append(Environment.NewLine)
            End If

            If CBool(dr("IsActive")) Then
                countActiveStores += 1
            End If
        Next

        'validate if DELETE can happen
        Select Case offerBO.ValidateDelete(offerID, flagUnbatchedDetails, flagUnprocessedDetails, (StoreList.ToString.Length > 0), countActiveStores)
            Case PromotionOfferStatus.Error_ItemAssociated
                MessageBox.Show(ResourcesEPromotions.GetString("msg_deleteoffer_associateditem"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)

            Case PromotionOfferStatus.Error_Delete_AssociatedStores
                MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_deleteoffer_associatedstores"), Chr(13), StoreList.ToString), _
                    Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)

            Case PromotionOfferStatus.Error_Delete_UnprocessedBatchDetails
                MessageBox.Show(ResourcesEPromotions.GetString("msg_deleteoffer_unprocessedDetails"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)

            Case PromotionOfferStatus.Valid
                'confirm user wishes to delete this item
                Dim result As DialogResult = MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_confirmOfferDelete"), CType(currentRow.Cells("Desc").Value, String).TrimEnd), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                Dim success As Boolean = True
                If result = Windows.Forms.DialogResult.Yes Then
                    Dim transact As SqlTransaction = offerDAO.GetTransaction()

                    ' delete Unbatched details for the offer
                    success = pricebatchDAO.DeleteUnbatchedPriceDetails(offerID, 0, 0, transact)

                    If success Then
                        ' Delete Offer
                        offerDAO.DeleteOffer(offerID, transact)
                        If Not transact.Connection Is Nothing Then
                            transact.Commit()
                        End If

                        'remove item from grid
                        currentRow.Delete()
                    Else
                        MessageBox.Show("Could not delete unbatched Price Batch Details for this offer - Cannot Delete.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If

                End If

        End Select

        tableStores.Dispose()

        UpdateButtonState()
    End Sub

    Private Sub UpdateButtonState()

        Button_Edit.Enabled = Not UltraGrid_PromotionOffers.ActiveRow Is Nothing
        Button_Delete.Enabled = Not UltraGrid_PromotionOffers.ActiveRow Is Nothing

    End Sub
#End Region

    Private Sub UltraGrid_PromotionOffers_BeforeRowsDeleted(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs) Handles UltraGrid_PromotionOffers.BeforeRowsDeleted
        ' DO NOT prompt to confirm delete
        e.DisplayPromptMsg = False
    End Sub
End Class
