Public Class TimeDifference

    Private mvarCurrentDateTime As Date
    Private mvarLocalTimeDifference As Long

    Public Sub New(ByVal iLocalTimeDifference As Long)

        mvarLocalTimeDifference = iLocalTimeDifference

        mvarCurrentDateTime = DateAdd(DateInterval.Minute, iLocalTimeDifference, Now)

    End Sub

    Public ReadOnly Property CurrentDateTime() As Date

        Get
            mvarCurrentDateTime = DateAdd(DateInterval.Minute, mvarLocalTimeDifference, Now)
            Return mvarCurrentDateTime
        End Get

    End Property

End Class
