Option Strict Off
Option Explicit On
Friend Class clsSelectedItem
	
	'local variable(s) to hold property value(s)
	Private m_sItemDescription As String 'local copy
	Private m_sItemIdentifier As String 'local copy
	Private m_lItem_Key As Integer 'local copy
	Private m_lItemSubTeam As Integer 'local copy
	Private m_iItemCategoryID As Short
	Private m_iBrand_ID As Short
	
	
	Public Property ItemSubTeam() As Integer
		Get
			ItemSubTeam = m_lItemSubTeam
		End Get
		Set(ByVal Value As Integer)
			m_lItemSubTeam = Value
		End Set
	End Property
	
	
	Public Property Item_Key() As Integer
		Get
			Item_Key = m_lItem_Key
		End Get
		Set(ByVal Value As Integer)
			m_lItem_Key = Value
		End Set
	End Property
	
	
	Public Property ItemIdentifier() As String
		Get
			ItemIdentifier = m_sItemIdentifier
		End Get
		Set(ByVal Value As String)
			m_sItemIdentifier = Value
		End Set
	End Property
	
	
	Public Property ItemDescription() As String
		Get
			ItemDescription = m_sItemDescription
		End Get
		Set(ByVal Value As String)
			m_sItemDescription = Value
		End Set
	End Property
	
	Public ReadOnly Property ItemCategoryID() As Short
		Get
			ItemCategoryID = m_iItemCategoryID
		End Get
	End Property
	Public ReadOnly Property Brand_ID() As Short
		Get
			Brand_ID = m_iBrand_ID
		End Get
	End Property
End Class