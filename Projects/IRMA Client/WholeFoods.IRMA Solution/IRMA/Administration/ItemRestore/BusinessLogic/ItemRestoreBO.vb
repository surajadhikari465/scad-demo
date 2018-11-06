Imports log4net
Imports System.Data.SqlClient

Public Class ItemRestoreBO

    ' Business Rules
    ' 1) If the specified identifier is active, no restore action can be taken.
    ' 2) If the specified identifier is in Pending-Delete status (active PBD rows exist), no restore action can be taken.
    ' 3) If the specified identifier has multiple, deleted item keys, the most recent will be restored.

    ' ----------------------------------------------------------------------------
    ' Revision History
    ' ----------------------------------------------------------------------------
    ' 8/18/10             Tom Lux               TFS 13138        Converted to shared methods and updated references appropriately.
    ' 8/18/10             Tom Lux               TFS 13138        Added validation (validateRestore method), logging, and message constants.
    ' 8/18/10             Tom Lux               TFS 13138        Added buildPendingDeleteInfo method to build comma-delimited set of data that user can copy out of error dialog.


#Region "Private Members"

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const ERROR_IDENTIFIER_EMPTY_OR_BLANK As String = "No identifier was specified."
    Private Const ERROR_ITEM_IS_ACTIVE As String = "Cannot restore the item because it is in a normal/active state."
    Private Const ERROR_ITEM_DELETE_PENDING As String = "Cannot restore the item because it is in a pending-delete state." & vbCrLf & "The delete must be batched and a POS Push run before the item can be restored." & vbCrLf & "See the error details for the pending-delete (price batch) data blocking this restore."
    Private Const ERROR_UNEXPECTED_DB_ERROR As String = "An error occurred while retrieving information from the database."
#End Region

#Region "Property Declarations"

    Private _identifier As String

#End Region

