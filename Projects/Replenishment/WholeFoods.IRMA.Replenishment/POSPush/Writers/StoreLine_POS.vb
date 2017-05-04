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

    Public Class StoreLine_POS
        Inherits POSWriter

        ' POSFilename is the name of the file placed on the FTP server
        Private _POSFilename As String = "PLU" & Date.Now.ToString("MMddyyhhmm") & ".CSV"
        ' Private _isBinary As Boolean = False
        Private _outputFileFormat As FileFormat = FileFormat.Text

#Region "Writer Constructors"
        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)
        End Sub
#End Region

#Region "Writer Specific Header & Footer Methods"
        ''' <summary>
        ''' Add the Storeline header.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddMainHeaderTextToFile(ByVal changeType As Common.Writers.ChangeType, ByVal filename As String, ByVal headerInfo As BusinessLogic.POSBatchHeaderBO)
            '500ªtest batchª07/01/2010 04:28:33ª20ªªªªªªªªªªªªª4ªªªª        -  EXAMPLE HEADER
        Dim header As New StringBuilder
        Dim delimit As String = "ª"
            header.Append(headerInfo.POSBatchId)
            header.Append(delimit)
            header.Append(headerInfo.BatchDesc)
            header.Append(delimit)
            header.Append(Date.Now)
            header.Append(delimit)
            header.Append("1")                  'Number of Items - Not required on the POS side, so default to 1
            For cnt As Integer = 1 To 13
                header.Append(delimit)
            Next
            header.Append("4")                  '4 = Will run automatically when BatchExe picks it up.
            For cnt As Integer = 1 To 4
                header.Append(delimit)
            Next


        ' write the line to the file
            WriteLine(header.ToString)


        End Sub

        ''' <summary>
        ''' Retalix POS file does not use section headers - all batch records under one header
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionHeaderTextToFile(ByVal changeType As Common.Writers.ChangeType, ByVal filename As String, ByVal headerInfo As BusinessLogic.POSBatchHeaderBO)
            MyBase.AddSectionHeaderTextToFile(changeType, filename, headerInfo)
            ' Nothing here
        End Sub

        ''' <summary>
        ''' Retalix POS file does not have footer
        ''' </summary>
        ''' <param name="chgType"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionFooterTextToFile(ByVal chgType As ChangeType, ByVal footerInfo As POSBatchFooterBO)
            'Nothing here
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

#End Region

    End Class

End Namespace

