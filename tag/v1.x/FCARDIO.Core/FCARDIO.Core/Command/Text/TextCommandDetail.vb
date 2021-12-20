Imports DoNetDrive.Core.Connector

Namespace Command.Text
    Public Class TextCommandDetail
        Inherits AbstractCommandDetail
        Public Sub New(cnt As INConnectorDetail)
            MyBase.New(cnt)
        End Sub

        Protected Overrides Sub Release0()
            Return
        End Sub

        Public Overrides Function Clone() As Object
            Return MemberwiseClone()
        End Function

        Public Overrides Function Equals(other As INCommandDetail) As Boolean
            Dim ot As TextCommandDetail = TryCast(other, TextCommandDetail)
            If ot Is Nothing Then
                Return False
            Else
                Return False
            End If
        End Function
    End Class

End Namespace

