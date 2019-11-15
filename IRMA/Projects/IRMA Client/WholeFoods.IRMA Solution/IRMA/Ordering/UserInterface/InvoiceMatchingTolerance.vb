Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win
Imports log4net




Public Class frm_InvoiceMatchingTolerance



#Region "Private Variables"

    Private Shared Logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' DATASET HOLDING ALL INVOICE TOLERANCE DATA
    Dim _toleranceData As New DataSet
    ' DATASET HOLDING ALL GRID DROP DOWN DATA
    Dim _gridDropDownData As New DataSet
    Dim _stores As New DataTable
    Dim _storeID_List As New List(Of Integer)
    Dim _vendors As New DataTable
    Dim _vendorID_List As New List(Of Integer)
    ' Declare a binding manager field
    Private bMgr As BindingManagerBase
    ' declare object of clsEventViewer to handle viewer text box
    'Private c_eventViewer As clsEventViewer
    ' reference to cell containing bad input
    Private c_cellBadCell As UltraGridCell

#End Region


#Region "Properties"
    Public Property ToleranceData() As DataSet
        Get
            Return _toleranceData
        End Get
        Set(ByVal value As DataSet)
            _toleranceData = value
        End Set
    End Property

    Public Property GridDropDowndata() As DataSet
        Get
            Return _gridDropDownData
        End Get
        Set(ByVal value As DataSet)
            _gridDropDownData = value
        End Set
    End Property



    Public Property Stores() As DataTable
        Get
            Return _stores
        End Get
        Set(ByVal value As DataTable)
            _stores = value
        End Set
    End Property

    Public Property StoreID_List() As List(Of Integer)
        Get
            Return _storeID_List
        End Get
        Set(ByVal value As List(Of Integer))
            _storeID_List = value
        End Set
    End Property


    Public Property Vendors() As DataTable
        Get
            Return _vendors
        End Get
        Set(ByVal value As DataTable)
            _vendors = value
        End Set
    End Property

    Public Property VendorID_List() As List(Of Integer)
        Get
            Return _vendorID_List
        End Get
        Set(ByVal value As List(Of Integer))
            _vendorID_List = value
        End Set
    End Property


#End Region

