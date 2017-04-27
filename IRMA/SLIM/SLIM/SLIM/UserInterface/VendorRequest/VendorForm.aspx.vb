Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports SLIM.WholeFoods.IRMA.Common.DataAccess

Partial Class UserInterface_VendorRequest_VendorForm
    Inherits System.Web.UI.Page

    Protected Sub SubmitVendor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SubmitVendor.Click
        Dim venReqExists, venExists As Boolean
        ' ****************** Check For Duplicate Vendors *****************
        'TODO: Check for duplicate Vendors or similar
        Dim da As New VendorRequest1TableAdapters.VendorRequestTableAdapter
        Dim da1 As New IrmaVendorTableAdapters.VendorTableAdapter
        Dim dt As New VendorRequest1.VendorRequestDataTable
        Dim dr As VendorRequest1.VendorRequestRow = dt.NewVendorRequestRow()
        ' ******************************************************************
        Try
            venReqExists = da.VendorRequestExists(Trim(VendorName.Text))
            venExists = da1.VendorExists(Trim(VendorName.Text))
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            MsgLabel.Text = ex.Message
        End Try
        ' ******************************************************************
        If Not venReqExists = True And Not venExists = True Then
            Try
                ' ***************** Insert Vendor Request  ***************************
                dr.Insert_Date = Date.Now
                dr.CompanyName = Trim(VendorName.Text).ToUpper
                dr.Email = Trim(Email.Text)
                dr.City = Trim(City.Text).ToUpper
                dr.State = Trim(State.Text.ToUpper)
                dr.ZipCode = Trim(ZipCode.Text)
                dr.Phone = Trim(Phone.Text)
                dr.Fax = Trim(Fax.Text)
                dr.User_Store = Session("Store_No")
                dr.VendorStatus_ID = 1
                dr.Address_Line_1 = Trim(Address1.Text).ToUpper
                dr.Address_Line_2 = Trim(Address2.Text).ToUpper
                dr.InsuranceNumber = Trim(Liability.Text)

                If PSVendor.Text <> String.Empty Then
                    dr.PS_Vendor_ID = Trim(PSVendor.Text).ToUpper
                Else
                    dr.SetPS_Vendor_IDNull()
                    ' vendors without PS vendor info cannot be applied.
                    dr.Ready_To_Apply = 0
                End If

                If txtPSVendorExport.Text.Trim().Length > 0 Then
                    dr.PS_Export_Vendor_ID = Trim(txtPSVendorExport.Text).ToUpper()
                Else
                    dr.SetPS_Export_Vendor_IDNull()
                    ' vendors without PS vendor info cannot be applied.
                    dr.Ready_To_Apply = 0
                End If

                'TFS#7771************************************
                If VendorKey.Text <> String.Empty Then
                    dr.Vendor_Key = Trim(VendorKey.Text).ToUpper
                Else
                    dr.SetVendor_KeyNull()
                    ' vendor key may be a required field
                    If InstanceDataDAO.IsFlagActive("VendorKeyRequired") Then
                        dr.Ready_To_Apply = 0
                    End If
                End If
                '********************************************

                    dr.Comment = Notes.Text.ToUpper
                    dr.User_ID = Session("UserID")
                    dr.UserAccessLevel_ID = Session("AccessLevel")
                    dt.AddVendorRequestRow(dr)
                    da.Update(dt)
                    MsgLabel.Text = "Vendor SuccessFully Requested!"
                    ' *********************************************
                    'TODO: Send E-mail Notes after Vendor requested
                    ' *********************************************
                    Try
                    If Application.Get("VendorRequestEmail") = "1" Then
                        Dim em As New EmailNotifications
                        em.EmailType = "VendorRequest"
                        em.Identifier = ""
                        em.ItemDescription = "New Vendor"
                        em.Store_Name = Session("Store_Name")
                        em.Store_No = Session("Store_No")
                        em.SubTeam_No = 9999
                        em.SubTeam_Name = "VendorRequest"
                        em.VendorName = dr.CompanyName
                        em.User = Session("UserName")
                        em.SentEmail()
                    End If
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message)
                    End Try

            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                MsgLabel.Text = "Vendor Request Failed!"
                Error_Log.throwException(ex.Message, ex)
            End Try
        ElseIf venReqExists = True Then
            MsgLabel.Text = "Vendor has already been requested!"
        ElseIf venExists = True Then
            MsgLabel.Text = "Vendor already exists in IRMA!"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("VendorRequest") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
        End If
        ' *****************************
        UpdateMenuLinks()
    End Sub


    Protected Sub UpdateMenuLinks()
        If Not Session("Store_No") > 0 Then
            Master.HideMenuLinks("ISS", "ISSNew", False)
            Master.HideMenuLinks("ItemRequest", "NewItem", False)
        Else
            Master.HideMenuLinks("ISS", "ISSNew", True)
            Master.HideMenuLinks("ItemRequest", "NewItem", True)
        End If
    End Sub
End Class
