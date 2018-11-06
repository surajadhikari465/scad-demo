Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.TagPush.Controller
Imports WholeFoods.IRMA.Replenishment.TagPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.TagPush.TagException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess


' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.TagPush.Writers
    ''' <summary>
    ''' POSWriter defines the base class for creating a POS Push file.
    ''' This class is subclassed to provide processing for each WriterType.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TagWriter
        Inherits BaseWriter

        ' POSFilename is the name of the file placed on the FTP server
        Private _POSFilename As String = "HTMAINT"
        Private _outputFileFormat As FileFormat
        Private _exemptTagFile As Boolean
        Private _exemptTagFileName As String = "DEFAULT"

#Region "Writer Constructors"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="FileWriterKey"></param>
        ''' <exception cref="TagWriterException" />
        Public Sub New(ByVal fileWriterKey As Integer)
            Logger.LogDebug("Init entry: FileWriterKey=" & fileWriterKey.ToString(), Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim currentElement As POSDataElementBO = Nothing

            Try
                ' Execute the stored procedure - use each row returned in the result set to 
                ' initialize a POSDataElement object
                _posFileWriterKey = fileWriterKey

                results = TagWriterDAO.GetTagWriterFileConfigData(fileWriterKey)

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

#Region "Property Definitions"
        Public Overrides Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
            Get
                If currentStore.RemoteFileName Is Nothing Then
                    Return _POSFilename
                Else
                    Return currentStore.RemoteFileName
                End If
            End Get
            Set(ByVal value As String)
                _POSFilename = value
            End Set
        End Property
        Public Overrides Property OutputFileFormat() As FileFormat
            Get
                Return _outputFileFormat
            End Get
            Set(ByVal value As FileFormat)
                _outputFileFormat = value
            End Set
        End Property
        Public Overrides Property ExemptTagFileName(ByVal currentStore As StoreUpdatesBO) As String
            Get
                If currentStore.RemoteExemptTagFileName Is Nothing Then
                    Return _exemptTagFileName
                Else
                    Return currentStore.RemoteExemptTagFileName
                End If
            End Get
            Set(ByVal value As String)
                _exemptTagFileName = value
            End Set
        End Property
        Public Property ExemptTagFile() As Boolean
            Get
                Return _exemptTagFile
            End Get
            Set(ByVal value As Boolean)
                _exemptTagFile = value
            End Set
        End Property
#End Region

#Region "Extended Tag Methods"
        Public Overridable Sub AddSubTeamRecordToFile(ByVal filename As String, ByRef itemRec As SqlDataReader, ByVal exemptTagFile As Boolean)
        End Sub
        Public Overridable Sub AddPlanogramHeaderToFile(ByVal filename As String, ByRef itemRec As SqlDataReader)
        End Sub
        Public Overridable Sub AddPlanogramFooterToFile(ByVal recCount As Integer)
        End Sub
#End Region

#Region "OverRide Property"
#End Region
#Region "Exceptions"
        ''' <summary>
        ''' Log an error and throw a new POSWriterException.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="innerException"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)
            Dim newException As TagWriterException
            If innerException IsNot Nothing Then
                Logger.LogError(message, Me.GetType(), innerException)
                newException = New TagWriterException(message, innerException)
            Else
                Logger.LogError(message, Me.GetType())
                newException = New TagWriterException(message)
            End If

            ' Throw the exception
            Throw newException
        End Sub
#End Region

    End Class

End Namespace

