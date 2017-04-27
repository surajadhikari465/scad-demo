Imports System.IO
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Object
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.TagPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.TagPush.Writers
Imports WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.TagPush.Controller

    ''' <summary>
    ''' POSProcessor defines the base class for delivering a type of change to the POS
    ''' system for each store.  This class is subclassed to provide processing for each 
    ''' ChangeType.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class TagProcessor
        'Date used for all calls to stored procedures
        Protected batchId As Integer
        Protected storeUpdates As Hashtable = New Hashtable
        Protected storeTagFTPs As Hashtable = New Hashtable
        Protected itemTagBOs As Hashtable = New Hashtable

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="batchId"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal batchId As Integer)
            Logger.LogDebug("New entry: inDate=" & batchId, Me.GetType())
            Me.batchId = batchId
            Logger.LogDebug("New exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Reads the database records from IRMA for the type of change being handled by the subclass, 
        ''' adding the results to the POS Push file for each store.
        ''' </summary>
        ''' <param name="batchId"></param>
        ''' <param name="itemList"></param>
        ''' <param name="itemListSeparator"></param>
        ''' <param name="startLabelPosition"></param>
        ''' <param name="store_no"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub GetPriceBatchTagDataFromIRMA(ByVal itemList As String, ByVal itemListSeparator As Char, ByVal batchId As Integer, ByVal startLabelPosition As Integer, ByVal store_no As Integer)

        ''' <summary>
        ''' Reads the database records from IRMA for the type of change being handled by the subclass, 
        ''' adding the results to the POS Push file for each store.
        ''' </summary>
        ''' <param name="itemList"></param>
        ''' <param name="itemListSeparator"></param>
        ''' <param name="startLabelPosition"></param>
        ''' <param name="store_no"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub GetReprintTagDataFromIRMA(ByVal itemList As String, ByVal itemListSeparator As Char, ByVal startLabelPosition As Integer, ByVal store_no As Integer, ByVal blnSortTags As Boolean)

        ''' <summary>
        ''' Reads the database records from IRMA for the type of change being handled by the subclass, 
        ''' adding the results to the POS Push file for each store.
        ''' </summary>
        ''' <param name="itemList"></param>
        ''' <param name="itemListSeparator"></param>
        ''' <param name="store_no"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub GetPlanogramTagDataFromIRMA(ByVal itemList As String, ByVal itemListSeparator As Char, ByVal store_no As Integer, ByVal isRegular As Boolean, ByVal startDate As Date)
        'DaveStacey 20070717 - rolling in shelftag fixes from 2.4.0 launch - this one was problematic 
        'due to the fact that the date picked on the form wasn't actually sent to the query 
        ' so i'm stringing the date value through here
        ''' <summary>
        ''' Processes a result set, adding the records to the POS Push file the store associated with
        ''' the change.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <param name="chgType"></param>
        ''' <remarks></remarks>
        Public Sub WriteResultsToFile(ByRef storeBO As StoreUpdatesBO, ByRef results As SqlDataReader, ByVal chgType As ChangeType)
            Logger.LogDebug("WriteResultsToFile entry: changeType=" + chgType.ToString(), Me.GetType())
            Dim currentPosWriter As TagWriter = Nothing
            Dim currentStoreNum As Integer = -1
            Dim currentBatchID As Integer = -1
            Dim headerInfo As POSBatchHeaderBO = Nothing
            Dim footerInfo As POSBatchFooterBO = Nothing
            Dim isBatchChange As Boolean = False

            'manage store config errors
            Dim storeConfigErrorMsg As New StringBuilder
            Dim configErrorStores As New Hashtable

            currentPosWriter = CType(storeBO.FileWriter, TagWriter)

            ' results contains an entry for each change record that should be added to the POS Push file
            Logger.LogDebug("adding detail records to the TAG push file", Me.GetType())
            ' get the store # for this record
            currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))

            ' Append this type of change to the POS file for the store
            currentPosWriter.AddRecordToFile(chgType, storeBO, results)

            ' track num of lines written to file
            currentPosWriter.RecordCount += 1

            'send message about store config exceptions
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("WriteResultsToFile exit", Me.GetType())
        End Sub

        Public Sub WriteElectronicResultsToFile(ByRef storeBO As StoreUpdatesBO, ByRef results As SqlDataReader, ByVal chgType As ChangeType)
            Logger.LogDebug("WriteElectronicResultsToFile entry: changeType=" + chgType.ToString(), Me.GetType())
            Dim currentPosWriter As ElectronicShelfTagWriter = Nothing
            Dim currentStoreNum As Integer = -1
            Dim currentBatchID As Integer = -1
            Dim headerInfo As POSBatchHeaderBO = Nothing
            Dim footerInfo As POSBatchFooterBO = Nothing
            Dim isBatchChange As Boolean = False

            'manage store config errors
            Dim storeConfigErrorMsg As New StringBuilder
            Dim configErrorStores As New Hashtable

            currentPosWriter = CType(storeBO.FileWriter, ElectronicShelfTagWriter)

            ' results contains an entry for each change record that should be added to the POS Push file
            Logger.LogDebug("adding detail records to the TAG push file", Me.GetType())
            ' get the store # for this record
            currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))

            ' Append this type of change to the POS file for the store
            currentPosWriter.AddRecordToFile(chgType, storeBO, results)

            ' track num of lines written to file
            currentPosWriter.RecordCount += 1

            'send message about store config exceptions
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("WriteElectronicResultsToFile exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Processes a result set, adding the records to the POS Push file the store associated with
        ''' the change.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <param name="chgType"></param>
        ''' <remarks></remarks>
        Public Sub WriteExemptTagResultsToFile(ByRef storeBO As StoreUpdatesBO, ByRef results As SqlDataReader, ByVal chgType As ChangeType)
            Logger.LogDebug("WriteExemptTagResultsToFile entry: changeType=" + chgType.ToString(), Me.GetType())
            Dim currentPosWriter As TagWriter = Nothing
            Dim currentStoreNum As Integer = -1
            Dim currentBatchID As Integer = -1
            Dim headerInfo As POSBatchHeaderBO = Nothing
            Dim footerInfo As POSBatchFooterBO = Nothing
            Dim isBatchChange As Boolean = False

            'manage store config errors
            Dim storeConfigErrorMsg As New StringBuilder
            Dim configErrorStores As New Hashtable

            currentPosWriter = CType(storeBO.ExemptFileWriter, TagWriter)
            ' results contains an entry for each change record that should be added to the POS Push file
            Logger.LogDebug("adding detail records to the TAG push file", Me.GetType())
            ' get the store # for this record
            currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))

            ' Append this type of change to the POS file for the store
            currentPosWriter.AddRecordToFile(chgType, storeBO, results)

            ' track num of lines written to file
            currentPosWriter.RecordCount += 1

            'send message about store config exceptions
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("WriteResultsToFile exit", Me.GetType())
        End Sub

        Public Sub WriteExemptElectronicShelfTagResultsToFile(ByRef storeBO As StoreUpdatesBO, ByRef results As SqlDataReader, ByVal chgType As ChangeType)
            Logger.LogDebug("WriteExemptElectronicShelfTagResultsToFile entry: changeType=" + chgType.ToString(), Me.GetType())
            Dim currentPosWriter As ElectronicShelfTagWriter = Nothing
            Dim currentStoreNum As Integer = -1
            Dim currentBatchID As Integer = -1
            Dim headerInfo As POSBatchHeaderBO = Nothing
            Dim footerInfo As POSBatchFooterBO = Nothing
            Dim isBatchChange As Boolean = False

            'manage store config errors
            Dim storeConfigErrorMsg As New StringBuilder
            Dim configErrorStores As New Hashtable

            currentPosWriter = CType(storeBO.ExemptFileWriter, ElectronicShelfTagWriter)
            ' results contains an entry for each change record that should be added to the POS Push file
            Logger.LogDebug("adding detail records to the TAG push file", Me.GetType())
            ' get the store # for this record
            currentStoreNum = results.GetInt32(results.GetOrdinal("Store_No"))

            ' Append this type of change to the POS file for the store
            currentPosWriter.AddRecordToFile(chgType, storeBO, results)

            ' track num of lines written to file
            currentPosWriter.RecordCount += 1

            'send message about store config exceptions
            If storeConfigErrorMsg.Length > 0 Then
                Dim args(1) As String
                args(0) = storeConfigErrorMsg.ToString
                ErrorHandler.ProcessError(ErrorType.POSPush_StoreConfig, args, SeverityLevel.Warning)
            End If

            Logger.LogDebug("WriteExemptElectronicShelfTagResultsToFile exit", Me.GetType())
        End Sub
    End Class

End Namespace

