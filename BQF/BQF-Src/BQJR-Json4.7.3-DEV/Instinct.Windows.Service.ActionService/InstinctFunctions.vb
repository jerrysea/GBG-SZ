Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO

Public Class InstinctFunctions

    Public Shared Function GetInstinctParameter(ByVal strParameterName As String, ByVal intParameterNo As Integer) As String
        Dim arParams() As SqlParameter = New SqlParameter(2) {}

        Try
            arParams(0) = New SqlParameter("@Parameter_Name", SqlDbType.VarChar, 50)
            arParams(0).Value = strParameterName
            arParams(1) = New SqlParameter("@Parameter_Number", SqlDbType.Int)
            arParams(1).Value = intParameterNo
            arParams(2) = New SqlParameter("@Parameter_Value", SqlDbType.VarChar, 8000)
            arParams(2).Direction = ParameterDirection.Output
            SqlHelper.ExecuteNonQuery(strConnection, CommandType.StoredProcedure, "USP_InstinctParameter_Specific_Select", arParams)

            Return arParams(2).Value

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetActionedApplications(ByVal sSite As String) As DataSet
        Dim arParams() As SqlParameter = New SqlParameter(0) {}

        Try
            arParams(0) = New SqlParameter("@SiteWithSpecialFunctions", SqlDbType.NVarChar)
            arParams(0).Value = sSite
            Return SqlHelper.ExecuteDataset(strConnection, CommandType.StoredProcedure, "USP_ActionWindowsService_ActionedApplications_Select", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetCriminalRecords() As DataSet
        Try
            Return SqlHelper.ExecuteDataset(strConnection, CommandType.Text, "SELECT [Serial_Number],[AppKey],[InsertedDateTime],[Data] FROM [ING_Criminal_Extract] (NOLOCK) ORDER BY [Serial_Number] ASC")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Sub DeleteActionedApplications(ByVal sSerialNumber As String)
        Dim arParams() As SqlParameter = New SqlParameter(0) {}

        Try
            arParams(0) = New SqlParameter("@SerialNumber", SqlDbType.Int)
            arParams(0).Value = CInt(sSerialNumber)

            SqlHelper.ExecuteNonQuery(strConnection, CommandType.StoredProcedure, "USP_ActionWindowsService_ActionedApplications_Delete", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function GetTriggeredRulesDefinitions(ByVal appkey As String) As DataSet

        Dim cmd As SqlClient.SqlCommand
        Dim objLocalConnection As SqlConnection = Nothing

        Try
            ' Intialize connection
            objLocalConnection = New SqlConnection(strConnection)
            objLocalConnection.Open()

            cmd = New SqlClient.SqlCommand("USP_TriggeredRules_Definitions", objLocalConnection)
            cmd.CommandTimeout = 0
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@appkey", SqlDbType.NVarChar, 34)
            cmd.Parameters(0).Value = appkey

            Dim da As SqlDataAdapter = New SqlDataAdapter()
            da.SelectCommand = cmd

            Dim ds As DataSet = New DataSet()
            da.Fill(ds)

            Return ds

        Catch ex As Exception
            Return Nothing
        Finally
            objLocalConnection.Dispose()
        End Try

    End Function

    Public Shared Sub DeleteCriminal(ByVal sSerialNumber As String)
        Try
            SqlHelper.ExecuteNonQuery(strConnection, CommandType.Text, "DELETE FROM [ING_Criminal_Extract] WHERE [Serial_Number]=" & sSerialNumber)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function GetSpecificApplicationDetails(ByVal sAppKey As String, ByVal sRulesInOutputFile As String, _
    ByVal sRulesDescInOutputFile As String, ByVal sActionCountNumberInOutputFile As String, _
    ByVal sNatureOfFraudInOutputFile As String, ByVal sDiaryInOutputFile As String, ByVal sSite As String) As DataSet
        Dim arParams() As SqlParameter = New SqlParameter(6) {}

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey
            arParams(1) = New SqlParameter("@RulesInOutputFile", SqlDbType.NVarChar, 1)
            arParams(1).Value = sRulesInOutputFile
            arParams(2) = New SqlParameter("@RulesDescriptionInOutputFile", SqlDbType.NVarChar, 1)
            arParams(2).Value = sRulesDescInOutputFile
            arParams(3) = New SqlParameter("@DiaryInOutputFile", SqlDbType.NVarChar, 1)
            arParams(3).Value = sDiaryInOutputFile
            arParams(4) = New SqlParameter("@ActionCountNbrInOutputFile", SqlDbType.NVarChar, 1)
            arParams(4).Value = sActionCountNumberInOutputFile
            arParams(5) = New SqlParameter("@NatureOfFraudInOutputFile", SqlDbType.NVarChar, 1)
            arParams(5).Value = sNatureOfFraudInOutputFile
            arParams(6) = New SqlParameter("@SiteWithSpecialFunctions", SqlDbType.NVarChar)
            arParams(6).Value = sSite

            Return SqlHelper.ExecuteDataset(strConnection, CommandType.StoredProcedure, "USP_ActionWindowsService_SpecificApplication_Select", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetApplicationTypes() As DataSet
        Try
            Return SqlHelper.ExecuteDataset(strConnection, CommandType.StoredProcedure, "USP_Definitions_ApplicationType_Select")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Sub WriteToInstinctLog(ByVal sLog_Message As String)
        Dim sInstinct_Log_Path As String

        Try
            If INI_Write_Log_File = "Y" Then
                sInstinct_Log_Path = INI_Output_Directory & "\Instinct_Action_Log_" & Format(Now, "yyyyMMdd") & INIParameter.GetINIParameterValue("Startup", "Second Service Suffix") & ".TXT"

                Call SaveTextToFile(sInstinct_Log_Path, sLog_Message)
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Sub SaveTextToFile(ByVal sFile_Full_Path As String, ByVal sText As String, Optional ByVal Overwrite As Boolean = False)
        Dim sbLog As StringBuilder
        Dim swOutput As StreamWriter

        Try
            sbLog = New StringBuilder
            sbLog.Append(sText)

            swOutput = New StreamWriter(sFile_Full_Path, True, Encoding.Unicode)
            swOutput.Write(sbLog.ToString)
            swOutput.Close()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function PadWithSpaces(ByVal sField As String, ByVal iField_Length As Short) As String
        Dim Pad_Count As Short

        Try
            PadWithSpaces = Trim(sField)

            If Len(PadWithSpaces) > iField_Length Then
                'Truncate
                PadWithSpaces = Left(PadWithSpaces, iField_Length)
            Else
                If Len(PadWithSpaces) < iField_Length Then
                    'Pad with spaces
                    Pad_Count = iField_Length - Len(PadWithSpaces)
                    PadWithSpaces = PadWithSpaces & Space(Pad_Count)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function AddDateTimeToMessage(ByVal sLog_Message As String) As String
        Return Format(Now, "dd/MM/yyyy") & " " & Format(Now, "HH:mm:ss") & " - " & sLog_Message & vbCrLf
    End Function

    Public Shared Function AddTimeStampExitStatusToOutput(ByVal strExitStatus As String, ByVal strDelimeter As String, Optional ByVal isFixed As Boolean = False) As String
        Dim dtPart As String = Now.ToString("dd/MM/yyyy HH:mm:ss.fff")
        Return IIf(isFixed, PadWithSpaces(dtPart, 23), dtPart) & strDelimeter & IIf(isFixed, PadWithSpaces(strExitStatus, 5), strExitStatus) & strDelimeter
    End Function

    Public Shared Function GetReplyTo(ByVal strAppKey As String) As String
        Dim spName As String = INI_MqSpGetReplyTo
        Dim sReturn As String = String.Empty
        Dim arParams() As SqlParameter = New SqlParameter(0) {}
        arParams(0) = New SqlParameter("@AppKey", strAppKey)
        Try
            If (spName <> String.Empty And strAppKey <> String.Empty) Then
                sReturn = SqlHelper.ExecuteScalar(strConnection, spName, arParams)
                sReturn = IIf(sReturn Is Nothing, "", sReturn)
            End If
        Catch ex As Exception

        End Try
        Return sReturn
    End Function

End Class
