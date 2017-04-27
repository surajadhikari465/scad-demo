Imports Microsoft.VisualBasic
Imports SLIM.WholeFoods.Utility.SMTP

Public Class EmailNotifications

    Dim StoreName As String
    Dim StoreNo As String
    Dim SubTeamName As String
    Dim SubTeamNo As String
    Dim MailType As String
    Dim UPC As String
    Dim UserName As String
    Dim Vendor As String
    Dim Description As String
    Dim UserID As Integer
    Dim ItemKey As Integer
    Dim EmailID As Integer
    Dim CurrentPrice As String
    Dim CurrentCost As String
    Dim NewCost As String
    Dim NewPrice As String
    Dim CurrentCaseSize As String
    Dim NewCaseSize As String
    Dim StartDate As String
    Dim EndDate As String
    Dim RejectReason As String
    Dim Requestor As String
    Dim RequestStore As String
    Dim MessageBody As String

#Region "Properties"

    Public Property EmailType() As String
        Get
            Return MailType
        End Get
        Set(ByVal value As String)
            MailType = value
        End Set
    End Property

    Public Property Identifier() As String
        Get
            Return UPC
        End Get
        Set(ByVal value As String)
            UPC = value
        End Set
    End Property

    Public Property SubTeam_Name() As String
        Get
            Return SubTeamName
        End Get
        Set(ByVal value As String)
            SubTeamName = value
        End Set
    End Property
    Public Property SubTeam_No() As String
        Get
            Return SubTeamNo
        End Get
        Set(ByVal value As String)
            SubTeamNo = value
        End Set
    End Property

    Public Property Store_Name() As String
        Get
            Return StoreName
        End Get
        Set(ByVal value As String)
            StoreName = value
        End Set
    End Property
    Public Property Store_No() As String
        Get
            Return StoreNo
        End Get
        Set(ByVal value As String)
            StoreNo = value
        End Set
    End Property

    Public Property User() As String
        Get
            Return UserName
        End Get
        Set(ByVal value As String)
            UserName = value
        End Set
    End Property
    Public Property User_ID() As Integer
        Get
            Return UserID
        End Get
        Set(ByVal value As Integer)
            UserID = value
        End Set
    End Property
    Public Property Email_ID() As Integer
        Get
            Return EmailID
        End Get
        Set(ByVal value As Integer)
            EmailID = value
        End Set
    End Property

    Public Property ItemDescription() As String
        Get
            Return Description
        End Get
        Set(ByVal value As String)
            Description = value
        End Set
    End Property

    Public Property VendorName() As String
        Get
            Return Vendor
        End Get
        Set(ByVal value As String)
            Vendor = value
        End Set
    End Property
    Public Property Item_Key() As Integer
        Get
            Return ItemKey
        End Get
        Set(ByVal value As Integer)
            ItemKey = value
        End Set
    End Property
    Public Property Current_Price() As String
        Get
            Return CurrentPrice
        End Get
        Set(ByVal value As String)
            CurrentPrice = value
        End Set
    End Property
    Public Property Current_Cost() As String
        Get
            Return CurrentCost
        End Get
        Set(ByVal value As String)
            CurrentCost = value
        End Set
    End Property
    Public Property New_Cost() As String
        Get
            Return NewCost
        End Get
        Set(ByVal value As String)
            NewCost = value
        End Set
    End Property
    Public Property New_Price() As String
        Get
            Return NewPrice
        End Get
        Set(ByVal value As String)
            NewPrice = value
        End Set
    End Property
    Public Property Start_Date() As String
        Get
            Return StartDate
        End Get
        Set(ByVal value As String)
            StartDate = value
        End Set
    End Property
    Public Property End_Date() As String
        Get
            Return EndDate
        End Get
        Set(ByVal value As String)
            EndDate = value
        End Set
    End Property
    Public Property New_CaseSize() As String
        Get
            Return NewCaseSize
        End Get
        Set(ByVal value As String)
            NewCaseSize = value
        End Set
    End Property
    Public Property Current_CaseSize() As String
        Get
            Return CurrentCaseSize
        End Get
        Set(ByVal value As String)
            CurrentCaseSize = value
        End Set
    End Property
    Public Property Reject_Reason() As String
        Get
            Return RejectReason
        End Get
        Set(ByVal value As String)
            RejectReason = value
        End Set
    End Property
    Public Property Reques_tor() As String
        Get
            Return Requestor
        End Get
        Set(ByVal value As String)
            Requestor = value
        End Set
    End Property
    Public Property Request_Store() As String
        Get
            Return RequestStore
        End Get
        Set(ByVal value As String)
            RequestStore = value
        End Set
    End Property
    Public Property Message_Body() As String
        Get
            Return MessageBody
        End Get
        Set(ByVal value As String)
            MessageBody = value
        End Set
    End Property
