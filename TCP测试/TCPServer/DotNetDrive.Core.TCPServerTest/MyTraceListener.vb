Imports log4net

<Assembly: log4net.Config.XmlConfigurator()>
Public Class MyTraceListener
    Inherits TraceListener

    Shared Sub Ini()
        Trace.Listeners.Add(New MyTraceListener())
        Trace.WriteLine("系统开始运行")
    End Sub


    Dim Log As ILog

    Public Sub New()
        Log = LogManager.GetLogger(GetType(MyTraceListener))
    End Sub

    Public Overrides Sub Write(message As String)

        Log.Info(message)
    End Sub

    Public Overrides Sub WriteLine(message As String)
        Log.Info(message & vbNewLine)
    End Sub
End Class
