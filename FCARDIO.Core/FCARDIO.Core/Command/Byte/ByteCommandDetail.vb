Imports DoNetDrive.Core.Command.Text
Imports DoNetDrive.Core.Connector

Namespace Command.Byte
    Public Class ByteCommandDetail
        Inherits TextCommandDetail

        Public Sub New(cnt As INConnectorDetail)
            MyBase.New(cnt)
        End Sub
    End Class
End Namespace

