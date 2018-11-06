
Partial Class UserInterface_ItemRequest_ItemRequestComments
    Inherits System.Web.UI.Page

    Protected Sub RejectStoreSpecial(ByRef RequestID As String, ByRef ProcessedBy As String, ByRef Comments As String)
        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList
        Try
            currentParam = New DBParam
            currentParam.Name = "requestId" '"ItemRequest_ID"
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

            df.ExecuteStoredProcedure("SLIM_RejectItemRequest", paramList)

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Protected Sub Button_Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        Dim RequestID As String = String.Empty
        Dim ProcessedBy As String = String.Empty
        Dim Comments As String

        RequestID = Request.QueryString("ItemRequest_ID")
        Comments = TextBox_Comments.Text
        ProcessedBy = Session("UserName")
        If Len(TextBox_Comments.Text) > 0 Then
            RejectStoreSpecial(RequestID, ProcessedBy, Comments)
            ' ********** Send E-Mail Notification --- Item was Rejected --- ******************
            Try
                If Application.Get("ItemRequestRejectEmail") = "1" Then
                    Dim em As New EmailNotifications
                    em.EmailType = "ItemRequestReject"
                    em.Identifier = Request.QueryString("Identifier")
                    em.ItemDescription = Request.QueryString("Item_Description")
                    em.Store_Name = "Regional Office"
                    em.Store_No = Session("Store_No")
                    em.SubTeam_No = 600000
                    em.SubTeam_Name = Request.QueryString("SubTeam_Name")
                    em.Reject_Reason = Comments
                    em.User = Session("UserName")
                    em.User_ID = Session("UserID")
                    em.Reques_tor = Request.QueryString("RequestedBy")
                    em.SentEmail()
                End If
                ' ***********************************************************
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
            End Try
            ' ********************************************************************************
            'This refreshes the parent page and then closes the window. 
            Response.Write("<script language='javascript'> window.opener.location.reload(); window.close();</script>")
        Else
            Label_Error.Visible = True
            Label_Error.Text = "You must provide a comment."
        End If

    End Sub


    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Label_UPC.Text = Request.QueryString("Identifier")
        Label_Item_Desc.Text = Request.QueryString("Item_Description")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label_Error.Visible = False

    End Sub
End Class
