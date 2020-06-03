Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient

Public Class SPDB
#Region "Singleton"
    Private Sub New()
    End Sub

    Private Shared m_instance As SPDB = Nothing
    Public Shared ReadOnly Property Instance() As SPDB
        Get
            If m_instance Is Nothing Then
                m_instance = New SPDB()
            End If
            Return m_instance
        End Get
    End Property
#End Region

#Region "Consts"
    Private ReadOnly TOTAL_NORMAL_RULE As Integer = 25
    Private ReadOnly TOTAL_AUDIT_RULE As Integer = 50
    Private ReadOnly TOTAL_PHONE_RECORD As Integer = 10
#End Region

#Region "Methods"
    Public Function GetTriggeredRules(ByVal connstr As String, ByVal sAppKey As String, ByVal bIsAudit As Boolean) As DataSet
        Dim arParams() As SqlParameter = New SqlParameter(1) {}

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey
            arParams(1) = New SqlParameter("@IsAudit", SqlDbType.Bit)
            arParams(1).Value = bIsAudit

            Return SqlHelper.ExecuteDataset(connstr, CommandType.StoredProcedure, "USP_SPDB_TriggeredRules_Select", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function TriggeredRule(constr As String, strAppKey As String, bNormal As Boolean) As String
        Dim dsApplication As DataSet
        Dim index As Integer
        Dim result As String = String.Empty
        Dim recordCounts As Integer = TOTAL_NORMAL_RULE
        Dim isAudit As Boolean = False

        If Not bNormal Then
            recordCounts = TOTAL_AUDIT_RULE
            isAudit = True
        End If

        dsApplication = GetTriggeredRules(constr, strAppKey, isAudit)

        If dsApplication.Tables(0).Rows.Count > 0 Then
            If dsApplication.Tables(0).Columns.Count <> 1 Then
                ' No rules triggered
                For i As Integer = 1 To recordCounts
                    result += INI_Delimiter_Character
                Next
            Else
                If dsApplication.Tables(0).Rows.Count <= recordCounts Then
                    For Each drRulesDescription As DataRow In dsApplication.Tables(0).Rows
                        result += IIf(Convert.IsDBNull(drRulesDescription(0)), "", drRulesDescription(0)) + INI_Delimiter_Character
                    Next

                    For index = dsApplication.Tables(0).Rows.Count + 1 To recordCounts
                        result += INI_Delimiter_Character
                    Next
                Else
                    index = 0
                    For Each drRulesDescription As DataRow In dsApplication.Tables(0).Rows
                        index += 1
                        If index > recordCounts Then
                            Exit For
                        End If
                        result += IIf(Convert.IsDBNull(drRulesDescription(0)), "", drRulesDescription(0)) + INI_Delimiter_Character
                    Next
                End If
            End If
        Else
            For i As Integer = 1 To recordCounts
                result += INI_Delimiter_Character
            Next
        End If
        Return result
    End Function

    Public Function GetPhoneRecords(ByVal connstr As String, ByVal sAppKey As String) As DataSet
        Dim arParams() As SqlParameter = New SqlParameter(0) {}

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey

            Return SqlHelper.ExecuteDataset(connstr, CommandType.StoredProcedure, "USP_SPDB_PhoneRecords_Select", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function PhoneRecords(constr As String, strAppKey As String) As String
        Dim dsApplication As DataSet
        Dim result As String = String.Empty
        Dim index As Integer

        dsApplication = GetPhoneRecords(constr, strAppKey)

        If dsApplication.Tables(0).Rows.Count > 0 Then
            If dsApplication.Tables(0).Columns.Count <> 2 Then
                ' No rules triggered
                For i As Integer = 1 To TOTAL_PHONE_RECORD
                    result += INI_Delimiter_Character
                    result += INI_Delimiter_Character
                Next
            Else
                If dsApplication.Tables(0).Rows.Count <= TOTAL_PHONE_RECORD Then
                    For Each drRulesDescription As DataRow In dsApplication.Tables(0).Rows
                        result += IIf(Convert.IsDBNull(drRulesDescription(0)), "", Convert.ToDateTime(drRulesDescription(0)).ToString("yyyy-MM-dd HH:mm:ss")) + INI_Delimiter_Character
                        result += IIf(Convert.IsDBNull(drRulesDescription(1)), "", drRulesDescription(1)) + INI_Delimiter_Character
                    Next

                    For index = dsApplication.Tables(0).Rows.Count + 1 To TOTAL_PHONE_RECORD
                        result += INI_Delimiter_Character
                        result += INI_Delimiter_Character
                    Next
                Else
                    index = 0
                    For Each drRulesDescription As DataRow In dsApplication.Tables(0).Rows
                        index += 1
                        If index > TOTAL_PHONE_RECORD Then
                            Exit For
                        End If
                        result += IIf(Convert.IsDBNull(drRulesDescription(0)), "", Convert.ToDateTime(drRulesDescription(0)).ToString("yyyy-MM-dd HH:mm:ss")) + INI_Delimiter_Character
                        result += IIf(Convert.IsDBNull(drRulesDescription(1)), "", drRulesDescription(1)) + INI_Delimiter_Character
                    Next
                End If
            End If
        Else
            For i As Integer = 1 To TOTAL_PHONE_RECORD
                result += INI_Delimiter_Character
                result += INI_Delimiter_Character
            Next
        End If
        Return result
    End Function
#End Region
End Class
