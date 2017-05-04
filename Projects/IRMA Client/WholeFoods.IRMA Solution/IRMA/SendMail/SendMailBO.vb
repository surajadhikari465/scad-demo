Imports WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.POHeaderBO
Imports WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic.SendOrdersBO
Imports WholeFoods.IRMA.Replenishment.SendOrders.DataAccess
Imports System.IO
Imports log4net

Namespace WholeFoods.Utility

    Public Class SendMailBO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Sub EmailPO(ByVal OrderHeader_ID As Integer)

            Dim _subject As String = String.Empty
            Dim _from As String = String.Empty

            Dim _fileName As String = String.Empty
            Dim logText As ArrayList = New ArrayList

            Dim POHeader As New POHeaderBO

            Try

                POHeader = SendOrdersDAO.GetPOHeader(OrderHeader_ID)

                logger.Info("PO #" & POHeader.OrderHeader_ID & " - Send to Email Server")
                logText.Add("PO #" & POHeader.OrderHeader_ID & " - Send to Email Server")

                _from = API.GetADUserInfo(Environment.UserName, "mail")

                If POHeader.OverrideTransmissionMethod Then
                    POHeader.Vendor_Email = POHeader.OverrideTransmissionTarget
                End If

                _fileName = SendOrdersBO.createHTMLFromXSL(POHeader)

                If My.Application.IsProduction Then
                    _subject = "Attention: " & POHeader.vendor.CompanyName & " *** Whole Foods Market Order *** Store: " & POHeader.StoreNo & " *** PO#: " & OrderHeader_ID
                Else
                    _subject = "TEST ORDER Attention: " & POHeader.vendor.CompanyName & " *** Whole Foods Market Order *** Store: " & POHeader.StoreNo & " *** PO#: " & OrderHeader_ID
                End If

                logger.Info("PO #" & OrderHeader_ID & " - Order HTML file created.")
                logText.Add("PO #" & OrderHeader_ID & " - Order HTML file created.")

                SendMailWithAttachment(POHeader.Vendor_Email, _subject, "", _fileName, _from, logText)

                SendOrdersDAO.UpdateOrderSentToEmailDate(OrderHeader_ID)
                logger.Info("PO #" & OrderHeader_ID & " - Order emailed successfully")

            Catch ex As Exception

                SendOrdersDAO.UpdateOrderCancelSend(OrderHeader_ID)

                logger.Info("PO #" & OrderHeader_ID & " - Order email FAILED")
                logger.Info(ex.Message)

                Throw ex

            Finally

                If File.Exists(_fileName) Then
                    Kill(_fileName)
                End If

            End Try

        End Sub

    End Class

End Namespace