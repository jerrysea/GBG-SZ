﻿//// Copyright (c) .NET Core Community. All rights reserved.
//// Licensed under the MIT License. See License.txt in the project root for license information.

//using System;
//using System.Data.Common;
//using System.Data.Entity;
//using DotNetFramework.CAP;

//// ReSharper disable once CheckNamespace
//namespace Microsoft.EntityFrameworkCore.Storage
//{
//    internal class CapEFDbTransaction 
//    {
//        private readonly ICapTransaction _transaction;

//        public CapEFDbTransaction(ICapTransaction transaction)
//        {
//            _transaction = transaction;
//            var dbContextTransaction = (DbContextTransaction)_transaction.DbTransaction;
//            TransactionId = dbContextTransaction.TransactionId;
//        }

//        public void Dispose()
//        {
//            _transaction.Dispose();
//        }

//        public void Commit()
//        {
//            _transaction.Commit();
//        }

//        public void Rollback()
//        {
//            _transaction.Rollback();
//        }

//        public DbTransaction GetDbTransaction()
//        {
//            //heng
//            throw new NotImplementedException();
//        }

//        public Guid TransactionId { get; }
//    }
//}