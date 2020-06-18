Public Class OutputXML

    Private TOTAL_RULE_TRIGGERED As Integer = 20

    Public Organisation As String
    Public Country_Code As String
    Public Group_Member_Code As String
    Public Application_Number As String
    Public Capture_Date As String
    Public Capture_Time As String
    Public Application_Type As String
    Public Fraud_Score As String
    Public Fraud_Alert As String
    Public Action_Taken As String
    Public Error_Code As String
    Public Action_User As String
    Public Rule_Triggered(TOTAL_RULE_TRIGGERED) As String 'Ignore the Element 0
    Public Decision_Reason As String
    Public User_Defined_Alert As String
    Public CountNbr As String
    Public NatureOfFraud As ArrayList
    Public Description_Rule_Triggered(TOTAL_RULE_TRIGGERED) As String 'Ignore the Element 0
    Public Application_User_Field16 As String
    Public Application_User_Field17 As String
    Public Application_User_Field18 As String
    Public Application_User_Field19 As String
    Public CBA_User_Field29 As String
    Public LowFraudScore As String
    Public Fraud_Alert_UserId As String
    Public Diary As ArrayList
    Public TriggeredRulesDefinitions As DataSet



    Private bolUserOutput As Boolean
    Private bolRulesOutput As Boolean
    Private bolActionCountNumberOutput As Boolean
    Private bolNatureOfFraudOutput As Boolean
    Private bolRulesDescriptionOutput As Boolean
    Private bolDiaryOutput As Boolean
    Private bolDecisionOutput As Boolean
    Private bolUserDefinedAlertOutput As Boolean
    Private bolFraudAlertUserId As Boolean
    Private strSiteWithSpecialFunctions As String
    'For CreditEase
    Public Origination_Database_1 As String
    Public Origination_Database_2 As String
    Public Origination_Database_3 As String
    Public Origination_Database_4 As String
    Public Origination_Database_5 As String
    Public Origination_Database_6 As String
    Public Origination_Database_7 As String
    Public Origination_Database_8 As String
    Public Origination_Database_9 As String
    Public Origination_Database_10 As String
    Public Origination_Database_11 As String
    Public Origination_Database_12 As String
    Public Origination_Database_13 As String
    Public Origination_Database_14 As String
    Public Origination_Database_15 As String
    Public Origination_Database_16 As String
    Public Origination_Database_17 As String
    Public Origination_Database_18 As String
    Public Origination_Database_19 As String
    Public Origination_Database_20 As String
    Public Matched_AppKeys_1 As String
    Public Matched_AppKeys_2 As String
    Public Matched_AppKeys_3 As String
    Public Matched_AppKeys_4 As String
    Public Matched_AppKeys_5 As String
    Public Matched_AppKeys_6 As String
    Public Matched_AppKeys_7 As String
    Public Matched_AppKeys_8 As String
    Public Matched_AppKeys_9 As String
    Public Matched_AppKeys_10 As String
    Public Matched_AppKeys_11 As String
    Public Matched_AppKeys_12 As String
    Public Matched_AppKeys_13 As String
    Public Matched_AppKeys_14 As String
    Public Matched_AppKeys_15 As String
    Public Matched_AppKeys_16 As String
    Public Matched_AppKeys_17 As String
    Public Matched_AppKeys_18 As String
    Public Matched_AppKeys_19 As String
    Public Matched_AppKeys_20 As String

    'For SPDB
    Public TriggeredAndProved_NormalRule_Code(25) As String 'Ignore the Element 0

    Public TriggeredAndProved_AuditRule_Code(50) As String 'Ignore the Element 0

    Public PhoneRecord_DateTime(10) As String 'Ignore the Element 0
    Public PhoneRecord_Note(10) As String 'Ignore the Element 0

    Public Sub New()
        Organisation = ""
        Country_Code = ""
        Group_Member_Code = ""
        Application_Number = ""
        Capture_Date = ""
        Capture_Time = ""
        Application_Type = ""
        Fraud_Score = ""
        Fraud_Alert = ""
        Action_Taken = ""
        Error_Code = ""
        User_Defined_Alert = ""
        Action_User = ""

        For I As Integer = 1 To TOTAL_RULE_TRIGGERED
            Rule_Triggered(I) = ""
        Next
        Decision_Reason = ""
        CountNbr = ""
        NatureOfFraud = New ArrayList
        For i As Integer = 1 To TOTAL_RULE_TRIGGERED
            Description_Rule_Triggered(i) = ""
        Next
        Application_User_Field16 = ""
        Application_User_Field17 = ""
        Application_User_Field18 = ""
        Application_User_Field19 = ""
        CBA_User_Field29 = ""
        LowFraudScore = ""
        Fraud_Alert_UserId = ""
        'For CreditEase
        Origination_Database_1 = ""
        Origination_Database_2 = ""
        Origination_Database_3 = ""
        Origination_Database_4 = ""
        Origination_Database_5 = ""
        Origination_Database_6 = ""
        Origination_Database_7 = ""
        Origination_Database_8 = ""
        Origination_Database_9 = ""
        Origination_Database_10 = ""
        Origination_Database_11 = ""
        Origination_Database_12 = ""
        Origination_Database_13 = ""
        Origination_Database_14 = ""
        Origination_Database_15 = ""
        Origination_Database_16 = ""
        Origination_Database_17 = ""
        Origination_Database_18 = ""
        Origination_Database_19 = ""
        Origination_Database_20 = ""
        Matched_AppKeys_1 = ""
        Matched_AppKeys_2 = ""
        Matched_AppKeys_3 = ""
        Matched_AppKeys_4 = ""
        Matched_AppKeys_5 = ""
        Matched_AppKeys_6 = ""
        Matched_AppKeys_7 = ""
        Matched_AppKeys_8 = ""
        Matched_AppKeys_9 = ""
        Matched_AppKeys_10 = ""
        Matched_AppKeys_11 = ""
        Matched_AppKeys_12 = ""
        Matched_AppKeys_13 = ""
        Matched_AppKeys_14 = ""
        Matched_AppKeys_15 = ""
        Matched_AppKeys_16 = ""
        Matched_AppKeys_17 = ""
        Matched_AppKeys_18 = ""
        Matched_AppKeys_19 = ""
        Matched_AppKeys_20 = ""
        Diary = New ArrayList

        'For SPDB
        For I As Integer = 1 To 25
            TriggeredAndProved_NormalRule_Code(I) = ""
        Next

        For I As Integer = 1 To 50
            TriggeredAndProved_AuditRule_Code(I) = ""
        Next

        For I As Integer = 1 To 10
            PhoneRecord_DateTime(I) = ""
        Next

        For I As Integer = 1 To 10
            PhoneRecord_Note(I) = ""
        Next
    End Sub

    Public Sub New(ByVal OutputString As String, ByVal UserDefinedAlertOutput As Boolean, ByVal userOutput As Boolean, ByVal rulesOutput As Boolean, ByVal rulesDescriptionOutput As Boolean, ByVal decisionOutput As Boolean, _
    ByVal actionCountOutput As Boolean, ByVal natureOfFraudOutput As Boolean, ByVal diaryOutput As Boolean, ByVal sSite As String, Optional FraudAlertUserIdOutput As Boolean = False)

        Me.New()
        Dim i As Integer
        Dim endOfDiary As Integer
        Dim output() As String

        Try
            bolUserDefinedAlertOutput = UserDefinedAlertOutput
            bolUserOutput = userOutput
            bolRulesOutput = rulesOutput
            bolRulesDescriptionOutput = rulesDescriptionOutput
            bolDecisionOutput = decisionOutput
            bolActionCountNumberOutput = actionCountOutput
            bolNatureOfFraudOutput = natureOfFraudOutput
            bolDiaryOutput = diaryOutput
            bolFraudAlertUserId = FraudAlertUserIdOutput
            strSiteWithSpecialFunctions = sSite

            If strSiteWithSpecialFunctions.ToUpper = "SPDB" Then
                TOTAL_RULE_TRIGGERED = 75
            End If

            ReDim Me.Rule_Triggered(TOTAL_RULE_TRIGGERED)
            ReDim Me.Description_Rule_Triggered(TOTAL_RULE_TRIGGERED)

            output = OutputString.Split("|")
            If OutputString.StartsWith("99") Then
                With Me
                    .Organisation = ""
                    .Country_Code = ""
                    .Group_Member_Code = ""
                    .Application_Number = ""
                    .Capture_Date = ""
                    .Capture_Time = ""
                    .Application_Type = ""
                    .Fraud_Score = ""
                    .Fraud_Alert = ""
                    .Action_Taken = ""
                    .Error_Code = "99"
                    .User_Defined_Alert = ""
                    .Action_User = ""
                    For J As Integer = 1 To TOTAL_RULE_TRIGGERED
                        .Rule_Triggered(J) = ""
                    Next
                    .Decision_Reason = output(1)
                    .CountNbr = ""
                    For J As Integer = 1 To TOTAL_RULE_TRIGGERED
                        .Description_Rule_Triggered(J) = ""
                    Next
                    If sSite.ToUpper = "GEINDIA" Then
                        .Application_User_Field16 = ""
                        .Application_User_Field17 = ""
                        .Application_User_Field18 = ""
                        .Application_User_Field19 = ""
                    ElseIf sSite.ToUpper = "SDB" Then
                        .Application_User_Field19 = ""
                    ElseIf sSite.ToUpper = "VAB" Then
                        .CBA_User_Field29 = ""
                    ElseIf sSite.ToUpper = "SPDB" Then
                        .LowFraudScore = ""
                        .Fraud_Alert_UserId = ""

                        'For SPDB
                        For J As Integer = 1 To 25
                            .TriggeredAndProved_NormalRule_Code(J) = ""
                        Next

                        For J As Integer = 1 To 50
                            .TriggeredAndProved_AuditRule_Code(J) = ""
                        Next

                        For J As Integer = 1 To 10
                            .PhoneRecord_DateTime(J) = ""
                        Next

                        For J As Integer = 1 To 10
                            .PhoneRecord_Note(J) = ""
                        Next
                    ElseIf sSite.ToUpper = "CRE" Then
                        'For CreditEase
                        Origination_Database_1 = ""
                        Origination_Database_2 = ""
                        Origination_Database_3 = ""
                        Origination_Database_4 = ""
                        Origination_Database_5 = ""
                        Origination_Database_6 = ""
                        Origination_Database_7 = ""
                        Origination_Database_8 = ""
                        Origination_Database_9 = ""
                        Origination_Database_10 = ""
                        Origination_Database_11 = ""
                        Origination_Database_12 = ""
                        Origination_Database_13 = ""
                        Origination_Database_14 = ""
                        Origination_Database_15 = ""
                        Origination_Database_16 = ""
                        Origination_Database_17 = ""
                        Origination_Database_18 = ""
                        Origination_Database_19 = ""
                        Origination_Database_20 = ""
                        Matched_AppKeys_1 = ""
                        Matched_AppKeys_2 = ""
                        Matched_AppKeys_3 = ""
                        Matched_AppKeys_4 = ""
                        Matched_AppKeys_5 = ""
                        Matched_AppKeys_6 = ""
                        Matched_AppKeys_7 = ""
                        Matched_AppKeys_8 = ""
                        Matched_AppKeys_9 = ""
                        Matched_AppKeys_10 = ""
                        Matched_AppKeys_11 = ""
                        Matched_AppKeys_12 = ""
                        Matched_AppKeys_13 = ""
                        Matched_AppKeys_14 = ""
                        Matched_AppKeys_15 = ""
                        Matched_AppKeys_16 = ""
                        Matched_AppKeys_17 = ""
                        Matched_AppKeys_18 = ""
                        Matched_AppKeys_19 = ""
                        Matched_AppKeys_20 = ""
                    End If
                End With
            Else
                With Me
                    If strSiteWithSpecialFunctions.ToUpper = "HLB" Then
                        .Organisation = output(0)
                        .Application_Number = output(1)
                        .Fraud_Score = output(2)
                        .Fraud_Alert = output(3)
                        .Action_Taken = output(4)
                        .Error_Code = output(5)
                        .Action_User = output(6)
                        .Decision_Reason = output(7)
                    Else
                        .Organisation = output(0)
                        .Country_Code = output(1)
                        .Group_Member_Code = output(2)
                        .Application_Number = output(3)
                        .Capture_Date = output(4)
                        .Capture_Time = output(5)
                        .Application_Type = output(6)
                        .Fraud_Score = output(7)
                        .Fraud_Alert = output(8)
                        .Action_Taken = output(9)
                        .Error_Code = output(10)
                        i = 10

                        If bolUserDefinedAlertOutput Then
                            .User_Defined_Alert = output(i + 1)
                            i = i + 1
                        End If

                        If bolUserOutput Then
                            .Action_User = output(i + 1)
                            i = i + 1
                        End If

                        If bolRulesOutput Then
                            For J As Integer = 1 To TOTAL_RULE_TRIGGERED
                                i = i + 1
                                Rule_Triggered(J) = output(i)
                            Next
                        End If

                        If bolDecisionOutput Then
                            .Decision_Reason = output(i + 1)
                            i = i + 1
                        End If

                        If bolActionCountNumberOutput Then
                            i = i + 1
                            .CountNbr = output(i)
                        End If

                        If bolNatureOfFraudOutput Then
                            While output(i + 1) = "F" AndAlso i + 2 <= output.Length
                                i = i + 2
                                .NatureOfFraud.Add(output(i))
                            End While
                        End If

                        If strSiteWithSpecialFunctions.ToUpper = "GEINDIA" Then
                            i = i + 1
                            .Application_User_Field16 = output(i)
                            i = i + 1
                            .Application_User_Field17 = output(i)
                            i = i + 1
                            .Application_User_Field18 = output(i)
                            i = i + 1
                            .Application_User_Field19 = output(i)

                            ' GE India wants the first 3 characters for Decision Reason and Sub Codes
                            If .Decision_Reason.Length > 3 Then
                                .Decision_Reason = .Decision_Reason.Substring(0, 3)
                            End If

                            If .Application_User_Field16.Length > 3 Then
                                .Application_User_Field16 = .Application_User_Field16.Substring(0, 3)
                            End If

                            If .Application_User_Field17.Length > 3 Then
                                .Application_User_Field17 = .Application_User_Field17.Substring(0, 3)
                            End If

                            If .Application_User_Field18.Length > 3 Then
                                .Application_User_Field18 = .Application_User_Field18.Substring(0, 3)
                            End If

                            If .Application_User_Field19.Length > 3 Then
                                .Application_User_Field19 = .Application_User_Field19.Substring(0, 3)
                            End If
                        ElseIf strSiteWithSpecialFunctions.ToUpper = "SDB" Then
                            i = i + 1
                            .Application_User_Field19 = output(i)
                        End If

                        If bolRulesDescriptionOutput Then
                            For J As Integer = 1 To TOTAL_RULE_TRIGGERED
                                i = i + 1
                                Description_Rule_Triggered(J) = output(i)
                            Next
                        End If

                        If sSite.ToUpper = "CRE" Then
                            .Origination_Database_1 = output(i + 1)
                            .Origination_Database_2 = output(i + 2)
                            .Origination_Database_3 = output(i + 3)
                            .Origination_Database_4 = output(i + 4)
                            .Origination_Database_5 = output(i + 5)
                            .Origination_Database_6 = output(i + 6)
                            .Origination_Database_7 = output(i + 7)
                            .Origination_Database_8 = output(i + 8)
                            .Origination_Database_9 = output(i + 9)
                            .Origination_Database_10 = output(i + 10)
                            .Origination_Database_11 = output(i + 11)
                            .Origination_Database_12 = output(i + 12)
                            .Origination_Database_13 = output(i + 13)
                            .Origination_Database_14 = output(i + 14)
                            .Origination_Database_15 = output(i + 15)
                            .Origination_Database_16 = output(i + 16)
                            .Origination_Database_17 = output(i + 17)
                            .Origination_Database_18 = output(i + 18)
                            .Origination_Database_19 = output(i + 19)
                            .Origination_Database_20 = output(i + 20)
                            i = i + 20
                            .Matched_AppKeys_1 = output(i + 1)
                            .Matched_AppKeys_2 = output(i + 2)
                            .Matched_AppKeys_3 = output(i + 3)
                            .Matched_AppKeys_4 = output(i + 4)
                            .Matched_AppKeys_5 = output(i + 5)
                            .Matched_AppKeys_6 = output(i + 6)
                            .Matched_AppKeys_7 = output(i + 7)
                            .Matched_AppKeys_8 = output(i + 8)
                            .Matched_AppKeys_9 = output(i + 9)
                            .Matched_AppKeys_10 = output(i + 10)
                            .Matched_AppKeys_11 = output(i + 11)
                            .Matched_AppKeys_12 = output(i + 12)
                            .Matched_AppKeys_13 = output(i + 13)
                            .Matched_AppKeys_14 = output(i + 14)
                            .Matched_AppKeys_15 = output(i + 15)
                            .Matched_AppKeys_16 = output(i + 16)
                            .Matched_AppKeys_17 = output(i + 17)
                            .Matched_AppKeys_18 = output(i + 18)
                            .Matched_AppKeys_19 = output(i + 19)
                            .Matched_AppKeys_20 = output(i + 20)
                            i = i + 20
                        End If

                        If bolDiaryOutput Then
                            endOfDiary = output.Length
                            If strSiteWithSpecialFunctions.ToUpper = "SPDB" Then
                                endOfDiary = endOfDiary - (25 + 50 + 10 * 2)
                            End If

                            While i + 4 < endOfDiary
                                i = i + 1
                                .Diary.Add(output(i))
                                i = i + 1
                                .Diary.Add(output(i))
                                i = i + 1
                                .Diary.Add(output(i))
                                i = i + 1
                                .Diary.Add(output(i))
                            End While
                        End If

                        If strSiteWithSpecialFunctions.ToUpper = "VAB" Then
                            i = i + 1
                            .CBA_User_Field29 = output(i)
                        End If

                        If strSiteWithSpecialFunctions.ToUpper = "SPDB" Then
                            i = i + 1
                            .LowFraudScore = output(i)
                            If bolFraudAlertUserId Then
                                i = i + 1
                                .Fraud_Alert_UserId = output(i)
                            End If

                            'For SPDB
                            For J As Integer = 1 To 25
                                i = i + 1
                                .TriggeredAndProved_NormalRule_Code(J) = output(i)
                            Next

                            For J As Integer = 1 To 50
                                i = i + 1
                                .TriggeredAndProved_AuditRule_Code(J) = output(i)
                            Next

                            For J As Integer = 1 To 10
                                i = i + 1
                                .PhoneRecord_DateTime(J) = output(i)
                                i = i + 1
                                .PhoneRecord_Note(J) = output(i)
                            Next
                        End If

                    End If

                End With
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Overrides Function ToString() As String
        Dim doc As New Xml.XmlDocument
        Dim node As Xml.XmlElement
        Dim diary_node As Xml.XmlElement
        Dim nature_of_fraud As Xml.XmlElement

        node = doc.CreateElement("OutputSchema")
        doc.AppendChild(node)

        node = doc.CreateElement("Output")
        doc.DocumentElement.AppendChild(node)

        node = doc.CreateElement("Organisation")
        node.InnerText = Organisation
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Country_Code")
        node.InnerText = Country_Code
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Group_Member_Code")
        node.InnerText = Group_Member_Code
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Application_Number")
        node.InnerText = Application_Number
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Capture_Date")
        node.InnerText = Capture_Date
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Capture_Time")
        node.InnerText = Capture_Time
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Application_Type")
        node.InnerText = Application_Type
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Fraud_Score")
        node.InnerText = Fraud_Score
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Fraud_Alert")
        node.InnerText = Fraud_Alert
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Action_Taken")
        node.InnerText = Action_Taken
        doc.DocumentElement("Output").AppendChild(node)

        node = doc.CreateElement("Error_Code")
        node.InnerText = Error_Code
        doc.DocumentElement("Output").AppendChild(node)

        If bolUserDefinedAlertOutput Then
            node = doc.CreateElement("User_Defined_Alert")
            node.InnerText = User_Defined_Alert
            doc.DocumentElement("Output").AppendChild(node)
        End If

        If bolUserOutput Then
            node = doc.CreateElement("Action_User")
            node.InnerText = Action_User
            doc.DocumentElement("Output").AppendChild(node)
        End If

        If bolRulesOutput Then
            For I As Integer = 1 To TOTAL_RULE_TRIGGERED
                node = doc.CreateElement("Rule_Triggered_" + I.ToString)
                node.InnerText = Rule_Triggered(I)
                doc.DocumentElement("Output").AppendChild(node)
            Next
        End If

        If bolDecisionOutput Then
            node = doc.CreateElement("Decision_Reason")
            node.InnerText = Decision_Reason
            doc.DocumentElement("Output").AppendChild(node)
        End If

        If bolActionCountNumberOutput Then
            node = doc.CreateElement("CountNbr")
            node.InnerText = CountNbr
            doc.DocumentElement("Output").AppendChild(node)
        End If

        If bolNatureOfFraudOutput Then
            If NatureOfFraud.Count > 0 Then
                nature_of_fraud = doc.CreateElement("NatureOfFraud")
                doc.DocumentElement("Output").AppendChild(nature_of_fraud)

                For index As Integer = 0 To NatureOfFraud.Count - 1
                    node = doc.CreateElement("FraudCode")
                    node.InnerText = NatureOfFraud(index)
                    nature_of_fraud.AppendChild(node)
                Next
            End If

        End If

        If strSiteWithSpecialFunctions.ToUpper = "GEINDIA" Then
            node = doc.CreateElement("Sub_Code2")
            node.InnerText = Application_User_Field16
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Sub_Code3")
            node.InnerText = Application_User_Field17
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Sub_Code4")
            node.InnerText = Application_User_Field18
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Sub_Code5")
            node.InnerText = Application_User_Field19
            doc.DocumentElement("Output").AppendChild(node)
        ElseIf strSiteWithSpecialFunctions.ToUpper = "SDB" Then
            node = doc.CreateElement("Call_Number")
            node.InnerText = Application_User_Field19
            doc.DocumentElement("Output").AppendChild(node)
        End If

        If bolRulesDescriptionOutput Then
            For I As Integer = 1 To TOTAL_RULE_TRIGGERED
                node = doc.CreateElement("Description_Rule_Triggered_" + I.ToString)
                node.InnerText = Description_Rule_Triggered(I)
                doc.DocumentElement("Output").AppendChild(node)
            Next
        End If

        If strSiteWithSpecialFunctions.ToUpper = "CRE" Then
            node = doc.CreateElement("Origination_Database_1")
            node.InnerText = Origination_Database_1
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_2")
            node.InnerText = Origination_Database_2
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_3")
            node.InnerText = Origination_Database_3
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_4")
            node.InnerText = Origination_Database_4
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_5")
            node.InnerText = Origination_Database_5
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_6")
            node.InnerText = Origination_Database_6
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_7")
            node.InnerText = Origination_Database_7
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_8")
            node.InnerText = Origination_Database_8
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_9")
            node.InnerText = Origination_Database_9
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_10")
            node.InnerText = Origination_Database_10
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_11")
            node.InnerText = Origination_Database_11
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_12")
            node.InnerText = Origination_Database_12
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_13")
            node.InnerText = Origination_Database_13
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_14")
            node.InnerText = Origination_Database_14
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_15")
            node.InnerText = Origination_Database_15
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_16")
            node.InnerText = Origination_Database_16
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_17")
            node.InnerText = Origination_Database_17
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_18")
            node.InnerText = Origination_Database_18
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_19")
            node.InnerText = Origination_Database_19
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Origination_Database_20")
            node.InnerText = Origination_Database_20
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_1")
            node.InnerText = Matched_AppKeys_1
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_2")
            node.InnerText = Matched_AppKeys_2
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_3")
            node.InnerText = Matched_AppKeys_3
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_4")
            node.InnerText = Matched_AppKeys_4
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_5")
            node.InnerText = Matched_AppKeys_5
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_6")
            node.InnerText = Matched_AppKeys_6
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_7")
            node.InnerText = Matched_AppKeys_7
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_8")
            node.InnerText = Matched_AppKeys_8
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_9")
            node.InnerText = Matched_AppKeys_9
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_10")
            node.InnerText = Matched_AppKeys_10
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_11")
            node.InnerText = Matched_AppKeys_11
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_12")
            node.InnerText = Matched_AppKeys_12
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_13")
            node.InnerText = Matched_AppKeys_13
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_14")
            node.InnerText = Matched_AppKeys_14
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_15")
            node.InnerText = Matched_AppKeys_15
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_16")
            node.InnerText = Matched_AppKeys_16
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_17")
            node.InnerText = Matched_AppKeys_17
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_18")
            node.InnerText = Matched_AppKeys_18
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_19")
            node.InnerText = Matched_AppKeys_19
            doc.DocumentElement("Output").AppendChild(node)

            node = doc.CreateElement("Matched_AppKeys_20")
            node.InnerText = Matched_AppKeys_20
            doc.DocumentElement("Output").AppendChild(node)
        End If

        If bolDiaryOutput Then
            If strSiteWithSpecialFunctions.ToUpper = "VAB" Then
                For index As Integer = 0 To Diary.Count - 1 Step 4
                    node = doc.CreateElement("Diary")
                    node.InnerText = Diary(index + 3)
                    doc.DocumentElement("Output").AppendChild(node)
                Next
            Else
                For index As Integer = 0 To Diary.Count - 1 Step 4
                    diary_node = doc.CreateElement("Diary")
                    doc.DocumentElement("Output").AppendChild(diary_node)

                    node = doc.CreateElement("Diary_Date")
                    node.InnerText = Diary(index)
                    diary_node.AppendChild(node)

                    node = doc.CreateElement("Diary_Time")
                    node.InnerText = Diary(index + 1)
                    diary_node.AppendChild(node)

                    node = doc.CreateElement("Diary_User_Id")
                    node.InnerText = Diary(index + 2)
                    diary_node.AppendChild(node)

                    node = doc.CreateElement("Diary_Note")
                    node.InnerText = Diary(index + 3)
                    diary_node.AppendChild(node)
                Next
            End If

        End If

        If strSiteWithSpecialFunctions.ToUpper = "VAB" Then
            node = doc.CreateElement("CBA_User_Field29")
            node.InnerText = CBA_User_Field29
            doc.DocumentElement("Output").AppendChild(node)
        End If

        If strSiteWithSpecialFunctions.ToUpper = "SPDB" Then
            node = doc.CreateElement("Low_Fraud_Score")
            node.InnerText = LowFraudScore
            doc.DocumentElement("Output").AppendChild(node)
            If bolFraudAlertUserId Then
                node = doc.CreateElement("Fraud_Alert_UserId")
                node.InnerText = Fraud_Alert_UserId
                doc.DocumentElement("Output").AppendChild(node)
            End If

            For I As Integer = 1 To 25
                node = doc.CreateElement("TriggeredAndApprovedNormalRuleCode" + I.ToString)
                node.InnerText = TriggeredAndProved_NormalRule_Code(I)
                doc.DocumentElement("Output").AppendChild(node)
            Next

            For I As Integer = 1 To 50
                node = doc.CreateElement("TriggeredAndApprovedAuditRuleCode" + I.ToString)
                node.InnerText = TriggeredAndProved_AuditRule_Code(I)
                doc.DocumentElement("Output").AppendChild(node)
            Next

            For I As Integer = 1 To 10
                node = doc.CreateElement("PhoneRecordDatetime" + I.ToString)
                node.InnerText = PhoneRecord_DateTime(I)
                doc.DocumentElement("Output").AppendChild(node)

                node = doc.CreateElement("PhoneRecordNote" + I.ToString)
                node.InnerText = PhoneRecord_Note(I)
                doc.DocumentElement("Output").AppendChild(node)
            Next

        End If

        'SIGCN
        If strSiteWithSpecialFunctions.ToUpper = "SIGCN" Then

            Dim triggeredRulesNode As Xml.XmlElement

            If TriggeredRulesDefinitions.Tables(0).Rows.Count > 0 Then


                triggeredRulesNode = doc.CreateElement("Triggered_Rules")
                doc.DocumentElement("Output").AppendChild(triggeredRulesNode)


                For Each row As DataRow In TriggeredRulesDefinitions.Tables(1).Rows
                    ' For each rule
                    Dim currRule As String = row(0).ToString()
                    Dim rows As DataRow() = TriggeredRulesDefinitions.Tables(0).Select("Rule_Code = '" + currRule + "'")


                    Dim triggeredRuleNode As Xml.XmlElement

                    triggeredRuleNode = doc.CreateElement("Triggered_Rule")
                    triggeredRulesNode.AppendChild(triggeredRuleNode)

                    Dim ruleCode As String = rows(0)("Rule_Code").ToString()
                    Dim ruleDescrition As String = rows(0)("Rules_Description").ToString()
                    Dim matchedKeys As String = rows(0)("Matched_AppKeys").ToString()

                    node = doc.CreateElement("Rule_Code")
                    node.InnerText = ruleCode
                    triggeredRuleNode.AppendChild(node)

                    node = doc.CreateElement("Rule_Description")
                    node.InnerText = ruleDescrition
                    triggeredRuleNode.AppendChild(node)


                    ' Rule Definitions
                    Dim triggeredRuleDefinitions As Xml.XmlElement
                    triggeredRuleDefinitions = doc.CreateElement("Rule_Definitions")
                    triggeredRuleNode.AppendChild(triggeredRuleDefinitions)


                    For Each ruleRow As DataRow In rows
                        ' For each rule row definition                       
                        Dim triggeredRuleDefinition As Xml.XmlElement
                        triggeredRuleDefinition = doc.CreateElement("Rule_Definition")                        
                        triggeredRuleDefinitions.AppendChild(triggeredRuleDefinition)

                        Dim applicationCategoryLabelNode As Xml.XmlElement
                        applicationCategoryLabelNode = doc.CreateElement("ApplicationCategory_Label")
                        applicationCategoryLabelNode.InnerText = ruleRow("ApplicationCategory_Label").ToString()
                        triggeredRuleDefinition.AppendChild(applicationCategoryLabelNode)

                        Dim applicationFieldLabelNode As Xml.XmlElement
                        applicationFieldLabelNode = doc.CreateElement("ApplicationField_Label")
                        applicationFieldLabelNode.InnerText = ruleRow("ApplicationField_Label").ToString()
                        triggeredRuleDefinition.AppendChild(applicationFieldLabelNode)

                        Dim operatorTypeNode As Xml.XmlElement
                        operatorTypeNode = doc.CreateElement("Operator_Type")
                        operatorTypeNode.InnerText = ruleRow("Operator_Type").ToString()
                        triggeredRuleDefinition.AppendChild(operatorTypeNode)

                        Dim assignmentValueNode As Xml.XmlElement
                        assignmentValueNode = doc.CreateElement("Assignment_Value")
                        assignmentValueNode.InnerText = ruleRow("Assignment_Value").ToString()
                        triggeredRuleDefinition.AppendChild(assignmentValueNode)

                        Dim databaseCategoryLabelNode As Xml.XmlElement
                        databaseCategoryLabelNode = doc.CreateElement("DatabaseCategory_Label")
                        databaseCategoryLabelNode.InnerText = ruleRow("DatabaseCategory_Label").ToString()
                        triggeredRuleDefinition.AppendChild(databaseCategoryLabelNode)

                        Dim databaseFieldLabelNode As Xml.XmlElement
                        databaseFieldLabelNode = doc.CreateElement("DatabaseField_Label")
                        databaseFieldLabelNode.InnerText = ruleRow("DatabaseField_Label").ToString()
                        triggeredRuleDefinition.AppendChild(databaseFieldLabelNode)
                    Next


                    node = doc.CreateElement("Matched_AppKeys")
                    node.InnerText = matchedKeys
                    triggeredRuleNode.AppendChild(node)
                Next

            End If

        End If

        If strSiteWithSpecialFunctions = "MBBMY" Then
            'Return "<?xml version=""1.0"" encoding=""UTF-8""?><![CDATA[" & doc.OuterXml & "]]>"

            Dim soapWrapperStart As String = "<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ins=""http://dectechsolutions.com/Instinct""><soapenv:Header/><soapenv:Body><ins:InstinctActionXMLStringReturn soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/""><actionString xsi:type=""soapenv:_clsParamsApplication"">"
            Dim soapWrapperEnd As String = " </actionString></ins:InstinctActionXMLStringReturn></soapenv:Body></soapenv:Envelope>"

            Return soapWrapperStart + "<![CDATA[" + doc.OuterXml + "]]>" + soapWrapperEnd
        Else
            Return "<?xml version=""1.0"" encoding=""UTF-8""?>" & doc.OuterXml
        End If

    End Function

End Class
