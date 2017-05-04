Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

Imports System.Threading
Imports WeOnlyDo.Client.SSH
Imports WeOnlyDo.Security.Cryptography.KeyManager
Imports System.Windows.Forms

' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.POSPush.Writers

    Public Class IBM_ADDMI_Writer
        Inherits IBM_Writer


#Region "Writer Constructors"

        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)

            ' POSFilename is the name of the file placed on the FTP server
            _POSFilename = "EAMMAINT.DAT" 'append XXX to filename, which is the currentStore.BatchID
            _isPOSFilenameDynamic = False
            _RemoteJobCommand = ""
            _RemoteJobDirectory = ""

        End Sub

#End Region

#Region "Property Definitions"
        ' Overrides IBM_Writer's WriterFilename, which appends the Batch Number to the Filename. The ADDMI
        ' maintenance file does not have the batch number as part of the filename
        Overrides Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
            Get
                Dim filename As New StringBuilder
                filename.Append(_POSFilename)

                Return filename.ToString
            End Get
            Set(ByVal value As String)
                _POSFilename = value
            End Set
        End Property

#End Region

#Region "Common Header & Footer Methods"
        ''' <summary>
        ''' The ADDMI Writer requires a header that contains the follwoing fields:
        ''' 
        ''' HeaderID = must be xFFFFFFFF11 - indicates to the system that the record length is > 63 bytes
        ''' Batch = the Date and time in MMDDHH format
        ''' Flags1 = bit two is set to indicate Timer Controlled maintenance
        '''      Bit 0 X'80'  Reserved
        '''      Bit 1 X'40'  Operator controlled maintenance
        '''      Bit 2 X'20'  Timer controlled maintenance
        '''      Bit 3 X'10'  Immediate maintenance
        '''      Bit 4 X'08'  Report records in batch
        '''      Bit 5 X'04'  Do not validate department numbers
        '''      Bit 6 X'02'  Do not print/display source data in store
        '''      Bit 7 X'01'  Do not write changed item codes to IR change file.
        ''' 
        ''' Flags2 = 
        '''      Bit 0 X'80'  Log each item processed (direct immediate only)
        '''      Bit 1 X'40'  Full 46-byte records are treated as replaces not adds
        '''      Bit 2 X'20'  Replace can do an add (with log)
        '''      Bit 3 X'10'  Add can do a replace
        '''      Bit 4 X'08'  Add can replace pricing data
        '''      Bit 5 X'04'  Add can replace department numbers
        '''      Bit 6 X'02'  If add does a replace log as error
        '''      Bit 7 X'01'  Direct item record update (direct immediate only).
        ''' 
        ''' MsgData = describes maintenance type
        ''' End Characters = 
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Protected Overrides Sub AddMainHeaderTextToFile(ByVal changeType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            Dim header As New StringBuilder

            If InstanceDataDAO.IsFlagActive("ADDMIExtendedItemRecord") Then
                ' indicates to the system that the record length is > 63 bytes
                header.Append(Chr(255) + Chr(255) + Chr(255) + Chr(255) + Chr(255) + Chr(17))
            Else
                ' indicates to the system that the record is the standard length
                header.Append(Chr(255) + Chr(255) + Chr(255) + Chr(255) + Chr(255) + Chr(255))
            End If

            ' BatchID - Packed Decimal of the Date and Time in MMDDHH format
            header.Append(BuildPackedDecimal(Right("0" + Str(Month(Now())), 2) + Right("0" + Str(Microsoft.VisualBasic.DateAndTime.Day(Now())), 2) + Right("0" + Str(Hour(Now())), 2), 3))

            ' Flags 1 - Set bit 2 to indicate this is a timer batch
            header.Append(Chr(16))

            ' Flags 2 - 00111100
            header.Append(Chr(60))

            ' Msg Data
            header.Append("CHANGE ITEM                   ")

            ' End Characters
            header.Append(Chr(13) + Chr(10))

            ' write the line to the file
            WriteLine(header.ToString, True, False)

        End Sub

#End Region

#Region "Common methods for additional business logic"
        ''' <summary>
        ''' Allow for the Writer to create an additional control file, separate from the POS or Scale Writer, that
        ''' can be sent to the store to kick off a job.
        ''' </summary>
        ''' <remarks>Do  not create control file fro ADDMI maintenance</remarks>
        Public Overrides Function CreateControlFile(ByRef currentStore As StoreUpdatesBO) As Boolean
            Logger.LogDebug("CreateControlFile entry", Me.GetType)
            Logger.LogDebug("CreateControlFile exit", Me.GetType)

            Return True
        End Function

        ''' <summary>
        ''' Do not send the remote command to process the ADDMI maintenance file - the IBM-side process will
        ''' take care of it
        ''' </summary>
        ''' <remarks>Do  not create control file fro ADDMI maintenance</remarks>
        Protected Overrides Sub ssh1_DataReceivedEvent(ByVal Sender As Object, ByVal Args As WeOnlyDo.Client.SSH.DataReceivedArgs) Handles Ssh.DataReceivedEvent
            Logger.LogDebug("ssh1_DataReceivedEvent entry", Me.GetType)
            Logger.LogDebug("ssh1_DataReceivedEvent exit", Me.GetType)
        End Sub

        Public Overrides Function CallSSHRemoteJobProcess(ByRef currentStore As StoreUpdatesBO, ByVal filename As String) As Boolean
            Logger.LogDebug("Call SSH Remote Job Process entry", Me.GetType)
            Logger.LogDebug("Call SSH Remote Job Process exit", Me.GetType)

            Overrides_RemoteSSHStoreList.Append(currentStore.StoreNum.ToString)
            Overrides_RemoteSSHStoreList.Append(" = **ADDMI-No Job**")

            Return True
        End Function

        Public Overrides Function CallRemoteJobProcess(ByRef currentStore As BusinessLogic.StoreUpdatesBO) As Boolean
            Logger.LogDebug("Call Remote Job Process entry", Me.GetType)

            Overrides_RemoteSSHStoreList.Append(currentStore.StoreNum.ToString)
            Overrides_RemoteSSHStoreList.Append(" = **ADDMI-No Job**")

            Logger.LogDebug("Call Remote Job Process exit", Me.GetType)

            Return True
        End Function
#End Region

    End Class
End Namespace

