Imports System.Runtime.Remoting
Imports instinct.General

Public Class RemotingProxy

    Private Shared wellknownTypes As System.Collections.Generic.IDictionary(Of Type, WellKnownClientTypeEntry) = Nothing

    Public Shared Function CreateProxy(ByVal type As Type) As Object
        If wellknownTypes Is Nothing Then
            InitTypeCache()
        End If

        Dim entry As WellKnownClientTypeEntry = Nothing

        wellknownTypes.TryGetValue(type, entry)

        If entry Is Nothing Then
            Throw New RemotingException("Type not found.")
        End If

        Return Activator.GetObject(type, entry.ObjectUrl)

    End Function

    Private Shared Sub InitTypeCache()

        wellknownTypes = New System.Collections.Generic.Dictionary(Of Type, WellKnownClientTypeEntry)()

        For Each entry As WellKnownClientTypeEntry In RemotingConfiguration.GetRegisteredWellKnownClientTypes()
            If entry.ObjectType Is Nothing Then
                Throw New RemotingException("A configured type could not be found. Please check the configuration file.")
            End If

            wellknownTypes.Add(entry.ObjectType, entry)
        Next
    End Sub

End Class