Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.ScalePush.Writers

    Public Class PlumHost_ZoneWriter
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
                Case ChangeType.ZoneScalePriceChange
                    header.Append("BNA")
                    header.Append("IRMA Import - Zone Chg ")
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
                    header.Append("E")  ' S = Store, H = Host, O = Other, E = Exception
                    header.Append("ý")
                Case ChangeType.ZoneScaleItemAuthPriceChange
                    header.Append("BNA")
                    header.Append("IRMA Import - Zone Auth ")
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
                    header.Append("E")  ' S = Store, H = Host, O = Other, E = Exception
                    header.Append("ý")
                Case ChangeType.ZoneScaleItemDeAuthPriceChange
                    header.Append("BNA")
                    header.Append("IRMA Import - Zone DeAuth ")
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
                    header.Append("E")  ' S = Store, H = Host, O = Other, E = Exception
                    header.Append("ý")
                Case ChangeType.ZoneScaleItemDelete
                    header.Append("BNA")
                    header.Append("IRMA Import - Zone Delete ")
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
                    header.Append("E")  ' S = Store, H = Host, O = Other, E = Exception
                    header.Append("ý")
            End Select

            WriteLine(header.ToString)
        End Sub

#End Region

    End Class

End Namespace