Imports System.Data.SqlClient
Imports System.IO
Imports System.Text

Public Class Parameter
    Public Shared Database As String
    Public Shared Delimiter As String
    Public Shared SpecialCharacters As String    
    Public Shared CountryCode As String
    Public Shared Organisation As ArrayList
    Public Shared ApplicationType As ArrayList
    Public Shared UserField7 As ArrayList
    Public Shared CategoryName As ArrayList
    Public Shared NameSequence As String
    Public Shared AddressSequence As String
    Public Shared CompanySuffix As ArrayList
    Public Shared ValidDatePeriod As Integer
    Public Shared WriteToLog As Boolean
    Public Shared Site As String
    Public Shared ScorecardXML As String
    Public Shared Application(1) As ArrayList
    Public Shared Applicant(1) As ArrayList
    Public Shared AccountantSolicitor(1) As ArrayList
    Public Shared Guarantor(1) As ArrayList
    Public Shared IntroducerAgent(1) As ArrayList
    Public Shared Reference(1) As ArrayList
    Public Shared Security(1) As ArrayList
    Public Shared PreviousAddressCompany(1) As ArrayList
    Public Shared Valuer(1) As ArrayList
    Public Shared User(1) As ArrayList
    Public Shared CreditBureau(1) As ArrayList
    Public Shared User2(1) As ArrayList
    Public Shared UCA(1) As ArrayList
    Public Shared CBA(1) As ArrayList
    Public Shared U2A(1) As ArrayList
    Public Shared UCB(1) As ArrayList
    Public Shared CBB(1) As ArrayList
    Public Shared U2B(1) As ArrayList
    Public Shared UCC(1) As ArrayList
    Public Shared CBC(1) As ArrayList
    Public Shared U2C(1) As ArrayList

    Public Shared Sub Initialize(ByVal connection As String, ByVal sCountryCode As String, ByVal sDelimiter As String, ByVal sDatabase As String, ByVal bWriteToLog As Boolean, ByVal sSite As String)
        Dim conn As SqlConnection
        Dim cmd As SqlCommand
        Dim dsData As DataSet
        Dim daData As SqlDataAdapter
        Dim drData As DataRow

        Try
            conn = New SqlConnection(connection)
            conn.Open()
            Database = sDatabase.Trim
            Delimiter = sDelimiter.Trim
            CountryCode = sCountryCode
            WriteToLog = bWriteToLog
            Site = sSite
            Organisation = New ArrayList
            ApplicationType = New ArrayList
            CompanySuffix = New ArrayList
            UserField7 = New ArrayList
            CategoryName = New ArrayList
            Application(0) = New ArrayList
            Application(1) = New ArrayList
            Applicant(0) = New ArrayList
            Applicant(1) = New ArrayList
            AccountantSolicitor(0) = New ArrayList
            AccountantSolicitor(1) = New ArrayList
            Guarantor(0) = New ArrayList
            Guarantor(1) = New ArrayList
            IntroducerAgent(0) = New ArrayList
            IntroducerAgent(1) = New ArrayList
            Reference(0) = New ArrayList
            Reference(1) = New ArrayList
            Security(0) = New ArrayList
            Security(1) = New ArrayList
            PreviousAddressCompany(0) = New ArrayList
            PreviousAddressCompany(1) = New ArrayList
            Valuer(0) = New ArrayList
            Valuer(1) = New ArrayList
            User(0) = New ArrayList
            User(1) = New ArrayList
            CreditBureau(0) = New ArrayList
            CreditBureau(1) = New ArrayList
            User2(0) = New ArrayList
            User2(1) = New ArrayList
            UCA(0) = New ArrayList
            UCA(1) = New ArrayList
            CBA(0) = New ArrayList
            CBA(1) = New ArrayList
            U2A(0) = New ArrayList
            U2A(1) = New ArrayList
            UCB(0) = New ArrayList
            UCB(1) = New ArrayList
            CBB(0) = New ArrayList
            CBB(1) = New ArrayList
            U2B(0) = New ArrayList
            U2B(1) = New ArrayList
            UCC(0) = New ArrayList
            UCC(1) = New ArrayList
            CBC(0) = New ArrayList
            CBC(1) = New ArrayList
            U2C(0) = New ArrayList
            U2C(1) = New ArrayList

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

            ' Scorecard XML
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_Scorecard_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            If Not dsData Is Nothing Then
                Dim sbScorecard As New StringBuilder
                Dim swScorecard As New StringWriter(sbScorecard)

                dsData.Tables(0).TableName = "Scorecard_Definition"
                dsData.Tables(1).TableName = "Scorecard_Attributes"
                dsData.Tables(2).TableName = "Scorecard_Intervals"
                dsData.WriteXml(swScorecard, XmlWriteMode.IgnoreSchema)
                ScorecardXML = sbScorecard.ToString
            Else
                ScorecardXML = ""
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

            ' Application
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 1

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    Application(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    Application(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Applicant
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 2

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                            AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    Applicant(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    Applicant(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Accountant Solicitor
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 3

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    AccountantSolicitor(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    AccountantSolicitor(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Guarantor
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 4

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    Guarantor(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    Guarantor(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Introducer Agent
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 5

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    IntroducerAgent(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    IntroducerAgent(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Reference
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 6

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    Reference(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    Reference(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Security
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 7

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    Security(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    Security(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Previous Address Company
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 8

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    PreviousAddressCompany(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    PreviousAddressCompany(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Valuer
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 9

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    Valuer(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    Valuer(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' User
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 10

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    User(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    User(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' Credit Bureau
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 11

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    CreditBureau(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    CreditBureau(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' User2
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 12

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    User2(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    User2(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' UCA
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 13

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    UCA(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    UCA(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' CBA
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 14

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    CBA(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    CBA(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' U2A
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 15

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    U2A(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    U2A(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' UCB
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 16

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    UCB(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    UCB(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' CBB
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 17

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    CBB(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    CBB(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' U2B
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 18

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    U2B(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    U2B(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' UCC
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 19

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    UCC(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    UCC(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' CBC
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 20

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    CBC(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    CBC(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            ' U2C
            dsData = New DataSet
            cmd = New SqlCommand("USP_Common_LayoutAndType_Select", conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@CategoryNumber", SqlDbType.SmallInt)
            cmd.Parameters(0).Value = 21

            daData = New SqlDataAdapter(cmd)
            daData.Fill(dsData)
            For Each drData In dsData.Tables(0).Rows
                If Not Convert.IsDBNull(drData("Category_Field_Name")) _
                AndAlso Not Convert.IsDBNull(drData("Category_Field_Type")) Then
                    U2C(0).Add(CStr(drData("Category_Field_Name")).Trim)
                    U2C(1).Add(CStr(drData("Category_Field_Type")).Trim)
                End If
            Next

            conn.Close()
        Catch ex As Exception
            Throw ex
        Finally
            conn.Dispose()
        End Try
    End Sub

End Class

