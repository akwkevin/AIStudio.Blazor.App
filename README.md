# AIStudio.Blazor.App

### 框架介绍：
CS架构的AIStudio.Wpf.AClient的后端，同时内嵌wasm的bs客户端。

采用ASP.NET Core 7.0的框架，内部实现有jwt验证，DI自动注入，nlog日志，事件总线，SqlSugar，aop拦截，quartz等。
blazor客户端采用 https://ant-design-blazor.gitee.io/zh-CN/components/overview 组件库

API参照框架有https://github.com/Coldairarrow/Colder.Admin.AntdVue ，https://gitee.com/lisheng741/simpleapp https://gitee.com/hgflydream/Gardener https://gitee.com/zhengguojing/magic-net https://gitee.com/Cherryblossoms/caviar 等，感谢上诉作者的开源作品。

参照上述框架的同时，尽量保证框架简洁，实现功能的同时，不参杂过多的复杂技术。

### 框架结构如下：
```
├─Client  (客户端部分)
│  ├─AIStudio.BlazorUI （客户端页面）
│  ├─AIStudio.Client.Business （向后台请求方法）
│  ├─Application
│  │  ├─AIStudio.BlazorMaui.Client （启动项目，嵌入在maui中）
│  │  ├─AIStudio.BlazorServer.Client（启动项目，Blazor的server模式，暂未完成）
│  │  ├─AIStudio.BlazorWasm.Client（启动项目，Blazor的wasm模式，推荐模式）
│  │  ├─AIStudio.BlazorWinform.Client（启动项目，嵌入在winform中）
│  │  └─AIStudio.BlazorWpf.Client（启动项目，嵌入在wpf中)
│  └─Diagram
│      ├─AIStudio.BlazorDiagram (Diagram的流程图封装)
│      └─AIStudio.BlazorDiagram.Test
├─Common
│  ├─AIStudio.Common （API的基础实现）
│  ├─AIStudio.DbFactory （代码生成使用）
│  ├─AIStudio.Entity （实体类）
│  ├─AIStudio.Util （公共方法）
│  └─WorkflowCore （开源的工作流实现）
│      ├─providers
│      │  ├─WorkflowCore.LockProviders.SqlServer
│      │  ├─WorkflowCore.Persistence.EntityFramework
│      │  ├─WorkflowCore.Persistence.MySQL
│      │  ├─WorkflowCore.Persistence.PostgreSQL
│      │  ├─WorkflowCore.Persistence.Sqlite
│      │  ├─WorkflowCore.Persistence.SqlServer
│      ├─WorkflowCore
│      ├─WorkflowCore.DSL
└─Server (服务端部分)
    ├─AIStudio.Api （API启动项目）
    ├─AIStudio.BlazorWasm.Server （WASM托管在API中的启动项目）
    ├─AIStudio.Business 
    ├─AIStudio.IBusiness
```

### 快速启动
选择AIStudio.BlazorWasm.Server，直接运行即可。



