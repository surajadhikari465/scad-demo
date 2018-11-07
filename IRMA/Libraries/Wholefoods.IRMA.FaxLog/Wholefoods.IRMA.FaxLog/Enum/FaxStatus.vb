
Public Enum FaxStatus

    ''' <summary>
    ''' Notes the successful transmisson of a fax.
    ''' </summary>
    ''' <remarks>Assigned to a fax when a success notification is received from the fax service.</remarks>
    TransmissionSuccess
    ''' <summary>
    ''' Notes the failed transmisson of a fax.
    ''' </summary>
    ''' <remarks>Assigned to a fax when a failure notification is received from the fax service.</remarks>
    TransmissionFailure
    ''' <summary>
    ''' Notes the unknown status of a fax transmission.
    ''' </summary>
    ''' <remarks>Assgined to a fax when the status of a fax is unknown (e.g. Fax Service fails to generate a success or failure message).</remarks>
    TransmissionUnknown
    ''' <summary>
    ''' Notes the fax transmission was processed by the SendOrders job.
    ''' </summary>
    TransmissionSent
End Enum
