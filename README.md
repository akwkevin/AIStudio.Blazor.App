# AIStudio.Blazor.App

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
