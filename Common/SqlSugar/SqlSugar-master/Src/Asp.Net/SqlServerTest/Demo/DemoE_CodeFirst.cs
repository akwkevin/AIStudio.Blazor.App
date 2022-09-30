﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrmTest
{
    public class DemoE_CodeFirst
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### CodeFirst Start ####");
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.SqlServer,
                ConnectionString = Config.ConnectionString3,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            db.Aop.OnLogExecuted = (s, p) =>
            {
                Console.WriteLine(s);
            };
            db.DbMaintenance.CreateDatabase();
            db.CodeFirst.InitTables(typeof(CodeFirstTable1));//Create CodeFirstTable1 
            db.DbMaintenance.TruncateTable<CodeFirstTable1>();
            db.Insertable(new CodeFirstTable1() { Name = "a", Text="a",CreateTime=DateTime.Now }).ExecuteCommand();
            db.Insertable(new CodeFirstTable1() { Name = "a", Text = "a", CreateTime = DateTime.Now.AddDays(1) }).ExecuteCommand();
            var list = db.Queryable<CodeFirstTable1>().ToList();
            db.CodeFirst.InitTables<UnituLong>();
            db.Insertable(new UnituLong() { longx = 1 }).ExecuteCommand();
            var ulList=db.Queryable<UnituLong>().Where(x => x.longx > 0).ToList();
            db.CodeFirst.As<UnituLong>("UnituLong0011").InitTables<UnituLong>();
            Console.WriteLine("#### CodeFirst end ####");
        }
    }

    public class UnituLong
    {

        public ulong longx{get;set;}
    }


    [SugarIndex("index_codetable1_name",nameof(CodeFirstTable1.Name),OrderByType.Asc)]
    [SugarIndex("index_codetable1_nameid", nameof(CodeFirstTable1.Name), OrderByType.Asc,nameof(CodeFirstTable1.Id),OrderByType.Desc)]
    [SugarIndex("unique_codetable1_CreateTime", nameof(CodeFirstTable1.CreateTime), OrderByType.Desc,true)]
    public class CodeFirstTable1
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        [SugarColumn(ColumnDataType = "Nvarchar(255)")]//custom
        public string Text { get; set; }
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; }
    }
}
