Imports System.ServiceProcess

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InstinctService
    Inherits System.ServiceProcess.ServiceBase

    'UserService overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' The main entry point for the process
    <MTAThread()> _
    <System.Diagnostics.DebuggerNonUserCode()> _
    Shared Sub Main()
        '#If DEBUG Then
        'Dim service As InstinctService = New InstinctService()

        'service.OnStart(New String() {})
        'System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite)

        '#Else
        Dim ServicesToRun() As System.ServiceProcess.ServiceBase                        '--->>

        ' More than one NT Service may run within the same process. To add
        ' another service to this process, change the following line to
        ' create a second service object. For example,
        '
        '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
        '


        ServicesToRun = New System.ServiceProcess.ServiceBase() {New InstinctService}   '----->>

        System.ServiceProcess.ServiceBase.Run(ServicesToRun)                            '-->>>>>
        '#End If
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        'Me.ServiceName = "InstinctServiceAction"
        Me.ServiceName = "InstinctServiceAction" & CStr(INIParameter.GetINIParameterValue("Startup", "Organisation")).Trim & CStr(INIParameter.GetINIParameterValue("Startup", "Default Country")).Trim & CStr(INIParameter.GetINIParameterValue("Startup", "Second Service Suffix")).Trim
    End Sub

End Class
