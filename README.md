# aspnet-core-mssql-ef-demos

> 这个示例的主要目的是为了了解 `Linq to EF` 生成的sql语句是不是经过优化的，以及一些开发常用的配置项。因为我相信Linq to EF，只是一个ORM，数据库搞挂了往往是代码本身的问题，开发人员的问题，就算你用`sql`一样会有问题。

## 准备工作

- 一个能够连接的`mssql`服务（我本机用的是`docker Microsoft/mssql-server-linux`的image）为了方便docker的端口都设置成1433.
- 一个数据库 这里用的是`NorthWind` sql 在assets目录可以找到
- `ASP.NET CORE` 需要安装
- 可选（如果你用vscode开发，可以安装https://marketplace.visualstudio.com/items?itemName=ms-mssql.mssql

### 创建项目

- 在一个空白目录创建一个新的`mvc`项目

```sh
dotnet new mvc
```
创建成功后确保可以正常运行，打开浏览器5000端口可以看到默认的页面

```sh
dotnet run
```
- 执行assets下面的sql，创建数据库

- 测试你能连接到数据库

- 实体生成

```sh
# 使用ef工具生成Models,连接字符串修改成自己的。
dotnet ef dbcontext scaffold "Server=localhost;Database=NorthWind;User Id=sa;Password=qtdqQoNOCz42;" Microsoft.EntityFrameworkC
ore.SqlServer -o Models -f -d
```

- 配置Dbcontext使用log
```csharp
# 依赖注入：loggerFactory

private ILoggerFactory loggerFactory;
public NorthWindContext(ILoggerFactory loggerFactory)
{
  this.loggerFactory = loggerFactory;
}
...
...
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
  optionsBuilder.UseSqlServer(@"Server=localhost;Database=NorthWind;User Id=sa;Password=qtdqQoNOCz42;");
  optionsBuilder.UseLoggerFactory(loggerFactory);
}

```

- 配置数据库连接的DI，具体查看startup.cs


配置到此结束。

> 现在就可以在`HomeController`使用`Linq to ef`查询数据然后追踪生成的sql语句

```csharp
public async Task<IActionResult> Index()
{
  var employees = await _context.Employees.Take(5).AsNoTracking().ToListAsync();

  return View(employees);
}
```
上面的语句查询5条employ的记录，生成的sql语句类似：
```sql
# info: Microsoft.EntityFrameworkCore.Storage.IRelationalCommandBuilderFactory[1]
#      Executed DbCommand (180ms) [Parameters=[@__p_0='?'], CommandType='Text', CommandTimeout='30']
      SELECT TOP(@__p_0) [e].[EmployeeID], [e].[Address], [e].[BirthDate], [e].[City], [e].[Country], [e].[Extension], [e].[FirstName]
, [e].[HireDate], [e].[HomePhone], [e].[LastName], [e].[Notes], [e].[Photo], [e].[PhotoPath], [e].[PostalCode], [e].[Region], [e].[Rep
ortsTo], [e].[Title], [e].[TitleOfCourtesy]
      FROM [Employees] AS [e]
```

#### TODO 

- 更多demo
- cli工具介绍


