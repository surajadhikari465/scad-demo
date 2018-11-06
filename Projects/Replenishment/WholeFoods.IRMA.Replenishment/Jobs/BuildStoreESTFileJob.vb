Imports System.Configuration.ConfigurationSettings
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.IRMA.Replenishment.ScalePush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility.FTP
Imports WholeFoods.IRMA.Replenishment.TagPush.Controller
Imports WholeFoods.IRMA.Replenishment.TagPush.Writers
Imports WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.TagPush.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that performs the POS Audit Report generation.
    ''' </summary>
    ''' <remarks>
    ''' ASSUMPTION: this is NOT designed to be multi-threaded.  If process must become multi-threaded then 
    ''' file management in StoreUpdatesBO.vb should no longer remove file if temp file exists upon
    ''' process startup.
    ''' </remarks>
    Public Class BuildStoreESTFileJob
        ''' <summary>
        ''' Collection of StoreUpdatesBO objects to store the Scale Push configuration for each store.
        ''' </summary>
        ''' <remarks></remarks>
        Private _storeUpdates As Hashtable = New Hashtable
        Private _storeTagFTPs As Hashtable = New Hashtable
        Private _itemTagBOs As Hashtable = New Hashtable

        ''' <summary>
        ''' Contains a message describing the error condition if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorMessage As String

        ''' <summary>
        ''' Contains any exception caught during processing if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorException As Exception

        ''' <summary>
        ''' The date should be the same for all POS Push and Scale Push stored procedure calls to keep the
        ''' process consistent.  This is important if the job starts before midnight and completes after midnight.
        ''' </summary>
        ''' <remarks></remarks>
        Private _jobRunDate As Date = Now

        ''' <summary>
        ''' store to generate a full scale file for
        ''' </summary>
        ''' <remarks></remarks>
        Private _storeNo As Integer

#Region "Events raised by this job"
        ' These events are raised during key steps of the process so the U.I. can let the user know
        ' where in the process things are.
        Public Event ESTPushStarted()
        Public Event ESTReadStoreConfigurationData(ByVal NumStores As Integer)
        Public Event ESTReadScaleItemsForStore()
        Public Event ESTTransferFiles(ByVal FileStatus As String)
        Public Event ESTCompleteSuccess()
        Public Event ESTCompleteError()
#End Region

        ''' <summary>
        ''' Generates Scale Push files for all stores configured in the admin application.
        ''' The files contain data for ALL items available in the store, not just items that were
        ''' added/deleted/batched.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Public Function Main(iStoreNo As Integer) As Boolean
            Logger.LogDebug("Main entry", Me.GetType())
            ' Set the return flag
            Dim returnStatus As Boolean = True

            Dim results As SqlDataReader = Nothing
            Dim currentStoreUpdate As StoreUpdatesBO
            Dim currentStoreNum As Integer
            Dim currentStoreName As String = Nothing
            Dim currentTagExt As String = Nothing
            Dim exemptTagExt As String = Nothing
            Dim prev_tagType As Integer = 0
            Dim prev_tagType2 As Integer = 0
            Dim tagType As Integer = 0
            Dim tagType2 As Integer = 0
            Dim tagExt As String = Nothing
            Dim prevSBTNum As Integer = 0
            Dim subTeamNum As Integer = 0
            Dim RowNumber As Integer = 0
            Dim currentTagWriter As ElectronicShelfTagWriter = Nothing
            Dim exemptTagWriter As ElectronicShelfTagWriter = Nothing
            Dim itemKey As Integer
            Dim PBD_ID As Integer
            Dim tagBO As ShelfTagBO = Nothing
            Dim printExemptTags As Boolean = False
            Dim tagProc As New TagPriceBatchProcessor(0)

            Try
                RaiseEvent ESTPushStarted()

                _storeUpdates = StorePOSConfigDAO.GetElectronicShelfTagStoreConfigurations(Constants.FileWriterType_ELECTRONICSHELFTAG, iStoreNo.ToString)

                RaiseEvent ESTReadStoreConfigurationData(_storeUpdates.Count)

                results = TagWriterDAO.GetFullElectronicShelfTagFile(iStoreNo)

                RaiseEvent ESTReadScaleItemsForStore()

                While results.Read
                    ' get the store # for this record
                    currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))
                    subTeamNum = results.GetInt32(results.GetOrdinal("SubTeam_No"))

                    ' verify this store is configured in StorePOSConfig
                    If (_storeUpdates.ContainsKey(currentStoreNum)) Then
                        currentStoreUpdate = CType(_storeUpdates.Item(currentStoreNum), StoreUpdatesBO)
                        'currentTagWriter = CType(currentStoreUpdate.FileWriter, PrintLab_Writer)

                        tagType = results.GetInt32(results.GetOrdinal("ShelfTagTypeID"))

                        If (Not results.IsDBNull(results.GetOrdinal("ExemptTagTypeID"))) Then
                            tagType2 = results.GetInt32(results.GetOrdinal("ExemptTagTypeID"))
                        Else
                            tagType2 = 0
                        End If

                        If (prev_tagType2 <> 0) AndAlso (tagType2 <> prev_tagType2) Then
                            If exemptTagWriter IsNot Nothing Then
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType = tagType2) Then
                            tagType2 = 0
                        End If

                        itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                        PBD_ID = results.GetInt32(results.GetOrdinal("PriceBatchDetailID"))

                        If (prev_tagType <> 0) AndAlso (tagType <> prev_tagType) Then
                            If currentTagWriter IsNot Nothing Then
                                currentTagWriter.CloseTempFile()
                            End If
                            prevSBTNum = 0
                        End If
                        If (tagType <> prev_tagType) Then
                            currentStoreName = results.GetString(results.GetOrdinal("Store_Name"))
                            tagExt = results.GetString(results.GetOrdinal("tagExt"))
                            If tagType <> tagType2 Then
                                ' get the writer instance for the store
                                currentTagWriter = CType(currentStoreUpdate.FileWriter, ElectronicShelfTagWriter)
                                currentStoreUpdate.StoreName = currentStoreName
                                currentStoreUpdate.SetShelfTagTempFilenames(tagExt, "R")
                                currentStoreUpdate.ShelfTagFiles.Add(currentStoreUpdate.BatchFileName)
                            End If
                        End If
                        If printExemptTags Then
                            If (tagType2 > 0 AndAlso tagType2 <> prev_tagType2) Then
                                ' Set the Exempt Tag Writer and its file name
                                exemptTagExt = results.GetString(results.GetOrdinal("ExemptTagExt"))
                                currentStoreUpdate.ExemptFileWriter = currentStoreUpdate.FileWriter.Copy()
                                exemptTagWriter = CType(currentStoreUpdate.ExemptFileWriter, ElectronicShelfTagWriter)
                                exemptTagWriter.ExemptTagFile = True
                                currentStoreUpdate.StoreName = currentStoreName
                                currentStoreUpdate.SetExemptTagTempFilenames(exemptTagExt, "R")
                                currentStoreUpdate.ExemptShelfTagFiles.Add(currentStoreUpdate.ExemptTagFileName)
                                If subTeamNum = prevSBTNum Then
                                    If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                        exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                        exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                        exemptTagWriter.CloseTempFile()
                                    End If
                                End If
                            End If
                        End If
                        If subTeamNum <> prevSBTNum Then
                            If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                                currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                                currentTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.BatchFileName, results, False)
                                currentTagWriter.CloseTempFile()
                            End If
                            If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                                exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                                exemptTagWriter.AddSubTeamRecordToFile(currentStoreUpdate.ExemptTagFileName, results, True)
                                exemptTagWriter.CloseTempFile()
                            End If
                        End If
                        If (tagType2 > 0 AndAlso exemptTagWriter IsNot Nothing) Then
                            exemptTagWriter.OpenTempFile(currentStoreUpdate.ExemptTagFileName)
                            tagProc.WriteExemptElectronicShelfTagResultsToFile(currentStoreUpdate, results, ChangeType.ElectronicShelfTagChange)
                            exemptTagWriter.CloseTempFile()
                        End If
                        If currentTagWriter IsNot Nothing AndAlso tagType <> tagType2 Then
                            currentTagWriter.OpenTempFile(currentStoreUpdate.BatchFileName)
                            tagProc.WriteElectronicResultsToFile(currentStoreUpdate, results, ChangeType.ElectronicShelfTagChange)
                            currentTagWriter.CloseTempFile()
                        End If
                        If (Not _storeTagFTPs.ContainsKey(currentStoreUpdate.StoreNum)) Then
                            _storeTagFTPs.Add(currentStoreUpdate.StoreNum, currentStoreUpdate)
                        End If

                        tagBO = New ShelfTagBO(PBD_ID, itemKey, tagType, tagType2)

                        _itemTagBOs.Add(tagBO.ItemKey.ToString() + ":" + RowNumber.ToString(), tagBO)
                        RowNumber = RowNumber + 1

                        If tagType2 > 0 Then
                            prev_tagType2 = tagType2
                        End If
                        prev_tagType = tagType
                        prevSBTNum = subTeamNum
                    Else
                        'send message about exception
                        Dim args(1) As String
                        args(0) = currentStoreNum.ToString
                        ErrorHandler.ProcessError(ErrorType.POSPush_StoreNotFound, args, SeverityLevel.Warning)
                        Continue While
                    End If
                End While

                ' check if any stores require SCALE hosting
                If _storeUpdates.Keys.Count > 0 AndAlso returnStatus Then
                    ' Deliver the file to each of the stores
                    Dim transfer As New TransferWriterFiles
                    Dim transferSuccess As Boolean
                    Dim transferStores As String    ' the FTP class includes status for each store
                    transferSuccess = transfer.TransferStoreFiles(_storeUpdates)
                    transferStores = transfer.StoreList()

                    RaiseEvent ESTTransferFiles(transferStores)
                ElseIf returnStatus Then
                    ' there were not any stores to update, so scale push is considered a success
                    RaiseEvent ESTTransferFiles("no stores to update")
                    returnStatus = True
                End If
            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Me.GetType(), e)
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Fatal, e)
                returnStatus = False
                _errorMessage = e.Message()
                _errorException = e
            Catch e1 As Exception
                Logger.LogError("Exception: ", Me.GetType(), e1)
                ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Fatal, e1)
                _errorMessage = e1.Message()
                returnStatus = False
                _errorException = e1
            End Try

            If returnStatus Then
                RaiseEvent ESTCompleteSuccess()
            Else
                RaiseEvent ESTCompleteError()
            End If

            Logger.LogDebug("Main exit", Me.GetType())
            Return returnStatus
        End Function

        Public Property ErrorMessage() As String
            Get
                Return _errorMessage
            End Get
            Set(ByVal Value As String)
                _errorMessage = Value
            End Set
        End Property

        Public Property ErrorException() As Exception
            Get
                Return _errorException
            End Get
            Set(ByVal Value As Exception)
                _errorException = Value
            End Set
        End Property

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property
    End Class
End Namespace
