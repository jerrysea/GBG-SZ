'Modification Date:14/09/2012
'Modified by :Avikal Bajpai
'Purpose: To Modify time in log file using the local time difference in ini file
Public Class TimeDifference

    Private mvarCurrentDateTime As Date
    Private mvarLocalTimeDifference As Long

    Public Sub New(ByVal iLocalTimeDifference As Long)

        mvarLocalTimeDifference = iLocalTimeDifference

        mvarCurrentDateTime = DateAdd(DateInterval.Minute, iLocalTimeDifference, Now)

    End Sub

    Public ReadOnly Property CurrentDateTime() As Date

        Get
            mvarCurrentDateTime = DateAdd(DateInterval.Minute, mvarLocalTimeDifference, Now())
            Return mvarCurrentDateTime
        End Get

    End Property
    Public Shared Function ConvertDateToEnglishDate(ByVal inputDate As String) As String
        Dim cultureDate As DateTime

        Try
            If System.Globalization.CultureInfo.CurrentCulture.Calendar.ToString.ToLower.EndsWith("thaibuddhistcalendar") Then
                If DateTime.TryParse(inputDate.ToString(New System.Globalization.CultureInfo("en-AU")), cultureDate) Then
                    Return cultureDate.ToString("dd/MM/yyyy")
                Else
                    Return inputDate
                End If
            Else
                Return inputDate
                ''If DateTime.TryParse(inputDate.ToString(New System.Globalization.CultureInfo("en-AU")), cultureDate) Then
                ''    Return cultureDate.ToString("dd/MM/yyyy", New System.Globalization.CultureInfo("en-AU"))
                ''Else
                ''    Return inputDate
                ''End If
            End If
        Catch ex As Exception
            Return inputDate
        End Try
    End Function

    Public Shared Function ConvertDateToBuddistDate(ByVal inputDate As DateTime) As DateTime
        Dim cultureDate As DateTime

        Try
            If System.Globalization.CultureInfo.CurrentCulture.Calendar.ToString.ToLower.EndsWith("thaibuddhistcalendar") Then
                If DateTime.TryParse(inputDate.ToString(New System.Globalization.CultureInfo("th-TH")), cultureDate) Then
                    Return cultureDate
                Else
                    Return inputDate
                End If
            Else
                Return inputDate
            End If
        Catch ex As Exception
            Return inputDate
        End Try
    End Function
    Public Shared Function ConvertDateStringToBuddistDateString(ByVal inputDate As String) As String
        Dim cultureDate As DateTime

        Try
            If IsDate(inputDate) Then
                If System.Globalization.CultureInfo.CurrentCulture.Calendar.ToString.ToLower.EndsWith("thaibuddhistcalendar") Then

                    cultureDate = DateTime.Parse(inputDate, New System.Globalization.CultureInfo("en-AU")).ToString(System.Globalization.CultureInfo.CurrentCulture)

                    Return cultureDate.ToString("dd/MM/yyyy")

                Else
                    Return inputDate
                End If
            Else
                Return inputDate
            End If

        Catch ex As Exception
            Return inputDate
        End Try
    End Function

End Class