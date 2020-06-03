Public Class SetINIValue

    Public Property UseWindowsAuthentication() As String
        Get
            Return INI_Use_Windows_Authentication
        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Use_Windows_Authentication = Value

        End Set
    End Property

    Public Property UseDefinedEncryptionKey() As String
        Get
            Return INI_Use_Defined_Encryption_Key
        End Get

        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Use_Defined_Encryption_Key = Value

        End Set
    End Property

    Public Property Key1Path() As String
        Get
            Return INI_Key1_Path
        End Get

        Set(ByVal Value As String)
            INI_Key1_Path = Value
        End Set
    End Property

    Public Property Key2Path() As String
        Get
            Return INI_Key2_Path
        End Get

        Set(ByVal Value As String)
            INI_Key2_Path = Value
        End Set
    End Property

    Public Property SiteWithSpecialFunctions() As String
        Get
            Return INI_Site_With_Special_Functions
        End Get

        Set(ByVal Value As String)
            INI_Site_With_Special_Functions = Value
        End Set
    End Property
    Public Property SecondaryServicePrefix() As String
        Get
            Return INI_Second_Service_Suffix
        End Get

        Set(ByVal Value As String)
            INI_Second_Service_Suffix = Value
        End Set
    End Property
    Public Property LowFraudScore() As String
        Get
            Return INI_Low_Fraud_Score
        End Get

        Set(ByVal Value As String)
            INI_Low_Fraud_Score = Value
        End Set
    End Property


    Public Property DefaultCountry() As String
        Get

            Return INI_Default_Country

        End Get
        Set(ByVal Value As String)

            INI_Default_Country = Value

        End Set
    End Property

    Public Property InitialCatalog() As String
        Get

            Return INI_Initial_Catalog

        End Get
        Set(ByVal Value As String)

            INI_Initial_Catalog = Value

        End Set
    End Property

    Public Property DataSource() As String
        Get

            Return INI_Data_Source

        End Get
        Set(ByVal Value As String)

            INI_Data_Source = Value

        End Set
    End Property

    Public Property DatabaseUserId() As String
        Get

            Return INI_Database_User_Id

        End Get
        Set(ByVal Value As String)

            INI_Database_User_Id = Value

        End Set
    End Property

    Public Property DatabasePassword() As String
        Get

            Return INI_Database_Password

        End Get
        Set(ByVal Value As String)

            INI_Database_Password = Value

        End Set
    End Property

    Public Property DelimiterCharacters() As String
        Get

            Return INI_Delimiter_Character

        End Get
        Set(ByVal Value As String)

            INI_Delimiter_Character = Value

        End Set
    End Property

    Public Property AppOrganisation() As String
        Get

            Return INI_Organisation

        End Get
        Set(ByVal Value As String)

            INI_Organisation = Value

        End Set
    End Property

    Public Property AppOutputDirectory() As String
        Get

            Return INI_Output_Directory

        End Get
        Set(ByVal Value As String)

            INI_Output_Directory = Value

        End Set
    End Property

    Public Property AppLocalTimeDifference() As String
        Get

            Return INI_Local_Time_Difference

        End Get
        Set(ByVal Value As String)

            INI_Local_Time_Difference = Value

        End Set
    End Property

    Public Property AppOutputFormat() As String
        Get

            Return INI_Output_Format

        End Get
        Set(ByVal Value As String)

            INI_Output_Format = Value

        End Set
    End Property

    Public Property AppActionOutputFileFlag() As String
        Get

            Return INI_Action_Output_File_Flag

        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Action_Output_File_Flag = Value

        End Set
    End Property

    Public Property AppInputFormat() As String
        Get

            Return INI_Input_Format

        End Get
        Set(ByVal Value As String)

            INI_Input_Format = Value

        End Set
    End Property

    Public Property UserIdInOutputFile() As String
        Get

            Return INI_User_Id_in_Output_File

        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_User_Id_in_Output_File = Value

        End Set
    End Property

    Public Property RulesInOutputFile() As String
        Get

            Return INI_Rules_in_Output_File

        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Rules_in_Output_File = Value

        End Set
    End Property

    Public Property RulesDescriptionInOutputFile() As String
        Get

            Return INI_Rules_Description_in_Output_File

        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Rules_Description_in_Output_File = Value

        End Set
    End Property

    Public Property NatureOfFraudInOutputFile() As String
        Get

            Return INI_Nature_Of_Fraud_in_Output_File

        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Nature_Of_Fraud_in_Output_File = Value

        End Set
    End Property

    Public Property ActionCountNumberInOutputFile() As String
        Get

            Return INI_Action_Count_Number_in_Output_File

        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Action_Count_Number_in_Output_File = Value

        End Set
    End Property

    Public Property DiaryInOutputFile() As String
        Get

            Return INI_Diary_in_Output_File

        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Diary_in_Output_File = Value

        End Set
    End Property

    Public Property WriteLogFile() As String
        Get
            If INI_Write_Log_File Is Nothing Then
                Return ""
            Else
                Return INI_Write_Log_File
            End If


        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Write_Log_File = Value

        End Set
    End Property

    Public Property DecisionReasonInOutputFile() As String
        Get
            If INI_Decision_Reason_in_Output_File Is Nothing Then
                Return ""
            Else
                Return INI_Decision_Reason_in_Output_File
            End If
        End Get
        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Decision_Reason_in_Output_File = Value

        End Set
    End Property

    Public Property UserDefinedAlertInOutputFile() As String
        Get
            If INI_User_Defined_Alert_in_Output_File Is Nothing Then
                Return ""
            Else
                Return INI_User_Defined_Alert_in_Output_File
            End If
        End Get

        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_User_Defined_Alert_in_Output_File = Value
        End Set
    End Property

    Public Property GroupMemberCode() As String
        Get

            Return INI_Group_Member_Code

        End Get
        Set(ByVal Value As String)

            INI_Group_Member_Code = Value

        End Set
    End Property

    Public Property NewApplicationsAge() As String
        Get
            Return INI_New_Applications_Age
        End Get

        Set(ByVal Value As String)
            INI_New_Applications_Age = Value
        End Set
    End Property

    Public Property FraudAlertUserId() As String
        Get
            Return INI_Fraud_Alert_UserId
        End Get

        Set(ByVal Value As String)
            Value = Value.ToUpper
            If Value <> "Y" AndAlso Value <> "N" Then
                Value = "N"
            End If

            INI_Fraud_Alert_UserId = Value
        End Set
    End Property
    Public Property ApplicationName() As String
        Get
            Return INI_ApplicationName
        End Get

        Set(ByVal Value As String)
            INI_ApplicationName = Value
        End Set
    End Property
    Public Property MaxPoolSize() As String
        Get
            Return INI_Max_Pool_Size
        End Get

        Set(ByVal Value As String)
            INI_Max_Pool_Size = Value
        End Set
    End Property

End Class
