Imports log4net
Imports System.Configuration
Imports System.IO
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPull.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPull.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility.wodSFTP

Namespace WholeFoods.IRMA.Replenishment.Jobs

    ''' <summary>
    ''' Job that performs the POS Audit Report generation.
    ''' </summary>
    ''' <remarks>
    ''' ASSUMPTION: this is NOT designed to be multi-threaded.  If process must become multi-threaded then 
    ''' file management in StoreUpdatesBO.vb should no longer remove file if temp file exists upon
    ''' process startup.
    ''' </remarks>
    Public Class POSPullJob
        ' ---------------------------------------------------------------------------------------------------------------
        ' Update History
        ' ---------------------------------------------------------------------------------------------------------------
        ' TFS 12091 (v3.6)
        ' Tom Lux
        ' 3/11/2010
        ' 1) Removed list of messages property and logic, as log4net takes care of all this.
        ' 2) Replace Utility.Logger calls with local log4net object calls.
        ' ---------------------------------------------------------------------------------------------------------------

        ' Define the log4net logger for this class.
        Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Collection of StoreUpdatesBO objects to store the POS Push configuration for each store.
        ''' </summary>
        ''' <remarks></remarks>
        Private _stores As Hashtable = New Hashtable

        ''' <summary>
        ''' Contains a message describing the error condition if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorMessage As String

        ''' <summary>
        ''' Contains a message describing the additional info in case a store is missing POS setup
        ''' </summary>
        ''' <remarks></remarks>
        Private _statusMessage As String

        ''' <summary>
        ''' Contains any exception caught during processing if Main does not execute successfully.
        ''' </summary>
        ''' <remarks></remarks>
        Private _errorException As Exception

        ''' <summary>
        ''' Pulls POS Item data files for all stores configured in the POS admin application.
        ''' The files contain data for ALL items available in the store, not just items that were
        ''' added/deleted/batched.  If a store_no is not specified, it pulls all stores.
        ''' </summary>
        ''' <returns>True if it executes successfully; False otherwise</returns>
        ''' <remarks></remarks>
        Public Function Main(Optional ByVal storeNumber As Integer = -1) As Integer
            logger.Debug("Main entry")
            ' Set the return flag
            Dim returnStatus As Integer = 1
            Dim pullBO As POSPullBO

            Dim ftpClient As FTP.FTPclient = Nothing
            Dim sftpClient As wodSFTP.SFTPClient = Nothing

            Dim hasStores As Boolean = False
            Dim missing As String = String.Empty
            Dim wrong As String = String.Empty
            Dim completed As String = String.Empty

            Try
                'This is the main business object for this process
                ' Get list of stores:  This can be one store or all stores
                pullBO = New POSPullBO(storeNumber)

                'Loop through stores
                For Each store As StoreFTPConfigBO In pullBO.FTPInfo.Values
                    If Not (store Is Nothing) Then
                        If store.POSSystemType = "IBM" Then
                            If completed.Length = 0 Then
                                completed = CStr(store.StoreNo)
                            Else
                                completed = String.Format("{0}, {1}", completed, CStr(store.StoreNo))
                            End If

                            'Download the file from each store 
                            If (store.IsSecureTransfer) Then
                                sftpClient = New wodSFTP.SFTPClient(store.IPAddress, store.FTPUser, store.FTPPassword)
                                sftpClient.Download(store.ChangeDirectory & pullBO.SourceIBMFileName, pullBO.SourceIBMPathFileName, True)
                            Else
                                ftpClient = New FTP.FTPclient(store.IPAddress, store.FTPUser, store.FTPPassword, store.IsSecureTransfer)
                                ftpClient.Download(store.ChangeDirectory & pullBO.SourceIBMFileName, pullBO.SourceIBMPathFileName, True)
                            End If

                            ' Transform the source file contents into a target file ready for the IRMA database
                            pullBO.TransformIBM(store.StoreNo)
                        ElseIf store.POSSystemType = "NCR" Then
                            If completed.Length = 0 Then
                                completed = CStr(store.StoreNo)
                            Else
                                completed = String.Format("{0}, {1}", completed, CStr(store.StoreNo))
                            End If

                            'Download the file from each store 
                            If (store.IsSecureTransfer) Then
                                sftpClient = New wodSFTP.SFTPClient(store.IPAddress, store.FTPUser, store.FTPPassword)
                                sftpClient.Download(store.ChangeDirectory & pullBO.SourceNCRFileName, pullBO.SourceNCRPathFileName, True)
                            Else
                                ftpClient = New FTP.FTPclient(store.IPAddress, store.FTPUser, store.FTPPassword, store.IsSecureTransfer)
                                ftpClient.Download(store.ChangeDirectory & pullBO.SourceNCRFileName, pullBO.SourceNCRPathFileName, True)
                            End If

                            ' Transform the source file contents into a target file ready for the IRMA database
                            pullBO.TransformNCR(store.StoreNo)

                        Else
                            If wrong.Length = 0 Then
                                wrong = String.Format("{0}|{1}", CStr(store.StoreNo), store.POSSystemType)
                            Else
                                wrong = String.Format("{0}, {1}|{2}", wrong, CStr(store.StoreNo), store.POSSystemType)
                            End If
                        End If

                        'Before moving on to the next store, must upload this into the database
                        'Run stored procedure that does a 'bulk insert' on the file to the POSItem table in IRMA
                        POSPullDAO.InsertPOSItems(store.StoreNo, pullBO.DBServerSourceFile)
                        ' this transforms the data in POSItem table in IRMA to the temp_PriceAudit table in IRMA
                        POSPullDAO.InsertTempPriceAudit(store.StoreNo)
                    Else
                        If missing.Length = 0 Then
                            missing = CStr(storeNumber)
                        Else
                            missing = String.Format("{0}, {1}", missing, CStr(storeNumber))
                        End If
                    End If

                    If wrong.Length > 0 Then
                        returnStatus = -1
                        logger.Warn("Some stores not processed. " + wrong)
                        StatusMessage = String.Format("POS Pull does not support the following store(s) and POS type(s): {0}.", wrong)
                        ScheduledLog(wrong)
                    End If

                    If missing.Length > 0 Then
                        returnStatus = -1
                        logger.Warn("Some stores not processed. " + missing)
                        StatusMessage = String.Format("POS Pull could not process store(s): {0}.{1}Store POS type or POS Pull FTP info is not defined.", missing, vbLf)
                        ScheduledLog(missing)
                    End If

                    If completed.Length > 0 Then
                        StatusMessage = String.Format("POS Pull processed store(s): {0} successfully.", completed)
                        ScheduledLog(completed)
                    End If
                Next

                pullBO.POSAudit(storeNumber)

                If pullBO.POSAuditExceptionStatus = 1 Then
                    StatusMessage = String.Format("Audit Exception file is too large. Can not create Excel file...")
                    ScheduledLog(StatusMessage)
                End If

            Catch e As DataFactoryException
                logger.Error("Exception: ", e)
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Fatal, e)
                returnStatus = 0
                _errorMessage = e.Message()
                _errorException = e
            Catch e1 As Exception
                logger.Error("Exception: ", e1)
                ErrorHandler.ProcessError(ErrorType.GeneralApplicationError, SeverityLevel.Fatal, e1)
                returnStatus = 0
                _errorMessage = e1.Message()
                _errorException = e1
            End Try

            logger.Debug("Main exit")

            Return returnStatus

        End Function

        ''' <summary>
        ''' Add a status message to the scheduled job log file during application execution.
        ''' </summary>
        ''' <param name="msg">text of the message</param>
        ''' <remarks></remarks>
        Private Sub ScheduledLog(ByVal msg As String)
            logger.Info(msg)
        End Sub

        Public Property StatusMessage() As String
            Get
                Return _statusMessage
            End Get
            Set(ByVal Value As String)
                If _statusMessage Is Nothing Then
                    _statusMessage = Value
                Else
                    _statusMessage = _statusMessage + vbLf + Value
                End If
            End Set
        End Property

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

    End Class

End Namespace
