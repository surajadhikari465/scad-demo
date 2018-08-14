Imports System.Windows.Forms
Imports System
Imports System.Text
Imports System.Linq
Imports System.Data
Imports Microsoft.WindowsCE.Forms


Public Class Common

    Public Const ASCII_PIPE As Short = 124
    Public Const ASCII_PERIOD As Short = 46
    Public Const ASCII_BACKSPACE As Short = 8

    Public Shared Sub ToggleControlVisibility(ByVal ary As Array, ByVal ctlColl As Windows.Forms.Control.ControlCollection, Optional ByVal hide As Boolean = True)
        For Each ctl As Control In ctlColl
            Dim loc As Integer
            loc = Array.BinarySearch(ary, ctl.Name.ToString)

            If hide Then
                If loc >= 0 Or TypeOf ctl Is StatusBar Then
                    ctl.Visible = False
                Else
                    ctl.Visible = True
                End If
            Else
                If loc >= 0 Then
                    ctl.Visible = True
                Else
                    ctl.Visible = False
                End If
            End If
        Next
    End Sub

    Public Shared Function ScaleItemCheck(ByVal upc As String) As String
        Dim newUpc As String
        Dim charArray As Char() = {"0"c}
        Dim UPCAcode As String
        Dim EAN13code As String

        'trim only leading zeros
        upc = upc.TrimStart(charArray)

        'set output value
        newUpc = upc
        UPCAcode = newUpc.Substring(0, 1)
        EAN13code = newUpc.Substring(0, 2)

        'check if scale item and if so, format it to have 00000 at the end
        If (newUpc.Length = 11 And UPCAcode = "2") Or (newUpc.Length = 12 And EAN13code = "20") Then
            newUpc = newUpc.Remove(newUpc.Length - 5, 5)
            newUpc = newUpc.PadRight(upc.Length, "0")
        End If

        Return newUpc
    End Function

    ''' <summary>
    ''' Updates the OrderHeader and OrderInvoice tables before closing the order
    ''' </summary>
    ''' <param name="order">Order object</param>
    ''' <param name="session">Session object</param>
    ''' <returns>True or False for error handling</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateOrderBeforeClose(ByVal order As Order, ByVal session As Session) As Boolean

        Dim result As New WholeFoods.Mobile.IRMA.Result
        Dim success As Boolean

        ' Update OrderHeader and OrderInvoice information
        result = session.WebProxyClient.UpdateOrderBeforeClose(order.OrderHeader_ID, order.InvoiceNumber, order.InvoiceDate, _
                                                               order.InvoiceCost, order.VendorDocID, order.VendorDocDate, _
                                                               order.Transfer_To_SubTeam, order.PartialShipment)

        success = result.Status
        Return success

    End Function

    ''' <summary>
    ''' Closes the order and runs suspension logic
    ''' </summary>
    ''' <param name="order">Order object</param>
    ''' <param name="session">Session object</param>
    ''' <returns>Result object</returns>
    ''' <remarks>Returns result object so that both error handling and suspension can be captured</remarks>
    Public Shared Function CloseOrder(ByVal order As Order, ByVal session As Session) As Result

        Dim result As New WholeFoods.Mobile.IRMA.Result

        ' Close Order and run suspension logic
        result = session.WebProxyClient.CloseOrder(order.OrderHeader_ID, session.UserID)

        Return result

    End Function

    Public Shared Function GetReasonCodeID(ByVal reasonCodes As ReasonCode(), ByVal reasonCodeAbbreviation As String) As Integer

        ' Use the reason code abbreviation to get the reason code ID.
        Dim query = From code In reasonCodes _
                    Where code.ReasonCodeAbbreviation = reasonCodeAbbreviation _
                    Select code.ReasonCodeID

        Dim reasonCode As Integer
        If query.Count > 0 Then
            reasonCode = query.Single
        Else
            reasonCode = Nothing
        End If

        Return reasonCode

    End Function

    Public Shared Function GetReasonCodeAbbreviation(ByRef reasonCodes As ReasonCode(), ByVal reasonCodeID As Integer) As String
        ' Use the reason code ID to get the reason code abbreviation.
        Dim query = From code In reasonCodes _
                    Where code.ReasonCodeID = reasonCodeID _
                    Select code.ReasonCodeAbbreviation

        Dim reasonCodeAbbr As String
        If query.Count > 0 Then
            reasonCodeAbbr = query.Single
        Else
            reasonCodeAbbr = Nothing
        End If

        Return reasonCodeAbbr
    End Function

    Public Shared Function GetUpc(ByVal barcode As String) As String
        Dim upc As String = String.Empty
        Dim charArray As Char() = {"0"c}

        If (barcode.Length <= 13) Then
            upc = barcode.TrimStart(charArray)
        Else
            upc = ExtractUpcFromQrCode(barcode)
        End If

        Return upc

    End Function

    Private Shared Function ExtractUpcFromQrCode(ByVal barcode As String) As String
        Dim upc As String = String.Empty
        Dim startingPosition As Integer = 5
        Dim charArray As Char() = {"0"c}
        Dim startingPositionIfNoCharacters As Integer = 2
        Dim length As Integer = 13

        If (barcode.StartsWith("]C")) Then
            upc = barcode.Substring(startingPosition, length).TrimStart(charArray)
        Else
            upc = barcode.Substring(startingPositionIfNoCharacters, length).TrimStart(charArray)
        End If

        Return upc

    End Function

End Class