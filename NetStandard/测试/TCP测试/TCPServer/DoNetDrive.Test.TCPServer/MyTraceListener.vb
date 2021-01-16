Imports System.IO
Imports log4net
Imports log4net.Config

Public Class MyTraceListener
    Inherits TraceListener

    Shared Sub Ini()
        XmlConfigurator.Configure(
            New FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CfgFiles\log4net.config")))

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
