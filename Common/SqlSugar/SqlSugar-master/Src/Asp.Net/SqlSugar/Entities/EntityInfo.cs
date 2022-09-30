﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar
{
    public class EntityInfo
    {
        private string _DbTableName;
        public string EntityName { get; set; }
        public string DbTableName { get { return _DbTableName == null ? EntityName : _DbTableName;  } set { _DbTableName = value; } }
        public string TableDescription { get; set; }
        public Type Type { get; set; }
        public List<EntityColumnInfo> Columns { get; set; }
        public bool IsDisabledDelete { get;  set; }
        public bool IsDisabledUpdateAll { get; set; }
        public List<SugarIndexAttribute> Indexs { get;  set; }
        public bool IsCreateTableFiledSort { get; set; }
    }
}
