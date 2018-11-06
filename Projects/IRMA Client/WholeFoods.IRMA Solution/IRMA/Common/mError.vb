Option Strict Off
Option Explicit On
Module mError
	'if any of the App values change in the DB change they must be changed here
	Public Enum enumApp
		IRS = 1
		ReportManager = 2
		Replenishment = 3
		VendorCostImport = 4
		AutoOrders = 5
		ExReport = 6
	End Enum
	
	'If any of the ExType value change in the DB they must be changed here
	Public Enum enumExType
		VendCostPackSize = 1
		VendCostDiff = 2
		AutoOrdersNoItem = 1
		AutoOrdersNoEMail = 2
		AutoOrdersNoTitle = 3
	End Enum
	
	
	'------------User Defined Error Codes
	Public Const ERR_MUST_USE_CreateObj As Decimal = vbObjectError + 512
	Public Const ERR_NO_EXCEPTION_CLASS As Decimal = vbObjectError + 513
	Public Const ERR_NO_KEY_COLUMN As Decimal = vbObjectError + 514
	Public Const ERR_DBTYPE_CONVERT_TO_ADTYPE As Decimal = vbObjectError + 515
	Public Const ERR_RIPE_IMPORT_NO_NEW_ORDERS As Decimal = vbObjectError + 516
	Public Const ERR_RIPE_IMPORT_NO_TRANSFER_SUBTEAM As Decimal = vbObjectError + 517
	Public Const ERR_RIPE_IMPORT_NO_TRANSFER_TO_SUBTEAM As Decimal = vbObjectError + 518
	Public Const ERR_RIPE_IMPORT_CAN_NOT_CREATE_ORDER As Decimal = vbObjectError + 519
	
	'------------User Defined Error Messages
	Public Const ERR_MUST_USE_CreateObj_DESC As String = "You must use the CreatObj Constructor Function to use this object."
	Public Const ERR_NO_EXCEPTION_CLASS_Desc As String = "The Exception class for this exception does not exist"
	Public Const ERR_NO_KEY_COLUMN_DESC As String = "No Values can be returned because no Column was set as the Key column."
	Public Const ERR_DBTYPE_CONVERT_TO_ADTYPE_DESC As String = "Missing DAO field type conversion for ADO recordset: "
	Public Const ERR_RIPE_IMPORT_NO_NEW_ORDERS_DESC As String = "No New Orders Found in RIPE for the given criteria."
	Public Const ERR_RIPE_IMPORT_NO_TRANSFER_SUBTEAM_DESC As String = "Transfer_SubTeam is NULL - Recipe..LocationDepartment table must be corrected"
	Public Const ERR_RIPE_IMPORT_NO_TRANSFER_TO_SUBTEAM_DESC As String = "Transfer_To_SubTeam is NULL - Recipe..CustomerDept table must be corrected"
	Public Const ERR_RIPE_IMPORT_CAN_NOT_CREATE_ORDER_DESC As String = "Error durring Order Creation."
	
	Public Sub LoadExceptionTypes(ByRef cmbComboBox As System.Windows.Forms.ComboBox, ByRef iAppID As Short)
		cmbComboBox.Items.Clear()
		
        'Load all apps into combo box.
        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetExceptionTypes " & iAppID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                cmbComboBox.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("ExName").Value, gRSRecordset.Fields("ExType").Value))
                gRSRecordset.MoveNext()
            Loop
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try
    End Sub
	'
	'Public Function CreateException(iApp As enumApp, lExType As enumExType, Optional lExceptionID As Long = -1) As iException
	''    Dim tmpEx As iException
	'
	'    Select Case lExType
	'    Case VendCostPackSize
	'        Dim tmpEx As clsExVendorCostPackSizeDif
	'        Set tmpEx = New clsExVendorCostPackSizeDif
	'        tmpEx.Init (lExceptionID)
	'
	'    Case VendCostPackSize
	'
	'    Case Else
	'        Err.Raise ERR_NO_EXCEPTION_CLASS, "CreateException", ERR_NO_EXCEPTION_CLASS_Desc
	'    End Select
	'
	'    Set CreateException = tmpEx
	'
	'End Function
End Module