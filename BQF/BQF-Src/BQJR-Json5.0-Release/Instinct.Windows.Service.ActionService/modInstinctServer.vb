Imports System.Threading

Module modInstinctServer

    ' Instinct INI Parameters
    Public INI_Value As String
    Public INI_Use_Windows_Authentication As String
    Public INI_Database_User_Id As String
    Public INI_Database_Password As String
    Public INI_Data_Source As String
    Public INI_Initial_Catalog As String
    Public INI_Default_Country As String
    Public INI_Organisation As String
    Public INI_Group_Member_Code As String
    Public INI_Delimiter_Character As String
    Public INI_Pooling_Interval As Short
    Public INI_Output_Directory As String
    Public INI_Output_Format As String
    Public INI_User_Defined_Alert_in_Output_File As String
    Public INI_User_Id_in_Output_File As String
    Public INI_Rules_in_Output_File As String
    Public INI_Rules_Description_in_Output_File As String
    Public INI_Action_Count_Number_in_Output_File As String
    Public INI_Nature_Of_Fraud_in_Output_File As String
    Public INI_Decision_Reason_in_Output_File As String
    Public INI_Diary_in_Output_File As String
    Public INI_Site_With_Special_Functions As String
    Public INI_Second_Service_Suffix As String
    Public INI_Write_Log_File As String
    Public INI_Reply_Flag As String
    ''' <summary>
    ''' 传输方法 MQ/Webservice
    ''' </summary>
    ''' <remarks></remarks>
    Public INI_Transfer_Way As String
    ''' <summary>
    ''' 输出格式
    ''' XML格式/TXT格式/JASON格式
    ''' </summary>
    ''' <remarks></remarks>
    Public INI_Output_Layout As String
    Public INI_Return_Action_By_Application_Type As String
    Public INI_Fraud_Alert_UserId_in_Output_File As String
    Public INI_URL As String
    Public INI_Class As String
    Public INI_Method As String
    Public INI_Criminal_URL As String
    Public INI_Criminal_Class As String
    Public INI_Criminal_Method As String
    ''' <summary>
    ''' Mq Section
    ''' </summary>
    ''' <remarks></remarks>
    Public INI_MqHost As String
    Public INI_MqVHost As String
    Public INI_MqExchange As String
    Public INI_MqQueue As String
    Public INI_MqPort As Integer
    Public INI_MqUser As String
    Public INI_MqPassword As String

    Public INI_MqEncoding As String
    Public INI_MqResponseMethod As String
    Public INI_MqNeedDeclareQueue As Boolean
    Public INI_MqSpGetReplyTo As String
    ''' ============================
    ''' ============================
    Public INI_Criminal_MqHost As String
    Public INI_Criminal_MqVHost As String
    Public INI_Criminal_MqExchange As String
    Public INI_Criminal_MqQueue As String
    Public INI_Criminal_MqPort As Integer
    Public INI_Criminal_MqUser As String
    Public INI_Criminal_MqPassword As String

    Public INI_Low_Fraud_Score As Short
    Public INI_AuthorizerID As String
    Public INI_Retry_Delay As Integer
    Public INI_Application_Name As String
    Public INI_REST_Inteface As String

    Public INI_ActonSkip_ApplicationTypes As ArrayList

    ' Connection String
    Public strConnection As String

    ' Thread
    Public ActionThread As Thread
    Public CriminalThread As Thread

    ' Web service for each Application Type
    Public Application_Type As New ArrayList

    Public URL As New ArrayList
    Public Class_Name As New ArrayList
    Public Method_Name As New ArrayList

    Public MqHost As New ArrayList
    Public MqVHost As New ArrayList
    Public MqExchange As New ArrayList
    Public MqQueue As New ArrayList
    Public MqPort As New ArrayList
    Public MqUser As New ArrayList
    Public MqPassword As New ArrayList

    ' Others
    Public Const CatServices As Integer = 4
    Public Const evtNumber As Short = 1

    ' Delayed records
    Public Class RetryDelayRecord
        Public SerialNo As String
        Public RetryDateTime As DateTime

        Public Sub New(ByVal SerialNo As String, ByVal RetryDateTime As String)
            Me.SerialNo = SerialNo
            Me.RetryDateTime = RetryDateTime
        End Sub

        Public Function ReadyToRetry() As Boolean
            If (Me.RetryDateTime <= DateTime.Now()) Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class

    Public RetryDelayList As New List(Of RetryDelayRecord)

End Module
