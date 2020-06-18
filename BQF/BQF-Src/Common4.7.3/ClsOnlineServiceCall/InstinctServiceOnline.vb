Imports System.IO
Imports System.Text
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Threading
Imports Microsoft.ApplicationBlocks.Data
Imports DecTech.Library
Imports Instinct.XmlConversion
Imports DecTech.Security.LicenceKey

Public Class InstinctServiceOnline

    Private strInput As String
    Private TargetDatabase As String
    Private OnlineLoadRecord As String
    Private Load_Mode As String

    Shared objFraudCheck As Object = New Object
    Private objApplicationHasBeenLoadedAndChecked As Boolean
    Private oldCi As System.Globalization.CultureInfo

    Public TriggeredRulesDefinitions As DataSet


    'Instinct INI Parameters
    Private INI_Value As String
    Private INI_Use_Windows_Authentication As String
    Private INI_Use_Defined_Encryption_Key As String
    Private INI_Key1_Path As String
    Private INI_Key2_Path As String
    Private INI_Database_User_Id As String
    Private INI_Database_Password As String
    Private INI_Data_Source As String
    Private INI_Initial_Catalog As String
    Private INI_Organisation As String
    Private INI_Default_Country As String
    Private INI_Delimiter_Character As String
    Private INI_Pooling_Interval As Short
    Private INI_Port_Number As Short
    Private INI_Output_Directory As String
    Private INI_Input_Format As String
    Private INI_Output_Format As String
    Private INI_Output_File_Deletion_Period As Integer
    Private INI_Fraud_Check_Output_File_Flag As String
    Private INI_Action_Output_File_Flag As String
    Private INI_Local_Time_Difference As Long
    Private INI_Criminal_File_Type As String
    Private INI_Criminal_File_With_Column_Row As String
    Private INI_User_Defined_Alert_in_Output_File As String
    Private INI_User_Id_in_Output_File As String
    Private INI_Rules_in_Output_File As String
    Private INI_Action_Count_Number_in_Output_File As String
    Private INI_Nature_Of_Fraud_in_Output_File As String
    Private INI_Rules_Description_in_Output_File As String
    Private INI_Diary_in_Output_File As String
    Private INI_Site_With_Special_Functions As String
    Private INI_Second_Service_Suffix As String
    Private INI_Write_Log_File As String
    Private INI_Decision_Reason_in_Output_File As String
    Private INI_Group_Member_Code As String
    Private INI_Fraud_Alert_UserId_in_Output_File As String
    Private INI_ApplicationName As String

    Private TOTAL_RULE_TRIGGERED As Integer = 20

    Public Delegate Sub dlgWriteIISLong(ByVal message As String)
    Public WriteIISLong As dlgWriteIISLong

    Public Sub New(ByVal inputStr As String)
        OnlineCall_TCPIP(inputStr)
    End Sub

    'Rumesh MM TFS16582
    Public Sub New(ByVal inputStr As String, ByVal dlgWL As dlgWriteIISLong)
        WriteIISLong = dlgWL
        OnlineCall_TCPIP(inputStr)
    End Sub

    Public ReadOnly Property GetOutputStr() As String
        Get
            Return strInput
        End Get
    End Property

    Private Sub OnlineCall_TCPIP(ByVal inputString As String)
        Dim index As Integer
        Dim errStr As String
        Dim objOnlineConversion As OnlineConversion_MultipleThread
        Dim strXML As String
        Dim LogString As New System.Text.StringBuilder
        Dim OnlineLogPath As String
        Dim OnlineReturnRecord As String
        Dim strAppKey As String
        Dim sSql As String
        Dim strUserDefinedAlert As String = ""
        Dim strDecisionReason As String = ""
        Dim strFraudAlertUserId As String = ""
        Dim strActionTaken As String = ""
        Dim dsApplication As DataSet
        Dim dsMatchedAppKeys As DataSet
        Dim strLowFraudScoreFlag As String
        Dim AddFraudCheck As Boolean = False

        Try

            errStr = IniFraudCheck()
            If errStr <> "" Then
                strInput = errStr
                If INI_Site_With_Special_Functions.Trim.ToUpper = "HKBMX" Then
                    strInput = strInput & InstinctFunctions.AddTimeStampExitStatusToOutput("FALSE", INI_Delimiter_Character, False)
                End If
                Exit Sub
            End If

            errStr = ""
            errStr = ValidateLicence()
            If errStr <> "" Then
                strInput = errStr
                If INI_Site_With_Special_Functions.Trim.ToUpper = "HKBMX" Then
                    strInput = strInput & InstinctFunctions.AddTimeStampExitStatusToOutput("FALSE", INI_Delimiter_Character, False)
                End If
                Exit Sub
            End If

            If INI_Site_With_Special_Functions.ToUpper = "SPDB" Then
                TOTAL_RULE_TRIGGERED = 75
            End If

            objOnlineConversion = New OnlineConversion_MultipleThread(connstr, INI_Organisation, INI_Default_Country, INI_Delimiter_Character, TargetDatabase, bolWriteLog, INI_Site_With_Special_Functions, INI_Local_Time_Difference)
            'Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix , "OnlineCall TCPIP. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".", EventLogEntryType.Error, 1, 4)

            OnlineLoadRecord = inputString

            If OnlineLoadRecord.Trim = "" Then
                strInput = "||||||||||12|"

                If INI_User_Defined_Alert_in_Output_File.Trim.ToUpper = "Y" Then
                    strInput += "|"
                End If

                If INI_User_Id_in_Output_File.Trim.ToUpper = "Y" Then
                    strInput += "|"
                End If

                If INI_Rules_in_Output_File.Trim.ToUpper = "Y" Then
                    For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                        strInput += "|"
                    Next
                    'strInput += "||||||||||||||||||||"
                End If

                If INI_Site_With_Special_Functions.ToUpper.Trim = "HSBCTURKEY" Then
                    strInput += "||"
                End If

                If INI_Decision_Reason_in_Output_File.Trim.ToUpper = "Y" Then
                    strInput += "|"
                End If

                If INI_Action_Count_Number_in_Output_File.Trim.ToUpper = "Y" Then
                    strInput += "|"
                End If

                If INI_Rules_Description_in_Output_File.Trim.ToUpper = "Y" Then
                    For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                        strInput += "|"
                    Next
                    'strInput += "||||||||||||||||||||"
                End If

                If INI_Site_With_Special_Functions.Trim.ToUpper = "SPDB" AndAlso INI_Fraud_Alert_UserId = "Y" Then
                    strInput += "|"
                End If

                If INI_Site_With_Special_Functions.Trim.ToUpper = "SPDB" Then
                    For i As Integer = 1 To 25 + 50 + 10 * 2 '25 Normal Rules + 50 Audit Rules + 10 Phone Records * 2
                        strInput += "|"
                    Next
                End If
                If INI_Site_With_Special_Functions.Trim.ToUpper = "HKBMX" Then
                    strInput = strInput & InstinctFunctions.AddTimeStampExitStatusToOutput("FALSE", INI_Delimiter_Character, False)
                End If
                Exit Sub
            End If

            'Convert to Uppercase if necessary
            If SVR_Store_Instinct_Data_In_Uppercase.Trim.ToUpper = "ON" Then
                OnlineLoadRecord = OnlineLoadRecord.ToUpper
            End If

            'Removing the multiple spaces into single space
            Do While InStr(1, OnlineLoadRecord, "  ")
                OnlineLoadRecord = Replace(OnlineLoadRecord, "  ", " ")
            Loop

            Load_Mode = "O"

            ' HLB has Country Code and Group Member Code hardcoded
            If INI_Site_With_Special_Functions = "HLB" Then
                OnlineLoadRecord = "HLB" & INI_Delimiter_Character & "MY" & INI_Delimiter_Character & "HLBB" & OnlineLoadRecord.Trim.Substring(3)
            End If

            Try
                strXML = objOnlineConversion.ConvertToXML(OnlineLoadRecord, INI_Site_With_Special_Functions, bolMillisecondsInLogFile)
                'Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix , "OnlineCall TCPIP. XML = " & strXML, EventLogEntryType.Error, 1, 4)
            Catch ex As Exception
                If INI_Write_Log_File = "Y" Then

                    LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage("*************************************"))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage("*** Processing Online Application ***"))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage("*************************************"))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage("Online record follows -"))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage(OnlineLoadRecord))
                    LogString.Append(objOnlineConversion.GetLoadingLog & vbCrLf)
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage(errStr))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage("99" & INI_Delimiter_Character & ex.Message))

                End If

                SyncLock objFraudCheck
                    Call InstinctFunctions.WriteToInstinctLog(LogString.ToString, True)
                End SyncLock

                strInput = "99" & INI_Delimiter_Character & ex.Message
                If INI_Site_With_Special_Functions.Trim.ToUpper = "HKBMX" Then
                    strInput = strInput & InstinctFunctions.AddTimeStampExitStatusToOutput("FALSE", INI_Delimiter_Character, False)
                End If
                Exit Sub
            End Try

            If INI_Write_Log_File = "Y" Then

                LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage("*************************************"))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage("*** Processing Online Application ***"))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage("*************************************"))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage("Online record follows -"))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage(OnlineLoadRecord))
                LogString.Append(objOnlineConversion.GetLoadingLog & vbCrLf)

            End If

            Try
                objApplicationHasBeenLoadedAndChecked = objOnlineConversion.LoadAndFraudCheck(strXML, "O", bolSkipFraudCheckForUI, _
                                         bolSkipFraudCheckForKF, bolSkipFraudCheckForS, False, INI_Group_Member_Code, _
                                         bolWriteLog, bolUserDefinedAlertinOutput, bolUserIdinOutput, bolRulesInOutput, _
                                         bolDecisionReasonInOutput, INI_Site_With_Special_Functions, IIf(INI_Action_Output_File_Flag.Trim.ToUpper = "Y", 1, 0), True, -1, AddFraudCheck)


            Catch ex1 As Exception

                If INI_Write_Log_File = "Y" Then

                    LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage("Online response follows -"))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage(errStr))
                    LogString.Append(InstinctFunctions.AddDateTimeToMessage("99" & INI_Delimiter_Character & ex1.Message))

                End If

                SyncLock objFraudCheck
                    Call InstinctFunctions.WriteToInstinctLog(LogString.ToString, True)
                End SyncLock

                Try
                    objApplicationHasBeenLoadedAndChecked = objOnlineConversion.LoadAndFraudCheck(strXML, "O", bolSkipFraudCheckForUI, _
                                             bolSkipFraudCheckForKF, bolSkipFraudCheckForS, False, INI_Group_Member_Code, _
                                             bolWriteLog, bolUserDefinedAlertinOutput, bolUserIdinOutput, bolRulesInOutput, _
                                             bolDecisionReasonInOutput, INI_Site_With_Special_Functions, IIf(INI_Action_Output_File_Flag.Trim.ToUpper = "Y", 1, 0), True, -1, AddFraudCheck)
                Catch ex2 As Exception

                    If INI_Write_Log_File = "Y" Then

                        LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                        LogString.Append(InstinctFunctions.AddDateTimeToMessage("Online response follows -"))
                        LogString.Append(InstinctFunctions.AddDateTimeToMessage(errStr))
                        LogString.Append(InstinctFunctions.AddDateTimeToMessage("99" & INI_Delimiter_Character & ex2.Message))

                    End If

                    SyncLock objFraudCheck
                        Call InstinctFunctions.WriteToInstinctLog(LogString.ToString, True)
                    End SyncLock

                    Try
                        objApplicationHasBeenLoadedAndChecked = objOnlineConversion.LoadAndFraudCheck(strXML, "O", bolSkipFraudCheckForUI, _
                                                 bolSkipFraudCheckForKF, bolSkipFraudCheckForS, False, INI_Group_Member_Code, _
                                                 bolWriteLog, bolUserDefinedAlertinOutput, bolUserIdinOutput, bolRulesInOutput, _
                                                 bolDecisionReasonInOutput, INI_Site_With_Special_Functions, IIf(INI_Action_Output_File_Flag.Trim.ToUpper = "Y", 1, 0), True, -1, AddFraudCheck)
                    Catch ex3 As Exception

                        If INI_Write_Log_File = "Y" Then

                            LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                            LogString.Append(InstinctFunctions.AddDateTimeToMessage("Online response follows -"))
                            LogString.Append(InstinctFunctions.AddDateTimeToMessage(errStr))
                            LogString.Append(InstinctFunctions.AddDateTimeToMessage("99" & INI_Delimiter_Character & ex3.Message))

                        End If

                        SyncLock objFraudCheck
                            Call InstinctFunctions.WriteToInstinctLog(LogString.ToString, True)
                        End SyncLock

                        strInput = "99" & INI_Delimiter_Character & ex3.Message
                        If INI_Site_With_Special_Functions.Trim.ToUpper = "HKBMX" Then
                            strInput = strInput & InstinctFunctions.AddTimeStampExitStatusToOutput("FALSE", INI_Delimiter_Character, False)
                        End If

                        Exit Sub

                    End Try

                End Try

            End Try

            If objApplicationHasBeenLoadedAndChecked = False Then
                Exit Sub
            End If

            strAppKey = objOnlineConversion.GetAppKey


            If INI_Site_With_Special_Functions.Trim.ToUpper = "SIGCN" Then
                TriggeredRulesDefinitions = objOnlineConversion.GetTriggeredRulesDefinitions(strAppKey)
            End If



            'InstinctFunctions.Auto_Action(strAppKey, INI_Site_With_Special_Functions, INI_Action_Output_File_Flag, Load_Mode)
            If INI_Site_With_Special_Functions.Trim.ToUpper <> "SPDB" Then
                InstinctFunctions.Update_Assignment(strAppKey, INI_New_Applications_Age)
            Else
                AssignmentThread = New System.Threading.Thread(AddressOf Update_Assignment_SPDB)
                AssignmentThread.Start(strAppKey)
                'ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf Update_Assignment_SPDB), strAppKey)
            End If
            strUserDefinedAlert = InstinctFunctions.Get_User_Defined_Alert(strAppKey)

            sSql = "SELECT Action_Taken, Decision_Reason, Fraud_Alert_User_Id FROM A_Application (NOLOCK) WHERE Appkey = '" & strAppKey & "' "
            dsApplication = SqlHelper.ExecuteDataset(connstr, CommandType.Text, sSql)
            If Not dsApplication Is Nothing AndAlso dsApplication.Tables.Count > 0 AndAlso dsApplication.Tables(0).Rows.Count > 0 Then
                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Action_Taken")) Then
                    strActionTaken = dsApplication.Tables(0).Rows(0)("Action_Taken")
                End If

                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Decision_Reason")) Then
                    strDecisionReason = dsApplication.Tables(0).Rows(0)("Decision_Reason")
                End If

                If Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)("Fraud_Alert_User_Id")) Then
                    strFraudAlertUserId = dsApplication.Tables(0).Rows(0)("Fraud_Alert_User_Id")
                End If
            End If

            OnlineLogPath = INI_Output_Directory & "\O" & objOnlineConversion.GetAppKey & "-" & Format(Now, "yyyyMMddHHMMssffftt") & ".LOG"

            Try

                If INI_Write_Log_File = "Y" Then
                    SyncLock objFraudCheck
                        'Fix second language issue for the output file when the second language character is in the output file
                        If INI_Input_Format = "UNICODE" Then
                            'for unicode country, set output file as unicode to allow second language character                        
                            LogOutputFile = New StreamWriter(OnlineLogPath, True, Encoding.Unicode)
                        Else
                            LogOutputFile = New StreamWriter(OnlineLogPath, True, Encoding.Default)
                        End If
                        '-----------------------------------------------------------------------------------

                        LogOutputFile.Write(objOnlineConversion.GetOutputLog)
                        LogOutputFile.Flush()
                        LogOutputFile.Dispose()
                        LogOutputFile = Nothing
                    End SyncLock
                End If
            Catch ex As Exception
                If (WriteIISLong IsNot Nothing) Then
                    WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " TCPIP Handler error (Write to log). Application Number = " & strAppKey & " Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".")
                Else
                    Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "TCPIP Handler error (Write to log). Application Number = " & strAppKey & " Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".", EventLogEntryType.Warning, evtNumber, CatServices)
                End If
            End Try

            OnlineReturnRecord = ""
            'If INI_Fraud_Check_Output_File_Flag = "Y" Then
            OnlineReturnRecord = objOnlineConversion.GetOutputFile
            Dim decisionReasonPos As Integer = OnlineReturnRecord.Split(INI_Delimiter_Character).Length - 2

            dsApplication = GetApplicationDetails(strAppKey, INI_Site_With_Special_Functions.ToUpper.Trim, _
            INI_Rules_in_Output_File.ToUpper.Trim, INI_Rules_Description_in_Output_File.ToUpper.Trim, _
            INI_Diary_in_Output_File.ToUpper.Trim, INI_Action_Count_Number_in_Output_File.Trim, _
            INI_Nature_Of_Fraud_in_Output_File.Trim)

            If Not dsApplication Is Nothing _
            AndAlso dsApplication.Tables.Count = 6 Then
                ' Action Count Number
                If INI_Action_Count_Number_in_Output_File.ToUpper.Trim = "Y" Then
                    If dsApplication.Tables(4).Rows.Count > 0 Then
                        OnlineReturnRecord += IIf(Convert.IsDBNull(dsApplication.Tables(4).Rows(0)(0)), "", dsApplication.Tables(4).Rows(0)(0).ToString) + INI_Delimiter_Character
                    Else
                        OnlineReturnRecord += "0" + INI_Delimiter_Character
                    End If
                End If

                ' Nature of Fraud
                If INI_Nature_Of_Fraud_in_Output_File.ToUpper.Trim = "Y" Then
                    For Each drNarureOfFraud As DataRow In dsApplication.Tables(5).Rows
                        OnlineReturnRecord += "F" + INI_Delimiter_Character + IIf(Convert.IsDBNull(drNarureOfFraud(0)), "", drNarureOfFraud(0)) + INI_Delimiter_Character
                    Next
                End If

                ' Sub Code 2 - 5 (Application User Fields 16 - 19)
                If INI_Site_With_Special_Functions.ToUpper.Trim = "GEINDIA" Then
                    OnlineReturnRecord += IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(0)), "", dsApplication.Tables(0).Rows(0)(0)) + INI_Delimiter_Character + _
                                            IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(1)), "", dsApplication.Tables(0).Rows(0)(1)) + INI_Delimiter_Character + _
                                            IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(2)), "", dsApplication.Tables(0).Rows(0)(2)) + INI_Delimiter_Character + _
                                            IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(3)), "", dsApplication.Tables(0).Rows(0)(3)) + INI_Delimiter_Character
                ElseIf INI_Site_With_Special_Functions.ToUpper.Trim = "SDB" Then
                    OnlineReturnRecord += IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(0)), "", dsApplication.Tables(0).Rows(0)(0)) + INI_Delimiter_Character
                End If

                ' Rules Description 1-TOTAL_RULE_TRIGGERED
                If INI_Rules_Description_in_Output_File.ToUpper.Trim = "Y" Then
                    If dsApplication.Tables(1).Rows.Count > 0 Then
                        If dsApplication.Tables(1).Columns.Count <> 2 Then
                            ' No rules triggered
                            For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                                OnlineReturnRecord += INI_Delimiter_Character
                            Next
                        Else
                            If dsApplication.Tables(1).Rows.Count <= TOTAL_RULE_TRIGGERED Then
                                For Each drRulesDescription As DataRow In dsApplication.Tables(1).Rows
                                    OnlineReturnRecord += IIf(Convert.IsDBNull(drRulesDescription(1)), "", drRulesDescription(1)) + INI_Delimiter_Character
                                Next

                                For index = dsApplication.Tables(1).Rows.Count + 1 To TOTAL_RULE_TRIGGERED
                                    OnlineReturnRecord += INI_Delimiter_Character
                                Next
                            Else
                                index = 0
                                For Each drRulesDescription As DataRow In dsApplication.Tables(1).Rows
                                    index += 1
                                    If index > TOTAL_RULE_TRIGGERED Then
                                        Exit For
                                    End If
                                    OnlineReturnRecord += IIf(Convert.IsDBNull(drRulesDescription(1)), "", drRulesDescription(1)) + INI_Delimiter_Character
                                Next
                            End If
                        End If
                    Else
                        For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                            OnlineReturnRecord += INI_Delimiter_Character
                        Next
                    End If
                End If

                'For CreditEase add Origination Databse and Matched AppKeys
                If INI_Site_With_Special_Functions.Trim.ToUpper = "CRE" Then
                    dsMatchedAppKeys = GetApplicationMatchedApplicationDetails(strAppKey, INI_Site_With_Special_Functions.Trim.ToUpper)

                    If Not dsMatchedAppKeys Is Nothing AndAlso dsMatchedAppKeys.Tables.Count > 0 Then
                        'Origination Databse
                        If dsMatchedAppKeys.Tables(0).Rows.Count > 0 Then
                            If dsMatchedAppKeys.Tables(0).Rows.Count <= 20 Then
                                For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                    OnlineReturnRecord += IIf(Convert.IsDBNull(drMatchedApp(1)), "", drMatchedApp(1)) + INI_Delimiter_Character
                                Next

                                For index = dsMatchedAppKeys.Tables(0).Rows.Count + 1 To 20
                                    OnlineReturnRecord += INI_Delimiter_Character
                                Next
                            Else
                                index = 0
                                For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                    index += 1
                                    If index > 20 Then
                                        Exit For
                                    End If
                                    OnlineReturnRecord += IIf(Convert.IsDBNull(drMatchedApp(1)), "", drMatchedApp(1)) + INI_Delimiter_Character
                                Next
                            End If
                        Else
                            For i As Integer = 1 To 20
                                OnlineReturnRecord += INI_Delimiter_Character
                            Next
                        End If

                        'Matched AppKeys
                        If dsMatchedAppKeys.Tables(0).Rows.Count > 0 Then
                            If dsMatchedAppKeys.Tables(0).Rows.Count <= 20 Then
                                For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                    OnlineReturnRecord += IIf(Convert.IsDBNull(drMatchedApp(2)), "", drMatchedApp(2)) + INI_Delimiter_Character
                                Next

                                For index = dsMatchedAppKeys.Tables(0).Rows.Count + 1 To 20
                                    OnlineReturnRecord += INI_Delimiter_Character
                                Next
                            Else
                                index = 0
                                For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                    index += 1
                                    If index > 20 Then
                                        Exit For
                                    End If
                                    OnlineReturnRecord += IIf(Convert.IsDBNull(drMatchedApp(2)), "", drMatchedApp(2)) + INI_Delimiter_Character
                                Next
                            End If
                        Else
                            For i As Integer = 1 To 20
                                OnlineReturnRecord += INI_Delimiter_Character
                            Next
                        End If

                    End If
                End If

                ' Diary Notes
                If INI_Diary_in_Output_File.ToUpper.Trim = "Y" Then
                    If dsApplication.Tables(2).Rows.Count > 0 Then
                        For Each drManualDiary As DataRow In dsApplication.Tables(2).Rows
                            OnlineReturnRecord += IIf(Convert.IsDBNull(drManualDiary(0)), "", drManualDiary(0)) + INI_Delimiter_Character + _
                                                    IIf(Convert.IsDBNull(drManualDiary(1)), "", drManualDiary(1)) + INI_Delimiter_Character + _
                                                    IIf(Convert.IsDBNull(drManualDiary(2)), "", drManualDiary(2)) + INI_Delimiter_Character + _
                                                    IIf(Convert.IsDBNull(drManualDiary(3)), "", drManualDiary(3)) + INI_Delimiter_Character
                        Next
                    Else
                        For Each drManualDiary As DataRow In dsApplication.Tables(3).Rows
                            OnlineReturnRecord += IIf(Convert.IsDBNull(drManualDiary(0)), "", drManualDiary(0)) + INI_Delimiter_Character + _
                                                    IIf(Convert.IsDBNull(drManualDiary(1)), "", drManualDiary(1)) + INI_Delimiter_Character + _
                                                    IIf(Convert.IsDBNull(drManualDiary(2)), "", drManualDiary(2)) + INI_Delimiter_Character + _
                                                    IIf(Convert.IsDBNull(drManualDiary(3)), "", drManualDiary(3)) + INI_Delimiter_Character
                        Next
                    End If
                End If

                If INI_Site_With_Special_Functions.ToUpper.Trim = "VAB" Then
                    OnlineReturnRecord += IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(0)), "", dsApplication.Tables(0).Rows(0)(0)) + INI_Delimiter_Character
                End If

                ' Change the OnlineReturnRecord into FIXED format
                Dim iField_Length As Short
                Dim iIndex As Short
                Dim strOutput() As String

                strOutput = OnlineReturnRecord.Split(INI_Delimiter_Character)

                ' Update User Defined Alert
                If INI_User_Defined_Alert_in_Output_File.ToUpper.Trim = "Y" Then
                    strOutput(11) = strUserDefinedAlert
                End If

                ' Update Action Taken
                strOutput(9) = strActionTaken

                ' Update Decision Reason
                If INI_Decision_Reason_in_Output_File.ToUpper.Trim = "Y" Then
                    strOutput(decisionReasonPos) = strDecisionReason
                End If

                If INI_Site_With_Special_Functions.Trim = "HLB" Then
                    'Dim strOutput() As String
                    'Dim iField_Length As Short

                    'strOutput = OnlineReturnRecord.Split(INI_Delimiter_Character)

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
                        iIndex = 10
                        If strOutput.Length > iIndex Then
                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                iField_Length = 2
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            Else
                                OnlineReturnRecord = OnlineReturnRecord & strOutput(iIndex) & INI_Delimiter_Character
                            End If
                        End If

                        ' User Defined Alert
                        If INI_User_Defined_Alert_in_Output_File.ToUpper.Trim = "Y" Then
                            iIndex = iIndex + 1
                            If strOutput.Length > iIndex Then
                                If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                    iField_Length = 10
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                Else
                                    OnlineReturnRecord = OnlineReturnRecord & strOutput(iIndex) & INI_Delimiter_Character
                                End If
                            End If
                        End If

                        ' Action User Id
                        If INI_User_Id_in_Output_File.ToUpper.Trim = "Y" Then
                            iIndex = iIndex + 1
                            If strOutput.Length > iIndex Then
                                If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                    iField_Length = 20
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                Else
                                    OnlineReturnRecord = OnlineReturnRecord & strOutput(iIndex) & INI_Delimiter_Character
                                End If
                            End If
                        End If

                        ' Decision Reason
                        If INI_Decision_Reason_in_Output_File.ToUpper.Trim = "Y" Then
                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                iField_Length = 100
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strDecisionReason, iField_Length) & INI_Delimiter_Character
                            Else
                                OnlineReturnRecord = OnlineReturnRecord & strDecisionReason & INI_Delimiter_Character
                            End If


                        End If
                    End If
                Else
                    If INI_Output_Format.ToUpper.Trim = "FIXED" Then

                        If strOutput.Length > 0 Then
                            OnlineReturnRecord = ""

                            ' Organisation
                            iIndex = 0
                            If strOutput.Length > iIndex Then
                                iField_Length = 3
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Country Code
                            iIndex = 1
                            If strOutput.Length > iIndex Then
                                iField_Length = 2
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Group Member Code
                            iIndex = 2
                            If strOutput.Length > iIndex Then
                                iField_Length = 4
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Application Number
                            iIndex = 3
                            If strOutput.Length > iIndex Then
                                iField_Length = 25
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Capture Date
                            iIndex = 4
                            If strOutput.Length > iIndex Then
                                iField_Length = 10
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Capture Time
                            iIndex = 5
                            If strOutput.Length > iIndex Then
                                iField_Length = 6
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Application Type
                            iIndex = 6
                            If strOutput.Length > iIndex Then
                                iField_Length = 4
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Fraud Score
                            iIndex = 7
                            If strOutput.Length > iIndex Then
                                iField_Length = 3
                                If IsNumeric(strOutput(iIndex)) = True Then
                                    If CInt(strOutput(iIndex)) > 999 Then
                                        OnlineReturnRecord = OnlineReturnRecord & "999" & INI_Delimiter_Character
                                    ElseIf CInt(strOutput(iIndex)) < 0 Then
                                        OnlineReturnRecord = OnlineReturnRecord & "000" & INI_Delimiter_Character
                                    Else
                                        OnlineReturnRecord = OnlineReturnRecord & Format(CInt(strOutput(iIndex)), "000") & INI_Delimiter_Character
                                    End If

                                Else
                                    OnlineReturnRecord = OnlineReturnRecord & "000" & INI_Delimiter_Character
                                End If
                            End If

                            ' Fraud Alert
                            iIndex = 8
                            If strOutput.Length > iIndex Then
                                iField_Length = 1
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Action Taken
                            iIndex = 9
                            If strOutput.Length > iIndex Then
                                iField_Length = 1
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Error Code
                            iIndex = 10
                            If strOutput.Length > iIndex Then
                                iField_Length = 2
                                OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                            End If

                            ' User Defined Alert
                            If INI_User_Defined_Alert_in_Output_File.ToUpper.Trim = "Y" Then
                                iIndex = iIndex + 1
                                If strOutput.Length > iIndex Then
                                    iField_Length = 10
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                End If
                            End If

                            ' Action User Id
                            If INI_User_Id_in_Output_File.ToUpper.Trim = "Y" Then
                                iIndex = iIndex + 1
                                If strOutput.Length > iIndex Then
                                    iField_Length = 20
                                    OnlineReturnRecord = OnlineReturnRecord & InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                End If
                            End If

                            ' Rules(1 - 20)
                            If INI_Rules_in_Output_File.ToUpper.Trim = "Y" Then
                                ' Rules 1 - 20
                                For a As Integer = 1 To TOTAL_RULE_TRIGGERED
                                    iIndex = iIndex + 1
                                    If strOutput.Length > iIndex Then
                                        iField_Length = 7
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                    End If
                                Next
                            End If

                            ' HSBC Turkey
                            If INI_Site_With_Special_Functions.ToUpper.Trim = "HSBCTURKEY" Then
                                ' Instinct Call ID (User Field 5)
                                iIndex = iIndex + 1
                                If strOutput.Length > iIndex Then
                                    iField_Length = 50
                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                End If

                                ' Customer (User Field 6)
                                iIndex = iIndex + 1
                                If strOutput.Length > iIndex Then
                                    iField_Length = 50
                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                End If
                            End If

                            ' Decision Reason
                            If INI_Decision_Reason_in_Output_File.ToUpper.Trim = "Y" Then
                                iIndex = iIndex + 1
                                If strOutput.Length > iIndex Then
                                    iField_Length = 100
                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strOutput(iIndex), iField_Length) & INI_Delimiter_Character
                                End If
                            End If

                            ' Action Count Number
                            If INI_Action_Count_Number_in_Output_File.ToUpper.Trim = "Y" Then
                                iField_Length = 3

                                If dsApplication.Tables(4).Rows.Count > 0 Then
                                    OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(dsApplication.Tables(4).Rows(0)(0)), "", dsApplication.Tables(4).Rows(0)(0).ToString), iField_Length) & INI_Delimiter_Character
                                Else
                                    OnlineReturnRecord += "000" + INI_Delimiter_Character
                                End If
                            End If

                            ' Nature of Fraud
                            If INI_Nature_Of_Fraud_in_Output_File.ToUpper.Trim = "Y" Then
                                For Each drNarureOfFraud As DataRow In dsApplication.Tables(5).Rows
                                    iField_Length = 10
                                    OnlineReturnRecord += "F" & INI_Delimiter_Character & InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drNarureOfFraud(0)), "", drNarureOfFraud(0)), iField_Length) & INI_Delimiter_Character
                                Next
                            End If

                            ' Sub Code 2 - 5 (Application User Fields 16 - 19)
                            If INI_Site_With_Special_Functions.ToUpper.Trim = "GEINDIA" Then
                                iField_Length = 50
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(0)), "", dsApplication.Tables(0).Rows(0)(0)), iField_Length) & INI_Delimiter_Character
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(1)), "", dsApplication.Tables(0).Rows(0)(1)), iField_Length) & INI_Delimiter_Character
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(2)), "", dsApplication.Tables(0).Rows(0)(2)), iField_Length) & INI_Delimiter_Character
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(3)), "", dsApplication.Tables(0).Rows(0)(3)), iField_Length) & INI_Delimiter_Character
                            ElseIf INI_Site_With_Special_Functions.ToUpper.Trim = "SDB" Then
                                iField_Length = 50
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(0)), "", dsApplication.Tables(0).Rows(0)(0)), iField_Length) & INI_Delimiter_Character
                            End If

                            ' Rules Description 1 - TOTAL_RULE_TRIGGERED
                            If INI_Rules_Description_in_Output_File.ToUpper.Trim = "Y" Then
                                iField_Length = 100
                                If dsApplication.Tables(1).Rows.Count > 0 Then
                                    If dsApplication.Tables(1).Columns.Count <> 2 Then
                                        For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) + INI_Delimiter_Character
                                        Next
                                    Else
                                        If dsApplication.Tables(1).Rows.Count <= TOTAL_RULE_TRIGGERED Then
                                            For Each drRulesDescription As DataRow In dsApplication.Tables(1).Rows
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drRulesDescription(1)), "", drRulesDescription(0)), iField_Length) + INI_Delimiter_Character
                                            Next

                                            For index = dsApplication.Tables(1).Rows.Count + 1 To TOTAL_RULE_TRIGGERED
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) + INI_Delimiter_Character
                                            Next
                                        Else
                                            index = 0
                                            For Each drRulesDescription As DataRow In dsApplication.Tables(1).Rows
                                                index += 1
                                                If index > TOTAL_RULE_TRIGGERED Then
                                                    Exit For
                                                End If
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drRulesDescription(1)), "", drRulesDescription(0)), iField_Length) + INI_Delimiter_Character
                                            Next
                                        End If
                                    End If
                                Else
                                    For i As Integer = 1 To TOTAL_RULE_TRIGGERED
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) + INI_Delimiter_Character
                                    Next
                                End If
                            End If

                            'For CreditEase add Origination Databse and Matched AppKeys
                            If INI_Site_With_Special_Functions.Trim.ToUpper = "CRE" Then
                                If Not dsMatchedAppKeys Is Nothing AndAlso dsMatchedAppKeys.Tables.Count > 0 Then
                                    'Origination Databse
                                    iField_Length = 1
                                    If dsMatchedAppKeys.Tables(0).Rows.Count > 0 Then
                                        If dsMatchedAppKeys.Tables(0).Rows.Count <= 20 Then
                                            For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drMatchedApp(1)), "", drMatchedApp(1)), iField_Length) + INI_Delimiter_Character
                                            Next

                                            For index = dsMatchedAppKeys.Tables(0).Rows.Count + 1 To 20
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) + INI_Delimiter_Character
                                            Next
                                        Else
                                            index = 0
                                            For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                                index += 1
                                                If index > 20 Then
                                                    Exit For
                                                End If
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drMatchedApp(1)), "", drMatchedApp(1)), iField_Length) + INI_Delimiter_Character
                                            Next
                                        End If
                                    Else
                                        For i As Integer = 1 To 20
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) + INI_Delimiter_Character
                                        Next
                                    End If

                                    'Matched AppKeys
                                    iField_Length = 3499
                                    If dsMatchedAppKeys.Tables(0).Rows.Count > 0 Then
                                        If dsMatchedAppKeys.Tables(0).Rows.Count <= 20 Then
                                            For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drMatchedApp(2)), "", drMatchedApp(2)), iField_Length) + INI_Delimiter_Character
                                            Next

                                            For index = dsMatchedAppKeys.Tables(0).Rows.Count + 1 To 20
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) + INI_Delimiter_Character
                                            Next
                                        Else
                                            index = 0
                                            For Each drMatchedApp As DataRow In dsMatchedAppKeys.Tables(0).Rows
                                                index += 1
                                                If index > 20 Then
                                                    Exit For
                                                End If
                                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(drMatchedApp(2)), "", drMatchedApp(2)), iField_Length) + INI_Delimiter_Character
                                            Next
                                        End If
                                    Else
                                        For i As Integer = 1 To 20
                                            OnlineReturnRecord += InstinctFunctions.PadWithSpaces("", iField_Length) + INI_Delimiter_Character
                                        Next
                                    End If

                                End If
                            End If

                            ' Diary Notes
                            If INI_Diary_in_Output_File.ToUpper.Trim = "Y" Then
                                If dsApplication.Tables(2).Rows.Count > 0 Then
                                    For Each drManualDiary As DataRow In dsApplication.Tables(2).Rows
                                        iField_Length = 10
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(0), iField_Length) & INI_Delimiter_Character
                                        iField_Length = 6
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(1), iField_Length) & INI_Delimiter_Character
                                        iField_Length = 20
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(2), iField_Length) & INI_Delimiter_Character
                                        iField_Length = 2000
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(3), iField_Length) & INI_Delimiter_Character
                                    Next
                                Else
                                    For Each drManualDiary As DataRow In dsApplication.Tables(3).Rows
                                        iField_Length = 10
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(0), iField_Length) & INI_Delimiter_Character
                                        iField_Length = 6
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(1), iField_Length) & INI_Delimiter_Character
                                        iField_Length = 20
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(2), iField_Length) & INI_Delimiter_Character
                                        iField_Length = 2000
                                        OnlineReturnRecord += InstinctFunctions.PadWithSpaces(drManualDiary(3), iField_Length) & INI_Delimiter_Character
                                    Next
                                End If
                            End If

                            ' CBA User Fields 29
                            If INI_Site_With_Special_Functions.ToUpper.Trim = "VAB" Then
                                iField_Length = 5000
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(IIf(Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(0)), "", dsApplication.Tables(0).Rows(0)(0)), iField_Length) & INI_Delimiter_Character
                            End If

                        End If
                    Else
                        OnlineReturnRecord = String.Join(INI_Delimiter_Character, strOutput)
                    End If
                    '* -------------DecTech Code Modification Comment Start--------------
                    '* 
                    '* Description: add new field at the end for SPDB special function
                    '*               
                    '* Modified by : Hugh Hu
                    '* Modified on : 9/12/2011
                    '* Solution: wI 9170
                    If INI_Site_With_Special_Functions.ToUpper = "SPDB" Then
                        If INI_Low_Fraud_Score.Contains("-") Then
                            Dim LowFraudScoreFrom As String = INI_Low_Fraud_Score.Split("-")(0)
                            Dim LowFraudScoreTo As String = INI_Low_Fraud_Score.Split("-")(1)
                            If IsNumeric(strOutput(7)) = True And IsNumeric(LowFraudScoreFrom) = True _
                            And IsNumeric(LowFraudScoreTo) = True Then
                                If CInt(strOutput(7)) >= CInt(LowFraudScoreFrom) And CInt(strOutput(7)) <= CInt(LowFraudScoreTo) Then
                                    strLowFraudScoreFlag = "N"
                                Else
                                    strLowFraudScoreFlag = "Y"
                                End If
                            Else
                                strLowFraudScoreFlag = "N"
                            End If
                        Else
                            strLowFraudScoreFlag = "N"
                        End If
                        OnlineReturnRecord += strLowFraudScoreFlag & INI_Delimiter_Character

                        If INI_Fraud_Alert_UserId.ToUpper.Trim = "Y" Then
                            If INI_Output_Format.ToUpper.Trim = "FIXED" Then
                                OnlineReturnRecord += InstinctFunctions.PadWithSpaces(strFraudAlertUserId, 20) & INI_Delimiter_Character
                            Else
                                OnlineReturnRecord += strFraudAlertUserId & INI_Delimiter_Character
                            End If
                        End If

                        OnlineReturnRecord += SPDB.Instance.TriggeredRule(connstr, strAppKey, True)
                        OnlineReturnRecord += SPDB.Instance.TriggeredRule(connstr, strAppKey, False)
                        OnlineReturnRecord += SPDB.Instance.PhoneRecords(connstr, strAppKey)
                    End If

                    If INI_Site_With_Special_Functions.ToUpper.Trim = "HKBMX" Then
                        If strOutput.Length > 10 AndAlso strOutput(10).Trim <> String.Empty Then
                            OnlineReturnRecord += InstinctFunctions.AddTimeStampExitStatusToOutput("FALSE", INI_Delimiter_Character, IIf(INI_Output_Format.ToUpper.Trim = "FIXED", True, False))
                        Else
                            OnlineReturnRecord += InstinctFunctions.AddTimeStampExitStatusToOutput("TRUE", INI_Delimiter_Character, IIf(INI_Output_Format.ToUpper.Trim = "FIXED", True, False))
                        End If
                    End If

                End If
                '* --------------DecTech Code Modification Comment End---------------

            End If

            If INI_Write_Log_File = "Y" Then

                LogString.Append(InstinctFunctions.AddDateTimeToMessage(" "))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage("Online response follows -"))
                LogString.Append(InstinctFunctions.AddDateTimeToMessage(OnlineReturnRecord))

                SyncLock objFraudCheck
                    Call InstinctFunctions.WriteToInstinctLog(LogString.ToString, True)
                End SyncLock

            End If
            'End If

            If SVR_Store_Instinct_Data_In_Uppercase.Trim.ToUpper = "OFF" Then
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCi
            End If

            strInput = OnlineReturnRecord

            objOnlineConversion = Nothing

        Catch ex As Exception
            If (WriteIISLong IsNot Nothing) Then
                WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " OnlineCall TCPIP. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".")
            Else
                Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "OnlineCall TCPIP. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".", EventLogEntryType.Error, 1, 4)
            End If
        End Try

    End Sub
    Private Sub Update_Assignment_SPDB(ByVal strAppKey As String)
        InstinctFunctions.Update_Assignment(strAppKey, INI_New_Applications_Age)
    End Sub
    Public Function IniFraudCheck() As String

        Dim Negative_Time As Boolean
        Dim iPosition As Short

        Try

            '*
            '* Retrieve all the INI parameters
            '*

            TargetDatabase = "A"

            Dim clsSetIniValue As New SetINIValue


            'Write Log File
            INI_Value = clsSetIniValue.WriteLogFile.Trim.ToUpper
            INI_Write_Log_File = Trim(INI_Value.ToUpper)

            If INI_Write_Log_File <> "Y" Then
                INI_Write_Log_File = "N"
            End If

            ' User Defined Alert in Output File
            INI_Value = clsSetIniValue.UserDefinedAlertInOutputFile.Trim.ToUpper
            If INI_Value.Trim = "" Then
                'Set the default value
                INI_User_Defined_Alert_in_Output_File = "N"
            Else
                If INI_Value.Trim.ToUpper = "Y" Then
                    INI_User_Defined_Alert_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_User_Defined_Alert_in_Output_File = "N"
                End If
            End If

            'Decision Reason in Output File
            INI_Value = clsSetIniValue.DecisionReasonInOutputFile.Trim.ToUpper
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Decision_Reason_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Value)) = "Y" Then
                    INI_Decision_Reason_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_Decision_Reason_in_Output_File = "N"
                End If
            End If

            'Site with Special Functions
            INI_Site_With_Special_Functions = Trim(clsSetIniValue.SiteWithSpecialFunctions).ToUpper


            INI_Value = UCase(clsSetIniValue.SecondaryServicePrefix)

            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Second_Service_Suffix = ""
            Else
                INI_Second_Service_Suffix = INI_Value
            End If
            'Initial Catalog
            INI_Value = clsSetIniValue.InitialCatalog
            INI_Initial_Catalog = Trim(INI_Value)
            If INI_Initial_Catalog = "" Then
                'Set the default value
                INI_Initial_Catalog = "Instinct"
            End If

            'Data Source
            INI_Value = clsSetIniValue.DataSource
            INI_Data_Source = Trim(INI_Value)
            If INI_Data_Source = "" Then
                'Set the default value
                INI_Data_Source = "(local)"
            End If

            'Use Windows Authentication
            INI_Value = clsSetIniValue.UseWindowsAuthentication.Trim.ToUpper
            INI_Use_Windows_Authentication = Trim(INI_Value)
            If INI_Use_Windows_Authentication = "" Then
                'Default value
                INI_Use_Windows_Authentication = "Y"
            End If

            'Use Defined Encryption Key
            INI_Value = clsSetIniValue.UseDefinedEncryptionKey.Trim.ToUpper
            INI_Use_Defined_Encryption_Key = Trim(INI_Value)
            If INI_Use_Defined_Encryption_Key = "" Then
                'Default value
                INI_Use_Defined_Encryption_Key = "N"
            End If

            'Key 1 Path
            INI_Value = clsSetIniValue.Key1Path.Trim
            INI_Key1_Path = Trim(INI_Value)
            If INI_Key1_Path = "" Then
                INI_Key1_Path = "C:\Key1.key"
            End If

            'Key 2 Path
            INI_Value = clsSetIniValue.Key2Path.Trim
            INI_Key2_Path = Trim(INI_Value)
            If INI_Key2_Path = "" Then
                INI_Key2_Path = "C:\Key2.key"
            End If

            'Database User ID
            INI_Value = clsSetIniValue.DatabaseUserId.Trim
            INI_Database_User_Id = Trim(INI_Value)
            If INI_Database_User_Id = "" Then
                'Default value
                INI_Database_User_Id = "InstinctSysAdm"
            End If

            'Database Password
            'The password will be decrypted when connecting to database
            INI_Value = clsSetIniValue.DatabasePassword
            INI_Database_Password = Trim(INI_Value)

            'Delimiter Character
            INI_Value = clsSetIniValue.DelimiterCharacters
            If Trim(INI_Value) <> "" Then
                INI_Delimiter_Character = Trim(INI_Value)
            End If
            If INI_Delimiter_Character = "" Then
                'Set the default value
                INI_Delimiter_Character = "|"
            End If

            'Organisation
            INI_Value = clsSetIniValue.AppOrganisation
            If Trim(INI_Value) <> "" Then
                INI_Organisation = Trim(INI_Value)
            End If

            'Default Country
            INI_Value = clsSetIniValue.DefaultCountry
            If Trim(INI_Value) <> "" Then
                INI_Default_Country = Trim(INI_Value)
            End If

            'Group Member Code
            INI_Value = clsSetIniValue.GroupMemberCode
            If Trim(INI_Value) <> "" Then
                INI_Group_Member_Code = Trim(INI_Value)
            Else
                INI_Group_Member_Code = ""
            End If

            'Fraud Check Output File Flag
            INI_Value = clsSetIniValue.AppActionOutputFileFlag
            'If Trim(INI_Value) <> "" Then
            '    INI_Action_Output_File_Flag = Trim(INI_Value)
            'Else
            '    INI_Action_Output_File_Flag = "N"
            'End If
            If Trim(INI_Value) = "" Then
                INI_Action_Output_File_Flag = "Y"
            Else
                If UCase(Trim(INI_Value)) = "Y" OrElse UCase(Trim(INI_Value)) = "N" Then
                    INI_Action_Output_File_Flag = UCase(Trim(INI_Value))
                Else
                    INI_Action_Output_File_Flag = "Y"
                End If
            End If

            'If Trim(INI_Value) = "" Then
            '    'Set the default value
            'INI_Fraud_Check_Output_File_Flag = "N"
            'Else
            '    If UCase(Trim(INI_Value)) = "Y" Then
            '        INI_Fraud_Check_Output_File_Flag = "Y"
            '    Else
            '        'Set the default value
            '        INI_Fraud_Check_Output_File_Flag = "N"
            '    End If
            'End If

            'Output Directory
            INI_Value = clsSetIniValue.AppOutputDirectory
            If Trim(INI_Value) <> "" Then
                INI_Output_Directory = Trim(INI_Value)
            End If
            If INI_Output_Directory = "" Then
                'Set the default value
                INI_Output_Directory = "C:"
            End If


            INI_Value = clsSetIniValue.AppLocalTimeDifference
            If Trim(INI_Value) <> "" Then
                If IsNumeric(INI_Value) = True Then
                    iPosition = InStr(INI_Value, "-")
                    If iPosition = 0 Then
                        Negative_Time = False
                    Else
                        Negative_Time = True
                    End If

                    INI_Value = Replace(INI_Value, "-", "")
                    INI_Value = Replace(INI_Value, "+", "")
                    INI_Value = Format(CInt(INI_Value), "0000")

                    INI_Local_Time_Difference = (CInt(Left(INI_Value, 2)) * 60) + CInt(Mid(INI_Value, 3, 2))

                    If Negative_Time = True Then
                        INI_Local_Time_Difference = INI_Local_Time_Difference * -1
                    End If
                Else
                    'Set the default value
                    INI_Local_Time_Difference = 0
                End If
            Else
                'Set the default value
                INI_Local_Time_Difference = 0
            End If

            'Output Format
            INI_Value = UCase(clsSetIniValue.AppOutputFormat)
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Output_Format = "VARIABLE"
            Else
                If UCase(Trim(INI_Value)) = "FIXED" _
                Or UCase(Trim(INI_Value)) = "VARIABLE" Then
                    INI_Output_Format = UCase(Trim(INI_Value))
                Else
                    'Set the default value
                    INI_Output_Format = "VARIABLE"
                End If
            End If

            'Input Format
            INI_Value = UCase(clsSetIniValue.AppInputFormat)
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Input_Format = "ASCII"
            Else
                If UCase(Trim(INI_Value)) = "ASCII" _
                Or UCase(Trim(INI_Value)) = "UNICODE" _
                Or UCase(Trim(INI_Value)) = "DEFAULT" Then
                    INI_Input_Format = UCase(Trim(INI_Value))
                Else
                    'Set the default value
                    INI_Input_Format = "ASCII"
                End If
            End If

            'ININewApplicationsAge
            INI_Value = clsSetIniValue.NewApplicationsAge
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_New_Applications_Age = 1
            Else
                If Not Integer.TryParse(INI_Value, INI_New_Applications_Age) Then
                    INI_New_Applications_Age = 1
                End If
            End If

            'Fraud Alert UserId
            INI_Value = clsSetIniValue.FraudAlertUserId
            If INI_Site_With_Special_Functions.ToUpper.Trim = "SPDB" Then
                If Trim(INI_Value) = "" Then
                    INI_Fraud_Alert_UserId = "N"
                Else
                    If UCase(Trim(INI_Value)) = "Y" OrElse UCase(Trim(INI_Value)) = "N" Then
                        INI_Fraud_Alert_UserId = UCase(Trim(INI_Value))
                    Else
                        INI_Fraud_Alert_UserId = "N"
                    End If
                End If
            Else
                INI_Fraud_Alert_UserId = "N"
            End If
            INI_ApplicationName = clsSetIniValue.ApplicationName
            If Trim(INI_ApplicationName) = "" Then
                INI_ApplicationName = ""
            Else
                INI_ApplicationName = ";Application Name=" & INI_ApplicationName.Trim
            End If
            '****************************************

            'Max Pool Size
            INI_Value = clsSetIniValue.MaxPoolSize

            If Not INI_Value Is Nothing AndAlso INI_Value.Trim <> "" AndAlso INI_Site_With_Special_Functions.ToUpper.Trim = "SPDB" Then
                Dim maxPool As Int32 = 0
                If Int32.TryParse(INI_Max_Pool_Size.Trim, maxPool) = False Then
                    maxPool = 0
                End If
                If maxPool > 100 AndAlso maxPool < 32000 Then
                    INI_Max_Pool_Size = maxPool.ToString
                Else
                    INI_Max_Pool_Size = ""
                End If
            Else
                INI_Max_Pool_Size = ""
            End If


            If INI_Use_Defined_Encryption_Key = "Y" AndAlso INI_Use_Windows_Authentication = "N" Then
                ConnectDatabaseUsingDefinedEncryptionKey()
            Else
                ConnectDatabase()
            End If

            '*
            '* Retrieve all server parameters
            '*

            SVR_Command_Timeout = InstinctFunctions.GetInstinctParameter("Command Timeout", 1)
            SVR_Scheduled_Fraud_Check_Flag = InstinctFunctions.GetInstinctParameter("Scheduled Fraud Check Flag", 1)
            SVR_Audit_Log_Deletion_Period = InstinctFunctions.GetInstinctParameter("Audit Log Deletion Period", 2)
            SVR_Store_Instinct_Data_In_Uppercase = InstinctFunctions.GetInstinctParameter("Store Instinct Data In Uppercase", 1)
            SVR_Keep_UI_Action_When_Score_is_Changed = InstinctFunctions.GetInstinctParameter("Keep UI Action When Score is Changed", 1)
            SVR_Skip_Fraud_Check_For_KF_When_Update = InstinctFunctions.GetInstinctParameter("Skip Fraud Check For KF when Update", 1)
            SVR_Skip_Fraud_Check_For_Suspicious_When_Update = InstinctFunctions.GetInstinctParameter("Skip Fraud Check For Suspect when Update", 1)
            SVR_Skip_Fraud_Check_For_UI_When_Update = InstinctFunctions.GetInstinctParameter("Skip Fraud Check For UI when Update", 1)
            bolMillisecondsInLogFile = IIf(INI_Site_With_Special_Functions.Trim.ToUpper = "SPDB" AndAlso InstinctFunctions.GetInstinctParameter("Milliseconds in Log File", 1).Trim.ToUpper = "Y", True, False)

            If SVR_Store_Instinct_Data_In_Uppercase = "OFF" Then
                oldCi = System.Threading.Thread.CurrentThread.CurrentCulture
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-AU")
            End If

            '*
            '* Checking/Validation for Start-up
            '*
            If Trim(INI_Organisation) = "" Then

                If (WriteIISLong IsNot Nothing) Then
                    WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix + " Start Up - Mandatory value not found on INI file (Organisation).")
                Else
                    Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up - Mandatory value not found on INI file (Organisation).", EventLogEntryType.Error, 1, 4)
                End If

                Return "99|Start Up error - Mandatory value not found on INI file (Organisation). Error Description: " & EventLogEntryType.Error.ToString
            End If

            If Trim(INI_Default_Country) = "" Then
                If (WriteIISLong IsNot Nothing) Then
                    WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " Start Up - Mandatory value not found on INI file (Default Country).")
                Else
                    Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up - Mandatory value not found on INI file (Default Country).", EventLogEntryType.Error, 1, 4)
                End If

                Return "99|Start Up error - Mandatory value not found on INI file (Default Country). Error Description: " & EventLogEntryType.Error.ToString
            End If

            'Make sure the polling interval is within the valid range of values
            If INI_Pooling_Interval < 1 Or INI_Pooling_Interval > 64 Then
                INI_Pooling_Interval = 2
            End If

            'Output Directory
            If Not Directory.Exists(INI_Output_Directory) Then
                'Write error to Windows system event log
                If (WriteIISLong IsNot Nothing) Then
                    WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " Start Up - Output directory does not exist.")
                Else
                    Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up - Output directory does not exist.", EventLogEntryType.Error, 1, 4)
                End If

                Return "99|Start Up error - Output directory does not exist. "
            End If

            'User ID in Output File
            INI_Value = UCase(clsSetIniValue.UserIdInOutputFile)

            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_User_Id_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Value)) = "Y" Then
                    INI_User_Id_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_User_Id_in_Output_File = "N"
                End If
            End If

            ' Rules in Output File
            INI_Value = UCase(clsSetIniValue.RulesInOutputFile)
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Rules_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Value)) = "Y" Then
                    INI_Rules_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_Rules_in_Output_File = "N"
                End If
            End If

            ' Action Count Number in Output File
            INI_Value = UCase(clsSetIniValue.ActionCountNumberInOutputFile)
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Action_Count_Number_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Value)) = "Y" Then
                    INI_Action_Count_Number_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_Action_Count_Number_in_Output_File = "N"
                End If
            End If

            ' Nature of Fraud in Output File
            INI_Value = UCase(clsSetIniValue.NatureOfFraudInOutputFile)
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Nature_Of_Fraud_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Value)) = "Y" Then
                    INI_Nature_Of_Fraud_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_Nature_Of_Fraud_in_Output_File = "N"
                End If
            End If

            ' Rules Description in Output File
            INI_Value = UCase(clsSetIniValue.RulesDescriptionInOutputFile)
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Rules_Description_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Value)) = "Y" Then
                    INI_Rules_Description_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_Rules_Description_in_Output_File = "N"
                End If
            End If

            ' Diary in Output File
            INI_Value = UCase(clsSetIniValue.DiaryInOutputFile)
            If Trim(INI_Value) = "" Then
                'Set the default value
                INI_Diary_in_Output_File = "N"
            Else
                If UCase(Trim(INI_Value)) = "Y" Then
                    INI_Diary_in_Output_File = "Y"
                Else
                    'Set the default value
                    INI_Diary_in_Output_File = "N"
                End If
            End If

            If SVR_Skip_Fraud_Check_For_UI_When_Update.ToUpper.Trim = "Y" Then
                bolSkipFraudCheckForUI = True
            Else
                bolSkipFraudCheckForUI = False
            End If

            If SVR_Skip_Fraud_Check_For_KF_When_Update.ToUpper.Trim = "Y" Then
                bolSkipFraudCheckForKF = True
            Else
                bolSkipFraudCheckForKF = False
            End If

            If SVR_Skip_Fraud_Check_For_Suspicious_When_Update.ToUpper.Trim = "Y" Then
                bolSkipFraudCheckForS = True
            Else
                bolSkipFraudCheckForS = False
            End If

            If INI_Write_Log_File.Trim.ToUpper = "Y" Then
                bolWriteLog = True
            Else
                bolWriteLog = False
            End If

            If INI_User_Defined_Alert_in_Output_File.Trim.ToUpper = "Y" Then
                bolUserDefinedAlertinOutput = True
            Else
                bolUserDefinedAlertinOutput = False
            End If

            If INI_User_Id_in_Output_File.Trim.ToUpper = "Y" Then
                bolUserIdinOutput = True
            Else
                bolUserIdinOutput = False
            End If

            If INI_Rules_in_Output_File.Trim.ToUpper = "Y" Then
                bolRulesInOutput = True
            Else
                bolRulesInOutput = False
            End If

            If INI_Rules_Description_in_Output_File.Trim.ToUpper = "Y" Then
                bolRulesDescriptionInOutput = True
            Else
                bolRulesDescriptionInOutput = False
            End If

            If INI_Decision_Reason_in_Output_File.Trim.ToUpper = "Y" Then
                bolDecisionReasonInOutput = True
            Else
                bolDecisionReasonInOutput = False
            End If

            objTimeDifference = New TimeDifference(INI_Local_Time_Difference)

            clsSetIniValue = Nothing

            Return ""
        Catch ex As Exception
            If (WriteIISLong IsNot Nothing) Then
                WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " Start Up error. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".")
            Else
                Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up error. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".", EventLogEntryType.Error, 1, 4)
            End If

            Return "99|Start Up error. Error Number = " & ex.Source & ".  Error Description: " & ex.Message
        End Try
    End Function

    Public Function GetSubCodes(ByVal sAppKey As String) As ArrayList
        Dim cmd As SqlClient.SqlCommand
        Dim objLocalConnection As SqlClient.SqlConnection
        Dim dtrSubcodes As SqlClient.SqlDataReader
        Dim arrReturnValues As New ArrayList

        Try
            ' Intialize connection
            objLocalConnection = New SqlClient.SqlConnection(connstr)
            objLocalConnection.Open()

            ' Loading
            cmd = New SqlClient.SqlCommand("SELECT User_Field16, User_Field17, User_Field18, User_Field19 FROM A_Application WHERE AppKey = '" & sAppKey & "'", objLocalConnection)
            cmd.CommandTimeout = 0
            cmd.CommandType = CommandType.Text

            dtrSubcodes = cmd.ExecuteReader()
            dtrSubcodes.Read()

            ' User Field 16
            If Not Convert.IsDBNull(dtrSubcodes(0)) Then
                arrReturnValues.Add(CStr(dtrSubcodes(0)))
            Else
                arrReturnValues.Add("")
            End If
            ' User Field 17
            If Not Convert.IsDBNull(dtrSubcodes(1)) Then
                arrReturnValues.Add(CStr(dtrSubcodes(1)))
            Else
                arrReturnValues.Add("")
            End If
            ' User Field 18
            If Not Convert.IsDBNull(dtrSubcodes(2)) Then
                arrReturnValues.Add(CStr(dtrSubcodes(2)))
            Else
                arrReturnValues.Add("")
            End If
            ' User Field 19
            If Not Convert.IsDBNull(dtrSubcodes(3)) Then
                arrReturnValues.Add(CStr(dtrSubcodes(3)))
            Else
                arrReturnValues.Add("")
            End If

            dtrSubcodes.Close()

            Return arrReturnValues
        Catch ex As Exception
            If (WriteIISLong IsNot Nothing) Then
                WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " OnlineCall TCPIP. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".")
            Else
                Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "OnlineCall TCPIP. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".", EventLogEntryType.Error, 1, 4)
            End If

            Return Nothing
        Finally
            If Not dtrSubcodes Is Nothing Then
                dtrSubcodes.Close()
            End If
            If Not objLocalConnection Is Nothing Then
                objLocalConnection.Dispose()
            End If
        End Try
    End Function

    Public Function GetApplicationDetails(ByVal sAppKey As String, ByVal sSite As String, ByVal sRulesInOutputFile As String, _
    ByVal sRulesDescInOutputFile As String, ByVal sDiaryInOutputFile As String, ByVal sActionCountNbrInOutputFile As String, _
    ByVal sNatureOfFraudInOutputFile As String) As DataSet
        Dim arParams() As SqlParameter = New SqlParameter(6) {}

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey
            arParams(1) = New SqlParameter("@SiteWithSpecialFunctions", SqlDbType.NVarChar)
            arParams(1).Value = sSite
            arParams(2) = New SqlParameter("@RulesInOutputFile", SqlDbType.NVarChar, 1)
            arParams(2).Value = sRulesInOutputFile
            arParams(3) = New SqlParameter("@RulesDescriptionInOutputFile", SqlDbType.NVarChar, 1)
            arParams(3).Value = sRulesDescInOutputFile
            arParams(4) = New SqlParameter("@DiaryInOutputFile", SqlDbType.NVarChar, 1)
            arParams(4).Value = sDiaryInOutputFile
            arParams(5) = New SqlParameter("@ActionCountNbrInOutputFile", SqlDbType.NVarChar, 1)
            arParams(5).Value = sActionCountNbrInOutputFile
            arParams(6) = New SqlParameter("@NatureOfFraudInOutputFile", SqlDbType.NVarChar, 1)
            arParams(6).Value = sNatureOfFraudInOutputFile

            Return SqlHelper.ExecuteDataset(connstr, CommandType.StoredProcedure, "USP_FraudCheckWebService_SpecificApplication_Select", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub ConnectDatabase()
        Try
            If INI_Database_Password = "" Then
                INI_Database_Password = "instinct"
            Else
                INI_Database_Password = Encrypt.AESDecryption(INI_Database_Password)
            End If

            If INI_Use_Windows_Authentication.ToUpper = "Y" Then
                connstr = "database=" & INI_Initial_Catalog & ";server=" & INI_Data_Source & ";Integrated Security=SSPI" & INI_ApplicationName
            Else

                connstr = "database=" & INI_Initial_Catalog & ";server=" & INI_Data_Source & ";user id=" & _
                          INI_Database_User_Id & ";password=" & INI_Database_Password & INI_ApplicationName
            End If
            If INI_Max_Pool_Size.Trim <> "" Then
                connstr = connstr & ";Max Pool Size=" & INI_Max_Pool_Size.Trim
            End If
        Catch ex As Exception
            'Write error to Windows system event log
            If (WriteIISLong IsNot Nothing) Then
                WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " Database Connection Error = " & ex.Message & ".")
            Else
                Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Database Connection Error = " & ex.Message & ".", EventLogEntryType.Error, 1, 4)
            End If
        End Try
    End Sub

    Private Sub ConnectDatabaseUsingDefinedEncryptionKey()
        Dim objReader As StreamReader
        Dim strInstinctKey As String = "Qd2&#iR^-AlWp1.@"
        Dim strKey1 As String = ""
        Dim strKey2 As String = ""
        Dim strKey As String = ""

        Try
            If File.Exists(INI_Key1_Path) AndAlso File.Exists(INI_Key2_Path) Then
                ' Read 1st key
                objReader = New StreamReader(INI_Key1_Path)
                strKey1 = objReader.ReadToEnd().Trim
                objReader.Close()

                If strKey1.Trim.Length < 16 Then
                    Throw New Exception("The length of Key 1 must be at least 16 characters")
                Else
                    strKey1 = strKey1.Trim.Substring(0, 16)
                End If

                ' Read 2nd key
                objReader = New StreamReader(INI_Key2_Path)
                strKey2 = objReader.ReadToEnd().Trim
                objReader.Close()

                If strKey2.Trim.Length < 16 Then
                    Throw New Exception("The length of Key 2 must be at least 16 characters")
                Else
                    strKey2 = strKey2.Trim.Substring(0, 16)
                End If

                ' Decrypt password
                strKey = XorString(XorString(strKey1, strInstinctKey), strKey2)
                If INI_Database_Password = "" Then
                    INI_Database_Password = "instinct"
                Else
                    INI_Database_Password = AESDecryption(INI_Database_Password, strKey)
                End If
                connstr = "database=" & INI_Initial_Catalog & ";server=" & INI_Data_Source & ";user id=" & INI_Database_User_Id & ";password=" & INI_Database_Password
                If INI_Max_Pool_Size.Trim <> "" Then
                    connstr = connstr & ";Max Pool Size=" & INI_Max_Pool_Size.Trim
                End If
            Else
                If (WriteIISLong IsNot Nothing) Then
                    WriteIISLong("InstinctEncryptionService" & " The paths for Key 1 and/or Key 2 are not valid.")
                Else
                    Diagnostics.EventLog.WriteEntry("InstinctEncryptionService", "The paths for Key 1 and/or Key 2 are not valid.", EventLogEntryType.Error, 1, 4)
                End If
            End If
        Catch ex As Exception
            'Write error to Windows system event log
            If (WriteIISLong IsNot Nothing) Then
                WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " Database Connection Error = " & ex.Message & connstr & ".")
            Else
                Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Database Connection Error = " & ex.Message & connstr & ".", EventLogEntryType.Error, 1, 4)
            End If
        End Try
    End Sub

    Private Function AESDecryption(ByVal sInput As String, ByVal sKey As String) As String
        Dim textConverter As New ASCIIEncoding
        Dim myRijndael As New RijndaelManaged()
        Dim key() As Byte
        Dim IV() As Byte
        Dim fromEncrypt() As Byte

        Try
            key = Encoding.ASCII.GetBytes(sKey)

            IV = Encoding.ASCII.GetBytes(sKey)

            myRijndael.Mode = CipherMode.CBC

            myRijndael.Padding = PaddingMode.PKCS7

            Dim decryptor As ICryptoTransform = myRijndael.CreateDecryptor(key, IV)

            Dim msDecrypt As New MemoryStream()
            fromEncrypt = Convert.FromBase64String(sInput)
            Dim csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write)
            csDecrypt.Write(fromEncrypt, 0, fromEncrypt.Length)
            csDecrypt.FlushFinalBlock()

            Return (New System.Text.UTF8Encoding).GetString(msDecrypt.ToArray())
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function XorString(ByVal targetString As String, ByVal maskValue As String) As String
        Dim Index As Integer = 0
        Dim ReturnValue As String = ""

        Try
            For Each CharValue As Char In targetString.ToCharArray
                ReturnValue = String.Concat(ReturnValue, Chr(Asc(CharValue) Xor Asc(maskValue.Substring(Index, 1))))
                Index = (Index + 1) Mod maskValue.Length
            Next
            Return ReturnValue
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function ValidateLicence() As String
        Dim strRegistrationCode As String = String.Empty
        Dim dtDatabaseDate As Date
        Dim intLicenceValid As Integer = 0
        Dim ExpiryDays As Integer = 0
        Dim strMessage As String = String.Empty

        Try
            strRegistrationCode = Get_Licence_Key()
            dtDatabaseDate = Get_DatabaseDate()

            If strRegistrationCode.Trim <> String.Empty Then
                Dim lcValidate As New License("INSTINCT", INI_Organisation, INI_Default_Country, 30)
                Dim lcResult As New License.LicenceKeyValidateResults

                lcResult = lcValidate.ValidLicense(strRegistrationCode, dtDatabaseDate)

                ExpiryDays = lcResult.ExpireyDays

                If lcResult.ValidateResult = License.LicenceKeyValidateResults.LicenseResultEnum.LicenceKeyValid Then
                    intLicenceValid = 1
                ElseIf lcResult.ValidateResult = License.LicenceKeyValidateResults.LicenseResultEnum.LicenceExpireSoon Then
                    intLicenceValid = 2
                    If ExpiryDays = 0 Then
                        strMessage = "The license key in your system will expire today. Please contact DecTech Solutions to obtain a new one."
                    ElseIf ExpiryDays = 1 Then
                        strMessage = "The license key in your system will expire tomorrow. Please contact DecTech Solutions to obtain a new one."
                    Else
                        strMessage = "The license key in your system will expire in " + ExpiryDays.ToString() + " days. Please contact DecTech Solutions to obtain a new one."
                    End If
                ElseIf lcResult.ValidateResult = License.LicenceKeyValidateResults.LicenseResultEnum.LicenceExpired Then
                    intLicenceValid = 100
                    strMessage = "The license key in your system has expired. Please contact DecTech Solutions for further support."
                Else
                    intLicenceValid = 99
                End If

            Else
                intLicenceValid = 99
            End If

            If intLicenceValid = 99 Then
                strMessage = "The license key in your system is not valid. Please contact DecTech Solutions for further support."
            End If

            If intLicenceValid = 1 Then
                strMessage = ""
            ElseIf intLicenceValid = 2 Then
                strMessage = "Start Up Warning - " & strMessage
                If (WriteIISLong IsNot Nothing) Then
                    WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " " & strMessage)
                Else
                    Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, strMessage, EventLogEntryType.Information, 1, 4)
                End If
                strMessage = ""
            Else
                strMessage = "Start Up Error - " & strMessage
                If (WriteIISLong IsNot Nothing) Then
                    WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " " & strMessage)
                Else
                    Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, strMessage, EventLogEntryType.Error, 1, 4)
                End If
                strMessage = "99|" & strMessage
            End If

            Return strMessage
        Catch ex As Exception
            If (WriteIISLong IsNot Nothing) Then
                WriteIISLong("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix & " Start Up error. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".")
            Else
                Diagnostics.EventLog.WriteEntry("InstinctServiceFraudCheckWeb" & INI_Organisation & INI_Default_Country & INI_Second_Service_Suffix, "Start Up error. Error Number = " & Err.Number & ".  Error Description: " & Err.Description & ".", EventLogEntryType.Error, 1, 4)
            End If
            Return "99|Start Up error. Error Number = " & ex.Source & ".  Error Description: " & ex.Message
        End Try

    End Function

    Private Function Get_Licence_Key() As String
        Dim dsRegistration As DataSet
        Dim strRegistrationCode As String = ""

        Try

            dsRegistration = SqlHelper.ExecuteDataset(connstr, CommandType.StoredProcedure, "USP_System_InstinctRegistration_CheckRegistration")

            'Get registration code from Database
            If Not dsRegistration Is Nothing AndAlso dsRegistration.Tables.Count > 0 AndAlso _
              dsRegistration.Tables(0).Rows.Count > 0 AndAlso _
              Not IsDBNull(dsRegistration.Tables(0).Rows(0)(1)) AndAlso dsRegistration.Tables(0).Rows(0)(1).ToString.Trim <> String.Empty Then
                strRegistrationCode = dsRegistration.Tables(0).Rows(0)(1).ToString.Trim
            Else
                strRegistrationCode = ""
            End If

            Return strRegistrationCode
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function Get_DatabaseDate() As Date
        Dim dsResult As DataSet
        Dim dtDatabaseDate As Date
        dtDatabaseDate = Date.Now

        Try
            dsResult = SqlHelper.ExecuteDataset(connStr, CommandType.Text, "SELECT GETDATE()")

            If Not dsResult Is Nothing AndAlso dsResult.Tables.Count > 0 AndAlso dsResult.Tables(0).Rows.Count > 0 Then
                dtDatabaseDate = dsResult.Tables(0).Rows(0)(0)
            End If

            Return dtDatabaseDate
        Catch ex As Exception
            Return dtDatabaseDate
        End Try
    End Function

    Public Function GetApplicationMatchedApplicationDetails(ByVal sAppKey As String, ByVal sSite As String) As DataSet
        Dim arParams() As SqlParameter = New SqlParameter(1) {}

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey
            arParams(1) = New SqlParameter("@SiteWithSpecialFunction", SqlDbType.NVarChar)
            arParams(1).Value = sSite

            Return SqlHelper.ExecuteDataset(connstr, CommandType.StoredProcedure, "USP_Common_GetMatchedAppKeys", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
