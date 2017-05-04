Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_ManagePricingMethods

    Private Sub Form_ManagePricingMethods_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToScreen()

        LoadCombos()
        LoadDataGrid()
    End Sub

    Private Sub LoadCombos()
        Try
            ' Populate the selections for File Writer 
            Me.ComboBox_FileWriter.DataSource = POSWriterDAO.GetFileWriters(POSWriterBO.WRITER_TYPE_POS).Tables(0)
            Me.ComboBox_FileWriter.DisplayMember = "POSFileWriterCode"
            Me.ComboBox_FileWriter.ValueMember = "POSFileWriterKey"
            Me.ComboBox_FileWriter.SelectedIndex = -1

            ' Populate the selections for Pricing Methods
            Me.ComboBox_PricingMethod.DataSource = PricingMethodsDAO.GetPricingMethods.Tables(0)
            Me.ComboBox_PricingMethod.DisplayMember = "PricingMethod_Name"
            Me.ComboBox_PricingMethod.ValueMember = "PricingMethod_ID"
            Me.ComboBox_PricingMethod.SelectedIndex = -1

        Catch ex As DataFactoryException
            Logger.LogError("Exception: ", Me.GetType(), ex)

            'send message about exception
            Dim args(1) As String
            args(0) = "Form_ManagePricingMethods form: Form_Load sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
        Logger.LogDebug("Form_ManagePricingMethods_Load exit", Me.GetType())
    End Sub

    Private Sub LoadDataGrid()
        Me.UltraGrid_PricingMethods.DataSource = PricingMethodsDAO.GetPricingMethodMapings.Tables(0)
    End Sub

    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Dim pricingMethodsBO As New PricingMethodsBO
        Dim pricingMethodsDAO As New PricingMethodsDAO

        If Me.ComboBox_FileWriter.Text.Equals("") Then
            MsgBox("A File Writer must be selected.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If ComboBox_PricingMethod.Text.Equals("") Then
            MsgBox("A Pricing Method must be selected.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If TextBox_PricingMethodValue.Text = "" Then
            MsgBox("A Pricing Method Value must be entered.", MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        pricingMethodsBO.PricingMethodFileWriterKey = CInt(ComboBox_FileWriter.SelectedValue)
        pricingMethodsBO.PricingMethodKey = CInt(ComboBox_PricingMethod.SelectedValue)
        pricingMethodsBO.PricingMethodID = CInt(TextBox_PricingMethodValue.Text)

        Try
            'save changes
            pricingMethodsDAO.InsertPricingMethodMapping(pricingMethodsBO)
            LoadDataGrid()
        Catch ex As Exception
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            MessageBox.Show(Form_IRMABase.ERROR_DB, "IRMA Application Error", MessageBoxButtons.OK)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_MaanagePricingMethods form: ApplyChanges sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
        End Try
    End Sub

    Private Sub Button_Remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Remove.Click
        Dim pricingMethodsBO As New PricingMethodsBO
        Dim pricingMethodsDAO As New PricingMethodsDAO

        If UltraGrid_PricingMethods.Selected.Rows.Count = 1 Then
            pricingMethodsBO.PricingMethodFileWriterKey = CInt(UltraGrid_PricingMethods.Selected.Rows(0).Cells("POSFileWriterKey").Value)
            pricingMethodsBO.PricingMethodKey = CInt(UltraGrid_PricingMethods.Selected.Rows(0).Cells("PricingMethod_Key").Value)
            pricingMethodsBO.PricingMethodID = CInt(UltraGrid_PricingMethods.Selected.Rows(0).Cells("PricingMethod_ID").Value)

            Try
                'save changes
                pricingMethodsDAO.RemovePricingMethodMapping(pricingMethodsBO)
                LoadDataGrid()
            Catch ex As Exception
                Logger.LogError("Exception: ", Me.GetType(), ex)
                'display a message to the user
                MessageBox.Show(Form_IRMABase.ERROR_DB, "IRMA Application Error", MessageBoxButtons.OK)
                'send message about exception
                Dim args(1) As String
                args(0) = "Form_ManagePricingMethods form: ApplyChanges sub"
                ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            End Try
        Else
            MsgBox("A single row must be selected.", MsgBoxStyle.Critical, Me.Text)
        End If
    End Sub
End Class