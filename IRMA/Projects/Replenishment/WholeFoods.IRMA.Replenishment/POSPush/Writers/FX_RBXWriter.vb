Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.Utility

' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.POSPush.Writers

    Public Class FX_RBXWriter
        Inherits POSWriter

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
        ''' Add the RBX section header.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionHeaderTextToFile(ByVal changeType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            '|HM|1|store number|2|host id|3|apply date|4|# retention days|5|file to update|6|batch description|7|starting batch #|8|screen batch desc|13|auto apply|20|ignore bad fields|23|truncate|
            Dim header As New StringBuilder
            header.Append("|HM")
            header.Append("|1|")
            header.Append(headerInfo.StoreNo)
            header.Append("|2|")
            header.Append("9999")   ' host id always 9999 for CIX
            header.Append("|3|")

            'If auto apply is NOT checked, and a value is provided in the Apply Date field, send down the Apply Date
            If headerInfo.AutoApply = False And headerInfo.ApplyDate <> Nothing Then
                header.Append(headerInfo.ApplyDate.ToString("MM/dd/yy"))
            Else
                'default = BatchDate, which = current date
                header.Append(headerInfo.BatchDate.ToString("MM/dd/yy"))
            End If

            header.Append("|4|")
            header.Append("1")      ' always retain for 1 day
            If changeType = Common.Writers.ChangeType.VendorIDAdd Then
                ' for vendor adds, FX 5 is set to dsdvnd
                header.Append("|5|dsdvnd")
            Else
                ' for all other change types, FX 5 is set to item
                header.Append("|5|item")
            End If
            header.Append("|6|")
            header.Append("From IRMA POS Push") ' batch description

            header.Append("|7|")
            ' send the batch id of 500 of one is not specified
            If Not headerInfo.POSBatchId Is Nothing AndAlso Not headerInfo.POSBatchId.Equals("") Then
                header.Append(headerInfo.POSBatchId)
            Else
                header.Append("500")
            End If

            header.Append("|8|")
            'desc of change type + date
            If headerInfo.BatchDesc IsNot Nothing Then
                'limit to 20 chars
                header.Append(Left(headerInfo.BatchDesc, 20))
            Else
                header.Append(headerInfo.BatchDesc)
            End If

            header.Append("|13|")
            If changeType = Common.Writers.ChangeType.VendorIDAdd Then
                header.Append("1")  ' auto apply flag is turned on for vendor adds
            Else
                If headerInfo.AutoApply Then
                    header.Append("1")
                Else
                    header.Append("0")  ' auto apply flag is turned off for all others
                End If
            End If
            header.Append("|20|")
            header.Append("1")  ' ingore bad fields in the record
            header.Append("|23|")
            header.Append("3")  ' truncate formatting option
            header.Append("|")

            ' write the line to the file
            WriteLine(header.ToString)

            ' if this is not a vendor add, include the SA line
            If changeType <> Common.Writers.ChangeType.VendorIDAdd Then
                WriteLine("|SA|189|1|")
            End If
        End Sub

        ''' <summary>
        ''' Add the RBX footer.
        ''' </summary>
        ''' <param name="chgType"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionFooterTextToFile(ByVal chgType As ChangeType, ByVal footerInfo As POSBatchFooterBO)
            Dim totalRecords As Integer
            Dim totalAdds As Integer
            Dim totalChanges As Integer
            Dim totalDeletes As Integer
            Dim totalSubstitutions As Integer
            Dim totalHeaders As Integer = 1
            Dim totalSA As Integer = 1

            Select Case chgType
                Case ChangeType.ItemDataChange
                    totalSubstitutions = Me.RecordCount
                Case ChangeType.ItemDataDelete
                    totalDeletes = Me.RecordCount
                Case ChangeType.ItemDataDeAuth
                    totalDeletes = Me.RecordCount
                Case ChangeType.ItemIdAdd
                    totalSubstitutions = Me.RecordCount
                Case ChangeType.ItemIdDelete
                    totalDeletes = Me.RecordCount
                Case Else
                    totalSubstitutions = Me.RecordCount
            End Select

            ' SA records are not sent for vendor id adds
            If chgType = Common.Writers.ChangeType.VendorIDAdd Then
                totalSA = 0
            End If

            'increment totalRecords to account for 1 footer line
            totalRecords = 1 + totalAdds + totalChanges + totalDeletes + totalSubstitutions + totalHeaders + totalSA

            '|T|1|total records|2|total HM records|3|total adds|4|total changes|5|total deletes|6|total SA|13|total subs|
            Dim footer As New StringBuilder
            footer.Append("|T")
            footer.Append("|1|")
            footer.Append(totalRecords)
            footer.Append("|2|")
            footer.Append(totalHeaders)
            footer.Append("|3|")
            footer.Append(totalAdds)
            footer.Append("|4|")
            footer.Append(totalChanges)
            footer.Append("|5|")
            footer.Append(totalDeletes)
            footer.Append("|6|")
            footer.Append(totalSA)
            footer.Append("|13|")
            footer.Append(totalSubstitutions)
            footer.Append("|")

            ' write the line to the file
            WriteLine(footer.ToString)
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

