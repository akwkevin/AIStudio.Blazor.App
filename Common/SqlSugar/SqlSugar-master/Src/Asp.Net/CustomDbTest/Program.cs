﻿using SqlSugar;
using System;

namespace OrmTest
{
    class Program
    {
        /// <summary>
        /// Set up config.cs file and start directly F5
        /// 设置Config.cs文件直接F5启动例子
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Set Custom  Db
            InstanceFactory.CustomDbName = "Access";
            InstanceFactory.CustomDllName = "SqlSugar.Access";
            InstanceFactory.CustomNamespace = "SqlSugar.Access";

            Demo0_SqlSugarClient.Init();
            Demo1_Queryable.Init();
            Demo2_Updateable.Init();
            Demo3_Insertable.Init();
            Demo4_Deleteable.Init();
            Demo5_SqlQueryable.Init();
            Demo7_Ado.Init();
            DemoD_DbFirst.Init();
            Console.WriteLine("all successfully.");
            Console.ReadKey();
        }

 
    }
}
