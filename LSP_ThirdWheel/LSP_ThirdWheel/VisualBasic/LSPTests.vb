<TestClass()>
Public Class LSPTests
    <TestMethod()>
    Public Sub Square_SetHeight_DoesNotAffectWidth()
        'arrange
        Dim square = New Square()

        'act and assert
        SetHeight_DoesNotAffectWidth(square)
    End Sub

    <TestMethod()>
    Public Sub Rectangle_SetHeight_DoesNotAffectWidth()
        'arrange
        Dim rectangle = New Rectangle()

        'act and assert
        SetHeight_DoesNotAffectWidth(rectangle)
    End Sub

    Public Sub SetHeight_DoesNotAffectWidth(rectangle As Rectangle)
        'arrange
        Dim expectedWidth = 4
        rectangle.Width = 4

        'act
        rectangle.Height = 7

        'assert
        Assert.AreEqual(expectedWidth, rectangle.Width)
    End Sub
End Class