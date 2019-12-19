'Imports System.Management

'Public Class MacAddress

'    Public Sub GetAddress()
'        Dim list As New List(Of String)
'        Dim mc As System.Management.ManagementClass
'        Dim mo As ManagementObject
'        mc = New ManagementClass("Win32_NetworkAdapterConfiguration")
'        Dim moc As ManagementObjectCollection = mc.GetInstances()
'        For Each mo In moc
'            If mo.Item("IPEnabled") = True Then
'                list.Add("MAC address " & mo.Item("MacAddress").ToString())
'            End If
'        Next

'    End Sub


'End Class
