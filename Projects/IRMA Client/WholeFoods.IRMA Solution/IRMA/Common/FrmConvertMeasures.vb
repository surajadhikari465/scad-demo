Option Strict Off
Option Explicit On

Imports IRMAService
Imports log4net
Imports System.Collections.Generic
Imports System.ServiceModel
Imports WholeFoods.Utility

Public Class FrmConvertMeasures

    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Members"

    Public Amount As Object
    Private myBinding As New WSHttpBinding
    Private myEndpoint As New EndpointAddress(ConfigurationServices.AppSettings("ConversionCalculatorServiceAddress").ToString)
    Private myChannelFactory As ChannelFactory(Of IGatewayChannel) = New ChannelFactory(Of IGatewayChannel)(myBinding, myEndpoint)
    Private wcfClient1 As IGatewayChannel = myChannelFactory.CreateChannel()

#End Region

    Private Sub PerformConversion()
        Try
            If (IsNumeric(Amount)) Then
                txtAmountOut.Text = Math.Round(wcfClient1.CalculateConversion(cboUnitsIn.SelectedItem, cboUnitsOut.SelectedItem, CDec(Amount)), 4).ToString
                txtAmountOut.Focus()
                txtAmountOut.SelectAll()
            Else
                MessageBox.Show("You must specify a Numeric amount", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("InnerException: " & ex.InnerException.Message.ToString, "Error" & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PopulateInUnits()
        Try
            cboUnitsIn.Items.Clear()
            cboUnitsIn.Items.AddRange(wcfClient1.GetInUnits())
            cboUnitsIn.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show("InnerException: " & ex.InnerException.Message.ToString, "Error" & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PopulateOutUnits()
        Try
            cboUnitsOut.Items.Clear()
            cboUnitsOut.Items.AddRange(wcfClient1.GetOutUnits(cboUnitsIn.SelectedItem.ToString))
            cboUnitsOut.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show("InnerException: " & ex.InnerException.Message.ToString, "Error" & ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class