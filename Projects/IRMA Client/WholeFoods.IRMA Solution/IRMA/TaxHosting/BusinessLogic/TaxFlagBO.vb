Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess
Imports WholeFoods.IRMA.Common.BusinessLogic    ' for Instance data flags
Imports WholeFoods.IRMA.Common.DataAccess       ' for Instance data flags

Public Enum TaxFlagStatus
    Valid
    Error_Duplicate_TaxFlagKey
    Error_Required_TaxClass
    Error_Required_TaxJurisdiction
    Error_Required_TaxFlagKey
    Error_Required_TaxPercent
    Error_Required_POSID
    Error_NotNumeric_TaxPercent
    Error_NotNumeric_POSID
    Error_MultipleActiveFlags
    Error_POSIDValue
    Error_TaxPercentValue
    Warning_TaxPercentDecimalPrecision
End Enum

Namespace WholeFoods.IRMA.TaxHosting.BusinessLogic
    Public Class TaxFlagBO

#Region "Property Definitions"

        Private _taxClassId As Integer
        Private _taxClassDesc As String
        Private _taxJurisdictionId As Integer
        Private _taxJurisdictionDesc As String
        Private _taxFlagKey As String
        Private _taxFlagValue As Boolean
        Private _taxPercent As String   ' Decimal in db; store as String so it can be empty on UI
        Private _posId As String
        Private _resetActiveFlags As Boolean
        Private _externalTaxGroupCode As String ' TFS 11988, Tom Lux, 2/20/10, v3.5.9: Will house "CCH Group Item Code" value.

#End Region

#Region "Constructors"

        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub
#End Region

#Region "Property Access Methods"

        Public Property TaxClassId() As Integer
            Get
                Return _taxClassId
            End Get
            Set(ByVal value As Integer)
                _taxClassId = value
            End Set
        End Property

        Public Property TaxClassDesc() As String
            Get
                Return _taxClassDesc
            End Get
            Set(ByVal value As String)
                _taxClassDesc = value
            End Set
        End Property

        Public Property TaxJurisdictionId() As Integer
            Get
                Return _taxJurisdictionId
            End Get
            Set(ByVal value As Integer)
                _taxJurisdictionId = value
            End Set
        End Property

        Public Property TaxJurisdictionDesc() As String
            Get
                Return _taxJurisdictionDesc
            End Get
            Set(ByVal value As String)
                _taxJurisdictionDesc = value
            End Set
        End Property

        Public Property TaxFlagKey() As String
            Get
                Return _taxFlagKey
            End Get
            Set(ByVal value As String)
                _taxFlagKey = value
            End Set
        End Property

        Public Property TaxFlagValue() As Boolean
            Get
                Return _taxFlagValue
            End Get
            Set(ByVal value As Boolean)
                _taxFlagValue = value
            End Set
        End Property

        Public Property TaxPercent() As String
            Get
                Return _taxPercent
            End Get
            Set(ByVal value As String)
                _taxPercent = value
            End Set
        End Property

        Public Property POSId() As String
            Get
                Return _posId
            End Get
            Set(ByVal value As String)
                _posId = value
            End Set
        End Property

        Public Property ResetActiveFlags() As Boolean
            Get
                Return _resetActiveFlags
            End Get
            Set(ByVal value As Boolean)
                _resetActiveFlags = value
            End Set
        End Property

        Public Property ExternalTaxGroupCode() As String
            Get
                Return _externalTaxGroupCode
            End Get
            Set(ByVal value As String)
                ' Tax group codes should be printable chars only.
                _externalTaxGroupCode = Trim(value)
            End Set
        End Property


#End Region

