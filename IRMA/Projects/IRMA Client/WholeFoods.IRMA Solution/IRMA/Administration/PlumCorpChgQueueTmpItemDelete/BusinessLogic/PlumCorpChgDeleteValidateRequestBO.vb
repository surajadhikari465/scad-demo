Imports System.Linq
Public Class PlumCorpChgDeleteValidateRequestBO
  Public Property Models As List(Of PlumCorpChgDeleteModel)

  Public ReadOnly Property ValidModels As List(Of PlumCorpChgDeleteModel)
    Get
      Return If(Models Is Nothing, New List(Of PlumCorpChgDeleteModel), Models.Where(Function(x) x.IsExists).ToList())
    End Get
  End Property

  Public ReadOnly Property ErrorModels As List(Of PlumCorpChgDeleteModel)
    Get
      Return If(Models Is Nothing, New List(Of PlumCorpChgDeleteModel), Models.Where(Function(x) Not x.IsExists).ToList())
    End Get
  End Property
End Class
