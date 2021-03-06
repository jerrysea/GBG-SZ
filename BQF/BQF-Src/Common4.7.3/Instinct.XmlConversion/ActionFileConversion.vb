Imports System.Data.SqlClient
Imports System.Text
Imports System.IO

Public Class FraudFileConversion
    ' Variables
    Private Shared strOutputDirectory As String
    Private Shared strConn As String
    Private Shared conn As SqlConnection
    Private Shared Database As String
    Private Shared Delimiter As String
    Private Shared AddressSequence As String
    Private Shared SpecialCharacters As String    
    Private Shared CountryCode As String
    Private Shared ErrorCode As String
    Private Shared ErrorCategory As String
    Private Shared ErrorField As String
    Private Shared ErrorValue As String
    Private Shared ErrorXML As StringBuilder
    Private Shared DiaryXML As StringBuilder
    Private Shared arrInputString() As String
    Private Shared OutputString As String
    Private Shared CurrentAppKey As String
    Private Shared CurrentApplicationType As String
    Private Shared Organisation As ArrayList
    Private Shared ApplicationType As ArrayList
    Private Shared UserField7 As ArrayList
    Private Shared CategoryName As ArrayList
    Private Shared NameSequence As String
    Private Shared CompanySuffix As ArrayList
    Private Shared ValidDatePeriod As Integer
    Private Shared Application(1) As ArrayList
    Private Shared Applicant(1) As ArrayList
    Private Shared IntroducerAgent(1) As ArrayList
    Private Shared WriteToLog As Boolean
    Private Shared LoadingLog As StringBuilder

    ' Class
    Private Class ErrorDescription
        Public Shared InvalidDate As String = "An invalid date was detected."
        Public Shared InvalidTime As String = "An invalid time was detected."
        Public Shared InvalidAmountLimit As String = "Amount/Limit contained an invalid integer value."
        Public Shared InvalidDecisionDate As String = "Decision Date was invalid."
        Public Shared InvalidSex As String = "An invalid sex value was detected."
        Public Shared InvalidUserField7 As String = "Applicant User Field 7 was invalid."
        Public Shared InvalidNumber As String = "An invalid integer value was detected."
    End Class

    Private Class KeyFields
        Public Shared Organisation As String = ""
        Public Shared CountryCode As String = ""
        Public Shared ApplicationNumber As String = ""
        Public Shared ApplicationType As String = ""
        Public Shared AppKey As String = ""

        Public Shared Sub Reset()
            Organisation = ""
            CountryCode = ""
            ApplicationNumber = ""
            ApplicationType = ""
            AppKey = ""
        End Sub
    End Class

    Private Class FullName
        Public Shared FirstName As String = ""
        Public Shared MiddleName As String = ""
        Public Shared LastName As String = ""

        Public Shared Sub Reset()
            FirstName = ""
            MiddleName = ""
            LastName = ""
        End Sub
    End Class

    Private Class FullHomeAddress
        Public Shared HomeAddress1 As String = ""
        Public Shared HomeAddress2 As String = ""
        Public Shared HomeAddress3 As String = ""
        Public Shared HomeAddress4 As String = ""
        Public Shared HomeAddress5 As String = ""
        Public Shared HomeAddress6 As String = ""
        Public Shared HomePostcode As String = ""

        Public Shared Sub Reset()
            HomeAddress1 = ""
            HomeAddress2 = ""
            HomeAddress3 = ""
            HomeAddress4 = ""
            HomeAddress5 = ""
            HomeAddress6 = ""
            HomePostcode = ""
        End Sub
    End Class

    Private Class FullCompanyAddress
        Public Shared CompanyAddress1 As String = ""
        Public Shared CompanyAddress2 As String = ""
        Public Shared CompanyAddress3 As String = ""
        Public Shared CompanyAddress4 As String = ""
        Public Shared CompanyAddress5 As String = ""
        Public Shared CompanyAddress6 As String = ""
        Public Shared CompanyPostcode As String = ""

        Public Shared Sub Reset()
            CompanyAddress1 = ""
            CompanyAddress2 = ""
            CompanyAddress3 = ""
            CompanyAddress4 = ""
            CompanyAddress5 = ""
            CompanyAddress6 = ""
            CompanyPostcode = ""
        End Sub
    End Class

    Public Shared Sub Initialize(ByVal connection As String, ByVal sCountryCode As String, ByVal sDelimiter As String, ByVal sDatabase As String, ByVal bWriteToLog As Boolean)
        Dim cmd As SqlCommand
        Dim dsData As DataSet
        Dim daData As SqlDataAdapter
        Dim drData As DataRow

        Try
            strConn = connection
            conn = New SqlConnection(connection)
            conn.Open()
            Database = sDatabase
            Delimiter = sDelimiter.Trim            
            CountryCode = sCountryCode
            Organisation = New ArrayList
            ApplicationType = New ArrayList
            CompanySuffix = New ArrayList
            UserField7 = New ArrayList
            CategoryName = New ArrayList
            Application(0) = New ArrayList
            Application(1) = New ArrayList
            Applicant(0) = New ArrayList
            Applicant(1) = New ArrayList
            IntroducerAgent(0) = New ArrayList
            IntroducerAgent(1) = New ArrayList
            WriteToLog = bWriteToLog

            ' Get sequence number for First Name
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Name Sequence (First Name)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                NameSequence = NameSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                NameSequence = NameSequence & "1"
            End If

            ' Get sequence number for Middle Name
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Name Sequence (Middle Name)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                NameSequence = NameSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                NameSequence = NameSequence & "2"
            End If

            ' Get sequence number for Surname
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Name Sequence (Surname)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                NameSequence = NameSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                NameSequence = NameSequence & "3"
            End If

            AddressSequence = String.Empty
            ' Get sequence number for Address1
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 1)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "1"
            End If

            ' Get sequence number for Address2
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 2)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "2"
            End If

            ' Get sequence number for Address3
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 3)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "3"
            End If

            ' Get sequence number for Address4
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 4)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "4"
            End If

            ' Get sequence number for Address5
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 5)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "5"
            End If

            ' Get sequence number for Address6
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 6)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "6"
            End If

            ' Get sequence number for Postcode
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Postcode)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "7"
            End If

            ' Special Characters
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Special Characters"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                SpecialCharacters = CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                SpecialCharacters = ""
            End If

            ' Organisation
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_Organisation_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Organisation_Code")) Then
                    Organisation.Add(CStr(drData("Organisation_Code")))
                End If
            Next

            ' Application Type
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_ApplicationTypes_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Application_Code")) Then
                    ApplicationType.Add(CStr(drData("Application_Code")))
                End If
            Next

            ' Company Suffix
            dsData = New DataSet
            cmd = New SqlCommand("USP_Definitions_CompanySuffix_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Company Suffix")) Then
                    CompanySuffix.Add(drData("Company Suffix"))
                End If
            Next

            ' User Field 7 (Nature of Fraud)
            dsData = New DataSet
            cmd = New SqlCommand("USP_Definitions_LookUpList_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Code")) Then
                    UserField7.Add(drData("Code"))
                End If
            Next

            ' Valid Date Period
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Valid Date Period"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                ValidDatePeriod = CInt(dsData.Tables(0).Rows(0)("Default"))
            Else
                ValidDatePeriod = 150
            End If

            ' Category Name
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_CategoryName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Name")) Then
                    CategoryName.Add(drData("Category_Name"))
                End If
            Next

            '**************************************************
            '* Application
            '**************************************************
            ' Base(3) - Reporter Code
            Application(0).Add("Organisation")
            Application(1).Add("nvarchar(3)")
            ' No mapping for Country Code
            Application(0).Add("Country_Code")
            Application(1).Add("nvarchar(2)")
            ' Base(10) - Application No
            Application(0).Add("Application_Number")
            Application(1).Add("nvarchar(25)")
            ' Header(5) - Capture Date
            Application(0).Add("Capture_Date")
            Application(1).Add("datetime(8)")
            ' Segment(6) - Decision(Y/N)
            Application(0).Add("Decision")
            Application(1).Add("nvarchar(10)")
            ' Segment(9) - Product Code
            Application(0).Add("Application_Type")
            Application(1).Add("nvarchar(4)")
            ' Segment(10) - Amount of the application/Amount of the loan
            Application(0).Add("Amount_Limit")
            Application(1).Add("bigint(8)")
            ' Segment(11) - Date of application/Date opened
            Application(0).Add("Application_Date")
            Application(1).Add("datetime(8)")
            ' Segment(28) - Fraud Transaction Reason Code
            ' Segment(31) - Reason Code for Closure
            ' Segment(33) - Reason Code for Deletion
            Application(0).Add("Decision_Reason")
            Application(1).Add("nvarchar(201)")
            ' Segment(29) - Date Fraud Confirmed
            ' Segment(32) - Date Closed
            ' Segment(34) - Date Deleted
            Application(0).Add("Action_Date")
            Application(1).Add("datetime(8)")
            ' Base(3) - Reporter Code
            Application(0).Add("User_Field1")
            Application(1).Add("nvarchar(50)")
            ' Base(3) - Consumer Account No
            Application(0).Add("User_Field3")
            Application(1).Add("nvarchar(50)")
            ' Segment(2) - Branch Code
            Application(0).Add("User_Field4")
            Application(1).Add("nvarchar(50)")
            ' Segment(3) - Branch Name
            Application(0).Add("User_Field5")
            Application(1).Add("nvarchar(50)")
            ' Base(2) - Serial No
            Application(0).Add("User_Field6")
            Application(1).Add("nvarchar(50)")
            ' Base(4) - Contract Subject Code
            Application(0).Add("User_Field7")
            Application(1).Add("nvarchar(50)")
            ' Segment(4) - Reporting Reason Code
            Application(0).Add("User_Field8")
            Application(1).Add("nvarchar(50)")
            ' Segment(5) - Activity Date for Reporting
            Application(0).Add("User_Field9")
            Application(1).Add("nvarchar(50)")
            ' Segment(12) - Credit(Y/N)
            Application(0).Add("User_Field10")
            Application(1).Add("nvarchar(50)")
            ' Segment(13) - Collateral(Y/N)
            Application(0).Add("User_Field11")
            Application(1).Add("nvarchar(50)")
            ' Segment(14) - Guarantor(Y/N)
            Application(0).Add("User_Field12")
            Application(1).Add("nvarchar(50)")

            '**************************************************
            '* Applicant
            '**************************************************
            ' Base(5) - Resident Registration No.
            Applicant(0).Add("Id_Number1")
            Applicant(1).Add("nvarchar(30)")
            ' Base(6) - Business Owner No.
            Applicant(0).Add("Id_Number2")
            Applicant(1).Add("nvarchar(21)")
            ' Base(7) - Corporation No.
            Applicant(0).Add("Id_Number3")
            Applicant(1).Add("nvarchar(21)")
            ' Base(8) - Name
            Applicant(0).Add("Surname")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(19) - Home Address
            Applicant(0).Add("Home_Address1")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Home_Address2")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Home_Address3")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(18) - Home Postal Code
            Applicant(0).Add("Home_Postcode")
            Applicant(1).Add("nvarchar(10)")
            ' Segment(20) - Home Phone Number
            Applicant(0).Add("Home_Phone_Number")
            Applicant(1).Add("nvarchar(32)")
            ' Segment(16) - Mobile Phone No
            Applicant(0).Add("Mobile_Phone_Number")
            Applicant(1).Add("nvarchar(32)")
            ' Segment(24) - Employer Name/Company Name
            Applicant(0).Add("Company_Name")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(22) - Business Address
            Applicant(0).Add("Company_Address1")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Company_Address2")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Company_Address3")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(21) - Business Postal Code
            Applicant(0).Add("Company_Postcode")
            Applicant(1).Add("nvarchar(10)")
            ' Segment(23) - Business Phone Number
            Applicant(0).Add("Company_Phone_Number")
            Applicant(1).Add("nvarchar(32)")
            ' Segment(15) - Mobile Communication Code
            Applicant(0).Add("User_Field2")
            Applicant(1).Add("nvarchar(30)")
            ' Segment(25) - Job Title
            Applicant(0).Add("User_Field3")
            Applicant(1).Add("nvarchar(30)")
            ' Segment(17) - Email Address
            Applicant(0).Add("User_Field8")
            Applicant(1).Add("nvarchar(50)")
            ' Segment(26) - Income
            Applicant(0).Add("User_Field10")
            Applicant(1).Add("bigint(8)")
            ' Segment(27) - Property Tax
            Applicant(0).Add("User_Field11")
            Applicant(1).Add("bigint(8)")

            '**************************************************
            '* Introducer Agent
            '**************************************************            
            ' Segment(8) - Introducer Registration No.
            IntroducerAgent(0).Add("Id_Number1")
            IntroducerAgent(1).Add("nvarchar(21)")
            ' Segment(7) - Channel Code
            IntroducerAgent(0).Add("User_Field1")
            IntroducerAgent(1).Add("nvarchar(30)")

            conn.Close()
        Catch ex As Exception
            Throw ex
        Finally
            conn.Dispose()
        End Try
    End Sub

    Public Shared Function ConvertToXml(ByVal sInputString As String, Optional ByVal SiteWithSpecialFunction As String = "") As Boolean
        Dim ErrorCode As Integer
        Dim strOutput As StringBuilder
        Dim index, i, recordId As Integer
        Dim strFieldXml As StringBuilder

        Try
            ErrorCode = 0
            OutputString = ""
            ErrorXML = New StringBuilder
            DiaryXML = New StringBuilder
            strOutput = New StringBuilder
            strFieldXml = New StringBuilder
            LoadingLog = New StringBuilder
            KeyFields.Reset()

            ' Initialize                         
            strOutput.Append("<Application>")
            arrInputString = sInputString.Split(Delimiter)

            '================================================== 
            ' Process Application details
            '==================================================
            ' Write to log file
            If WriteToLog Then
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Loading/Validating Application Category" & vbCrLf)
            End If
            ErrorCategory = CategoryName(0)
            ' Get XML string for Application category            
            index = 0
            For i = index To index + Application(0).Count - 1
                ' Remove invalid characters & check errors
                If ValidateApplication(i - index, i) Then
                    ' Insert values to XML string
                    If String.Compare(Application(0)(i - index), "Application_Type") = 0 Then
                        strFieldXml.Append("<" & Application(0)(i - index) & ">" & arrInputString(i) & "</" & Application(0)(i - index) & ">")
                    Else
                        If arrInputString(i).Trim <> "" Then
                            strFieldXml.Append("<" & Application(0)(i - index) & ">" & arrInputString(i) & "</" & Application(0)(i - index) & ">")
                        End If
                    End If

                Else
                    OutputString = ("<Application>" & ErrorXML.ToString & "</Application>")
                    Return True
                End If
            Next
            strFieldXml.Append("<AppKey>" & KeyFields.AppKey & "</AppKey>")
            strFieldXml.Append("<Field_Error_Flag></Field_Error_Flag>")
            ' Append XML string to output string
            strOutput.Append(strFieldXml.ToString)

            ' Write to log file
            If WriteToLog Then
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Organisation = " & KeyFields.Organisation & ", Country Code = " & KeyFields.CountryCode & ", Application Number = " & KeyFields.ApplicationNumber & ", Application Type = " & KeyFields.ApplicationType & vbCrLf)
            End If

            '==================================================
            ' Process Applicant details
            '==================================================
            ErrorCategory = CategoryName(1)
            index = i
            recordId = 0
            While (index < arrInputString.Length AndAlso arrInputString(index) = "A")
                ' Write to log file
                If recordId = 0 AndAlso WriteToLog Then
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Loading/Validating Applicant Category" & vbCrLf)
                End If

                index = index + 1
                recordId = recordId + 1

                ' Reset FullName, FullHomeAddress,FullCompanyAddress 
                FullName.Reset()
                FullHomeAddress.Reset()
                FullCompanyAddress.Reset()

                ' Get XML string for Applicant category
                strFieldXml = strFieldXml.Remove(0, strFieldXml.ToString.Length)
                strFieldXml.Append("<Organisation>" & KeyFields.Organisation & "</Organisation><Country_Code>" & KeyFields.CountryCode & "</Country_Code><Application_Number>" & KeyFields.ApplicationNumber & "</Application_Number><Application_Type>" & KeyFields.ApplicationType & "</Application_Type><Sequence_Number>" & CStr(recordId) & "</Sequence_Number><Income_Increase_Percentage>0</Income_Increase_Percentage><AppKey>" & KeyFields.AppKey & "</AppKey>")
                strOutput.Append("<Applicant>")
                For i = index To index + Applicant(0).Count - 1
                    If ValidateApplicant(i - index, i) Then
                        If arrInputString(i).Trim <> "" Then
                            strFieldXml.Append("<" & Applicant(0)(i - index) & ">" & arrInputString(i) & "</" & Applicant(0)(i - index) & ">")
                        End If
                    Else
                        OutputString = ("<Application>" & ErrorXML.ToString & "</Application>")
                        Return True
                    End If
                Next
                ' Populate Full_Name, Full_Home_Address, Full_Company_Address tags
                strFieldXml.Append("<Full_Name>" & BuildFullName() & "</Full_Name>")
                strFieldXml.Append("<Full_Home_Address>" & BuildFullHomeAddress(SiteWithSpecialFunction) & "</Full_Home_Address>")
                strFieldXml.Append("<Full_Company_Address>" & BuildFullCompanyAddress(SiteWithSpecialFunction) & "</Full_Company_Address>")

                ' Append XML string to output string
                strOutput.Append(strFieldXml.ToString)
                strOutput.Append("</Applicant>")
                index = i
            End While

            '==================================================
            ' Process Introducer Agent details
            '==================================================
            ErrorCategory = CategoryName(4)
            index = i
            If (index < arrInputString.Length AndAlso arrInputString(index) = "I") Then
                ' Write to log file
                If WriteToLog Then
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Loading/Validating Introducer/Agent Category" & vbCrLf)
                End If

                index = index + 1

                ' Reset FullName, FullCompanyAddress 
                FullName.Reset()
                FullCompanyAddress.Reset()

                ' Get XML string for Introducer_Agent category
                strFieldXml = strFieldXml.Remove(0, strFieldXml.ToString.Length)
                strFieldXml.Append("<Organisation>" & KeyFields.Organisation & "</Organisation><Country_Code>" & KeyFields.CountryCode & "</Country_Code><Application_Number>" & KeyFields.ApplicationNumber & "</Application_Number><Application_Type>" & KeyFields.ApplicationType & "</Application_Type><AppKey>" & KeyFields.AppKey & "</AppKey>")
                strOutput.Append("<Introducer_Agent>")
                For i = index To index + IntroducerAgent(0).Count - 1
                    If ValidateIntroducerAgent(i - index, i) Then
                        If arrInputString(i).Trim <> "" Then
                            strFieldXml.Append("<" & IntroducerAgent(0)(i - index) & ">" & arrInputString(i) & "</" & IntroducerAgent(0)(i - index) & ">")
                        End If
                    Else
                        OutputString = ("<Application>" & ErrorXML.ToString & "</Application>")
                        Return True
                    End If
                Next
                ' Populate Full_Name, Full_Company_Address tags
                strFieldXml.Append("<Full_Name>" & BuildFullName() & "</Full_Name>")
                strFieldXml.Append("<Full_Company_Address>" & BuildFullCompanyAddress(SiteWithSpecialFunction) & "</Full_Company_Address>")

                ' Append XML string to output string
                strOutput.Append(strFieldXml.ToString)
                strOutput.Append("</Introducer_Agent>")
            End If

            ' Remove empty fields
            strOutput = strOutput.Replace("<Full_Name></Full_Name>", "")
            strOutput = strOutput.Replace("<Full_Home_Address></Full_Home_Address>", "")
            strOutput = strOutput.Replace("<Full_Company_Address></Full_Company_Address>", "")

            ' Set Field Error Flag
            If DiaryXML.Length > 0 Then
                strOutput = strOutput.Replace("<Field_Error_Flag></Field_Error_Flag>", "<Field_Error_Flag>1</Field_Error_Flag>")
            Else
                strOutput = strOutput.Replace("<Field_Error_Flag></Field_Error_Flag>", "<Field_Error_Flag>0</Field_Error_Flag>")
            End If

            If Database = "C" Then
                index = i
                While (index < arrInputString.Length AndAlso arrInputString(index) = "N")
                    index = index + 1
                    i = i + 1
                    DiaryXML.Append("<Diary>")
                    DiaryXML.Append("<Diary_Note>")
                    DiaryXML.Append(System.Security.SecurityElement.Escape(arrInputString(i)))
                    DiaryXML.Append("</Diary_Note>")
                    DiaryXML.Append("</Diary>")
                    i = i + 1
                    index = i
                End While
            End If

            If index > arrInputString.Length OrElse index < arrInputString.Length - 1 Then
                Throw New Exception("Invalid array lenth")
                Return True
            End If

            If ErrorXML.Length > 0 Then
                ' Write to log file
                If WriteToLog Then
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Error Code = " & ErrorCode & ", Error Field = " & ErrorField & ", Error Value = " & ErrorValue & vbCrLf)
                End If
                strOutput.Append(ErrorXML.ToString)
            End If
            If DiaryXML.Length > 0 Then
                strOutput.Append(DiaryXML.ToString)
            End If
            strOutput.Append("</Application>")
            OutputString = strOutput.ToString
            Return True
        Catch ex As Exception
            Throw ex
        Finally
            CurrentAppKey = KeyFields.AppKey
            CurrentApplicationType = KeyFields.ApplicationType
        End Try
    End Function

    Public Shared Sub SetTargetDatabase(ByVal sDatabase As String)
        Try
            Database = sDatabase
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function GetOutputString() As String
        Try
            Return OutputString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetLoadingLog() As String
        Try
            Return LoadingLog.ToString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetAppKey() As String
        Try
            Return CurrentAppKey
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetApplicationType() As String
        Try
            Return CurrentApplicationType
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function ValidateApplication(ByVal ApplicationIndex As Integer, ByVal ArrayIndex As Integer) As Boolean
        Dim len As Integer

        Try
            If Application(1)(ApplicationIndex).ToString.StartsWith("varchar") _
            OrElse Application(1)(ApplicationIndex).ToString.StartsWith("nvarchar") Then
                If Application(0)(ApplicationIndex).ToString.StartsWith("Organisation") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex))
                    KeyFields.Organisation = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    Dim i As Integer
                    If arrInputString(ArrayIndex).Trim <> "" Then
                        For i = 0 To Organisation.Count - 1
                            If Organisation(i) = arrInputString(ArrayIndex) Then
                                Exit For
                            End If
                        Next
                        If i = Organisation.Count Then                            
                            Throw New Exception("Invalid Organisation")
                            Return False
                        End If
                    End If                    
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Country_Code") Then
                    arrInputString(ArrayIndex) = LeaveAtoZ(arrInputString(ArrayIndex))
                    KeyFields.CountryCode = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    If String.Compare(arrInputString(ArrayIndex).ToString.Trim, CountryCode) <> 0 Then                       
                        Throw New Exception("Invalid Country Code")
                        Return False
                    End If
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Application_Number") Then
                    If Database = "C" AndAlso arrInputString(ArrayIndex).ToString.Trim = "" Then
                        arrInputString(ArrayIndex) = GenerateCriminalAutoNumber()
                    End If                    
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(CStr(arrInputString(ArrayIndex)).Replace(" ", ""), "/-()_" & SpecialCharacters)
                    KeyFields.ApplicationNumber = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    If arrInputString(ArrayIndex).ToString.Trim = "" Then
                        Throw New Exception("Invalid Application Number")
                        Return False
                    End If
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Application_Type") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex))
                    KeyFields.ApplicationType = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    If Database = "A" AndAlso arrInputString(ArrayIndex).ToString.Trim = "" Then
                        Throw New Exception("Invalid Application Type")
                        Return False
                    End If
                    Dim i As Integer
                    If arrInputString(ArrayIndex).Trim <> "" Then
                        For i = 0 To ApplicationType.Count - 1
                            If ApplicationType(i) = arrInputString(ArrayIndex) Then
                                Exit For
                            End If
                        Next
                        If i = ApplicationType.Count Then
                            Throw New Exception("Invalid Application Type")
                            Return False
                        End If
                    End If
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Branch") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-/" & SpecialCharacters)
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Decision_Reason") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Decision") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), " " & SpecialCharacters)
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("User_Field") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                End If
                ' Truncate string
                len = CInt(Application(1)(ApplicationIndex).ToString.Replace("nvarchar(", "").Replace("varchar(", "").Replace(")", ""))
                If arrInputString(ArrayIndex).Length > len Then
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).Substring(0, len))
                Else
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex))
                End If
            ElseIf Application(1)(ApplicationIndex).ToString.StartsWith("datetime") Then
                If Application(0)(ApplicationIndex).ToString.StartsWith("Application_Date") Then
                    Dim iDay As Integer
                    Dim iMonth As Integer
                    Dim iYear As Integer

                    Try
                        iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                        iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                        iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                        If iYear < 1900 _
                        OrElse iYear < Now.Year - ValidDatePeriod _
                        OrElse iYear > Now.Year + ValidDatePeriod Then
                            Throw New Exception("Invalid Application Date")
                            Return False
                        Else
                            If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                Throw New Exception("Invalid Application Date")
                                Return False
                            Else
                                arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                            End If
                        End If
                    Catch ex As Exception
                        Throw New Exception("Invalid Application Date")
                        Return False
                    End Try
                Else
                    If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                        If Application(0)(ApplicationIndex).ToString.StartsWith("Capture_Time") Then
                            Dim iHour As Integer
                            Dim iMinute As Integer
                            Dim iSecond As Integer

                            Try
                                iHour = CInt(arrInputString(ArrayIndex).ToString.Trim.Substring(0, 2))
                                iMinute = CInt(arrInputString(ArrayIndex).ToString.Trim.Substring(2, 2))
                                iSecond = CInt(arrInputString(ArrayIndex).ToString.Trim.Substring(4, 2))
                                If Not (IsNumeric(iHour) AndAlso IsNumeric(iMinute) AndAlso IsNumeric(iSecond)) Then
                                    Throw New Exception("Invalid Capture Time")
                                    Return False
                                ElseIf iHour < 0 OrElse iHour >= 24 OrElse iMinute < 0 OrElse iMinute >= 60 OrElse iSecond < 0 OrElse iSecond >= 60 Then
                                    Throw New Exception("Invalid Capture Time")
                                    Return False
                                Else
                                    arrInputString(ArrayIndex) = arrInputString(ArrayIndex).ToString.Trim.Substring(0, 2) & ":" & _
                                                                arrInputString(ArrayIndex).ToString.Trim.Substring(2, 2) & ":" & _
                                                                arrInputString(ArrayIndex).ToString.Trim.Substring(4, 2)
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Capture Time")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Capture_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 _
                                OrElse iYear < Now.Year - ValidDatePeriod _
                                OrElse iYear > Now.Year + ValidDatePeriod Then
                                    Throw New Exception("Invalid Capture Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Capture Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Capture Date")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Expiry_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 Then
                                    Throw New Exception("Invalid Expiry Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Expiry Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Expiry Date")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Decision_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 _
                                OrElse iYear < Now.Year - ValidDatePeriod _
                                OrElse iYear > Now.Year + ValidDatePeriod Then
                                    Throw New Exception("Invalid Decision Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Decision Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Decision Date")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Action_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 _
                                OrElse iYear < Now.Year - ValidDatePeriod _
                                OrElse iYear > Now.Year + ValidDatePeriod Then
                                    Throw New Exception("Invalid Action Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Action Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Action Date")
                                Return False
                            End Try
                        End If
                    End If
                End If
            Else
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If Application(0)(ApplicationIndex).ToString.StartsWith("Amount_Limit") Then
                        If IsNumeric(arrInputString(ArrayIndex)) Then
                            Dim dtNumericValue As Long

                            Try
                                dtNumericValue = CLng(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = CStr(dtNumericValue)
                            Catch ex As Exception
                                Throw New Exception("Invalid Amount/Limit")
                                Return False
                            End Try
                        Else
                            Throw New Exception("Invalid Amount/Limit")
                            Return False
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message & " - Application." & Application(0)(ApplicationIndex).ToString & " = " & arrInputString(ArrayIndex).ToString)
        End Try
    End Function

    Private Shared Function ValidateApplicant(ByVal ApplicantIndex As Integer, ByVal ArrayIndex As Integer) As Boolean
        Dim len As Integer

        Try
            If Applicant(1)(ApplicantIndex).ToString.StartsWith("varchar") _
            OrElse Applicant(1)(ApplicantIndex).ToString.StartsWith("nvarchar") Then
                ' No need to validate Applicant's Id Number for KCB
                If Applicant(0)(ApplicantIndex).ToString.StartsWith("Surname") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.LastName = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("First_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.FirstName = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Middle_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.MiddleName = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Sex") Then
                    If Not arrInputString(ArrayIndex).StartsWith("M") _
                    AndAlso Not arrInputString(ArrayIndex).StartsWith("F") _
                    AndAlso Not arrInputString(ArrayIndex).Trim = "" Then
                        ErrorCode = "11"
                        ErrorField = Applicant(0)(ApplicantIndex).ToString
                        ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                        arrInputString(ArrayIndex) = ""
                        BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidSex, ErrorValue)
                    End If
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Home_Address") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuoteCommaDot(arrInputString(ArrayIndex))
                    Select Case Applicant(0)(ApplicantIndex).ToString.Trim.Replace("Home_Address", "")
                        Case "1"
                            FullHomeAddress.HomeAddress1 = arrInputString(ArrayIndex)
                        Case "2"
                            FullHomeAddress.HomeAddress2 = arrInputString(ArrayIndex)
                        Case "3"
                            FullHomeAddress.HomeAddress3 = arrInputString(ArrayIndex)
                        Case "4"
                            FullHomeAddress.HomeAddress4 = arrInputString(ArrayIndex)
                        Case "5"
                            FullHomeAddress.HomeAddress5 = arrInputString(ArrayIndex)
                        Case "6"
                            FullHomeAddress.HomeAddress6 = arrInputString(ArrayIndex)
                    End Select
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Home_Postcode") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-" & SpecialCharacters)
                    FullHomeAddress.HomePostcode = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Home_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Mobile_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    arrInputString(ArrayIndex) = " " + arrInputString(ArrayIndex).Trim + " "
                    For i As Integer = 0 To CompanySuffix.Count - 1
                        arrInputString(ArrayIndex) = Microsoft.VisualBasic.Strings.Replace(arrInputString(ArrayIndex), CompanySuffix(i), " ", 1, -1, Constants.vbTextCompare)
                    Next
                    arrInputString(ArrayIndex) = arrInputString(ArrayIndex).Trim
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Address") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuoteCommaDot(arrInputString(ArrayIndex))
                    Select Case Applicant(0)(ApplicantIndex).ToString.Trim.Replace("Company_Address", "")
                        Case "1"
                            FullCompanyAddress.CompanyAddress1 = arrInputString(ArrayIndex)
                        Case "2"
                            FullCompanyAddress.CompanyAddress2 = arrInputString(ArrayIndex)
                        Case "3"
                            FullCompanyAddress.CompanyAddress3 = arrInputString(ArrayIndex)
                        Case "4"
                            FullCompanyAddress.CompanyAddress4 = arrInputString(ArrayIndex)
                        Case "5"
                            FullCompanyAddress.CompanyAddress5 = arrInputString(ArrayIndex)
                        Case "6"
                            FullCompanyAddress.CompanyAddress6 = arrInputString(ArrayIndex)
                    End Select
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Postcode") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-" & SpecialCharacters)
                    FullCompanyAddress.CompanyPostcode = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    If Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field7") Then
                        Dim i As Integer
                        For i = 0 To UserField7.Count - 1
                            If UserField7(i) = arrInputString(ArrayIndex) Then
                                Exit For
                            End If
                        Next
                        If i >= UserField7.Count AndAlso arrInputString(ArrayIndex).Trim <> "" Then
                            ErrorCode = "17"
                            ErrorField = Applicant(0)(ApplicantIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidUserField7, ErrorValue)
                        End If
                    End If
                End If
                ' Truncate string
                len = CInt(Applicant(1)(ApplicantIndex).ToString.Replace("nvarchar(", "").Replace("varchar(", "").Replace(")", ""))
                If arrInputString(ArrayIndex).Length > len Then
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).Substring(0, len))
                Else
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex))
                End If
            ElseIf Applicant(1)(ApplicantIndex).ToString.StartsWith("datetime") Then
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If Applicant(0)(ApplicantIndex).ToString.StartsWith("Date_Of_Birth") _
                    OrElse Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field") Then
                        Dim iDay As Integer
                        Dim iMonth As Integer
                        Dim iYear As Integer

                        Try
                            iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                            iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                            iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                            If iYear < 1900 _
                            OrElse iYear < Now.Year - ValidDatePeriod _
                            OrElse iYear > Now.Year + ValidDatePeriod Then
                                ErrorCode = "08"
                                ErrorField = Applicant(0)(ApplicantIndex).ToString
                                ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = ""
                                BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidDate, ErrorValue)
                            Else
                                If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                    ErrorCode = "08"
                                    ErrorField = Parameter.Applicant(0)(ApplicantIndex).ToString
                                    ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                    arrInputString(ArrayIndex) = ""
                                    BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidDate, ErrorValue)
                                Else
                                    arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                End If
                            End If
                        Catch ex As Exception
                            ErrorCode = "08"
                            ErrorField = Applicant(0)(ApplicantIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidDate, ErrorValue)
                        End Try
                    End If
                End If
            Else
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field") Then
                        If IsNumeric(arrInputString(ArrayIndex)) Then
                            Dim dtNumericValue As Long

                            Try
                                dtNumericValue = CLng(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = CStr(dtNumericValue)
                            Catch ex As Exception
                                ErrorCode = "10"
                                ErrorField = Applicant(0)(ApplicantIndex).ToString
                                ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = ""
                                BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                            End Try
                        Else
                            ErrorCode = "10"
                            ErrorField = Applicant(0)(ApplicantIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message & " - Applicant." & Applicant(0)(ApplicantIndex).ToString & " = " & arrInputString(ArrayIndex).ToString)
        End Try
    End Function

    Private Shared Function ValidateIntroducerAgent(ByVal IntroducerAgentIndex As Integer, ByVal ArrayIndex As Integer) As Boolean
        Dim len As Integer

        Try
            If IntroducerAgent(1)(IntroducerAgentIndex).ToString.StartsWith("varchar") _
            OrElse IntroducerAgent(1)(IntroducerAgentIndex).ToString.StartsWith("nvarchar") Then
                If IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Id_Number") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), SpecialCharacters)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Surname") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.LastName = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("First_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.FirstName = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Middle_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.MiddleName = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Mobile_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    arrInputString(ArrayIndex) = " " + arrInputString(ArrayIndex).Trim + " "
                    For i As Integer = 0 To CompanySuffix.Count - 1
                        arrInputString(ArrayIndex) = Microsoft.VisualBasic.Strings.Replace(arrInputString(ArrayIndex), CompanySuffix(i), " ", 1, -1, Constants.vbTextCompare)
                    Next
                    arrInputString(ArrayIndex) = arrInputString(ArrayIndex).Trim
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Address") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuoteCommaDot(arrInputString(ArrayIndex))
                    Select Case IntroducerAgent(0)(IntroducerAgentIndex).ToString.Trim.Replace("Company_Address", "")
                        Case "1"
                            FullCompanyAddress.CompanyAddress1 = arrInputString(ArrayIndex)
                        Case "2"
                            FullCompanyAddress.CompanyAddress2 = arrInputString(ArrayIndex)
                        Case "3"
                            FullCompanyAddress.CompanyAddress3 = arrInputString(ArrayIndex)
                        Case "4"
                            FullCompanyAddress.CompanyAddress4 = arrInputString(ArrayIndex)
                        Case "5"
                            FullCompanyAddress.CompanyAddress5 = arrInputString(ArrayIndex)
                        Case "6"
                            FullCompanyAddress.CompanyAddress6 = arrInputString(ArrayIndex)
                    End Select
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Postcode") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-")
                    FullCompanyAddress.CompanyPostcode = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("User_Field") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                End If
                ' Truncate string
                len = CInt(IntroducerAgent(1)(IntroducerAgentIndex).ToString.Replace("nvarchar(", "").Replace("varchar(", "").Replace(")", ""))
                If arrInputString(ArrayIndex).Length > len Then
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).Substring(0, len))
                Else
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex))
                End If
            ElseIf IntroducerAgent(1)(IntroducerAgentIndex).ToString.StartsWith("datetime") Then
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("User_Field") Then
                        Dim iDay As Integer
                        Dim iMonth As Integer
                        Dim iYear As Integer

                        Try
                            iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                            iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                            iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                            If iYear < 1900 _
                            OrElse iYear < Now.Year - ValidDatePeriod _
                            OrElse iYear > Now.Year + ValidDatePeriod Then
                                Throw New Exception("Invalid Date")
                                Return False
                            Else
                                If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                    Throw New Exception("Invalid Date")
                                    Return False
                                Else
                                    arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                End If
                            End If
                        Catch ex As Exception
                            Throw New Exception("Invalid Date")
                            Return False
                        End Try
                    End If
                End If
            Else
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("User_Field") Then
                        If IsNumeric(arrInputString(ArrayIndex)) Then
                            Dim dtNumericValue As Long

                            Try
                                dtNumericValue = CLng(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = CStr(dtNumericValue)
                            Catch ex As Exception
                                ErrorCode = "10"
                                ErrorField = IntroducerAgent(0)(IntroducerAgentIndex).ToString
                                ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = ""
                                BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                            End Try
                        Else
                            ErrorCode = "10"
                            ErrorField = IntroducerAgent(0)(IntroducerAgentIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message & " - IntroducerAgent." & IntroducerAgent(0)(IntroducerAgentIndex).ToString & " = " & arrInputString(ArrayIndex).ToString)
        End Try
    End Function

    Private Shared Function LeaveAtoZand0to9(ByVal sValidatingString As String, Optional ByVal sExtendedCharacters As String = "") As String
        Dim i, j As Integer
        Dim arrExtendedCharacters() As Char
        Dim arrValidatingString() As Char

        Try
            arrExtendedCharacters = sExtendedCharacters.ToCharArray
            arrValidatingString = sValidatingString.ToCharArray
            For i = 0 To arrValidatingString.Length - 1
                If (arrValidatingString(i) < "A" OrElse arrValidatingString(i) > "Z") _
                AndAlso (arrValidatingString(i) < "a" OrElse arrValidatingString(i) > "z") _
                AndAlso (arrValidatingString(i) < "0" OrElse arrValidatingString(i) > "9") _
                AndAlso (AscW(arrValidatingString(i)) >= 32 AndAlso AscW(arrValidatingString(i)) <= 126) Then
                    If sExtendedCharacters <> "" Then
                        For j = 0 To arrExtendedCharacters.Length - 1
                            If arrExtendedCharacters(j) = arrValidatingString(i) Then
                                Exit For
                            End If
                        Next
                        If j = arrExtendedCharacters.Length Then
                            'arrValidatingString(i) = ""
                            sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                        End If
                    Else
                        sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                    End If
                End If
            Next
            Return sValidatingString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function LeaveAtoZ(ByVal sValidatingString As String, Optional ByVal sExtendedCharacters As String = "") As String
        Dim i, j As Integer
        Dim arrEntendedCharacters() As Char
        Dim arrValidatingString() As Char

        Try
            arrEntendedCharacters = sExtendedCharacters.ToCharArray
            arrValidatingString = sValidatingString.ToCharArray
            For i = 0 To arrValidatingString.Length - 1
                If (arrValidatingString(i) < "A" OrElse arrValidatingString(i) > "Z") _
                AndAlso (arrValidatingString(i) < "a" OrElse arrValidatingString(i) > "z") _
                AndAlso (AscW(arrValidatingString(i)) >= 32 AndAlso AscW(arrValidatingString(i)) <= 126) Then
                    If sExtendedCharacters <> "" Then
                        For j = 0 To arrEntendedCharacters.Length - 1
                            If arrEntendedCharacters(j) = arrValidatingString(i) Then
                                Exit For
                            End If
                        Next
                        If j = arrEntendedCharacters.Length Then
                            'arrValidatingString(i) = ""
                            sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                        End If
                    Else
                        sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                    End If
                End If
            Next
            Return sValidatingString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function Leave0to9(ByVal sValidatingString As String, Optional ByVal sExtendedCharacters As String = "") As String
        Dim i, j As Integer
        Dim arrEntendedCharacters() As Char
        Dim arrValidatingString() As Char

        Try
            arrEntendedCharacters = sExtendedCharacters.ToCharArray
            arrValidatingString = sValidatingString.ToCharArray
            For i = 0 To arrValidatingString.Length - 1
                If (arrValidatingString(i) < "0" OrElse arrValidatingString(i) > "9") Then
                    If sExtendedCharacters <> "" Then
                        For j = 0 To arrEntendedCharacters.Length - 1
                            If arrEntendedCharacters(j) = arrValidatingString(i) Then
                                Exit For
                            End If
                        Next
                        If j = arrEntendedCharacters.Length Then
                            'arrValidatingString(i) = ""
                            sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                        End If
                    Else
                        sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                    End If
                End If
            Next
            Return sValidatingString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function RemoveSingleQuote(ByVal sValidatingString As String) As String
        Try
            Return sValidatingString.Replace("'", "").Trim
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function RemoveSingleQuoteCommaDot(ByVal sValidatingString As String) As String
        Try
            Return sValidatingString.Replace("'", "").Replace(",", "").Replace(".", "").Trim
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Sub BuildDiaryXML(ByVal sCategory As String, ByVal sField As String, ByVal sDescription As String, ByVal sValue As String)
        DiaryXML.Append("<Diary>")
        DiaryXML.Append("<Diary_Note>")
        DiaryXML.Append("Field in error was set to null. " & vbNewLine)
        DiaryXML.Append("Error Category : " & System.Security.SecurityElement.Escape(sCategory) & vbNewLine)
        DiaryXML.Append("Error Field : " & System.Security.SecurityElement.Escape(sField) & vbNewLine)
        DiaryXML.Append("Error Description : " & System.Security.SecurityElement.Escape(sDescription) & vbNewLine)
        DiaryXML.Append("Error Value : " & System.Security.SecurityElement.Escape(sValue) & vbNewLine)
        DiaryXML.Append("</Diary_Note>")
        DiaryXML.Append("</Diary>")
    End Sub

    Private Shared Sub BuildErrorXML(ByVal sCode As String, ByVal sValue As String)
        LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
        LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & "Error Code = " & sCode & ", Value = " & sValue & vbCrLf)
        LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
        ErrorXML.Append("<Errors>")
        ErrorXML.Append("<Organisation>" & KeyFields.Organisation & "</Organisation>")
        ErrorXML.Append("<Country_Code>" & KeyFields.CountryCode & "</Country_Code>")
        ErrorXML.Append("<Application_Number>" & KeyFields.ApplicationNumber & "</Application_Number>")
        ErrorXML.Append("<Application_Type>" & KeyFields.ApplicationType & "</Application_Type>")
        ErrorXML.Append("<Error_Code>" & sCode & "</Error_Code>")
        ErrorXML.Append("<Error_Value>" & System.Security.SecurityElement.Escape(sValue) & "</Error_Value>")
        ErrorXML.Append("<AppKey>" & KeyFields.AppKey & "</AppKey>")
        ErrorXML.Append("</Errors>")
    End Sub

    Private Shared Function BuildFullName() As String
        If String.Compare(NameSequence, "123") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.FirstName.Trim & " " & FullName.MiddleName.Trim & " " & FullName.LastName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "132") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.FirstName.Trim & " " & FullName.LastName.Trim & " " & FullName.MiddleName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "213") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.MiddleName.Trim & " " & FullName.FirstName.Trim & " " & FullName.LastName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "231") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.LastName.Trim & " " & FullName.FirstName.Trim & " " & FullName.MiddleName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "312") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.MiddleName.Trim & " " & FullName.LastName.Trim & " " & FullName.FirstName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "321") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.LastName.Trim & " " & FullName.MiddleName.Trim & " " & FullName.FirstName.Trim).Replace("  ", " ").Trim
        Else
            Return ""
        End If
    End Function

    Private Shared Function BuildFullHomeAddress(Optional ByVal SiteWithSpecialFunction As String = "") As String
        Dim strFullHomeAddress As String

        strFullHomeAddress = ""

        'If it is for CMBC then address is without space and postcode
        If SiteWithSpecialFunction.Trim.ToUpper = "CMBC" Then

            'This is because we are returing substring(1) at last
            strFullHomeAddress = " "

            If String.Compare(FullHomeAddress.HomeAddress1.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress1
            End If

            If String.Compare(FullHomeAddress.HomeAddress2.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress2
            End If

            If String.Compare(FullHomeAddress.HomeAddress3.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress3
            End If

            If String.Compare(FullHomeAddress.HomeAddress4.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress4
            End If

            If String.Compare(FullHomeAddress.HomeAddress5.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress5
            End If

            If String.Compare(FullHomeAddress.HomeAddress6.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress6
            End If

            'If FINANSBANK then only Address 1 to 4 in FULL ADDRESS
        ElseIf SiteWithSpecialFunction.Trim.ToUpper = "FINANSBANK" Then

            If String.Compare(FullHomeAddress.HomeAddress1.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress1
            End If

            If String.Compare(FullHomeAddress.HomeAddress2.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress2
            End If

            If String.Compare(FullHomeAddress.HomeAddress3.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress3
            End If

            If String.Compare(FullHomeAddress.HomeAddress4.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress4
            End If

        Else


            If AddressSequence.Trim.Length >= 7 Then
                Dim strData() As String = {FullHomeAddress.HomeAddress1.Trim, FullHomeAddress.HomeAddress2.Trim, FullHomeAddress.HomeAddress3.Trim, FullHomeAddress.HomeAddress4.Trim, _
                                           FullHomeAddress.HomeAddress5.Trim, FullHomeAddress.HomeAddress6.Trim, FullHomeAddress.HomePostcode.Trim}
                Dim strIndex(6) As String

                For iIndex As Integer = 0 To 6
                    strIndex(iIndex) = AddressSequence.Trim.Substring(iIndex, 1)
                Next

                For iIndex As Integer = 1 To 7
                    If (Array.IndexOf(strIndex, iIndex.ToString.Trim) >= 0) Then strFullHomeAddress = (strFullHomeAddress & " " & strData(Array.IndexOf(strIndex, iIndex.ToString.Trim))).Trim
                Next
                strFullHomeAddress = " " & strFullHomeAddress
            Else
                If String.Compare(FullHomeAddress.HomeAddress1.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress1
                End If

                If String.Compare(FullHomeAddress.HomeAddress2.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress2
                End If

                If String.Compare(FullHomeAddress.HomeAddress3.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress3
                End If

                If String.Compare(FullHomeAddress.HomeAddress4.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress4
                End If

                If String.Compare(FullHomeAddress.HomeAddress5.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress5
                End If

                If String.Compare(FullHomeAddress.HomeAddress6.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress6
                End If

                If String.Compare(FullHomeAddress.HomePostcode.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomePostcode
                End If
            End If

        End If

        If strFullHomeAddress.Length > 0 Then
            Return System.Security.SecurityElement.Escape(strFullHomeAddress.Substring(1))
        Else
            Return ""
        End If
    End Function

    Private Shared Function BuildFullCompanyAddress(Optional ByVal SiteWithSpecialFunction As String = "") As String
        Dim strFullCompanyAddress As String

        strFullCompanyAddress = ""

        'If it is for CMBC then address is without space and postcode
        If SiteWithSpecialFunction.Trim.ToUpper = "CMBC" Then

            'This is because we are returing substring(1) at last
            strFullCompanyAddress = " "

            If String.Compare(FullCompanyAddress.CompanyAddress1.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress1
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress2.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress2
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress3.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress3
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress4.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress4
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress5.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress5
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress6.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress6
            End If

            'If FINANSBANK then only Address 1 to 4 in FULL ADDRESS
        ElseIf SiteWithSpecialFunction.Trim.ToUpper = "FINANSBANK" Then

            If String.Compare(FullCompanyAddress.CompanyAddress1.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress1
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress2.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress2
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress3.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress3
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress4.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress4
            End If

        Else

            If AddressSequence.Trim.Length >= 7 Then
                Dim strData() As String = {FullCompanyAddress.CompanyAddress1.Trim, FullCompanyAddress.CompanyAddress2.Trim, FullCompanyAddress.CompanyAddress3.Trim, FullCompanyAddress.CompanyAddress4.Trim, _
                                           FullCompanyAddress.CompanyAddress5.Trim, FullCompanyAddress.CompanyAddress6.Trim, FullCompanyAddress.CompanyPostcode.Trim}
                Dim strIndex(6) As String

                For iIndex As Integer = 0 To 6
                    strIndex(iIndex) = AddressSequence.Trim.Substring(iIndex, 1)
                Next

                For iIndex As Integer = 1 To 7
                    If (Array.IndexOf(strIndex, iIndex.ToString.Trim) >= 0) Then strFullCompanyAddress = (strFullCompanyAddress & " " & strData(Array.IndexOf(strIndex, iIndex.ToString.Trim))).Trim
                Next
                strFullCompanyAddress = " " & strFullCompanyAddress
            Else
                If String.Compare(FullCompanyAddress.CompanyAddress1.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress1
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress2.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress2
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress3.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress3
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress4.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress4
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress5.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress5
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress6.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress6
                End If

                If String.Compare(FullCompanyAddress.CompanyPostcode.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyPostcode
                End If
            End If

        End If

        If strFullCompanyAddress.Length > 0 Then
            Return System.Security.SecurityElement.Escape(strFullCompanyAddress.Substring(1))
        Else
            Return ""
        End If
    End Function

    Public Shared Function GenerateCriminalAutoNumber() As String
        Dim cmd As SqlCommand
        Dim daData As SqlDataAdapter
        Dim dsData As DataSet
        Dim strGeneratedApplicationNumber As String

        Try
            conn = New SqlConnection(strConn)
            conn.Open()

            dsData = New DataSet

            cmd = New SqlCommand("USP_Criminal_AutoNumber_Generate", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)(0)) Then
                strGeneratedApplicationNumber = CStr(dsData.Tables(0).Rows(0)(0))
            Else
                strGeneratedApplicationNumber = ""
            End If
            conn.Close()

            Return strGeneratedApplicationNumber
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class

Public Class SuspiciousFileConversion
    ' Variables
    Private Shared strOutputDirectory As String
    Private Shared strConn As String
    Private Shared conn As SqlConnection
    Private Shared Database As String
    Private Shared Delimiter As String
    Private Shared AddressSequence As String
    Private Shared SpecialCharacters As String
    Private Shared CountryCode As String
    Private Shared ErrorCode As String
    Private Shared ErrorCategory As String
    Private Shared ErrorField As String
    Private Shared ErrorValue As String
    Private Shared ErrorXML As StringBuilder
    Private Shared DiaryXML As StringBuilder
    Private Shared arrInputString() As String
    Private Shared OutputString As String
    Private Shared CurrentAppKey As String
    Private Shared CurrentApplicationType As String
    Private Shared Organisation As ArrayList
    Private Shared ApplicationType As ArrayList
    Private Shared UserField7 As ArrayList
    Private Shared CategoryName As ArrayList
    Private Shared NameSequence As String
    Private Shared CompanySuffix As ArrayList
    Private Shared ValidDatePeriod As Integer
    Private Shared Application(1) As ArrayList
    Private Shared Applicant(1) As ArrayList
    Private Shared IntroducerAgent(1) As ArrayList
    Private Shared WriteToLog As Boolean
    Private Shared LoadingLog As StringBuilder

    ' Class
    Private Class ErrorDescription
        Public Shared InvalidDate As String = "An invalid date was detected."
        Public Shared InvalidTime As String = "An invalid time was detected."
        Public Shared InvalidAmountLimit As String = "Amount/Limit contained an invalid integer value."
        Public Shared InvalidDecisionDate As String = "Decision Date was invalid."
        Public Shared InvalidSex As String = "An invalid sex value was detected."
        Public Shared InvalidUserField7 As String = "Applicant User Field 7 was invalid."
        Public Shared InvalidNumber As String = "An invalid integer value was detected."
    End Class

    Private Class KeyFields
        Public Shared Organisation As String = ""
        Public Shared CountryCode As String = ""
        Public Shared ApplicationNumber As String = ""
        Public Shared ApplicationType As String = ""
        Public Shared AppKey As String = ""

        Public Shared Sub Reset()
            Organisation = ""
            CountryCode = ""
            ApplicationNumber = ""
            ApplicationType = ""
            AppKey = ""
        End Sub
    End Class

    Private Class FullName
        Public Shared FirstName As String = ""
        Public Shared MiddleName As String = ""
        Public Shared LastName As String = ""

        Public Shared Sub Reset()
            FirstName = ""
            MiddleName = ""
            LastName = ""
        End Sub
    End Class

    Private Class FullHomeAddress
        Public Shared HomeAddress1 As String = ""
        Public Shared HomeAddress2 As String = ""
        Public Shared HomeAddress3 As String = ""
        Public Shared HomeAddress4 As String = ""
        Public Shared HomeAddress5 As String = ""
        Public Shared HomeAddress6 As String = ""
        Public Shared HomePostcode As String = ""

        Public Shared Sub Reset()
            HomeAddress1 = ""
            HomeAddress2 = ""
            HomeAddress3 = ""
            HomeAddress4 = ""
            HomeAddress5 = ""
            HomeAddress6 = ""
            HomePostcode = ""
        End Sub
    End Class

    Private Class FullCompanyAddress
        Public Shared CompanyAddress1 As String = ""
        Public Shared CompanyAddress2 As String = ""
        Public Shared CompanyAddress3 As String = ""
        Public Shared CompanyAddress4 As String = ""
        Public Shared CompanyAddress5 As String = ""
        Public Shared CompanyAddress6 As String = ""
        Public Shared CompanyPostcode As String = ""

        Public Shared Sub Reset()
            CompanyAddress1 = ""
            CompanyAddress2 = ""
            CompanyAddress3 = ""
            CompanyAddress4 = ""
            CompanyAddress5 = ""
            CompanyAddress6 = ""
            CompanyPostcode = ""
        End Sub
    End Class

    Public Shared Sub Initialize(ByVal connection As String, ByVal sCountryCode As String, ByVal sDelimiter As String, ByVal sDatabase As String, ByVal bWriteToLog As Boolean)
        Dim cmd As SqlCommand
        Dim dsData As DataSet
        Dim daData As SqlDataAdapter
        Dim drData As DataRow

        Try
            strConn = connection
            conn = New SqlConnection(connection)
            conn.Open()
            Database = sDatabase
            Delimiter = sDelimiter.Trim
            CountryCode = sCountryCode
            Organisation = New ArrayList
            ApplicationType = New ArrayList
            CompanySuffix = New ArrayList
            UserField7 = New ArrayList
            CategoryName = New ArrayList
            Application(0) = New ArrayList
            Application(1) = New ArrayList
            Applicant(0) = New ArrayList
            Applicant(1) = New ArrayList
            IntroducerAgent(0) = New ArrayList
            IntroducerAgent(1) = New ArrayList
            WriteToLog = bWriteToLog

            ' Get sequence number for First Name
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Name Sequence (First Name)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                NameSequence = NameSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                NameSequence = NameSequence & "1"
            End If

            ' Get sequence number for Middle Name
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Name Sequence (Middle Name)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                NameSequence = NameSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                NameSequence = NameSequence & "2"
            End If

            ' Get sequence number for Surname
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Name Sequence (Surname)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                NameSequence = NameSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                NameSequence = NameSequence & "3"
            End If

            AddressSequence = String.Empty
            ' Get sequence number for Address1
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 1)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "1"
            End If

            ' Get sequence number for Address2
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 2)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "2"
            End If

            ' Get sequence number for Address3
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 3)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "3"
            End If

            ' Get sequence number for Address4
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 4)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "4"
            End If

            ' Get sequence number for Address5
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 5)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "5"
            End If

            ' Get sequence number for Address6
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Address 6)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "6"
            End If

            ' Get sequence number for Postcode
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Full Address Sequence (Postcode)"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 AndAlso dsData.Tables(0).Rows.Count > 0 AndAlso Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                AddressSequence = AddressSequence & CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                AddressSequence = AddressSequence & "7"
            End If

            ' Special Characters
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Special Characters"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                SpecialCharacters = CStr(dsData.Tables(0).Rows(0)("Default"))
            Else
                SpecialCharacters = ""
            End If

            ' Organisation
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_Organisation_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Organisation_Code")) Then
                    Organisation.Add(CStr(drData("Organisation_Code")))
                End If
            Next

            ' Application Type
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_ApplicationTypes_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Application_Code")) Then
                    ApplicationType.Add(CStr(drData("Application_Code")))
                End If
            Next

            ' Company Suffix
            dsData = New DataSet
            cmd = New SqlCommand("USP_Definitions_CompanySuffix_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Company Suffix")) Then
                    CompanySuffix.Add(drData("Company Suffix"))
                End If
            Next

            ' User Field 7 (Nature of Fraud)
            dsData = New DataSet
            cmd = New SqlCommand("USP_Definitions_LookUpList_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Code")) Then
                    UserField7.Add(drData("Code"))
                End If
            Next

            ' Valid Date Period
            dsData = New DataSet
            cmd = New SqlCommand("USP_InstinctParameters_ParameterName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@Parameter_Name", SqlDbType.NVarChar, 50)
            cmd.Parameters(0).Value = "Valid Date Period"

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)("Default")) Then
                ValidDatePeriod = CInt(dsData.Tables(0).Rows(0)("Default"))
            Else
                ValidDatePeriod = 150
            End If

            ' Category Name
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_CategoryName_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Name")) Then
                    CategoryName.Add(drData("Category_Name"))
                End If
            Next

            '**************************************************
            '* Application
            '**************************************************
            ' Base(3) - Reporter Code
            Application(0).Add("Organisation")
            Application(1).Add("nvarchar(3)")
            ' No mapping for Country Code
            Application(0).Add("Country_Code")
            Application(1).Add("nvarchar(2)")
            ' Base(10) - Application No
            Application(0).Add("Application_Number")
            Application(1).Add("nvarchar(25)")
            ' Header(5) - Capture Date
            Application(0).Add("Capture_Date")
            Application(1).Add("datetime(8)")
            ' Segment(6) - Decision(Y/N)
            Application(0).Add("Decision")
            Application(1).Add("nvarchar(10)")
            ' Segment(9) - Product Code
            Application(0).Add("Application_Type")
            Application(1).Add("nvarchar(4)")
            ' Segment(10) - Amount of the application/Amount of the loan
            Application(0).Add("Amount_Limit")
            Application(1).Add("bigint(8)")
            ' Segment(11) - Date of application/Date opened
            Application(0).Add("Application_Date")
            Application(1).Add("datetime(8)")
            ' Segment(31) - Action Taken
            Application(0).Add("Action_Taken")
            Application(1).Add("nvarchar(1)")
            ' Segment(28) - Fraud Transaction Reason Code
            ' Segment(32) - Reason Code for Closure
            ' Segment(34) - Reason Code for Deletion
            Application(0).Add("Decision_Reason")
            Application(1).Add("nvarchar(201)")
            ' Segment(29) - Date Fraud Confirmed
            ' Segment(33) - Date Closed
            ' Segment(35) - Date Deleted
            Application(0).Add("Action_Date")
            Application(1).Add("datetime(8)")
            ' Base(3) - Reporter Code
            Application(0).Add("User_Field1")
            Application(1).Add("nvarchar(50)")
            ' Base(3) - Consumer Account No
            Application(0).Add("User_Field3")
            Application(1).Add("nvarchar(50)")
            ' Segment(2) - Branch Code
            Application(0).Add("User_Field4")
            Application(1).Add("nvarchar(50)")
            ' Segment(3) - Branch Name
            Application(0).Add("User_Field5")
            Application(1).Add("nvarchar(50)")
            ' Base(2) - Serial No
            Application(0).Add("User_Field6")
            Application(1).Add("nvarchar(50)")
            ' Base(4) - Contract Subject Code
            Application(0).Add("User_Field7")
            Application(1).Add("nvarchar(50)")
            ' Segment(4) - Reporting Reason Code
            Application(0).Add("User_Field8")
            Application(1).Add("nvarchar(50)")
            ' Segment(5) - Activity Date for Reporting
            Application(0).Add("User_Field9")
            Application(1).Add("nvarchar(50)")
            ' Segment(12) - Credit(Y/N)
            Application(0).Add("User_Field10")
            Application(1).Add("nvarchar(50)")
            ' Segment(13) - Collateral(Y/N)
            Application(0).Add("User_Field11")
            Application(1).Add("nvarchar(50)")
            ' Segment(14) - Guarantor(Y/N)
            Application(0).Add("User_Field12")
            Application(1).Add("nvarchar(50)")

            '**************************************************
            '* Applicant
            '**************************************************
            ' Base(5) - Resident Registration No.
            Applicant(0).Add("Id_Number1")
            Applicant(1).Add("nvarchar(30)")
            ' Base(6) - Business Owner No.
            Applicant(0).Add("Id_Number2")
            Applicant(1).Add("nvarchar(21)")
            ' Base(7) - Corporation No.
            Applicant(0).Add("Id_Number3")
            Applicant(1).Add("nvarchar(21)")
            ' Base(8) - Name
            Applicant(0).Add("Surname")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(19) - Home Address
            Applicant(0).Add("Home_Address1")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Home_Address2")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Home_Address3")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(18) - Home Postal Code
            Applicant(0).Add("Home_Postcode")
            Applicant(1).Add("nvarchar(10)")
            ' Segment(20) - Home Phone Number
            Applicant(0).Add("Home_Phone_Number")
            Applicant(1).Add("nvarchar(32)")
            ' Segment(16) - Mobile Phone No
            Applicant(0).Add("Mobile_Phone_Number")
            Applicant(1).Add("nvarchar(32)")
            ' Segment(24) - Employer Name/Company Name
            Applicant(0).Add("Company_Name")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(22) - Business Address
            Applicant(0).Add("Company_Address1")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Company_Address2")
            Applicant(1).Add("nvarchar(70)")
            Applicant(0).Add("Company_Address3")
            Applicant(1).Add("nvarchar(70)")
            ' Segment(21) - Business Postal Code
            Applicant(0).Add("Company_Postcode")
            Applicant(1).Add("nvarchar(10)")
            ' Segment(23) - Business Phone Number
            Applicant(0).Add("Company_Phone_Number")
            Applicant(1).Add("nvarchar(32)")
            ' Segment(15) - Mobile Communication Code
            Applicant(0).Add("User_Field2")
            Applicant(1).Add("nvarchar(30)")
            ' Segment(25) - Job Title
            Applicant(0).Add("User_Field3")
            Applicant(1).Add("nvarchar(30)")
            ' Segment(17) - Email Address
            Applicant(0).Add("User_Field8")
            Applicant(1).Add("nvarchar(50)")
            ' Segment(26) - Income
            Applicant(0).Add("User_Field10")
            Applicant(1).Add("bigint(8)")
            ' Segment(27) - Property Tax
            Applicant(0).Add("User_Field11")
            Applicant(1).Add("bigint(8)")

            '**************************************************
            '* Introducer Agent
            '**************************************************            
            ' Segment(8) - Introducer Registration No.
            IntroducerAgent(0).Add("Id_Number1")
            IntroducerAgent(1).Add("nvarchar(21)")
            ' Segment(7) - Channel Code
            IntroducerAgent(0).Add("User_Field1")
            IntroducerAgent(1).Add("nvarchar(30)")

            conn.Close()
        Catch ex As Exception
            Throw ex
        Finally
            conn.Dispose()
        End Try
    End Sub

    Public Shared Function ConvertToXml(ByVal sInputString As String) As Boolean
        Dim ErrorCode As Integer
        Dim strOutput As StringBuilder
        Dim index, i, recordId As Integer
        Dim strFieldXml As StringBuilder

        Try
            ErrorCode = 0
            OutputString = ""
            ErrorXML = New StringBuilder
            DiaryXML = New StringBuilder
            strOutput = New StringBuilder
            strFieldXml = New StringBuilder
            LoadingLog = New StringBuilder
            KeyFields.Reset()

            ' Initialize                         
            strOutput.Append("<Application>")
            arrInputString = sInputString.Split(Delimiter)

            '================================================== 
            ' Process Application details
            '==================================================
            ' Write to log file
            If WriteToLog Then
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Loading/Validating Application Category" & vbCrLf)
            End If
            ErrorCategory = CategoryName(0)
            ' Get XML string for Application category            
            index = 0
            For i = index To index + Application(0).Count - 1
                ' Remove invalid characters & check errors
                If ValidateApplication(i - index, i) Then
                    ' Insert values to XML string
                    If String.Compare(Application(0)(i - index), "Application_Type") = 0 Then
                        strFieldXml.Append("<" & Application(0)(i - index) & ">" & arrInputString(i) & "</" & Application(0)(i - index) & ">")
                    Else
                        If arrInputString(i).Trim <> "" Then
                            strFieldXml.Append("<" & Application(0)(i - index) & ">" & arrInputString(i) & "</" & Application(0)(i - index) & ">")
                        End If
                    End If

                Else
                    OutputString = ("<Application>" & ErrorXML.ToString & "</Application>")
                    Return True
                End If
            Next
            strFieldXml.Append("<AppKey>" & KeyFields.AppKey & "</AppKey>")
            strFieldXml.Append("<Field_Error_Flag></Field_Error_Flag>")
            ' Append XML string to output string
            strOutput.Append(strFieldXml.ToString)

            ' Write to log file
            If WriteToLog Then
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Organisation = " & KeyFields.Organisation & ", Country Code = " & KeyFields.CountryCode & ", Application Number = " & KeyFields.ApplicationNumber & ", Application Type = " & KeyFields.ApplicationType & vbCrLf)
            End If

            '==================================================
            ' Process Applicant details
            '==================================================
            ErrorCategory = CategoryName(1)
            index = i
            recordId = 0
            While (index < arrInputString.Length AndAlso arrInputString(index) = "A")
                ' Write to log file
                If recordId = 0 AndAlso WriteToLog Then
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Loading/Validating Applicant Category" & vbCrLf)
                End If

                index = index + 1
                recordId = recordId + 1

                ' Reset FullName, FullHomeAddress,FullCompanyAddress 
                FullName.Reset()
                FullHomeAddress.Reset()
                FullCompanyAddress.Reset()

                ' Get XML string for Applicant category
                strFieldXml = strFieldXml.Remove(0, strFieldXml.ToString.Length)
                strFieldXml.Append("<Organisation>" & KeyFields.Organisation & "</Organisation><Country_Code>" & KeyFields.CountryCode & "</Country_Code><Application_Number>" & KeyFields.ApplicationNumber & "</Application_Number><Application_Type>" & KeyFields.ApplicationType & "</Application_Type><Sequence_Number>" & CStr(recordId) & "</Sequence_Number><Income_Increase_Percentage>0</Income_Increase_Percentage><AppKey>" & KeyFields.AppKey & "</AppKey>")
                strOutput.Append("<Applicant>")
                For i = index To index + Applicant(0).Count - 1
                    If ValidateApplicant(i - index, i) Then
                        If arrInputString(i).Trim <> "" Then
                            strFieldXml.Append("<" & Applicant(0)(i - index) & ">" & arrInputString(i) & "</" & Applicant(0)(i - index) & ">")
                        End If
                    Else
                        OutputString = ("<Application>" & ErrorXML.ToString & "</Application>")
                        Return True
                    End If
                Next
                ' Populate Full_Name, Full_Home_Address, Full_Company_Address tags
                strFieldXml.Append("<Full_Name>" & BuildFullName() & "</Full_Name>")
                strFieldXml.Append("<Full_Home_Address>" & BuildFullHomeAddress() & "</Full_Home_Address>")
                strFieldXml.Append("<Full_Company_Address>" & BuildFullCompanyAddress() & "</Full_Company_Address>")

                ' Append XML string to output string
                strOutput.Append(strFieldXml.ToString)
                strOutput.Append("</Applicant>")
                index = i
            End While

            '==================================================
            ' Process Introducer Agent details
            '==================================================
            ErrorCategory = CategoryName(4)
            index = i
            If (index < arrInputString.Length AndAlso arrInputString(index) = "I") Then
                ' Write to log file
                If WriteToLog Then
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Loading/Validating Introducer/Agent Category" & vbCrLf)
                End If

                index = index + 1

                ' Reset FullName, FullCompanyAddress 
                FullName.Reset()
                FullCompanyAddress.Reset()

                ' Get XML string for Introducer_Agent category
                strFieldXml = strFieldXml.Remove(0, strFieldXml.ToString.Length)
                strFieldXml.Append("<Organisation>" & KeyFields.Organisation & "</Organisation><Country_Code>" & KeyFields.CountryCode & "</Country_Code><Application_Number>" & KeyFields.ApplicationNumber & "</Application_Number><Application_Type>" & KeyFields.ApplicationType & "</Application_Type><AppKey>" & KeyFields.AppKey & "</AppKey>")
                strOutput.Append("<Introducer_Agent>")
                For i = index To index + IntroducerAgent(0).Count - 1
                    If ValidateIntroducerAgent(i - index, i) Then
                        If arrInputString(i).Trim <> "" Then
                            strFieldXml.Append("<" & IntroducerAgent(0)(i - index) & ">" & arrInputString(i) & "</" & IntroducerAgent(0)(i - index) & ">")
                        End If
                    Else
                        OutputString = ("<Application>" & ErrorXML.ToString & "</Application>")
                        Return True
                    End If
                Next
                ' Populate Full_Name, Full_Company_Address tags
                strFieldXml.Append("<Full_Name>" & BuildFullName() & "</Full_Name>")
                strFieldXml.Append("<Full_Company_Address>" & BuildFullCompanyAddress() & "</Full_Company_Address>")

                ' Append XML string to output string
                strOutput.Append(strFieldXml.ToString)
                strOutput.Append("</Introducer_Agent>")
            End If

            ' Remove empty fields
            strOutput = strOutput.Replace("<Full_Name></Full_Name>", "")
            strOutput = strOutput.Replace("<Full_Home_Address></Full_Home_Address>", "")
            strOutput = strOutput.Replace("<Full_Company_Address></Full_Company_Address>", "")

            ' Set Field Error Flag
            If DiaryXML.Length > 0 Then
                strOutput = strOutput.Replace("<Field_Error_Flag></Field_Error_Flag>", "<Field_Error_Flag>1</Field_Error_Flag>")
            Else
                strOutput = strOutput.Replace("<Field_Error_Flag></Field_Error_Flag>", "<Field_Error_Flag>0</Field_Error_Flag>")
            End If

            If Database = "C" Then
                index = i
                While (index < arrInputString.Length AndAlso arrInputString(index) = "N")
                    index = index + 1
                    i = i + 1
                    DiaryXML.Append("<Diary>")
                    DiaryXML.Append("<Diary_Note>")
                    DiaryXML.Append(System.Security.SecurityElement.Escape(arrInputString(i)))
                    DiaryXML.Append("</Diary_Note>")
                    DiaryXML.Append("</Diary>")
                    i = i + 1
                    index = i
                End While
            End If

            If index > arrInputString.Length OrElse index < arrInputString.Length - 1 Then
                Throw New Exception("Invalid array length")
                Return True
            End If

            If ErrorXML.Length > 0 Then
                ' Write to log file
                If WriteToLog Then
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
                    LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - Error Code = " & ErrorCode & ", Error Field = " & ErrorField & ", Error Value = " & ErrorValue & vbCrLf)
                End If
                strOutput.Append(ErrorXML.ToString)
            End If
            If DiaryXML.Length > 0 Then
                strOutput.Append(DiaryXML.ToString)
            End If
            strOutput.Append("</Application>")
            OutputString = strOutput.ToString
            Return True
        Catch ex As Exception
            Throw ex
            Return True
        Finally
            CurrentAppKey = KeyFields.AppKey
            CurrentApplicationType = KeyFields.ApplicationType
        End Try
    End Function

    Public Shared Sub SetTargetDatabase(ByVal sDatabase As String)
        Try
            Database = sDatabase
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function GetOutputString() As String
        Try
            Return OutputString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetLoadingLog() As String
        Try
            Return LoadingLog.ToString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetAppKey() As String
        Try
            Return CurrentAppKey
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetApplicationType() As String
        Try
            Return CurrentApplicationType
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function ValidateApplication(ByVal ApplicationIndex As Integer, ByVal ArrayIndex As Integer) As Boolean
        Dim len As Integer

        Try
            If Application(1)(ApplicationIndex).ToString.StartsWith("varchar") _
            OrElse Application(1)(ApplicationIndex).ToString.StartsWith("nvarchar") Then
                If Application(0)(ApplicationIndex).ToString.StartsWith("Organisation") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex))
                    KeyFields.Organisation = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    Dim i As Integer
                    If arrInputString(ArrayIndex).Trim <> "" Then
                        For i = 0 To Organisation.Count - 1
                            If Organisation(i) = arrInputString(ArrayIndex) Then
                                Exit For
                            End If
                        Next
                        If i = Organisation.Count Then
                            Throw New Exception("Invalid Organisation")
                            Return False
                        End If
                    End If
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Country_Code") Then
                    arrInputString(ArrayIndex) = LeaveAtoZ(arrInputString(ArrayIndex))
                    KeyFields.CountryCode = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    If String.Compare(arrInputString(ArrayIndex).ToString.Trim, CountryCode) <> 0 Then
                        Throw New Exception("Invalid Country Code")
                        Return False
                    End If
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Application_Number") Then
                    If Database = "C" AndAlso arrInputString(ArrayIndex).ToString.Trim = "" Then
                        arrInputString(ArrayIndex) = GenerateCriminalAutoNumber()
                    End If
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(CStr(arrInputString(ArrayIndex)).Replace(" ", ""), "/-()_" & SpecialCharacters)
                    KeyFields.ApplicationNumber = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    If arrInputString(ArrayIndex).ToString.Trim = "" Then
                        Throw New Exception("Invalid Application Number")
                        Return False
                    End If
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Application_Type") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex))
                    KeyFields.ApplicationType = arrInputString(ArrayIndex)
                    KeyFields.AppKey = KeyFields.Organisation & KeyFields.CountryCode & KeyFields.ApplicationNumber & KeyFields.ApplicationType
                    If Database = "A" AndAlso arrInputString(ArrayIndex).ToString.Trim = "" Then
                        Throw New Exception("Invalid Application Type")
                        Return False
                    End If
                    Dim i As Integer
                    If arrInputString(ArrayIndex).Trim <> "" Then
                        For i = 0 To ApplicationType.Count - 1
                            If ApplicationType(i) = arrInputString(ArrayIndex) Then
                                Exit For
                            End If
                        Next
                        If i = ApplicationType.Count Then
                            Throw New Exception("Invalid Application Type")
                            Return False
                        End If
                    End If
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Branch") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-/" & SpecialCharacters)
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Decision_Reason") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Decision") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), " " & SpecialCharacters)
                ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("User_Field") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                End If
                ' Truncate string
                len = CInt(Application(1)(ApplicationIndex).ToString.Replace("nvarchar(", "").Replace("varchar(", "").Replace(")", ""))
                If arrInputString(ArrayIndex).Length > len Then
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).Substring(0, len))
                Else
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex))
                End If
            ElseIf Application(1)(ApplicationIndex).ToString.StartsWith("datetime") Then
                If Application(0)(ApplicationIndex).ToString.StartsWith("Application_Date") Then
                    Dim iDay As Integer
                    Dim iMonth As Integer
                    Dim iYear As Integer

                    Try
                        iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                        iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                        iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                        If iYear < 1900 _
                        OrElse iYear < Now.Year - ValidDatePeriod _
                        OrElse iYear > Now.Year + ValidDatePeriod Then
                            Throw New Exception("Invalid Application Date")
                            Return False
                        Else
                            If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                Throw New Exception("Invalid Application Date")
                                Return False
                            Else
                                arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                            End If
                        End If
                    Catch ex As Exception
                        Throw New Exception("Invalid Application Date")
                        Return False
                    End Try
                Else
                    If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                        If Application(0)(ApplicationIndex).ToString.StartsWith("Capture_Time") Then
                            Dim iHour As Integer
                            Dim iMinute As Integer
                            Dim iSecond As Integer

                            Try
                                iHour = CInt(arrInputString(ArrayIndex).ToString.Trim.Substring(0, 2))
                                iMinute = CInt(arrInputString(ArrayIndex).ToString.Trim.Substring(2, 2))
                                iSecond = CInt(arrInputString(ArrayIndex).ToString.Trim.Substring(4, 2))
                                If Not (IsNumeric(iHour) AndAlso IsNumeric(iMinute) AndAlso IsNumeric(iSecond)) Then
                                    Throw New Exception("Invalid Capture Time")
                                    Return False
                                ElseIf iHour < 0 OrElse iHour >= 24 OrElse iMinute < 0 OrElse iMinute >= 60 OrElse iSecond < 0 OrElse iSecond >= 60 Then
                                    Throw New Exception("Invalid Capture Time")
                                    Return False
                                Else
                                    arrInputString(ArrayIndex) = arrInputString(ArrayIndex).ToString.Trim.Substring(0, 2) & ":" & _
                                                                arrInputString(ArrayIndex).ToString.Trim.Substring(2, 2) & ":" & _
                                                                arrInputString(ArrayIndex).ToString.Trim.Substring(4, 2)
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Capture Time")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Capture_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 _
                                OrElse iYear < Now.Year - ValidDatePeriod _
                                OrElse iYear > Now.Year + ValidDatePeriod Then
                                    Throw New Exception("Invalid Capture Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Capture Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Capture Date")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Expiry_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 Then
                                    Throw New Exception("Invalid Expiry Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Expiry Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Expiry Date")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Decision_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 _
                                OrElse iYear < Now.Year - ValidDatePeriod _
                                OrElse iYear > Now.Year + ValidDatePeriod Then
                                    Throw New Exception("Invalid Decision Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Decision Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Decision Date")
                                Return False
                            End Try
                        ElseIf Application(0)(ApplicationIndex).ToString.StartsWith("Action_Date") Then
                            Dim iDay As Integer
                            Dim iMonth As Integer
                            Dim iYear As Integer

                            Try
                                iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                                iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                                iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                                If iYear < 1900 _
                                OrElse iYear < Now.Year - ValidDatePeriod _
                                OrElse iYear > Now.Year + ValidDatePeriod Then
                                    Throw New Exception("Invalid Action Date")
                                    Return False
                                Else
                                    If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                        Throw New Exception("Invalid Action Date")
                                        Return False
                                    Else
                                        arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                    End If
                                End If
                            Catch ex As Exception
                                Throw New Exception("Invalid Action Date")
                                Return False
                            End Try
                        End If
                    End If
                End If
            Else
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If Application(0)(ApplicationIndex).ToString.StartsWith("Amount_Limit") Then
                        If IsNumeric(arrInputString(ArrayIndex)) Then
                            Dim dtNumericValue As Long

                            Try
                                dtNumericValue = CLng(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = CStr(dtNumericValue)
                            Catch ex As Exception
                                Throw New Exception("Invalid Amount/Limit")
                                Return False
                            End Try
                        Else
                            Throw New Exception("Invalid Amount/Limit")
                            Return False
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message & " - Application." & Application(0)(ApplicationIndex).ToString & " = " & arrInputString(ArrayIndex).ToString)
        End Try
    End Function

    Private Shared Function ValidateApplicant(ByVal ApplicantIndex As Integer, ByVal ArrayIndex As Integer) As Boolean
        Dim len As Integer

        Try
            If Applicant(1)(ApplicantIndex).ToString.StartsWith("varchar") _
            OrElse Applicant(1)(ApplicantIndex).ToString.StartsWith("nvarchar") Then
                ' No need to validate Applicant's Id Number for KCB
                If Applicant(0)(ApplicantIndex).ToString.StartsWith("Surname") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.LastName = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("First_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.FirstName = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Middle_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.MiddleName = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Sex") Then
                    If Not arrInputString(ArrayIndex).StartsWith("M") _
                    AndAlso Not arrInputString(ArrayIndex).StartsWith("F") _
                    AndAlso Not arrInputString(ArrayIndex).Trim = "" Then
                        ErrorCode = "11"
                        ErrorField = Applicant(0)(ApplicantIndex).ToString
                        ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                        arrInputString(ArrayIndex) = ""
                        BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidSex, ErrorValue)
                    End If
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Home_Address") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuoteCommaDot(arrInputString(ArrayIndex))
                    Select Case Applicant(0)(ApplicantIndex).ToString.Trim.Replace("Home_Address", "")
                        Case "1"
                            FullHomeAddress.HomeAddress1 = arrInputString(ArrayIndex)
                        Case "2"
                            FullHomeAddress.HomeAddress2 = arrInputString(ArrayIndex)
                        Case "3"
                            FullHomeAddress.HomeAddress3 = arrInputString(ArrayIndex)
                        Case "4"
                            FullHomeAddress.HomeAddress4 = arrInputString(ArrayIndex)
                        Case "5"
                            FullHomeAddress.HomeAddress5 = arrInputString(ArrayIndex)
                        Case "6"
                            FullHomeAddress.HomeAddress6 = arrInputString(ArrayIndex)
                    End Select
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Home_Postcode") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-" & SpecialCharacters)
                    FullHomeAddress.HomePostcode = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Home_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Mobile_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    arrInputString(ArrayIndex) = " " + arrInputString(ArrayIndex).Trim + " "
                    For i As Integer = 0 To CompanySuffix.Count - 1
                        arrInputString(ArrayIndex) = Microsoft.VisualBasic.Strings.Replace(arrInputString(ArrayIndex), CompanySuffix(i), " ", 1, -1, Constants.vbTextCompare)
                    Next
                    arrInputString(ArrayIndex) = arrInputString(ArrayIndex).Trim
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Address") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuoteCommaDot(arrInputString(ArrayIndex))
                    Select Case Applicant(0)(ApplicantIndex).ToString.Trim.Replace("Company_Address", "")
                        Case "1"
                            FullCompanyAddress.CompanyAddress1 = arrInputString(ArrayIndex)
                        Case "2"
                            FullCompanyAddress.CompanyAddress2 = arrInputString(ArrayIndex)
                        Case "3"
                            FullCompanyAddress.CompanyAddress3 = arrInputString(ArrayIndex)
                        Case "4"
                            FullCompanyAddress.CompanyAddress4 = arrInputString(ArrayIndex)
                        Case "5"
                            FullCompanyAddress.CompanyAddress5 = arrInputString(ArrayIndex)
                        Case "6"
                            FullCompanyAddress.CompanyAddress6 = arrInputString(ArrayIndex)
                    End Select
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Postcode") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-" & SpecialCharacters)
                    FullCompanyAddress.CompanyPostcode = arrInputString(ArrayIndex)
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("Company_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    If Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field7") Then
                        Dim i As Integer
                        For i = 0 To UserField7.Count - 1
                            If UserField7(i) = arrInputString(ArrayIndex) Then
                                Exit For
                            End If
                        Next
                        If i >= UserField7.Count AndAlso arrInputString(ArrayIndex).Trim <> "" Then
                            ErrorCode = "17"
                            ErrorField = Applicant(0)(ApplicantIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidUserField7, ErrorValue)
                        End If
                    End If
                End If
                ' Truncate string
                len = CInt(Applicant(1)(ApplicantIndex).ToString.Replace("nvarchar(", "").Replace("varchar(", "").Replace(")", ""))
                If arrInputString(ArrayIndex).Length > len Then
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).Substring(0, len))
                Else
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex))
                End If
            ElseIf Applicant(1)(ApplicantIndex).ToString.StartsWith("datetime") Then
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If Applicant(0)(ApplicantIndex).ToString.StartsWith("Date_Of_Birth") _
                    OrElse Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field") Then
                        Dim iDay As Integer
                        Dim iMonth As Integer
                        Dim iYear As Integer

                        Try
                            iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                            iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                            iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                            If iYear < 1900 _
                            OrElse iYear < Now.Year - ValidDatePeriod _
                            OrElse iYear > Now.Year + ValidDatePeriod Then
                                ErrorCode = "08"
                                ErrorField = Applicant(0)(ApplicantIndex).ToString
                                ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = ""
                                BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidDate, ErrorValue)
                            Else
                                If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                    ErrorCode = "08"
                                    ErrorField = Parameter.Applicant(0)(ApplicantIndex).ToString
                                    ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                    arrInputString(ArrayIndex) = ""
                                    BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidDate, ErrorValue)
                                Else
                                    arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                End If
                            End If
                        Catch ex As Exception
                            ErrorCode = "08"
                            ErrorField = Applicant(0)(ApplicantIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidDate, ErrorValue)
                        End Try
                    End If
                End If
            Else
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If Applicant(0)(ApplicantIndex).ToString.StartsWith("User_Field") Then
                        If IsNumeric(arrInputString(ArrayIndex)) Then
                            Dim dtNumericValue As Long

                            Try
                                dtNumericValue = CLng(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = CStr(dtNumericValue)
                            Catch ex As Exception
                                ErrorCode = "10"
                                ErrorField = Applicant(0)(ApplicantIndex).ToString
                                ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = ""
                                BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                            End Try
                        Else
                            ErrorCode = "10"
                            ErrorField = Applicant(0)(ApplicantIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message & " - Applicant." & Applicant(0)(ApplicantIndex).ToString & " = " & arrInputString(ArrayIndex).ToString)
        End Try
    End Function

    Private Shared Function ValidateIntroducerAgent(ByVal IntroducerAgentIndex As Integer, ByVal ArrayIndex As Integer) As Boolean
        Dim len As Integer

        Try
            If IntroducerAgent(1)(IntroducerAgentIndex).ToString.StartsWith("varchar") _
            OrElse IntroducerAgent(1)(IntroducerAgentIndex).ToString.StartsWith("nvarchar") Then
                If IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Id_Number") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), SpecialCharacters)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Surname") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.LastName = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("First_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.FirstName = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Middle_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    FullName.MiddleName = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Mobile_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Name") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                    arrInputString(ArrayIndex) = " " + arrInputString(ArrayIndex).Trim + " "
                    For i As Integer = 0 To CompanySuffix.Count - 1
                        arrInputString(ArrayIndex) = Microsoft.VisualBasic.Strings.Replace(arrInputString(ArrayIndex), CompanySuffix(i), " ", 1, -1, Constants.vbTextCompare)
                    Next
                    arrInputString(ArrayIndex) = arrInputString(ArrayIndex).Trim
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Address") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuoteCommaDot(arrInputString(ArrayIndex))
                    Select Case IntroducerAgent(0)(IntroducerAgentIndex).ToString.Trim.Replace("Company_Address", "")
                        Case "1"
                            FullCompanyAddress.CompanyAddress1 = arrInputString(ArrayIndex)
                        Case "2"
                            FullCompanyAddress.CompanyAddress2 = arrInputString(ArrayIndex)
                        Case "3"
                            FullCompanyAddress.CompanyAddress3 = arrInputString(ArrayIndex)
                        Case "4"
                            FullCompanyAddress.CompanyAddress4 = arrInputString(ArrayIndex)
                        Case "5"
                            FullCompanyAddress.CompanyAddress5 = arrInputString(ArrayIndex)
                        Case "6"
                            FullCompanyAddress.CompanyAddress6 = arrInputString(ArrayIndex)
                    End Select
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Postcode") Then
                    arrInputString(ArrayIndex) = LeaveAtoZand0to9(arrInputString(ArrayIndex), "-")
                    FullCompanyAddress.CompanyPostcode = arrInputString(ArrayIndex)
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("Company_Phone_Number") Then
                    arrInputString(ArrayIndex) = Leave0to9(arrInputString(ArrayIndex))
                ElseIf IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("User_Field") Then
                    arrInputString(ArrayIndex) = RemoveSingleQuote(arrInputString(ArrayIndex))
                End If
                ' Truncate string
                len = CInt(IntroducerAgent(1)(IntroducerAgentIndex).ToString.Replace("nvarchar(", "").Replace("varchar(", "").Replace(")", ""))
                If arrInputString(ArrayIndex).Length > len Then
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).Substring(0, len))
                Else
                    arrInputString(ArrayIndex) = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex))
                End If
            ElseIf IntroducerAgent(1)(IntroducerAgentIndex).ToString.StartsWith("datetime") Then
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("User_Field") Then
                        Dim iDay As Integer
                        Dim iMonth As Integer
                        Dim iYear As Integer

                        Try
                            iDay = arrInputString(ArrayIndex).ToString.Split("/")(0)
                            iMonth = arrInputString(ArrayIndex).ToString.Split("/")(1)
                            iYear = arrInputString(ArrayIndex).ToString.Split("/")(2)
                            If iYear < 1900 _
                            OrElse iYear < Now.Year - ValidDatePeriod _
                            OrElse iYear > Now.Year + ValidDatePeriod Then
                                Throw New Exception("Invalid Date")
                                Return False
                            Else
                                If Not IsDate(iYear & "-" & iMonth & "-" & iDay) Then
                                    Throw New Exception("Invalid Date")
                                    Return False
                                Else
                                    arrInputString(ArrayIndex) = iYear & "-" & iMonth & "-" & iDay
                                End If
                            End If
                        Catch ex As Exception
                            Throw New Exception("Invalid Date")
                            Return False
                        End Try
                    End If
                End If
            Else
                If arrInputString(ArrayIndex).ToString.Trim <> "" Then
                    If IntroducerAgent(0)(IntroducerAgentIndex).ToString.StartsWith("User_Field") Then
                        If IsNumeric(arrInputString(ArrayIndex)) Then
                            Dim dtNumericValue As Long

                            Try
                                dtNumericValue = CLng(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = CStr(dtNumericValue)
                            Catch ex As Exception
                                ErrorCode = "10"
                                ErrorField = IntroducerAgent(0)(IntroducerAgentIndex).ToString
                                ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                                arrInputString(ArrayIndex) = ""
                                BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                            End Try
                        Else
                            ErrorCode = "10"
                            ErrorField = IntroducerAgent(0)(IntroducerAgentIndex).ToString
                            ErrorValue = System.Security.SecurityElement.Escape(arrInputString(ArrayIndex).ToString)
                            arrInputString(ArrayIndex) = ""
                            BuildDiaryXML(ErrorCategory, ErrorField, ErrorDescription.InvalidNumber, ErrorValue)
                        End If
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(ex.Message & " - IntroducerAgent." & IntroducerAgent(0)(IntroducerAgentIndex).ToString & " = " & arrInputString(ArrayIndex).ToString)
        End Try
    End Function

    Private Shared Function LeaveAtoZand0to9(ByVal sValidatingString As String, Optional ByVal sExtendedCharacters As String = "") As String
        Dim i, j As Integer
        Dim arrExtendedCharacters() As Char
        Dim arrValidatingString() As Char

        Try
            arrExtendedCharacters = sExtendedCharacters.ToCharArray
            arrValidatingString = sValidatingString.ToCharArray
            For i = 0 To arrValidatingString.Length - 1
                If (arrValidatingString(i) < "A" OrElse arrValidatingString(i) > "Z") _
                AndAlso (arrValidatingString(i) < "a" OrElse arrValidatingString(i) > "z") _
                AndAlso (arrValidatingString(i) < "0" OrElse arrValidatingString(i) > "9") _
                AndAlso (AscW(arrValidatingString(i)) >= 32 AndAlso AscW(arrValidatingString(i)) <= 126) Then
                    If sExtendedCharacters <> "" Then
                        For j = 0 To arrExtendedCharacters.Length - 1
                            If arrExtendedCharacters(j) = arrValidatingString(i) Then
                                Exit For
                            End If
                        Next
                        If j = arrExtendedCharacters.Length Then
                            'arrValidatingString(i) = ""
                            sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                        End If
                    Else
                        sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                    End If
                End If
            Next
            Return sValidatingString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function LeaveAtoZ(ByVal sValidatingString As String, Optional ByVal sExtendedCharacters As String = "") As String
        Dim i, j As Integer
        Dim arrEntendedCharacters() As Char
        Dim arrValidatingString() As Char

        Try
            arrEntendedCharacters = sExtendedCharacters.ToCharArray
            arrValidatingString = sValidatingString.ToCharArray
            For i = 0 To arrValidatingString.Length - 1
                If (arrValidatingString(i) < "A" OrElse arrValidatingString(i) > "Z") _
                AndAlso (arrValidatingString(i) < "a" OrElse arrValidatingString(i) > "z") _
                AndAlso (AscW(arrValidatingString(i)) >= 32 AndAlso AscW(arrValidatingString(i)) <= 126) Then
                    If sExtendedCharacters <> "" Then
                        For j = 0 To arrEntendedCharacters.Length - 1
                            If arrEntendedCharacters(j) = arrValidatingString(i) Then
                                Exit For
                            End If
                        Next
                        If j = arrEntendedCharacters.Length Then
                            'arrValidatingString(i) = ""
                            sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                        End If
                    Else
                        sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                    End If
                End If
            Next
            Return sValidatingString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function Leave0to9(ByVal sValidatingString As String, Optional ByVal sExtendedCharacters As String = "") As String
        Dim i, j As Integer
        Dim arrEntendedCharacters() As Char
        Dim arrValidatingString() As Char

        Try
            arrEntendedCharacters = sExtendedCharacters.ToCharArray
            arrValidatingString = sValidatingString.ToCharArray
            For i = 0 To arrValidatingString.Length - 1
                If (arrValidatingString(i) < "0" OrElse arrValidatingString(i) > "9") Then
                    If sExtendedCharacters <> "" Then
                        For j = 0 To arrEntendedCharacters.Length - 1
                            If arrEntendedCharacters(j) = arrValidatingString(i) Then
                                Exit For
                            End If
                        Next
                        If j = arrEntendedCharacters.Length Then
                            'arrValidatingString(i) = ""
                            sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                        End If
                    Else
                        sValidatingString = sValidatingString.Replace(arrValidatingString(i), "")
                    End If
                End If
            Next
            Return sValidatingString
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function RemoveSingleQuote(ByVal sValidatingString As String) As String
        Try
            Return sValidatingString.Replace("'", "").Trim
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Function RemoveSingleQuoteCommaDot(ByVal sValidatingString As String) As String
        Try
            Return sValidatingString.Replace("'", "").Replace(",", "").Replace(".", "").Trim
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Shared Sub BuildDiaryXML(ByVal sCategory As String, ByVal sField As String, ByVal sDescription As String, ByVal sValue As String)
        DiaryXML.Append("<Diary>")
        DiaryXML.Append("<Diary_Note>")
        DiaryXML.Append("Field in error was set to null. " & vbNewLine)
        DiaryXML.Append("Error Category : " & System.Security.SecurityElement.Escape(sCategory) & vbNewLine)
        DiaryXML.Append("Error Field : " & System.Security.SecurityElement.Escape(sField) & vbNewLine)
        DiaryXML.Append("Error Description : " & System.Security.SecurityElement.Escape(sDescription) & vbNewLine)
        DiaryXML.Append("Error Value : " & System.Security.SecurityElement.Escape(sValue) & vbNewLine)
        DiaryXML.Append("</Diary_Note>")
        DiaryXML.Append("</Diary>")
    End Sub

    Private Shared Sub BuildErrorXML(ByVal sCode As String, ByVal sValue As String)
        LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
        LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & "Error Code = " & sCode & ", Value = " & sValue & vbCrLf)
        LoadingLog.Append(Format(Now, "dd/MM/yyyy") & " " & Format(TimeOfDay, "HH:mm:ss") & " - " & vbCrLf)
        ErrorXML.Append("<Errors>")
        ErrorXML.Append("<Organisation>" & KeyFields.Organisation & "</Organisation>")
        ErrorXML.Append("<Country_Code>" & KeyFields.CountryCode & "</Country_Code>")
        ErrorXML.Append("<Application_Number>" & KeyFields.ApplicationNumber & "</Application_Number>")
        ErrorXML.Append("<Application_Type>" & KeyFields.ApplicationType & "</Application_Type>")
        ErrorXML.Append("<Error_Code>" & sCode & "</Error_Code>")
        ErrorXML.Append("<Error_Value>" & System.Security.SecurityElement.Escape(sValue) & "</Error_Value>")
        ErrorXML.Append("<AppKey>" & KeyFields.AppKey & "</AppKey>")
        ErrorXML.Append("</Errors>")
    End Sub

    Private Shared Function BuildFullName() As String
        If String.Compare(NameSequence, "123") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.FirstName.Trim & " " & FullName.MiddleName.Trim & " " & FullName.LastName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "132") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.FirstName.Trim & " " & FullName.LastName.Trim & " " & FullName.MiddleName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "213") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.MiddleName.Trim & " " & FullName.FirstName.Trim & " " & FullName.LastName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "231") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.LastName.Trim & " " & FullName.FirstName.Trim & " " & FullName.MiddleName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "312") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.MiddleName.Trim & " " & FullName.LastName.Trim & " " & FullName.FirstName.Trim).Replace("  ", " ").Trim
        ElseIf String.Compare(NameSequence, "321") = 0 Then
            Return System.Security.SecurityElement.Escape(FullName.LastName.Trim & " " & FullName.MiddleName.Trim & " " & FullName.FirstName.Trim).Replace("  ", " ").Trim
        Else
            Return ""
        End If
    End Function

    Private Shared Function BuildFullHomeAddress(Optional ByVal SiteWithSpecialFunction As String = "") As String
        Dim strFullHomeAddress As String

        strFullHomeAddress = ""

        'If it is for CMBC then address is without space and postcode
        If SiteWithSpecialFunction.Trim.ToUpper = "CMBC" Then

            'This is because we are returing substring(1) at last
            strFullHomeAddress = " "

            If String.Compare(FullHomeAddress.HomeAddress1.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress1
            End If

            If String.Compare(FullHomeAddress.HomeAddress2.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress2
            End If

            If String.Compare(FullHomeAddress.HomeAddress3.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress3
            End If

            If String.Compare(FullHomeAddress.HomeAddress4.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress4
            End If

            If String.Compare(FullHomeAddress.HomeAddress5.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress5
            End If

            If String.Compare(FullHomeAddress.HomeAddress6.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & FullHomeAddress.HomeAddress6
            End If

            'If FINANSBANK then only Address 1 to 4 in FULL ADDRESS
        ElseIf SiteWithSpecialFunction.Trim.ToUpper = "FINANSBANK" Then

            If String.Compare(FullHomeAddress.HomeAddress1.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress1
            End If

            If String.Compare(FullHomeAddress.HomeAddress2.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress2
            End If

            If String.Compare(FullHomeAddress.HomeAddress3.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress3
            End If

            If String.Compare(FullHomeAddress.HomeAddress4.Trim, "") <> 0 Then
                strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress4
            End If

        Else

            If AddressSequence.Trim.Length >= 7 Then
                Dim strData() As String = {FullHomeAddress.HomeAddress1.Trim, FullHomeAddress.HomeAddress2.Trim, FullHomeAddress.HomeAddress3.Trim, FullHomeAddress.HomeAddress4.Trim, _
                                           FullHomeAddress.HomeAddress5.Trim, FullHomeAddress.HomeAddress6.Trim, FullHomeAddress.HomePostcode.Trim}
                Dim strIndex(6) As String

                For iIndex As Integer = 0 To 6
                    strIndex(iIndex) = AddressSequence.Trim.Substring(iIndex, 1)
                Next

                For iIndex As Integer = 1 To 7
                    If (Array.IndexOf(strIndex, iIndex.ToString.Trim) >= 0) Then strFullHomeAddress = (strFullHomeAddress & " " & strData(Array.IndexOf(strIndex, iIndex.ToString.Trim))).Trim
                Next
                strFullHomeAddress = " " & strFullHomeAddress
            Else
                If String.Compare(FullHomeAddress.HomeAddress1.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress1
                End If

                If String.Compare(FullHomeAddress.HomeAddress2.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress2
                End If

                If String.Compare(FullHomeAddress.HomeAddress3.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress3
                End If

                If String.Compare(FullHomeAddress.HomeAddress4.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress4
                End If

                If String.Compare(FullHomeAddress.HomeAddress5.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress5
                End If

                If String.Compare(FullHomeAddress.HomeAddress6.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomeAddress6
                End If

                If String.Compare(FullHomeAddress.HomePostcode.Trim, "") <> 0 Then
                    strFullHomeAddress = strFullHomeAddress & " " & FullHomeAddress.HomePostcode
                End If
            End If

        End If

        If strFullHomeAddress.Length > 0 Then
            Return System.Security.SecurityElement.Escape(strFullHomeAddress.Substring(1))
        Else
            Return ""
        End If
    End Function

    Private Shared Function BuildFullCompanyAddress(Optional ByVal SiteWithSpecialFunction As String = "") As String
        Dim strFullCompanyAddress As String

        strFullCompanyAddress = ""

        'If it is for CMBC then address is without space and postcode
        If SiteWithSpecialFunction.Trim.ToUpper = "CMBC" Then

            'This is because we are returing substring(1) at last
            strFullCompanyAddress = " "

            If String.Compare(FullCompanyAddress.CompanyAddress1.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress1
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress2.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress2
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress3.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress3
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress4.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress4
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress5.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress5
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress6.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & FullCompanyAddress.CompanyAddress6
            End If

            'If FINANSBANK then only Address 1 to 4 in FULL ADDRESS
        ElseIf SiteWithSpecialFunction.Trim.ToUpper = "FINANSBANK" Then

            If String.Compare(FullCompanyAddress.CompanyAddress1.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress1
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress2.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress2
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress3.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress3
            End If

            If String.Compare(FullCompanyAddress.CompanyAddress4.Trim, "") <> 0 Then
                strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress4
            End If

        Else

            If AddressSequence.Trim.Length >= 7 Then
                Dim strData() As String = {FullCompanyAddress.CompanyAddress1.Trim, FullCompanyAddress.CompanyAddress2.Trim, FullCompanyAddress.CompanyAddress3.Trim, FullCompanyAddress.CompanyAddress4.Trim, _
                                           FullCompanyAddress.CompanyAddress5.Trim, FullCompanyAddress.CompanyAddress6.Trim, FullCompanyAddress.CompanyPostcode.Trim}
                Dim strIndex(6) As String

                For iIndex As Integer = 0 To 6
                    strIndex(iIndex) = AddressSequence.Trim.Substring(iIndex, 1)
                Next

                For iIndex As Integer = 1 To 7
                    If (Array.IndexOf(strIndex, iIndex.ToString.Trim) >= 0) Then strFullCompanyAddress = (strFullCompanyAddress & " " & strData(Array.IndexOf(strIndex, iIndex.ToString.Trim))).Trim
                Next
                strFullCompanyAddress = " " & strFullCompanyAddress
            Else
                If String.Compare(FullCompanyAddress.CompanyAddress1.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress1
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress2.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress2
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress3.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress3
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress4.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress4
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress5.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress5
                End If

                If String.Compare(FullCompanyAddress.CompanyAddress6.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyAddress6
                End If

                If String.Compare(FullCompanyAddress.CompanyPostcode.Trim, "") <> 0 Then
                    strFullCompanyAddress = strFullCompanyAddress & " " & FullCompanyAddress.CompanyPostcode
                End If
            End If

        End If

        If strFullCompanyAddress.Length > 0 Then
            Return System.Security.SecurityElement.Escape(strFullCompanyAddress.Substring(1))
        Else
            Return ""
        End If
    End Function

    Public Shared Function GenerateCriminalAutoNumber() As String
        Dim cmd As SqlCommand
        Dim daData As SqlDataAdapter
        Dim dsData As DataSet
        Dim strGeneratedApplicationNumber As String

        Try
            conn = New SqlConnection(strConn)
            conn.Open()

            dsData = New DataSet

            cmd = New SqlCommand("USP_Criminal_AutoNumber_Generate", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not Convert.IsDBNull(dsData.Tables(0).Rows(0)(0)) Then
                strGeneratedApplicationNumber = CStr(dsData.Tables(0).Rows(0)(0))
            Else
                strGeneratedApplicationNumber = ""
            End If
            conn.Close()

            Return strGeneratedApplicationNumber
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class