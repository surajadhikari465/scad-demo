Option Strict Off
Partial Class UserInterface_RetailCost_RetailCost
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Label5.Text = Request.QueryString("UPC")
        Label6.Text = Request.QueryString("Description")
        Label8.Text = Session("Store_Name")
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        ' **********************************************************
        ' Dim Price As Single
        Dim UnitCost As Single
        Dim Multiple As Integer
        ' Dim CaseCost As Single
        Dim VendorName As String
        ' **********************************************************
        VendorName = GridView1.SelectedRow.Cells(4).Text.ToString
        Dim rc As New RetailCost(GridView1.SelectedDataKey.Value)
        rc.StoreNo = Session("Store_No")
        rc.CasePack = CInt(IIf(GridView1.SelectedRow.Cells(5).Text = "&nbsp;", 0, _
        GridView1.SelectedRow.Cells(5).Text))
        rc.VendorNameID = VendorName
        If "$" & NewPriceTxBx.Text.ToString <> GridView1.SelectedRow.Cells(2).Text Then
            ' ************* New Price **************************
            If Not PriceMultipleTxBx.Text.ToString = "" Then
                Multiple = CInt(PriceMultipleTxBx.Text)
            Else
                Multiple = CInt(GridView1.SelectedRow.Cells(1).Text)
            End If
            If Not NewPriceTxBx.Text.ToString = "" Then
                Try
                    rc.ChangePrice(CDec(NewPriceTxBx.Text), Multiple)
                    Label4.Text = "Pending Price Change Applied!"
                    Try
                        ' ***************** Sent the Notification E-Mail ************
                        'TODO: Send E-mail Notes after Item requested
                        If Application.Get("RetailCostChangeEmail") = "1" Then
                            Dim em As New EmailNotifications
                            em.EmailType = "RetailCost"
                            em.Identifier = Request.QueryString("UPC")
                            em.ItemDescription = Request.QueryString("Description")
                            em.Store_Name = Session("Store_Name")
                            em.Store_No = Session("Store_No")
                            em.SubTeam_No = Session("SubTeam_No")
                            em.SubTeam_Name = Session("SubTeam_Name")
                            em.Current_Price = GridView1.SelectedRow.Cells(2).Text
                            em.New_Price = NewPriceTxBx.Text
                            em.Current_Cost = GridView1.SelectedRow.Cells(6).Text
                            em.New_Cost = NewCaseCostTxBx.Text.ToString
                            em.Current_CaseSize = GridView1.SelectedRow.Cells(5).Text
                            em.New_CaseSize = NewCaseSizeTxBx.Text.ToString
                            em.User = Session("UserName")
                            em.User_ID = Session("UserID")
                            em.Item_Key = Request.QueryString("ItemKey")
                            em.SentEmail()
                        End If
                        ' ***********************************************************
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                        'Error_Log.throwException(ex.Message, ex)
                    End Try
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                    Label4.Text = "Price Change Not Applied!"
                    Error_Log.throwException(ex.Message, ex)
                End Try
            End If
        Else
            Label4.Text = "New Price Equals existing Price"
        End If
        ' **************************************************************************
        If Not NewCaseCostTxBx.Text.ToString = "" Then
            UnitCost = CDec(NewCaseCostTxBx.Text.ToString)
        End If
        ' ***************************************************************************
        If Not NewCaseSizeTxBx.Text = "" Then
            rc.CasePack = CInt(NewCaseSizeTxBx.Text)
        End If
        If "$" & UnitCost <> GridView1.SelectedRow.Cells(6).Text Then
            If UnitCost > 0 And Not rc.CasePack = 0 Then
                Try
                    rc.ChangeCost(CDec(NewCaseCostTxBx.Text))
                    Label7.Text = "Cost Change Applied!"
                    Try
                        ' ***************** Sent the Notification E-Mail ************
                        'TODO: Send E-mail Notes after Item requested
                        If Application.Get("RetailCostChangeEmail") = "1" Then
                            Dim em As New EmailNotifications
                            em.EmailType = "RetailCost"
                            em.Identifier = Request.QueryString("UPC")
                            em.ItemDescription = Request.QueryString("Description")
                            em.Store_Name = Session("Store_Name")
                            em.Store_No = Session("Store_No")
                            em.SubTeam_No = Session("SubTeam_No")
                            em.SubTeam_Name = Session("SubTeam_Name")
                            em.Current_Price = GridView1.SelectedRow.Cells(2).Text
                            em.New_Price = NewPriceTxBx.Text
                            em.Current_Cost = GridView1.SelectedRow.Cells(6).Text
                            em.New_Cost = NewCaseCostTxBx.Text.ToString
                            em.Current_CaseSize = GridView1.SelectedRow.Cells(5).Text
                            em.New_CaseSize = NewCaseSizeTxBx.Text.ToString
                            em.User = Session("UserName")
                            em.User_ID = Session("UserID")
                            em.Item_Key = Request.QueryString("ItemKey")
                            em.SentEmail()
                        End If
                        ' ***********************************************************
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                        'Error_Log.throwException(ex.Message, ex)
                    End Try
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                    Label7.Text = "Cost Change Not Applied!"
                    Error_Log.throwException(ex.Message, ex)
                End Try
            End If
        ElseIf Not UnitCost > 0 Then
            Label7.Text = "Cost Not Greater Than Zero"
        ElseIf rc.CasePack = 0 Then
            Label7.Text = "Please supply a Case Size"
        ElseIf NewCaseSizeTxBx.Text = 0 Then
            Label7.Text = "Case Size Cannot Equal Zero"
        Else
            Label7.Text = "New Cost Equals existing Cost"
        End If
        rc = Nothing
        ' **************************************************************************
        GridView1.SelectedIndex() = -1
        GridView1.DataBind()
    End Sub


End Class
