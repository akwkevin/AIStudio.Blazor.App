﻿using SqlSugar.Access;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace SqlSugar.Access
{
    public class AccessDbMaintenance : DbMaintenanceProvider
    {
        #region DML
        protected override string GetDataBaseSql
        {
            get
            {
                return "SELECT NAME FROM MASTER.DBO.SYSDATABASES ORDER BY NAME";
            }
        }
        protected override string GetColumnInfosByTableNameSql
        {
            get
            {
                string sql = @"SELECT sysobjects.name AS TableName,
                           syscolumns.Id AS TableId,
                           syscolumns.name AS DbColumnName,
                           systypes.name AS DataType,
                           COLUMNPROPERTY(syscolumns.id,syscolumns.name,'PRECISION') as [length],
                           isnull(COLUMNPROPERTY(syscolumns.id,syscolumns.name,'Scale'),0) as Scale, 
						   isnull(COLUMNPROPERTY(syscolumns.id,syscolumns.name,'Scale'),0) as DecimalDigits,
                           sys.extended_properties.[value] AS [ColumnDescription],
                           syscomments.text AS DefaultValue,
                           syscolumns.isnullable AS IsNullable,
	                       columnproperty(syscolumns.id,syscolumns.name,'IsIdentity')as IsIdentity,
                           (CASE
                                WHEN EXISTS
                                       ( 
                                             	select 1
												from sysindexes i
												join sysindexkeys k on i.id = k.id and i.indid = k.indid
												join sysobjects o on i.id = o.id
												join syscolumns c on i.id=c.id and k.colid = c.colid
												where o.xtype = 'U' 
												and exists(select 1 from sysobjects where xtype = 'PK' and name = i.name) 
												and o.name=sysobjects.name and c.name=syscolumns.name
                                       ) THEN 1
                                ELSE 0
                            END) AS IsPrimaryKey
                    FROM syscolumns
                    INNER JOIN systypes ON syscolumns.xtype = systypes.xtype
                    LEFT JOIN sysobjects ON syscolumns.id = sysobjects.id
                    LEFT OUTER JOIN sys.extended_properties ON (sys.extended_properties.minor_id = syscolumns.colid
                                                                AND sys.extended_properties.major_id = syscolumns.id)
                    LEFT OUTER JOIN syscomments ON syscolumns.cdefault = syscomments.id
                    WHERE syscolumns.id IN
                        (SELECT id
                         FROM sysobjects
                         WHERE upper(xtype) IN('U',
                                        'V') )
                      AND (systypes.name <> 'sysname')
                      AND sysobjects.name='{0}'
                      AND systypes.name<>'geometry'
                      AND systypes.name<>'geography'
                    ORDER BY syscolumns.colid";
                return sql;
            }
        }
        protected override string GetTableInfoListSql
        {
            get
            {
                return @"SELECT s.Name,Convert(varchar(max),tbp.value) as Description
                            FROM sysobjects s
					     	LEFT JOIN sys.extended_properties as tbp ON s.id=tbp.major_id and tbp.minor_id=0 AND (tbp.Name='MS_Description' OR tbp.Name is null)  WHERE s.xtype IN('U') ";
            }
        }
        protected override string GetViewInfoListSql
        {
            get
            {
                return @"SELECT s.Name,Convert(varchar(max),tbp.value) as Description
                            FROM sysobjects s
					     	LEFT JOIN sys.extended_properties as tbp ON s.id=tbp.major_id and tbp.minor_id=0  AND (tbp.Name='MS_Description' OR tbp.Name is null) WHERE s.xtype IN('V')  ";
            }
        }
        #endregion

        #region DDL
        protected override string CreateDataBaseSql
        {
            get
            {
                return @"create database {0}  ";
            }
        }
        protected override string AddPrimaryKeySql
        {
            get
            {
                return "ALTER TABLE {0} ADD CONSTRAINT {1} PRIMARY KEY({2})";
            }
        }
        protected override string AddColumnToTableSql
        {
            get
            {
                return "ALTER TABLE {0} ADD {1} {2}{3} {4} {5} {6}";
            }
        }
        protected override string AlterColumnToTableSql
        {
            get
            {
                return "ALTER TABLE {0} ALTER COLUMN {1} {2}{3} {4} {5} {6}";
            }
        }
        protected override string BackupDataBaseSql
        {
            get
            {
                return @"USE master;BACKUP DATABASE {0} TO disk = '{1}'";
            }
        }
        protected override string CreateTableSql
        {
            get
            {
                return "CREATE TABLE {0}(\r\n{1})";
            }
        }
        protected override string CreateTableColumn
        {
            get
            {
                return "{0} {1}{2} {3} {4} {5}";
            }
        }
        protected override string TruncateTableSql
        {
            get
            {
                return "TRUNCATE TABLE {0}";
            }
        }
        protected override string BackupTableSql
        {
            get
            {
                return "SELECT TOP {0} * INTO {1} FROM  {2}";
            }
        }
        protected override string DropTableSql
        {
            get
            {
                return "DROP TABLE {0}";
            }
        }
        protected override string DropColumnToTableSql
        {
            get
            {
                return "ALTER TABLE {0} DROP COLUMN {1}";
            }
        }
        protected override string DropConstraintSql
        {
            get
            {
                return "ALTER TABLE {0} DROP CONSTRAINT  {1}";
            }
        }
        protected override string RenameColumnSql
        {
            get
            {
                return "exec sp_rename '{0}.{1}','{2}','column';";
            }
        }
        protected override string AddColumnRemarkSql
        {
            get
            {
                return "EXECUTE sp_addextendedproperty N'MS_Description', '{2}', N'user', N'dbo', N'table', N'{1}', N'column', N'{0}'"; ;
            }
        }

        protected override string DeleteColumnRemarkSql
        {
            get
            {
                return "EXEC sp_dropextendedproperty 'MS_Description','user',dbo,'table','{1}','column','{0}'";
            }

        }

        protected override string IsAnyColumnRemarkSql
        {
            get
            {
                return @"SELECT" +
                                " A.name AS table_name," +
                                " B.name AS column_name," +
                                " C.value AS column_description" +
                                " FROM sys.tables A" +
                                " LEFT JOIN sys.extended_properties C ON C.major_id = A.object_id" +
                                " LEFT JOIN sys.columns B ON B.object_id = A.object_id AND C.minor_id = B.column_id" +
                                " INNER JOIN sys.schemas SC ON SC.schema_id = A.schema_id AND SC.name = 'dbo'" +
                                " WHERE A.name = '{1}' and b.name = '{0}'";

            }
        }

        protected override string AddTableRemarkSql
        {
            get
            {
                return "EXECUTE sp_addextendedproperty N'MS_Description', '{1}', N'user', N'dbo', N'table', N'{0}', NULL, NULL";
            }
        }

        protected override string DeleteTableRemarkSql
        {
            get
            {
                return "EXEC sp_dropextendedproperty 'MS_Description','user',dbo,'table','{0}' ";
            }

        }

        protected override string IsAnyTableRemarkSql
        {
            get
            {
                return @"SELECT 1 AS ID";
            }

        }

        protected override string RenameTableSql
        {
            get
            {
                return "EXEC sp_rename '{0}','{1}'";
            }
        }

        protected override string CreateIndexSql
        {
            get
            {
                return "CREATE {3} NONCLUSTERED INDEX Index_{0}_{2} ON {0}({1})";
            }
        }
        protected override string AddDefaultValueSql
        {
            get
            {
                return "alter table {0} ADD DEFAULT '{2}' FOR {1}";
            }
        }
        protected override string IsAnyIndexSql
        {
            get
            {
                return "select count(*) from sys.indexes where name='{0}'";
            }
        }
        #endregion

        #region Check
        protected override string CheckSystemTablePermissionsSql
        {
            get
            {
                return "select   1  AS ID ";
            }
        }
        #endregion

        #region Scattered
        protected override string CreateTableNull
        {
            get
            {
                return "NULL";
            }
        }
        protected override string CreateTableNotNull
        {
            get
            {
                return "NOT NULL";
            }
        }
        protected override string CreateTablePirmaryKey
        {
            get
            {
                return "PRIMARY KEY";
            }
        }
        protected override string CreateTableIdentity
        {
            get
            {
                return "IDENTITY(1,1)";
            }
        }

        #endregion

        #region Methods
        public override bool AddRemark(EntityInfo entity)
        {
            return true;
        }
        public override void AddDefaultValue(EntityInfo entityInfo)
        {
            //base.AddDefaultValue(entityInfo);
        }
        public override List<DbTableInfo> GetViewInfoList(bool isCache = true)
        {
            return new List<DbTableInfo>();
        }
        public override List<DbTableInfo> GetTableInfoList(bool isCache = true)
        {
            bool isOpen = Open();
            var table = (this.Context.Ado.Connection as OleDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "Table" });
            var result = table
              .Rows.Cast<DataRow>().Select(it => new DbTableInfo
              {

                  Name = it["TABLE_NAME"] + "",
                  Description = it["DESCRIPTION"] + ""
              }).ToList();
            Close(isOpen);
            return result;
        }

        private void Close(bool isOpen)
        {
            if (this.Context.CurrentConnectionConfig.IsAutoCloseConnection&& isOpen)
            {
                this.Context.Ado.Connection.Close();
            }
        }

        private bool Open()
        {
            var isOpen = false;
            if (this.Context.CurrentConnectionConfig.IsAutoCloseConnection&& this.Context.Ado.Connection.State == ConnectionState.Closed)
            {
                this.Context.Ado.Connection.Open();
                isOpen = true;
            }

            return isOpen;
        }

        public List<string> GetSchemas()
        {
            return this.Context.Ado.SqlQuery<string>("SELECT name FROM  sys.schemas where name <> 'dbo'");
        }
        public override bool DeleteTableRemark(string tableName)
        {
            string sql = string.Format(this.DeleteTableRemarkSql, tableName);
            if (tableName.Contains("."))
            {
                var schemas = GetSchemas();
                var tableSchemas = this.SqlBuilder.GetNoTranslationColumnName(tableName.Split('.').First());
                if (schemas.Any(y => y.EqualCase(tableSchemas)))
                {
                    sql = string.Format(this.DeleteTableRemarkSql, this.SqlBuilder.GetNoTranslationColumnName(tableName.Split('.').Last()));
                    sql = sql.Replace(",dbo,", $",{tableSchemas},").Replace("'user'", "'SCHEMA'");
                }
            }
            this.Context.Ado.ExecuteCommand(sql);
            return true;
        }
        public override bool IsAnyTableRemark(string tableName)
        {
            string sql = string.Format(this.IsAnyTableRemarkSql, tableName);
            if (tableName.Contains("."))
            {
                var schemas = GetSchemas();
                var tableSchemas = this.SqlBuilder.GetNoTranslationColumnName(tableName.Split('.').First());
                if (schemas.Any(y => y.EqualCase(tableSchemas)))
                {
                    sql = string.Format(this.IsAnyTableRemarkSql, this.SqlBuilder.GetNoTranslationColumnName(tableName.Split('.').Last()));
                    sql = sql.Replace("'dbo'", $"'{tableSchemas}'");
                }
            }
            var dt = this.Context.Ado.GetDataTable(sql);
            return dt.Rows != null && dt.Rows.Count > 0;
        }
        public override bool AddTableRemark(string tableName, string description)
        {
            string sql = string.Format(this.AddTableRemarkSql, tableName, description);
            if (tableName.Contains(".")) 
            {
                var schemas = GetSchemas();
                var tableSchemas =this.SqlBuilder.GetNoTranslationColumnName(tableName.Split('.').First());
                if (schemas.Any(y => y.EqualCase(tableSchemas))) 
                {
                    sql = string.Format(this.AddTableRemarkSql, this.SqlBuilder.GetNoTranslationColumnName(tableName.Split('.').Last()), description);
                    sql = sql.Replace("N'dbo'", $"N'{tableSchemas}'").Replace("N'user'", "N'SCHEMA'");
                }
            }
            this.Context.Ado.ExecuteCommand(sql);
            return true;
        }
        public override bool AddDefaultValue(string tableName, string columnName, string defaultValue)
        {
            if (defaultValue == "''")
            {
                defaultValue = "";
            }
            var template = AddDefaultValueSql;
            if (defaultValue != null && defaultValue.ToLower() == "getdate()") 
            {
                template = template.Replace("'{2}'", "{2}");
            }
            string sql = string.Format(template, tableName, columnName, defaultValue);
            this.Context.Ado.ExecuteCommand(sql);
            return true;
        }

        /// <summary>
        ///by current connection string
        /// </summary>
        /// <param name="databaseDirectory"></param>
        /// <returns></returns>
        public override bool CreateDatabase(string databaseName, string databaseDirectory = null)
        {
            throw new Exception("Access no support CreateDatabase");
        }
        public override bool CreateTable(string tableName, List<DbColumnInfo> columns, bool isCreatePrimaryKey = true)
        {
            tableName = this.SqlBuilder.GetTranslationTableName(tableName);
            foreach (var item in columns)
            {
                if (item.DataType == "decimal" && item.DecimalDigits == 0 && item.Length == 0)
                {
                    item.DecimalDigits = 4;
                    item.Length = 18;
                }
            }
            string sql = GetCreateTableSql(tableName, columns);
            this.Context.Ado.ExecuteCommand(sql);
            if (isCreatePrimaryKey)
            {
                var pkColumns = columns.Where(it => it.IsPrimarykey).ToList();
                if (pkColumns.Count > 1)
                {
                    this.Context.DbMaintenance.AddPrimaryKeys(tableName, pkColumns.Select(it => it.DbColumnName).ToArray());
                }
                else
                {
                    foreach (var item in pkColumns)
                    {
                        this.Context.DbMaintenance.AddPrimaryKey(tableName, item.DbColumnName);
                    }
                }
            }
            return true;
        }
        public override List<DbColumnInfo> GetColumnInfosByTableName(string tableName, bool isCache = true)
        {

            List<DbColumnInfo> columns = new List<DbColumnInfo>();
            var dt = this.Context.Ado.GetDataTable("select top 8 * from " +this.SqlBuilder.GetTranslationTableName(tableName));
            var oleDb = (this.Context.Ado.Connection as OleDbConnection);
            bool isOpen = Open();
            DataTable columnTable = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null,tableName, null });
            DataTable Pk = oleDb.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new string[] { null, null, tableName });
            foreach (DataRow dr in columnTable.Rows)
            {
                DbColumnInfo info = new DbColumnInfo();
                info.TableName = tableName;
                info.DbColumnName = dr["COLUMN_NAME"] + "";
                info.IsNullable = Convert.ToBoolean(dr["IS_NULLABLE"]);
                info.DataType = dt.Columns[info.DbColumnName].DataType.Name;
                if (info.DataType.EqualCase( "int32")) 
                {
                    info.DataType = "int";
                }
                else if (info.DataType.EqualCase("int64"))
                {
                    info.DataType = "long";
                }
                if (info.DataType.EqualCase("string") ) 
                {
                    info.Length = dr["CHARACTER_MAXIMUM_LENGTH"].ObjToInt();
                }
                if (info.DataType.EqualCase("Decimal"))
                {
                    info.Length = dr["numeric_precision"].ObjToInt();
                    info.Scale=info.DecimalDigits = dr["numeric_scale"].ObjToInt();
                }
                foreach (DataRow pkRow in Pk.Rows)
                {
                    if (pkRow["COLUMN_NAME"].ObjToString() == dr["COLUMN_NAME"].ObjToString()) 
                    {
                        info.IsPrimarykey = true;
                        if (info.DataType == "int") 
                        {
                            info.IsIdentity = true;
                        }
                    }
                }
                columns.Add(info);
            }
            Close(isOpen);
            return columns;
        }
        public override bool RenameColumn(string tableName, string oldColumnName, string newColumnName)
        {
            tableName = this.SqlBuilder.GetTranslationTableName(tableName);
            oldColumnName = this.SqlBuilder.GetTranslationColumnName(oldColumnName);
            newColumnName = this.SqlBuilder.GetNoTranslationColumnName(newColumnName);
            string sql = string.Format(this.RenameColumnSql, tableName, oldColumnName, newColumnName);
            this.Context.Ado.ExecuteCommand(sql);
            return true;
        } 
        #endregion
    }
}
