Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.POSException
Imports WholeFoods.IRMA.Replenishment.POSPush.Writers
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic

    ''' <summary>
    ''' This class contains the POS Push configuration data for a single store.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StoreUpdatesBO

#Region "Property Definitions"
        ' The Store.Store_No to identify which store the changes are associated with.
        Private _StoreNum As Integer
        Private _StoreName As String
        ' Last batch id number used for a batch sent to the POS system
        Private _BatchId As Integer
        ' Count for the # of items in the current batch being built
        Private _BatchRecords As Integer
        ' The type of FileWriter the store is configured to use.
        Private _FileWriter As BaseWriter
        ' The type of ExemptFileWriter the store is configured to use.
        Private _ExemptFileWriter As BaseWriter
        ' The type of host configuration (direct or sent) used to communicate with the store.
        Private _ConfigType As String
        ' Hashtable of POSBatchHeaderBO objects representing the PriceBatchHeader for the POS Deletes.
        Private _POSDeleteItemHeaders As New Hashtable
        ' Hashtable of POSBatchHeaderBO objects representing the POS Price Changes.
        Private _POSPriceChangeHeaders As New Hashtable
        ' Array of Identifier_IDs representing the item ids added.
        Private _POSItemIdAdds As New ArrayList
        ' Array of Identifier_IDs representing the item ids deleted.
        Private _POSItemIdDeletes As New ArrayList
        ' Hashtable of POSBatchHeaderBO objects representing the POS Promo Offers.
        Private _POSPromoOfferHeaders As New Hashtable
        ' Array of Vendor_IDs representing the vendor ids added.
        Private _POSVendorIdAdds As New ArrayList
        ' Array of StoreItemAuthorizationID values representing the de-auths sent to the POS.
        Private _POSItemDeAuthorizations As New ArrayList
        ' Array of Identifier_IDs representing the item refreshes sent to the scale.
        Private _POSItemRefreshes As New ArrayList

        Private _ScaleAuthorizations As New ArrayList
        ' Array of StoreItemAuthorizationID values representing the de-auths sent to the scale.
        Private _ScaleDeAuthorizations As New ArrayList
        Private _ShelfTagFiles As New ArrayList
        Private _ExemptShelfTagFiles As New ArrayList
        'Stores the tax information for the items assigned to the store. The _ItemTaxFlags Hashtable is keyed by item key.
        'Each entry is the _ItemTaxFlags Hashtable is a Hashtable of TaxFlagBO objects, keyed by _taxFlagKey.
        Private _ItemTaxFlags As New Hashtable
        ' Name of the file (including the file path) that contains the POS updates for the store
        Private _BatchFileName As String
        ' Name of the file with out the path for Remote File Transfer
        Private _RemoteFileName As String
        ' Name of the file (including the file path) that contains the POS updates for the store
        Private _ExemptTagFileName As String
        ' Name of the file with out the path for Remote File Transfer
        Private _RemoteExemptTagFileName As String
        ' Path of the file that contains the POS updates for the store
        Private _BatchFilePath As String
        ' Name of the control file (including the file path) that controls the POS updates on the store POS server
        Private _ControlFileName As String
        ' Path of the control file that controls the POS updates on the store POS server
        Private _ControlFilePath As String
        ' Flag that indicates if POSPush file was sent to store server successfully
        Private _changesDelivered As Boolean
        'holds FTP information (ip,user,pass,secure flag,port) for specific writer type
        Private _ftpInfo As StoreFTPConfigBO
        'Flag that indicates if a control file is generated successfully for IBM POS 
        Private _generatedControlFile As Boolean
