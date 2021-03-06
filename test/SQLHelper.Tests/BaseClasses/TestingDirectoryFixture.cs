﻿using BigBook;
using BigBook.Registration;
using FileCurator;
using FileCurator.Registration;
using Microsoft.Extensions.DependencyInjection;
using SQLHelper.ExtensionMethods;
using SQLHelper.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using Xunit;

namespace SQLHelper.Tests.BaseClasses
{
    [Collection("DirectoryCollection")]
    public class TestingDirectoryFixture : IDisposable
    {
        public TestingDirectoryFixture()
        {
            if (Canister.Builder.Bootstrapper == null)
                Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                    .AddAssembly(typeof(TestingDirectoryFixture).GetTypeInfo().Assembly)
                    .RegisterSQLHelper()
                    .RegisterFileCurator()
                    .Build();

            using (var TempConnection = SqlClientFactory.Instance.CreateConnection())
            {
                TempConnection.ConnectionString = "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false";
                using (var TempCommand = TempConnection.CreateCommand())
                {
                    try
                    {
                        TempCommand.CommandText = "Create Database TestDatabase";
                        TempCommand.Open();
                        TempCommand.ExecuteNonQuery();
                    }
                    finally { TempCommand.Close(); }
                }
            }
            using (var TempConnection = SqlClientFactory.Instance.CreateConnection())
            {
                TempConnection.ConnectionString = "Data Source=localhost;Initial Catalog=TestDatabase;Integrated Security=SSPI;Pooling=false";
                using (var TempCommand = TempConnection.CreateCommand())
                {
                    try
                    {
                        TempCommand.CommandText = "Create Table TestTable(ID INT PRIMARY KEY IDENTITY,StringValue1 NVARCHAR(100),StringValue2 NVARCHAR(MAX),BigIntValue BIGINT,BitValue BIT,DecimalValue DECIMAL(12,6),FloatValue FLOAT,DateTimeValue DATETIME,GUIDValue UNIQUEIDENTIFIER,TimeSpanValue TIME(7))";
                        TempCommand.Open();
                        TempCommand.ExecuteNonQuery();
                    }
                    finally { TempCommand.Close(); }
                }
            }
        }

        public void Dispose()
        {
            using (var TempConnection = SqlClientFactory.Instance.CreateConnection())
            {
                TempConnection.ConnectionString = "Data Source=localhost;Initial Catalog=master;Integrated Security=SSPI;Pooling=false";
                using (var TempCommand = TempConnection.CreateCommand())
                {
                    try
                    {
                        TempCommand.CommandText = "ALTER DATABASE TestDatabase SET OFFLINE WITH ROLLBACK IMMEDIATE\r\nALTER DATABASE TestDatabase SET ONLINE\r\nDROP DATABASE TestDatabase";
                        TempCommand.Open();
                        TempCommand.ExecuteNonQuery();
                    }
                    finally { TempCommand.Close(); }
                }
            }
        }
    }
}