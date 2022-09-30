
<p  align="center">
    <span>English</span> |  
    <a  href="https://www.donet5.com/Home/Doc"><font color="red">中文</font></a>
</p>
 

## SqlSugar ORM
  
SqlSugar ORM is a library providing Object/Relational Mapping (ORM) 

An ORM framework from the future

Using SqlSugar is very simple , And it's powerful.

## Description

- Support SqlServer、MySql、PgSql and Oracle  insert blukcopy  
- Split table big data self-processing 
- Support Multi-tenant, multi-library transactions
- Support Support CodeFirst data migration.
- Support Join query 、  Union all 、 Subquery 
- Support Configure the query  
- Support DbFirst import entity class from database, or use Generation Tool.
- Support one-to-many and many-to-many navigation properties
- Support MySql、SqlServer、Sqlite、Oracle 、 postgresql 、QuestDb、ClickHouse、达梦、人大金仓 、神通数据库、瀚高、MsAccess、华为GaussDB、GBase 8s、Odbc、Custom
- Support  AOP 、 Diff Log 、 Query Filter 

##  Documentation
|Other |Select  | Insert    | Update  | Delete| 
| ----- | --------- | ----------- | ------- |------- |
 <a target="_blank" href="https://github.com/donet5/SqlSugar/wiki/NUGET">Nuget</a>| <a href="https://www.donet5.com/Home/Doc?typeId=1187">Query</a> | <a  target="_blank" href="https://www.donet5.com/Home/Doc?typeId=1193"> Insert </a> |<a  target="_blank" href="https://www.donet5.com/Home/Doc?typeId=1191">Update</a>|    <a  target="_blank" href="https://www.donet5.com/Home/Doc?typeId=1195">Delete</a>    | 
<a target="_blank" href="https://github.com/donet5/SqlSugar/wiki/Create--database-operation-object"> Start guide</a>  | <a target="_bank" href="https://www.donet5.com/Home/Doc?typeId=1185">Join query </a> |<a href="https://www.donet5.com/Home/Doc?typeId=2422">Insert without entity </a> | <a href="https://www.donet5.com/Home/Doc?typeId=2423">Update without entity</a>  | <a href="https://www.donet5.com/Home/Doc?typeId=2424">  Delete without entity </a>   |     |
|<a href="https://www.donet5.com/Home/Doc?typeId=2246">Multiple databases</a> | <a target="_bank" href="https://www.donet5.com/Home/Doc?typeId=1188">Include query</a>|<a target="_bank" href="https://www.donet5.com/Home/Doc?typeId=2430">Include Insert</a>| <a target="_bank" href="https://www.donet5.com/Home/Doc?typeId=2432">Include Update</a>| <a target="_bank" href="https://www.donet5.com/Home/Doc?typeId=2431">Include Delete</a> 
 |<a  href="https://www.donet5.com/Home/Doc"><font color="red">中文文档</font></a>|||||

##  Feature characteristic

###  Feature1 : Join query  
Super simple query syntax
```cs
var query  = db.Queryable<Order>()
            .LeftJoin<Custom>  ((o, cus) => o.CustomId == cus.Id)
            .LeftJoin<OrderItem> ((o, cus, oritem ) => o.Id == oritem.OrderId)
            .LeftJoin<OrderItem> ((o, cus, oritem , oritem2) => o.Id == oritem2.OrderId)
            .Where(o => o.Id == 1)  
            .Select((o, cus) => new ViewOrder { Id = o.Id, CustomName = cus.Name })
            .ToList();   
```
```sql
SELECT
  [o].[Id] AS [Id],
  [cus].[Name] AS [CustomName]
FROM
  [Order] o
  Left JOIN [Custom] cus ON ([o].[CustomId] = [cus].[Id])
  Left JOIN [OrderDetail] oritem ON ([o].[Id] = [oritem].[OrderId])
  Left JOIN [OrderDetail] oritem2 ON ([o].[Id] = [oritem2].[OrderId])
WHERE
  ([o].[Id] = @Id0)
``` 
###   Feature2 :Include Query、Insert、Delete and Update
```cs 
//query  by nav
var list=db.Queryable<Test>()
           .Includes(x => x.Provinces,x=>x.Citys ,x=>x.Street) //multi-level
           .Includes(x => x.ClassInfo) 
           .ToList();
           
//insert by nav
 db.InsertNav(list) //Finer operation than EFCore's SaveChange
            .Include(z1 => z1.SchoolA) 
            .ThenInclude(z1 => z1.RoomList)  
            .Include(z1 => z1.Books) 
            .ExecuteCommand(); 
            
//delete by nav               
 db.DeleteNav<Student>(it=>it.Id==1) 
            .Include(z1 => z1.SchoolA) 
            .ThenInclude(z1 => z1.RoomList) st
            .Include(z1 => z1.Books) 
            .ExecuteCommand();  
            
//update by nav     
 db.UpdateNav(list)
            .Include(z1 => z1.SchoolA) 
            .ThenInclude(z1 => z1.RoomList)  
            .Include(z1 => z1.Books) 
            .ExecuteCommand();           
```

###   Feature3 : Page query
```cs

 int pageIndex = 1; 
 int pageSize = 20;
 int totalCount=0;
 var page = db.Queryable<Student>().ToPageList(pageIndex, pageSize, ref totalCount);
```
 
