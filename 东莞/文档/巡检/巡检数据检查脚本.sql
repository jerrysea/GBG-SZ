select @@version
--查看所用表数量
sp_spaceused A_Application
go
sp_spaceused A_Applicant
GO
sp_spaceused A_Accountant_Solicitor
go
sp_spaceused A_Guarantor
go
sp_spaceused A_Introducer_Agent
go
sp_spaceused A_Reference
GO
sp_spaceused A_Security
go
sp_spaceused A_PreviousAddress_Company
go
sp_spaceused A_Valuer
go
sp_spaceused A_User
go
sp_spaceused A_CreditBureau
go
sp_spaceused A_User2
go
sp_spaceused A_UCA
go
sp_spaceused A_CBA
go
sp_spaceused A_U2A
go
sp_spaceused A_UCB
go
sp_spaceused A_CBB
go
sp_spaceused A_U2B
go
sp_spaceused A_UCC
go
sp_spaceused A_CBC
go
sp_spaceused A_U2C
go
sp_spaceused A_Diary_Notes
go
sp_spaceused Audit_Log
go
sp_spaceused C_Application
go
sp_spaceused C_Applicant
GO
sp_spaceused C_Accountant_Solicitor
go
sp_spaceused C_Guarantor
go
sp_spaceused C_Introducer_Agent
go
sp_spaceused C_Reference
GO
sp_spaceused C_Security
go
sp_spaceused C_PreviousAddress_Company
go
sp_spaceused C_Valuer
go
sp_spaceused C_User
go
sp_spaceused C_CreditBureau
go
sp_spaceused C_User2
go
sp_spaceused C_UCA
go
sp_spaceused C_CBA
go
sp_spaceused C_U2A
go
sp_spaceused C_UCB
go
sp_spaceused C_CBB
go
sp_spaceused C_U2B
go
sp_spaceused C_UCC
go
sp_spaceused C_CBC
go
sp_spaceused C_U2C
go
sp_spaceused Matched_Applications
go
sp_spaceused Matched_Applications_History
go
--查看按月份申请件增加量
SELECT COUNT(AppKey) AS '数量',CONVERT(VARCHAR(7),Application_Date,121) AS '月份' FROM dbo.A_Application(NOLOCK) GROUP BY CONVERT(VARCHAR(7),Application_Date,121) ORDER BY CONVERT(VARCHAR(7),Application_Date,121)


