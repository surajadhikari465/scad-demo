Imports System.Configuration
Imports WholeFoods.Utility

Imports System.Data
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic

Public Class EInvoicing_SuspendedEInvoicesReport


    Private Sub LinkLabel_ClearStartDate_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_ClearStartDate.LinkClicked
        UltraDate_StartDate.Value = Nothing

    End Sub

    Private Sub LinkLabel_ClearEndDate_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_ClearEndDate.LinkClicked
        UltraDate_EndDate.Value = Nothing
    End Sub

    Private Sub EInvoicing_SuspendedEInvoicesReport_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        loadStores()
        loadVendors()
        Call LinkLabel_ClearStartDate_LinkClicked(sender, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))
        Call LinkLabel_ClearEndDate_LinkClicked(sender, New LinkLabelLinkClickedEventArgs(New LinkLabel.Link()))

    End Sub


    Private Sub loadStores()
        Dim stores As ArrayList = StoreDAO.GetStores

        Dim bo As StoreBO = New StoreBO()
        bo.StoreNo = -1
        bo.StoreName = "All"


        stores.Insert(0, bo)


        ComboBox_Stores.DataSource = stores
        ComboBox_Stores.DisplayMember = "StoreName"
        ComboBox_Stores.ValueMember = "StoreNo"

        ComboBox_Stores.SelectedIndex = 0
    End Sub

    Private Sub loadVendors()

        Dim dao As EInvoicingDAO = New EInvoicingDAO
        Dim dt As DataTable = dao.getVendors()

        Dim dr As DataRow = dt.NewRow()
        dr("Vendor_Id") = -1
        dr("CompanyName") = "All"
        dt.Rows.InsertAt(dr, 0)

        ComboBox_Vendors.DataSource = dt
        ComboBox_Vendors.DisplayMember = "Companyname"
        ComboBox_Vendors.ValueMember = "Vendor_Id"



    End Sub

    Private Sub LinkLabel_ClearStore_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_ClearStore.LinkClicked
        ComboBox_Stores.SelectedIndex = 0
    End Sub

    Private Sub LinkLabel_ClearVendor_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_ClearVendor.LinkClicked
        ComboBox_Vendors.SelectedIndex = 0
    End Sub

    Private Sub LinkLabel_ClearAll_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel_ClearAll.LinkClicked
        Call LinkLabel_ClearStore_LinkClicked(sender, e)
        Call LinkLabel_ClearVendor_LinkClicked(sender, e)
        Call LinkLabel_ClearStartDate_LinkClicked(sender, e)
        Call LinkLabel_ClearEndDate_LinkClicked(sender, e)
    End Sub

    Private Sub Button_RunReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_RunReport.Click
        Dim Msg As String = String.Empty


        If UltraDate_StartDate.Value Is Nothing Then
            If Not UltraDate_EndDate.Value Is Nothing Then
                Msg = "You must choose a Start and End date or no dates at all."
            End If
        End If

        If UltraDate_EndDate.Value Is Nothing Then
            If Not UltraDate_StartDate.Value Is Nothing Then
                Msg = "You must choose a Start and End date or no dates at all."
            End If
        Else
            If UltraDate_EndDate.DateTime < UltraDate_StartDate.DateTime Then
                Msg = "You must choose an End Date  that is greater then Start Date."
            End If
        End If


        If Not Msg = String.Empty Then
            MessageBox.Show(Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1)
            Exit Sub
        End If

        Dim sReportURL As New System.Text.StringBuilder
        sReportURL.Append("EInvSuspendedInvoices")
        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")



        '-----------------------------------------------
        ' Add Report Parameters
        '-----------------------------------------------

        If UltraDate_StartDate.Value Is Nothing Then
            sReportURL.Append("&StartDate:isNull=True")

        Else
            sReportURL.Append(String.Format("&StartDate={0}", UltraDate_StartDate.Value.ToString()))
        End If

        If UltraDate_EndDate.Value Is Nothing Then
            sReportURL.Append("&EndDate:isNull=True")
        Else
            sReportURL.Append(String.Format("&EndDate={0}", UltraDate_EndDate.Value.ToString()))
        End If

        If ComboBox_Stores.SelectedIndex = 0 Then
            sReportURL.Append("&Store:isNull=True")
        Else
            sReportURL.Append(String.Format("&Store={0}", ComboBox_Stores.SelectedValue.ToString()))
        End If

        If ComboBox_Vendors.SelectedIndex = 0 Then
            sReportURL.Append("&Vendor:isNull=True")
        Else
            sReportURL.Append(String.Format("&Vendor={0}", ComboBox_Vendors.SelectedValue.ToString()))
        End If

        Dim sReportingServicesURL As String = ConfigurationServices.AppSettings("reportingServicesURL")
        System.Diagnostics.Process.Start(sReportingServicesURL & sReportURL.ToString())


    End Sub
End Class