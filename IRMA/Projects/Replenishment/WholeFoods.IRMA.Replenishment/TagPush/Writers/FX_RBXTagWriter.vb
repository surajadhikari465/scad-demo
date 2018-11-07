Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.TagPush.Controller
Imports WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.Utility

' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.TagPush.Writers

    Public Class FX_RBXTagWriter
        Inherits TagWriter

        ' POSFilename is the name of the file placed on the FTP server
        Private _POSFilename As String = "HTMAINT"
        ' Private _isBinary As Boolean = False
        Private _outputFileFormat As FileFormat = FileFormat.Text

#Region "Writer Constructors"
        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)
        End Sub
#End Region

#Region "Writer Specific Header & Footer Methods"
        ''' <summary>
        ''' Add the RBX Planogram header.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <param name="itemRec"></param>
        ''' <remarks></remarks>
        Public Overrides Sub AddPlanogramHeaderToFile(ByVal filename As String, ByRef itemRec As SqlDataReader)
            '|HM|1|store number|2|host id|3|apply date|4|# retention days|5|file to update|6|batch description|7|starting batch #|8|screen batch desc|13|auto apply|20|ignore bad fields|23|truncate|
            Dim planoHeader As New StringBuilder

            planoHeader.Append("|HM|")
            planoHeader.Append("1|").Append(itemRec.GetInt32(itemRec.GetOrdinal("Store_No")))
            planoHeader.Append("|2|0000|")
            planoHeader.Append("3|")
            planoHeader.Append(Date.Now.Month.ToString("00"))
            planoHeader.Append("/")
            planoHeader.Append(Date.Now.Day.ToString("00"))
            planoHeader.Append("/")
            planoHeader.Append(Right(Date.Now.Year.ToString("00"), 2))
            planoHeader.Append("|4|5|")
            planoHeader.Append("5|item|")
            planoHeader.Append("6|HXplan").Append(itemRec.GetString(itemRec.GetOrdinal("ProductPlanogramCode")))
            planoHeader.Append("|8|HXplan").Append(itemRec.GetString(itemRec.GetOrdinal("ProductPlanogramCode")))
            planoHeader.Append("|16|9999|20|1|")

            ' write the line to the file
            WriteLine(planoHeader.ToString)

        End Sub
        Public Overrides Sub AddPlanogramFooterToFile(ByVal recCount As Integer)
            '|HM|1|store number|2|host id|3|apply date|4|# retention days|5|file to update|6|batch description|7|starting batch #|8|screen batch desc|13|auto apply|20|ignore bad fields|23|truncate|
            Dim planoFooter As New StringBuilder

            planoFooter.Append("|T|1|").Append(recCount + 2)
            planoFooter.Append("|2|1|13|").Append(recCount)
            planoFooter.Append("|")

            ' write the line to the file
            WriteLine(planoFooter.ToString)

        End Sub

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

