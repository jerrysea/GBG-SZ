Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Xml.XmlDocument
Imports DecTech.Library
Imports Microsoft.ApplicationBlocks.Data


Public Class InstinctService

    Private TOTAL_RULE_TRIGGERED As Integer = 20

    Public Sub Test()
        Call OnStart(Nothing)
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        Try
            ' Sleep for debugging
            System.Threading.Thread.Sleep(5000)

            Retrieve_INI_Parameters()

            If CStr(INIParameter.GetINIParameterValue("Startup", "Use Defined Encryption Key", "N")).Trim = "Y" _
            AndAlso CStr(INIParameter.GetINIParameterValue("Startup", "Use Windows Authentication", "N")).Trim = "N" Then
                System.Runtime.Remoting.RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, False)
                ConnectDatabaseUsingDefinedEncryptionKey()
            Else
                ConnectDatabase()
            End If

            System.Threading.Thread.Sleep(INI_Pooling_Interval * 1000)

            Verify_OrganisationAndDefaultCountry()

            If INI_Return_Action_By_Application_Type = "Y" Then
                Retrieve_Web_Service_Info()
                Retrieve_Mq_Service_Info()
            End If

            If String.Compare(INI_Site_With_Special_Functions, "SPDB", True) = 0 Then
                TOTAL_RULE_TRIGGERED = 75
            End If

            ' Add new thread for polling application actions
            ActionThread = New Thread(AddressOf Poll_Application_Actions)
            ActionThread.Start()

            If String.Compare(INI_Site_With_Special_Functions, "INGTR", True) = 0 Then
                CriminalThread = New Thread(AddressOf Poll_Criminal_Extract)
                CriminalThread.Start()
            End If
        Catch ex As Exception
            ' Write error to WIN2K system event log
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up error. Error Number = " & Err.Number & ".  Error Descripion: " & Err.Description & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
    End Sub

    Private Sub Poll_Application_Actions()
        Try
            Dim dsApplicationAction As DataSet
            Dim dsApplication As DataSet

            Dim bUserIdInOutput As Boolean = False
            Dim bRulesInOutput As Boolean = False
            Dim bRulesDescriptionInOutput As Boolean = False
            Dim bActionCountNumberInOutput As Boolean = False
            Dim bNatureOfFraudInOutput As Boolean = False
            Dim bDiaryInOutput As Boolean = False
            Dim bDecisionReasonInOutput As Boolean = False
            Dim bUserDefinedAlert As Boolean = False
            Dim bFraudAlertUserId As Boolean = False
            Dim iField_Length As Short
            Dim OnlineReturnRecord As String

            Dim sAppKey As String
            Dim sOrganisation As String
            Dim sCountry_Code As String
            Dim sGroup_Member_Code As String
            Dim sApplication_Number As String
            Dim sCapture_Date As String
            Dim sCapture_Time As String
            Dim sApplication_Type As String
            Dim iFraud_Score As Integer
            Dim sFraud_Alert As String
            Dim sAction_Taken As String
            Dim sAction_User As String
            Dim bActionByMachine As Boolean = False ' check if action = system or blank ,then think it is action by auto

            Dim sError_Code As String
            Dim sRule_Code As String
            Dim sRule_Description As String
            Dim sLoad_Mode As String
            Dim strLowFraudScoreFlag As String
            Dim sFraud_Alert_UserId As String = ""
            Dim iIndex As Integer

            Dim bSkipAppFlag As Boolean = False

            While True
                sApplication_Number = ""

                Try
                    dsApplicationAction = InstinctFunctions.GetActionedApplications(INI_Site_With_Special_Functions.Trim.ToUpper)

                    If Not dsApplicationAction Is Nothing _
                    AndAlso dsApplicationAction.Tables.Count > 0 _
                    AndAlso dsApplicationAction.Tables(0).Rows.Count > 0 Then
                        For Each drApplicationAction As DataRow In dsApplicationAction.Tables(0).Rows

                            '****************************************************************************************************
                            '--Description: Adding Retry Delay parameter logic - Task 22704
                            '--Edit by : Patrick - 03/10/2014
                            '****************************************************************************************************

                            ' Check if this app is in the RetryDelayList
                            bSkipAppFlag = False
                            bActionByMachine = False
                            For Each currentRecord As RetryDelayRecord In RetryDelayList
                                If currentRecord.SerialNo = drApplicationAction.Item("Serial_Number").ToString.Trim Then
                                    ' See if it is time for a retry
                                    If (currentRecord.ReadyToRetry() = False) Then
                                        ' Do not continue with this app, skip to next
                                        bSkipAppFlag = True
                                        Exit For
                                    Else
                                        ' Time to retry this app
                                        Exit For
                                    End If
                                End If
                            Next

                            If bSkipAppFlag = True Then
                                ' This app isn't ready to be retried, continue to the next app
                                Continue For
                            End If

                            'Get AppKey
                            If Not Convert.IsDBNull(drApplicationAction("AppKey")) Then
                                sAppKey = drApplicationAction("AppKey").ToString.Trim
                            Else
                                sAppKey = ""
                            End If

                            ' Get Organisation
                            If Not Convert.IsDBNull(drApplicationAction("Organisation")) Then
                                sOrganisation = drApplicationAction("Organisation").ToString.Trim
                            Else
                                sOrganisation = ""
                            End If

                            ' Get Country Code
                            If Not Convert.IsDBNull(drApplicationAction("Country_Code")) Then
                                sCountry_Code = drApplicationAction("Country_Code").ToString.Trim
                            Else
                                sCountry_Code = ""
                            End If

                            ' Get Application Number
                            If Not Convert.IsDBNull(drApplicationAction("Application_Number")) Then
                                sApplication_Number = drApplicationAction("Application_Number").ToString.Trim
                            Else
                                sApplication_Number = ""
                            End If

                            ' Get Application Type
                            If Not Convert.IsDBNull(drApplicationAction("Application_Type")) Then
                                sApplication_Type = drApplicationAction("Application_Type").ToString.Trim
                            Else
                                sApplication_Type = ""
                            End If

                            ' Make sure this application exists                            
                            dsApplication = InstinctFunctions.GetSpecificApplicationDetails(sAppKey, _
                            INI_Rules_in_Output_File.Trim.ToUpper, INI_Rules_Description_in_Output_File.Trim.ToUpper, _
                            INI_Action_Count_Number_in_Output_File.Trim.ToUpper, INI_Nature_Of_Fraud_in_Output_File.Trim.ToUpper, _
                            INI_Diary_in_Output_File.Trim.ToUpper, INI_Site_With_Special_Functions.ToUpper)

                            If Not dsApplication Is Nothing _
                            AndAlso dsApplication.Tables.Count = 6 _
                            AndAlso dsApplication.Tables(0).Rows.Count > 0 Then
                                OnlineReturnRecord = ""

                                ' Get Group Member Code
                                If INI_Site_With_Special_Functions.ToUpper.Trim = "GMC" Then
                                    If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field20")) Then
                                        sGroup_Member_Code = dsApplication.Tables(0).Rows(0)("Application_User_Field20").ToString.Trim
                                    Else
                                        sGroup_Member_Code = INI_Group_Member_Code.Trim
                                    End If
                                Else
                                    sGroup_Member_Code = INI_Group_Member_Code.Trim
                                End If

                                ' Get Capture Date
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Capture_Date")) Then
                                    sCapture_Date = Format(dsApplication.Tables(0).Rows(0)("Capture_Date"), "dd/MM/yyyy")
                                Else
                                    sCapture_Date = ""
                                End If

                                ' Get Capture Time
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Capture_Time")) Then
                                    sCapture_Time = Format(dsApplication.Tables(0).Rows(0)("Capture_Time"), "HHmmss")
                                Else
                                    sCapture_Time = ""
                                End If

                                ' Get Fraud Score
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Fraud_Score")) Then
                                    iFraud_Score = dsApplication.Tables(0).Rows(0)("Fraud_Score").ToString.Trim
                                Else
                                    iFraud_Score = 0
                                End If

                                ' Get Fraud Alert
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Fraud_Alert")) Then
                                    sFraud_Alert = dsApplication.Tables(0).Rows(0)("Fraud_Alert").ToString.Trim
                                Else
                                    sFraud_Alert = ""
                                End If

                                ' Get Action Taken
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Action_Taken")) Then
                                    sAction_Taken = dsApplication.Tables(0).Rows(0)("Action_Taken").ToString.Trim
                                Else
                                    sAction_Taken = ""
                                End If

                                ' Get Action User
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("User_Id")) Then
                                    sAction_User = dsApplication.Tables(0).Rows(0)("User_Id").ToString.Trim
                                Else
                                    sAction_User = ""
                                End If

                                'Check if Action User is SYSTEM or "" then it is action by machine
                                If sAction_User = "" Or sAction_User.ToUpper() = "SYSTEM" Then
                                    bActionByMachine = True
                                End If

                                ' Get Error Code
                                sError_Code = ""

                                ' Get Load Mode
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Load_Mode")) Then
                                    sLoad_Mode = dsApplication.Tables(0).Rows(0)("Load_Mode").ToString.Trim
                                Else
                                    sLoad_Mode = ""
                                End If

                                'Get Fraud_Alert_UserId
                                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Fraud_Alert_User_Id")) Then
                                    sFraud_Alert_UserId = dsApplication.Tables(0).Rows(0)("Fraud_Alert_User_Id").ToString.Trim
                                Else
                                    sFraud_Alert_UserId = ""
                                End If

                                ' Create the Action Output file                                 
                                If INI_Output_Format = "FIXED" Then
                                    ' Fixed
                                    iField_Length = 3
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sOrganisation, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 2
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sCountry_Code, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 4
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sGroup_Member_Code, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 25
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sApplication_Number, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 10
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sCapture_Date, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 6
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sCapture_Time, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 4
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sApplication_Type, iField_Length) & INI_Delimiter_Character

                                    If iFraud_Score > 999 Then
                                        ' Only return a maximum Fraud Score value of 999
                                        OnlineReturnRecord = OnlineReturnRecord & "999" & INI_Delimiter_Character
                                    ElseIf iFraud_Score < 0 Then
                                        ' Only return a zero Fraud Score value if negative
                                        OnlineReturnRecord = OnlineReturnRecord & "000" & INI_Delimiter_Character
                                    Else
                                        OnlineReturnRecord = OnlineReturnRecord & Format(iFraud_Score, "000") & INI_Delimiter_Character
                                    End If

                                    iField_Length = 1
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sFraud_Alert, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 1
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sAction_Taken, iField_Length) & INI_Delimiter_Character

                                    iField_Length = 2
                                    OnlineReturnRecord = OnlineReturnRecord & "  " & INI_Delimiter_Character

                                    If INI_User_Defined_Alert_in_Output_File = "Y" Then
                                        bUserDefinedAlert = True
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0).Item("User_Defined_Alert")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0).Item("User_Defined_Alert").ToString.Trim, 10) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", 10) & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_User_Id_in_Output_File = "Y" Then
                                        bUserIdInOutput = True
                                        If sAction_User <> "" Then 'Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0).Item("User_Id")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(sAction_User, 20) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", 20) & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Rules_in_Output_File = "Y" Then
                                        bRulesInOutput = True
                                        sRule_Code = ""

                                        ' Rule table having 1 row, 1 column means no data
                                        If dsApplication.Tables(1).Rows.Count <= 1 _
                                        AndAlso dsApplication.Tables(1).Columns.Count <= 1 Then
                                            For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                                                sRule_Code += "       " & INI_Delimiter_Character
                                            Next
                                        Else
                                            iIndex = 1
                                            For Each drRuleCode As DataRow In dsApplication.Tables(1).Rows
                                                sRule_Code += InstinctFunctions.PadWithSpaces(drRuleCode(0).ToString.Trim, 7) & INI_Delimiter_Character
                                                iIndex += 1
                                                If iIndex > TOTAL_RULE_TRIGGERED Then Exit For
                                            Next

                                            ' Add up to 20 rules
                                            ' If not up to 20, use space to add for fixed length
                                            Do Until sRule_Code.Split(INI_Delimiter_Character).Length > TOTAL_RULE_TRIGGERED
                                                sRule_Code += "       " & INI_Delimiter_Character
                                            Loop
                                        End If

                                        OnlineReturnRecord += sRule_Code
                                    End If

                                    If INI_Decision_Reason_in_Output_File = "Y" Then
                                        bDecisionReasonInOutput = True
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Decision_Reason")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0)("Decision_Reason").ToString.Trim, 100) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", 100) & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Action_Count_Number_in_Output_File = "Y" Then
                                        bActionCountNumberInOutput = True

                                        If Not Convert.IsDBNull(dsApplication.Tables(4).Rows(0)(0)) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(4).Rows(0)(0).ToString.Trim, 3) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += "000" & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Nature_Of_Fraud_in_Output_File = "Y" Then
                                        bNatureOfFraudInOutput = True

                                        For Each drNarureOfFraud As DataRow In dsApplication.Tables(5).Rows
                                            iField_Length = 10
                                            OnlineReturnRecord += "F" & INI_Delimiter_Character & InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drNarureOfFraud(0)), "", drNarureOfFraud(0)), iField_Length) & INI_Delimiter_Character
                                        Next
                                    End If

                                    If INI_Site_With_Special_Functions.ToUpper.Trim = "GEINDIA" Then
                                        ' Sub Code 2 - 5 (Application User Fields 16 - 19)
                                        iField_Length = 50
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field16")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0)("Application_User_Field16").ToString.Trim, iField_Length) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                        End If

                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field17")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0)("Application_User_Field17").ToString.Trim, iField_Length) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                        End If

                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field18")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0)("Application_User_Field18").ToString.Trim, iField_Length) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                        End If

                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field19")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0)("Application_User_Field19").ToString.Trim, iField_Length) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                        End If
                                    ElseIf INI_Site_With_Special_Functions.ToUpper.Trim = "SDB" Then
                                        iField_Length = 50
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field19")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0)("Application_User_Field19").ToString.Trim, iField_Length) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Rules_Description_in_Output_File = "Y" Then
                                        bRulesDescriptionInOutput = True
                                        sRule_Description = ""

                                        ' Rule table having 1 row, 1 column means no data
                                        If dsApplication.Tables(1).Rows.Count <= 1 _
                                        AndAlso dsApplication.Tables(1).Columns.Count <= 1 Then
                                            For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                                                sRule_Description += Space(100) & INI_Delimiter_Character
                                            Next
                                        Else
                                            iIndex = 1
                                            For Each drRuleDescription As DataRow In dsApplication.Tables(1).Rows
                                                sRule_Description += InstinctFunctions.PadWithSpaces(drRuleDescription(1).ToString.Trim, 100) & INI_Delimiter_Character
                                                iIndex += 1
                                                If iIndex > TOTAL_RULE_TRIGGERED Then Exit For
                                            Next

                                            ' Add up to 20 rules
                                            ' If not up to 20, use space to add for fixed length
                                            Do Until sRule_Description.Split(INI_Delimiter_Character).Length > TOTAL_RULE_TRIGGERED
                                                sRule_Description += Space(100) & INI_Delimiter_Character
                                            Loop
                                        End If

                                        OnlineReturnRecord += sRule_Description
                                    End If

                                    If INI_Diary_in_Output_File = "Y" Then
                                        bDiaryInOutput = True

                                        ' Get the manual diary                                                                               
                                        If dsApplication.Tables(2).Rows.Count > 0 Then
                                            For Each drManualDiary As DataRow In dsApplication.Tables(2).Rows
                                                iField_Length = 10
                                                If Not Convert.IsDBNull(drManualDiary(0)) Then
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(0).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                                End If

                                                iField_Length = 6
                                                If Not Convert.IsDBNull(drManualDiary(1)) Then
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(1).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                                End If

                                                iField_Length = 20
                                                If Not Convert.IsDBNull(drManualDiary(2)) Then
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(2).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                                End If

                                                iField_Length = 2000
                                                If Not Convert.IsDBNull(drManualDiary(3)) Then
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(3).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                                End If
                                            Next
                                        ElseIf dsApplication.Tables(3).Rows.Count > 0 Then
                                            iField_Length = 10
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(3).Rows(0)(0).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                            iField_Length = 6
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(3).Rows(0)(1).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                            iField_Length = 20
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(3).Rows(0)(2).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                            iField_Length = 2000
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(3).Rows(0)(3).ToString.Trim, iField_Length) & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Site_With_Special_Functions.ToUpper.Trim = "VAB" Then
                                        iField_Length = 5000
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("CBA_User_Field29")) Then
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces(dsApplication.Tables(0).Rows(0)("CBA_User_Field29").ToString.Trim, iField_Length) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) & INI_Delimiter_Character
                                        End If
                                    End If
                                Else    ' Variable   
                                    OnlineReturnRecord = sOrganisation & INI_Delimiter_Character & _
                                                         sCountry_Code & INI_Delimiter_Character & _
                                                         sGroup_Member_Code & INI_Delimiter_Character & _
                                                         sApplication_Number & INI_Delimiter_Character & _
                                                         sCapture_Date & INI_Delimiter_Character & _
                                                         sCapture_Time & INI_Delimiter_Character & _
                                                         sApplication_Type & INI_Delimiter_Character

                                    If iFraud_Score > 999 Then
                                        ' Only return a maximum Fraud Score value of 999
                                        OnlineReturnRecord = OnlineReturnRecord & "999" & INI_Delimiter_Character
                                    ElseIf iFraud_Score < 0 Then
                                        ' Only return a zero Fraud Score value if negative
                                        OnlineReturnRecord = OnlineReturnRecord & "000" & INI_Delimiter_Character
                                    Else
                                        OnlineReturnRecord = OnlineReturnRecord & iFraud_Score & INI_Delimiter_Character
                                    End If

                                    OnlineReturnRecord = OnlineReturnRecord & _
                                                         sFraud_Alert & INI_Delimiter_Character & _
                                                         sAction_Taken & INI_Delimiter_Character & _
                                                         sError_Code & INI_Delimiter_Character

                                    If INI_User_Defined_Alert_in_Output_File = "Y" Then
                                        bUserDefinedAlert = True
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("User_Defined_Alert")) Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("User_Defined_Alert").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_User_Id_in_Output_File = "Y" Then
                                        bUserIdInOutput = True
                                        If sAction_User <> "" Then 'Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("User_Id")) Then
                                            OnlineReturnRecord += sAction_User & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Rules_in_Output_File = "Y" Then
                                        bRulesInOutput = True
                                        sRule_Code = ""

                                        ' Rule table having 1 row, 1 column means no data
                                        If dsApplication.Tables(1).Rows.Count <= 1 _
                                        AndAlso dsApplication.Tables(1).Columns.Count <= 1 Then
                                            For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                                                sRule_Code += INI_Delimiter_Character
                                            Next
                                        Else
                                            iIndex = 1
                                            For Each drRuleCode As DataRow In dsApplication.Tables(1).Rows
                                                sRule_Code += drRuleCode(0).ToString.Trim & INI_Delimiter_Character
                                                iIndex += 1
                                                If iIndex > TOTAL_RULE_TRIGGERED Then Exit For
                                            Next

                                            ' Add up to 20 rules                                            
                                            Do Until sRule_Code.Split(INI_Delimiter_Character).Length > TOTAL_RULE_TRIGGERED
                                                sRule_Code += INI_Delimiter_Character
                                            Loop
                                        End If

                                        OnlineReturnRecord += sRule_Code
                                    End If

                                    If INI_Decision_Reason_in_Output_File = "Y" Then
                                        bDecisionReasonInOutput = True
                                        If IsDBNull(dsApplication.Tables(0).Rows(0)("Decision_Reason")) = False Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("Decision_Reason").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Action_Count_Number_in_Output_File = "Y" Then
                                        bActionCountNumberInOutput = True
                                        If Not Convert.IsDBNull(dsApplication.Tables(4).Rows(0)(0)) Then
                                            OnlineReturnRecord += dsApplication.Tables(4).Rows(0)(0).ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += "0" & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Nature_Of_Fraud_in_Output_File = "Y" Then
                                        bNatureOfFraudInOutput = True

                                        For Each drNarureOfFraud As DataRow In dsApplication.Tables(5).Rows
                                            iField_Length = 10
                                            OnlineReturnRecord += "F" & INI_Delimiter_Character & IIf(Convert.IsDBNull(drNarureOfFraud(0)), "", drNarureOfFraud(0)) & INI_Delimiter_Character
                                        Next
                                    End If

                                    If INI_Site_With_Special_Functions.ToUpper.Trim = "GEINDIA" Then
                                        ' Sub Code 2 - 5 (Application User Fields 16 - 19)
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field16")) Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("Application_User_Field16").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If

                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field17")) Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("Application_User_Field17").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If

                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field18")) Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("Application_User_Field18").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If

                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field19")) Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("Application_User_Field19").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If
                                    ElseIf INI_Site_With_Special_Functions.ToUpper.Trim = "SDB" Then
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Application_User_Field19")) Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("Application_User_Field19").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Rules_Description_in_Output_File = "Y" Then
                                        bRulesDescriptionInOutput = True
                                        sRule_Description = ""

                                        ' Rule table having 1 row, 1 column means no data
                                        If dsApplication.Tables(1).Rows.Count <= 1 _
                                        AndAlso dsApplication.Tables(1).Columns.Count <= 1 Then
                                            For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                                                sRule_Description += INI_Delimiter_Character
                                            Next
                                        Else
                                            iIndex = 1
                                            For Each drRuleDescription As DataRow In dsApplication.Tables(1).Rows
                                                sRule_Description += drRuleDescription(1).ToString.Trim & INI_Delimiter_Character
                                                iIndex += 1
                                                If iIndex > TOTAL_RULE_TRIGGERED Then Exit For
                                            Next

                                            ' Add up to 20 rules                                            
                                            Do Until sRule_Description.Split(INI_Delimiter_Character).Length > TOTAL_RULE_TRIGGERED
                                                sRule_Description += INI_Delimiter_Character
                                            Loop
                                        End If

                                        OnlineReturnRecord += sRule_Description
                                    End If

                                    If INI_Diary_in_Output_File = "Y" Then
                                        bDiaryInOutput = True

                                        ' Get the manual diary        
                                        If dsApplication.Tables(2).Rows.Count > 0 Then
                                            For Each drManualDiary As DataRow In dsApplication.Tables(2).Rows
                                                If Not Convert.IsDBNull(drManualDiary(0)) Then
                                                    OnlineReturnRecord += drManualDiary(0).ToString.Trim & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += INI_Delimiter_Character
                                                End If

                                                If Not Convert.IsDBNull(drManualDiary(1)) Then
                                                    OnlineReturnRecord += drManualDiary(1).ToString.Trim & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += INI_Delimiter_Character
                                                End If

                                                If Not Convert.IsDBNull(drManualDiary(2)) Then
                                                    OnlineReturnRecord += drManualDiary(2).ToString.Trim & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += INI_Delimiter_Character
                                                End If

                                                If Not Convert.IsDBNull(drManualDiary(3)) Then
                                                    OnlineReturnRecord += drManualDiary(3).ToString.Trim & INI_Delimiter_Character
                                                Else
                                                    OnlineReturnRecord += INI_Delimiter_Character
                                                End If
                                            Next
                                        ElseIf dsApplication.Tables(3).Rows.Count > 0 Then
                                            OnlineReturnRecord += dsApplication.Tables(3).Rows(0)(0).ToString.Trim & INI_Delimiter_Character
                                            OnlineReturnRecord += dsApplication.Tables(3).Rows(0)(1).ToString.Trim & INI_Delimiter_Character
                                            OnlineReturnRecord += dsApplication.Tables(3).Rows(0)(2).ToString.Trim & INI_Delimiter_Character
                                            OnlineReturnRecord += dsApplication.Tables(3).Rows(0)(3).ToString.Trim & INI_Delimiter_Character
                                        End If
                                    End If

                                    If INI_Site_With_Special_Functions.ToUpper.Trim = "VAB" Then
                                        If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("CBA_User_Field29")) Then
                                            OnlineReturnRecord += dsApplication.Tables(0).Rows(0)("CBA_User_Field29").ToString.Trim & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += INI_Delimiter_Character
                                        End If
                                    End If
                                End If
                                '* -------------DecTech Code Modification Comment Start--------------
                                '* 
                                '* Description: add new field at the end for SPDB special function
                                '*               
                                '* Modified by : Hugh Hu
                                '* Modified on : 9/12/2011
                                '* Solution: wI 9170
                                If INI_Site_With_Special_Functions.ToUpper = "SPDB" Then

                                    If iFraud_Score > INI_Low_Fraud_Score Then
                                        strLowFraudScoreFlag = "N"
                                    Else
                                        strLowFraudScoreFlag = "Y"
                                    End If

                                    OnlineReturnRecord += strLowFraudScoreFlag & INI_Delimiter_Character

                                    If INI_Fraud_Alert_UserId_in_Output_File.Trim.ToUpper = "Y" Then
                                        bFraudAlertUserId = True
                                        If INI_Output_Format = "FIXED" Then
                                            OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(sFraud_Alert_UserId, 20) & INI_Delimiter_Character
                                        Else
                                            OnlineReturnRecord += sFraud_Alert_UserId & INI_Delimiter_Character
                                        End If
                                    End If

                                    OnlineReturnRecord += SPDB.Instance.TriggeredRule(strConnection, drApplicationAction.Item("Appkey").ToString.Trim, True)
                                    OnlineReturnRecord += SPDB.Instance.TriggeredRule(strConnection, drApplicationAction.Item("Appkey").ToString.Trim, False)
                                    OnlineReturnRecord += SPDB.Instance.PhoneRecords(strConnection, drApplicationAction.Item("Appkey").ToString.Trim)
                                End If
                                '* --------------DecTech Code Modification Comment End---------------

                                If INI_Site_With_Special_Functions.ToUpper.Trim = "HKBMX" Then
                                    OnlineReturnRecord += InstinctFunctions.AddTimeStampExitStatusToOutput("TRUE", INI_Delimiter_Character, IIf(INI_Output_Format.ToUpper.Trim = "FIXED", True, False))
                                End If

                                '****************************************************************************************************
                                '--Description: HLB request special return message fromat as below,
                                '--             Organisation|Application(Number)|Fraud(Score)|Fraud(Alert)|Action(Taken)|Error Code|Action User Id|Decision(Reason)
                                '--Edit by : Hugh on 28/02/2012
                                '****************************************************************************************************
                                If INI_Site_With_Special_Functions.Trim = "HLB" Then
                                    Dim strOutput() As String
                                    'Dim iField_Length As Short

                                    strOutput = OnlineReturnRecord.Split(INI_Delimiter_Character)

                                    If strOutput.Length > 0 Then
                                        OnlineReturnRecord = ""

                                        ' Organisation
                                        If strOutput.Length > 0 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 3
                                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(0), iField_Length) & INI_Delimiter_Character
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(0) & INI_Delimiter_Character
                                            End If
                                        End If

                                        ' Application Number
                                        If strOutput.Length > 3 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 25
                                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(3), iField_Length) & INI_Delimiter_Character
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(3) & INI_Delimiter_Character
                                            End If
                                        End If

                                        ' Fraud Score
                                        If strOutput.Length > 7 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 3
                                                If IsNumeric(strOutput(7)) = True Then
                                                    If CInt(strOutput(7)) > 999 Then
                                                        OnlineReturnRecord = OnlineReturnRecord & "999" & INI_Delimiter_Character
                                                    ElseIf CInt(strOutput(7)) < 0 Then
                                                        OnlineReturnRecord = OnlineReturnRecord & "000" & INI_Delimiter_Character
                                                    Else
                                                        OnlineReturnRecord = OnlineReturnRecord & Format(CInt(strOutput(7)), "000") & INI_Delimiter_Character
                                                    End If
                                                Else
                                                    OnlineReturnRecord = OnlineReturnRecord & "000" & INI_Delimiter_Character
                                                End If
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(7) & INI_Delimiter_Character
                                            End If
                                        End If

                                        ' Fraud Alert
                                        If strOutput.Length > 8 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 1
                                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(8), iField_Length) & INI_Delimiter_Character
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(8) & INI_Delimiter_Character
                                            End If
                                        End If

                                        ' Action Taken
                                        If strOutput.Length > 9 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 1
                                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(9), iField_Length) & INI_Delimiter_Character
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(9) & INI_Delimiter_Character
                                            End If
                                        End If

                                        ' Error Code
                                        If strOutput.Length > 10 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 2
                                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(10), iField_Length) & INI_Delimiter_Character
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(10) & INI_Delimiter_Character
                                            End If
                                        End If

                                        ' Action User Id
                                        If INI_User_Id_in_Output_File.ToUpper.Trim = "Y" AndAlso strOutput.Length > 11 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 20
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strOutput(11), iField_Length) & INI_Delimiter_Character
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(11) & INI_Delimiter_Character
                                            End If
                                        End If

                                        ' Decision Reason
                                        If INI_Decision_Reason_in_Output_File.ToUpper.Trim = "Y" AndAlso strOutput.Length >= 12 Then
                                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                                iField_Length = 100
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strOutput(12), iField_Length) & INI_Delimiter_Character
                                            Else
                                                OnlineReturnRecord = OnlineReturnRecord & strOutput(12) & INI_Delimiter_Character
                                            End If
                                        End If
                                    End If
                                End If
                                '********************End*****************************************************************************

                                ' Has the online application been loaded via an Input file or TCP/IP?                                
                                If INI_Output_Layout.Trim.ToUpper() = "XML" Or INI_Output_Layout.Trim.ToUpper() = "JSON" Then
                                    Dim clsXMLParse As New ClassXMLParse.ClsXMLParse

                                    If INI_Site_With_Special_Functions.Trim = "SIGCN" Then
                                        clsXMLParse.TriggeredRulesDefinitions = InstinctFunctions.GetTriggeredRulesDefinitions(drApplicationAction.Item("Appkey").ToString.Trim)
                                    End If

                                    OnlineReturnRecord = clsXMLParse.GetOutputXMLString(OnlineReturnRecord,
                                                                                        bUserIdInOutput,
                                                                                        bRulesInOutput,
                                                                                        bRulesDescriptionInOutput,
                                                                                        bDecisionReasonInOutput,
                                                                                        bUserDefinedAlert,
                                                                                        bActionCountNumberInOutput,
                                                                                        bNatureOfFraudInOutput,
                                                                                        bDiaryInOutput,
                                                                                        INI_Site_With_Special_Functions,
                                                                                        bFraudAlertUserId)

                                    Select Case INI_Site_With_Special_Functions
                                        Case "BACES", "BARES", "BACPT", "BARPT"
                                            OnlineReturnRecord = Add_Header(OnlineReturnRecord, drApplicationAction.Item("Serial_Number").ToString.Trim, INI_AuthorizerID)

                                            '****************************************************************************************************
                                            '--Description: CIMBN request special format for request message.
                                            '--             Task 21568
                                            '--Edit by : Yong Liu on 26/07/2014
                                            '****************************************************************************************************
                                        Case "CIMBID"
                                            OnlineReturnRecord = Add_CIMBID_SOAP_Message(OnlineReturnRecord, sApplication_Type)

                                    End Select
                                    ''ADD A CONVERTER FOR XML TO JSON
                                    If INI_Output_Layout.ToUpper() = "JSON" Then
                                        OnlineReturnRecord = JsonXmlObjectParser.XmlToJson(OnlineReturnRecord)
                                    End If
                                End If

                                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Start Calling Action Web Service (" & sApplication_Number.Trim & ")"))

                                '****************************************************************************************************
                                '--Description: Adding Retry Delay parameter logic - Task 22704
                                '--Edit by : Patrick - 03/10/2014
                                '****************************************************************************************************

                                If ReturnAction(sAppKey, OnlineReturnRecord, sApplication_Type, bActionByMachine) Then
                                    ' Remove the record from the Application Actions table
                                    InstinctFunctions.DeleteActionedApplications(drApplicationAction.Item("Serial_Number").ToString.Trim)
                                Else
                                    If INI_Retry_Delay > 0 Then
                                        ' Add this app to the retry list
                                        RetryDelayListSafeInsert(drApplicationAction.Item("Serial_Number").ToString.Trim)
                                    End If
                                End If

                                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Finish Calling Action Web Service (" & sApplication_Number.Trim & ")"))
                                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage(" "))
                            End If

                            dsApplication = Nothing
                        Next
                    End If

                    dsApplicationAction = Nothing
                Catch ex As Exception
                    ' Write error to WIN2K system event log
                    InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Poll Application Actions error. Application Number=" & sApplication_Number.Trim & " Error Number = " & ex.Source & ".  Error Descripion: " & ex.Message & "."))
                    Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Poll Application Actions error. Application Number=" & sApplication_Number & " Error Number = " & ex.Source & ".  Error Descripion: " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
                Finally
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(INI_Pooling_Interval))
                End Try
            End While
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Poll_Application_Actions_XML = " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Private Sub Poll_Criminal_Extract()
        Try
            Dim dsCriminal As DataSet
            Dim strSerialNumber As String
            Dim strAppKey As String = ""

            While True

                Try
                    dsCriminal = InstinctFunctions.GetCriminalRecords

                    If Not dsCriminal Is Nothing _
                    AndAlso dsCriminal.Tables.Count > 0 _
                    AndAlso dsCriminal.Tables(0).Rows.Count > 0 Then
                        For Each drCriminal As DataRow In dsCriminal.Tables(0).Rows
                            strSerialNumber = ""
                            strAppKey = ""
                            strSerialNumber = drCriminal("Serial_Number").ToString.Trim
                            strAppKey = drCriminal("AppKey").ToString.Trim

                            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Start Calling Criminal Web Service (" & strAppKey & ")"))

                            If ReturnCriminal(drCriminal("Data").ToString.Trim) Then
                                ' Remove the record from the Application Actions table
                                InstinctFunctions.DeleteCriminal(strSerialNumber)
                            End If

                            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Finish Calling Criminal Web Service (" & strAppKey & ")"))
                            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage(" "))
                        Next
                    End If
                Catch ex As Exception
                    ' Write error to WIN2K system event log
                    InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Poll Criminal Extract error. AppKey=" & strAppKey & " Error Number = " & ex.Source & ".  Error Descripion: " & ex.Message & "."))
                    Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Poll Criminal Extract error. AppKey=" & strAppKey & " Error Number = " & ex.Source & ".  Error Descripion: " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
                Finally
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(INI_Pooling_Interval))
                End Try
            End While
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Poll_Criminal_Extract = " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Public Sub ConnectDatabase()
        Try
            If INI_Use_Windows_Authentication.ToUpper = "Y" Then
                strConnection = "database=" & INI_Initial_Catalog & ";server=" & INI_Data_Source & ";Integrated Security=SSPI" & ";Max Pool Size=1000;Connect Timeout=300" & INI_Application_Name
            Else
                strConnection = "database=" & INI_Initial_Catalog & ";server=" & INI_Data_Source & ";user id=" & INI_Database_User_Id & ";password=" & INI_Database_Password & ";Max Pool Size=1000;Connect Timeout=300" & INI_Application_Name
            End If
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "ConnectDatabase = " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Private Sub ConnectDatabaseUsingDefinedEncryptionKey()
        Dim objEncryption As Instinct.General.IEncryption = RemotingProxy.CreateProxy(GetType(Instinct.General.IEncryption))

        Try
            strConnection = objEncryption.GetConnectionString
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "ConnectDatabase = " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Private Function ReturnAction(ByVal sAppKey As String, ByVal sOnlineReturnRecord As String, ByVal sApplicationType As String, Optional ByVal bAuto As Boolean = False) As Boolean
        Dim webServiceResult As Object
        Dim i As Integer = 0
        Dim strURL As String
        Dim strClassName As String
        Dim strMethodName As String

        Dim strMqUser As String
        Dim strMqPassword As String
        Dim strMqHost As String
        Dim strMqVHost As String
        Dim iMqPort As Integer
        Dim strMqExchange As String
        Dim strMqQueue As String
        Dim strReplyTo As String

        Try
            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))

            If INI_ActonSkip_ApplicationTypes.Contains(sApplicationType) And bAuto Then 'if the action by auto ,and don't send the reuslt
                'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Current Application Type would Skip to Return Action Result."))
                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Service Return Result: SUCCESS"))
                Return True
            End If

            If INI_Return_Action_By_Application_Type = "Y" Then
                i = Application_Type.IndexOf(sApplicationType.Trim)

                If INI_Transfer_Way.ToUpper.Contains("MQ") Then
                    strMqUser = MqUser(i)
                    strMqPassword = MqPassword(i)
                    strMqHost = MqHost(i)
                    strMqVHost = MqVHost(i)
                    iMqPort = MqPort(i)
                    strMqExchange = MqExchange(i)
                    strMqQueue = MqQueue(i)
                    If strMqUser = "" Or strMqPassword = "" Or strMqHost = "" Or strMqQueue = "" Then
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("MQ INFO ""SOME CONFIGURATION INFORMATION IS EMPTY""[APPKEY=" & sAppKey & "]: MQUSER=" & strMqUser & ",MQPASSWORD=" & strMqPassword & ",MQHOST=" & strMqHost & ",MQQUEUE=" & strMqQueue))
                        Return False
                    End If

                    strReplyTo = InstinctFunctions.GetReplyTo(sAppKey)
                    strReplyTo = IIf(strReplyTo = "", strMqQueue, strReplyTo)

                    'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                    InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("MQ INFO[APPKEY=" & sAppKey & "]: REPLYTO=" & strReplyTo))

                    Dim errMsg As String = String.Empty

                    If InvokeMq(sOnlineReturnRecord, strMqHost, strMqVHost, strMqExchange, iMqPort, strReplyTo, strMqUser, strMqPassword, errMsg) Then
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Service Return Result: SUCCESS"))
                        Return True
                    Else
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Service Return Result: FAIL"))
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Error Message: " & errMsg))
                        Return False
                    End If

                End If

                strURL = URL(i)
                strClassName = Class_Name(i)
                strMethodName = Method_Name(i)

                If strURL = "" OrElse strClassName = "" OrElse strMethodName = "" Then
                    Return False
                End If

                If INI_Reply_Flag = "Y" Then

                    If String.Compare(INI_Site_With_Special_Functions, "CIMBID", True) = 0 Then
                        '****************************************************************************************************
                        '--Description: CIMBN request to use HTTPRequest and HTTPResponse to handle SOAP messages.
                        '--             Task 22070
                        '--Edit by : Yong Liu on 26/08/2014
                        '****************************************************************************************************

                        ' Need return result
                        webServiceResult = Me.InvokeHttpRequest(sOnlineReturnRecord, strURL)

                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Web Service Return Result: " & webServiceResult.ToString))

                        If webServiceResult.ToString.ToUpper.Contains("SUCCESS") Then
                            Return True
                        Else
                            Return False
                        End If



                    ElseIf String.Compare(INI_Site_With_Special_Functions, "MBBMY", True) = 0 Then
                        '****************************************************************************************************
                        '--Description: For MBBMY a option added to call REST interface to pass action string                       
                        '--Edit by : Sergey on 05/02/2015
                        '****************************************************************************************************

                        If INI_REST_Inteface = "Y" Then

                            If CallRESTInterface(sOnlineReturnRecord, strURL).Contains("SUCCESS") Then
                                Return True
                            Else
                                Return False
                            End If

                        Else
                            ' normal SOAP call
                            webServiceResult = WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(strURL, strClassName, strMethodName, New String() {sOnlineReturnRecord})

                            'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Web Service Return Result: " & webServiceResult.ToString))

                            If webServiceResult.ToString.ToUpper.Contains("SUCCESS") Then
                                Return True
                            Else
                                Return False
                            End If

                        End If



                    Else
                        ' Need return result
                        webServiceResult = WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(strURL, strClassName, strMethodName, New String() {sOnlineReturnRecord})

                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Web Service Return Result: " & webServiceResult.ToString))

                        If webServiceResult.ToString.ToUpper.Contains("SUCCESS") Then
                            Return True
                        Else
                            Return False
                        End If
                    End If

                Else
                    If String.Compare(INI_Site_With_Special_Functions, "CIMBID", True) = 0 Then
                        '****************************************************************************************************
                        '--Description: CIMBN request to use HTTPRequest and HTTPResponse to handle SOAP messages.
                        '--             Task 22070
                        '--Edit by : Yong Liu on 26/08/2014
                        '****************************************************************************************************

                        ' No need return result

                        webServiceResult = Me.InvokeHttpRequest(sOnlineReturnRecord, strURL)
                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))

                    ElseIf String.Compare(INI_Site_With_Special_Functions, "MBBMY", True) = 0 Then
                        '****************************************************************************************************
                        '--Description: For MBBMY a option added to call REST interface to pass action string                       
                        '--Edit by : Sergey on 05/02/2015
                        '****************************************************************************************************

                        If INI_REST_Inteface = "Y" Then
                            CallRESTInterface(sOnlineReturnRecord, strURL).Contains("SUCCESS")
                        Else
                            ' normal SOAP call
                            WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(strURL, strClassName, strMethodName, New String() {sOnlineReturnRecord})
                            'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                        End If

                    Else
                        ' No need return result
                        WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(strURL, strClassName, strMethodName, New String() {sOnlineReturnRecord})

                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                    End If

                    Return True
                End If
            Else

                If INI_Transfer_Way.ToUpper.Contains("MQ") Then

                    If INI_MqUser = "" Or INI_MqPassword = "" Or INI_MqHost = "" Or INI_MqQueue = "" Then
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("MQ INFO ""SOME CONFIGURATION INFORMATION IS EMPTY""[APPKEY=" & sAppKey & "]: MQUSER=" & INI_MqUser & ",MQPASSWORD=" & INI_MqPassword & ",MQHOST=" & INI_MqHost & ",MQQUEUE=" & INI_MqQueue))
                        Return False
                    End If

                    strReplyTo = InstinctFunctions.GetReplyTo(sAppKey)
                    strReplyTo = IIf(strReplyTo = "", INI_MqQueue, strReplyTo)

                    'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                    InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("MQ INFO[APPKEY=" & sAppKey & "]: REPLYTO=" & strReplyTo))

                    Dim errMsg As String = String.Empty

                    If InvokeMq(sOnlineReturnRecord, INI_MqHost, INI_MqVHost, INI_MqExchange, INI_MqPort, strReplyTo, INI_MqUser, INI_MqPassword, errMsg) Then
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Service Return Result: SUCCESS"))
                        Return True
                    Else
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Service Return Result: FAIL"))
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Error Message: " & errMsg))
                        Return False
                    End If

                End If

                If INI_Reply_Flag = "Y" Then

                    If String.Compare(INI_Site_With_Special_Functions, "CIMBID", True) = 0 Then

                        '****************************************************************************************************
                        '--Description: CIMBN request to use HTTPRequest and HTTPResponse to handle SOAP messages.
                        '--             Task 22070
                        '--Edit by : Yong Liu on 26/08/2014
                        '****************************************************************************************************

                        ' Need return result
                        webServiceResult = Me.InvokeHttpRequest(sOnlineReturnRecord, INI_URL)

                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Web Service Return Result: " & webServiceResult.ToString))

                        If webServiceResult.ToString.ToUpper.Contains("SUCCESS") Then
                            Return True
                        Else
                            Return False
                        End If


                    ElseIf String.Compare(INI_Site_With_Special_Functions, "MBBMY", True) = 0 Then
                        '****************************************************************************************************
                        '--Description: For MBBMY a option added to call REST interface to pass action string                       
                        '--Edit by : Sergey on 05/02/2015
                        '****************************************************************************************************

                        If INI_REST_Inteface = "Y" Then

                            If CallRESTInterface(sOnlineReturnRecord, INI_URL).Contains("SUCCESS") Then
                                Return True
                            Else
                                Return False
                            End If

                        Else
                            ' Need return result
                            webServiceResult = WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(INI_URL, INI_Class, INI_Method, New String() {sOnlineReturnRecord})

                            'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Web Service Return Result: " & webServiceResult.ToString))

                            If webServiceResult.ToString.ToUpper.Contains("SUCCESS") Then
                                Return True
                            Else
                                Return False
                            End If

                        End If


                    Else

                        ' Need return result
                        webServiceResult = WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(INI_URL, INI_Class, INI_Method, New String() {sOnlineReturnRecord})

                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                        InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Web Service Return Result: " & webServiceResult.ToString))

                        If webServiceResult.ToString.ToUpper.Contains("SUCCESS") Then
                            Return True
                        Else
                            Return False
                        End If
                    End If

                Else
                    If String.Compare(INI_Site_With_Special_Functions, "CIMBID", True) = 0 Then
                        '****************************************************************************************************
                        '--Description: CIMBN request to use HTTPRequest and HTTPResponse to handle SOAP messages.
                        '--             Task 22070
                        '--Edit by : Yong Liu on 26/08/2014
                        '****************************************************************************************************

                        ' No need return result

                        webServiceResult = Me.InvokeHttpRequest(sOnlineReturnRecord, INI_URL)
                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))


                    ElseIf String.Compare(INI_Site_With_Special_Functions, "MBBMY", True) = 0 Then
                        '****************************************************************************************************
                        '--Description: For MBBMY a option added to call REST interface to pass action string                       
                        '--Edit by : Sergey on 05/02/2015
                        '****************************************************************************************************

                        If INI_REST_Inteface = "Y" Then
                            CallRESTInterface(sOnlineReturnRecord, INI_URL).Contains("SUCCESS")
                        Else
                            ' normal SOAP call
                            WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(INI_URL, INI_Class, INI_Method, New String() {sOnlineReturnRecord})
                            'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                        End If



                    Else

                        ' No need return result
                        WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(INI_URL, INI_Class, INI_Method, New String() {sOnlineReturnRecord})

                        'InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sOnlineReturnRecord))
                    End If

                    Return True

                End If
            End If

        Catch ex As Exception
            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Calling web service error: " & ex.Message))
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Calling web service error: " & ex.Message, EventLogEntryType.Error, evtNumber, CatServices)
            Return False
        End Try
    End Function


    Private Function CallRESTInterface(ByVal recordToPass As String, ByVal uri As String) As String
        Dim req As HttpWebRequest = WebRequest.Create(uri)
        req.KeepAlive = False
        req.ContentType = "text/xml"
        req.Method = "POST"


        System.Net.ServicePointManager.ServerCertificateValidationCallback = Function() True

        Dim buffer As Byte() = System.Text.Encoding.UTF8.GetBytes(recordToPass)
        Dim PostData As Stream = req.GetRequestStream()
        PostData.Write(buffer, 0, buffer.Length)
        PostData.Close()


        Using response As HttpWebResponse = DirectCast(req.GetResponse(), HttpWebResponse)

            Dim responseValue As String = String.Empty

            'get the response
            Using responseStream As Stream = response.GetResponseStream()
                If (Not responseStream Is Nothing) Then

                    Using reader As StreamReader = New StreamReader(responseStream)
                        responseValue = reader.ReadToEnd()
                    End Using

                End If

            End Using

            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & recordToPass))
            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Action Web Service Return Result: " & responseValue))


            Return responseValue
        End Using

    End Function


    Private Function ReturnCriminal(ByVal sCriminalReturnRecord As String) As Boolean
        Dim webServiceResult As Object
        Dim i As Integer = 0

        Try
            If INI_Reply_Flag = "Y" Then

                ' Need return result
                webServiceResult = WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(INI_Criminal_URL, INI_Criminal_Class, INI_Criminal_Method, New String() {sCriminalReturnRecord})

                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sCriminalReturnRecord))
                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Criminal Web Service Return Result: " & webServiceResult.ToString))

                If webServiceResult.ToString.ToUpper.Contains("SUCCESS") Then
                    Return True
                Else
                    Return False
                End If

            Else

                ' No need return result
                WebServiceInvokeClass.clsWebServiceInvoke.InvokeWs(INI_Criminal_URL, INI_Criminal_Class, INI_Criminal_Method, New String() {sCriminalReturnRecord})

                InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Message: " & sCriminalReturnRecord))

                Return True

            End If

        Catch ex As Exception
            InstinctFunctions.WriteToInstinctLog(InstinctFunctions.AddDateTimeToMessage("Calling criminal web service error: " & ex.Message))
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Calling criminal web service error: " & ex.Message, EventLogEntryType.Error, evtNumber, CatServices)
            Return False
        End Try
    End Function

    Private Sub Retrieve_INI_Parameters()
        Try
            Dim sINIResult As String = String.Empty

            '*
            '* Retrieve INI Parameter
            '*
            INI_Use_Windows_Authentication = CStr(INIParameter.GetINIParameterValue("Startup", "Use Windows Authentication")).Trim
            If INI_Use_Windows_Authentication = "" Then
                INI_Use_Windows_Authentication = "Y"
            End If

            INI_Database_User_Id = CStr(INIParameter.GetINIParameterValue("Startup", "Database User Id")).Trim
            If INI_Database_User_Id = "" Then
                INI_Database_User_Id = "InstinctSysAdm"
            End If

            INI_Database_Password = CStr(INIParameter.GetINIParameterValue("Startup", "Database Password")).Trim
            If INI_Database_Password = "" Then
                INI_Database_Password = "Instinct"
            Else
                INI_Database_Password = Encrypt.AESDecryption(INI_Database_Password)
            End If

            INI_Data_Source = CStr(INIParameter.GetINIParameterValue("Startup", "Data Source")).Trim
            If INI_Data_Source = "" Then
                INI_Data_Source = "(local)"
            End If

            INI_Initial_Catalog = CStr(INIParameter.GetINIParameterValue("Startup", "Initial Catalog")).Trim
            If INI_Initial_Catalog = "" Then
                INI_Initial_Catalog = "Instinct"
            End If

            INI_Organisation = CStr(INIParameter.GetINIParameterValue("Startup", "Organisation")).Trim
            If Trim(INI_Organisation) <> "" Then
                INI_Organisation = Trim(INI_Organisation)
            End If

            INI_Default_Country = CStr(INIParameter.GetINIParameterValue("Startup", "Default Country")).Trim
            If Trim(INI_Default_Country) <> "" Then
                INI_Default_Country = Trim(INI_Default_Country)
            End If

            INI_Delimiter_Character = CStr(INIParameter.GetINIParameterValue("Startup", "Delimiter Character")).Trim
            If INI_Delimiter_Character = "" Then
                INI_Delimiter_Character = "|"
            End If

            INI_Value = CStr(INIParameter.GetINIParameterValue("Startup", "Pooling Interval"))
            If Trim(INI_Value) <> "" Then
                If IsNumeric(INI_Value) = True Then
                    INI_Pooling_Interval = CShort(INI_Value)
                Else
                    INI_Pooling_Interval = 2
                End If
            Else
                INI_Pooling_Interval = 2
            End If

            INI_Output_Directory = CStr(INIParameter.GetINIParameterValue("Startup", "Output Directory")).Trim
            If Trim(INI_Output_Directory) <> "" Then
                INI_Output_Directory = Trim(INI_Output_Directory)
            Else
                INI_Output_Directory = "C:"
            End If

            INI_Output_Format = CStr(INIParameter.GetINIParameterValue("Startup", "Output Format")).Trim
            If Trim(INI_Output_Format) = "" Then
                INI_Output_Format = "VARIABLE"
            Else
                If UCase(Trim(INI_Output_Format)) = "FIXED" _
                Or UCase(Trim(INI_Output_Format)) = "VARIABLE" Then
                    INI_Output_Format = UCase(Trim(INI_Output_Format))
                Else
                    INI_Output_Format = "VARIABLE"
                End If
            End If

            INI_User_Defined_Alert_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "User Defined Alert in Output File")).Trim
            If INI_User_Defined_Alert_in_Output_File = "" Then
                INI_User_Defined_Alert_in_Output_File = "N"
            Else
                If INI_User_Defined_Alert_in_Output_File.ToUpper = "Y" Then
                    INI_User_Defined_Alert_in_Output_File = "Y"
                Else
                    INI_User_Defined_Alert_in_Output_File = "N"
                End If
            End If

            INI_User_Id_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "User Id in Output File")).Trim
            If Trim(INI_User_Id_in_Output_File) = "" Then
                INI_User_Id_in_Output_File = "N"
            Else
                If UCase(Trim(INI_User_Id_in_Output_File)) = "Y" Then
                    INI_User_Id_in_Output_File = "Y"
                Else
                    INI_User_Id_in_Output_File = "N"
                End If
            End If

            INI_Rules_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "Rules in Output File")).Trim
            If Trim(INI_Rules_in_Output_File) = "" Then
                INI_Rules_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Rules_in_Output_File)) = "Y" Then
                    INI_Rules_in_Output_File = "Y"
                Else
                    INI_Rules_in_Output_File = "N"
                End If
            End If

            INI_Rules_Description_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "Rules Description in Output File")).Trim
            If Trim(INI_Rules_Description_in_Output_File) = "" Then
                INI_Rules_Description_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Rules_Description_in_Output_File)) = "Y" Then
                    INI_Rules_Description_in_Output_File = "Y"
                Else
                    INI_Rules_Description_in_Output_File = "N"
                End If
            End If

            INI_Action_Count_Number_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "Action Count Number in Output File")).Trim
            If Trim(INI_Action_Count_Number_in_Output_File) = "" Then
                INI_Action_Count_Number_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Action_Count_Number_in_Output_File)) = "Y" Then
                    INI_Action_Count_Number_in_Output_File = "Y"
                Else
                    INI_Action_Count_Number_in_Output_File = "N"
                End If
            End If

            INI_Nature_Of_Fraud_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "Nature Of Fraud in Output File")).Trim
            If Trim(INI_Nature_Of_Fraud_in_Output_File) = "" Then
                INI_Nature_Of_Fraud_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Nature_Of_Fraud_in_Output_File)) = "Y" Then
                    INI_Nature_Of_Fraud_in_Output_File = "Y"
                Else
                    INI_Nature_Of_Fraud_in_Output_File = "N"
                End If
            End If

            INI_Diary_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "Diary in Output File")).Trim
            If Trim(INI_Diary_in_Output_File) = "" Then
                INI_Diary_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Diary_in_Output_File)) = "Y" Then
                    INI_Diary_in_Output_File = "Y"
                Else
                    INI_Diary_in_Output_File = "N"
                End If
            End If

            INI_Site_With_Special_Functions = CStr(INIParameter.GetINIParameterValue("Startup", "Site With Special Functions")).Trim

            INI_Second_Service_Suffix = CStr(INIParameter.GetINIParameterValue("Startup", "Second Service Suffix")).Trim

            INI_Write_Log_File = CStr(INIParameter.GetINIParameterValue("Startup", "Write Log File")).Trim
            If INI_Write_Log_File <> "Y" Then
                INI_Write_Log_File = "N"
            End If

            INI_Decision_Reason_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "Decision Reason in Output File")).Trim
            If Trim(INI_Decision_Reason_in_Output_File) = "" Then
                INI_Decision_Reason_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Decision_Reason_in_Output_File)) = "Y" Then
                    INI_Decision_Reason_in_Output_File = "Y"
                Else
                    INI_Decision_Reason_in_Output_File = "N"
                End If
            End If

            INI_Group_Member_Code = CStr(INIParameter.GetINIParameterValue("Startup", "Group Member Code")).Trim

            INI_Output_Layout = CStr(INIParameter.GetINIParameterValue("Startup", "Output Layout")).Trim
            If INI_Output_Layout <> "JSON" And INI_Output_Layout <> "XML" Then
                INI_Output_Layout = "TXT"
            End If

            INI_Reply_Flag = CStr(INIParameter.GetINIParameterValue("Startup", "Reply Flag")).Trim
            If INI_Reply_Flag <> "Y" Then
                INI_Reply_Flag = "N"
            End If

            INI_Return_Action_By_Application_Type = CStr(INIParameter.GetINIParameterValue("Startup", "Return Action By Application Type")).Trim
            If INI_Reply_Flag <> "Y" Then
                INI_Reply_Flag = "N"
            End If

            INI_URL = CStr(INIParameter.GetINIParameterValue("Startup", "URL")).Trim
            If INI_URL = "" Then
                INI_URL = "http://localhost/Action%20Web%20Service%20Test%20Harness/InstinctAction.asmx"
            End If

            INI_Class = CStr(INIParameter.GetINIParameterValue("Startup", "Web Service Class Name")).Trim
            If INI_Class = "" Then
                INI_Class = "InstinctActionWebService"
            End If

            INI_Method = CStr(INIParameter.GetINIParameterValue("Startup", "Web Service Method Name")).Trim
            If INI_Method = "" Then
                INI_Method = "InstinctActionXMLStringReturn"
            End If

            ' For ING Turkey only
            INI_Criminal_URL = CStr(INIParameter.GetINIParameterValue("Criminal Interface", "URL")).Trim
            INI_Criminal_Class = CStr(INIParameter.GetINIParameterValue("Criminal Interface", "Web Service Class Name")).Trim
            INI_Criminal_Method = CStr(INIParameter.GetINIParameterValue("Criminal Interface", "Web Service Method Name")).Trim

            'FOR BQF
            INI_MqHost = CStr(INIParameter.GetINIParameterValue("Startup", "MqHost")).Trim
            INI_MqVHost = CStr(INIParameter.GetINIParameterValue("Startup", "MqvHost")).Trim
            INI_MqExchange = CStr(INIParameter.GetINIParameterValue("Startup", "MqExchange")).Trim
            INI_MqQueue = CStr(INIParameter.GetINIParameterValue("Startup", "MqQueue")).Trim
            If IsNumeric(CStr(INIParameter.GetINIParameterValue("Startup", "MqPort")).Trim) Then
                INI_MqPort = Convert.ToInt32(CStr(INIParameter.GetINIParameterValue("Startup", "MqPort")).Trim)
            Else
                INI_MqPort = 0
            End If
            INI_MqUser = CStr(INIParameter.GetINIParameterValue("Startup", "MqUserName")).Trim
            INI_MqPassword = CStr(INIParameter.GetINIParameterValue("Startup", "MqPassword")).Trim
            INI_MqResponseMethod = CStr(INIParameter.GetINIParameterValue("Startup", "MqResponseMethod")).Trim
            INI_MqEncoding = CStr(INIParameter.GetINIParameterValue("Startup", "MqEncoding")).Trim

            INI_Transfer_Way = CStr(INIParameter.GetINIParameterValue("Startup", "Transfer Way")).Trim

            INI_MqNeedDeclareQueue = IIf(CStr(INIParameter.GetINIParameterValue("Startup", "MqNeedDeclareQueue")).Trim.ToUpper() = "TRUE", True, False)

            INI_MqSpGetReplyTo = CStr(INIParameter.GetINIParameterValue("Startup", "MqSpGetReplyTo")).Trim

            ''Skip Action For Application Types Below£º
            'INI_ActonSkip_ApplicationTypes:
            INI_ActonSkip_ApplicationTypes = New ArrayList()
            Dim sINI_ActonSkip_ApplicationTypes As String = CStr(INIParameter.GetINIParameterValue("Startup", "INI_ActonSkip_ApplicationTypes")).Trim
            If sINI_ActonSkip_ApplicationTypes <> "" Then
                Dim arrValues As String() = Split(sINI_ActonSkip_ApplicationTypes, INI_Delimiter_Character, , CompareMethod.Text)
                INI_ActonSkip_ApplicationTypes.AddRange(arrValues)
            End If

            'FOR BAR-ES, BAR-PT, BAC-ES, BAC-PT
            INI_AuthorizerID = CStr(INIParameter.GetINIParameterValue("Startup", "AuthorizerID")).Trim

            '* -------------DecTech Code Modification Comment Start--------------
            '* 
            '* Description: add new field at the end for SPDB special function
            '*               
            '* Modified by : Hugh Hu
            '* Modified on : 9/12/2011
            '* Solution: wI 9170
            If INI_Site_With_Special_Functions = "SPDB" Then
                If IsNumeric(CStr(INIParameter.GetINIParameterValue("Startup", "Low Fraud Score")).Trim) = False Then
                    INI_Low_Fraud_Score = 0
                Else
                    INI_Low_Fraud_Score = CInt(CStr(INIParameter.GetINIParameterValue("Startup", "Low Fraud Score")).Trim)
                End If
            Else
                INI_Low_Fraud_Score = 0
            End If
            '* -------------DecTech Code Modification Comment End--------------

            '****************************************************************************************************
            '--Description: Adding Retry Delay parameter - Task 22704
            '--Edit by : Patrick - 03/10/2014
            '****************************************************************************************************
            INI_Retry_Delay = -1
            sINIResult = CStr(INIParameter.GetINIParameterValue("Startup", "Retry Delay")).Trim
            If sINIResult <> "" Then
                ' Attempt to convert the string to Int
                Try
                    INI_Retry_Delay = Convert.ToInt32(sINIResult)
                Catch e As OverflowException
                    INI_Retry_Delay = -1
                Catch e As FormatException
                    INI_Retry_Delay = -1
                End Try
            End If

            'Fraud Alert UserId in Output file
            INI_Fraud_Alert_UserId_in_Output_File = CStr(INIParameter.GetINIParameterValue("Startup", "Decision Reason in Output File")).Trim
            If INI_Site_With_Special_Functions.ToUpper = "SPDB" Then
                If Trim(INI_Fraud_Alert_UserId_in_Output_File) = "" Then
                    INI_Fraud_Alert_UserId_in_Output_File = "N"
                Else
                    If UCase(Trim(INI_Fraud_Alert_UserId_in_Output_File)) = "Y" Then
                        INI_Fraud_Alert_UserId_in_Output_File = "Y"
                    Else
                        INI_Fraud_Alert_UserId_in_Output_File = "N"
                    End If
                End If
            Else
                INI_Fraud_Alert_UserId_in_Output_File = "N"
            End If

            INI_Application_Name = CStr(INIParameter.GetINIParameterValue("Startup", "Application Name")).Trim
            If INI_Application_Name <> "" Then
                INI_Application_Name = ";Application Name=" & INI_Application_Name
            Else
                INI_Application_Name = ""
            End If


            INI_REST_Inteface = CStr(INIParameter.GetINIParameterValue("Startup", "REST Interface")).Trim


        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Retrieve_INI_Parameters. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Private Sub Verify_OrganisationAndDefaultCountry()
        Try
            If Trim(INI_Organisation) = "" Then
                Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up - Mandatory value not found on INI file (Organisation).", EventLogEntryType.Error, evtNumber, CatServices)
                End
            End If

            If Trim(INI_Default_Country) = "" Then
                Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up - Mandatory value not found on INI file (Country).", EventLogEntryType.Error, evtNumber, CatServices)
                End
            End If
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Verify_OrganisationAndDefaultCountry = " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Private Sub Retrieve_Web_Service_Info()
        Dim dsApplicationType As DataSet

        Try
            dsApplicationType = InstinctFunctions.GetApplicationTypes()
            If Not dsApplicationType Is Nothing _
            AndAlso dsApplicationType.Tables.Count > 0 Then
                For Each drApplicationType As DataRow In dsApplicationType.Tables(0).Rows
                    If Not Convert.IsDBNull(drApplicationType(1)) Then
                        Application_Type.Add(drApplicationType(1).ToString.Trim)
                        URL.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "URL")).Trim)
                        Class_Name.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "Web Service Class Name")).Trim)
                        Method_Name.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "Web Service Method Name")).Trim)
                    End If
                Next
            End If
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Retrieve_Web_Service_Info = " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Private Sub Retrieve_Mq_Service_Info()
        Dim dsApplicationType As DataSet

        Try
            dsApplicationType = InstinctFunctions.GetApplicationTypes()
            If Not dsApplicationType Is Nothing _
            AndAlso dsApplicationType.Tables.Count > 0 Then
                For Each drApplicationType As DataRow In dsApplicationType.Tables(0).Rows
                    If Not Convert.IsDBNull(drApplicationType(1)) Then
                        'Application_Type.Add(drApplicationType(1).ToString.Trim)
                        MqHost.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqHost")).Trim)
                        MqVHost.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqvHost")).Trim)
                        MqExchange.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqExchange")).Trim)
                        MqQueue.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqQueue")).Trim)
                        MqUser.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqUserName")).Trim)
                        MqPassword.Add(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqPassword")).Trim)
                        If CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqPort")).Trim = "" Then
                            MqPort.Add(0)
                        Else
                            MqPort.Add(CInt(CStr(INIParameter.GetINIParameterValue(drApplicationType(1).ToString.Trim, "MqPort")).Trim))
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry("InstinctServiceAction" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Retrieve_Web_Service_Info = " & ex.Message & ".", EventLogEntryType.Error, evtNumber, CatServices)
            End
        End Try
    End Sub

    Private Function Add_Header(ByVal sXML As String, ByVal sSerial_Number As String, ByVal strAuthorizerID As String) As String
        Dim strReturnXML As String
        Dim strCountryCode As String
        Dim strBusinessID As String
        Dim strChannel As String = "BRU"
        Dim strBranchCode As String = "3012"

        Try
            If INI_Site_With_Special_Functions.EndsWith("ES") Then
                strCountryCode = "ES"
                strBusinessID = "ESBRB"
            Else
                strCountryCode = "PT"
                strBusinessID = "PTBRB"
            End If

            strReturnXML = "<?xml version=""1.0"" encoding=""utf-8""?>"
            strReturnXML += "<UpdateCustomerAndProductMarkersRequest xmlns=""http://barclays.com/bem/UpdateCustomerAndProductMarkers"">"
            strReturnXML += "<RequestHeader><ConsumerContext p3:RESP_POPULATE_FLAG=""true"" xmlns:p3=""http://barclays.com/bem/BEMServiceHeader"" xmlns="""">"
            strReturnXML += "<CountryCode>" + strCountryCode + "</CountryCode>"
            strReturnXML += "<Channel>BRU</Channel>"
            strReturnXML += "<SystemID>INSTINCT</SystemID>"
            strReturnXML += "<BusinessID>" + strBusinessID + "</BusinessID>"
            strReturnXML += "<BranchCode>3012</BranchCode>"
            strReturnXML += "<TerminalID>NULL</TerminalID>"
            strReturnXML += "</ConsumerContext>"
            strReturnXML += "<ServiceContext xmlns="""">"
            strReturnXML += "<ServiceID>UpdateCustomerAndProductMarkers</ServiceID>"
            strReturnXML += "<ConsumerUniqueRefNo>" + sSerial_Number + "</ConsumerUniqueRefNo>"
            strReturnXML += "<ServiceDateTime>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm.ffffff") + "</ServiceDateTime>"
            strReturnXML += "<ServiceVersionNo>2.0.0</ServiceVersionNo>"
            strReturnXML += "</ServiceContext>"
            strReturnXML += "<BankUserContext p3:RESP_POPULATE_FLAG=""true"" xmlns:p3=""http://barclays.com/bem/BEMServiceHeader"" xmlns="""">"
            strReturnXML += "<AuthorizerID>" + strAuthorizerID + "</AuthorizerID>"
            strReturnXML += "</BankUserContext>"
            strReturnXML += "</RequestHeader>" + sXML.Substring(sXML.IndexOf("<OutputSchema>")) + "</UpdateCustomerAndProductMarkersRequest>"

            Return strReturnXML
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Private Function Add_Barclays_SOAP_Message(ByVal sXML As String, ByVal sSerial_Number As String, ByVal strAuthorizerID As String) As String
        Dim strReturnXML As String
        'Dim strProviderSystemCode As String
        'Dim strRqUID As String
        Dim xmlPathDoc As XPath.XPathDocument
        Dim xmlNav As XPath.XPathNavigator
        Dim xmlNodeDiaryNote As XPath.XPathNodeIterator

        Try
            xmlPathDoc = New XPath.XPathDocument(New StringReader(sXML))
            xmlNav = xmlPathDoc.CreateNavigator()
            xmlNodeDiaryNote = xmlNav.Select("/OutputSchema/Output/Diary")

            strReturnXML = "<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:upd=""http://barclays.com/bem/UpdateCustomerAndProductMarkers"" xmlns:bem=""http://barclays.com/bem/BEMServiceHeader"">"
            strReturnXML += "<soapenv:Header/>"
            strReturnXML += "<soapenv:Body>"
            strReturnXML += "<upd:UpdateCustomerAndProductMarkersRequest>"
            strReturnXML += Add_Header(sXML, sSerial_Number, strAuthorizerID)
            strReturnXML += "<upd:FraudInvestigationResult>"
            ' Body of the SOAP message

            If xmlNav.MoveToFollowing("Organisation", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:Organisation>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:Organisation>"
                Else
                    strReturnXML += "<upd:Organisation /> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Country_Code", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:CountryCode>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:CountryCode>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Group_Member_Code", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:GroupMemberCode>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:GroupMemberCode>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Application_Number", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:Application_Number>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:Application_Number>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Capture_Date", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:CaptureDate>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:CaptureDate>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Capture_Time", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:CaptureTime>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:CaptureTime>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Application_Type", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:ApplicationType>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:ApplicationType>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Fraud_Score", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:FraudScore>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:FraudScore>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Fraud_Alert", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:FraudAlert>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:FraudAlert>"
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Action_Taken", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:ActionTaken>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:ActionTaken>"
                End If
            End If

            xmlNav.MoveToRoot()
            If xmlNav.MoveToFollowing("Action_User", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:ActionUserId>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:ActionUserId>"
                End If
            End If

            For index As Integer = 1 To 20
                xmlNav.MoveToRoot()
                If xmlNav.MoveToFollowing("Rule_Triggered_" + index.ToString(), "") Then
                    If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                        strReturnXML += "<upd:RuleTriggered>"
                        strReturnXML += "<upd:Name>" + index.ToString() + "</upd:Name>"
                        strReturnXML += "<upd:Value>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:Value>"
                        xmlNav.MoveToRoot()
                        If xmlNav.MoveToFollowing("Description_Rule_Triggered_" + index.ToString(), "") Then
                            If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                                strReturnXML += "<upd:Description>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:Description>"
                            End If
                        End If
                        strReturnXML += "</upd:RuleTriggered>"
                    End If
                End If
            Next

            xmlNav.MoveToRoot()
            If xmlNav.MoveToFollowing("Decision_Reason", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<upd:DecisionReason>" + Format_XML_Value(xmlNav.Value.ToString()) + "</upd:DecisionReason>"
                End If
            End If

            ' Diary Notes

            xmlNav.MoveToRoot()

            Dim xmlNodeDiaryNoteCount As Integer = xmlNodeDiaryNote.Count

            If xmlNodeDiaryNoteCount > 0 Then

                For Each xDiarNote As XPath.XPathNavigator In xmlNodeDiaryNote

                    If xmlNodeDiaryNote.CurrentPosition = xmlNodeDiaryNoteCount - 1 Then
                        strReturnXML += "<upd:Diary> "
                        If String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_Note").Value.ToString) = False Then
                            strReturnXML += "<upd:Note>" + Format_XML_Value(xDiarNote.SelectSingleNode("Diary_Note").Value.ToString) + "</upd:Note> "
                        End If

                        If String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_User_Id").Value.ToString) = False Then
                            strReturnXML += "<upd:CreatedById>" + Format_XML_Value(xDiarNote.SelectSingleNode("Diary_User_Id").Value.ToString) + "</upd:CreatedById> "
                        End If

                        If String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_Date").Value.ToString) = False AndAlso String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_Time").Value.ToString) = False Then
                            strReturnXML += "<upd:CreateDateTime>" + Format_XML_Value(Convert.ToDateTime(xDiarNote.SelectSingleNode("Diary_Date").Value.ToString + " " + ConvertToTimeFormat(xDiarNote.SelectSingleNode("Diary_Time").Value.ToString)).ToString("yyyy-MM-ddTHH:mm:ss.ffffff")) + "</upd:CreateDateTime> "

                        End If
                        strReturnXML += "</upd:Diary> "
                    End If

                    xmlNodeDiaryNote.MoveNext()
                Next
            End If

            strReturnXML += "</upd:FraudInvestigationResult>"
            strReturnXML += "</upd:UpdateCustomerAndProductMarkersRequest>"

            strReturnXML += "</soapenv:Body>"
            strReturnXML += "</soapenv:Envelope>"

            xmlNav.MoveToRoot()

            Return strReturnXML

        Catch ex As Exception
            Return ex.Message

        End Try

    End Function

    Private Function Format_XML_Value(ByVal inputStr As String) As String
        Return System.Security.SecurityElement.Escape(inputStr)
    End Function

    Private Function Add_CIMBID_SOAP_Message(ByVal sXML As String, ByVal sApplication_Type As String) As String
        Dim strReturnXML As String
        Dim strProviderSystemCode As String
        Dim strRqUID As String
        Dim xmlPathDoc As XPath.XPathDocument
        Dim xmlNav As XPath.XPathNavigator
        Dim xmlNodeDiaryNote As XPath.XPathNodeIterator

        Try
            xmlPathDoc = New XPath.XPathDocument(New StringReader(sXML))
            xmlNav = xmlPathDoc.CreateNavigator()
            xmlNodeDiaryNote = xmlNav.Select("/OutputSchema/Output/Diary")

            ' Set CIMB Provider System code according to the application type
            If String.Compare(sApplication_Type, "AUDR", True) = 0 Then
                strProviderSystemCode = "RCS_ID"
            ElseIf String.Compare(sApplication_Type, "AUIN", True) = 0 Then
                strProviderSystemCode = "RCS_ID"
            ElseIf String.Compare(sApplication_Type, "CARD", True) = 0 Then
                strProviderSystemCode = "SPEKTACC_ID"
            ElseIf String.Compare(sApplication_Type, "MTGE", True) = 0 Then
                strProviderSystemCode = "RCS_ID"
            ElseIf String.Compare(sApplication_Type, "PL", True) = 0 Then
                strProviderSystemCode = "SPEKTAPL_ID"
            Else
                strProviderSystemCode = String.Empty
            End If

            ' Generate random number for RqUID in the format of "00000000-0000-0000-0000-000000000019"
            Dim rndGenerator As System.Random = New System.Random()
            strRqUID = String.Empty
            strRqUID += rndGenerator.Next(1, 99999999).ToString.PadLeft(8, "0") + "-"
            strRqUID += rndGenerator.Next(1, 9999).ToString.PadLeft(4, "0") + "-"
            strRqUID += rndGenerator.Next(1, 9999).ToString.PadLeft(4, "0") + "-"
            strRqUID += rndGenerator.Next(1, 9999).ToString.PadLeft(4, "0") + "-"
            strRqUID += rndGenerator.Next(1, 999999).ToString.PadLeft(6, "0")
            strRqUID += rndGenerator.Next(1, 999999).ToString.PadLeft(6, "0")

            strReturnXML = "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""> "
            strReturnXML += "<SOAP-ENV:Body> "
            strReturnXML += "<esb:CIMB_FraudActionTakenOpr xmlns:esb=""urn:ifxforum-org:XSD:1"" coreSchemaVersion=""1.7""> "
            strReturnXML += "<esb:CIMB_SignonRq> "
            strReturnXML += "<esb:ClientDt>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffff") + "</esb:ClientDt> "
            strReturnXML += "<esb:CustLangPref>en</esb:CustLangPref> "
            strReturnXML += "<esb:ClientApp> "
            strReturnXML += "<esb:Org>cimb.id.cimbniaga</esb:Org> "
            strReturnXML += "<esb:Name>CIMB Niaga</esb:Name> "
            strReturnXML += "<esb:Version>1.0</esb:Version> "
            strReturnXML += "</esb:ClientApp> "
            strReturnXML += "<esb:CIMB_HeaderRq> "
            strReturnXML += "<esb:CIMB_ConsumerId>" + "FDS_CIMB" + "</esb:CIMB_ConsumerId> "
            strReturnXML += "<esb:CIMB_ConsumerPswd>" + "password" + "</esb:CIMB_ConsumerPswd> "
            strReturnXML += "<esb:CIMB_ServiceName>FRAUDACTIONTAKEN</esb:CIMB_ServiceName> "
            strReturnXML += "<esb:CIMB_ServiceVersion>1.0</esb:CIMB_ServiceVersion> "
            strReturnXML += "<esb:CIMB_SrcSystem>" + "FDS_ID" + "</esb:CIMB_SrcSystem> "
            strReturnXML += "<esb:CIMB_ProviderList> "
            strReturnXML += "<esb:CIMB_Provider> "

            If String.IsNullOrEmpty(strProviderSystemCode) = False Then
                strReturnXML += "<esb:CIMB_ProviderSystemCode>" + strProviderSystemCode + "</esb:CIMB_ProviderSystemCode> "
            Else
                strReturnXML += "<esb:CIMB_ProviderSystemCode /> "
            End If

            strReturnXML += "<esb:CIMB_ProviderAuthDetail> "
            strReturnXML += "<esb:CIMB_ProviderUserId/> "
            strReturnXML += "<esb:CIMB_ProviderUserPaswd/> "
            strReturnXML += "</esb:CIMB_ProviderAuthDetail> "
            strReturnXML += "</esb:CIMB_Provider> "
            strReturnXML += "</esb:CIMB_ProviderList> "
            strReturnXML += "<esb:CIMB_SrcCountryCode>" + "IDN" + "</esb:CIMB_SrcCountryCode> "
            strReturnXML += "</esb:CIMB_HeaderRq> "
            strReturnXML += "</esb:CIMB_SignonRq> "

            ' Body of the SOAP message
            strReturnXML += "<esb:CIMB_FraudActionTakenRq> "
            strReturnXML += "<esb:RqUID>" + strRqUID + "</esb:RqUID> "
            strReturnXML += "<esb:CIMB_FraudCheckOutput> "
            strReturnXML += "<esb:CIMB_FraudHeaderInfo> "
            strReturnXML += "<esb:ClientApp> "
            strReturnXML += "<esb:Org>CMB</esb:Org> "
            strReturnXML += "<esb:Name/> "
            strReturnXML += "<esb:Version/> "
            strReturnXML += "</esb:ClientApp> "
            strReturnXML += "<esb:CIMB_CountryCode>IDN</esb:CIMB_CountryCode> "
            strReturnXML += "<esb:CIMB_GroupMemberCode/> "
            strReturnXML += "</esb:CIMB_FraudHeaderInfo> "

            ' ApplicationInfo section
            strReturnXML += "<esb:CIMB_AppInfo> "

            If xmlNav.MoveToFollowing("Application_Number", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_AppNumber>" + xmlNav.Value.ToString() + "</esb:CIMB_AppNumber> "
                Else
                    strReturnXML += "<esb:CIMB_AppNumber /> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Capture_Date", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_CaptureDt>" + Convert.ToDateTime(xmlNav.Value.ToString()).ToString("yyyy-MM-dd") _
                         + "</esb:CIMB_CaptureDt> "
                Else
                    strReturnXML += "<esb:CIMB_CaptureDt>0000-00-00</esb:CIMB_CaptureDt> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Capture_Time", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_CaptureTm>" + ConvertToTimeFormat(xmlNav.Value.ToString) _
                        + "</esb:CIMB_CaptureTm> "
                Else
                    strReturnXML += "<esb:CIMB_CaptureTm>00:00:00</esb:CIMB_CaptureTm> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Application_Type", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_AppType>" + xmlNav.Value.ToString() + "</esb:CIMB_AppType> "
                Else
                    strReturnXML += "<esb:CIMB_AppType /> "
                End If
            End If

            xmlNav.MoveToRoot()

            strReturnXML += "<esb:CIMB_AppDecision> "

            If xmlNav.MoveToFollowing("Decision_Reason", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_DecisionReason>" + xmlNav.Value.ToString() + "</esb:CIMB_DecisionReason> "
                Else
                    strReturnXML += "<esb:CIMB_DecisionReason /> "
                End If
            End If

            xmlNav.MoveToRoot()

            strReturnXML += "</esb:CIMB_AppDecision> "
            strReturnXML += "</esb:CIMB_AppInfo> "

            If xmlNav.MoveToFollowing("Fraud_Score", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_FraudScore>" + xmlNav.Value.ToString() + "</esb:CIMB_FraudScore> "
                Else
                    strReturnXML += "<esb:CIMB_FraudScore /> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Fraud_Alert", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_FraudAlert>" + xmlNav.Value.ToString() + "</esb:CIMB_FraudAlert> "
                Else
                    strReturnXML += "<esb:CIMB_FraudAlert /> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Action_Taken", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ActionTaken>" + xmlNav.Value.ToString() + "</esb:CIMB_ActionTaken> "
                Else
                    strReturnXML += "<esb:CIMB_ActionTaken /> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Error_Code", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ErrorCode>" + xmlNav.Value.ToString() + "</esb:CIMB_ErrorCode> "
                Else
                    strReturnXML += "<esb:CIMB_ErrorCode /> "
                End If
            End If

            xmlNav.MoveToRoot()

            strReturnXML += "<esb:CIMB_UserDefinedAlert /> "

            If xmlNav.MoveToFollowing("Action_User", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ActionUser>" + xmlNav.Value.ToString() + "</esb:CIMB_ActionUser> "
                Else
                    strReturnXML += "<esb:CIMB_ActionUser /> "
                End If
            End If

            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("CountNbr", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_CountNbr>" + xmlNav.Value.ToString() + "</esb:CIMB_CountNbr> "
                Else
                    strReturnXML += "<esb:CIMB_CountNbr /> "
                End If
            End If

            xmlNav.MoveToRoot()


            ' Nature of Fraud
            If xmlNav.MoveToFollowing("FraudCode", "") Then
                strReturnXML += "<esb:CIMB_NatureOfFraud> "

                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_FraudCode>" + xmlNav.Value.ToString() + "</esb:CIMB_FraudCode> "
                Else
                    strReturnXML += "<esb:CIMB_FraudCode /> "
                End If

                strReturnXML += "</esb:CIMB_NatureOfFraud> "

            End If

            ' Diary Notes

            xmlNav.MoveToRoot()

            If xmlNodeDiaryNote.Count > 0 Then

                For Each xDiarNote As XPath.XPathNavigator In xmlNodeDiaryNote

                    strReturnXML += "<esb:CIMB_Diary> "

                    If String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_Date").Value.ToString) = False Then
                        strReturnXML += "<esb:CIMB_DiaryDt>" + Convert.ToDateTime(xDiarNote.SelectSingleNode("Diary_Date").Value.ToString).ToString("yyyy-MM-dd") _
                                + "</esb:CIMB_DiaryDt> "
                    Else
                        strReturnXML += "<esb:CIMB_DiaryDt>0000-00-00</esb:CIMB_DiaryDt>"
                    End If

                    If String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_Time").Value.ToString) = False Then
                        strReturnXML += "<esb:CIMB_DiaryTm>" + ConvertToTimeFormat(xDiarNote.SelectSingleNode("Diary_Time").Value.ToString) _
                        + "</esb:CIMB_DiaryTm> "
                    Else
                        strReturnXML += "<esb:CIMB_DiaryTm>00:00:00</esb:CIMB_DiaryTm>"
                    End If

                    If String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_User_Id").Value.ToString) = False Then
                        strReturnXML += "<esb:CIMB_DiaryUserId>" + xDiarNote.SelectSingleNode("Diary_User_Id").Value.ToString + "</esb:CIMB_DiaryUserId> "
                    Else
                        strReturnXML += "<esb:CIMB_DiaryUserId />"
                    End If

                    If String.IsNullOrEmpty(xDiarNote.SelectSingleNode("Diary_Note").Value.ToString) = False Then
                        strReturnXML += "<esb:CIMB_DiaryNote>" + xDiarNote.SelectSingleNode("Diary_Note").Value.ToString + "</esb:CIMB_DiaryNote> "
                    Else
                        strReturnXML += "<esb:CIMB_DiaryNote />"
                    End If

                    strReturnXML += "</esb:CIMB_Diary> "
                Next

            End If

            xmlNav.MoveToRoot()

            ' Rules triggered
            If xmlNav.MoveToFollowing("Rule_Triggered_1", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode1"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode1"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_2", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode2"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode2"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_3", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode3"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode3"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_4", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode4"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode4"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_5", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode5"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode5"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_6", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode6"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode6"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_7", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode7"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode7"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_8", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode8"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode8"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_9", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode9"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode9"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_10", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode10"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode10"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_11", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode11"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode11"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_12", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode12"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode12"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_13", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode13"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode13"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_14", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode14"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode14"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_15", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode15"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode15"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_16", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode16"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode16"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_17", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode17"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode17"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_18", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode18"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode18"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_19", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode19"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode19"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Rule_Triggered_20", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode20"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_RuleTriggCode20"" /> "
                End If
            End If

            ' Rule Triggered descriptions
            xmlNav.MoveToRoot()

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_1", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg1"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg1"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_2", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg2"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg2"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_3", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg3"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg3"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_4", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg4"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg4"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_5", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg5"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg5"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_6", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg6"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg6"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_7", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg7"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg7"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_8", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg8"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg8"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_9", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg9"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg9"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_10", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg10"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg10"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_11", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg11"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg11"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_12", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg12"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg12"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_13", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg13"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg13"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_14", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg14"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg14"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_15", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg15"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg15"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_16", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg16"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg16"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_17", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg17"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg17"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_18", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg18"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg18"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_19", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg19"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg19"" /> "
                End If
            End If

            If xmlNav.MoveToFollowing("Description_Rule_Triggered_20", "") Then
                If String.IsNullOrEmpty(xmlNav.Value.ToString()) = False Then
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg20"">" + xmlNav.Value.ToString() + "</esb:CIMB_ExtnInfo> "
                Else
                    strReturnXML += "<esb:CIMB_ExtnInfo FieldName=""CIMB_DescRuleTrigg20"" /> "
                End If
            End If

            strReturnXML += "</esb:CIMB_FraudCheckOutput> "
            strReturnXML += "</esb:CIMB_FraudActionTakenRq> "
            strReturnXML += "</esb:CIMB_FraudActionTakenOpr> "
            strReturnXML += "</SOAP-ENV:Body> "
            strReturnXML += "</SOAP-ENV:Envelope> "

            xmlNav.MoveToRoot()

            Return strReturnXML

        Catch ex As Exception
            Return ex.Message

        End Try

    End Function

    Private Function InvokeHttpRequest(ByVal sOnlineReturnRecord As String, ByVal sUrl As String) As String

        Try
            Dim webRequest As HttpWebRequest = CType(Net.WebRequest.Create(sUrl), HttpWebRequest)
            Dim strReponse As String = String.Empty

            If webRequest IsNot Nothing Then
                webRequest.Method = "POST"
                webRequest.ContentType = "text/xml; charset=utf-8"
                webRequest.Headers.Add("SOAPAction: http://tempuri.org/")
                webRequest.ProtocolVersion = HttpVersion.Version11
                webRequest.Credentials = CredentialCache.DefaultCredentials

                ' Put in the request data
                Dim streamRequest As Stream = webRequest.GetRequestStream()
                Dim sw As StreamWriter = New StreamWriter(streamRequest)
                sw.Write(sOnlineReturnRecord)
                sw.Close()
                streamRequest.Close()

                ' Get the reponse data
                Dim streamResponse As Stream = webRequest.GetResponse().GetResponseStream()
                Dim sr As StreamReader = New StreamReader(streamResponse)
                ' Dim strStatusDesc As String = CType(webRequest.GetResponse(), HttpWebResponse).StatusDescription
                strReponse = sr.ReadToEnd()
                sr.Close()

                streamRequest.Close()
                streamResponse.Close()

            End If

            Return strReponse

        Catch ex As Exception

            Return ex.Message

        End Try

    End Function

    Private Function InvokeMq(ByVal sOnlineReturnRecord As String, ByVal host As String, ByVal vhost As String, ByVal exchange As String, ByVal port As Integer, ByVal queue As String, ByVal user As String, ByVal password As String, ByRef errMsg As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            Dim client As New SCM.RabbitMQClient.RabbitMqClientJSONSimple(host, vhost, queue, port, user, password, INI_MqEncoding, INI_MqNeedDeclareQueue)

            Dim encoder As System.Text.Encoding = System.Text.Encoding.GetEncoding(INI_MqEncoding)

            Dim sMarkCode As String = Guid.NewGuid().ToString()

            Dim sendMessage As SCM.RabbitMQClient.EventMessage = SCM.RabbitMQClient.EventMessageFactory.CreateEventMessageInstance(sOnlineReturnRecord, sMarkCode, encoder, INI_MqResponseMethod)

            client.TriggerEventMessage(sendMessage, exchange, queue)

        Catch ex As Exception
            bReturn = False
            errMsg = ex.ToString
        End Try
        Return bReturn
    End Function

    ' Convert the time format (hhmmss) back to (hh:mm:ss) 
    Private Function ConvertToTimeFormat(ByVal sTime As String) As String

        Try
            Dim sFormattedTime As String = String.Empty
            Dim i As Integer = 0

            If String.IsNullOrEmpty(sTime) Then
                Return sTime
            End If

            ' Already in correct format, no need to format it again
            If (sTime.IndexOf(":"c) <> -1) Then
                Return sTime
            End If

            sTime = sTime.Trim

            ' Must only contain 6 digits (hhmmss) and must be a number
            If (sTime.Length = 6 AndAlso IsNumeric(sTime)) Then

                For i = 0 To (sTime.Length - 1) Step 2
                    sFormattedTime = sFormattedTime + sTime.Substring(i, 2) + ":"
                Next

                ' Remove the last ":" from the string
                sFormattedTime = sFormattedTime.Remove(sFormattedTime.Length - 1)

            Else
                Return sTime
            End If

            Return sFormattedTime

        Catch ex As Exception
            Return ex.Message
        End Try


    End Function

    Private Sub RetryDelayListSafeInsert(ByVal sIncomingSerialNo As String)
        ' See if this record is already in the list
        For Each currentRecord As RetryDelayRecord In RetryDelayList
            If currentRecord.SerialNo = sIncomingSerialNo Then
                ' Update the delay time for this item
                currentRecord.RetryDateTime = Date.Now().AddSeconds(INI_Retry_Delay)
                Exit Sub
            End If
        Next

        ' This is a new record for the list
        RetryDelayList.Add(New RetryDelayRecord(sIncomingSerialNo, DateTime.Now.AddSeconds(INI_Retry_Delay)))
    End Sub

End Class

