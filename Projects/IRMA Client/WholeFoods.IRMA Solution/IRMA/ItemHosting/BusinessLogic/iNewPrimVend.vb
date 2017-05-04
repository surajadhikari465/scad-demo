Option Strict Off
Option Explicit On
Interface _iNewPrimVend
	WriteOnly Property GoodCreate As Boolean
	WriteOnly Property VendorID As Integer
	WriteOnly Property ItemID As Integer
	Sub RefreshControls()
	WriteOnly Property StoreNo As Integer
End Interface
Friend Class iNewPrimVend
	Implements _iNewPrimVend
	
	Public WriteOnly Property GoodCreate() As Boolean Implements _iNewPrimVend.GoodCreate
		Set(ByVal Value As Boolean)
			
		End Set
	End Property
	Public WriteOnly Property VendorID() As Integer Implements _iNewPrimVend.VendorID
		Set(ByVal Value As Integer)
			
		End Set
	End Property
	Public WriteOnly Property ItemID() As Integer Implements _iNewPrimVend.ItemID
		Set(ByVal Value As Integer)
			
		End Set
	End Property
	Public WriteOnly Property StoreNo() As Integer Implements _iNewPrimVend.StoreNo
		Set(ByVal Value As Integer)
			
		End Set
	End Property
	Public Sub RefreshControls() Implements _iNewPrimVend.RefreshControls
		
	End Sub
End Class