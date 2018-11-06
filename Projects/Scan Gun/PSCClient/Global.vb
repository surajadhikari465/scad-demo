Option Strict Off
Option Explicit On
Module [Global]

    Public Enum FooterType
        ESC = 1
        Enter = 2
        More = 3
    End Enum

    Structure SelectionType
        Dim Data As Int32
        Dim Info As String
        Dim Not_Available As Boolean
        Dim Mega_Store As Boolean
        Dim WFM_Store As Boolean
        Dim PrimaryVendor As Boolean
    End Structure

    Structure UserStateStructure
        Dim UserName As String
        Dim PrevStatus As Short
        Dim Status As Short
        Dim CurrentString As String
        Dim NextString As String
        Dim ProcessString As String
        Dim SelectionList() As SelectionType
        Dim SelectionStart As Short
        Dim SelectionNext As Boolean
        Dim SelectionPrevious As Boolean
        Dim SelectionMax As Short
        Dim TerminalType As Int32
        Dim Quantity As Decimal
        Dim Weight As Decimal
        Dim Identifier As String
        Dim vUnit_id As Int32
        Dim MobilePrinter_NetworkName As String
        Dim SignCopies As Short
        Dim LastUse As Date
        Dim ExistingQuantity As Decimal
        Dim IsTransferOrder As Boolean
        Dim IsCreditOrder As Boolean
        Dim StartScan As Date
        Dim EndScan As Date
        Dim InvLocID As Long
        Dim InvLocName As String
        Dim IsSameSubTeamScannedForWaste As Boolean
    End Structure

    Structure StatusType
        Public Description As String
        Dim PromptType As Short
    End Structure

    Structure OrderItemList
        Dim OrderItemID As Int32
        Dim Quantity As Decimal
    End Structure

    Public Const lPSC As Short = 0
    Public Const lSymbol As Short = 1

    '-- Status Types
    Public Const StatusClosed As Short = 1
    Public Const Connected As Short = 2
    Public Const ErrorMsg As Short = 3

    Public Const UserName As Short = 100
    Public Const Password As Short = 101
    Public Const StoreMenu As Short = 102
    Public Const FunctionMenu As Short = 103
    Public Const AlreadyLoggedIn As Short = 104
    Public Const InvalidLogon As Short = 105
    Public Const StoreSubteam As Short = 106
    Public Const SetStoreSubteam As Short = 107

    Public Const WasteSubTeam As Short = 200
    Public Const WasteMenu As Short = 201
    Public Const WasteNoItem As Short = 203
    Public Const WasteEnterQuantity As Short = 205
    Public Const WasteNoCost As Short = 204

    Public Const WasteSubTeamVerify As Short = 222
    Public Const WasteSubTeamFixedSpoilage As Short = 223

    Public Const WasteEnterUnit As Short = 206
    Public Const WasteEnterWeight As Short = 207
    Public Const WasteVerify As Short = 208
    Public Const WasteInventorySubTeamConflict As Short = 210

    Public Const WasteUnitsOutofRange As Short = 211
    Public Const AverageUnitWeightMissing As Short = 888

    Public Const WasteTypeMenu As Short = 900
    Public Const WasteSpoilage As Short = 901
    Public Const WasteSampling As Short = 902
    Public Const WasteFoodBank As Short = 903

    Public Const PriceCheckMenu As Short = 300
    Public Const PriceNoItem As Short = 303
    Public Const PriceViewItem As Short = 309
    Public Const PriceDiscontinueItem As Short = 310
    Public Const PriceItemVendorList As Short = 311

    Public Const PerpetualSubTeam As Short = 700
    Public Const PerpetualMenu As Short = 701
    Public Const PerpetualNoItem As Short = 703
    Public Const PerpetualEnterQuantity As Short = 705
    Public Const PerpetualEnterUnit As Short = 706
    Public Const PerpetualEnterWeight As Short = 707
    Public Const PerpetualVerify As Short = 708
    Public Const PerpetualItemInventorySubTeamConflict As Short = 710
    Public Const PerpetualNoMaster As Short = 711
    Public Const PerpetualBeforeStartScan As Short = 712
    Public Const PerpetualAfterEndScan As Short = 713
    Public Const PerpetualSubTeamLoc As Short = 714
    Public Const PerpetualInvLoc As Short = 715
    Public Const PerpetualUnitsOutofRange As Short = 716

    Public Const TransferTransferToSubTeamMenu As Short = 600
    Public Const CreditTransferToSubTeamMenu As Short = 800

    Public Const OrderItemMenu As Short = 1000
    Public Const OrderNoItem As Short = 1001
    Public Const OrderItemNotSold As Short = 1003
    Public Const OrderEnterQuantity As Short = 1004
    Public Const OrderEnterUnit As Short = 1005
    Public Const OrderEnterWeight As Short = 1007
    Public Const OrderTransferToSubTeamMenu As Short = 1028
    Public Const OrderItemInventorySubTeamConflict As Short = 1029

    Public Const ReceiveMenu As Short = 1100
    Public Const ReceiveNoOrder As Short = 1102
    Public Const ReceiveClosedOrder As Short = 1103
    Public Const ReceiveWrongStore As Short = 1105
    Public Const ReceiveNotSent As Short = 1106
    Public Const ReceiveEnterItem As Short = 1108
    Public Const ReceiveNoItem As Short = 1112
    Public Const ReceiveEnterQuantity As Short = 1114
    Public Const ReceiveEnterWeight As Short = 1115
    Public Const ReceiveInventorySubTeamException As Short = 1118

    Public Const PrintSignsMenu As Short = 1200
    Public Const PrintSignsViewItem As Short = 1201
    Public Const PrintSignsNoItem As Short = 1202

    Public Const PrintSignsNowMenu As Short = 1300
    Public Const PrintSignsNowScanItem As Short = 1301
    Public Const PrintSignsNowNoItem As Short = 1302
    Public Const PrintSignsNowViewItem As Short = 1303
    Public Const PrintSignsNowSignType As Short = 1304

    Public Const InvLocSubTeam As Short = 1400
    Public Const InvLocLocation As Short = 1401
    Public Const InvLocScanItem As Short = 1402
    Public Const InvLocNoItem As Short = 1403
    Public Const InvLocItemExists As Short = 1404
    Public Const InvLocInventorySubTeamConflict As Short = 1405
    Public Const InvLocViewItem As Short = 1406

    '-- String Type
    Public Const NameString As Short = 0
    Public Const PasswordString As Short = 1
    Public Const SelectionString As Short = 2
    Public Const IdentifierString As Short = 3
    Public Const QuantityString As Short = 4
    Public Const QuantityExtString As Short = 5
    Public Const QuantityOnlyString As Short = 6
    Public Const EnterString As Short = 7
    Public Const UnitString As Short = 8
    Public Const UnitExtString As Short = 9
    Public Const WeightString As Short = 10
    Public Const VerifyString As Short = 11
    Public Const WasteTypeString As Short = 31
    Public Const RegString As Short = 12
    Public Const OverwriteDeleteString As Short = 13
    Public Const POString As Short = 14
    Public Const VendorPOString As Short = 15
    Public Const ReprintTagsString As Short = 16

    '-- Item Adjustment Types
    Public Const WasteRecord As Short = 1
    Public Const TransferRecord As Short = 7

    '-- Auto email addresses
    Public gsSupportEmail As String

    '-- Status List
    Public tStatus(1499) As StatusType

    '-- Database info
    Public gsCrystal_Connect As String

    Public controller As ScanGunController.Controller
    Public UserState As UserStateStructure

    Public sClrScr As String
    Public sBS As String
    Public sClrEOL As String
    Public sEndScan(1) As String
    Public sBeginScan(1) As String
    Public sCursor(1) As String
    Public sNoCursor(1) As String
    Public sInverseVideo As String
    Public sRegularVideo As String
    Public sSpecialVideo As String
    Public sNoSpecialVideo As String
    Public sClrEOS As String

    Public CurrentSubTeamName As String
    Public ScannedSubTeamName As String

    Public bValid As Boolean
    Public vUnit_id As Int32

    Sub GetUnitID(ByRef sUnitCode As String)

        Select Case sUnitCode
            Case "c"
                UserState.vUnit_id = controller.GetItemUnitID("Case")
            Case "p"
                UserState.vUnit_id = controller.GetItemUnitID("Pound")
            Case "b"
                UserState.vUnit_id = controller.GetItemUnitID("Box")
            Case "u"
                UserState.vUnit_id = controller.GetItemUnitID("Unit")
            Case Else
                Err.Raise(vbObjectError + 518, , "Unknown Unit Code selected")
        End Select

    End Sub

    Public Sub SendMail(ByRef sRecipient As String, ByRef sSubject As String, ByRef sMessage As String)

        On Error Resume Next

        Dim mClient As New System.Net.Mail.SmtpClient
        mClient.SendAsync("SO.ScanGun@WholeFoods.com", sRecipient, sSubject, sMessage, String.Empty)

    End Sub

    Public Sub InitState()

        controller.Clear()

        With UserState
            .CurrentString = ""
            .ProcessString = ""
            .NextString = ""

            ReDim .SelectionList(0)
            .SelectionStart = 0
            .SelectionNext = False
            .SelectionPrevious = False
            .SelectionMax = 0

            .Quantity = 0
            .Weight = 0

            .Identifier = ""

            .vUnit_id = 0
            .SignCopies = 0

            .LastUse = Now

            .IsTransferOrder = False
            .IsCreditOrder = False
            .InvLocID = 0
            .InvLocName = ""
        End With

    End Sub

    Function ProcessString(ByRef sNewString As String, ByRef sOldString As String, ByRef sSendString As String, ByVal lType As Int32) As Boolean

        Dim Char_Renamed As New VB6.FixedLengthString(1)
        Dim InChar As New VB6.FixedLengthString(1)

        Do While Len(sNewString) >= 1
            InChar.Value = Mid(sNewString, 1, 1)
            Char_Renamed.Value = LCase(Mid(sNewString, 1, 1))
            sNewString = Mid(sNewString, 2)
            If Char_Renamed.Value = Chr(27) Then
                ProcessString = True
                sOldString = Chr(27)
                Exit Function
            End If

            Select Case lType
                Case RegString, POString
                    Select Case (Char_Renamed.Value)
                        Case Chr(13) : ProcessString = True
                            Exit Function
                        Case Chr(8)
                            If Len(sOldString) >= 1 Then
                                sOldString = Mid(sOldString, 1, Len(sOldString) - 1)
                                sSendString = sSendString & sBS
                            End If
                        Case Else
                            If InStr(1, "!@#$%^&*~`.;<>,?abcdefghijklmnopqrstuvwxyz 1234567890", Char_Renamed.Value, CompareMethod.Binary) > 0 And Len(sOldString) < IIf(lType = RegString, 18, 9) Then
                                sOldString = sOldString & Char_Renamed.Value
                                sSendString = sSendString & Char_Renamed.Value
                            End If
                    End Select
                Case NameString
                    Select Case (Char_Renamed.Value)
                        Case Chr(13) : ProcessString = True
                            Exit Function
                        Case Chr(8)
                            If Len(sOldString) >= 1 Then
                                sOldString = Mid(sOldString, 1, Len(sOldString) - 1)
                                sSendString = sSendString & sBS
                            End If
                        Case Else
                            If (Char_Renamed.Value >= "a" And Char_Renamed.Value <= "z") Or (Char_Renamed.Value = ".") Then
                                sOldString = sOldString & Char_Renamed.Value
                                sSendString = sSendString & Char_Renamed.Value
                            End If
                    End Select
                Case PasswordString
                    Select Case (Char_Renamed.Value)
                        Case Chr(13) : ProcessString = True
                            Exit Function
                        Case Chr(8)
                            If Len(sOldString) >= 1 Then
                                sOldString = Mid(sOldString, 1, Len(sOldString) - 1)
                                sSendString = sSendString & sBS
                            End If
                        Case Else
                            sOldString = sOldString & InChar.Value
                            sSendString = sSendString & "*"
                    End Select
                Case SelectionString
                    If (Char_Renamed.Value = "p" And UserState.SelectionPrevious) Or (Char_Renamed.Value = "n" And UserState.SelectionNext) Or (Char_Renamed.Value >= "0" And Char_Renamed.Value <= Trim(Str(UserState.SelectionMax))) Then
                        sOldString = Char_Renamed.Value
                        ProcessString = True
                        Exit Function
                    End If
                Case IdentifierString
                    Select Case (Char_Renamed.Value)
                        Case Chr(13) : ProcessString = True
                            '-- Remove extra scans
                            While InStr(1, Mid(sNewString, 2), sNewString, CompareMethod.Binary) > 5
                                sNewString = Mid(sNewString, InStr(1, Mid(sNewString, 2), sNewString, CompareMethod.Binary) + 1)
                            End While
                            Exit Function
                        Case Chr(8)
                            If Len(sOldString) >= 1 Then
                                sOldString = Mid(sOldString, 1, Len(sOldString) - 1)
                                sSendString = sSendString & sBS
                            End If
                            '-- Start Scanning again if nothing has been entered
                            If Len(sOldString) = 0 Then sSendString = sSendString & sBeginScan(UserState.TerminalType)
                        Case Else
                            If Char_Renamed.Value >= "0" And Char_Renamed.Value <= "9" And Len(sOldString) < 18 Then
                                'Ignore leading zeros
                                If Char_Renamed.Value <> "0" Or Len(sOldString) > 0 Then sOldString = sOldString & Char_Renamed.Value

                                'But send back all numeric chars
                                sSendString = sSendString & Char_Renamed.Value
                                '-- End Scanning if something was entered
                                If Len(sOldString) = 1 Then sSendString = sSendString & sEndScan(UserState.TerminalType)
                            End If
                    End Select
                Case QuantityString, QuantityOnlyString, QuantityExtString
                    Select Case (Char_Renamed.Value)
                        Case Chr(13) : ProcessString = True
                            Exit Function
                        Case Chr(8)
                            If Len(sOldString) >= 1 Then
                                sOldString = Mid(sOldString, 1, Len(sOldString) - 1)
                                sSendString = sSendString & sBS
                            End If
                        Case "c", "u"
                            If (sOldString <> "") And (lType = QuantityString Or lType = QuantityExtString) Then
                                ProcessString = True
                                sSendString = sSendString & Char_Renamed.Value
                                sOldString = sOldString & Char_Renamed.Value
                                Exit Function
                            End If
                        Case "b", "p"
                            If (sOldString <> "") And (lType = QuantityExtString) Then
                                ProcessString = True
                                sSendString = sSendString & Char_Renamed.Value
                                sOldString = sOldString & Char_Renamed.Value
                                Exit Function
                            End If
                        Case Else
                            If Char_Renamed.Value >= "0" And Char_Renamed.Value <= "9" Then
                                sOldString = sOldString & Char_Renamed.Value
                                sSendString = sSendString & Char_Renamed.Value
                            End If
                    End Select
                Case EnterString
                    If Char_Renamed.Value = Chr(13) Then
                        ProcessString = True
                        Exit Function
                    End If
                Case UnitString
                    If Char_Renamed.Value = "c" Or Char_Renamed.Value = "u" Then
                        ProcessString = True
                        sSendString = Char_Renamed.Value
                        sOldString = sOldString & Char_Renamed.Value
                        Exit Function
                    End If
                Case UnitExtString
                    If Char_Renamed.Value = "c" Or Char_Renamed.Value = "u" Or Char_Renamed.Value = "b" Or Char_Renamed.Value = "p" Then
                        ProcessString = True
                        sSendString = Char_Renamed.Value
                        sOldString = sOldString & Char_Renamed.Value
                        Exit Function
                    End If
                Case WeightString
                    Select Case (Char_Renamed.Value)
                        Case Chr(13) : ProcessString = True
                            Exit Function
                        Case Chr(8)
                            If Len(sOldString) >= 1 Then
                                sOldString = Mid(sOldString, 1, Len(sOldString) - 1)
                                sSendString = sSendString & sBS
                            End If
                        Case "."
                            If InStr(1, sOldString, Char_Renamed.Value) = 0 Then
                                sOldString = sOldString & Char_Renamed.Value
                                sSendString = sSendString & Char_Renamed.Value
                            End If
                        Case Else
                            If Char_Renamed.Value >= "0" And Char_Renamed.Value <= "9" And Len(sOldString) <= 4 Then
                                sOldString = sOldString & Char_Renamed.Value
                                sSendString = sSendString & Char_Renamed.Value
                            End If
                    End Select
                Case WasteTypeString
                    If Char_Renamed.Value = "p" Or Char_Renamed.Value = "f" Or Char_Renamed.Value = "s" Then
                        ProcessString = True
                        sOldString = sOldString & Char_Renamed.Value
                        Exit Function
                    End If
                Case VerifyString
                    If Char_Renamed.Value = "y" Or Char_Renamed.Value = "n" Then
                        ProcessString = True
                        sOldString = sOldString & Char_Renamed.Value
                        Exit Function
                    End If
                Case OverwriteDeleteString
                    If Char_Renamed.Value = "O" Or Char_Renamed.Value = "D" Then
                        ProcessString = True
                        sOldString = sOldString & Char_Renamed.Value
                        Exit Function
                    End If
                Case ReprintTagsString
                    ProcessString = True
                    Exit Function
            End Select
        Loop

        ProcessString = False

    End Function

    Sub InitVars()

        ReDim UserState.SelectionList(0)

        '-- Set status
        tStatus(StatusClosed).Description = "Closed"
        tStatus(Connected).Description = "Connected"

        tStatus(ErrorMsg).Description = "Error Occurred"
        tStatus(ErrorMsg).PromptType = EnterString

        tStatus(UserName).Description = "Login - Username"
        tStatus(UserName).PromptType = NameString
        tStatus(Password).Description = "Login - Password"
        tStatus(Password).PromptType = PasswordString
        tStatus(StoreMenu).Description = "Store Menu"
        tStatus(StoreMenu).PromptType = SelectionString
        tStatus(FunctionMenu).Description = "Function Menu"
        tStatus(FunctionMenu).PromptType = SelectionString
        tStatus(AlreadyLoggedIn).Description = "Function Menu"
        tStatus(AlreadyLoggedIn).PromptType = EnterString
        tStatus(InvalidLogon).Description = "Invalid Logon"
        tStatus(InvalidLogon).PromptType = EnterString
        tStatus(StoreSubteam).Description = "Store Sub-Team"
        tStatus(StoreSubteam).PromptType = EnterString
        tStatus(SetStoreSubteam).Description = "Set Sub-Team"
        tStatus(SetStoreSubteam).PromptType = SelectionString

        tStatus(WasteTypeMenu).Description = "Shrink Menu"
        tStatus(WasteTypeMenu).PromptType = SelectionString
        tStatus(WasteSubTeam).Description = "Shrink Menu"
        tStatus(WasteSubTeam).PromptType = SelectionString
        tStatus(WasteMenu).Description = "Shrink Menu"
        tStatus(WasteMenu).PromptType = IdentifierString
        tStatus(WasteNoItem).Description = "Shrink Menu"
        tStatus(WasteNoItem).PromptType = EnterString
        tStatus(WasteEnterQuantity).Description = "Shrink Menu"
        tStatus(WasteEnterQuantity).PromptType = QuantityString
        tStatus(WasteEnterUnit).Description = "Shrink Menu"
        tStatus(WasteEnterUnit).PromptType = UnitString
        tStatus(WasteEnterWeight).Description = "Shrink Menu"
        tStatus(WasteEnterWeight).PromptType = WeightString
        tStatus(WasteVerify).Description = "Shrink Menu"
        tStatus(WasteVerify).PromptType = VerifyString
        tStatus(WasteInventorySubTeamConflict).Description = "Shrink Menu"
        tStatus(WasteInventorySubTeamConflict).PromptType = EnterString
        tStatus(WasteUnitsOutofRange).Description = "Shrink Menu"
        tStatus(WasteUnitsOutofRange).PromptType = EnterString

        tStatus(AverageUnitWeightMissing).Description = "Shrink Menu"
        tStatus(AverageUnitWeightMissing).PromptType = EnterString

        tStatus(PriceCheckMenu).Description = "Item Check Menu"
        tStatus(PriceCheckMenu).PromptType = IdentifierString
        tStatus(PriceNoItem).Description = "Item Check Menu"
        tStatus(PriceNoItem).PromptType = EnterString
        tStatus(PriceViewItem).Description = "Item Check Menu"
        tStatus(PriceViewItem).PromptType = EnterString
        tStatus(PriceItemVendorList).Description = "Item Vendor List"
        tStatus(PriceItemVendorList).PromptType = SelectionString

        tStatus(PerpetualSubTeam).Description = "Cycle Count Menu"
        tStatus(PerpetualSubTeam).PromptType = SelectionString
        tStatus(PerpetualMenu).Description = "Cycle Count Menu"
        tStatus(PerpetualMenu).PromptType = IdentifierString
        tStatus(PerpetualNoItem).Description = "Cycle Count Menu"
        tStatus(PerpetualNoItem).PromptType = EnterString
        tStatus(PerpetualEnterQuantity).Description = "Cycle Count Menu"
        tStatus(PerpetualEnterQuantity).PromptType = QuantityString
        tStatus(PerpetualEnterUnit).Description = "Cycle Count Menu"
        tStatus(PerpetualEnterUnit).PromptType = UnitString
        tStatus(PerpetualEnterWeight).Description = "Cycle Count Menu"
        tStatus(PerpetualEnterWeight).PromptType = WeightString
        tStatus(PerpetualVerify).Description = "Cycle Count Menu"
        tStatus(PerpetualVerify).PromptType = VerifyString
        tStatus(PerpetualItemInventorySubTeamConflict).Description = "Cycle Count Menu"
        tStatus(PerpetualItemInventorySubTeamConflict).PromptType = EnterString
        tStatus(PerpetualNoMaster).Description = "Cycle Count Menu"
        tStatus(PerpetualNoMaster).PromptType = EnterString
        tStatus(PerpetualBeforeStartScan).Description = "Cycle Count Menu"
        tStatus(PerpetualBeforeStartScan).PromptType = EnterString
        tStatus(PerpetualAfterEndScan).Description = "Cycle Count Menu"
        tStatus(PerpetualAfterEndScan).PromptType = EnterString
        tStatus(PerpetualSubTeamLoc).Description = "Cycle Count Menu"
        tStatus(PerpetualSubTeamLoc).PromptType = SelectionString
        tStatus(PerpetualInvLoc).Description = "Cycle Count Menu"
        tStatus(PerpetualInvLoc).PromptType = SelectionString
        tStatus(PerpetualUnitsOutofRange).Description = "Cycle Count Menu"
        tStatus(PerpetualUnitsOutofRange).PromptType = EnterString

        tStatus(OrderItemMenu).Description = "Order Menu"
        tStatus(OrderItemMenu).PromptType = IdentifierString
        tStatus(OrderNoItem).Description = "Order Menu"
        tStatus(OrderNoItem).PromptType = EnterString
        tStatus(OrderItemNotSold).Description = "Order Menu"
        tStatus(OrderItemNotSold).PromptType = EnterString
        tStatus(OrderEnterQuantity).Description = "Order Menu"
        tStatus(OrderEnterQuantity).PromptType = QuantityExtString
        tStatus(OrderEnterWeight).Description = "Order Menu"
        tStatus(OrderEnterWeight).PromptType = WeightString
        tStatus(OrderEnterUnit).Description = "Order Menu"
        tStatus(OrderEnterUnit).PromptType = UnitExtString
        tStatus(OrderTransferToSubTeamMenu).Description = "Order Menu"
        tStatus(OrderTransferToSubTeamMenu).PromptType = SelectionString
        tStatus(OrderItemInventorySubTeamConflict).Description = "Order Menu"
        tStatus(OrderItemInventorySubTeamConflict).PromptType = EnterString

        tStatus(TransferTransferToSubTeamMenu).Description = "Transfer Menu"
        tStatus(TransferTransferToSubTeamMenu).PromptType = SelectionString

        tStatus(CreditTransferToSubTeamMenu).Description = "Credit Menu"
        tStatus(CreditTransferToSubTeamMenu).PromptType = SelectionString

        tStatus(ReceiveMenu).Description = "DSD Receiving Menu"
        tStatus(ReceiveMenu).PromptType = POString
        tStatus(ReceiveNoOrder).Description = "DSD Receiving Menu"
        tStatus(ReceiveNoOrder).PromptType = EnterString
        tStatus(ReceiveClosedOrder).Description = "DSD Receiving Menu"
        tStatus(ReceiveClosedOrder).PromptType = EnterString
        tStatus(ReceiveNotSent).Description = "DSD Receiving Menu"
        tStatus(ReceiveNotSent).PromptType = EnterString
        tStatus(ReceiveEnterItem).Description = "DSD Receiving Menu"
        tStatus(ReceiveEnterItem).PromptType = IdentifierString
        tStatus(ReceiveNoItem).Description = "DSD Receiving Menu"
        tStatus(ReceiveNoItem).PromptType = EnterString
        tStatus(ReceiveEnterQuantity).Description = "DSD Receiving Menu"
        tStatus(ReceiveEnterQuantity).PromptType = WeightString
        tStatus(ReceiveEnterWeight).Description = "DSD Receiving Menu"
        tStatus(ReceiveEnterWeight).PromptType = WeightString
        tStatus(ReceiveInventorySubTeamException).Description = "DSD Receiving Menu"
        tStatus(ReceiveInventorySubTeamException).PromptType = EnterString

        tStatus(PrintSignsMenu).Description = "Print Signs Menu"
        tStatus(PrintSignsMenu).PromptType = IdentifierString
        tStatus(PrintSignsNoItem).Description = "Print Signs Menu"
        tStatus(PrintSignsNoItem).PromptType = IdentifierString
        tStatus(PrintSignsViewItem).Description = "Print Sign Check Menu"
        tStatus(PrintSignsViewItem).PromptType = ReprintTagsString

        tStatus(PrintSignsNowMenu).Description = "Print Signs Now Menu"
        tStatus(PrintSignsNowMenu).PromptType = SelectionString
        tStatus(PrintSignsNowScanItem).Description = "Print Signs Now Menu"
        tStatus(PrintSignsNowScanItem).PromptType = IdentifierString
        tStatus(PrintSignsNowNoItem).Description = "Print Signs Now - Item Not Found"
        tStatus(PrintSignsNowNoItem).PromptType = VerifyString
        tStatus(PrintSignsNowViewItem).Description = "Print Signs Now - View Item"
        tStatus(PrintSignsNowViewItem).PromptType = QuantityOnlyString
        tStatus(PrintSignsNowSignType).Description = "Print Signs Now - Sign Type"
        tStatus(PrintSignsNowSignType).PromptType = SelectionString

        tStatus(InvLocSubTeam).Description = "Inventory Location"
        tStatus(InvLocSubTeam).PromptType = SelectionString
        tStatus(InvLocLocation).Description = "Inventory Location"
        tStatus(InvLocLocation).PromptType = SelectionString
        tStatus(InvLocScanItem).Description = "Inventory Location"
        tStatus(InvLocScanItem).PromptType = IdentifierString
        tStatus(InvLocNoItem).Description = "Inventory Location"
        tStatus(InvLocNoItem).PromptType = EnterString
        tStatus(InvLocItemExists).Description = "Inventory Location"
        tStatus(InvLocItemExists).PromptType = EnterString
        tStatus(InvLocInventorySubTeamConflict).Description = "Inventory Location"
        tStatus(InvLocInventorySubTeamConflict).PromptType = EnterString
        tStatus(InvLocViewItem).Description = "Inventory Location"
        tStatus(InvLocViewItem).PromptType = EnterString

        '-- Set emulation codes
        sClrScr = Chr(27) & "[2J"
        sBS = vbBack & " " & vbBack
        sClrEOL = Chr(27) & "[K"

        sBeginScan(1) = Chr(27) & "%1S"
        sEndScan(1) = Chr(27) & "%0S"

        sBeginScan(0) = Chr(27) & "[!1;0z"
        sEndScan(0) = Chr(27) & "[!1;3z"

        sCursor(1) = Chr(27) & "[>5l"
        sNoCursor(1) = Chr(27) & "[>5h"

        sCursor(0) = Chr(27) & "[?25h"
        sNoCursor(0) = Chr(27) & "[?25l"

        sInverseVideo = Chr(27) & "[7m"
        sRegularVideo = Chr(27) & "[m"
        sSpecialVideo = Chr(27) & "(0"
        sNoSpecialVideo = Chr(27) & "(B"
        sClrEOS = Chr(27) & "[0J"

    End Sub
    Sub FillStoreSelection()

        ReDim UserState.SelectionList(0)

        UserState.SelectionStart = 1

        Dim stores As ArrayList = controller.GetThisRegionsStores
        Dim store As ScanGunController.Controller.Store
        Dim i As Int32

        For i = 0 To stores.Count - 1
            store = stores(i)
            If controller.StoreLimit > 0 And controller.StoreLimit <> store.Store_No Then
                'Skip
            Else
                ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
                UserState.SelectionList(UBound(UserState.SelectionList)).Data = store.Store_No
                UserState.SelectionList(UBound(UserState.SelectionList)).Info = store.Store_Name
                UserState.SelectionList(UBound(UserState.SelectionList)).Mega_Store = store.Mega_Store
                UserState.SelectionList(UBound(UserState.SelectionList)).WFM_Store = store.WFM_Store
            End If
        Next

    End Sub
    Sub FillMobilePrinterSelection()

        ReDim UserState.SelectionList(0)

        UserState.SelectionStart = 1

        Dim list As ArrayList = controller.GetWirelessPrinters
        Dim item As ScanGunController.ReferenceList
        Dim i As Long

        For i = 0 To list.Count - 1
            item = list(i)
            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = item.ListID
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = UCase(item.ListDesc)
        Next

    End Sub
    Sub FillWasteTypeSelection()

        ReDim UserState.SelectionList(0)
        UserState.SelectionStart = 1

        If Not controller.IsAccountEnabled Then Exit Sub

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = WasteSpoilage
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Spoilage"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = WasteSampling
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Sampling"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = WasteFoodBank
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Food Bank"
    End Sub

    Sub FillFunctionSelection()

        ReDim UserState.SelectionList(0)
        UserState.SelectionStart = 1

        If Not controller.IsAccountEnabled Then Exit Sub

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = SetStoreSubteam
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Set Sub-Team"

        If controller.IsBuyer Then
            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = OrderTransferToSubTeamMenu
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Order"

            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = TransferTransferToSubTeamMenu
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Transfer"

            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = CreditTransferToSubTeamMenu
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Credit"
        End If

        If controller.IsReceiver Then
            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = ReceiveMenu
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Receive"
        End If

        If controller.IsShrinkUser Then
            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = WasteSubTeam
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Shrink"
        End If

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = PriceCheckMenu
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Item Check"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = PrintSignsNowMenu
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Print Sign(Now)"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = PrintSignsMenu
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Print Sign(Queue)"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = PerpetualSubTeam
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Cycle Cnt Subteam"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = PerpetualSubTeamLoc
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Cycle Cnt Location"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = InvLocSubTeam
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Inventory Location"

    End Sub
    Public Sub FillSignTypeSelection()

        ReDim UserState.SelectionList(0)
        UserState.SelectionStart = 1

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = 0
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Grocery"

        ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
        UserState.SelectionList(UBound(UserState.SelectionList)).Data = 1
        UserState.SelectionList(UBound(UserState.SelectionList)).Info = "Nutrition"

    End Sub
    Sub FillItemSelection(ByVal sScan_Code As String, Optional ByVal IncludeOrderingInfo As Boolean = False, Optional ByVal UseCurrentSubteam As Boolean = True)

        Dim sIdentifier As String
        Dim sIdentifier2 As String
        Dim bItemFound As Boolean
        Dim sIdentifierToLookUp As String = ""

        Try
            ReDim UserState.SelectionList(0)

            UserState.IsSameSubTeamScannedForWaste = True

            UserState.Identifier = sScan_Code

            sIdentifier = sScan_Code
            '-- Strip last 5 digits on the weighted Items
            If Len(sScan_Code) = 11 And CDbl(Mid(sScan_Code, 1, 1)) = 2 Then
                Dim str As String = sScan_Code
                Mid(str, Len(str) - 4, 5) = New String("0", 5)
                sIdentifier2 = str
            Else
                sIdentifier2 = ""
            End If

            Dim item As ScanGunController.Controller.StoreItem

            If Not controller.SetItem(sIdentifier, UseCurrentSubteam) Then
                If sIdentifier2.Length > 0 Then
                    bItemFound = controller.SetItem(sIdentifier2, UseCurrentSubteam)
                    If bItemFound = True Then sIdentifierToLookUp = sIdentifier2
                Else
                    bItemFound = False
                End If
            Else
                bItemFound = True
                sIdentifierToLookUp = sIdentifier
            End If

            UserState.SelectionStart = 1

            If bItemFound Then
                If controller.GetPackSize(controller.GetItemKey(sIdentifierToLookUp), controller.Store_No) = 0 Then
                    ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 2)
                Else
                    item = controller.GetStoreItem
                    Dim currentSubTeam As Integer = item.SubTeam_No
                    Dim scannedSubTeam As Integer = controller.GetItemSubTeamNo(item.Item_Key)

                    CurrentSubTeamName = controller.GetSubTeamName(currentSubTeam)
                    ScannedSubTeamName = controller.GetSubTeamName(scannedSubTeam)

                    If (currentSubTeam <> scannedSubTeam) Then
                        UserState.IsSameSubTeamScannedForWaste = False
                    End If

                    ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
                    UserState.SelectionList(UBound(UserState.SelectionList)).Data = item.Item_Key
                    UserState.SelectionList(UBound(UserState.SelectionList)).Info = item.Sign_Description
                End If
            End If
        Catch ex As ScanGunController.Exception.ItemInventorySubTeamException
            Throw ex
        Catch ex As Exception
            LogError(ex.ToString)
            Throw New System.ApplicationException("Error occurred") 'This is just to throw an error to the caller - actual error is logged above
        End Try

    End Sub
    Function FillVendorSelection() As Boolean

        '-- Displays the Vendor_Key in as list for users to choose
        ReDim UserState.SelectionList(0)

        UserState.SelectionStart = 1

        Dim list As ArrayList = controller.GetVendors
        Dim vend As ScanGunController.Controller.Vendor
        Dim i As Long

        If list.Count = 0 Then
            FillVendorSelection = False
        Else
            For i = 0 To list.Count - 1
                vend = list(i)
                ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
                UserState.SelectionList(UBound(UserState.SelectionList)).Data = vend.ID
                UserState.SelectionList(UBound(UserState.SelectionList)).Info = vend.Key
                UserState.SelectionList(UBound(UserState.SelectionList)).PrimaryVendor = vend.IsPrimary

                If vend.Cost > 0 Then
                    If vend.Cost <= 9999.99 Then
                        UserState.SelectionList(UBound(UserState.SelectionList)).Info = IIf(vend.Key.Length < 10, vend.Key & Space(10 - vend.Key.Length), vend.Key) & " " & Space(7 - VB6.Format(vend.Cost, "###0.00").Trim.Length) & VB6.Format(vend.Cost, "###0.00").Trim
                    Else
                        UserState.SelectionList(UBound(UserState.SelectionList)).Info = IIf(vend.Key.Length < 10, vend.Key & Space(10 - vend.Key.Length), vend.Key) & " ****.**"
                    End If
                End If
            Next

            FillVendorSelection = True
        End If

    End Function

    Function GotoXY(ByRef x As Short, ByRef Y As Short) As String
        '-- Move the cursor to a position on the screen
        GotoXY = Chr(27) & "[" & Y & ";" & x & "H"

    End Function

    Function ShowSelection() As String

        Dim lLoop As Int32
        Dim sString As String = String.Empty

        UserState.SelectionPrevious = UserState.SelectionStart > 1
        UserState.SelectionNext = (UBound(UserState.SelectionList) - UserState.SelectionStart + 1) > 10
        UserState.SelectionMax = -1

        sString = sString & sNoCursor(UserState.TerminalType) & GotoXY(1, 4) & sClrEOL
        If UserState.SelectionPrevious Then sString = sString & "P) PREVIOUS SCREEN"

        For lLoop = UserState.SelectionStart To UserState.SelectionStart + 9
            sString = sString & GotoXY(1, 5 + lLoop - UserState.SelectionStart) & sClrEOL
            If lLoop <= UBound(UserState.SelectionList) Then
                sString = sString & ((lLoop - UserState.SelectionStart) Mod 10) & IIf(UserState.SelectionList(lLoop).PrimaryVendor, "*", " ") & Trim(Mid(UserState.SelectionList(lLoop).Info, 1, 18))
                UserState.SelectionMax = (lLoop - UserState.SelectionStart) Mod 10
            End If
        Next lLoop

        sString = sString & GotoXY(1, 15) & sClrEOL
        If UserState.SelectionNext Then sString = sString & "N) NEXT SCREEN"

        ShowSelection = sString

    End Function

    Function ShowHeader(ByRef sHeader As String) As String

        Dim sString As String

        If UserState.TerminalType = 2 Then
            sString = sSpecialVideo & GotoXY(1, 1) & sClrEOL & "l" & New String("q", 18) & "k" & GotoXY(1, 2) & sClrEOL & "x" & GotoXY(20, 2) & "x"
            sString = sString & GotoXY(1, 3) & sClrEOL & "m" & New String("q", 18) & "j" & sNoSpecialVideo
            sString = sString & CenterString(2, sHeader)
        Else
            sString = sSpecialVideo & GotoXY(1, 1) & sClrEOL & "l" & New String("q", 19) & "k" & GotoXY(1, 2) & sClrEOL & "x" & GotoXY(21, 2) & "x"
            sString = sString & GotoXY(1, 3) & sClrEOL & "m" & New String("q", 19) & "j" & sNoSpecialVideo
            sString = sString & CenterString(2, sHeader)
        End If
        If controller.Store_Name <> "" Then sString = sString & CenterString(1, " " & controller.Store_Name & " ")

        ShowHeader = sString

    End Function

    Function ShowFooter(ByVal Type As FooterType) As String
        Dim sFooter As String = String.Empty
        Dim sSubTeam As String = String.Empty

        Select Case Type
            Case FooterType.Enter
                sFooter = " PRESS ENTER "
            Case FooterType.ESC
                sSubTeam = controller.GetSubTeam.Abbreviation
                If sSubTeam.Length > 0 Then
                    sFooter = sSubTeam & " | ESC TO EXIT "
                Else
                    sFooter = " ESC TO EXIT "
                End If
            Case FooterType.More
                sFooter = " ESC | ENTER(MORE) "
        End Select

        ShowFooter = GotoXY(1, 16) & sInverseVideo & sClrEOL & GotoXY(((21 - Len(sFooter)) \ 2) + 1, 16) & sFooter & sRegularVideo

    End Function

    Function LogonUser(ByVal Password As String) As Boolean

        Try
            controller.LogonUser(Trim(UserState.UserName), Password)
            Return True
        Catch ex As ScanGunController.Exception.InvalidLogonException
            Return False
        Catch ex As System.Exception
            LogError("GetUserInfo: " & ex.ToString)
            Throw New System.ApplicationException("Error occurred")
        End Try

    End Function
    Function GetItemInfo(Optional ByVal IncludePrice As Boolean = False) As String

        Dim sString As String
        Dim si As ScanGunController.Controller.StoreItem = controller.GetStoreItem

        sString = si.Sign_Description & vbCrLf & vbCrLf & "Identifier: " & si.Identifier & vbCrLf

        If IncludePrice Then
            If si.IsSoldByWeight Then
                sString = sString & "Price: " & VB6.Format(si.Price / si.Multiple, "$####0.00") & "/lb" & vbCrLf
            Else
                sString = sString & "Price:      " & si.Multiple & " / " & VB6.Format(si.Price, "$####0.00") & vbCrLf
            End If
            If si.IsOnSale Then
                If si.IsSoldByWeight Then
                    sString = sString & "Sale: " & VB6.Format(si.SalePrice / si.SaleMultiple, "$####0.00") & "/lb" & vbCrLf
                Else
                    sString = sString & "Sale:       " & si.SaleMultiple & " / " & VB6.Format(si.SalePrice, "$####0.00") & vbCrLf
                End If

                sString = sString & "Start: " & si.SaleStartDate & vbCrLf
                sString = sString & "End: " & si.SaleEndDate & vbCrLf
            End If
        End If

        sString = sString & "Price Type: " & si.PriceChgTypeDesc & vbCrLf
        sString = sString & "Pkg:        " & si.Package_Desc1.ToString("#####.####") & " / " & si.Package_Desc2.ToString("#####.####") & " " & si.Package_Unit_Abbr & vbCrLf

        sString = sString & "Subteam:    " & si.SubTeam_Name & vbCrLf
        sString = sString & "PV: " & Mid(si.VendorName, 1, 25) & vbCrLf

        If si.IsSoldByWeight Then
            Select Case UserState.Status
                Case WasteEnterQuantity
                    UserState.Status = WasteEnterWeight
                Case PerpetualEnterQuantity
                    UserState.Status = PerpetualEnterWeight
                Case OrderEnterQuantity
                    UserState.Status = OrderEnterWeight
            End Select
        ElseIf ScanGunController.Controller.IsRetailNotCostedByWeight(si.Item_Key) Then
            UserState.Status = WasteEnterQuantity
        End If

        GetItemInfo = sString

    End Function
    Public Function GetItemCycleCountInfo(Optional ByVal InventoryLocationID As Long = 0) As String

        Dim sString As String
        Dim ci As ScanGunController.Controller.CycleCountInfo = controller.GetCycleCountInfo(InventoryLocationID)
        Dim si As ScanGunController.Controller.StoreItem = controller.GetStoreItem
        If si.IsSoldByWeight Then
            sString = "TOTAL COUNTED" & vbCrLf & "WT: " & ci.Weight.ToString("N")
        Else
            sString = "TOTAL COUNTED" & vbCrLf & "QTY: " & ci.Quantity.ToString("N")
        End If
        Return sString

    End Function
    Public Function GetItemOrderInfo() As String
        Dim sString As String = String.Empty
        Dim oi As ScanGunController.Controller.OrderInfo = controller.GetOrderInfo
        If Not UserState.IsCreditOrder And Not UserState.IsTransferOrder Then
            sString = sString & "Ordered: " & oi.OnOrder.ToString("0") & vbCrLf
            sString = sString & "Queued: " & oi.InQueue.ToString("0") & vbCrLf
            If oi.LastReceived > 0 Then
                sString = sString & "RCV: " & oi.LastReceived.ToString("0") & "\" & oi.LastReceivedDate.ToString("M-d-yy") & vbCrLf
            Else
                sString = sString & "RCV: 0" & vbCrLf
            End If
            sString = sString & "PVnd: " & IIf(oi.PrimaryVendorKey.Length > 0, oi.PrimaryVendorKey, IIf(oi.PrimaryVendorName.Length > 0, oi.PrimaryVendorName.Substring(0, IIf(oi.PrimaryVendorName.Length >= 10, 10, oi.PrimaryVendorName.Length)), String.Empty))
        End If
        If UserState.IsTransferOrder Then sString = sString & "Queued: " & oi.InQueueTransfer.ToString("0") & vbCrLf
        If UserState.IsCreditOrder Then sString = sString & "Queued: " & oi.InQueueCredit.ToString("0") & vbCrLf
        Return sString
    End Function

    Function CenterString(ByRef Y As Short, ByRef sString As String) As String

        CenterString = GotoXY(((21 - Len(sString)) \ 2) + 1, Y) & sString

    End Function

    Function GetVendorUnitQuantity() As String

        Dim si As ScanGunController.Controller.StoreItem = controller.GetStoreItem

        UserState.vUnit_id = si.VendorOrderUnitID

        If UserState.vUnit_id <> 0 Then
            GetVendorUnitQuantity = UCase(si.VendorOrderUnitName) & " QUANTITY "
        Else
            GetVendorUnitQuantity = "QUANTITY? "
        End If

    End Function

    Public Sub LogError(ByRef sErrDesc As String)

        On Error Resume Next

        FileOpen(99, VB6.GetPath & "\wireless_errs.txt", OpenMode.Append)
        If Err.Number <> 0 Then
            SendMail(gsSupportEmail, "Wireless: LogError Failed", "Error opening " & VB6.GetPath & "\wireless_errs.txt - " & Err.Description & vbCrLf & "The error that could not be logged was " & sErrDesc)
        Else
            WriteLine(99, CStr(Now) & vbCrLf & "User_ID = " & controller.User_ID.ToString & " " & sErrDesc & vbCrLf)
            If Err.Number <> 0 Then
                SendMail(gsSupportEmail, "Wireless: LogError Failed", "Error writing to " & VB6.GetPath & "\wireless_errs.txt - " & Err.Description & vbCrLf & "The error that could not be logged was " & sErrDesc)
            End If
        End If

        FileClose(99)

    End Sub
    Sub FillStoreSubTeamSelection()

        ReDim UserState.SelectionList(0)

        UserState.SelectionStart = 1

        Dim list As ArrayList = controller.GetStoreSubTeams
        Dim subteam As ScanGunController.Controller.SubTeam
        Dim i As Int32

        For i = 0 To list.Count - 1
            subteam = list(i)
            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = subteam.Number
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = UCase(subteam.Name)
        Next

    End Sub
    Sub FillInventoryLocationSelection()

        ReDim UserState.SelectionList(0)

        UserState.SelectionStart = 1

        Dim list As ArrayList = controller.GetInventoryLocations
        Dim loc As ScanGunController.Controller.InventoryLocation
        Dim i As Int32

        For i = 0 To list.Count - 1
            loc = list(i)
            ReDim Preserve UserState.SelectionList(UBound(UserState.SelectionList) + 1)
            UserState.SelectionList(UBound(UserState.SelectionList)).Data = loc.ID
            UserState.SelectionList(UBound(UserState.SelectionList)).Info = UCase(loc.Name)
        Next

    End Sub
End Module
