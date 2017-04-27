Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Writers

    Public Class SmartX_Writer
        Inherits ScaleWriter

        ' scaleFilename is the name of the file placed on the FTP server
        Private _scaleFilename As String = "Text.txt"
        Private _outputFileFormat As FileFormat = FileFormat.Text
        Protected Overrides_fileEncoding As Encoding = Encoding.ASCII

        Private Const MAX_EXTRA_TEXT_LENGTH As Integer = 48

#Region "Property Definitions"
        Overrides Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
            Get
                ' The filename changes based on the type of data being written to the file.
                Return _scaleFilename
            End Get
            Set(ByVal value As String)
                _scaleFilename = value
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

        Public Overrides ReadOnly Property CorpRecordsIncludePricing() As Boolean
            Get
                ' Flag set to true if the writer includes the zone pricing data with the corporate records
                Return False
            End Get
        End Property

#End Region

#Region "Writer Constructors"

        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)
        End Sub

#End Region
#Region "Writer specific methods to add a single record to the SCALE Push file"
        ''' <summary>
        ''' This function parses the extra text string into an ArrayList of strings, with each entry in the
        ''' array representing a single line of text.
        ''' </summary>
        ''' <param name="extraText"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ParseExtraText(ByVal extraText As String, Optional ByVal maxExtraTextLength As Integer = MAX_EXTRA_TEXT_LENGTH) As ArrayList
            Dim extraTextArray As New ArrayList

            If extraText.Length > maxExtraTextLength Then
                ' Break into chunks, with each chunk having a max length of maxExtraTextLength, making sure
                ' that lines are only broken up on spaces
                Dim splitStr As String() = extraText.Split(" "c)
                Dim index As Integer = 0
                Dim currentStr As New StringBuilder()

                Try
                    While index < splitStr.Length
                        ' See if you can add the next string of text to the end of the current text without
                        ' exceeding the max length for a single line.
                        If (currentStr.Length + splitStr(index).Length) <= maxExtraTextLength Then
                            currentStr.Append(splitStr(index))
                            currentStr.Append(" ")
                            index += 1

                            ' Check to see if this is the last word to be added - if so, add to ExtraText array
                            If index = splitStr.Length Then
                                extraTextArray.Add(currentStr)
                            End If
                        ElseIf currentStr.Length > 0 Then
                            ' Add the current string being built to the array and start a new string for the next line.
                            ' If adding the last trailing space made the string to long, it should be removed.
                            If currentStr.Length > maxExtraTextLength Then
                                currentStr.Remove(currentStr.Length - 1, 1)
                            End If
                            extraTextArray.Add(currentStr)
                            currentStr = New StringBuilder()
                        Else
                            ' The text being looked at is longer than the max length, and it does not contain any spaces. 
                            ' Just split in the middle of words.
                            Dim splitPos As Integer = 0
                            While splitPos < splitStr(index).Length
                                extraTextArray.Add(splitStr(index).Substring(splitPos, maxExtraTextLength))
                                splitPos += maxExtraTextLength
                            End While
                            index += 1
                        End If
                    End While

                Catch ex As Exception
                    '' looking for index out of range to handle it.
                    Logger.LogError("ParseExtraText exception: ", Me.GetType, ex)
                End Try

            Else
                ' The text is not longer than the length of a single line so it does not need extra processing.
                extraTextArray.Add(extraText)
            End If

            Return extraTextArray
        End Function

        ''' <summary>
        ''' The Extra Text file includes special logic to handle the Line # and Extra Text Data.  The stored proc returns the
        ''' Scale_ExtraText.ExtraText value in one record of the result set.  The writer must:
        ''' 1. Include a header row that sets the Line # to "00" and the Extra Text Data is empty.
        ''' 2. Include a row for each "chunk" of ExtraText, not including more than 48 characters in a single row.
        '''    The Line # is incremented for each row that is written.
        ''' </summary>
        ''' <param name="currentRowNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function RepeatRowInFile(ByVal currentRowNum As Integer, ByVal rowRepeatCount As Integer) As Boolean
            Dim repeatRow As Boolean = False
            Dim extraText As String = ""
            Dim extraTextArray As ArrayList

            If _chgType = ChangeType.ExtraText Then
                ' See how many rows are needed to write this set of extra text.
                ' The first row is the header and does not contain any extra text, so at least two rows are always needed.
                If rowRepeatCount = 0 Then
                    repeatRow = True
                ElseIf (Not _currentChange.IsDBNull(_currentChange.GetOrdinal("ExtraText"))) Then
                    extraText = _currentChange.GetValue(_currentChange.GetOrdinal("ExtraText")).ToString()

                    'Try to get the size (characters per line) specified for the Extra Text label type if it exists,
                    'and let ParseExtraText decide what to do if it doesn't
                    If (Not _currentChange.IsDBNull(_currentChange.GetOrdinal("Characters"))) Then
                        extraTextArray = ParseExtraText(extraText, CInt(_currentChange.GetValue(_currentChange.GetOrdinal("Characters"))))
                    Else
                        extraTextArray = ParseExtraText(extraText)
                    End If

                    If rowRepeatCount < extraTextArray.Count Then ' the count is not incremented by the base writer until after the row is repeated
                        repeatRow = True
                    End If
                End If
            End If

            Return repeatRow
        End Function

        ''' <summary>
        ''' This function returns a String for the data element columns (IsLiteral = False and IsTaxFlag = False).
        ''' </summary>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function BuildDataElementDataString(ByRef currentColumn As POSDataElementBO, ByVal currentRowNum As Integer, ByVal rowRepeatCount As Integer) As String
            Dim lineStr As New StringBuilder
            Dim dataContent As String = Nothing
            Dim extraText As String
            Dim extraTextArray As ArrayList

            If _chgType <> ChangeType.ExtraText Then
                ' No extra logic is required for other change types.  Just return the value from the base class.
                dataContent = MyBase.BuildDataElementDataString(currentColumn, currentRowNum, rowRepeatCount)
            Else
                ' Scale writers require extra logic for the Record Type, Line #, and Extra Text columns.  
                ' All other columns just return the value from the base class.
                If currentColumn.DataElement.Equals("SmartX_Sequence") Then
                    ' Look at the repeat row count to determine if this is a header or sequence record.
                    If rowRepeatCount = 0 Then
                        dataContent = """H"""
                    Else
                        dataContent = """S"""
                    End If
                ElseIf currentColumn.DataElement.Equals("SmartX_LineNum") Then
                    ' Look at the repeat row count to determine if this is a header or sequence record.
                    ' If this is a sequenct record, the row count will match the repeat number.
                    dataContent = rowRepeatCount.ToString("00")
                ElseIf currentColumn.DataElement.Equals("ExtraText") Then
                    ' The extra text that corresponds to the current line number should be written to the file.
                    If rowRepeatCount = 0 Then
                        ' Return an empty text string, enclosed in quotes, for the header record.
                        dataContent = """"""
                    Else
                        Try

                            If (Not _currentChange.IsDBNull(_currentChange.GetOrdinal(currentColumn.DataElement))) Then
                                extraText = _currentChange.GetValue(_currentChange.GetOrdinal(currentColumn.DataElement)).ToString()

                                'Try to get the size (characters per line) specified for the Extra Text label type if it exists,
                                'and let ParseExtraText decide what to do if it doesn't
                                If (Not _currentChange.IsDBNull(_currentChange.GetOrdinal("Characters"))) Then
                                    extraTextArray = ParseExtraText(extraText, CInt(_currentChange.GetValue(_currentChange.GetOrdinal("Characters"))))
                                Else
                                    extraTextArray = ParseExtraText(extraText)
                                End If

                                dataContent = """" + extraTextArray.Item(rowRepeatCount - 1).ToString + """"
                            Else
                                dataContent = """""" ' return an empty text string, enclosed in quotes
                            End If

                        Catch ex As Exception
                            '' looking for index out of range to handle it.
                            Logger.LogError("BuildDataElementDataString exception: ", Me.GetType, ex)
                        End Try
                    End If
                Else
                    ' No extra logic is required for other columns.  Just return the value from the base class.
                    dataContent = MyBase.BuildDataElementDataString(currentColumn, currentRowNum, rowRepeatCount)
                End If
            End If

            Return dataContent
        End Function

#End Region

    End Class

End Namespace
