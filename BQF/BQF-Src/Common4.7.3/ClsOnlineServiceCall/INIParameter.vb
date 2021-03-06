Imports DecTech.Library
Imports VB = Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility

Public Class INIParameter

    Public Shared Function GetINIParameterValue(ByVal groupName As String, ByVal keyName As String, Optional ByVal defaultValue As String = "")

        Dim paraValue As String

        paraValue = RetrieveINIFile.RetrieveIniParameter(groupName, keyName, VB6.GetPath & "\Instinct.ini")
        If paraValue = "" Then
            Return defaultValue.Trim
        Else
            Return paraValue.Trim
        End If

    End Function
End Class
