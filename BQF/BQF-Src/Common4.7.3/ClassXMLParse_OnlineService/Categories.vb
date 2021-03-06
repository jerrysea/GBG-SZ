Imports System.Reflection

Public Module Categories

    Public Delimiter As String = "|"

    Public Class Application
        Public Organisation As String
        Public Country_Code As String
        Public Group_Member_Code As String
        Public Application_Number As String
        Public Capture_Date As String
        Public Capture_Time As String
        Public Expiry_Date As String
        Public Application_Date As String
        Public Application_Type As String
        Public Amount_Limit As String
        Public Branch As String
        Public Decision As String
        Public Decision_Reason As String
        Public Decision_Date As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String

        Public applicants As New ArrayList
        Public guarantors As New ArrayList
        Public accountantSolicitors As New ArrayList
        Public introducers As New ArrayList
        Public references As New ArrayList
        Public securities As New ArrayList
        Public previousAddresses As New ArrayList
        Public valuers As New ArrayList
        Public uc1 As New ArrayList
        Public creditbureaus As New ArrayList
        Public uc2 As New ArrayList
        Public uca As New ArrayList
        Public cba As New ArrayList
        Public u2a As New ArrayList
        Public ucb As New ArrayList
        Public cbb As New ArrayList
        Public u2b As New ArrayList
        Public ucc As New ArrayList
        Public cbc As New ArrayList
        Public u2C As New ArrayList
        Public diary As New ArrayList

        Public Sub New()
        End Sub

        Public Sub New(ByVal XMLDocString As String)
            Dim xmldoc As New Xml.XmlDocument
            xmldoc.LoadXml(XMLDocString)
            Initialize(xmldoc)
        End Sub

        Private Sub Initialize(ByVal xmldoc As Xml.XmlDocument)
            Dim dset As New DataSet
            Dim s As String = xmldoc.OuterXml
            Dim x As New Xml.XmlTextReader(s, Xml.XmlNodeType.Document, Nothing)
            dset.ReadXml(x)
            x.Close()


            Dim objsource As Object = Me
            'Get the object type
            Dim objType As Type = objsource.GetType

            'Get all the information on Dim  fields/properties of that particular type
            Dim objFields() As FieldInfo = objType.GetFields()

            'start with applicationdetails
            For Each fi As FieldInfo In objFields
                Select Case fi.Name.ToLower
                    Case "applicants", "accountantsolicitors", "guarantors", "introducers", "references", "securities", "previousaddresses", "valuers", "uc1", "creditbureaus", "uc2", "uca", "cba", "u2a", "ucb", "cbb", "u2b", "ucc", "cbc", "u2c", "diary"
                        'do nothing
                    Case Else
                        'check if the fields in the xml exist, if not then means disabled
                        'and it should not be loaded into instinct, so add '?' so that it can be removed after build string
                        If dset.Tables("Application").Columns.Contains(fi.Name) Then
                            fi.SetValue(Me, dset.Tables("Application").Rows(0)(fi.Name))
                        Else
                            'set value as '?' so that it can be removed in the input string
                            fi.SetValue(Me, "?")
                        End If

                End Select
            Next

            Dim complexType As New Object
            For Each tbl As DataTable In dset.Tables
                For i As Integer = 0 To tbl.Rows.Count - 1
                    Select Case tbl.TableName.ToLower
                        Case "application"
                            complexType = Me
                        Case "applicant"
                            applicants.Add(New ApplicantCategory)
                            complexType = applicants(i)
                            CType(complexType, ApplicantCategory).Applicant = "A"
                        Case "a_accountant_solicitor"
                            accountantSolicitors.Add(New AccountantSolicitorCategory)
                            complexType = accountantSolicitors(i)
                            CType(complexType, AccountantSolicitorCategory).AccountantSolicitor = "S"
                        Case "guarantor"
                            guarantors.Add(New GuarantorCategory)
                            complexType = guarantors(i)
                            CType(complexType, GuarantorCategory).Guarantor = "G"
                        Case "introducer_agent"
                            introducers.Add(New IntroducerCategory)
                            complexType = introducers(i)
                            CType(complexType, IntroducerCategory).Introducer = "I"
                        Case "reference"
                            references.Add(New ReferenceCategory)
                            complexType = references(i)
                            CType(complexType, ReferenceCategory).Reference = "R"
                        Case "security"
                            securities.Add(New SecurityCategory)
                            complexType = securities(i)
                            CType(complexType, SecurityCategory).Security = "Y"
                        Case "previousaddress_company"
                            previousAddresses.Add(New PreviousAddressCompanyCategory)
                            complexType = previousAddresses(i)
                            CType(complexType, PreviousAddressCompanyCategory).Previous_Address = "P"
                        Case "valuer"
                            valuers.Add(New ValuerCategory)
                            complexType = valuers(i)
                            CType(complexType, ValuerCategory).Valuer = "V"
                        Case "user"
                            uc1.Add(New UserCategory)
                            complexType = uc1(i)
                            CType(complexType, UserCategory).User = "U"
                        Case "creditbureau"
                            creditbureaus.Add(New CreditBureauCategory)
                            complexType = creditbureaus(i)
                            CType(complexType, CreditBureauCategory).CreditBureau = "C"
                        Case "user2"
                            uc2.Add(New UserCategory2)
                            complexType = uc2(i)
                            CType(complexType, UserCategory2).User2 = "O"
                        Case "uca"
                            uca.Add(New UCACategory)
                            complexType = uca(i)
                            CType(complexType, UCACategory).UCA = "W"
                        Case "cba"
                            cba.Add(New CBACategory)
                            complexType = cba(i)
                            CType(complexType, CBACategory).CBA = "B"
                        Case "u2a"
                            u2a.Add(New U2ACategory)
                            complexType = u2a(i)
                            CType(complexType, U2ACategory).U2A = "F"
                        Case "ucb"
                            ucb.Add(New UCBCategory)
                            complexType = ucb(i)
                            CType(complexType, UCBCategory).UCB = "J"
                        Case "cbb"
                            cbb.Add(New CBBCategory)
                            complexType = cbb(i)
                            CType(complexType, CBBCategory).CBB = "K"
                        Case "u2b"
                            u2b.Add(New U2BCategory)
                            complexType = u2b(i)
                            CType(complexType, U2BCategory).U2B = "L"
                        Case "ucc"
                            ucc.Add(New UCCCategory)
                            complexType = ucc(i)
                            CType(complexType, UCCCategory).UCC = "M"
                        Case "cbc"
                            cbc.Add(New CBCCategory)
                            complexType = cbc(i)
                            CType(complexType, CBCCategory).CBC = "X"
                        Case "u2c"
                            u2c.Add(New U2CCategory)
                            complexType = u2c(i)
                            CType(complexType, U2CCategory).U2C = "Z"
                        Case "diary"
                            diary.Add(New Diary)
                            complexType = diary(i)
                            CType(complexType, Diary).Diary = "N"
                    End Select
                    LoadData(complexType, tbl.Rows(i))
                Next
            Next
        End Sub

        Public Sub New(ByVal XMLDoc As Xml.XmlDocument)
            Initialize(XMLDoc)
        End Sub

        Private Sub LoadData(ByRef objSource As Object, ByVal dRow As DataRow)
            For Each fi As FieldInfo In objSource.GetType.GetFields()
                If dRow.Table.Columns.Contains(fi.Name) Then
                    If Not IsDBNull(dRow(fi.Name)) Then
                        fi.SetValue(objSource, dRow(fi.Name))
                    End If
                Else
                    'not found the field in the XML,so that means this field is disabled
                    'set a special value '?' so that it can be removed after build string
                    If (fi.GetValue(objSource) Is Nothing) And fi.FieldType.ToString.ToLower = "system.string" Then
                        fi.SetValue(objSource, "?")
                    End If
                End If
            Next
        End Sub

        Public Overrides Function ToString() As String
            Dim rec As New System.Text.StringBuilder

            rec.Append(Organisation + Delimiter + _
            Country_Code + Delimiter + _
            Group_Member_Code + Delimiter + _
            Application_Number + Delimiter + _
            Capture_Date + Delimiter + _
            Capture_Time + Delimiter + _
            Expiry_Date + Delimiter + _
            Application_Date + Delimiter + _
            Application_Type + Delimiter + _
            Amount_Limit + Delimiter + _
            Branch + Delimiter + _
            Decision + Delimiter + _
            Decision_Reason + Delimiter + _
            Decision_Date + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30)

            For Each o As ApplicantCategory In applicants
                rec.Append(Delimiter + o.ToString)
            Next

            If Not IsNothing(accountantSolicitors) AndAlso accountantSolicitors.Count > 0 Then
                For Each o As AccountantSolicitorCategory In accountantSolicitors
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(guarantors) AndAlso _
            guarantors.Count > 0 Then
                For Each o As GuarantorCategory In guarantors
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(introducers) AndAlso introducers.Count > 0 Then
                For Each o As IntroducerCategory In introducers
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(references) AndAlso _
            references.Count > 0 Then
                For Each o As ReferenceCategory In references
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(securities) AndAlso _
            securities.Count > 0 Then
                For Each o As SecurityCategory In securities
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(previousAddresses) AndAlso _
            previousAddresses.Count > 0 Then
                For Each o As PreviousAddressCompanyCategory In previousAddresses
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(valuers) AndAlso valuers.Count > 0 Then
                For Each o As ValuerCategory In valuers
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(uc1) AndAlso _
            uc1.Count > 0 Then
                For Each o As UserCategory In uc1
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(creditbureaus) AndAlso _
            creditbureaus.Count > 0 Then
                For Each o As CreditBureauCategory In creditbureaus
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(uc2) AndAlso _
            uc2.Count > 0 Then
                For Each o As UserCategory2 In uc2
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(uca) AndAlso _
            uca.Count > 0 Then
                For Each o As UCACategory In uca
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(cba) AndAlso _
            cba.Count > 0 Then
                For Each o As CBACategory In cba
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(u2a) AndAlso _
            u2a.Count > 0 Then
                For Each o As U2ACategory In u2a
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(ucb) AndAlso _
            ucb.Count > 0 Then
                For Each o As UCBCategory In ucb
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(cbb) AndAlso _
            cbb.Count > 0 Then
                For Each o As CBBCategory In cbb
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(u2b) AndAlso _
            u2b.Count > 0 Then
                For Each o As U2BCategory In u2b
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(ucc) AndAlso _
            ucc.Count > 0 Then
                For Each o As UCCCategory In ucc
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(cbc) AndAlso _
            cbc.Count > 0 Then
                For Each o As CBCCategory In cbc
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(u2C) AndAlso _
            u2C.Count > 0 Then
                For Each o As U2CCategory In u2C
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not IsNothing(diary) AndAlso _
            diary.Count > 0 Then
                For Each o As Diary In diary
                    rec.Append(Delimiter + o.ToString)
                Next
            End If

            If Not rec.ToString.EndsWith("|") Then rec.Append("|")

            'replace '|?|' as empty so that the disabled fields can be removed
            'need to loop to replace all the fields which contains |?|
            While rec.ToString.IndexOf("|?|") <> -1
                rec.Replace("|?|", "|")
            End While
            If Not rec.ToString.EndsWith("|") Then rec.Append("|")

            Return rec.ToString

        End Function

        Public Function ToStringForSpecificSite(ByVal SiteName As String) As String
            Dim rec As New System.Text.StringBuilder

            If SiteName = "HLB" Then
                rec.Append(Organisation + Delimiter + _
                Application_Number + Delimiter + _
                Application_Date + Delimiter + _
                Application_Type + Delimiter + _
                Amount_Limit + Delimiter + _
                Branch + Delimiter + _
                Decision + Delimiter + _
                Decision_Reason + Delimiter + _
                Decision_Date)


                For Each o As ApplicantCategory In applicants
                    rec.Append(Delimiter + o.ToStringForSpecificSite(SiteName))
                Next

                If Not IsNothing(accountantSolicitors) AndAlso accountantSolicitors.Count > 0 Then
                    For Each o As AccountantSolicitorCategory In accountantSolicitors
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(guarantors) AndAlso _
                guarantors.Count > 0 Then
                    For Each o As GuarantorCategory In guarantors
                        rec.Append(Delimiter + o.ToStringForSpecificSite(SiteName))
                    Next
                End If

                If Not IsNothing(introducers) AndAlso introducers.Count > 0 Then
                    For Each o As IntroducerCategory In introducers
                        rec.Append(Delimiter + o.ToStringForSpecificSite(SiteName))
                    Next
                End If

                If Not IsNothing(references) AndAlso _
                references.Count > 0 Then
                    For Each o As ReferenceCategory In references
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(securities) AndAlso _
                securities.Count > 0 Then
                    For Each o As SecurityCategory In securities
                        rec.Append(Delimiter + o.ToStringForSpecificSite(SiteName))
                    Next
                End If

                If Not IsNothing(previousAddresses) AndAlso _
                previousAddresses.Count > 0 Then
                    For Each o As PreviousAddressCompanyCategory In previousAddresses
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(valuers) AndAlso valuers.Count > 0 Then
                    For Each o As ValuerCategory In valuers
                        rec.Append(Delimiter + o.ToStringForSpecificSite(SiteName))
                    Next
                End If

                If Not IsNothing(uc1) AndAlso _
                uc1.Count > 0 Then
                    For Each o As UserCategory In uc1
                        rec.Append(Delimiter + o.ToStringForSpecificSite(SiteName))
                    Next
                End If

                If Not IsNothing(creditbureaus) AndAlso _
                creditbureaus.Count > 0 Then
                    For Each o As CreditBureauCategory In creditbureaus
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(uc2) AndAlso _
                uc2.Count > 0 Then
                    For Each o As UserCategory2 In uc2
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(uca) AndAlso _
                uca.Count > 0 Then
                    For Each o As UCACategory In uca
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(cba) AndAlso _
                cba.Count > 0 Then
                    For Each o As CBACategory In cba
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(u2a) AndAlso _
                u2a.Count > 0 Then
                    For Each o As U2ACategory In u2a
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(ucb) AndAlso _
                ucb.Count > 0 Then
                    For Each o As UCBCategory In ucb
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(cbb) AndAlso _
                cbb.Count > 0 Then
                    For Each o As CBBCategory In cbb
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(u2b) AndAlso _
                u2b.Count > 0 Then
                    For Each o As U2BCategory In u2b
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(ucc) AndAlso _
                ucc.Count > 0 Then
                    For Each o As UCCCategory In ucc
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(cbc) AndAlso _
                cbc.Count > 0 Then
                    For Each o As CBCCategory In cbc
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(u2C) AndAlso _
                u2C.Count > 0 Then
                    For Each o As U2CCategory In u2C
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(diary) AndAlso _
                diary.Count > 0 Then
                    For Each o As Diary In diary
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If
            Else
                rec.Append(Organisation + Delimiter + _
                Country_Code + Delimiter + _
                Group_Member_Code + Delimiter + _
                Application_Number + Delimiter + _
                Capture_Date + Delimiter + _
                Capture_Time + Delimiter + _
                Expiry_Date + Delimiter + _
                Application_Date + Delimiter + _
                Application_Type + Delimiter + _
                Amount_Limit + Delimiter + _
                Branch + Delimiter + _
                Decision + Delimiter + _
                Decision_Reason + Delimiter + _
                Decision_Date + Delimiter + _
                User_Field1 + Delimiter + _
                User_Field2 + Delimiter + _
                User_Field3 + Delimiter + _
                User_Field4 + Delimiter + _
                User_Field5 + Delimiter + _
                User_Field6 + Delimiter + _
                User_Field7 + Delimiter + _
                User_Field8 + Delimiter + _
                User_Field9 + Delimiter + _
                User_Field10 + Delimiter + _
                User_Field11 + Delimiter + _
                User_Field12 + Delimiter + _
                User_Field13 + Delimiter + _
                User_Field14 + Delimiter + _
                User_Field15 + Delimiter + _
                User_Field16 + Delimiter + _
                User_Field17 + Delimiter + _
                User_Field18 + Delimiter + _
                User_Field19 + Delimiter + _
                User_Field20)

                For Each o As ApplicantCategory In applicants
                    rec.Append(Delimiter + o.ToString)
                Next

                If Not IsNothing(accountantSolicitors) AndAlso accountantSolicitors.Count > 0 Then
                    For Each o As AccountantSolicitorCategory In accountantSolicitors
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(guarantors) AndAlso _
                guarantors.Count > 0 Then
                    For Each o As GuarantorCategory In guarantors
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(introducers) AndAlso introducers.Count > 0 Then
                    For Each o As IntroducerCategory In introducers
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(references) AndAlso _
                references.Count > 0 Then
                    For Each o As ReferenceCategory In references
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(securities) AndAlso _
                securities.Count > 0 Then
                    For Each o As SecurityCategory In securities
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(previousAddresses) AndAlso _
                previousAddresses.Count > 0 Then
                    For Each o As PreviousAddressCompanyCategory In previousAddresses
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(valuers) AndAlso valuers.Count > 0 Then
                    For Each o As ValuerCategory In valuers
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(uc1) AndAlso _
                uc1.Count > 0 Then
                    For Each o As UserCategory In uc1
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(creditbureaus) AndAlso _
                creditbureaus.Count > 0 Then
                    For Each o As CreditBureauCategory In creditbureaus
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(uc2) AndAlso _
                uc2.Count > 0 Then
                    For Each o As UserCategory2 In uc2
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(uca) AndAlso _
                uca.Count > 0 Then
                    For Each o As UCACategory In uca
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(cba) AndAlso _
                cba.Count > 0 Then
                    For Each o As CBACategory In cba
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(u2a) AndAlso _
                u2a.Count > 0 Then
                    For Each o As U2ACategory In u2a
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(ucb) AndAlso _
                ucb.Count > 0 Then
                    For Each o As UCBCategory In ucb
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(cbb) AndAlso _
                cbb.Count > 0 Then
                    For Each o As CBBCategory In cbb
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(u2b) AndAlso _
                u2b.Count > 0 Then
                    For Each o As U2BCategory In u2b
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(ucc) AndAlso _
                ucc.Count > 0 Then
                    For Each o As UCCCategory In ucc
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(cbc) AndAlso _
                cbc.Count > 0 Then
                    For Each o As CBCCategory In cbc
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(u2C) AndAlso _
                u2C.Count > 0 Then
                    For Each o As U2CCategory In u2C
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If

                If Not IsNothing(diary) AndAlso _
                diary.Count > 0 Then
                    For Each o As Diary In diary
                        rec.Append(Delimiter + o.ToString)
                    Next
                End If
            End If

            If Not rec.ToString.EndsWith("|") Then rec.Append("|")

            'replace '|?|' as empty so that the disabled fields can be removed
            'need to loop to replace all the fields which contains |?|
            While rec.ToString.IndexOf("|?|") <> -1
                rec.Replace("|?|", "|")
            End While
            If Not rec.ToString.EndsWith("|") Then rec.Append("|")


            Return rec.ToString

        End Function

    End Class

    Public Class ApplicantCategory
        Public Applicant As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Sex As String
        Public Date_Of_Birth As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String

        Public Overrides Function ToString() As String
            Return Applicant + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Sex + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20
        End Function

        Public Function ToStringForSpecificSite(ByVal SiteName As String) As String
            If SiteName = "HLB" Then
                Return Applicant + Delimiter + _
                Id_Number1 + Delimiter + _
                Id_Number2 + Delimiter + _
                Surname + Delimiter + _
                Sex + Delimiter + _
                Date_Of_Birth + Delimiter + _
                Home_Address1 + Delimiter + _
                Home_Address2 + Delimiter + _
                Home_Address3 + Delimiter + _
                Home_Address4 + Delimiter + _
                Home_Address5 + Delimiter + _
                Home_Postcode + Delimiter + _
                Home_Phone_Number + Delimiter + _
                Mobile_Phone_Number + Delimiter + _
                Company_Name + Delimiter + _
                Company_Address1 + Delimiter + _
                Company_Address2 + Delimiter + _
                Company_Address3 + Delimiter + _
                Company_Address4 + Delimiter + _
                Company_Address5 + Delimiter + _
                Company_Postcode + Delimiter + _
                Company_Phone_Number
            Else
                Return Applicant + Delimiter + _
                Id_Number1 + Delimiter + _
                Id_Number2 + Delimiter + _
                Id_Number3 + Delimiter + _
                Surname + Delimiter + _
                First_Name + Delimiter + _
                Middle_Name + Delimiter + _
                Sex + Delimiter + _
                Date_Of_Birth + Delimiter + _
                Home_Address1 + Delimiter + _
                Home_Address2 + Delimiter + _
                Home_Address3 + Delimiter + _
                Home_Address4 + Delimiter + _
                Home_Address5 + Delimiter + _
                Home_Address6 + Delimiter + _
                Home_Postcode + Delimiter + _
                Home_Phone_Number + Delimiter + _
                Mobile_Phone_Number + Delimiter + _
                Company_Name + Delimiter + _
                Company_Address1 + Delimiter + _
                Company_Address2 + Delimiter + _
                Company_Address3 + Delimiter + _
                Company_Address4 + Delimiter + _
                Company_Address5 + Delimiter + _
                Company_Address6 + Delimiter + _
                Company_Postcode + Delimiter + _
                Company_Phone_Number + Delimiter + _
                User_Field1 + Delimiter + _
                User_Field2 + Delimiter + _
                User_Field3 + Delimiter + _
                User_Field4 + Delimiter + _
                User_Field5 + Delimiter + _
                User_Field6 + Delimiter + _
                User_Field7 + Delimiter + _
                User_Field8 + Delimiter + _
                User_Field9 + Delimiter + _
                User_Field10 + Delimiter + _
                User_Field11 + Delimiter + _
                User_Field12 + Delimiter + _
                User_Field13 + Delimiter + _
                User_Field14 + Delimiter + _
                User_Field15 + Delimiter + _
                User_Field16 + Delimiter + _
                User_Field17 + Delimiter + _
                User_Field18 + Delimiter + _
                User_Field19 + Delimiter + _
                User_Field20
            End If

        End Function

    End Class

    Public Class AccountantSolicitorCategory
        Public AccountantSolicitor As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String
            Return AccountantSolicitor + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10
        End Function

    End Class

    Public Class GuarantorCategory
        Public Guarantor As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Sex As String
        Public Date_Of_Birth As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String
            Return Guarantor + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Sex + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10
        End Function

        Public Function ToStringForSpecificSite(ByVal SiteName As String) As String
            If SiteName = "HLB" Then
                Return Guarantor + Delimiter + _
                Id_Number1 + Delimiter + _
                Id_Number2 + Delimiter + _
                Surname + Delimiter + _
                Sex + Delimiter + _
                Date_Of_Birth + Delimiter + _
                Home_Address1 + Delimiter + _
                Home_Address2 + Delimiter + _
                Home_Address3 + Delimiter + _
                Home_Address4 + Delimiter + _
                Home_Address5 + Delimiter + _
                Home_Postcode + Delimiter + _
                Home_Phone_Number + Delimiter + _
                Mobile_Phone_Number + Delimiter + _
                Company_Name + Delimiter + _
                Company_Address1 + Delimiter + _
                Company_Address2 + Delimiter + _
                Company_Address3 + Delimiter + _
                Company_Address4 + Delimiter + _
                Company_Address5 + Delimiter + _
                Company_Postcode + Delimiter + _
                Company_Phone_Number
            Else
                Return Guarantor + Delimiter + _
                Id_Number1 + Delimiter + _
                Id_Number2 + Delimiter + _
                Id_Number3 + Delimiter + _
                Surname + Delimiter + _
                First_Name + Delimiter + _
                Middle_Name + Delimiter + _
                Sex + Delimiter + _
                Date_Of_Birth + Delimiter + _
                Home_Address1 + Delimiter + _
                Home_Address2 + Delimiter + _
                Home_Address3 + Delimiter + _
                Home_Address4 + Delimiter + _
                Home_Address5 + Delimiter + _
                Home_Address6 + Delimiter + _
                Home_Postcode + Delimiter + _
                Home_Phone_Number + Delimiter + _
                Mobile_Phone_Number + Delimiter + _
                Company_Name + Delimiter + _
                Company_Address1 + Delimiter + _
                Company_Address2 + Delimiter + _
                Company_Address3 + Delimiter + _
                Company_Address4 + Delimiter + _
                Company_Address5 + Delimiter + _
                Company_Address6 + Delimiter + _
                Company_Postcode + Delimiter + _
                Company_Phone_Number + Delimiter + _
                User_Field1 + Delimiter + _
                User_Field2 + Delimiter + _
                User_Field3 + Delimiter + _
                User_Field4 + Delimiter + _
                User_Field5 + Delimiter + _
                User_Field6 + Delimiter + _
                User_Field7 + Delimiter + _
                User_Field8 + Delimiter + _
                User_Field9 + Delimiter + _
                User_Field10
            End If
        End Function

    End Class

    Public Class IntroducerCategory
        Public Introducer As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String
            Return Introducer + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10

        End Function

        Public Function ToStringForSpecificSite(ByVal SiteName As String) As String
            If SiteName = "HLB" Then
                Return Introducer + Delimiter + _
                           Id_Number1 + Delimiter + _
                           Surname + Delimiter + _
                           Company_Phone_Number + Delimiter + _
                           User_Field1
            Else
                Return Introducer + Delimiter + _
                            Id_Number1 + Delimiter + _
                            Id_Number2 + Delimiter + _
                            Id_Number3 + Delimiter + _
                            Surname + Delimiter + _
                            First_Name + Delimiter + _
                            Middle_Name + Delimiter + _
                            Mobile_Phone_Number + Delimiter + _
                            Company_Name + Delimiter + _
                            Company_Address1 + Delimiter + _
                            Company_Address2 + Delimiter + _
                            Company_Address3 + Delimiter + _
                            Company_Address4 + Delimiter + _
                            Company_Address5 + Delimiter + _
                            Company_Address6 + Delimiter + _
                            Company_Postcode + Delimiter + _
                            Company_Phone_Number + Delimiter + _
                            User_Field1 + Delimiter + _
                            User_Field2 + Delimiter + _
                            User_Field3 + Delimiter + _
                            User_Field4 + Delimiter + _
                            User_Field5 + Delimiter + _
                            User_Field6 + Delimiter + _
                            User_Field7 + Delimiter + _
                            User_Field8 + Delimiter + _
                            User_Field9 + Delimiter + _
                            User_Field10
            End If
        End Function

    End Class

    Public Class ReferenceCategory
        Public Reference As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String
            Return Reference + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10
        End Function

    End Class

    Public Class SecurityCategory
        Public Security As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String

        'edit by wilson on 10may-----------------------------------------------------
        'this is for GE MX, as the layout was wrong
        'so they will use comp info to map
        'but it doesn't matter as for other clients, they won't populuate those tags
        'so it won't be converted anyway
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        '----------------------------------------------------------------------------
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String

            'edit by wilson on 10may-----------------------------------------------------
            'this is for GE MX, as the layout was wrong
            'so they will use comp info to map
            'but it doesn't matter as for other clients, they won't populuate those tags
            'so it won't be converted anyway
            Return Security + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10
            'edit by wilson on 10may-----------------------------------------------------

        End Function

        Public Function ToStringForSpecificSite(ByVal SiteName As String) As String
            If SiteName = "HLB" Then
                Return Security + Delimiter + _
                Id_Number1 + Delimiter + _
                Surname + Delimiter + _
                Home_Address1 + Delimiter + _
                Home_Address2 + Delimiter + _
                Home_Address3 + Delimiter + _
                Home_Address4 + Delimiter + _
                Home_Address5 + Delimiter + _
                Home_Postcode
            Else
                Return Security + Delimiter + _
                Id_Number1 + Delimiter + _
                Id_Number2 + Delimiter + _
                Id_Number3 + Delimiter + _
                Surname + Delimiter + _
                First_Name + Delimiter + _
                Middle_Name + Delimiter + _
                Company_Name + Delimiter + _
                Company_Address1 + Delimiter + _
                Company_Address2 + Delimiter + _
                Company_Address3 + Delimiter + _
                Company_Address4 + Delimiter + _
                Home_Address1 + Delimiter + _
                Home_Address2 + Delimiter + _
                Home_Address3 + Delimiter + _
                Home_Address4 + Delimiter + _
                Home_Address5 + Delimiter + _
                Home_Address6 + Delimiter + _
                Home_Postcode + Delimiter + _
                User_Field1 + Delimiter + _
                User_Field2 + Delimiter + _
                User_Field3 + Delimiter + _
                User_Field4 + Delimiter + _
                User_Field5 + Delimiter + _
                User_Field6 + Delimiter + _
                User_Field7 + Delimiter + _
                User_Field8 + Delimiter + _
                User_Field9 + Delimiter + _
                User_Field10
            End If
        End Function

    End Class

    Public Class PreviousAddressCompanyCategory
        Public Previous_Address As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String
            Return Previous_Address + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10
        End Function

    End Class

    Public Class ValuerCategory
        Public Valuer As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String
            Return Valuer + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10
        End Function

        Public Function ToStringForSpecificSite(ByVal SiteName As String) As String
            If SiteName = "HLB" Then
                Return Valuer + Delimiter + _
                Id_Number1 + Delimiter + _
                Id_Number2 + Delimiter + _
                Id_Number3 + Delimiter + _
                Surname + Delimiter + _
                Company_Phone_Number + Delimiter + _
                User_Field1 + Delimiter + _
                User_Field2
            Else
                Return Valuer + Delimiter + _
                Id_Number1 + Delimiter + _
                Id_Number2 + Delimiter + _
                Id_Number3 + Delimiter + _
                Surname + Delimiter + _
                First_Name + Delimiter + _
                Middle_Name + Delimiter + _
                Mobile_Phone_Number + Delimiter + _
                Company_Name + Delimiter + _
                Company_Address1 + Delimiter + _
                Company_Address2 + Delimiter + _
                Company_Address3 + Delimiter + _
                Company_Address4 + Delimiter + _
                Company_Address5 + Delimiter + _
                Company_Address6 + Delimiter + _
                Company_Postcode + Delimiter + _
                Company_Phone_Number + Delimiter + _
                User_Field1 + Delimiter + _
                User_Field2 + Delimiter + _
                User_Field3 + Delimiter + _
                User_Field4 + Delimiter + _
                User_Field5 + Delimiter + _
                User_Field6 + Delimiter + _
                User_Field7 + Delimiter + _
                User_Field8 + Delimiter + _
                User_Field9 + Delimiter + _
                User_Field10
            End If
        End Function

    End Class

    Public Class UserCategory
        Public User As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Sex As String
        Public Date_Of_Birth As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String

        Public Overrides Function ToString() As String
            Return User + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Sex + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10
        End Function

        Public Function ToStringForSpecificSite(ByVal SiteName As String) As String
            If SiteName = "HLB" Then
                Return User + Delimiter + _
                          Id_Number1 + Delimiter + _
                          Company_Name + Delimiter + _
                          Company_Address1 + Delimiter + _
                          Company_Address2 + Delimiter + _
                          Company_Address3 + Delimiter + _
                          Company_Address4 + Delimiter + _
                          Company_Address5 + Delimiter + _
                          Company_Postcode + Delimiter + _
                          Company_Phone_Number
            Else
                Return User + Delimiter + _
                           Id_Number1 + Delimiter + _
                           Id_Number2 + Delimiter + _
                           Id_Number3 + Delimiter + _
                           Surname + Delimiter + _
                           First_Name + Delimiter + _
                           Middle_Name + Delimiter + _
                           Sex + Delimiter + _
                           Date_Of_Birth + Delimiter + _
                           Home_Address1 + Delimiter + _
                           Home_Address2 + Delimiter + _
                           Home_Address3 + Delimiter + _
                           Home_Address4 + Delimiter + _
                           Home_Address5 + Delimiter + _
                           Home_Address6 + Delimiter + _
                           Home_Postcode + Delimiter + _
                           Home_Phone_Number + Delimiter + _
                           Mobile_Phone_Number + Delimiter + _
                           Company_Name + Delimiter + _
                           Company_Address1 + Delimiter + _
                           Company_Address2 + Delimiter + _
                           Company_Address3 + Delimiter + _
                           Company_Address4 + Delimiter + _
                           Company_Address5 + Delimiter + _
                           Company_Address6 + Delimiter + _
                           Company_Postcode + Delimiter + _
                           Company_Phone_Number + Delimiter + _
                           User_Field1 + Delimiter + _
                           User_Field2 + Delimiter + _
                           User_Field3 + Delimiter + _
                           User_Field4 + Delimiter + _
                           User_Field5 + Delimiter + _
                           User_Field6 + Delimiter + _
                           User_Field7 + Delimiter + _
                           User_Field8 + Delimiter + _
                           User_Field9 + Delimiter + _
                           User_Field10
            End If
        End Function

    End Class

    Public Class CreditBureauCategory
        Public CreditBureau As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Short_Name As String
        Public Date_Of_Birth As String
        Public Sex As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Office_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Other_Phone_Number As String
        Public Company_Name As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String

        Public Overrides Function ToString() As String
            Return CreditBureau + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Short_Name + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Sex + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Office_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Other_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23
        End Function

    End Class

    Public Class UserCategory2
        Public User2 As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String
        Public User_Field31 As String
        Public User_Field32 As String
        Public User_Field33 As String
        Public User_Field34 As String
        Public User_Field35 As String
        Public User_Field36 As String
        Public User_Field37 As String
        Public User_Field38 As String
        Public User_Field39 As String
        Public User_Field40 As String
        Public User_Field41 As String
        Public User_Field42 As String
        Public User_Field43 As String
        Public User_Field44 As String
        Public User_Field45 As String
        Public User_Field46 As String
        Public User_Field47 As String
        Public User_Field48 As String
        Public User_Field49 As String
        Public User_Field50 As String

        Public Overrides Function ToString() As String
            Return User2 + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30 + Delimiter + _
            User_Field31 + Delimiter + _
            User_Field32 + Delimiter + _
            User_Field33 + Delimiter + _
            User_Field34 + Delimiter + _
            User_Field35 + Delimiter + _
            User_Field36 + Delimiter + _
            User_Field37 + Delimiter + _
            User_Field38 + Delimiter + _
            User_Field39 + Delimiter + _
            User_Field40 + Delimiter + _
            User_Field41 + Delimiter + _
            User_Field42 + Delimiter + _
            User_Field43 + Delimiter + _
            User_Field44 + Delimiter + _
            User_Field45 + Delimiter + _
            User_Field46 + Delimiter + _
            User_Field47 + Delimiter + _
            User_Field48 + Delimiter + _
            User_Field49 + Delimiter + _
            User_Field50
        End Function

    End Class

    Public Class UCACategory
        Public UCA As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Sex As String
        Public Date_Of_Birth As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String

        Public Overrides Function ToString() As String
            Return UCA + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Sex + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20
        End Function

    End Class

    Public Class CBACategory
        Public CBA As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Short_Name As String
        Public Date_Of_Birth As String
        Public Sex As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Office_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Other_Phone_Number As String
        Public Company_Name As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String
        Public User_Field31 As String
        Public User_Field32 As String
        Public User_Field33 As String

        Public Overrides Function ToString() As String
            Return CBA + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Short_Name + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Sex + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Office_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Other_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30 + Delimiter + _
            User_Field31 + Delimiter + _
            User_Field32 + Delimiter + _
            User_Field33
        End Function

    End Class

    Public Class U2ACategory
        Public U2A As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String
        Public User_Field31 As String
        Public User_Field32 As String
        Public User_Field33 As String
        Public User_Field34 As String
        Public User_Field35 As String
        Public User_Field36 As String
        Public User_Field37 As String
        Public User_Field38 As String
        Public User_Field39 As String
        Public User_Field40 As String
        Public User_Field41 As String
        Public User_Field42 As String
        Public User_Field43 As String
        Public User_Field44 As String
        Public User_Field45 As String
        Public User_Field46 As String
        Public User_Field47 As String
        Public User_Field48 As String
        Public User_Field49 As String
        Public User_Field50 As String
        Public User_Field51 As String
        Public User_Field52 As String
        Public User_Field53 As String
        Public User_Field54 As String
        Public User_Field55 As String
        Public User_Field56 As String
        Public User_Field57 As String
        Public User_Field58 As String
        Public User_Field59 As String
        Public User_Field60 As String

        Public Overrides Function ToString() As String
            Return U2A + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30 + Delimiter + _
            User_Field31 + Delimiter + _
            User_Field32 + Delimiter + _
            User_Field33 + Delimiter + _
            User_Field34 + Delimiter + _
            User_Field35 + Delimiter + _
            User_Field36 + Delimiter + _
            User_Field37 + Delimiter + _
            User_Field38 + Delimiter + _
            User_Field39 + Delimiter + _
            User_Field40 + Delimiter + _
            User_Field41 + Delimiter + _
            User_Field42 + Delimiter + _
            User_Field43 + Delimiter + _
            User_Field44 + Delimiter + _
            User_Field45 + Delimiter + _
            User_Field46 + Delimiter + _
            User_Field47 + Delimiter + _
            User_Field48 + Delimiter + _
            User_Field49 + Delimiter + _
            User_Field50 + Delimiter + _
            User_Field51 + Delimiter + _
            User_Field52 + Delimiter + _
            User_Field53 + Delimiter + _
            User_Field54 + Delimiter + _
            User_Field55 + Delimiter + _
            User_Field56 + Delimiter + _
            User_Field57 + Delimiter + _
            User_Field58 + Delimiter + _
            User_Field59 + Delimiter + _
            User_Field60
        End Function

    End Class

    Public Class UCBCategory
        Public UCB As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Sex As String
        Public Date_Of_Birth As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String

        Public Overrides Function ToString() As String
            Return UCB + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Sex + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20
        End Function

    End Class

    Public Class CBBCategory
        Public CBB As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Short_Name As String
        Public Date_Of_Birth As String
        Public Sex As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Office_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Other_Phone_Number As String
        Public Company_Name As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String
        Public User_Field31 As String
        Public User_Field32 As String
        Public User_Field33 As String

        Public Overrides Function ToString() As String
            Return CBB + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Short_Name + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Sex + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Office_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Other_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30 + Delimiter + _
            User_Field31 + Delimiter + _
            User_Field32 + Delimiter + _
            User_Field33
        End Function

    End Class

    Public Class U2BCategory
        Public U2B As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String
        Public User_Field31 As String
        Public User_Field32 As String
        Public User_Field33 As String
        Public User_Field34 As String
        Public User_Field35 As String
        Public User_Field36 As String
        Public User_Field37 As String
        Public User_Field38 As String
        Public User_Field39 As String
        Public User_Field40 As String
        Public User_Field41 As String
        Public User_Field42 As String
        Public User_Field43 As String
        Public User_Field44 As String
        Public User_Field45 As String
        Public User_Field46 As String
        Public User_Field47 As String
        Public User_Field48 As String
        Public User_Field49 As String
        Public User_Field50 As String
        Public User_Field51 As String
        Public User_Field52 As String
        Public User_Field53 As String
        Public User_Field54 As String
        Public User_Field55 As String
        Public User_Field56 As String
        Public User_Field57 As String
        Public User_Field58 As String
        Public User_Field59 As String
        Public User_Field60 As String

        Public Overrides Function ToString() As String
            Return U2B + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30 + Delimiter + _
            User_Field31 + Delimiter + _
            User_Field32 + Delimiter + _
            User_Field33 + Delimiter + _
            User_Field34 + Delimiter + _
            User_Field35 + Delimiter + _
            User_Field36 + Delimiter + _
            User_Field37 + Delimiter + _
            User_Field38 + Delimiter + _
            User_Field39 + Delimiter + _
            User_Field40 + Delimiter + _
            User_Field41 + Delimiter + _
            User_Field42 + Delimiter + _
            User_Field43 + Delimiter + _
            User_Field44 + Delimiter + _
            User_Field45 + Delimiter + _
            User_Field46 + Delimiter + _
            User_Field47 + Delimiter + _
            User_Field48 + Delimiter + _
            User_Field49 + Delimiter + _
            User_Field50 + Delimiter + _
            User_Field51 + Delimiter + _
            User_Field52 + Delimiter + _
            User_Field53 + Delimiter + _
            User_Field54 + Delimiter + _
            User_Field55 + Delimiter + _
            User_Field56 + Delimiter + _
            User_Field57 + Delimiter + _
            User_Field58 + Delimiter + _
            User_Field59 + Delimiter + _
            User_Field60
        End Function

    End Class

    Public Class UCCCategory
        Public UCC As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Sex As String
        Public Date_Of_Birth As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Company_Name As String
        Public Company_Address1 As String
        Public Company_Address2 As String
        Public Company_Address3 As String
        Public Company_Address4 As String
        Public Company_Address5 As String
        Public Company_Address6 As String
        Public Company_Postcode As String
        Public Company_Phone_Number As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String

        Public Overrides Function ToString() As String
            Return UCC + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Sex + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            Company_Address1 + Delimiter + _
            Company_Address2 + Delimiter + _
            Company_Address3 + Delimiter + _
            Company_Address4 + Delimiter + _
            Company_Address5 + Delimiter + _
            Company_Address6 + Delimiter + _
            Company_Postcode + Delimiter + _
            Company_Phone_Number + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20
        End Function

    End Class

    Public Class CBCCategory
        Public CBC As String
        Public Id_Number1 As String
        Public Id_Number2 As String
        Public Id_Number3 As String
        Public Surname As String
        Public First_Name As String
        Public Middle_Name As String
        Public Short_Name As String
        Public Date_Of_Birth As String
        Public Sex As String
        Public Home_Address1 As String
        Public Home_Address2 As String
        Public Home_Address3 As String
        Public Home_Address4 As String
        Public Home_Address5 As String
        Public Home_Address6 As String
        Public Home_Postcode As String
        Public Home_Phone_Number As String
        Public Office_Phone_Number As String
        Public Mobile_Phone_Number As String
        Public Other_Phone_Number As String
        Public Company_Name As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String
        Public User_Field31 As String
        Public User_Field32 As String
        Public User_Field33 As String

        Public Overrides Function ToString() As String
            Return CBC + Delimiter + _
            Id_Number1 + Delimiter + _
            Id_Number2 + Delimiter + _
            Id_Number3 + Delimiter + _
            Surname + Delimiter + _
            First_Name + Delimiter + _
            Middle_Name + Delimiter + _
            Short_Name + Delimiter + _
            Date_Of_Birth + Delimiter + _
            Sex + Delimiter + _
            Home_Address1 + Delimiter + _
            Home_Address2 + Delimiter + _
            Home_Address3 + Delimiter + _
            Home_Address4 + Delimiter + _
            Home_Address5 + Delimiter + _
            Home_Address6 + Delimiter + _
            Home_Postcode + Delimiter + _
            Home_Phone_Number + Delimiter + _
            Office_Phone_Number + Delimiter + _
            Mobile_Phone_Number + Delimiter + _
            Other_Phone_Number + Delimiter + _
            Company_Name + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30 + Delimiter + _
            User_Field31 + Delimiter + _
            User_Field32 + Delimiter + _
            User_Field33
        End Function

    End Class

    Public Class U2CCategory
        Public U2C As String
        Public User_Field1 As String
        Public User_Field2 As String
        Public User_Field3 As String
        Public User_Field4 As String
        Public User_Field5 As String
        Public User_Field6 As String
        Public User_Field7 As String
        Public User_Field8 As String
        Public User_Field9 As String
        Public User_Field10 As String
        Public User_Field11 As String
        Public User_Field12 As String
        Public User_Field13 As String
        Public User_Field14 As String
        Public User_Field15 As String
        Public User_Field16 As String
        Public User_Field17 As String
        Public User_Field18 As String
        Public User_Field19 As String
        Public User_Field20 As String
        Public User_Field21 As String
        Public User_Field22 As String
        Public User_Field23 As String
        Public User_Field24 As String
        Public User_Field25 As String
        Public User_Field26 As String
        Public User_Field27 As String
        Public User_Field28 As String
        Public User_Field29 As String
        Public User_Field30 As String
        Public User_Field31 As String
        Public User_Field32 As String
        Public User_Field33 As String
        Public User_Field34 As String
        Public User_Field35 As String
        Public User_Field36 As String
        Public User_Field37 As String
        Public User_Field38 As String
        Public User_Field39 As String
        Public User_Field40 As String
        Public User_Field41 As String
        Public User_Field42 As String
        Public User_Field43 As String
        Public User_Field44 As String
        Public User_Field45 As String
        Public User_Field46 As String
        Public User_Field47 As String
        Public User_Field48 As String
        Public User_Field49 As String
        Public User_Field50 As String
        Public User_Field51 As String
        Public User_Field52 As String
        Public User_Field53 As String
        Public User_Field54 As String
        Public User_Field55 As String
        Public User_Field56 As String
        Public User_Field57 As String
        Public User_Field58 As String
        Public User_Field59 As String
        Public User_Field60 As String
        Public User_Field61 As String
        Public User_Field62 As String
        Public User_Field63 As String
        Public User_Field64 As String
        Public User_Field65 As String
        Public User_Field66 As String
        Public User_Field67 As String
        Public User_Field68 As String
        Public User_Field69 As String
        Public User_Field70 As String
        Public User_Field71 As String
        Public User_Field72 As String
        Public User_Field73 As String
        Public User_Field74 As String
        Public User_Field75 As String
        Public User_Field76 As String
        Public User_Field77 As String
        Public User_Field78 As String
        Public User_Field79 As String
        Public User_Field80 As String
        Public User_Field81 As String
        Public User_Field82 As String
        Public User_Field83 As String
        Public User_Field84 As String
        Public User_Field85 As String
        Public User_Field86 As String
        Public User_Field87 As String
        Public User_Field88 As String
        Public User_Field89 As String
        Public User_Field90 As String
        Public User_Field91 As String
        Public User_Field92 As String
        Public User_Field93 As String
        Public User_Field94 As String
        Public User_Field95 As String
        Public User_Field96 As String
        Public User_Field97 As String
        Public User_Field98 As String
        Public User_Field99 As String
        Public User_Field100 As String
        Public User_Field101 As String
        Public User_Field102 As String
        Public User_Field103 As String
        Public User_Field104 As String
        Public User_Field105 As String
        Public User_Field106 As String
        Public User_Field107 As String
        Public User_Field108 As String
        Public User_Field109 As String
        Public User_Field110 As String
        Public User_Field111 As String
        Public User_Field112 As String
        Public User_Field113 As String
        Public User_Field114 As String
        Public User_Field115 As String
        Public User_Field116 As String
        Public User_Field117 As String
        Public User_Field118 As String
        Public User_Field119 As String
        Public User_Field120 As String
        Public User_Field121 As String
        Public User_Field122 As String
        Public User_Field123 As String
        Public User_Field124 As String
        Public User_Field125 As String
        Public User_Field126 As String
        Public User_Field127 As String
        Public User_Field128 As String
        Public User_Field129 As String
        Public User_Field130 As String
        Public User_Field131 As String
        Public User_Field132 As String
        Public User_Field133 As String
        Public User_Field134 As String
        Public User_Field135 As String
        Public User_Field136 As String
        Public User_Field137 As String
        Public User_Field138 As String
        Public User_Field139 As String
        Public User_Field140 As String
        Public User_Field141 As String
        Public User_Field142 As String
        Public User_Field143 As String
        Public User_Field144 As String
        Public User_Field145 As String
        Public User_Field146 As String
        Public User_Field147 As String
        Public User_Field148 As String
        Public User_Field149 As String
        Public User_Field150 As String
        Public User_Field151 As String
        Public User_Field152 As String
        Public User_Field153 As String
        Public User_Field154 As String
        Public User_Field155 As String
        Public User_Field156 As String
        Public User_Field157 As String
        Public User_Field158 As String
        Public User_Field159 As String
        Public User_Field160 As String
        Public User_Field161 As String
        Public User_Field162 As String
        Public User_Field163 As String
        Public User_Field164 As String
        Public User_Field165 As String
        Public User_Field166 As String
        Public User_Field167 As String
        Public User_Field168 As String
        Public User_Field169 As String
        Public User_Field170 As String
        Public User_Field171 As String
        Public User_Field172 As String
        Public User_Field173 As String
        Public User_Field174 As String
        Public User_Field175 As String

        Public Overrides Function ToString() As String
            Return U2C + Delimiter + _
            User_Field1 + Delimiter + _
            User_Field2 + Delimiter + _
            User_Field3 + Delimiter + _
            User_Field4 + Delimiter + _
            User_Field5 + Delimiter + _
            User_Field6 + Delimiter + _
            User_Field7 + Delimiter + _
            User_Field8 + Delimiter + _
            User_Field9 + Delimiter + _
            User_Field10 + Delimiter + _
            User_Field11 + Delimiter + _
            User_Field12 + Delimiter + _
            User_Field13 + Delimiter + _
            User_Field14 + Delimiter + _
            User_Field15 + Delimiter + _
            User_Field16 + Delimiter + _
            User_Field17 + Delimiter + _
            User_Field18 + Delimiter + _
            User_Field19 + Delimiter + _
            User_Field20 + Delimiter + _
            User_Field21 + Delimiter + _
            User_Field22 + Delimiter + _
            User_Field23 + Delimiter + _
            User_Field24 + Delimiter + _
            User_Field25 + Delimiter + _
            User_Field26 + Delimiter + _
            User_Field27 + Delimiter + _
            User_Field28 + Delimiter + _
            User_Field29 + Delimiter + _
            User_Field30 + Delimiter + _
            User_Field31 + Delimiter + _
            User_Field32 + Delimiter + _
            User_Field33 + Delimiter + _
            User_Field34 + Delimiter + _
            User_Field35 + Delimiter + _
            User_Field36 + Delimiter + _
            User_Field37 + Delimiter + _
            User_Field38 + Delimiter + _
            User_Field39 + Delimiter + _
            User_Field40 + Delimiter + _
            User_Field41 + Delimiter + _
            User_Field42 + Delimiter + _
            User_Field43 + Delimiter + _
            User_Field44 + Delimiter + _
            User_Field45 + Delimiter + _
            User_Field46 + Delimiter + _
            User_Field47 + Delimiter + _
            User_Field48 + Delimiter + _
            User_Field49 + Delimiter + _
            User_Field50 + Delimiter + _
            User_Field51 + Delimiter + _
            User_Field52 + Delimiter + _
            User_Field53 + Delimiter + _
            User_Field54 + Delimiter + _
            User_Field55 + Delimiter + _
            User_Field56 + Delimiter + _
            User_Field57 + Delimiter + _
            User_Field58 + Delimiter + _
            User_Field59 + Delimiter + _
            User_Field60 + Delimiter + _
            User_Field61 + Delimiter + _
            User_Field62 + Delimiter + _
            User_Field63 + Delimiter + _
            User_Field64 + Delimiter + _
            User_Field65 + Delimiter + _
            User_Field66 + Delimiter + _
            User_Field67 + Delimiter + _
            User_Field68 + Delimiter + _
            User_Field69 + Delimiter + _
            User_Field70 + Delimiter + _
            User_Field71 + Delimiter + _
            User_Field72 + Delimiter + _
            User_Field73 + Delimiter + _
            User_Field74 + Delimiter + _
            User_Field75 + Delimiter + _
            User_Field76 + Delimiter + _
            User_Field77 + Delimiter + _
            User_Field78 + Delimiter + _
            User_Field79 + Delimiter + _
            User_Field80 + Delimiter + _
            User_Field81 + Delimiter + _
            User_Field82 + Delimiter + _
            User_Field83 + Delimiter + _
            User_Field84 + Delimiter + _
            User_Field85 + Delimiter + _
            User_Field86 + Delimiter + _
            User_Field87 + Delimiter + _
            User_Field88 + Delimiter + _
            User_Field89 + Delimiter + _
            User_Field90 + Delimiter + _
            User_Field91 + Delimiter + _
            User_Field92 + Delimiter + _
            User_Field93 + Delimiter + _
            User_Field94 + Delimiter + _
            User_Field95 + Delimiter + _
            User_Field96 + Delimiter + _
            User_Field97 + Delimiter + _
            User_Field98 + Delimiter + _
            User_Field99 + Delimiter + _
            User_Field100 + Delimiter + _
            User_Field101 + Delimiter + _
            User_Field102 + Delimiter + _
            User_Field103 + Delimiter + _
            User_Field104 + Delimiter + _
            User_Field105 + Delimiter + _
            User_Field106 + Delimiter + _
            User_Field107 + Delimiter + _
            User_Field108 + Delimiter + _
            User_Field109 + Delimiter + _
            User_Field110 + Delimiter + _            
            User_Field111 + Delimiter + _
            User_Field112 + Delimiter + _
            User_Field113 + Delimiter + _
            User_Field114 + Delimiter + _
            User_Field115 + Delimiter + _
            User_Field116 + Delimiter + _
            User_Field117 + Delimiter + _
            User_Field118 + Delimiter + _
            User_Field119 + Delimiter + _
            User_Field120 + Delimiter + _
            User_Field121 + Delimiter + _
            User_Field122 + Delimiter + _
            User_Field123 + Delimiter + _
            User_Field124 + Delimiter + _
            User_Field125 + Delimiter + _
            User_Field126 + Delimiter + _
            User_Field127 + Delimiter + _
            User_Field128 + Delimiter + _
            User_Field129 + Delimiter + _
            User_Field130 + Delimiter + _
            User_Field131 + Delimiter + _
            User_Field132 + Delimiter + _
            User_Field133 + Delimiter + _
            User_Field134 + Delimiter + _
            User_Field135 + Delimiter + _
            User_Field136 + Delimiter + _
            User_Field137 + Delimiter + _
            User_Field138 + Delimiter + _
            User_Field139 + Delimiter + _
            User_Field140 + Delimiter + _
            User_Field141 + Delimiter + _
            User_Field142 + Delimiter + _
            User_Field143 + Delimiter + _
            User_Field144 + Delimiter + _
            User_Field145 + Delimiter + _
            User_Field146 + Delimiter + _
            User_Field147 + Delimiter + _
            User_Field148 + Delimiter + _
            User_Field149 + Delimiter + _
            User_Field150 + Delimiter + _
            User_Field151 + Delimiter + _
            User_Field152 + Delimiter + _
            User_Field153 + Delimiter + _
            User_Field154 + Delimiter + _
            User_Field155 + Delimiter + _
            User_Field156 + Delimiter + _
            User_Field157 + Delimiter + _
            User_Field158 + Delimiter + _
            User_Field159 + Delimiter + _
            User_Field160 + Delimiter + _
            User_Field161 + Delimiter + _
            User_Field162 + Delimiter + _
            User_Field163 + Delimiter + _
            User_Field164 + Delimiter + _
            User_Field165 + Delimiter + _
            User_Field166 + Delimiter + _
            User_Field167 + Delimiter + _
            User_Field168 + Delimiter + _
            User_Field169 + Delimiter + _
            User_Field170 + Delimiter + _
            User_Field171 + Delimiter + _
            User_Field172 + Delimiter + _
            User_Field173 + Delimiter + _
            User_Field174 + Delimiter + _
            User_Field175             
        End Function

    End Class

    Public Class Diary
        Public Diary As String
        Public Diary_Note As String

        Public Overrides Function ToString() As String
            Return Diary + Delimiter + Diary_Note
        End Function
    End Class

End Module
