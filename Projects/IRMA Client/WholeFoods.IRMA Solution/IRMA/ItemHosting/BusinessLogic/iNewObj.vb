Option Strict Off
Option Explicit On
Interface _iNewObj
	WriteOnly Property GoodCreate As Boolean
End Interface
Friend Class iNewObj
	Implements _iNewObj
	Public WriteOnly Property GoodCreate() As Boolean Implements _iNewObj.GoodCreate
		Set(ByVal Value As Boolean)
			
		End Set
	End Property
End Class