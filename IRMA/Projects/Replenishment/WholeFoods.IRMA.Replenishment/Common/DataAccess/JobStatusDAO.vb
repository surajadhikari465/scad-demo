Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Common.DataAccess
    Public Class JobStatusDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Const DBSTAUS_RUNNING As String = "RUNNING"
        Public Const DBSTAUS_COMPLETE As String = "COMPLETE"
        Public Const DBSTAUS_FAILED As String = "FAILED"
        Public Const DBSTAUS_QUEUEING As String = "QUEUEING"

#Region "Get Methods"
        ''' <summary>
        ''' Translates the database status value to the enum value used by the applications.
        ''' </summary>
        ''' <param name="dbStatusStr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function TranslateDBStatusValue(ByRef dbStatusStr As String) As DBJobStatus
            logger.Debug("TranslateDBStatusValue entry: dbStatusStr=" + dbStatusStr.ToString())
            Dim returnStatus As DBJobStatus
            If dbStatusStr.ToUpper.Equals(DBSTAUS_RUNNING) Then
                returnStatus = DBJobStatus.Running
            ElseIf dbStatusStr.ToUpper.Equals(DBSTAUS_COMPLETE) Then
                returnStatus = DBJobStatus.Complete
            ElseIf dbStatusStr.ToUpper.Equals(DBSTAUS_FAILED) Then
                returnStatus = DBJobStatus.Failed
            Else
                ' We did not get an expected value back.  Return the error code.
                returnStatus = DBJobStatus.JobError
            End If
            logger.Debug("TranslateDBStatusValue exit")
            Return returnStatus
        End Function

        ''' <summary>
        ''' Reads the current job status data for a given class.
        ''' </summary>
        ''' <param name="classname"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetJobStatus(ByVal classname As String) As JobStatusBO
            logger.Debug("GetJobStatus entry: classname=" + classname)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentStatus As JobStatusBO = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList

            Try
                ' setup parameters for stored proc
                paramList = New ArrayList

                currentParam = New DBParam
                currentParam.Name = "Classname"
                currentParam.Value = classname
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetJobStatus", paramList)

                If results.Read Then
                    currentStatus = New JobStatusBO()
                    If (Not results.IsDBNull(results.GetOrdinal("Status"))) Then
                        currentStatus.Status = TranslateDBStatusValue(results.GetString(results.GetOrdinal("Status")))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("LastRun"))) Then
                        currentStatus.LastRun = results.GetDateTime(results.GetOrdinal("LastRun"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("ServerName"))) Then
                        currentStatus.ServerName = results.GetString(results.GetOrdinal("ServerName"))
                    End If
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("GetJobStatus exit")
            Return currentStatus
        End Function

        ''' <summary>
        ''' Reads all of the current classes defined in the job status table.
        ''' </summary>
        ''' <returns>ArrayList of JobStatusBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetScheduledJobClassList() As ArrayList
            logger.Debug("GetScheduledJobClassList entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim classList As New ArrayList
            Dim classBO As JobStatusBO

            Try
                ' Execute the stored procedure
                results = factory.GetStoredProcedureDataReader("GetScheduledJobClasses")

                While results.Read
                    classBO = New JobStatusBO
                    If Not results.IsDBNull(results.GetOrdinal("Classname")) Then
                        classBO.Classname = results.GetString(results.GetOrdinal("Classname"))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("Status")) Then
                        classBO.Status = TranslateDBStatusValue(results.GetString(results.GetOrdinal("Status")))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("LastRun")) Then
                        classBO.LastRun = results.GetDateTime(results.GetOrdinal("LastRun"))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("ServerName")) Then
                        classBO.ServerName = results.GetString(results.GetOrdinal("ServerName"))
                    End If

                    classList.Add(classBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetScheduledJobClassList exit")
            Return classList
        End Function

        ''' <summary>
        ''' Reads all of the current errors from the job error table for a given class.
        ''' </summary>
        ''' <param name="classname"></param>
        ''' <returns>ArrayList of JobErrorBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetScheduledJobErrorList(ByVal classname As String) As ArrayList
            logger.Debug("GetScheduledJobErrorList entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim errorList As New ArrayList
            Dim errorBO As JobErrorBO

            Try
                ' setup parameters for stored proc
                paramList = New ArrayList

                currentParam = New DBParam
                currentParam.Name = "Classname"
                currentParam.Value = classname
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure
                results = factory.GetStoredProcedureDataReader("GetScheduledJobErrors", paramList)

                While results.Read
                    errorBO = New JobErrorBO
                    errorBO.Classname = classname
                    If Not results.IsDBNull(results.GetOrdinal("RunDate")) Then
                        errorBO.LastRun = results.GetDateTime(results.GetOrdinal("RunDate"))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("ServerName")) Then
                        errorBO.ServerName = results.GetString(results.GetOrdinal("ServerName"))
                    End If
                    If Not results.IsDBNull(results.GetOrdinal("ExceptionText")) Then
                        errorBO.ExceptionText = results.GetString(results.GetOrdinal("ExceptionText"))
                    End If

                    errorList.Add(errorBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetScheduledJobErrorList exit")
            Return errorList
        End Function
#End Region

#Region "Set Methods"
        ''' <summary>
        ''' Inserts a new job status record for a given class.
        ''' </summary>
        ''' <param name="classname"></param>
        ''' <param name="jobStatus"></param>
        ''' <remarks></remarks>
        Public Shared Sub InsertJobStatus(ByVal classname As String, ByVal jobStatus As DBJobStatus)
            logger.Info("InsertJobStatus entry: classname=" + classname + ", jobStatus=" + jobStatus.ToString())
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Classname"
            currentParam.Value = classname
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Status"
            Select Case jobStatus
                Case DBJobStatus.Running
                    currentParam.Value = DBSTAUS_RUNNING
                Case DBJobStatus.Complete
                    currentParam.Value = DBSTAUS_COMPLETE
                Case DBJobStatus.Failed
                    currentParam.Value = DBSTAUS_FAILED
                Case DBJobStatus.Queueing
                    currentParam.Value = DBSTAUS_QUEUEING
                Case Else
                    currentParam.Value = DBNull.Value
            End Select
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("InsertJobStatus", paramList)

            logger.Debug("InsertJobStatus exit")
        End Sub

        ''' <summary>
        ''' Updates the job status for a given class.
        ''' </summary>
        ''' <param name="classname"></param>
        ''' <param name="jobStatus"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdateJobStatus(ByVal classname As String, ByVal jobStatus As DBJobStatus)
            logger.Info("UpdateJobStatus entry: classname=" + classname + ", jobStatus=" + jobStatus.ToString())
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Classname"
            currentParam.Value = classname
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Status"
            Select Case jobStatus
                Case DBJobStatus.Running
                    currentParam.Value = DBSTAUS_RUNNING
                Case DBJobStatus.Complete
                    currentParam.Value = DBSTAUS_COMPLETE
                Case DBJobStatus.Failed
                    currentParam.Value = DBSTAUS_FAILED
                Case DBJobStatus.Queueing
                    currentParam.Value = DBSTAUS_QUEUEING
                Case Else
                    currentParam.Value = DBNull.Value
            End Select
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("UpdateJobStatus", paramList)
            logger.Debug("UpdateJobStatus exit")
        End Sub

        ''' <summary>
        ''' Records an error record in the job status table when an error is encountered.
        ''' </summary>
        ''' <param name="classname"></param>
        ''' <param name="exceptionText"></param>
        ''' <remarks></remarks>
        Public Shared Sub InsertJobError(ByVal classname As String, ByVal exceptionText As String)
            logger.Debug("InsertJobError entry: classname= " + classname + ", exceptionText = " + exceptionText)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Classname"
            currentParam.Value = classname
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExceptionText"
            currentParam.Value = exceptionText
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("InsertJobError", paramList)
            logger.Debug("InsertJobError exit")
        End Sub

        ''' <summary>
        ''' Resets a job in the FAILED status to the COMPLETE status, allowing the job to be re-executed.
        ''' </summary>
        ''' <param name="classname"></param>
        ''' <remarks></remarks>
        Public Shared Function ResetFailedJob(ByVal classname As String) As Integer
            logger.Debug("ResetFailedJob entry: classname=" + classname)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim validationCode As Integer

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Classname"
            currentParam.Value = classname
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("ResetFailedScheduledJob", paramList)
            validationCode = CInt(outputList(0))

            logger.Debug("ResetFailedJob exit: validationCode=" + validationCode.ToString())
            Return validationCode
        End Function
#End Region
    End Class
End Namespace

