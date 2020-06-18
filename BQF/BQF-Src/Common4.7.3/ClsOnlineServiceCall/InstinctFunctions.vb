Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Imports System.Text

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
            SqlHelper.ExecuteNonQuery(connStr, CommandType.StoredProcedure, "USP_InstinctParameter_Specific_Select", arParams)

            Return arParams(2).Value

        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function GetLastAutoApplicationNumber(ByVal sAutoGenerateText As String) As Integer

        Dim dsAutoGenerate As DataSet

        Dim sApplicationNumber As String

        '*
        '* Get the last auto-generated application number on the database
        '*

        GetLastAutoApplicationNumber = 0

        sAutoGenerateText = sAutoGenerateText & Format(objTimeDifference.CurrentDateTime, "yyyyMMdd")

        Try

            'Check the Criminal Application table for any auto-generated application numbers
            dsAutoGenerate = GetCriminalTodayApplicationNumber(sAutoGenerateText)

            If Not dsAutoGenerate Is Nothing AndAlso dsAutoGenerate.Tables.Count > 0 AndAlso dsAutoGenerate.Tables(0).Rows.Count > 0 Then

                For Each drAutoGenerate As DataRow In dsAutoGenerate.Tables(0).Rows
                    If IsDBNull(drAutoGenerate.Item("Application_Number")) = False Then
                        sApplicationNumber = Replace(drAutoGenerate.Item("Application_Number").ToString.Trim, sAutoGenerateText, "")
                    Else
                        sApplicationNumber = ""
                    End If

                    If Trim(sApplicationNumber) <> "" Then
                        If IsNumeric(sApplicationNumber) = True Then
                            If CInt(sApplicationNumber) > GetLastAutoApplicationNumber Then
                                GetLastAutoApplicationNumber = CInt(sApplicationNumber)
                            End If
                        End If
                    End If
                Next
            End If

            dsAutoGenerate = Nothing

            'Check the Application Errors table for any auto-generated application numbers
            dsAutoGenerate = GetApplicationErrorsTodayApplicationNumber(sAutoGenerateText)

            If Not dsAutoGenerate Is Nothing AndAlso dsAutoGenerate.Tables.Count > 0 AndAlso dsAutoGenerate.Tables(0).Rows.Count > 0 Then

                For Each drAutoGenerate As DataRow In dsAutoGenerate.Tables(0).Rows
                    If IsDBNull(drAutoGenerate.Item("Application_Number")) = False Then
                        sApplicationNumber = Replace(drAutoGenerate.Item("Application_Number").ToString.Trim, sAutoGenerateText, "")
                    Else
                        sApplicationNumber = ""
                    End If

                    If Trim(sApplicationNumber) <> "" Then
                        If IsNumeric(sApplicationNumber) = True Then
                            If CInt(sApplicationNumber) > GetLastAutoApplicationNumber Then
                                GetLastAutoApplicationNumber = CInt(sApplicationNumber)
                            End If
                        End If
                    End If

                Next
            End If

            dsAutoGenerate = Nothing

        Catch ex As Exception

            GetLastAutoApplicationNumber = 0

        End Try

    End Function

    Public Shared Function GetCriminalTodayApplicationNumber(ByVal sAutoGenerateText As String) As DataSet
        Dim arParams As SqlParameter = New SqlParameter

        Try
            arParams = New SqlParameter("@Auto_Generate_Text", SqlDbType.VarChar, 50)
            arParams.Value = sAutoGenerateText

            Return SqlHelper.ExecuteDataset(connStr, CommandType.StoredProcedure, "[USP_Criminal_Today_Application_Number]", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetApplicationErrorsTodayApplicationNumber(ByVal sAutoGenerateText As String) As DataSet
        Dim arParams As SqlParameter = New SqlParameter

        Try
            arParams = New SqlParameter("@Auto_Generate_Text", SqlDbType.VarChar, 50)
            arParams.Value = sAutoGenerateText

            Return SqlHelper.ExecuteDataset(connStr, CommandType.StoredProcedure, "[USP_ApplicationErrors_Today_Application_Number]", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetApplicationTypes() As DataSet
        Try
            Dim oDs As New DataSet
            oDs = SqlHelper.ExecuteDataset(connStr, CommandType.StoredProcedure, "USP_Common_ApplicationTypes_Select")
            If Not oDs Is Nothing Then
                Return oDs
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getScoreCard() As DataSet
        Try
            Return SqlHelper.ExecuteDataset(connStr, CommandType.StoredProcedure, "[USP_Common_Scorecard_Select]")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Sub WriteToInstinctLog(ByVal sLog_Message As String, Optional ByVal noDateTime As Boolean = False)

        If INI_Write_Log_File = "Y" Then

            Dim sInstinct_Log_Path As String

            sInstinct_Log_Path = INI_Output_Directory & "\###Instinct_Log_" & Format(objTimeDifference.CurrentDateTime, "yyyyMMdd") & INIParameter.GetINIParameterValue("Startup", "Second Service Suffix") & ".TXT"

            If Not noDateTime Then
                sLog_Message = Format(objTimeDifference.CurrentDateTime, "dd/MM/yyyy") & " " & Format(objTimeDifference.CurrentDateTime, "HH:mm:ss") & " - " & sLog_Message
            End If

            Call SaveTextToFile(sInstinct_Log_Path, sLog_Message)

        End If

    End Sub

    Public Shared Function AddDateTimeToMessage(ByVal sLog_Message As String) As String

        Return Format(objTimeDifference.CurrentDateTime, "dd/MM/yyyy") & " " & IIf(bolMillisecondsInLogFile, Format(objTimeDifference.CurrentDateTime, "HH:mm:ss.fff"), Format(objTimeDifference.CurrentDateTime, "HH:mm:ss")) & " - " & sLog_Message & vbCrLf

    End Function

    Public Shared Sub SaveTextToFile(ByVal sFile_Full_Path As String, ByVal sText As String, Optional ByVal Overwrite As Boolean = False)

        'Dim iFile_Number As Short

        Try
            'iFile_Number = FreeFile()

            'If Overwrite Then
            '    FileOpen(iFile_Number, sFile_Full_Path, OpenMode.Output)
            'Else
            '    FileOpen(iFile_Number, sFile_Full_Path, OpenMode.Append)
            'End If

            'PrintLine(iFile_Number, sText)


            If INI_Input_Format = "UNICODE" Then
                'for unicode country, set output file as unicode to allow second language character
                LogOutputFile = New IO.StreamWriter(sFile_Full_Path, True, Encoding.Unicode)
            Else
                LogOutputFile = New IO.StreamWriter(sFile_Full_Path, True, Encoding.Default)
            End If
            '-----------------------------------------------------------------------------------

            LogFile = New System.Text.StringBuilder

            LogFile.Append(sText & vbCrLf)
            LogOutputFile.Write(LogFile.ToString)
            LogOutputFile.Close()
            LogOutputFile = Nothing
            LogFile = Nothing


        Catch ex As Exception
            'FileClose(iFile_Number)
        End Try
    End Sub

    Public Shared Function PadWithSpaces(ByVal sField As String, ByVal iField_Length As Short) As String

        Dim Pad_Count As Short

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

    End Function

    Public Shared Sub Update_Assignment(ByVal sAppKey As String, Optional ByVal iNewApplicationAge As Integer = 1)
        Dim arParams() As SqlParameter = New SqlParameter(1) {}

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey
            arParams(1) = New SqlParameter("@NewApplicationsAge", SqlDbType.Int)
            arParams(1).Value = iNewApplicationAge

            SqlHelper.ExecuteDataset(connstr, CommandType.StoredProcedure, "[USP_Common_ProcessManager_Assignment_Update]", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub Auto_Action(ByVal sAppKey As String, ByVal strSpecialFunction As String, ByVal strActionOutputFile As String, ByVal strLoadMode As String)
        Dim arParams() As SqlParameter = New SqlParameter(3) {}
       

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey

            arParams(1) = New SqlParameter("@SiteWithSpecialFunctions", SqlDbType.NVarChar, 25)
            arParams(1).Value = strSpecialFunction

            arParams(2) = New SqlParameter("@ActionOutputFile", SqlDbType.Int)
            If String.IsNullOrEmpty(strActionOutputFile) Then
                arParams(2).Value = 0
            ElseIf strActionOutputFile.Equals("Y") Then
                arParams(2).Value = 1
            Else
                arParams(2).Value = 0
            End If


            arParams(3) = New SqlParameter("@Load_Mode", SqlDbType.NVarChar, 1)
            arParams(3).Value = strLoadMode

            SqlHelper.ExecuteDataset(connstr, CommandType.StoredProcedure, "[USP_Common_Auto_Action_Update]", arParams)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function Get_User_Defined_Alert(ByVal sAppKey As String) As String
        Dim arParams() As SqlParameter = New SqlParameter(1) {}
        Dim dsApplication As DataSet

        Try
            arParams(0) = New SqlParameter("@AppKey", SqlDbType.NVarChar, 34)
            arParams(0).Value = sAppKey
            arParams(1) = New SqlParameter("@Database", SqlDbType.NVarChar, 1)
            arParams(1).Value = "A"

            dsApplication = SqlHelper.ExecuteDataset(connStr, CommandType.StoredProcedure, "USP_Common_Applications_UserDefinedAlert_Select", arParams)

            If dsApplication IsNot Nothing AndAlso dsApplication.Tables.Count > 0 _
            AndAlso dsApplication.Tables(0).Rows.Count > 0 _
            AndAlso Not Convert.IsDBNull(dsApplication.Tables(0).Rows(0)(0)) Then
                Return dsApplication.Tables(0).Rows(0)(0).ToString.Trim
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw ex
            Return ""
        End Try
    End Function

    Public Shared Function AddTimeStampExitStatusToOutput(ByVal strExitStatus As String, ByVal strDelimeter As String, Optional ByVal isFixed As Boolean = False) As String
        Dim dtPart As String = Now.ToString("dd/MM/yyyy HH:mm:ss.fff")
        Return IIf(isFixed, PadWithSpaces(dtPart, 23), dtPart) & strDelimeter & IIf(isFixed, PadWithSpaces(strExitStatus, 5), strExitStatus) & strDelimeter
    End Function

End Class