#Region "Property Accessor Methods"

    Public Property Identifier() As String
        Get
            Return Me._identifier
        End Get
        Set(ByVal value As String)
            Me._identifier = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Performs the necessary checks to see if this item can be restored from 'Deleted' status.
    ''' </summary>
    ''' <param name="identifier">Item identifier to be restored.</param>
    ''' <remarks>If any validation fails, an exception is thrown containing appropriate information, so caller must handle.</remarks>
    Public Shared Sub validateRestore(ByVal identifier As String)
        logger.InfoFormat("Validating deleted-item restore for identifier '{0}'.", identifier)
        ' Check arg.
        If identifier Is Nothing Or String.IsNullOrEmpty(identifier) Then
            Throw New Exception(ERROR_IDENTIFIER_EMPTY_OR_BLANK)
        End If

        ' Check if item is active.
        Dim itemIsActive As Boolean = False
        Try
            itemIsActive = ItemRestoreDAO.IsItemActive(identifier)
        Catch ex As Exception
            Throw New Exception(ERROR_UNEXPECTED_DB_ERROR, ex)
        End Try
        If itemIsActive Then
            Throw New Exception(ERROR_ITEM_IS_ACTIVE)
        End If
        logger.InfoFormat("Item for identifier '{0}' is active.", identifier)

        ' Get pending-delete data.  If data is returned, we throw an error because this means the item cannot be restored.
        ' Any data found will be included in the exception thrown so that it can be logged, displayed to the user, etc.
        Dim pendingDeleteData As String
        Try
            pendingDeleteData = buildPendingDeleteInfo(identifier)
        Catch ex As Exception
            Throw New Exception(ERROR_UNEXPECTED_DB_ERROR, ex)
        End Try
        If pendingDeleteData IsNot Nothing And Not String.IsNullOrEmpty(pendingDeleteData) Then
            logger.InfoFormat("Pending-delete data found for identifier '{0}', data length={1}.", identifier, pendingDeleteData.Length)
            Dim msg As String = ERROR_ITEM_DELETE_PENDING + vbCrLf + vbCrLf + "[Pending Delete Info, Comma-Delimited, Copy to Excel for Easier Viewing]" + vbCrLf + pendingDeleteData
            Throw New Exception(msg)
        End If
        logger.InfoFormat("No pending-delete data found for identifier '{0}'.", identifier)
    End Sub

    Public Shared Function Restore(ByVal identifier As String) As Boolean
        logger.InfoFormat("Restoring deleted item '{0}'.", identifier)
        Return ItemRestoreDAO.Restore(identifier)
    End Function

    ''' <summary>
    ''' Builds a comma-delimited table of price-batch data that can be displayed to the user for troubleshooting purposes.
    ''' Like, for example, if a user wants to delete an item, but cannot see one or more pending delete entries in the batch-search screen.
    ''' </summary>
    ''' <param name="identifier">Item identifier for which any pending-delete info will be pulled.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function buildPendingDeleteInfo(ByVal identifier As String)
        logger.InfoFormat("Building pending-deleted info for '{0}'.", identifier)
        Dim dataReader As SqlDataReader = ItemRestoreDAO.PendingDeleteGetInfo(identifier)
        Dim data As New System.Text.StringBuilder
        Try
            With dataReader
                If .HasRows Then
                    Do While .Read()
                        ' Build lines of pending-delete data.
                        data.Append(.GetInt32(.GetOrdinal("PriceBatchDetailID")))
                        data.Append(",")
                        data.Append(.GetInt32(.GetOrdinal("Item_Key")))
                        data.Append(",")
                        data.Append(.GetInt32(.GetOrdinal("Store_No")))
                        data.Append(",")
                        If Not .IsDBNull(.GetOrdinal("PriceBatchHeaderID")) Then
                            data.Append(.GetInt32(.GetOrdinal("PriceBatchHeaderID")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("PriceBatchStatus")) Then
                            data.Append(.GetString(.GetOrdinal("PriceBatchStatus")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("BatchDescription")) Then
                            data.Append(.GetString(.GetOrdinal("BatchDescription")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("ApplyDate")) Then
                            data.Append(.GetDateTime(.GetOrdinal("ApplyDate")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("PBHItemChgType")) Then
                            data.Append(.GetString(.GetOrdinal("PBHItemChgType")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("PBDItemChgType")) Then
                            data.Append(.GetString(.GetOrdinal("PBDItemChgType")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("PBHPriceChgType")) Then
                            data.Append(.GetString(.GetOrdinal("PBHPriceChgType")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("PBDPriceChgType")) Then
                            data.Append(.GetString(.GetOrdinal("PBDPriceChgType")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("PBHStartDate")) Then
                            data.Append(.GetDateTime(.GetOrdinal("PBHStartDate")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("Identifier")) Then
                            data.Append(.GetString(.GetOrdinal("Identifier")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("Insert_Date")) Then
                            data.Append(.GetDateTime(.GetOrdinal("Insert_Date")))
                            data.Append(",")
                        Else
                            data.Append("null,")
                        End If
                        If Not .IsDBNull(.GetOrdinal("InsertApplication")) Then
                            data.Append(.GetString(.GetOrdinal("InsertApplication")))
                        Else
                            data.Append("null")
                        End If
                        data.Append(vbCrLf)
                    Loop
                End If
            End With
        Catch ex As Exception
            logger.Error(ex)
            Throw ex
        Finally
            dataReader.Close()
        End Try

        ' Build header row for the pending-delete data and return it first, then the details.
        If data.Length > 0 Then
            Dim dataHdr As New System.Text.StringBuilder
            dataHdr.Append("PriceBatchDetailID")
            dataHdr.Append(",")
            dataHdr.Append("Item_Key")
            dataHdr.Append(",")
            dataHdr.Append("Store_No")
            dataHdr.Append(",")
            dataHdr.Append("PriceBatchHeaderID")
            dataHdr.Append(",")
            dataHdr.Append("PriceBatchStatus")
            dataHdr.Append(",")
            dataHdr.Append("BatchDescription")
            dataHdr.Append(",")
            dataHdr.Append("ApplyDate")
            dataHdr.Append(",")
            dataHdr.Append("PBHItemChgType")
            dataHdr.Append(",")
            dataHdr.Append("PBDItemChgType")
            dataHdr.Append(",")
            dataHdr.Append("PBHPriceChgType")
            dataHdr.Append(",")
            dataHdr.Append("PBDPriceChgType")
            dataHdr.Append(",")
            dataHdr.Append("PBHStartDate")
            dataHdr.Append(",")
            dataHdr.Append("Identifier")
            dataHdr.Append(",")
            dataHdr.Append("Insert_Date")
            dataHdr.Append(",")
            dataHdr.Append("InsertApplication")
            dataHdr.Append(vbCrLf)
            Return dataHdr.ToString + data.ToString
        Else
            Return data.ToString
        End If

    End Function
End Class
