Public Class Square
    Inherits Rectangle

    Private _sideLength As Integer

    Public Overrides Property Width As Integer
        Get
            Return _sideLength
        End Get
        Set(value As Integer)
            _sideLength = value
        End Set
    End Property

    Public Overrides Property Height As Integer
        Get
            Return _sideLength
        End Get
        Set(value As Integer)
            _sideLength = value
        End Set
    End Property
End Class