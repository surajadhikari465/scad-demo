Imports System.Windows.Forms
Public Class Messages

    Public Shared QTY_WARNING_EXCEEDS_100 As String = "Quantity exceeds 100."
    Public Shared QTY_ERROR As String = "Invalid quantity.  Cannot exceed 32,767."
    Public Shared NULL_SERVICEURI As String = "No Service URI info available.  Please contact your Regional IT IRMA support."
    Public Shared NULL_USERNAME As String = "No username info available.  Please contact your Regional IT IRMA support."
    Public Shared NULL_REGION As String = "No region info available.  Please contact your Regional IT IRMA support."
    Public Shared NULL_STORE As String = "Your store location is not set, which is required by your current security permissions.  Please contact your Regional IRMA Security Admin."
    Public Shared NO_AUTH As String = "You are not authorized to use this application for this region.  Please contact your Regional IRMA Security Admin."
    Public Shared NO_ACCTAUTH As String = "Your IRMA account is disabled.  Please contact your Regional IRMA Security Admin."
    Public Shared NO_ROLEAUTH As String = "Your IRMA security roles do not allow you to use this option. Please contact your Regional IRMA Security Admin."
    Public Shared NO_HHTYPE As String = "Handheld type not specified.  Please contact your Regional IT IRMA support."
    Public Shared MERROR As String = "Error"
    Public Shared INVALID_FILE_SELECTION As String = "Invalid Selection"
    Public Shared INVALID_ITEM_SUBTEAM As String = "This is a {0} item and you are shrinking it to {1}.  Are you sure you want to do this?"
    Public Shared DELETE_SESSION As String = "Would you like to delete the current session?"
    Public Shared DELETE_ORDER As String = "Would you like to delete the current order?"
    Public Shared SUPRESS_SUBTEAM_EXCEPTION As String = "Do this for all items in this session?"
    Public Shared SUPRESS_SUBTEAM_EXCEPTION_TITLE As String = "Warning Override"
    Public Shared FIXED_SPOILAGE As String = "{0} is a FIXED SPOILAGE subteam and cannot accept manual shrink adjustments."
    Public Shared EXPENSE_SUBTEAM As String = "{0} is an EXPENSE subteam. You can't place an order with EXPENSE subteam."
    Public Shared PACKAGING_SUBTEAM As String = "{0} is a PACKAGING subteam. You can't place an order with PACKAGING subteam."
    Public Shared OTHER_SUPPLIES_SUBTEAM As String = "{0} is a Supplies subteam."
    Public Shared SUBTEAM_RESTRICTED As String = "This is a {0} item and you cannot shrink it to {1}.  {1} is restricted to same-subteam items."
    Public Shared SUBTEAM_MISSING_GLACCT As String = "This item cannot be transferred because it's from subteam {0}, which doesn't have a GL {1} Account set up yet."
    Public Shared INVALID_SUBTEAM_TITLE As String = "Subteam Mismatch"
    Public Shared INVALID_SHRINK_SUBTEAM As String = "{0} cannot accept manual shrink adjustments due to its subteam type."
    Public Shared INVALID_ReceiveDocument_SUBTEAM As String = "This is a {0} item and cannot be scanned to {1}.  {1} is restricted to same-subteam items."
    Public Shared NULL_PO As String = "No PO number was input prior to searching.  Please try again."
    Public Shared NULL_UPC As String = "No UPC number was input prior to searching.  Please try again."
    Public Shared NEG_AMT As String = "Please input a positive amount."
    Public Shared POSTIVE_WEIGHT_AMT As String = "Weight amount must be greater than .01"
    Public Shared POSTIVE_QUANTITY_AMT As String = "Please enter a quantity of at least 1."
    Public Shared EXCESS_WEIGHT_AMT As String = "Weight amount must be less than 9999.99"
    Public Shared EXCESS_QUANTITY_AMT As String = "Quantity amount must be less than 9999."
    Public Shared SOAP_ERROR As String = "No internet or network connection is available.  Please see your store technology specialist."
    Public Shared QUANTITY_DISCREPANCY As String = "Quantity discrepancy between PO and eInvoice.  Please assign a reason code to all mismatched items."
    Public Shared INVOICE_NUMBER_MISSING As String = "Please enter the invoice number."
    Public Shared INVOICE_DATE_MISSING As String = "Please enter the invoice date."
    Public Shared INVOICE_TOTAL_MISSING As String = "Please enter the invoice total."
    Public Shared DOCUMENT_NUMBER_MISSING As String = "Please enter the vendor document number."
    Public Shared INVOICE_ZERO As String = "Cost is zero." & vbCrLf & "Close order anyway?"
    Public Shared NOT_ALL_ITEMS_RECEIVED As String = "Not all items have been received." & vbCrLf & "Do you want to continue?"
    Public Shared CLOSE_ORDER As String = "Close this order?        "
    Public Shared REOPEN_ORDER As String = "Re-open this order?"
    Public Shared COMM_EXCEPTION As String = "Service communication failed in method: "
    Public Shared TIMEOUT_EXCEPTION As String = "Service timeout occurred in method: "
    Public Shared PLEASE_RETRY As String = "Please retry your request.  If the problem persists, please contact support."

    Public Sub exceptionMessage(ByVal message As String)
        MessageBox.Show(message + "occurred.  Please contact support.", Messages.MERROR, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub invalidSavedOrderChoiceException()
        MessageBox.Show("Not a valid saved file.  Please try again.", Messages.INVALID_FILE_SELECTION, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub WebException()
        MessageBox.Show("Unable to get network connection.  Please contact support.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub LoadException()
        MessageBox.Show("Unable to load data.  Please contact support.", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub ItemNotFound()
        MessageBox.Show("Item not found.", "Invalid Item", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
    End Sub
    Public Shared Sub ZeroCostItem()
        MessageBox.Show("The cost for this item is 0.00, or it could not be determined. If this is incorrect, please enter the Adjusted Cost for the item.", "Item with Zero Cost", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
        'MessageBox.Show("Item cost cannot be determined.  Please manually enter a Vendor Cost.", "Item with Zero Cost", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
    End Sub
    Public Shared Sub AdjustedCostReasonMissing()
        MessageBox.Show("An adjusted cost needs to be entered with a reason.  Please select a reason code.", "Missing Adjusted Cost Reason", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub ItemNotInPO()
        MessageBox.Show("Item is not on the PO.", "Incorrect Item", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Function invalidItemSubteamException(ByVal SessionSubteam As String, ByVal ItemSubteam As String) As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question

        Dim response = MsgBox(String.Format(INVALID_ITEM_SUBTEAM, SessionSubteam, ItemSubteam), mystyle, INVALID_SUBTEAM_TITLE)

        Return response
    End Function

    Public Shared Function subteamRestricted(ByVal SessionSubteam As String, ByVal ItemSubteam As String) As MsgBoxResult
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.Information

        Dim response = MsgBox(String.Format(SUBTEAM_RESTRICTED, ItemSubteam, SessionSubteam), mystyle, INVALID_SUBTEAM_TITLE)

        Return response
    End Function

    Public Shared Function subteamRestrictedRD(ByVal SessionSubteam As String, ByVal ItemSubteam As String) As MsgBoxResult
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.Information

        Dim response = MsgBox(String.Format(INVALID_ReceiveDocument_SUBTEAM, ItemSubteam, SessionSubteam), mystyle, INVALID_SUBTEAM_TITLE)

        Return response
    End Function


    Public Shared Function supressSubteamExceptionWarning() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question

        Dim response = MsgBox(SUPRESS_SUBTEAM_EXCEPTION, mystyle, SUPRESS_SUBTEAM_EXCEPTION_TITLE)

        Return response
    End Function

    Public Shared Function subteamMissingGLAcct(ByVal SupplySubteamName As String, ByVal ProductType As String) As MsgBoxResult
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.Information

        Dim response = MsgBox(String.Format(SUBTEAM_MISSING_GLACCT, SupplySubteamName, ProductType), mystyle, INVALID_SUBTEAM_TITLE)

        Return response
    End Function

    Public Shared Function DeleteSession() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        Dim response = MsgBox(DELETE_SESSION, mystyle, "Delete Session")

        Return response
    End Function

    Public Shared Function DeleteOrder() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNoCancel Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        Dim response = MsgBox(DELETE_ORDER, mystyle, "Delete Order")

        Return response
    End Function

    Public Shared Function DeleteItemInQueue() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton1 Or MsgBoxStyle.Information
        Dim response = MsgBox("Item will be deleted from the queue. ", mystyle, "Delete Item")

        Return response
    End Function

    Public Shared Function NullItem() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        Dim response = MsgBox("Item could not be saved.  Would you still like to review?", mystyle, "Invalid Item")

        Return response
    End Function

    Public Shared Function UploadErrorContinue(ByVal upc As String, ByVal desc As String) As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question

        Dim response = MsgBox("Item " + upc + "(" + desc + ")" + " could not be uploaded.  Continue upload?. ", mystyle, "Upload Error")

        Return response
    End Function

    Public Shared Sub QtyNumberException()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("Quantity must be a valid number.", mystyle, "Invalid Qty")
    End Sub

    Public Shared Sub EmptyItem()
        MessageBox.Show("Null item data cannot be saved.  Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub ScannerNotAvailable()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical

        MsgBox("Handheld scanner not initialized.  Please contact support.", mystyle, "Error")
    End Sub

    Public Shared Sub ShowException(ByVal exp As Exception)
        MessageBox.Show(exp.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub ShrinkItemUploadError()
        MessageBox.Show("Shrink item(s) not uploaded.  Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
    End Sub

    Public Shared Sub WrongStoreOrderError(ByVal sPONumber As String, ByVal sStore As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("PO " & sPONumber & " is for " & sStore.Trim() & ".  Please try again.", mystyle, "Error")
    End Sub

    Public Shared Sub OrderAlreadyClosedError(ByVal sPONumber As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("PO " & sPONumber & " is already closed.  To review or re-open a closed order, please use the IRMA client.", mystyle, "Error")
    End Sub

    Public Shared Sub OrderNotSentError(ByVal sPONumber As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("PO " & sPONumber & " has not yet been sent.  Please try again.", mystyle, "Error")
    End Sub

    Public Shared Sub DeletedOrderError(ByVal sPONumber As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("PO " & sPONumber & " has been deleted.  You may review this order in the IRMA client.", mystyle, "Error")
    End Sub

    Public Shared Sub OrderNotForStore(ByVal sPONumber As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("PO " & sPONumber & "  is not for your selected store.  Please try again.", mystyle, "Error")
    End Sub

    Public Shared Sub OrderNotExist(ByVal sPONumber As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("PO " & sPONumber & " does not exist.  Please try again.", mystyle, "Error")
    End Sub

    Public Shared Sub ReceiveNumericErrorQty()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("Quantity Received must be a numeric value.  Please try again.", mystyle, "Error")
    End Sub

    Public Shared Sub ReceiveNumericErrorWeight()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("Weight Received must be a numeric value.  Please try again.", mystyle, "Error")
    End Sub

    Public Shared Sub CycleCountMasterNotFound()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Exclamation

        MsgBox("Cycle Count Master not found.", mystyle, "Error")
    End Sub

    Public Shared Sub InvalidOrderItem(ByVal sIdentifier As String, ByVal sSubTeam As String, Optional ByVal iReturnCode As Integer = 0)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        Select Case iReturnCode
            Case 1
                MsgBox("Item " & sIdentifier & " cannot be ordered because the RetailSale flag is not set.", mystyle, "Error")
            Case 2
                MsgBox("Item " & sIdentifier & " cannot be ordered because it is for subteam " & sSubTeam & ".", mystyle, "Error")
            Case 3
                MsgBox("Item " & sIdentifier & " cannot be ordered because the IsSellable flag is not set.", mystyle, "Error")
            Case Else
                MsgBox("Item " & sIdentifier & " cannot be ordered.", mystyle, "Error")
        End Select
    End Sub

    Public Shared Sub InvalidOrderItemCanInventory(ByVal sSubTeam As String, ByVal sIdentifier As String, ByVal itemDescription As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("Subteam " & sSubTeam & " cannot add item " & sIdentifier & " [" & itemDescription & "].", mystyle, "Error")
    End Sub
    Public Shared Sub InvalidItemForVendor(ByVal vendor As String, ByVal sIdentifier As String)
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("Vendor " & vendor & " does not supply item " & sIdentifier & ".", mystyle, "Error")
    End Sub

    Public Shared Function UPCScanned(ByVal upc As String, ByVal desc As String) As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        Dim response = MsgBox("Item " + upc + "(" + desc + ")" + " was already scanned.  Continue? ", mystyle, "UPC Already Scanned")

        Return response
    End Function

    Public Shared Sub CannotScanForDSDVendor()
        Dim mystyle = MsgBoxStyle.Information

        MsgBox("Cannot scan item for Guaranteed Sale Supplier.", mystyle, "Cannot Create Order")
    End Sub

    Public Shared Sub OrderSuspended()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Exclamation

        MsgBox("WARNING: The order was closed but is now in Suspended state." & vbCrLf & vbCrLf & _
               "**The invoice associated with this order will not be uploaded to PeopleSoft until the order has been approved.**", mystyle, _
               "Order Suspended")
    End Sub

    Public Shared Sub InvoiceTotalNegative()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information

        MsgBox("Invoice Total cannot be negative or zero.", mystyle, "Error")
    End Sub

    Public Shared Sub InvoiceNumberMissing()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information
        MsgBox(INVOICE_NUMBER_MISSING, mystyle, "Error")
    End Sub

    Public Shared Sub InvoiceTotalMissing()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information
        MsgBox(INVOICE_TOTAL_MISSING, mystyle, "Error")
    End Sub

    Public Shared Sub InvoiceDocumentDateMissing()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information
        MsgBox(INVOICE_DATE_MISSING, mystyle, "Error")
    End Sub

    Public Shared Sub DocumentNumberMissing()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information
        MsgBox(DOCUMENT_NUMBER_MISSING, mystyle, "Error")
    End Sub

    Public Shared Function InvoiceTotalZero() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        Dim response As MsgBoxResult = MsgBox(INVOICE_ZERO, mystyle, "Zero Invoice Cost")

        Return response
    End Function

    Public Shared Function NotAllItemsReceived() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        Dim response As MsgBoxResult = MsgBox(NOT_ALL_ITEMS_RECEIVED, mystyle, "Please Confirm")

        Return response
    End Function

    Public Shared Function CloseOrder(ByVal invoiceDate As Date, ByVal documentType As Enums.DocumentType) As MsgBoxResult
        Dim mystyle = MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        Dim response As MsgBoxResult

        If documentType = Enums.DocumentType.Invoice Or documentType = Enums.DocumentType.Other Then
            response = MsgBox(String.Format(" Invoice Date:" + Environment.NewLine + "    {0}" + Environment.NewLine + _
                                            Environment.NewLine + CLOSE_ORDER, invoiceDate.ToShortDateString()), mystyle, "Confirm Close Order")
        ElseIf documentType = Enums.DocumentType.None Then
            response = MsgBox("Close Order?", mystyle, "Confirm Close Order")
        End If

        Return response
    End Function

    Public Shared Function ReopenOrder() As MsgBoxResult
        Dim mystyle = MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question
        MsgBox(REOPEN_ORDER, mystyle, "Please Confirm")
    End Function

    Public Shared Sub UpdateNotSuccessful()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Exclamation
        MsgBox("The order was not updated.  Please try again.", mystyle, "Error")
    End Sub

    Public Shared Sub ReOpenOrderSuccessful()
        Dim mystyle = MsgBoxStyle.OkOnly Or MsgBoxStyle.DefaultButton1 Or MsgBoxStyle.Information
        MsgBox("The order was successfully re-opened.", mystyle, "Re-Open Order")
    End Sub

End Class