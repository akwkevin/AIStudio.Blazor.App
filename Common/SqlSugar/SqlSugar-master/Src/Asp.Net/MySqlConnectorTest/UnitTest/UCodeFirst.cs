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
            Db.CodeFirst.InitTables<UnitCodeTest1>();
            Db.CodeFirst.InitTables<UnitCodeTest2222>();
            Db.Insertable(new UnitCodeTest2222()
            {
                Id = 1,
                Id2 = 2,
                Id3 = 3,
                Id4 = 4
            }).ExecuteCommand();
            var list = Db.Queryable<UnitCodeTest2222>().ToList();
            Db.CodeFirst.InitTables<UnitCodeTest2a2c22>();
            Db.Insertable(new UnitCodeTest2a2c22()
            {
                a = 1,
                b = new byte[] { 1, 2, 3 }
            })
            .ExecuteCommand();
            var xx=Db.Queryable<UnitCodeTest2a2c22>().Select(it => new
            {
                id=it.a,
                b=it.b
            }).ToList();
            Db.CodeFirst.InitTables<UnitTest012213>();
            Db.CodeFirst.InitTables<UnitTest3131>();
            Db.CodeFirst.InitTables<UnitDateOfTime2>();
            Db.CodeFirst.InitTables<UnitDateOfTime222>();
            Db.Insertable(new UnitDateOfTime2() { DateTimeOffset1 = DateTimeOffset.Now }).ExecuteCommand();
            Db.Insertable(new List<UnitDateOfTime2> { new UnitDateOfTime2() { DateTimeOffset1 = DateTimeOffset.Now }, new UnitDateOfTime2() { DateTimeOffset1 = DateTimeOffset.Now } }).ExecuteCommand();
            var list2 = Db.Queryable<UnitDateOfTime2>().ToList();
            Db.Insertable(new UnitDateOfTime222() { DateTimeOffset1 = null }).ExecuteCommand();

        }

        public class UnitDateOfTime2
        {
            [SqlSugar.SugarColumn(ColumnDataType ="datetime(3)")]
            public DateTimeOffset DateTimeOffset1 { get; set; }
        }
        public class UnitDateOfTime222
        {
            [SqlSugar.SugarColumn(ColumnDataType = "datetime(3)",IsNullable =true)]
            public DateTimeOffset? DateTimeOffset1 { get; set; }
        }
        public class UnitTest3131 
        {
            public sbyte Id { get; set; }
        }
        public class UnitTest012213
        {

            [SugarColumn(ColumnDataType = "image,longblob")]
            public byte[] x { get; set; }
        }
        public class UnitCodeTest1
        {
            [SqlSugar.SugarColumn(IndexGroupNameList = new string[] { "group1" })]
            public int Id { get; set; }
            [SqlSugar.SugarColumn(DefaultValue="now()", IndexGroupNameList =new string[] {"group1" } )]
            public DateTime? CreateDate { get; set; }
        }

        public class UnitCodeTest2a2c22
        {
            public int a { get; set; }
            [SqlSugar.SugarColumn(ColumnDataType ="blob")]
            public byte[] b { get; set; }
        }

        public class UnitCodeTest2222
        {
            public uint Id { get; set; }
            public ulong Id2 { get; set; }
            public ushort Id3 { get; set; }
            public uint? Id4 { get; set; }
        }
    }
}
