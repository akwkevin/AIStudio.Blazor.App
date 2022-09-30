﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmTest
{
    public partial class NewUnitTest
    {
        public static void CodeFirst()
        {
            if (Db.DbMaintenance.IsAnyTable("UnitCodeTest1", false))
                Db.DbMaintenance.DropTable("UnitCodeTest1");
            Demo2();
            Demo3();
            Db.CodeFirst.InitTables<TESTA1>();
            var xx = Db.DbMaintenance.GetColumnInfosByTableName("TESTA1", false);
            Db.DbMaintenance.AddColumn("TESTA1", new DbColumnInfo()
            {
                DbColumnName = "aaa" + new Random().Next(2, 99999),
                PropertyName = "aaa" + new Random().Next(2, 99999),
                TableName = "TESTA1",
                IsNullable = true,
                DataType = "varchar",
                Length = 11
            });
            // db.Utilities.RemoveCacheAll();
            var xxxxx = Db.DbMaintenance.GetColumnInfosByTableName("TESTA1", false);
            Db.CodeFirst.InitTables<UnitDateOfTime22>();
            Db.Insertable(new UnitDateOfTime22() { DateTimeOffset1 = DateTimeOffset.Now }).ExecuteCommand();
            Db.Insertable(new List<UnitDateOfTime22> { new UnitDateOfTime22() { DateTimeOffset1 = DateTimeOffset.Now }, new UnitDateOfTime22() { DateTimeOffset1 = DateTimeOffset.Now } }).ExecuteCommand();
            var list2 = Db.Queryable<UnitDateOfTime22>().ToList();

            if (Db.DbMaintenance.IsAnyTable("UnitCodeFirst131", false))
                Db.DbMaintenance.DropTable("UnitCodeFirst131");
            Db.CodeFirst.InitTables<UnitCodeFirst131>();
            Db.Insertable(new UnitCodeFirst131() { Id = 1 }).ExecuteCommand();
            Db.CodeFirst.InitTables<UNITCODEFIRST131>();
            var xxx = Db.Queryable<UNITCODEFIRST131>().ToList();
            Db.CodeFirst.InitTables<UNITCOdEFIRST131>();
            Db.CodeFirst.InitTables<UnitADFA13131>();
            if (Db.DbMaintenance.IsAnyTable("UnitIndextest", false))
                Db.DbMaintenance.DropTable("UnitIndextest");
            Db.CodeFirst.InitTables<UnitIndextest>();
        }
        [SqlSugar.SugarIndex("UnitIndextestIndex2", nameof(UnitIndextest.Table1), SqlSugar.OrderByType.Asc)]
        public class UnitIndextest
        {
            public string Table1  { get; set; }
            public string Id { get; set; }
        }
        public class UnitADFA13131 
        {
            [SugarColumn(ColumnDescription = "order")]
            public string order { get; set; }
        }
        public class UnitCodeFirst131
        {
            public int Id { get; set; }
        }
        public class UNITCODEFIRST131
        {
            public int Id { get; set; }
            [SqlSugar.SugarColumn(DefaultValue = "a")]
            public string Name { get; set; }
        }
        public class UNITCOdEFIRST131
        {
            public int Id { get; set; }
            [SqlSugar.SugarColumn(DefaultValue = "a")]
            public string Name { get; set; }
            [SqlSugar.SugarColumn(DefaultValue = "0")]
            public int dt { get; set; }
        }
        [SqlSugar.SugarTable("UnitDateOfTime2211")]
        public class UnitDateOfTime22
        {
            public DateTimeOffset DateTimeOffset1 { get; set; }
        }
        public class TESTA1 { 
           public string X { get; set; }
        }

        private static void Demo2()
        {
            Db.CodeFirst.InitTables<UnitCodeTest1>();
            Db.CodeFirst.InitTables<Unitasdfa1>();
            Db.Insertable(new Unitasdfa1() { t1 = 1, t2 = 1 }).ExecuteCommand();
            var x = Db.Queryable<Unitasdfa1>().ToList();
            if (x.First().t1 == 0 || x.First().t2 == 0)
            {
                throw new Exception("UnitCodeTest1");
            }
        }
        private static void Demo3()
        {
            Db.CodeFirst.InitTables<Unitasdfa2>();
            Db.CodeFirst.InitTables<Unitasdfa2>();
            Db.Insertable(new Unitasdfa2() { t1 = 1, t2 = 1 }).ExecuteCommand();
            var x = Db.Queryable<Unitasdfa2>().ToList();
            if (x.First().t1 == 0 || x.First().t2 == 0)
            {
                throw new Exception("UnitCodeTest1");
            }
        }

        public class Unitasdfa1
        {
            [SqlSugar.SugarColumn(ColumnDataType = "number(8,2)")]
            public decimal t2 { get; set; }

            [SqlSugar.SugarColumn(ColumnDataType ="number(8,2)")]
            public decimal? t1 { get; set; }
        }
        public class Unitasdfa2
        {
            [SqlSugar.SugarColumn(ColumnDataType = "number(8,3)")]
            public decimal t2 { get; set; }

            [SqlSugar.SugarColumn(ColumnDataType = "number(8,3)")]
            public decimal? t1 { get; set; }
        }
        public class UnitCodeTest1
        {
            [SqlSugar.SugarColumn(IndexGroupNameList = new string[] { "group1" })]
            public int Id { get; set; }
            [SqlSugar.SugarColumn(DefaultValue="sysdate", IndexGroupNameList =new string[] {"group1" } )]
            public DateTime? CreateDate { get; set; }
        }
    }
}