#Region "Form Events"


    Private Sub InvoiceMatchingTolerance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' ****** Load the Form and Populate All Grids ***** 

        Logger.Info("Entry")
        ' CREATE BUSINESS OBJECT
        Dim bo As New InvoiceMatchingBO
        ' GET ALL TOLERANCE DATA
        ToleranceData = bo.GetAllToleranceValues()
        ToleranceData.AcceptChanges()
        ' GET DATA FOR THE GRID 
        Stores = bo.GetStores
        Vendors = bo.GetVendors
        ' GET LIST OF VENDOR/STORE ID'S FOR VALIDATION
        'StoreID_List = bo.GetStoreIDList(ToleranceData)
        'VendorID_List = bo.GetVendorIDList(ToleranceData)
        ' *************************************************
        ' Bind Data to DataGrids and Controls
        BindControls()
        Global.SetUltraGridSelectionStyle(StoreOverrideGrid)
        Global.SetUltraGridSelectionStyle(VendorOverrideGrid)

        ' Print Original Values from Dataset ******* FOR DEBUG PURPOSES *********
        'bo.PrintValues(ToleranceData, "OriginalData")
    End Sub


    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click

        Dim result As DialogResult
        ' Check for outstanding DataSet changes?
        If ToleranceData.HasChanges() Then
            result = MessageBox.Show("There are changes which have not been saved yet - Do you want to proceed?", "Unsaved Changes", MessageBoxButtons.YesNo)
        End If

        If result = Windows.Forms.DialogResult.Yes Then
            ToleranceData.RejectChanges()
            Me.Close()
        End If

        Logger.Info("Exit")
    End Sub

    Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click

        Dim bo As New InvoiceMatchingBO

        ' Print Modified Values from Dataset ******* FOR DEBUG PURPOSES *********
        'bo.PrintValues(ToleranceData, "ModifiedData")

        'End Edit for TxtBox data binding object - without it ...hasChanges does not return 1 
        Me.BindingContext(ToleranceData.Tables(0)).EndCurrentEdit()


        If ToleranceData.HasChanges() Then

            If ToleranceData.HasErrors Then
                MessageBox.Show("Entered Data is Invalid !", "Submit Error", _
                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Else
                bo.UpdateDataSource(ToleranceData)
                If bo.IsError = True Then
                    ' ERRROR OCCURRED
                    ToleranceData.RejectChanges()
                    MessageBox.Show(String.Format("ERROR IN UPDATING DATABASE - {0}", bo.ErrorMessage), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Else
                    ToleranceData.AcceptChanges()
                    MessageBox.Show("Invoice Tolerance Data Has Been Successfully Updated", "Changes Saved", MessageBoxButtons.OK)
                End If
            End If

        End If

        

    End Sub


    Private Sub txbx_PercentageValue_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txbx_PercentageValue.Leave

        If Not txbx_PercentageValue.Text = "" Then

            If Not IsNumeric(txbx_PercentageValue.Text) Then
                MessageBox.Show("The value has to be numeric", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Else
                If Not CType(txbx_PercentageValue.Text, Double) >= 0 Or Not CType(txbx_PercentageValue.Text, Double) <= 100 Then
                    MessageBox.Show("The value has to be between 0 and 100", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                End If
            End If
        Else
            ToleranceData.Tables(0).Rows(0).Item("Vendor_Tolerance") = DBNull.Value
        End If
    End Sub


    Private Sub txbx_AmountValue_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txbx_AmountValue.Leave
        If Not txbx_AmountValue.Text = "" Then
            If Not IsNumeric(txbx_AmountValue.Text) Then
                MessageBox.Show("The value has to be numeric", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            Else
                If Not CType(txbx_AmountValue.Text, Double) >= 0 Or Not CType(txbx_AmountValue.Text, Double) <= 10000 Then
                    MessageBox.Show("The value has to be between 0 and 10000", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                End If
            End If
        Else
            ToleranceData.Tables(0).Rows(0).Item("Vendor_Tolerance_Amount") = DBNull.Value
        End If
    End Sub

#End Region

#Region "Subs"

    Private Sub BindControls()

        ' Bind Data to DataGrids and Controls

        StoreOverrideGrid.DataSource = ToleranceData.Tables(ToleranceTypes.Store)
        VendorOverrideGrid.DataSource = ToleranceData.Tables(ToleranceTypes.Vendor)

        ' Bind Text Boxes
        txbx_PercentageValue.DataBindings.Add("Text", ToleranceData.Tables(ToleranceTypes.Regional), "Vendor_Tolerance")
        txbx_AmountValue.DataBindings.Add("Text", ToleranceData.Tables(ToleranceTypes.Regional), "Vendor_Tolerance_Amount")




    End Sub


#End Region

#Region "Functions"

    Private Function ValidatePercent( _
ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) _
As Boolean
        If Not e.NewValue Is DBNull.Value Then
            If e.NewValue < 0 Or e.NewValue > 100 Then

                MessageBox.Show("Value must be from 0 to 100")
                e.Cancel = True
                Return False

            Else

                Return True

            End If
        End If
    End Function

    Private Function ValidateAmount( _
    ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) _
    As Boolean
        If Not e.NewValue Is DBNull.Value Then
            If e.NewValue < 0 Or e.NewValue > 10000 Then

                MessageBox.Show("Value must be from 0 to 10000")
                e.Cancel = True
                Return False

            Else

                Return True

            End If
        End If
    End Function

    Private Function ValidateID(ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs, ByVal gridType As Integer) As Boolean

        Dim IDs As New List(Of Integer)
        Dim az As Integer
        Dim grid As UltraWinGrid.UltraGrid = Nothing

        Select Case gridType

            Case ToleranceTypes.Store

                grid = Me.StoreOverrideGrid

            Case ToleranceTypes.Vendor

                grid = Me.VendorOverrideGrid

        End Select

        For az = 0 To grid.DisplayLayout.Rows.Count - 2
            If Not grid.DisplayLayout.Rows(az).Cells(0).Value Is DBNull.Value Then
                IDs.Add(grid.DisplayLayout.Rows(az).Cells(0).Value)
            End If
        Next

        If IDs.Contains(e.NewValue) Then
            MessageBox.Show(grid.Name & " ID must be unique - Selection Cancelled")
            e.Cancel = True
            grid.DisplayLayout.Rows(az).Delete(False)
            Return False
        Else
            Return True
        End If

    End Function

#End Region

#Region "Enumerations"

    Public Enum ToleranceTypes
        Regional
        Store
        Vendor
    End Enum


#End Region


#Region "Grid Setup"

    Private Sub StoreOverrideGrid_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles StoreOverrideGrid.AfterRowActivate
        If StoreOverrideGrid.ActiveRow.IsAddRow Then
            StoreOverrideGrid.DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.AllowEdit
        End If
    End Sub

    Private Sub VendorOverrideGrid_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles VendorOverrideGrid.AfterRowActivate
        If VendorOverrideGrid.ActiveRow.IsAddRow Then
            VendorOverrideGrid.DisplayLayout.Bands(0).Columns(1).CellActivation = Activation.AllowEdit
        End If
    End Sub

    Private Sub StoreOverrideGrid_BeforeCellDeactivate(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles StoreOverrideGrid.BeforeCellDeactivate
        If Not c_cellBadCell Is Nothing Then

            e.Cancel = True
            StoreOverrideGrid.ActiveCell = c_cellBadCell
            StoreOverrideGrid.PerformAction( _
            Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode _
            , False _
            , False)

            c_cellBadCell = Nothing

        End If

    End Sub

    Private Sub StoreOverrideGrid_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles StoreOverrideGrid.BeforeCellUpdate
        ' select validation routine
        Select Case e.Cell.Column.Key

            Case "Vendor_Tolerance"

                If ValidatePercent(e) = False Then

                    c_cellBadCell = e.Cell

                End If

            Case "Vendor_Tolerance_Amount"

                If ValidateAmount(e) = False Then

                    c_cellBadCell = e.Cell

                End If

            Case "Store_Name"


                If ValidateID(e, ToleranceTypes.Store) = False Then

                    c_cellBadCell = e.Cell

                End If
        End Select
    End Sub

    Private Sub StoreOverrideGrid_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles StoreOverrideGrid.InitializeLayout

        Dim dd_store As New UltraDropDown
        ' ******** BIND THE DROPDOWN TO THE DATA TABLE ************************************
        dd_store.SetDataBinding(Stores, Nothing)
        dd_store.ValueMember = "Store_No"
        dd_store.DisplayMember = "Store_Name"
        dd_store.DisplayLayout.Bands(0).Columns(0).Hidden = True
        ' *********************************************************************************

        e.Layout.Bands(0).Columns.Item(0).Hidden = True
        With e.Layout.Bands(0).Columns.Item(1)
            .Header.Caption = "Store"
            .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate
            .ValueList = dd_store
            .Width = 195
            .CellActivation = Activation.NoEdit
        End With
        e.Layout.Bands(0).Columns.Item(2).Header.Caption = "Percent"
        e.Layout.Bands(0).Columns.Item(2).Width = 77
        e.Layout.Bands(0).Columns.Item(3).Header.Caption = "Amount"
        e.Layout.Bands(0).Columns.Item(3).Width = 77
        e.Layout.Bands(0).Columns.Item(4).Hidden = True
        e.Layout.Bands(0).Columns.Item(5).Hidden = True

    End Sub

    Private Sub VendorOverrideGrid_BeforeCellDeactivate(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles VendorOverrideGrid.BeforeCellDeactivate

        If Not c_cellBadCell Is Nothing Then

            e.Cancel = True
            VendorOverrideGrid.ActiveCell = c_cellBadCell
            VendorOverrideGrid.PerformAction( _
            Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode _
            , False _
            , False)

            c_cellBadCell = Nothing

        End If

    End Sub

    Private Sub VendorOverrideGrid_BeforeCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles VendorOverrideGrid.BeforeCellUpdate
        ' select validation routine
        Select Case e.Cell.Column.Key

            Case "Vendor_Tolerance"

                If ValidatePercent(e) = False Then

                    c_cellBadCell = e.Cell

                End If

            Case "Vendor_Tolerance_Amount"

                If ValidateAmount(e) = False Then

                    c_cellBadCell = e.Cell

                End If

            Case "Vendor_Name"


                If ValidateID(e, ToleranceTypes.Vendor) = False Then

                    c_cellBadCell = e.Cell

                End If

        End Select
    End Sub
    Private Sub VendorOverrideGrid_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles VendorOverrideGrid.InitializeLayout

        Dim dd_vendor As New UltraDropDown
        ' ******** BIND THE DROPDOWN TO THE DATA TABLE ************************************
        dd_vendor.DataSource = Vendors
        dd_vendor.ValueMember = "Vendor_ID"
        dd_vendor.DisplayMember = "CompanyName"
        dd_vendor.DisplayLayout.Bands(0).Columns(0).Hidden = True
        ' *********************************************************************************
        e.Layout.Bands(0).Columns.Item(0).Hidden = True
        With e.Layout.Bands(0).Columns.Item(1)
            .Header.Caption = "Vendor"
            .Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate
            e.Layout.Bands(0).Columns.Item(1).ValueList = dd_vendor
            .Width = 195
            .CellActivation = Activation.NoEdit
        End With
        e.Layout.Bands(0).Columns.Item(2).Header.Caption = "Percent"
        e.Layout.Bands(0).Columns.Item(2).Width = 77
        e.Layout.Bands(0).Columns.Item(3).Header.Caption = "Amount"
        e.Layout.Bands(0).Columns.Item(3).Width = 77
        e.Layout.Bands(0).Columns.Item(4).Hidden = True
        e.Layout.Bands(0).Columns.Item(5).Hidden = True

    End Sub


#End Region

End Class