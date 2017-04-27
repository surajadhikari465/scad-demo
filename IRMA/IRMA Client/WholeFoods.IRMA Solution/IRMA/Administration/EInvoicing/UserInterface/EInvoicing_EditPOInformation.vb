Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports WholeFoods.Utility
Imports Infragistics.Win.UltraWinGrid


Public Class EInvoicing_EditPOInformation

    Private _EInvoiceID As Integer
    Public Property EInvoiceId() As Integer
        Get
            Return _EInvoiceID
        End Get
        Set(ByVal value As Integer)
            _EInvoiceID = value
        End Set
    End Property

    Private _originalPONumber As String
    Public Property OriginalPONumber() As String
        Get
            Return _originalPONumber
        End Get
        Set(ByVal value As String)
            _originalPONumber = value
        End Set
    End Property

    Private _PSVendorId As String
    Public Property PSVendorId() As String
        Get
            Return _PSVendorId
        End Get
        Set(ByVal value As String)
            _PSVendorId = value
        End Set
    End Property

    Private _VendorName As String
    Public Property VendorName() As String
        Get
            Return _VendorName
        End Get
        Set(ByVal value As String)
            _VendorName = value
        End Set
    End Property

    Private Sub EInvoicing_EditPOInformation_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Label_OrignalPO.Text = String.Format("PO#: {0}", _originalPONumber.ToString())
        Label_VendorName.Text = String.Format("Vendor: {0}", _VendorName)
        TextBox_POSearch.Focus()

    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click

        Me.Close()

    End Sub

    Private Sub TextBox_POSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox_POSearch.KeyPress

        ' Tom Lux, v3.5.0, 7/23/2009
        ' Default or "accept" button for this form was the 'button_select' button, but this would cause admin to crash if you
        ' pressed <ENTER> before a search was performed and something was selected in the grid.
        ' This handles case when user presses <ENTER> in the PO-search field and performs the search.
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            e.Handled = True
            Button_Search.PerformClick()
        End If

    End Sub

    Private Sub POSearch_Rejected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MaskInputRejectedEventArgs) Handles TextBox_POSearch.MaskInputRejected

        If (Me.TextBox_POSearch.MaskFull) Then
            ToolTip1.ToolTipTitle = "Input Rejected - Too Much Data"
            ToolTip1.Show("You cannot enter any more data into the date field. Delete some characters in order to insert more data.", Me.TextBox_POSearch, 2000)
        Else
            ToolTip1.ToolTipTitle = "Input Rejected"
            ToolTip1.Show("You can only add numeric characters (0-9) into this date field.", Me.TextBox_POSearch, 2000)
        End If

    End Sub

    Private Sub Button_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Search.Click

        ' Tom Lux, v3.5.0, 7/23/2009
        ' Admin was crashing if spaces were entered between numbers in the PO-search text field, such as "12  34".
        ' So we're add a validation wrapper.
        Try
            Dim ponum As Long = CLng(TextBox_POSearch.Text.Trim())
            PerformSearch(TextBox_POSearch.Text.Trim)
        Catch ex As Exception
            ToolTip1.ToolTipTitle = "Invalid Search Criteria"
            ToolTip1.Show("You must enter a valid PO number.", Me.TextBox_POSearch, 2000)
        End Try

    End Sub

    Private Sub PerformSearch(ByVal NewPurchaseOrder As String)

        Dim da As EInvoicingDAO = New EInvoicingDAO
        With UltraGrid_ValidPurchaseOrders
            .DataSource = da.getPOsByPSVendor(_originalPONumber, NewPurchaseOrder, _PSVendorId)
            .DataBind()

            Label_ErrorMessage.Text = String.Empty
            Label_Information.Text = String.Empty

            FormatData()

            Select Case .DisplayLayout.Rows.Count
                Case 0
                    Label_ErrorMessage.Text = "No valid POs were found for that PO number."
                Case 1
                    Label_ErrorMessage.Text = "A single valid PO was found. Click Select to use that PO number."
                Case Is > 1
                    Label_ErrorMessage.Text = "More than one valid PO was found. Hightlight which one you wish to use and click Select."
            End Select

        End With

    End Sub

    Private Sub FormatData()

        With UltraGrid_ValidPurchaseOrders
            .DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns
            .DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
            .DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect
            If .Rows.Count > 0 Then
                .Rows(0).Selected = True
            End If
        End With

    End Sub

    Private Sub Button_Select_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Select.Click

        Dim newPO As String

        Dim row As UltraGridRow = UltraGrid_ValidPurchaseOrders.Selected.Rows(0)

        newPO = row.Cells("PO").Value

        Dim DAO As EInvoicingDAO = New EInvoicingDAO
        DAO.UpdateInvoicePOInformation(EInvoiceId, newPO, 0)
        DAO.Dispose()

        Me.Close()

    End Sub

    Private Sub UltraGrid_ValidPurchaseOrders_AfterSelectChange(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles UltraGrid_ValidPurchaseOrders.AfterSelectChange
        ' If e.Type Is GetType(UltraGridRow) Then
        Button_Select.Enabled = (UltraGrid_ValidPurchaseOrders.Selected.Rows.Count = 1)
        ' End If
    End Sub

End Class