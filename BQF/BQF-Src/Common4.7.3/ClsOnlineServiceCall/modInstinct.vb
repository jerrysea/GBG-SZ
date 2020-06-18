Imports System.Threading

Module modInstinct

    'DB Connection
    Public connstr As String

    'Const
    Public Const CatServices As Integer = 4
    Public Const evtNumber As Short = 1


    'Instinct Server Parameters
    Public SVR_Command_Timeout As String
    Public SVR_Scheduled_Fraud_Check_Flag As String
    Public SVR_Audit_Log_Deletion_Period As String
    Public SVR_Store_Instinct_Data_In_Uppercase As String
    Public SVR_Keep_UI_Action_When_Score_is_Changed As String
    Public SVR_Skip_Fraud_Check_For_KF_When_Update As String
    Public SVR_Skip_Fraud_Check_For_Suspicious_When_Update As String
    Public SVR_Skip_Fraud_Check_For_UI_When_Update As String

    'Instinct INI Parameters
    Public INI_Value As String
    Public INI_Use_Windows_Authentication As String
    Public INI_Use_Defined_Encryption_Key As String
    Public INI_Key1_Path As String
    Public INI_Key2_Path As String
    Public INI_Database_User_Id As String
    Public INI_Database_Password As String
    Public INI_Data_Source As String
    Public INI_Initial_Catalog As String
    Public INI_Organisation As String
    Public INI_Default_Country As String
    Public INI_Delimiter_Character As String
    Public INI_Pooling_Interval As Short
    Public INI_Port_Number As Short
    Public INI_Output_Directory As String
    Public INI_Input_Format As String
    Public INI_Output_Format As String
    Public INI_Output_File_Deletion_Period As Integer
    Public INI_Fraud_Check_Output_File_Flag As String
    Public INI_Action_Output_File_Flag As String
    Public INI_Local_Time_Difference As Long
    Public INI_Criminal_File_Type As String
    Public INI_Criminal_File_With_Column_Row As String
    Public INI_User_Id_in_Output_File As String
    Public INI_Rules_in_Output_File As String
    Public INI_Rules_Description_in_Output_File As String
    Public INI_Nature_Of_Fraud_in_Output_File As String
    Public INI_Action_Count_Number_in_Output_File As String
    Public INI_Diary_in_Output_File As String
    Public INI_Site_With_Special_Functions As String
    Public INI_Second_Service_Suffix As String
    Public INI_Write_Log_File As String
    Public INI_Decision_Reason_in_Output_File As String
    Public INI_User_Defined_Alert_in_Output_File As String
    Public INI_Group_Member_Code As String
    Public INI_Low_Fraud_Score As String
    Public INI_Fraud_Alert_UserId As String
    Public INI_Max_Pool_Size As String
    Public INI_New_Applications_Age As Int32
    Public INI_ApplicationName As String
    'Other
    Public objTimeDifference As TimeDifference
    Public LogOutputFile As IO.StreamWriter
    Public LogFile As System.Text.StringBuilder
    Public AssignmentThread As Thread

    'Boolean 
    Public bolSavedUserField As Boolean
    Public bolSkipFraudCheckForUI As Boolean
    Public bolSkipFraudCheckForKF As Boolean
    Public bolSkipFraudCheckForS As Boolean
    Public bolWriteLog As Boolean
    Public bolUserDefinedAlertinOutput As Boolean
    Public bolUserIdinOutput As Boolean
    Public bolRulesInOutput As Boolean
    Public bolRulesDescriptionInOutput As Boolean
    Public bolDecisionReasonInOutput As Boolean
    Public bolFraudAlertUserIdInOutput As Boolean
    Public bolMillisecondsInLogFile As String


End Module
