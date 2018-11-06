
Partial Class UserInterface_InStoreSpecials_StoreSpecialsComment
    Inherits System.Web.UI.Page

    Protected Sub RejectStoreSpecial(ByRef RequestID As String, ByRef ProcessedBy As String, ByRef Comments As String)
        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList

        Try
            currentParam = New DBParam
            currentParam.Name = "RequestId"
            currentParam.Value = CType(RequestID, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ProcessedBy"
            currentParam.Value = ProcessedBy
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Comments"
            currentParam.Value = Comments
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            df.ExecuteStoredProcedure("SLIM_RejectStoreSpecial", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub Button_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        Dim aryRequestID() As String
        Dim ProcessedBy As String = String.Empty
        Dim Comments As String
        Dim x As Integer = 0

        aryRequestID = Split(Session("ISSRejectId"), "|")
        Comments = TextBox_Comments.Text
        ProcessedBy = Session("UserName")

        For x = 0 To UBound(aryRequestID) - 1
            RejectStoreSpecial(aryRequestID(x), ProcessedBy, Comments)
        Next

        If Len(TextBox_Comments.Text) > 0 Then
            ' ********** Send E-Mail Notification --- InStoreSpecial was Rejected --- ******************
            Try
                If Application.Get("InStoreSpecialRejectEmail") = "1" Then
                    Dim em As New EmailNotifications
                    em.EmailType = "InStoreSpecialReject"
                    em.Identifier = Request.QueryString("U")
                    em.ItemDescription = Request.QueryString("I")
                    em.Store_Name = "Regional Office"
                    em.Store_No = Session("Store_No")
                    em.SubTeam_No = 600000
                    em.SubTeam_Name = Request.QueryString("SN")
                    em.Reject_Reason = Comments
                    em.User = Session("UserName")
                    em.User_ID = Session("UserID")
                    em.Current_Price = Request.QueryString("CP")
                    em.New_Price = Request.QueryString("SP")
                    em.Start_Date = Request.QueryString("SD")
                    em.End_Date = Request.QueryString("ED")
                    em.Reques_tor = Request.QueryString("RB")
                    em.Request_Store = Request.QueryString("ST")
                    em.SentEmail()
                End If
                ' ***********************************************************
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try

            'This refreshes the parent page and then closes the window. 
            Response.Write("<script language='javascript'> window.opener.location.reload(); window.close();</script>")
        Else
            Label_Error.Visible = True
            Label_Error.Text = "You must provide a comment."
        End If
    End Sub

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Label_Items.Text = GetRejectedItemInfo()

        'Button_Save.Attributes.Add("onclick", "window.opener.location.reload(); window.close();")
        'Button_Save.Attributes.Add("onclick", "window.close();")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label_Error.Visible = False
    End Sub

    Protected Function GetRejectedItemInfo() As String
        Dim RejectDAO As New RejectStoreSpecial
        Dim ds As DataSet
        Dim sData As String = ""

        ds = RejectDAO.GetRejectedItemInfo(Session("ISSRejectId"))

        For Each row As Data.DataRow In ds.Tables(0).Rows
            If sData = "" Then
                sData = row("Identifier") & " - " & row("Item_Description")
            Else
                sData = sData & vbCrLf & row("Identifier") & " - " & row("Item_Description")
            End If
        Next

        GetRejectedItemInfo = sData
    End Function
End Class
