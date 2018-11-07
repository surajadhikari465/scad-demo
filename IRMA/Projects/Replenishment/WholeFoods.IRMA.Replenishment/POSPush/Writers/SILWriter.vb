Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

' NOTE: If this Namespace is updated, the POSStoreUpdates constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.POSPush.Writers

    Public Class SILWriter
        Inherits POSWriter

        ' POSFilename is the name of the file placed on the FTP server
        Private _POSFilename As String = "HTMAINT.sil"
        '       Private _isBinary As Boolean = False
        Private _outputFileFormat As FileFormat = FileFormat.Text
        Private _useHeader As Boolean = False
        Private _tmpHeader As StringBuilder

#Region "Writer Constructors"
        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)
        End Sub
#End Region

#Region "Writer Specific Header & Footer Methods"
        ''' <summary>
        ''' The SIL Writer includes a header that applies to the entire file and then a header for each 
        ''' change type.  
        ''' </summary>
        ''' <param name="chgType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddMainHeaderTextToFile(ByVal chgType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            Logger.LogDebug("AddMainHeaderTextToFile entry: changeType=" & chgType.ToString() & ", filename=" & filename, Me.GetType())
            'add main header
            Dim header As New StringBuilder
            header.Append("insert into header_dct values(")
            '9999 = sourceID 203 = destination ID 
            header.Append("'HC','")
            header.Append(headerInfo.BatchID)
            header.Append("','9999','203'")
            header.Append("',,," + getBatchOriginDate(headerInfo.BatchDate) + "," + getBatchOriginTime(headerInfo.BatchDate))
            'TODO: append execution date and time //check logic to set this
            header.Append("2007031,0000,,")
            header.Append("'LOAD','FDRSG DICTIONARY',,,,,,'1/1.0',,'F01',,'5.0'")
            header.Append(");")
            Me.WriteLine(header.ToString)

            'create the data dictionary
            WriteItemDataDictionary()
            Logger.LogDebug("AddMainHeaderTextToFile exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Write the SIL section header, based on the change type.
        ''' </summary>
        ''' <param name="chgType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionHeaderTextToFile(ByVal chgType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            Logger.LogDebug("AddSectionHeaderTextToFile entry: changeType=" & chgType.ToString() & ", filename=" & filename, Me.GetType())
            Dim header As New StringBuilder
            Dim type As String = "Null"

            ' build the section header
            header.Append("insert into header_dct values(")
            header.Append("'HM','")
            header.Append(headerInfo.BatchID)
            header.Append("','9999','203'")
            header.Append("',,," + getBatchOriginDate(headerInfo.BatchDate) + "," + getBatchOriginTime(headerInfo.BatchDate))
            'TODO: append execution date and time //check logic to set this
            header.Append("2007031,001,,")
            'add request type
            Select Case chgType
                ' TODO: What about the other change types?
                Case ChangeType.ItemDataDelete
                    type = "REMOVE"
                Case ChangeType.ItemDataChange
                    type = "CHANGE"
            End Select
            'TODO: will type ever not be here

            header.Append(type + "," + headerInfo.BatchDesc + ",,,,,,'1/1.0',,'F01',")
            header.Append(");")
            ' write the line to the file
            Me.WriteLine(header.ToString)

            ' write the view to the file
            WriteItemDataView(chgType)
            Logger.LogDebug("AddSectionHeaderTextToFile exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Write the data dictionary for the SIL Writer.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub WriteItemDataDictionary()
            Logger.LogDebug("WriteItemDataDictionary entry", Me.GetType())
            Dim header As New StringBuilder()
            Dim dictionaryEnum As IEnumerator
            Dim currentDictionary As POSWriterDictionaryBO
            Dim valueAdded As Boolean = False

            Try
                ' Execute the stored procedure to read the collection of POSWriterDictionaryDAO objects and
                ' add each object to the header
                dictionaryEnum = POSWriterDAO.GetPOSWriterDictionary(_posFileWriterKey).GetEnumerator()

                WriteLine("CREATE TABLE ITEM_DCT (")
                While dictionaryEnum.MoveNext()
                    currentDictionary = CType(dictionaryEnum.Current, POSWriterDictionaryBO)
                    If (valueAdded) Then
                        header.Append(", ")
                        WriteLine(header.ToString)
                        header = New StringBuilder()
                    End If
                    header.Append(currentDictionary.FieldID)
                    header.Append(" ")
                    header.Append(currentDictionary.DataType)
                    valueAdded = True
                End While
                header.Append(");")
                WriteLine(header.ToString)

            Catch e As DataFactoryException
                throwException("SILWriter.WriteItemDataDictionary could not execute because of a database error", e)
            End Try
            Logger.LogDebug("WriteItemDataDictionary exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Write the view for the change type.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub WriteItemDataView(ByVal changeType As ChangeType)
            Logger.LogDebug("WriteItemDataView entry: changeType=" & changeType.ToString(), Me.GetType())
            Dim header As New StringBuilder()
            Dim columnEnum As IEnumerator = Nothing
            Dim currentColumn As POSDataElementBO
            Dim isDataWritten As Boolean = False
            Dim currentRow As Integer = -1
            Dim previousRow As Integer = -1

            ' Get the column definitions for this change type
            Select Case changeType
                Case changeType.ItemIdAdd
                    columnEnum = Me.ItemIdAddConfig.GetEnumerator()
                Case changeType.ItemIdDelete
                    columnEnum = Me.ItemIdDeleteConfig.GetEnumerator()
                Case changeType.ItemDataChange
                    columnEnum = Me.ItemDataChangeConfig.GetEnumerator()
                Case changeType.ItemDataDelete
                    columnEnum = Me.ItemDataDeleteConfig.GetEnumerator()
                Case changeType.ItemDataDeAuth
                    ' De-auth records are sent down automatically using the delete batch format
                    columnEnum = Me.ItemDataDeleteConfig.GetEnumerator()
            End Select

            ' Create the VIEW - adding each field id in the column definitions to the SELECT statement
            While (columnEnum.MoveNext)
                currentColumn = CType(columnEnum.Current, POSDataElementBO)
                currentRow = currentColumn.RowOrder

                'check if still on current row
                If previousRow <> -1 AndAlso currentRow <> previousRow AndAlso isDataWritten Then
                    'add the FROM statement to the row
                    header.Append(" FROM ITEM_DCT;")

                    'write previous row of data to file
                    WriteLine(header.ToString)

                    'start new StringBuilder for current row
                    header = New StringBuilder
                    isDataWritten = False
                End If

                'If this is the beginning of the row, add the CREATE statement to the row
                If Not isDataWritten Then
                    header.Append("CREATE VIEW ITEM_CHG AS SELECT ")
                End If

                ' Append the field id for the column
                ' add the comma separator if this is not the first column
                If isDataWritten Then
                    header.Append(", ")
                End If
                header.Append(currentColumn.FieldId)

                previousRow = currentRow
                isDataWritten = True
            End While

            ' write the line to the file for the final row of data
            If isDataWritten Then
                WriteLine(header.ToString)
            End If
            Logger.LogDebug("WriteItemDataView exit", Me.GetType())
        End Sub

        ''' <summary>
        ''' Write the SIL footer.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionFooterTextToFile(ByVal changeType As ChangeType, ByVal footerInfo As POSBatchFooterBO)
            Logger.LogDebug("AddSectionFooterTextToFile entry: changeType=" & changeType.ToString(), Me.GetType())
            Dim footer As New StringBuilder
            footer.Append(";")
            ' write the line to the file
            WriteLine(footer.ToString)
            Logger.LogDebug("AddSectionFooterTextToFile exit", Me.GetType())
        End Sub
#End Region

#Region "Writer Specific methods to add a single record to the POS Push file"
        ''' <summary>
        ''' The SIL writer overrides WriteDataLine to add the SQL-like formatting around the
        ''' data line.
        ''' </summary>
        ''' <param name="line"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub WriteDataLine(ByVal line As String)
            ' format the line with the SIL specific entry
            Dim lineContent As New StringBuilder
            lineContent.Append("INSERT INTO ITEM_CHG VALUES (")
            lineContent.Append(line)
            lineContent.Append(")")

            ' write the line to the file
            MyBase.WriteLine(lineContent.ToString())
        End Sub
#End Region

#Region "Writer Specific Formatting Methods"
        Private Function getBatchOriginDate(ByVal batchDate As Date) As String
            Return (batchDate.Year.ToString + (batchDate.Month - 1).ToString + batchDate.Day.ToString)
        End Function
        Private Function getBatchOriginTime(ByVal batchDate As Date) As String
            Return (batchDate.TimeOfDay.ToString)
        End Function
#End Region

#Region "Property Definitions"
        Overrides Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
            Get
                Return _POSFilename
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

        'Overrides Property IsBinary() As Boolean
        '    Get
        '        Return _isBinary
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _isBinary = value
        '    End Set
        'End Property
#End Region

    End Class

End Namespace