#End Region

    Private Function GetAllEmailAddresses() As String
        Dim da As New SlimEmailTableAdapters.SlimEmailTableAdapter
        Dim dt As New SlimEmail.SlimEmailDataTable
        Dim dr As SlimEmail.SlimEmailRow = dt.NewSlimEmailRow
        Dim TeamEmail As String = String.Empty
        Dim BaEmail As String = String.Empty
        Dim OtherEmail As String = String.Empty
        Dim CCMail As String = String.Empty
       
        dt = da.GetEmails(Email_ID)

        If Not dt.Rows.Count = 0 Then
            dr = dt.Rows(0)
            If (dr.IsOther_emailNull) Then
                OtherEmail = ""
            Else
                If Not Trim(dr.Other_email) = "" Then
                    OtherEmail = Trim(dr.Other_email) & ";"
                End If
            End If
            If (dr.IsTeamLeader_emailNull) Then
                TeamEmail = ""
            Else
                If Not Trim(dr.TeamLeader_email) = "" Then
                    TeamEmail = Trim(dr.TeamLeader_email) & ";"
                End If
            End If
            If (dr.IsBA_emailNull) Then
                BaEmail = ""
            Else
                If Not Trim(dr.BA_email) = "" Then
                    BaEmail = Trim(dr.BA_email) & ";"
                End If
            End If
            CCMail = Trim(TeamEmail & BaEmail & OtherEmail)
            If CCMail.EndsWith(";") Then
                CCMail = CCMail.Remove(((CCMail.Length) - 1), 1)
            End If
        End If
        Return CCMail
    End Function


    Public Sub SentEmail()
        Dim da As New SlimEmailTableAdapters.SlimEmailTableAdapter
        Dim RegOff As Integer
        Dim RegTeam As Integer
        Dim Team As Integer
        RegOff = CInt(HttpContext.Current.Application.Get("RegionalCorporateOffice"))
        RegTeam = CInt(HttpContext.Current.Application.Get("RegionalTeam"))
        If SubTeamNo = 0 Then
            SubTeam_No = da.GetItemsSubTeam(Item_Key)
            Team = da.GetTeamBySubTeam(SubTeam_No)
            SubTeamName = da.GetSubTeamName(SubTeam_No)
        ElseIf Not SubTeam_Name = Nothing Then
            SubTeamName = SubTeam_Name
        Else
            Team = da.GetTeamBySubTeam(SubTeam_No)
            SubTeamName = da.GetSubTeamName(SubTeam_No)
        End If

        ' **** Send Mail out to the blokes ****
        Dim MessageBody As String
        Dim CCEmail As String = ""
        Dim HeadLine As String = ""
        Select Case MailType
            Case "ItemRequest"
                HeadLine = "********** New ItemRequest **************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "ItemRequestReject"
                HeadLine = "********** New ItemRequest was Rejected **************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "VendorRequest"
                HeadLine = "********** New VendorRequest **************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "EIM/SLIM Push"
                HeadLine = "********** New Item Submitted to EIM **************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "InStoreSpecial"
                HeadLine = "********** New In Store Special **************" & vbCrLf
                Try
                    Dim CCEmailBA As String = ""
                    Dim CCEmailTL As String = ""
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()

                    ' ****** Get Regional Team BAs *********
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, Team)) = True, 0, da.EmailExists(RegOff, Team))
                    If Not Email_ID = 0 Then
                        CCEmail = CCEmail & ";" & GetAllEmailAddresses()
                    End If

                    ' ****** Get Store Team Leader *********
                    Email_ID = IIf(IsDBNull(da.EmailExists(StoreNo, Team)) = True, 0, da.EmailExists(StoreNo, Team))
                    If Not Email_ID = 0 Then
                        CCEmail = CCEmail & ";" & GetAllEmailAddresses()
                    End If
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "InStoreSpecialReject"
                HeadLine = "********** New In Store Special was Rejected **************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "InStoreSpecialProcess"
                HeadLine = "********** New In Store Special was Processed **************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "RetailCost"
                HeadLine = "********** New Retail/Cost Change **************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            Case "Authorization"
                HeadLine = "************** New Item Authorization ******************" & vbCrLf

            Case "Ecommerce"
                HeadLine = "************** New Item ECommerce ******************" & vbCrLf

            Case "EndSaleEarly"
                HeadLine = "*************** End Sale Early *******************" & vbCrLf
                Try
                    Email_ID = IIf(IsDBNull(da.EmailExists(RegOff, RegTeam)) = True, 0, da.EmailExists(RegOff, RegTeam))
                    CCEmail = GetAllEmailAddresses()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try

        End Select
        If Not Email_ID = 0 Then
            If CCEmail.Length = 0 Then
                CCEmail = GetAllEmailAddresses()
            End If
            MessageBody = CreateMessageBody(HeadLine)

            If CCEmail.StartsWith(";") Then
                CCEmail = CCEmail.Remove(0, 1)
            End If

            EmailSMTP(CCEmail, MessageBody)
        End If

    End Sub
    Private Function ParseEmail(ByVal dr As SlimEmail.SlimEmailRow) As String
        Dim ccMailAddress As String
        ccMailAddress = dr.BA_email & ";" & dr.TeamLeader_email & ";" & dr.Other_email
        Return ccMailAddress
    End Function
    Private Function CreateMessageBody(ByVal Headline As String) As String
        Dim msgBody As String = Headline & vbCrLf & "Date: " & Date.Now & vbCrLf
        msgBody &= "*************************************************************" & vbCrLf
        msgBody &= "*************************************************************" & vbCrLf
        msgBody &= "User: " & vbTab & UserName & vbCrLf
        msgBody &= "Store: " & vbTab & StoreName & vbCrLf
        msgBody &= "SubTeam: " & vbTab & SubTeamName & vbCrLf
        Select Case MailType
            Case "ItemRequest"
                msgBody &= "UPC: " & vbTab & UPC & vbCrLf
                msgBody &= "Item Description: " & vbTab & Description & vbCrLf
                msgBody &= "Price: " & vbTab & NewPrice & vbCrLf
                msgBody &= "Cost: " & vbTab & NewCost & vbCrLf
            Case "ItemRequestReject"
                msgBody &= "UPC: " & vbTab & UPC & vbCrLf
                msgBody &= "Item Description: " & vbTab & Description & vbCrLf
                msgBody &= "Requested By: " & vbTab & Requestor & vbCrLf
                msgBody &= "Reject Reason: " & vbTab & RejectReason & vbCrLf
            Case "InStoreSpecialReject"
                msgBody &= MessageBody
            Case "VendorRequest"
                msgBody &= "New Vendor: " & vbTab & Vendor & vbCrLf
            Case "IrmaPush"
                msgBody &= "UPC: " & vbTab & UPC & vbCrLf
                msgBody &= "Item Description: " & vbTab & Description & vbCrLf
                msgBody &= "Price: " & vbTab & NewPrice & vbCrLf
                msgBody &= "Cost: " & vbTab & NewCost & vbCrLf
            Case "InStoreSpecial"
                msgBody &= "UPC: " & vbTab & UPC & vbCrLf
                msgBody &= "Item Description: " & vbTab & Description & vbCrLf
                msgBody &= "Current Price: " & vbTab & "$" & CurrentPrice & vbCrLf
                msgBody &= "New Price: " & vbTab & "$" & NewPrice & vbCrLf
                msgBody &= "Start Date: " & vbTab & StartDate & vbCrLf
                msgBody &= "End Date: " & vbTab & EndDate & vbCrLf
            Case "InStoreSpecialProcess"
                msgBody &= MessageBody
            Case "EndSaleEarly"
                msgBody &= MessageBody
            Case "RetailCost"
                msgBody &= "UPC: " & vbTab & UPC & vbCrLf
                msgBody &= "Item Description: " & vbTab & Description & vbCrLf
                msgBody &= "Current Price: " & vbTab & CurrentPrice & vbCrLf
                msgBody &= "New Price: " & vbTab & "$" & NewPrice & vbCrLf
                msgBody &= "Current Cost: " & vbTab & CurrentCost & vbCrLf
                msgBody &= "New Cost: " & vbTab & "$" & NewCost & vbCrLf
                msgBody &= "Current CaseSize: " & vbTab & CurrentCaseSize & vbCrLf
                msgBody &= "New CaseSize: " & vbTab & "$" & NewCaseSize & vbCrLf
            Case "Authorization"
                msgBody &= "UPC: " & vbTab & UPC & vbCrLf
                msgBody &= "Item Description: " & vbTab & Description & vbCrLf
                'removed due to no longer associating item and vendor
                'msgBody &= "Vendor: " & vbTab & Vendor & vbCrLf
                msgBody &= "Authorized By: " & vbTab & Requestor & vbCrLf

            Case "ECommerce"
                msgBody &= "UPC: " & vbTab & UPC & vbCrLf
                msgBody &= "Item Description: " & vbTab & Description & vbCrLf
                msgBody &= "Updated By: " & vbTab & Requestor & vbCrLf

        End Select

        msgBody &= "*************************************************************" & vbCrLf
        msgBody &= "*************************************************************" & vbCrLf
        Return msgBody
    End Function

    Private Sub EmailSMTP(ByVal msgCC As String, ByVal msgBody As String)
        Const host As String = "smtp.WholeFoods.com"
        Const msgFrom As String = "SLIM@wholefoods.com"
        Dim smtp As New SMTP(host)
        Dim msgSubject As String
        Dim user As String = API.GetADUserInfo(UserName, "mail")
        Dim msgTo As String = HttpContext.Current.Application.Get("Admin") & ";" & user

        msgSubject = "IRMA-SLIM - E-Mail Notification (" & UCase(GetEnvironment) & ")"

        If (MailType = "ItemRequestReject" Or MailType = "InStoreSpecialReject") Then
            msgTo &= ";" & Requestor & "@wholefoods.com"
        End If

        Try
            smtp.send(msgBody, msgTo, msgCC, msgFrom, msgSubject)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Error_Log.throwException(ex.Message, ex)
        End Try
    End Sub

    Private Function GetEnvironment() As String
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        GetEnvironment = CStr(factory.ExecuteScalar("SELECT Environment FROM Version"))
    End Function
End Class