#End Region

        ''' <summary>
        ''' Constructor - initialize the object with the results from the Store and StorePOSConfig tables
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        ''' <exception cref="POSStoreUpdateInitializationException" />
        Public Sub New(ByRef results As SqlDataReader)
            Logger.LogDebug("New entry", Me.GetType())
            Dim className As System.Type = Nothing
            Dim writerKey As Integer

            ' Assign values to the properties that identify the store and writer file configuration settings
            If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                _StoreNum = results.GetInt32(results.GetOrdinal("Store_No"))
            Else
                ' Store # is a required field
                throwException("Missing Store_No, a required field")
            End If
            If (Not results.IsDBNull(results.GetOrdinal("BatchID"))) Then
                _BatchId = results.GetInt32(results.GetOrdinal("BatchID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("BatchRecords"))) Then
                _BatchRecords = results.GetInt32(results.GetOrdinal("BatchRecords"))
            End If
            'If (Not results.IsDBNull(results.GetOrdinal("ConfigType"))) Then
            '    _ConfigType = results.GetString(results.GetOrdinal("ConfigType"))
            'End If

            Try
                'determine what the namespace should be based on the POSWriter.FileWriterType
                Dim writerNamespace As String = ""
                If results.GetString(results.GetOrdinal("FileWriterType")).Equals(Constants.FileWriterType_POS) Then
                    writerNamespace = "WholeFoods.IRMA.Replenishment.POSPush.Writers."
                ElseIf results.GetString(results.GetOrdinal("FileWriterType")).Equals(Constants.FileWriterType_SCALE) Then
                    writerNamespace = "WholeFoods.IRMA.Replenishment.ScalePush.Writers."
                ElseIf results.GetString(results.GetOrdinal("FileWriterType")).Equals(Constants.FileWriterType_TAG) Then
                    writerNamespace = "WholeFoods.IRMA.Replenishment.TagPush.Writers."
                ElseIf results.GetString(results.GetOrdinal("FileWriterType")).Equals(Constants.FileWriterType_ELECTRONICSHELFTAG) Then
                    writerNamespace = "WholeFoods.IRMA.Replenishment.TagPush.Writers."
                End If

                ' Use reflection to instantiate the POSWriter that is used to generate the files for this store
                ' Read the POSFileWriterKey from the results so the constructor for the writer can be called
                className = System.Type.GetType(writerNamespace + results.GetString(results.GetOrdinal("POSFileWriterClass")))
                writerKey = CType(results.GetInt32(results.GetOrdinal("POSFileWriterKey")), Integer)

                Dim params() As Object = {writerKey}

                _FileWriter = CType(Activator.CreateInstance(className, params), BaseWriter)

                ' Configure the _POSFileWriter properties
                If (Not results.IsDBNull(results.GetOrdinal("EnforceDictionary"))) Then
                    _FileWriter.EnforceDictionary = results.GetBoolean(results.GetOrdinal("EnforceDictionary"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("DelimChar"))) Then
                    _FileWriter.DelimChar = results.GetString(results.GetOrdinal("DelimChar"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FieldIdDelim"))) Then
                    _FileWriter.FieldIdDelim = results.GetBoolean(results.GetOrdinal("FieldIdDelim"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("TaxFlagTrueChar"))) Then
                    _FileWriter.TaxFlagTrueChar = CType(results.GetString(results.GetOrdinal("TaxFlagTrueChar")), Char)
                End If

                If (Not results.IsDBNull(results.GetOrdinal("TaxFlagFalseChar"))) Then
                    _FileWriter.TaxFlagFalseChar = CType(results.GetString(results.GetOrdinal("TaxFlagFalseChar")), Char)
                End If

                If (Not results.IsDBNull(results.GetOrdinal("FixedWidth"))) Then
                    _FileWriter.FixedWidth = results.GetBoolean(results.GetOrdinal("FixedWidth"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("LeadingDelim"))) Then
                    _FileWriter.LeadingDelim = results.GetBoolean(results.GetOrdinal("LeadingDelim"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("TrailingDelim"))) Then
                    _FileWriter.TrailingDelim = results.GetBoolean(results.GetOrdinal("TrailingDelim"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("AppendToFile"))) Then
                    _FileWriter.AppendToFile = results.GetBoolean(results.GetOrdinal("AppendToFile"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("OutputByIrmaBatches"))) Then
                    _FileWriter.OutputByIrmaBatches = results.GetBoolean(results.GetOrdinal("OutputByIrmaBatches"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("ScaleWriterType"))) Then
                    _FileWriter.ScaleWriterType = results.GetString(results.GetOrdinal("ScaleWriterType"))
                End If

                If (Not results.IsDBNull(results.GetOrdinal("EscapeCharCount"))) Then
                    _FileWriter.EscapeCharCount = results.GetInt32(results.GetOrdinal("EscapeCharCount"))
                End If

                'get escape chars if any
                If _FileWriter.EscapeCharCount > 0 Then
                    _FileWriter.EscapeChars = POSWriterDAO.GetPOSWriterEscapeChars(writerKey)
                End If

                _ExemptFileWriter = CType(Activator.CreateInstance(className, params), BaseWriter)
                
            Catch ex As Exception
                Logger.LogError("Error instantiating the class for the configured writer using reflection: POSFileWriter: " + className.ToString() + " for Store # " + _StoreNum.ToString(), Me.GetType, ex)
                throwException("Could not create an instance of the POSFileWriter: " + className.ToString() + " for Store # " + _StoreNum.ToString(), ex)
            End Try

            'Check for existence of temp file for current store and remove file if it exists.
            'File is removed because error likely occurred that aborted processing for current store.
            If Me.HasCurrentChanges Then
                File.Delete(Me.BatchFileName)
            End If

            Logger.LogDebug("New exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Sets the current store's Batch (POS or Scale) file names; creates the store_no directory if needed.
        ''' Also sets the current store's Control file name.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetTempFilenames()
            Logger.LogDebug("SetTempFilenames entry", Me.GetType())
            Dim filepath As String = String.Empty
            Dim tempBatchFilename As New StringBuilder
            Dim tempControlFilename As New StringBuilder
            Dim RegionAbbr As String

            RegionAbbr = ConfigurationServices.AppSettings("region")

            'create sub directory in temp location matching store number
            filepath = Path.Combine(ConfigurationServices.AppSettings("tempPOSFileDir"), (RegionAbbr + Me.StoreName))

            Me.BatchFilePath = filepath
            Me.ControlFilePath = filepath

            'check for existence of directory before adding store file
            If Not Directory.Exists(filepath) Then
                Directory.CreateDirectory(filepath)
            End If

            'if SCALE file then create temp file w/ scale writer type as part of name, since two files will be created
            If _FileWriter.ScaleWriterType IsNot Nothing Then
                tempBatchFilename.Append(_FileWriter.ScaleWriterType)
                tempBatchFilename.Append("_")
            End If

            tempBatchFilename.Append(Me.StoreNum.ToString)
            tempControlFilename.Append(Me.StoreNum.ToString).Append("_control")
            Select Case _FileWriter.OutputFileFormat
                Case FileFormat.Binary
                    tempBatchFilename.Append(".txt")
                    tempControlFilename.Append(".txt")
                Case FileFormat.Text
                    tempBatchFilename.Append(".txt")
                    tempControlFilename.Append(".txt")
            End Select

            'root dir\store_no\store_no.txt
            Me.BatchFileName = Path.Combine(filepath, tempBatchFilename.ToString)
            'root_dir\store_no\store_no_control.txt
            Me.ControlFileName = Path.Combine(filepath, tempControlFilename.ToString)
            Logger.LogDebug("SetTempFilenames exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Sets the current store's Batch (POS or Scale) file names; creates the store_no directory if needed.
        ''' Also sets the current store's Control file name.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetShelfTagTempFilenames(ByVal extension As String, ByVal suffixType As String, Optional ByVal batchId As String = Nothing)
            Logger.LogDebug("SetShelfTagTempFilenames entry", Me.GetType())
            Dim filepath As String
            Dim tempBatchFilename As New StringBuilder
            Dim tempControlFilename As New StringBuilder
            Dim RegionAbbr As String

            RegionAbbr = ConfigurationServices.AppSettings("region")

            'create sub directory in temp location matching store number
            filepath = Path.Combine(ConfigurationServices.AppSettings("tempPOSFileDir"), (RegionAbbr + Me.StoreName))

            Me.BatchFilePath = filepath
            Me.ControlFilePath = filepath

            'check for existence of directory before adding store file
            If Not Directory.Exists(filepath) Then
                Directory.CreateDirectory(filepath)
            End If

            tempBatchFilename.Append(suffixType)
            tempBatchFilename.Append("-st0")
            tempBatchFilename.Append(Me._StoreNum)

            'Adding the datetime at the end of the file name
            'prevents the appending functionality from
            'working properly.  
            If Not _FileWriter.AppendToFile Then
                tempBatchFilename.Append("_")
                tempBatchFilename.Append(Date.Now.Month.ToString("00"))
                tempBatchFilename.Append(Date.Now.Day.ToString("00"))
                tempBatchFilename.Append(Date.Now.Hour.ToString("00"))
                tempBatchFilename.Append(Date.Now.Minute.ToString("00"))
                tempBatchFilename.Append(Date.Now.Second.ToString("00"))
                If Not (batchId = Nothing) Then
                    tempBatchFilename.Append("_")
                    tempBatchFilename.Append(batchId)
                End If
            End If

            tempControlFilename.Append(suffixType)
            tempControlFilename.Append("-st0")
            tempControlFilename.Append(Me._StoreNum).Append("_")
            tempControlFilename.Append(Date.Now.Month.ToString("00"))
            tempControlFilename.Append(Date.Now.Day.ToString("00"))
            tempControlFilename.Append(Date.Now.Hour.ToString("00"))
            tempControlFilename.Append(Date.Now.Minute.ToString("00"))
            tempControlFilename.Append(Date.Now.Second.ToString("00"))
            If Not (batchId = Nothing) Then
                tempControlFilename.Append("_")
                tempControlFilename.Append(batchId)
            End If
            tempControlFilename.Append("_control")

            Select Case _FileWriter.OutputFileFormat
                Case FileFormat.Binary
                    tempBatchFilename.Append(".bin")
                    tempControlFilename.Append(".bin")
                Case FileFormat.Text
                    tempBatchFilename.Append(".")
                    tempBatchFilename.Append(extension)
                    tempControlFilename.Append(".")
                    tempControlFilename.Append(extension)
            End Select

            'Save the filename for remote file transfer
            Me.RemoteFileName = tempBatchFilename.ToString

            'root dir\store_no\store_no.txt
            Me.BatchFileName = Path.Combine(filepath, tempBatchFilename.ToString)
            'root_dir\store_no\store_no_control.txt
            Me.ControlFileName = Path.Combine(filepath, tempControlFilename.ToString)
            Logger.LogDebug("SetShelfTagTempFilenames exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Sets the current store's Batch (POS or Scale) file names; creates the store_no directory if needed.
        ''' Also sets the current store's Control file name.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetShelfTagRBXFilenames(ByRef planoCode As String, ByVal extension As String)
            Logger.LogDebug("SetShelfTagRBXFilenames entry", Me.GetType())
            Dim filepath As String
            Dim tempBatchFilename As New StringBuilder
            Dim tempControlFilename As New StringBuilder
            Dim RegionAbbr As String

            RegionAbbr = ConfigurationServices.AppSettings("region")

            'create sub directory in temp location matching store number
            filepath = Path.Combine(ConfigurationServices.AppSettings("tempPOSFileDir"), (RegionAbbr + Me.StoreName))

            Me.BatchFilePath = filepath
            Me.ControlFilePath = filepath

            'check for existence of directory before adding store file
            If Not Directory.Exists(filepath) Then
                Directory.CreateDirectory(filepath)
            End If

            'tempBatchFilename.Append(suffixType)
            tempBatchFilename.Append("HXplan")
            'tempBatchFilename.Append(Me._StoreNum)
            tempBatchFilename.Append(planoCode)


            'tempControlFilename.Append(suffixType)
            tempControlFilename.Append("HXplan")
            tempControlFilename.Append(planoCode)

            tempControlFilename.Append("_control")

            Select Case _FileWriter.OutputFileFormat
                Case FileFormat.Binary
                    tempBatchFilename.Append(".bin")
                    tempControlFilename.Append(".bin")
                Case FileFormat.Text
                    tempBatchFilename.Append(".")
                    tempBatchFilename.Append(extension)
                    tempControlFilename.Append(".")
                    tempControlFilename.Append(extension)
            End Select

            'Save the filename for remote file transfer
            Me.RemoteFileName = tempBatchFilename.ToString

            'root dir\store_no\store_no.txt
            Me.BatchFileName = Path.Combine(filepath, tempBatchFilename.ToString)
            'root_dir\store_no\store_no_control.txt
            Me.ControlFileName = Path.Combine(filepath, tempControlFilename.ToString)
            Logger.LogDebug("SetShelfTagRBXFilenames exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Sets the current store's Batch (POS or Scale) file names; creates the store_no directory if needed.
        ''' Also sets the current store's Control file name.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetExemptTagTempFilenames(ByVal extension As String, ByVal suffixType As String, Optional ByVal batchId As String = Nothing)
            Logger.LogDebug("SetExemptTagTempFilenames entry", Me.GetType())
            Dim filepath As String
            Dim tempBatchFilename As New StringBuilder
            Dim RegionAbbr As String

            RegionAbbr = ConfigurationServices.AppSettings("region")

            'create sub directory in temp location matching store number
            filepath = Path.Combine(ConfigurationServices.AppSettings("tempPOSFileDir"), (RegionAbbr + Me.StoreName))

            Me.BatchFilePath = filepath

            'check for existence of directory before adding store file
            If Not Directory.Exists(filepath) Then
                Directory.CreateDirectory(filepath)
            End If
            tempBatchFilename.Append(suffixType)
            tempBatchFilename.Append("-st0")
            tempBatchFilename.Append(Me._StoreNum).Append("_")
            tempBatchFilename.Append(Date.Now.Month.ToString("00"))
            tempBatchFilename.Append(Date.Now.Day.ToString("00"))
            tempBatchFilename.Append(Date.Now.Hour.ToString("00"))
            tempBatchFilename.Append(Date.Now.Minute.ToString("00"))
            tempBatchFilename.Append(Date.Now.Second.ToString("00"))
            If Not (batchId = Nothing) Then
                tempBatchFilename.Append("_")
                tempBatchFilename.Append(batchId)
            End If

            Select Case _FileWriter.OutputFileFormat
                Case FileFormat.Binary
                    tempBatchFilename.Append(".bin")
                Case FileFormat.Text
                    tempBatchFilename.Append(".")
                    tempBatchFilename.Append(extension)
            End Select
            'Save the filename for remote file transfer
            Me.RemoteExemptTagFileName = tempBatchFilename.ToString

            'root dir\store_no\store_no.txt
            Me.ExemptTagFileName = Path.Combine(filepath, tempBatchFilename.ToString)
            Logger.LogDebug("SetExemptTagTempFilenames exit", Me.GetType())
        End Sub
        ''' <summary>
        ''' Moves current store's file to an archive directory after renaming it with a timestamp
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ArchiveProcessedBatchFile()
            MoveProcessedFile(Me.BatchFileName, Me.BatchFilePath, Me.StoreNum.ToString)
        End Sub

        ''' <summary>
        ''' Moves current store's file to an archive directory after renaming it with a timestamp
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ArchiveProcessedControlFile()
            MoveProcessedFile(Me.ControlFileName, Me.ControlFilePath, String.Concat(Me.StoreNum, "_control"))
        End Sub

        ''' <summary>
        ''' Moves current store's file to an archive directory after renaming it with a timestamp
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub MoveProcessedFile(ByVal fileToArchive As String, ByVal filePath As String, ByVal newFileName As String)
            Logger.LogDebug("MoveProcessedFile entry", Me.GetType())
            Dim archiveDir As String = Path.Combine(filePath, "archive")

            'check for existence of directory before adding processed store file
            If Not Directory.Exists(archiveDir) Then
                Directory.CreateDirectory(archiveDir)
            End If

            'append timestamp to file for new file name
            Dim processedFileName As New StringBuilder
            processedFileName.Append(newFileName)
            processedFileName.Append("_")
            processedFileName.Append(Now.ToString("yyyyMMdd_HHmmss"))
            processedFileName.Append(Now.Millisecond.ToString("000"))
            Select Case _FileWriter.OutputFileFormat
                Case FileFormat.Binary
                    processedFileName.Append(".txt")
                Case FileFormat.Text
                    processedFileName.Append(".txt")
            End Select

            '<filepath>\archive\<filename>_timestamp.txt
            Logger.LogInfo("MoveProcessedFile - archiving file: fileToArchive=" + fileToArchive + ", archiveFile=" + Path.Combine(archiveDir, processedFileName.ToString), Me.GetType())
            File.Move(fileToArchive, Path.Combine(archiveDir, processedFileName.ToString))

            'cleanup old archived files if any
            CleanupOldFiles(archiveDir)
            Logger.LogDebug("MoveProcessedFile exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Removes old POSPush files older than a number of days secified in the app.config file.
        ''' Files are kept for x number of days for troubleshooting purposes.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CleanupOldFiles(ByVal archiveDir As String)
            Logger.LogDebug("CleanupOldFiles entry", Me.GetType())
            Dim removeFilesOlderThanDays As Integer

            If ConfigurationServices.AppSettings("numDaysToKeepPOSPushFiles") IsNot Nothing Then
                removeFilesOlderThanDays = Integer.Parse(ConfigurationServices.AppSettings("numDaysToKeepPOSPushFiles"))
            End If

            If removeFilesOlderThanDays > 0 Then
                Dim dirInfo As New DirectoryInfo(archiveDir)
                Dim fileList As FileInfo() = dirInfo.GetFiles()
                Dim fileInfo As FileInfo

                Dim currentDay As Date = Today
                Dim timespan As New TimeSpan(removeFilesOlderThanDays, 0, 0, 0)

                'get list of files in archiveDir
                For Each fileInfo In fileList

                    Logger.LogDebug("create time: " + fileInfo.CreationTime.ToString, Me.GetType)
                    Logger.LogDebug("today - 1  : " + currentDay.Subtract(timespan).ToString, Me.GetType)

                    If fileInfo.CreationTime < currentDay.Subtract(timespan) Then
                        'remove file
                        fileInfo.Delete()
                    End If
                Next
            End If
            Logger.LogDebug("CleanupOldFiles exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Checks for existence of local file w/ POSPush data for this store
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function HasCurrentChanges() As Boolean
            If File.Exists(Me.BatchFileName) Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Helper method to populate the _ItemTaxFlags hashtable for an item.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="taxFlagData"></param>
        ''' <remarks></remarks>
        Public Sub AddItemTaxFlagData(ByVal itemKey As Integer, ByVal taxFlagData As TaxFlagBO)
            Logger.LogDebug("AddItemTaxFlagData entry: itemKey=" + itemKey.ToString() + ", taxFlagKey=" + taxFlagData.TaxFlagKey, Me.GetType())
            Dim itemTaxValues As Hashtable

            ' Does the store hashtable already contain an entry for this item?
            If _ItemTaxFlags.ContainsKey(itemKey) Then
                itemTaxValues = CType(_ItemTaxFlags.Item(itemKey), Hashtable)
                ' Does the item hashtable already contain an entry for this tax flag?
                If itemTaxValues.ContainsKey(taxFlagData.TaxFlagKey) Then
                    ' The tax flag value for an item in a store is always the same - update the entry.
                    itemTaxValues.Item(taxFlagData.TaxFlagKey) = taxFlagData
                Else
                    ' Add the tax flag to the item hashtable.
                    itemTaxValues.Add(taxFlagData.TaxFlagKey, taxFlagData)
                End If
            Else
                ' Create the item hashtable.
                itemTaxValues = New Hashtable()
                itemTaxValues.Add(taxFlagData.TaxFlagKey, taxFlagData)

                ' Add the item to the store hashtable.
                _ItemTaxFlags.Add(itemKey, itemTaxValues)
            End If
            Logger.LogDebug("AddItemTaxFlagData exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Helper method to retrieve the TaxFlagBO for an item assigned to the store.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="taxFlagKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemTaxFlagData(ByVal itemKey As Integer, ByVal taxFlagKey As String) As TaxFlagBO
            Logger.LogDebug("GetItemTaxFlagData entry: itemKey=" + itemKey.ToString() + ", taxFlagKey=" + taxFlagKey, Me.GetType())
            Dim itemTaxValues As Hashtable
            Dim taxFlagValue As TaxFlagBO

            ' Get the item from the store hashtable
            If _ItemTaxFlags.ContainsKey(itemKey) Then
                itemTaxValues = CType(_ItemTaxFlags.Item(itemKey), Hashtable)
                ' Get the tax flag from the item hashtable
                If itemTaxValues.ContainsKey(taxFlagKey) Then
                    taxFlagValue = CType(itemTaxValues.Item(taxFlagKey), TaxFlagBO)
                Else
                    ' The current tax flag does not appear in the item list
                    taxFlagValue = Nothing
                End If
            Else
                ' The current item does not appear in the store list
                taxFlagValue = Nothing
            End If

            Logger.LogDebug("GetItemTaxFlagData exit", Me.GetType())
            Return taxFlagValue
        End Function

        ''' <summary>
        ''' Return a sum of the number of change records for all change types processed for the store.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTotalPOSRecordCount() As Integer
            Logger.LogDebug("GetTotalPOSRecordCount entry", Me.GetType())
            Dim recordCount As Integer = 0
            If Me.POSVendorIdAdds IsNot Nothing AndAlso Me.POSVendorIdAdds.Count > 0 Then
                recordCount += Me.POSVendorIdAdds.Count
            End If
            If Me.POSDeleteItemHeaders IsNot Nothing AndAlso Me.POSDeleteItemHeaders.Count > 0 Then
                recordCount += Me.POSDeleteItemHeaders.Count
            End If
            If Me.POSPriceChangeHeaders IsNot Nothing AndAlso Me.POSPriceChangeHeaders.Count > 0 Then
                recordCount += Me.POSPriceChangeHeaders.Count
            End If
            If Me.POSItemIdDeletes IsNot Nothing AndAlso Me.POSItemIdDeletes.Count > 0 Then
                recordCount += Me.POSItemIdDeletes.Count
            End If
            If Me.POSItemIdAdds IsNot Nothing AndAlso Me.POSItemIdAdds.Count > 0 Then
                recordCount += Me.POSItemIdAdds.Count
            End If
            If Me.POSPromoOfferHeaders IsNot Nothing AndAlso Me.POSPromoOfferHeaders.Count > 0 Then
                recordCount += Me.POSPromoOfferHeaders.Count
            End If
            Logger.LogDebug("GetTotalPOSRecordCount exit: " & recordCount.ToString, Me.GetType())
            Return recordCount
        End Function

        ''' <summary>
        ''' Log an error and throw a new POSStoreUpdateInitializationException.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="innerException"></param>
        ''' <remarks></remarks>
        Private Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)
            Dim newException As POSStoreUpdateInitializationException
            If innerException IsNot Nothing Then
                Logger.LogError(message, Me.GetType(), innerException)
                newException = New POSStoreUpdateInitializationException(message, innerException)
            Else
                Logger.LogError(message, Me.GetType())
                newException = New POSStoreUpdateInitializationException(message)
            End If

            ' Throw the exception
            Throw newException
        End Sub

#Region "Property access methods"
        Public Property StoreNum() As Integer
            Get
                Return _StoreNum
            End Get
            Set(ByVal Value As Integer)
                _StoreNum = Value
            End Set
        End Property

        Public Property StoreName() As String
            Get
                Return _StoreName
            End Get
            Set(ByVal Value As String)
                _StoreName = Value
            End Set
        End Property

        Public Property BatchID() As Integer
            Get
                Return _BatchId
            End Get
            Set(ByVal Value As Integer)
                _BatchId = Value
            End Set
        End Property

        Public Property BatchRecords() As Integer
            Get
                Return _BatchRecords
            End Get
            Set(ByVal Value As Integer)
                _BatchRecords = Value
            End Set
        End Property

        Public Property FileWriter() As BaseWriter
            Get
                Return _FileWriter
            End Get
            Set(ByVal Value As BaseWriter)
                _FileWriter = Value
            End Set
        End Property
        Public Property ExemptFileWriter() As BaseWriter
            Get
                Return _ExemptFileWriter
            End Get
            Set(ByVal Value As BaseWriter)
                _ExemptFileWriter = Value
            End Set
        End Property
        Public Property ConfigType() As String
            Get
                Return _ConfigType
            End Get
            Set(ByVal Value As String)
                _ConfigType = Value
            End Set
        End Property

        Public Property POSDeleteItemHeaders() As Hashtable
            Get
                Return _POSDeleteItemHeaders
            End Get
            Set(ByVal value As Hashtable)
                _POSDeleteItemHeaders = value
            End Set
        End Property

        Public Property POSPriceChangeHeaders() As Hashtable
            Get
                Return _POSPriceChangeHeaders
            End Get
            Set(ByVal value As Hashtable)
                _POSPriceChangeHeaders = value
            End Set
        End Property

        Public Property POSItemIdAdds() As ArrayList
            Get
                Return _POSItemIdAdds
            End Get
            Set(ByVal value As ArrayList)
                _POSItemIdAdds = value
            End Set
        End Property

        Public Property POSItemRefreshes() As ArrayList
            Get
                Return _POSItemRefreshes
            End Get
            Set(ByVal value As ArrayList)
                _POSItemRefreshes = value
            End Set
        End Property

        Public Property ShelfTagFiles() As ArrayList
            Get
                Return _ShelfTagFiles
            End Get
            Set(ByVal value As ArrayList)
                _ShelfTagFiles = value
            End Set
        End Property
        Public Property ExemptShelfTagFiles() As ArrayList
            Get
                Return _ExemptShelfTagFiles
            End Get
            Set(ByVal value As ArrayList)
                _ExemptShelfTagFiles = value
            End Set
        End Property


        Public Property POSItemIdDeletes() As ArrayList
            Get
                Return _POSItemIdDeletes
            End Get
            Set(ByVal value As ArrayList)
                _POSItemIdDeletes = value
            End Set
        End Property

        Public Property POSPromoOfferHeaders() As Hashtable
            Get
                Return _POSPromoOfferHeaders
            End Get
            Set(ByVal value As Hashtable)
                _POSPromoOfferHeaders = value
            End Set
        End Property

        Public Property POSVendorIdAdds() As ArrayList
            Get
                Return _POSVendorIdAdds
            End Get
            Set(ByVal value As ArrayList)
                _POSVendorIdAdds = value
            End Set
        End Property

        Public Property POSItemDeAuthorizations() As ArrayList
            Get
                Return _POSItemDeAuthorizations
            End Get
            Set(ByVal value As ArrayList)
                _POSItemDeAuthorizations = value
            End Set
        End Property

        Public Property ScaleAuthorizations() As ArrayList
            Get
                Return _ScaleAuthorizations
            End Get
            Set(ByVal value As ArrayList)
                _ScaleAuthorizations = value
            End Set
        End Property

        Public Property ScaleDeAuthorizations() As ArrayList
            Get
                Return _ScaleDeAuthorizations
            End Get
            Set(ByVal value As ArrayList)
                _ScaleDeAuthorizations = value
            End Set
        End Property

        Public Property ItemTaxFlags() As Hashtable
            Get
                Return _ItemTaxFlags
            End Get
            Set(ByVal value As Hashtable)
                _ItemTaxFlags = value
            End Set
        End Property

        Public Property BatchFileName() As String
            Get
                'set filename if not already done
                If _BatchFileName Is Nothing Or _BatchFileName = "" Then
                    SetTempFilenames()
                End If
                Return _BatchFileName
            End Get
            Set(ByVal value As String)
                _BatchFileName = value
            End Set
        End Property
        Public Property RemoteFileName() As String
            Get
                Return _RemoteFileName
            End Get
            Set(ByVal value As String)
                _RemoteFileName = value
            End Set
        End Property
        Public Property ExemptTagFileName() As String
            Get
                'set filename if not already done
                'If _ExemptTagFileName Is Nothing Or _BatchFileName = "" Then
                If _ExemptTagFileName Is Nothing Then
                    SetTempFilenames()
                End If
                Return _ExemptTagFileName
            End Get
            Set(ByVal value As String)
                _ExemptTagFileName = value
            End Set
        End Property
        Public Property RemoteExemptTagFileName() As String
            Get
                Return _RemoteExemptTagFileName
            End Get
            Set(ByVal value As String)
                _RemoteExemptTagFileName = value
            End Set
        End Property

        Public Property BatchFilePath() As String
            Get
                Return _BatchFilePath
            End Get
            Set(ByVal value As String)
                _BatchFilePath = value
            End Set
        End Property

        Public Property ControlFileName() As String
            Get
                'set filename if not already done
                If _ControlFileName Is Nothing Or _ControlFileName = "" Then
                    SetTempFilenames()
                End If
                Return _ControlFileName
            End Get
            Set(ByVal value As String)
                _ControlFileName = value
            End Set
        End Property

        Public Property ControlFilePath() As String
            Get
                Return _ControlFilePath
            End Get
            Set(ByVal value As String)
                _ControlFilePath = value
            End Set
        End Property

        Public Property ChangesDelivered() As Boolean
            Get
                Return _changesDelivered
            End Get
            Set(ByVal value As Boolean)
                _changesDelivered = value
            End Set
        End Property

        Public Property FTPInfo() As StoreFTPConfigBO
            Get
                Return _ftpInfo
            End Get
            Set(ByVal value As StoreFTPConfigBO)
                _ftpInfo = value
            End Set
        End Property

        Public Property GeneratedControlFile() As Boolean
            Get
                Return _generatedControlFile
            End Get
            Set(ByVal value As Boolean)
                _generatedControlFile = value
            End Set
        End Property
#End Region

    End Class

End Namespace

