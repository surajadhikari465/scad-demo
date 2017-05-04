Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.ScalePush.ScaleException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Writers
    ''' <summary>
    ''' POSWriter defines the base class for creating a SCALE Push file.
    ''' This class is subclassed to provide processing for each WriterType.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ScaleWriter
        Inherits BaseWriter

#Region "Writer Constructors"
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="FileWriterKey"></param>
        ''' <exception cref="ScaleWriterException" />
        Public Sub New(ByVal fileWriterKey As Integer)
            Logger.LogDebug("Init entry: FileWriterKey=" & fileWriterKey.ToString(), Me.GetType())
            Dim results As SqlDataReader = Nothing
            Dim currentElement As POSDataElementBO = Nothing

            Try
                ' Execute the stored procedure - use each row returned in the result set to 
                ' initialize a POSDataElement object
                _posFileWriterKey = fileWriterKey

                'scale writer data is stored in same table as the POSWriter data; 
                'this data access method is shared for both writer types
                results = POSWriterDAO.GetPOSWriterFileConfigData(fileWriterKey)

                While (results.Read())
                    currentElement = New POSDataElementBO
                    currentElement.PopulateFromPOSWriterFileConfig(results)

                    Select Case currentElement.ChangeType
                        Case ChangeType.CorpScaleItemChange
                            Me.CorpScaleItemChangeConfig.Add(currentElement)
                        Case ChangeType.CorpScaleItemIdAdd
                            Me.CorpScaleIdAddConfig.Add(currentElement)
                        Case ChangeType.CorpScaleItemIdDelete
                            Me.CorpScaleIdDeleteConfig.Add(currentElement)
                        Case ChangeType.ZoneScalePriceChange
                            Me.ZoneScalePriceChangeConfig.Add(currentElement)
                        Case ChangeType.ZoneScaleSmartXPriceChange
                            Me.ZoneScaleSmartXPriceChangeConfig.Add(currentElement)
                        Case ChangeType.ZoneScaleItemDelete
                            Me.ZoneScaleItemDeleteConfig.Add(currentElement)
                        Case ChangeType.NutriFact
                            Me.NutrifactChangeConfig.Add(currentElement)
                        Case ChangeType.ExtraText
                            Me.ExtraTextChangeConfig.Add(currentElement)
                    End Select
                End While

            Catch e As DataFactoryException
                throwException("ScaleWriter could not be instantiated because of a database error", e)
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
        ''' Log an error and throw a new ScaleWriterException.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="innerException"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)
            Dim newException As ScaleWriterException
            If innerException IsNot Nothing Then
                Logger.LogError(message, Me.GetType(), innerException)
                newException = New ScaleWriterException(message, innerException)
            Else
                Logger.LogError(message, Me.GetType())
                newException = New ScaleWriterException(message)
            End If

            ' Throw the exception
            Throw newException
        End Sub

        Public Overrides Property OutputFileFormat() As FileFormat
            Get

            End Get
            Set(ByVal value As FileFormat)

            End Set
        End Property

        Public Overrides Property WriterFilename(ByVal currentStore As POSPush.BusinessLogic.StoreUpdatesBO) As String
            Get
                Return String.Empty
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public Overridable ReadOnly Property CorpRecordsIncludePricing() As Boolean
            Get
                ' Flag set to true if the writer includes the zone pricing data with the corporate records
                Return False
            End Get
        End Property
    End Class

End Namespace