#Region "Business Rules"

        ''' <summary>
        ''' validates that Add can happen if taxClassID and taxJurisdictionID are valid values.
        ''' </summary>
        ''' <param name="taxClassID"></param>
        ''' <param name="taxJurisdicitonID"></param>
        ''' <returns>ArrayList of TaxFlagStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateAdd(ByVal taxJurisdicitonID As Integer, ByVal taxClassID As Integer) As ArrayList
            Dim statusList As New ArrayList

            If taxJurisdicitonID <= 0 Then
                statusList.Add(TaxFlagStatus.Error_Required_TaxJurisdiction)
            End If

            If taxClassID <= 0 Then
                statusList.Add(TaxFlagStatus.Error_Required_TaxClass)
            End If

            If statusList.Count = 0 Then
                statusList.Add(TaxFlagStatus.Valid)
            End If

            Return statusList
        End Function

        ''' <summary>
        ''' validates data elements of current instance of TaxFlagBO object
        ''' </summary>
        ''' <returns>ArrayList of TaxFlagStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateTaxFlagData(ByVal isEdit As Boolean, ByVal existingTaxFlags As Hashtable) As ArrayList
            Dim statusList As New ArrayList

            'tax flag key required
            If Me.TaxFlagKey Is Nothing Or (Me.TaxFlagKey IsNot Nothing AndAlso Me.TaxFlagKey.Trim.Equals("")) Then
                statusList.Add(TaxFlagStatus.Error_Required_TaxFlagKey)
            End If

            If Not isEdit Then
                'validate key against existing keys
                If existingTaxFlags IsNot Nothing AndAlso existingTaxFlags.Contains(Me.TaxFlagKey) Then
                    statusList.Add(TaxFlagStatus.Error_Duplicate_TaxFlagKey)
                End If
            End If

            If statusList.Count = 0 Then
                statusList.Add(TaxFlagStatus.Valid)
            End If

            Return statusList
        End Function

        ''' <summary>
        ''' validates the tax percent value against the following criteria:
        '''  - must exist (if Instance Flag AllowTaxFlagNulls is False)
        '''  - must be a valid numeric value
        '''  - must be between 0 and 100
        '''  - checks if digits after decimal are more than 2 digits; if yes then user is warned that value will be rounded to 2 digits
        ''' </summary>
        ''' <param name="taxPercent"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateTaxPercent(ByVal taxPercent As String) As TaxFlagStatus
            Dim status As TaxFlagStatus

            'tax percent must be numeric
            If Not InstanceDataDAO.IsFlagActive("AllowTaxFlagNulls") AndAlso (taxPercent Is Nothing Or (taxPercent IsNot Nothing AndAlso taxPercent.Trim.Equals(""))) Then
                status = TaxFlagStatus.Error_Required_TaxPercent
            ElseIf Not taxPercent.Trim.Equals("") AndAlso Not IsNumeric(taxPercent) Then
                status = TaxFlagStatus.Error_NotNumeric_TaxPercent
            ElseIf IsNumeric(taxPercent) AndAlso (Double.Parse(taxPercent) > 100 Or Double.Parse(taxPercent) < 0) Then
                'tax percent must be between 0 and 100
                status = TaxFlagStatus.Error_TaxPercentValue
            ElseIf IsNumeric(taxPercent) AndAlso taxPercent.Substring(taxPercent.IndexOf(".") + 1).Length > 2 Then
                'warn user that they have entered more than 2 digits after the decimal point and it will be rounded if they want to proceed
                status = TaxFlagStatus.Warning_TaxPercentDecimalPrecision
            Else
                status = TaxFlagStatus.Valid
            End If

            Return status
        End Function

        ''' <summary>
        ''' validates the POS ID against the following criteria:
        '''  - must be a valid numeric value
        '''  - must be greater than 0
        ''' </summary>
        ''' <param name="posID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidatePOSID(ByVal posID As String) As TaxFlagStatus
            Dim status As TaxFlagStatus

            'pos id must be numeric
            If Not InstanceDataDAO.IsFlagActive("AllowTaxFlagNulls") AndAlso (posID Is Nothing Or (posID IsNot Nothing AndAlso posID.Trim.Equals(""))) Then
                status = TaxFlagStatus.Error_Required_POSID
            ElseIf Not posID.Trim.Equals("") AndAlso Not IsNumeric(posID) Then
                status = TaxFlagStatus.Error_NotNumeric_POSID
            ElseIf IsNumeric(posID) AndAlso Double.Parse(posID) <= 0 Then
                'posID must be greater than 0
                status = TaxFlagStatus.Error_POSIDValue
            Else
                status = TaxFlagStatus.Valid
            End If

            Return status
        End Function

        ''' <summary>
        ''' validated the active flag against the following criteria:
        '''  - warns user if they are activating a flag where another flag is already active
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateTaxFlagActive() As TaxFlagStatus
            Dim status As TaxFlagStatus
            Dim taxFlagDAO As New TaxFlagDAO

            'if current TaxFlag is set to be active then check for existence of other active flags for this jurisdiction/class
            If Me.TaxFlagValue AndAlso taxFlagDAO.GetTaxFlagActiveCount(Me) > 0 Then
                status = TaxFlagStatus.Error_MultipleActiveFlags
            Else
                status = TaxFlagStatus.Valid
            End If

            Return status
        End Function

#End Region

    End Class
End Namespace
