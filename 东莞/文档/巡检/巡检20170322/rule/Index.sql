SELECT TOP 100
                MID.statement AS ObjectName ,
                MID.equality_columns ,
                MID.inequality_columns ,
                MID.included_columns ,
                MIGS.avg_user_impact AS ExpectedPerformanceImprovement ,
                ( MIGS.user_seeks + MIGS.user_scans )
                * MIGS.avg_total_user_cost * MIGS.avg_user_impact AS PossibleImprovement
        FROM    sys.dm_db_missing_index_details AS MID
                INNER JOIN sys.dm_db_missing_index_groups AS MIG ON MID.index_handle = MIG.index_handle
                INNER JOIN sys.dm_db_missing_index_group_stats AS MIGS ON MIG.index_group_handle = MIGS.group_handle;