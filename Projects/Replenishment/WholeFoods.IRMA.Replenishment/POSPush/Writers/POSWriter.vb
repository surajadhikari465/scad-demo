Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.POSException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Enum FileFormat
    Text
    Binary
End Enum

' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.POSPush.Writers
    ''' <summary>
    ''' POSWriter defines the base class for creating a POS Push file.
    ''' This class is subclassed to provide processing for each WriterType.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class POSWriter
        Inherits BaseWriter

#Region "Writer Constructors"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="FileWriterKey"></param>
        ''' <exception cref="POSWriterException" />
        Public Sub New(ByVal fileWriterKey As Integer)
            Logger.LogDebug("Init entry: FileWriterKey=" & fileWriterKey.ToString(), Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim currentElement As POSDataElementBO = Nothing

            Try
                ' Execute the stored procedure - use each row returned in the result set to 
                ' initialize a POSDataElement object
                _posFileWriterKey = fileWriterKey

                results = POSWriterDAO.GetPOSWriterFileConfigData(fileWriterKey)

                While (results.Read())
                    currentElement = New POSDataElementBO
                    currentElement.PopulateFromPOSWriterFileConfig(results)

                    Select Case currentElement.ChangeType
                        Case ChangeType.ItemIdDelete
                            Me.ItemIdDeleteConfig.Add(currentElement)
                        Case ChangeType.ItemIdAdd
                            Me.ItemIdAddConfig.Add(currentElement)
                        Case ChangeType.ItemDataDelete
                            Me.ItemDataDeleteConfig.Add(currentElement)
                        Case ChangeType.ItemDataChange
                            Me.ItemDataChangeConfig.Add(currentElement)
                        Case ChangeType.PromoOffer
                            Me.PromoOfferConfig.Add(currentElement)
                        Case ChangeType.VendorIDAdd
                            Me.VendorIdAddConfig.Add(currentElement)
                        Case ChangeType.shelfTagChange
                            Me.ShelfTagChangeConfig.Add(currentElement)

                    End Select
                End While
            Catch e As DataFactoryException
                throwException("POSWriter could not be instantiated because of a database error", e)
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try
            Logger.LogDebug("Init exit", Me.GetType())
        End Sub
#End Region

        ''' <summary>
        ''' Log an error and throw a new POSWriterException.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="innerException"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)
            Dim newException As POSWriterException
            If innerException IsNot Nothing Then
                Logger.LogError(message, Me.GetType(), innerException)
                newException = New POSWriterException(message, innerException)
            Else
                Logger.LogError(message, Me.GetType())
                newException = New POSWriterException(message)
            End If

            ' Throw the exception
            Throw newException
        End Sub

    End Class

End Namespace

