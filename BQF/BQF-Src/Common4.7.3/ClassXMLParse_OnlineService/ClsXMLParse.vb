Public Class ClsXMLParse

    Public TriggeredRulesDefinitions As DataSet

    Public Function GetInstinctInputString(ByVal xmlString As String) As String

        Dim app As New Application(xmlString)

        Return (app.ToString)

    End Function

    Public Function GetInstinctInputStringForSpecificSite(ByVal xmlString As String, ByVal siteName As String) As String

        Dim app As New Application(xmlString)

        Return (app.ToStringForSpecificSite(siteName))

    End Function

    Public Function GetInstinctInputString(ByVal xmlString As Xml.XmlDocument) As String

        Dim app As New Application(xmlString)

        Return (app.ToString)

    End Function

    Public Function GetOutputXMLString(ByVal outputString As String, ByVal outputUser As Boolean, ByVal outputRules As Boolean, ByVal outputRulesDescription As Boolean, ByVal outputDecision As Boolean, _
    ByVal outputUserDefinedAlert As Boolean, ByVal outputActionCountNumber As Boolean, ByVal outputNatureOfFraud As Boolean, ByVal diaryOutput As Boolean, Optional ByVal sSite As String = "", Optional ByVal FraudAlertUserId As Boolean = False) As String

        Dim out As New OutputXML(outputString, outputUserDefinedAlert, outputUser, outputRules, outputRulesDescription, outputDecision, outputActionCountNumber, outputNatureOfFraud, diaryOutput, sSite, FraudAlertUserId)
        out.TriggeredRulesDefinitions = TriggeredRulesDefinitions

        Return (out.ToString)

    End Function

End Class
