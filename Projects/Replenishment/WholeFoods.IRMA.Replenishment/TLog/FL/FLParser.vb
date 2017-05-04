Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Diagnostics
Imports System.IO
Imports System.Security

Namespace WholeFoods.IRMA.Replenishment.TLog
    Public Class FLParser
        Public Enum DWFileType
            DWCMCARD
            DWCMRESERVE
            DWCMREWARD
            DWCMVAR
            DWDISCNT
            DWGIFTCRD
            DWITEM
            DWMRKDWN
            DWPERF
            DWTAXREC
            DWTENDER
        End Enum
        Private factory As New DataFactory(DataFactory.ItemCatalog)
        Private _LoginInfo As TlogLoginInfo


        Sub New(ByVal LoginInfo As TlogLoginInfo)
            factory.CommandTimeout = 3000
            _LoginInfo = LoginInfo
        End Sub



        Public Sub UpdateAggregates(ByRef Region As String, ByRef UseModifiedSubteamNo As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam

            CurrentParam = New DBParam
            CurrentParam.Name = "Region"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Region
            paramList.Add(CurrentParam)

            CurrentParam = New DBParam
            CurrentParam.Name = "UseModifiedSubteamNo"
            CurrentParam.Type = DBParamType.Bit
            CurrentParam.Value = IIf(UseModifiedSubteamNo = "True", 1, 0)
            paramList.Add(CurrentParam)



            Try
                factory.ExecuteStoredProcedure("Replenishment_Tlog_House_UpdateAggregates", paramList)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Public Sub CreateSalesFromItemHistory(ByVal ProcessDate As DateTime, ByVal StoreNo As Integer)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Date"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = ProcessDate.ToString("MM/dd/yyyy")
            paramList.Add(CurrentParam)

            CurrentParam = New DBParam
            CurrentParam.Name = "StoreNo"
            CurrentParam.Type = DBParamType.Int
            CurrentParam.Value = StoreNo
            paramList.Add(CurrentParam)
            Try

                factory.ExecuteStoredProcedure("UpdateItemHistoryFromSales", paramList)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub
        Public Sub MovedatafromQueue()
            Dim paramList As ArrayList = New ArrayList
            Try

                factory.ExecuteStoredProcedure("Moveotherdatatohist", paramList)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Public Sub ClearLoadTables()
            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_ClearLoadTables")
        End Sub
        Public Sub ImportDWFile(ByVal Path As String, ByVal DWType As DWFileType)
            Select Case DWType
                Case DWFileType.DWCMCARD
                    ImportDWCMCARD(Path)
                Case DWFileType.DWCMRESERVE
                    ImportDWCMRESERVE(Path)
                Case DWFileType.DWCMREWARD
                    ImportDWCMREWARD(Path)
                Case DWFileType.DWCMVAR
                    ImportDWCMVAR(Path)
                Case DWFileType.DWDISCNT
                    ImportDWDISCNT(Path)
                Case DWFileType.DWGIFTCRD
                    ImportDWGIFTCRD(Path)
                Case DWFileType.DWITEM
                    ImportDWITEM(Path)
                Case DWFileType.DWMRKDWN
                    ImportDWMRKDWN(Path)
                Case DWFileType.DWPERF
                    ImportDWPERF(Path)
                Case DWFileType.DWTAXREC
                    ImportDWTAXREC(Path)
                Case DWFileType.DWTENDER
                    ImportDWTENDER(Path)
            End Select
        End Sub



        Private Sub ImportDWCMCARD(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWCMCARD", paramList)
        End Sub
        Private Sub ImportDWCMRESERVE(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWCMRESERVE", paramList)
        End Sub
        Private Sub ImportDWCMREWARD(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWCMREWARD", paramList)
        End Sub
        Private Sub ImportDWCMVAR(ByVal Path As String)

            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWCMVAR", paramList)

        End Sub
        Private Sub ImportDWDISCNT(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWDISCNT", paramList)
        End Sub
        Private Sub ImportDWGIFTCRD(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWGIFTCRD", paramList)
        End Sub
        Private Sub ImportDWITEM(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            Dim Pass As SecureString = New SecureString()
            For Each x As Char In _LoginInfo.ProcessPass.ToCharArray
                Pass.AppendChar(x)
            Next


            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            If _LoginInfo.RunFromClient Then

                factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWITEM", paramList)
            Else
                '########### testing for new "Copy Local" parsing method to cut down on time ############
                File.Copy(Path, ".\DWITEM.001", True)
                Dim p As Process = New Process
                p.StartInfo.CreateNoWindow = True
                p.StartInfo.UserName = _LoginInfo.ProcessUser
                p.StartInfo.Password = Pass
                p = Process.Start("bcp.exe", _LoginInfo.Database & ".dbo.Tlog_Item in .\DWITEM.001 -S " & _LoginInfo.DBServer & " -U " & _LoginInfo.DBUser & " -P " & _LoginInfo.DBPass & " -c -t,")
                p.WaitForExit()
                p.Dispose()
            End If
            Pass.Dispose()




        End Sub
        Private Sub ImportDWMRKDWN(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWMRKDWN", paramList)
        End Sub
        Private Sub ImportDWPERF(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWPERF", paramList)
        End Sub
        Private Sub ImportDWTAXREC(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)

            factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWTAXREC", paramList)
        End Sub
        Private Sub ImportDWTENDER(ByVal Path As String)
            Dim paramList As ArrayList = New ArrayList
            Dim CurrentParam As DBParam = New DBParam
            Dim Pass As SecureString = New SecureString()
            For Each x As Char In _LoginInfo.ProcessPass.ToCharArray
                Pass.AppendChar(x)
            Next


            CurrentParam.Name = "Path"
            CurrentParam.Type = DBParamType.String
            CurrentParam.Value = Path
            paramList.Add(CurrentParam)


            If _LoginInfo.RunFromClient Then
                factory.ExecuteStoredProcedure("Replenishment_Tlog_House_LoadDWTENDER", paramList)
            Else
                ' ########### testing for new "Copy Local" parsing method to cut down on time ############

                File.Copy(Path, ".\DWTENDER.001", True)
                Dim p As Process = New Process
                p.StartInfo.CreateNoWindow = True
                p.StartInfo.UserName = _LoginInfo.ProcessUser
                p.StartInfo.Password = Pass
                p = Process.Start("bcp.exe", _LoginInfo.Database & ".dbo.Tlog_Tender in .\DWTENDER.001 -S " & _LoginInfo.DBServer & " -U " & _LoginInfo.DBUser & " -P " & _LoginInfo.DBPass & " -c -t,")
                p.WaitForExit()
                p.Dispose()
            End If
            Pass.Dispose()


        End Sub
    End Class
End Namespace

