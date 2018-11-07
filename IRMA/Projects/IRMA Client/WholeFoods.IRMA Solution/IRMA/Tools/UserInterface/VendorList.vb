Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmVendorList
	Inherits System.Windows.Forms.Form
	
	Dim VendorList() As Integer
	Dim sFileName As String
	Dim bChanged As Boolean
    Private mdt As DataTable


    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


	Private Sub cmdAddItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddItem.Click


        logger.Debug("cmdAddItem_Click Entry")

		Dim iLoop As Short
		
		'-- Make sure they don't excede 255 Vendors
        If UBound(VendorList) >= 255 Then

            MsgBox("Vendor List may only have up to 255 vendors", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("Vendor List may only have up to 255 vendors")
            logger.Debug("cmdAddItem_Click Exit")

            Exit Sub
        End If
		
		'-- Set search criteria
		glVendorID = 0
		giSearchType = iSearchVendorCompany
		
		'-- Open the search form
		frmSearch.Text = "Add Vendor to List"
		frmSearch.ShowDialog()
        frmSearch.Dispose()
		
		If glVendorID > 0 Then
			
			'-- Make sure it isn't already in the list
			For iLoop = 1 To UBound(VendorList)
				If glVendorID = VendorList(iLoop) Then
                    MsgBox("Vendor is already in this list.", MsgBoxStyle.Exclamation, "Error!")
                    logger.Info("Vendor is already in this list.")
				End If
			Next iLoop
			
			'-- Add it to the list
			ReDim Preserve VendorList(UBound(VendorList) + 1)
			VendorList(UBound(VendorList)) = glVendorID
			
			bChanged = True
			
			RunSearch()
		End If

        logger.Debug("cmdAddItem_Click Exit")
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

		Dim iLoop As Short
		
		'-- Make sure one item was selected
        If ugrdVendorList.Selected.Rows.Count = 1 Then

            glVendorID = CInt(ugrdVendorList.Selected.Rows(0).Cells("Vendor_ID").Value)

            '-- Remove all occurances of this vendor out of the loop
            iLoop = 1
            While iLoop <= UBound(VendorList)
                If VendorList(iLoop) = glVendorID Then

                    If iLoop < UBound(VendorList) Then
                        VendorList(iLoop) = VendorList(UBound(VendorList))
                    End If

                    ReDim Preserve VendorList(UBound(VendorList) - 1)
                Else
                    iLoop = iLoop + 1
                End If
            End While

            bChanged = True

            RunSearch()

        Else

            MsgBox("A vendor from the list must be selected.", MsgBoxStyle.Exclamation, "Error!")
            logger.Info("A vendor from the list must be selected.")

        End If

        logger.Debug("cmdDelete_Click Exit")
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
		'-- Unload search form
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
    End Sub

    Private Sub frmVendorList_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If bChanged Then
            Select Case MsgBox("Save this Vendor List for later use?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton1, "Save View")
                Case MsgBoxResult.Yes : mnuSave_Click(mnuSave, New System.EventArgs())
                Case MsgBoxResult.Cancel
                    e.Cancel = 1 : Exit Sub
            End Select
        End If

        glVendorID = 0
    End Sub
	
	Private Sub frmVendorList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmVendorList_Load Entry")

		'-- Center the form and the buttons on the form
        Me.Height = cmdExit.Height + cmdExit.Top + 52
		CenterForm(Me)
		
		'-- Set up the Vendor List
		ReDim VendorList(0)
		bChanged = False

        SetupDataTable()

        logger.Debug("frmVendorList_Load Exit")

	End Sub
	
    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("VendorList")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Vendor_ID", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")

    End Sub

    Public Sub RunSearch()

        logger.Debug("RunSearch Entry")


        Dim iLoop As Short
        Dim sSearch As String
        sSearch = String.Empty

        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow

        '-- Put all the vendors into a set
        For iLoop = 1 To UBound(VendorList)
            sSearch = sSearch & Trim(Str(VendorList(iLoop))) & ","
        Next iLoop
        If sSearch <> "" Then
            sSearch = Mid(sSearch, 1, Len(sSearch) - 1)
        Else
            sSearch = "0"
        End If

        Try
            '-- Query the set
            rsSearch = SQLOpenRecordSet("SELECT DISTINCT Vendor_ID, CompanyName " & "FROM Vendor " & "WHERE Vendor_ID IN (" & sSearch & ") " & "ORDER BY CompanyName", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            mdt.Clear()

            ReDim VendorList(0)

            '-- Display the info
            While (Not rsSearch.EOF)
                row = mdt.NewRow
                row("Vendor_ID") = rsSearch.Fields("Vendor_ID").Value
                row("CompanyName") = rsSearch.Fields("CompanyName").Value

                mdt.Rows.Add(row)

                ReDim Preserve VendorList(UBound(VendorList) + 1)
                VendorList(UBound(VendorList)) = rsSearch.Fields("Vendor_ID").Value
                rsSearch.MoveNext()

            End While
        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If
        End Try

        mdt.AcceptChanges()
        ugrdVendorList.DataSource = mdt

        If ugrdVendorList.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdVendorList.Rows(0).Selected = True
        End If

        logger.Debug("RunSearch Exit")

    End Sub
	
    Public Sub mnuLoad_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuLoad.Click

        logger.Debug("mnuLoad_Click Entry")

        Dim sString As String

        If bChanged Then
            Select Case MsgBox("Save this Vendor List for later use?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton1, "Save View")
                Case MsgBoxResult.Yes : mnuSave_Click(mnuSave, New System.EventArgs())
                Case MsgBoxResult.Cancel : Exit Sub
            End Select
        End If

        cdbFileOpen.InitialDirectory = My.Application.Info.DirectoryPath
        cdbFileSave.InitialDirectory = My.Application.Info.DirectoryPath
        cdbFileOpen.CheckFileExists = True
        cdbFileOpen.CheckPathExists = True
        cdbFileSave.CheckPathExists = True
        cdbFileOpen.ShowReadOnly = False
        cdbFileOpen.Filter = "Vendor List Files (*.vls)|*.vls"
        cdbFileSave.Filter = "Vendor List Files (*.vls)|*.vls"
        cdbFileOpen.ShowDialog()
        cdbFileSave.FileName = cdbFileOpen.FileName

        On Error Resume Next
        FileOpen(1, cdbFileOpen.FileName, OpenMode.Input)
        If Err.Number <> 0 Then
            On Error GoTo 0
            MsgBox("Error opening file.  Load Aborted.", MsgBoxStyle.Exclamation, "Notice")
            logger.Info("Error opening file.  Load Aborted.")
            logger.Debug("mnuLoad_Click Exit")
            Exit Sub
        End If
        On Error GoTo 0

        ReDim VendorList(0)

        While Not EOF(1)
            sString = LineInput(1)
            ReDim Preserve VendorList(UBound(VendorList) + 1)
            VendorList(UBound(VendorList)) = CInt(sString)
        End While
        FileClose(1)

        sFileName = cdbFileOpen.FileName
        Me.Text = "Vendor List [" & sFileName & "]"

        RunSearch()

        logger.Debug("mnuLoad_Click Exit")

    End Sub
	
	Public Sub mnuSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSave.Click
        logger.Debug("mnuSave_Click Entry")
		If Trim(sFileName) = "" Then
			Call mnuSaveAs_Click(mnuSaveAs, New System.EventArgs())
		Else
			SaveVendorList()
        End If
        logger.Debug("mnuSave_Click Exit")
		
	End Sub
	
	Public Sub mnuSaveAs_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSaveAs.Click


        logger.Debug("mnuSaveAs_Click Entry")

		cdbFileOpen.InitialDirectory = My.Application.Info.DirectoryPath
		cdbFileSave.InitialDirectory = My.Application.Info.DirectoryPath
        cdbFileOpen.CheckFileExists = True
		cdbFileOpen.CheckPathExists = True
		cdbFileSave.CheckPathExists = True
        cdbFileOpen.ShowReadOnly = False
        cdbFileOpen.Filter = "Vendor List Files (*.vls)|*.vls"
		cdbFileSave.Filter = "Vendor List Files (*.vls)|*.vls"
		cdbFileSave.ShowDialog()
		cdbFileOpen.FileName = cdbFileSave.FileName
		
		If cdbFileOpen.FileName = "" Then Exit Sub
		
        If Dir(cdbFileOpen.FileName) <> "" Then
            If MsgBox("File already exists. Overwrite it?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.No Then
                MsgBox("Save Aborted.", MsgBoxStyle.Exclamation, "Notice")

                logger.Info("Save Aborted.")
                logger.Debug("mnuSaveAs_Click Exit")
                Exit Sub
            End If
        End If
		
		Err.Clear()
		On Error Resume Next
		FileOpen(1, cdbFileOpen.FileName, OpenMode.Append)
		FileClose(1)
		On Error GoTo 0
		If Err.Number <> 0 Then
            MsgBox("Error opening file.  Save Aborted.", MsgBoxStyle.Exclamation, "Notice")
            logger.Info("Error opening file.  Save Aborted.")
            logger.Debug("mnuSaveAs_Click Exit")
			Exit Sub
		End If
		
		sFileName = cdbFileOpen.FileName
		Me.Text = "Vendor List [" & sFileName & "]"
        SaveVendorList()

        logger.Debug("mnuSaveAs_Click Exit")
		
	End Sub
	
    Function SaveVendorList() As Boolean

        logger.Debug("SaveVendorList entry")

        If Trim(sFileName) = "" Then
            MsgBox("Save Aborted.", MsgBoxStyle.Exclamation, "Warning!")
            SaveVendorList = False
            logger.Info("Save Aborted.")
        End If

        Dim iLoop As Short

        FileOpen(1, Trim(sFileName), OpenMode.Output)

        For iLoop = 1 To UBound(VendorList)
            PrintLine(1, VendorList(iLoop))
        Next iLoop

        FileClose(1)

        SaveVendorList = True

        bChanged = False

        logger.Debug("SaveVendorList exit")

    End Function

End Class