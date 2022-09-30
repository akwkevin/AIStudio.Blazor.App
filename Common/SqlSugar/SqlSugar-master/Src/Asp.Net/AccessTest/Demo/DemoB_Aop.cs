﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmTest
{
    public class DemoB_Aop
    {
        public static void Init()
        {
            Console.WriteLine("");
            Console.WriteLine("#### Aop Start ####");

            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.Access,
                ConnectionString = Config.ConnectionString,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });
            db.Aop.OnLogExecuted = (sql, pars) => //SQL executed event
            {
                Console.WriteLine("OnLogExecuted"+sql);
            };
            db.Aop.OnLogExecuting = (sql, pars) => //SQL executing event (pre-execution)
            {
                Console.WriteLine("OnLogExecuting" + sql);
            };
            db.Aop.OnError = (exp) =>//SQL execution error event
            {
                //exp.sql             
            };
            db.Aop.OnExecutingChangeSql = (sql, pars) => //SQL executing event (pre-execution,SQL script can be modified)
            {
                return new KeyValuePair<string, SugarParameter[]>(sql, pars);
            };
            db.Aop.OnDiffLogEvent = it =>//Get data changes
            {
                var editBeforeData = it.BeforeData;
                var editAfterData = it.AfterData;
                var sql = it.Sql;
                var parameter = it.Parameters;
                var businessData = it.BusinessData;
                var time = it.Time;
                var diffType = it.DiffType;//enum insert 、update and delete  
                Console.WriteLine(businessData);
                //Write logic
            };

     
            var list= db.Queryable<Order>().ToList();
            db.Queryable<OrderItem>().ToList();

            //OnDiffLogEvent
            var data = db.Queryable<Order>().First();

            db.Insertable(list.Take(5).ToList()).EnableDiffLogEvent().ExecuteCommand();

            db.Insertable(new Order() { CreateTime=DateTime.Now, CustomId=1, Name="a" ,Price=1 }).EnableDiffLogEvent().ExecuteCommand();

            data.Name = "changeName";
            db.Updateable(data).EnableDiffLogEvent("--update Order--").ExecuteCommand();

            db.Updateable(list.Take(5).ToList()).EnableDiffLogEvent("--update Order--").ExecuteCommand();


            db.Updateable<Order>().SetColumns(it=>it.Name=="asdfa").Where(it=>it.Id==1).EnableDiffLogEvent("--update Order--").ExecuteCommand();

            db.Updateable<Order>().SetColumns(it => it.Name == "asdfa")
                .Where(it =>SqlFunc.Subqueryable<Order>().Where(x=>x.Id==it.Id).Any()).EnableDiffLogEvent("--update Order--").ExecuteCommand();
            Console.WriteLine("#### Aop End ####");
        }
    }
}
