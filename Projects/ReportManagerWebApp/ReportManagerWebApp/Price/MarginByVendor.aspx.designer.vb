﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated. 
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class Price_MarginByVendor

    '''<summary>
    '''rdbtn_SearchType control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents rdbtn_SearchType As Global.System.Web.UI.WebControls.RadioButtonList

    '''<summary>
    '''custValidVendorID control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents custValidVendorID As Global.System.Web.UI.WebControls.CustomValidator

    '''<summary>
    '''txtSearch control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtSearch As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''btnGetVendors control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnGetVendors As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''reqfld_lstVendors control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents reqfld_lstVendors As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''lstVendors control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lstVendors As Global.System.Web.UI.WebControls.ListBox

    '''<summary>
    '''lblVendorCount control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents lblVendorCount As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''cmbStore control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents cmbStore As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''ICStores control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents ICStores As Global.System.Web.UI.WebControls.SqlDataSource

    '''<summary>
    '''reqfld_cmbStore control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents reqfld_cmbStore As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''txtMinval control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtMinval As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''reqfld_txtMinval control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents reqfld_txtMinval As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''txtMaxval control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents txtMaxval As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''reqfld_txtMaxval control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents reqfld_txtMaxval As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''rdbtn_InRange control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents rdbtn_InRange As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''rdbtn_OutRange control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents rdbtn_OutRange As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''cmbReportFormat control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents cmbReportFormat As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''btnReport control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents btnReport As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''rngValid_txtMinval control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents rngValid_txtMinval As Global.System.Web.UI.WebControls.RangeValidator

    '''<summary>
    '''rngValid_txtMaxval control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents rngValid_txtMaxval As Global.System.Web.UI.WebControls.RangeValidator

    '''<summary>
    '''ICVendors_CompanyName control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents ICVendors_CompanyName As Global.System.Web.UI.WebControls.SqlDataSource

    '''<summary>
    '''ICVendors_PSVendorID control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents ICVendors_PSVendorID As Global.System.Web.UI.WebControls.SqlDataSource

    '''<summary>
    '''ICVendors_VendorID control.
    '''</summary>
    '''<remarks>
    '''Auto-generated field.
    '''To modify move field declaration from designer file to code-behind file.
    '''</remarks>
    Protected WithEvents ICVendors_VendorID As Global.System.Web.UI.WebControls.SqlDataSource
End Class
