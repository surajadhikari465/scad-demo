Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Writers

    Public Class PlumHost_CorporateWriter
        Inherits ScaleWriter

        ' scaleFilename is the name of the file placed on the FTP server
        Private _scaleFilename As String = "hostchng.dat"
        Private _outputFileFormat As FileFormat = FileFormat.Text
        Protected Overrides_fileEncoding As Encoding = Encoding.ASCII

#Region "Property Definitions"
        'Overrides Property WriterFilename() As String
        '    Get
        '        Return _scaleFilename
        '    End Get
        '    Set(ByVal value As String)
        '        _scaleFilename = value
        '    End Set
        'End Property

        Overrides Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
            Get
                'don't use default filename above
                Dim filename As New StringBuilder
                filename.Append("PLUM")
                filename.Append(Date.Now.Year.ToString("0000"))
                filename.Append(Date.Now.Month.ToString("00"))
                filename.Append(Date.Now.Day.ToString("00"))
                filename.Append(Date.Now.Hour.ToString("00"))
                filename.Append(Date.Now.Minute.ToString("00"))
                filename.Append(Date.Now.Second.ToString("00"))
                filename.Append(Date.Now.Millisecond.ToString("000"))
                filename.Append(".DAT")

                Return filename.ToString
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
                Return True
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
        ''' This method adds a section header to the file.  This is the header that applies to the change type being processed - 
        ''' not the entire file.
        ''' </summary>
        ''' <param name="chgType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionHeaderTextToFile(ByVal chgType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            Dim header As New StringBuilder

            Select Case chgType
                Case ChangeType.CorpScaleItemIdAdd
                    header.Append("BNA")
                    header.Append("IRMA Import - Item ID Add ")
                    header.Append(Now.ToString("MM-dd"))
                    header.Append("ýBDA")
                    header.Append(Now.ToString("yyyy-MM-dd"))
                    header.Append("ýBTI")
                    header.Append(Now.ToString("HHmmss"))
                    header.Append("ýEFD")
                    header.Append(Now.ToString("yyyy-MM-dd"))
                    header.Append("ýEFT")
                    header.Append("00:01") ' always apply at midnight
                    header.Append("ýBTY")
                    header.Append("H")  ' S = Store, H = Host, O = Other, E = Exception
                    header.Append("ý")
                Case ChangeType.CorpScaleItemChange
                    header.Append("BNA")
                    header.Append("IRMA Import - Corp Chg ")
                    header.Append(Now.ToString("MM-dd"))
                    header.Append("ýBDA")
                    header.Append(Now.ToString("yyyy-MM-dd"))
                    header.Append("ýBTI")
                    header.Append(Now.ToString("HHmmss"))
                    header.Append("ýEFD")
                    header.Append(Now.ToString("yyyy-MM-dd"))
                    header.Append("ýEFT")
                    header.Append("00:01") ' always apply at midnight
                    header.Append("ýBTY")
                    header.Append("H")  ' S = Store, H = Host, O = Other, E = Exception
                    header.Append("ý")
                Case ChangeType.CorpScaleItemIdDelete
                    header.Append("BNA")
                    header.Append("IRMA Import - Corp Delete ")
                    header.Append(Now.ToString("MM-dd"))
                    header.Append("ýBDA")
                    header.Append(Now.ToString("yyyy-MM-dd"))
                    header.Append("ýBTI")
                    header.Append(Now.ToString("HHmmss"))
                    header.Append("ýEFD")
                    header.Append(Now.ToString("yyyy-MM-dd"))
                    header.Append("ýEFT")
                    header.Append("00:01") ' always apply at midnight
                    header.Append("ýBTY")
                    header.Append("H")  ' S = Store, H = Host, O = Other, E = Exception
                    header.Append("ý")
            End Select

            If chgType <> ChangeType.CorpScalePriceExceptions Then
                ' This change type is a second result set included with the other corporate records.
                ' It does not require a new header section.  
                WriteLine(header.ToString)
            End If
        End Sub

        ''' <summary>
        ''' Check to see if the Ingredients for the Item are included in the record.
        ''' </summary>
        ''' <param name="currentChange"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ValidIngredients(ByRef currentChange As SqlDataReader) As Boolean
            Dim validIngred As Boolean = True
            Dim ingredients As String = Nothing
            If (Not currentChange.IsDBNull(currentChange.GetOrdinal("Ingredients"))) Then
                ingredients = currentChange.GetValue(currentChange.GetOrdinal("Ingredients")).ToString()
                If ingredients Is Nothing Or ingredients = "" Then
                    validIngred = False
                End If
            Else
                validIngred = False
            End If

            Return validIngred
        End Function

        ''' <summary>
        ''' The first line of the file only includes the ingredient number if the ingredients has a valid value.
        ''' </summary>
        ''' <param name="currentRowNum"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function IncludeColumnInFile(ByVal currentRowNum As Integer, ByRef currentColumn As POSDataElementBO) As Boolean
            Dim includeCol As Boolean = True

            If currentRowNum = 1 AndAlso _
              (Not currentColumn.FieldId Is Nothing AndAlso currentColumn.FieldId = "INO") Then
                ' Do not write the ingredients number column if the ingredients text is not populated
                If Not ValidIngredients(_currentChange) Then
                    includeCol = False
                End If
            End If

            Return includeCol
        End Function

        ''' <summary>
        ''' The second line of the file is only included if the ingredients has a valid value.
        ''' </summary>
        ''' <param name="currentRowNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function IncludeRowInFile(ByVal currentRowNum As Integer) As Boolean
            Dim includeRow As Boolean = True

            ' Do not perform the validation for the pricing records
            If _chgType <> ChangeType.CorpScalePriceExceptions Then
                If currentRowNum = 2 Then
                    ' Do not write the 2nd row if the ingredients text is not populated
                    If Not ValidIngredients(_currentChange) Then
                        includeRow = False
                    End If
                End If
            End If

            Return includeRow
        End Function

        ''' <summary>
        ''' The Ingredients text in the second line of the file must replace the vbCrLf characters.  This is not
        ''' handled through the UI/database escape characters logic.
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function ApplyWriterFormatting(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            Dim returnStr As String = dataContent
            ' For the ingredients text in the 2nd row of data, VB constants are replaced.
            ' For storage text replace end of line with char(14)
            If (currentColumn.RowOrder = 2 AndAlso currentColumn.DataElement = "Ingredients") OrElse currentColumn.DataElement = "StorageText" Then
                returnStr = Replace(dataContent, vbCrLf, Chr(14))
            End If
            Return returnStr
        End Function

#End Region

    End Class

End Namespace