###    Feature4 : Dynamic expression
```cs
var names= new string [] { "a","b"};
Expressionable<Order> exp = new Expressionable<Order>();
foreach (var item in names)
{
    exp.Or(it => it.Name.Contains(item.ToString()));
}
var list= db.Queryable<Order>().Where(exp.ToExpression()).ToList();
 ```
 ```sql
SELECT [Id],[Name],[Price],[CreateTime],[CustomId]
        FROM [Order]  WHERE (
                      ([Name] like '%'+ CAST(@MethodConst0 AS NVARCHAR(MAX))+'%') OR 
                      ([Name] like '%'+ CAST(@MethodConst1 AS NVARCHAR(MAX))+'%')
                     )
```
###   Feature5 : Multi-tenant transaction
```cs
//Creaate  database object
SqlSugarClient db = new SqlSugarClient(new List<ConnectionConfig>()
{
    new ConnectionConfig(){ ConfigId="0", DbType=DbType.SqlServer,  ConnectionString=Config.ConnectionString, IsAutoCloseConnection=true },
    new ConnectionConfig(){ ConfigId="1", DbType=DbType.MySql, ConnectionString=Config.ConnectionString4 ,IsAutoCloseConnection=true}
});


var mysqldb = db.GetConnection("1");//mysql db
var sqlServerdb = db.GetConnection("0");// sqlserver db
 
db.BeginTran();
            mysqldb.Insertable(new Order()
            {
                CreateTime = DateTime.Now,
                CustomId = 1,
                Name = "a",
                Price = 1
            }).ExecuteCommand();
            mysqldb.Queryable<Order>().ToList();
            sqlServerdb.Queryable<Order>().ToList();

db.CommitTran();
```
###  Feature6 : Singleton Pattern
Implement transactions across methods
```CS
public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig()
 {
            DbType = SqlSugar.DbType.SqlServer,
            ConnectionString = Config.ConnectionString,
            IsAutoCloseConnection = true 
  },
  db=> {
            db.Aop.OnLogExecuting = (s, p) =>
            {
                Console.WriteLine(s);
            };
 });
 
 
  using (var tran = Db.UseTran())
  {
          
              
               new Test2().Insert(XX);
               new Test1().Insert(XX);
               ..... 
                ....
                         
             tran.CommitTran(); 
 }
```
### Feature7 : Query filter
```cs
//set filter
db.QueryFilter.Add(new TableFilterItem<Order>(it => it.Name.Contains("a")));  
 
   
db.Queryable<Order>().ToList();
//SELECT [Id],[Name],[Price],[CreateTime],[CustomId] FROM [Order]  WHERE  ([Name] like '%'+@MethodConst0+'%')  

db.Queryable<OrderItem, Order>((i, o) => i.OrderId == o.Id)
        .Where(i => i.OrderId != 0)
        .Select("i.*").ToList();
//SELECT i.* FROM [OrderDetail] i  ,[Order]  o  WHERE ( [i].[OrderId] = [o].[Id] )  AND 
//( [i].[OrderId] <> @OrderId0 )  AND  ([o].[Name] like '%'+@MethodConst1+'%')
 
```

### Feature8 : Insert or update 
insert or update 
```cs
    var x = Db.Storageable(list2).ToStorage();  
    x.AsInsertable.ExecuteCommand();  
    x.AsUpdateable.ExecuteCommand();  
```
insert into not exists  
```cs
var x = Db.Storageable(list).SplitInsert(it => !it.Any()).ToStorage()
x.AsInsertable.ExecuteCommand(); 
```
 
### Feature9 ：Auto split table
Split entity 
```cs
[SplitTable(SplitType.Year)]//Table by year (the table supports year, quarter, month, week and day)
[SugarTable("SplitTestTable_{year}{month}{day}")] 
 public class SplitTestTable
 {
     [SugarColumn(IsPrimaryKey =true)]
     public long Id { get; set; }
 
     public string Name { get; set; }
     
     //When the sub-table field is inserted, which table will be inserted according to this field. 
     //When it is updated and deleted, it can also be convenient to use this field to      
     //find out the related table 
     [SplitField] 
     public DateTime CreateTime { get; set; }
 }
 ```
Split query
```cs
 var lis2t = db.Queryable<OrderSpliteTest>()
.SplitTable(DateTime.Now.Date.AddYears(-1), DateTime.Now)
.ToPageList(1,2);　
``` 

### Feature10： Big data insert or update 
```cs
//Insert A million only takes a few seconds
db.Fastest<RealmAuctionDatum>().BulkCopy(GetList());
 
 
//update A million only takes a few seconds
db.Fastest<RealmAuctionDatum>().BulkUpdate(GetList());//A million only takes a few seconds完
db.Fastest<RealmAuctionDatum>().BulkUpdate(GetList(),new string[]{"id"},new string[]{"name","time"})//no primary key
 
//if exists update, else  insert
 var x= db.Storageable<Order>(data).ToStorage();
     x.BulkCopy();
     x.BulkUpdate(); 
     
//set table name
db.Fastest<RealmAuctionDatum>().AS("tableName").BulkCopy(GetList())
 
//set page 
db.Fastest<Order>().PageSize(300000).BulkCopy(insertObjs);

```
