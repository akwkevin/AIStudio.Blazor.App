# AIStudio.Blazor.App

### 框架介绍：

最新版本为8.0, 稳定版本见分支master

CS架构的AIStudio.Wpf.AClient的后端，同时内嵌wasm的bs客户端。

采用ASP.NET Core 8.0的框架，内部实现有jwt验证，DI自动注入，nlog日志，事件总线，SqlSugar，aop拦截，quartz等。
blazor客户端采用 https://ant-design-blazor.gitee.io/zh-CN/components/overview 组件库

API参照框架有https://github.com/Coldairarrow/Colder.Admin.AntdVue ，https://gitee.com/lisheng741/simpleapp https://gitee.com/hgflydream/Gardener https://gitee.com/zhengguojing/magic-net https://gitee.com/Cherryblossoms/caviar 等，感谢上诉作者的开源作品。

参照上述框架的同时，尽量保证框架简洁，实现功能的同时，不参杂过多的复杂技术。

### AIStudio框架汇总：[https://gitee.com/akwkevin/aistudio.-introduce](https://gitee.com/akwkevin/aistudio.-introduce)

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

### 快速启动（Wasm宿主在API中）
选择AIStudio.BlazorWasm.Server，直接运行即可。
![image](https://user-images.githubusercontent.com/27945492/210241264-b558d763-4889-45a5-a361-92b030be96b8.png)
用户名：Admin 密码：Admin

主界面如下：
![image](https://user-images.githubusercontent.com/27945492/210239274-b7f54270-25b1-4f63-9ad1-e032a1d7528a.png)

### 快速启动2（如果您的项目完全为前后台分离）
选择AIStudio.API运行
![image](https://user-images.githubusercontent.com/27945492/210241023-70bfc82d-fd60-4dee-a885-7d2ca87b3623.png)

### 框架知识点介绍：
待完善

### 以下为部分界面的截图：
权限管理
![image](https://user-images.githubusercontent.com/27945492/210239431-27382105-ca5e-44f5-81a0-53997077fd24.png)
用户管理
![image](https://user-images.githubusercontent.com/27945492/210239487-cd0ecbfe-1f07-41b5-b563-31fa41b1a595.png)
角色管理
![image](https://user-images.githubusercontent.com/27945492/210239502-28ba4035-dc38-4d82-9e2f-0c0e91bc183d.png)
部门管理
![image](https://user-images.githubusercontent.com/27945492/210239529-8cc77a6f-29b8-440a-9561-377d80adcff4.png)
字典管理
![image](https://user-images.githubusercontent.com/27945492/210239556-32c0a0c8-be6c-48b1-adcb-023d4c6d17f0.png)
表单配置
![image](https://user-images.githubusercontent.com/27945492/210239595-86d39071-62b9-4a06-92be-38babbebe64a.png)
任务管理
![image](https://user-images.githubusercontent.com/27945492/210239618-e238aa11-de9a-4f1a-b08e-b21f8a0fd0be.png)
异常日志
![image](https://user-images.githubusercontent.com/27945492/210239670-ee5ebf2f-805e-4d07-b6f8-967202affa62.png)
访问日志
![image](https://user-images.githubusercontent.com/27945492/210239716-27bcfda2-c586-4ed1-8642-b99b9f057729.png)
操作日志
![image](https://user-images.githubusercontent.com/27945492/210239733-82fe0f50-3e14-4b94-834e-4cb7fa701720.png)
系统日志
![image](https://user-images.githubusercontent.com/27945492/210239759-9a7308a3-a79d-4490-b2fd-ef92983fe108.png)
流程管理
![image](https://user-images.githubusercontent.com/27945492/210239913-ee57ccc8-cdbf-4b1a-bc7b-de2d9d52ec0f.png)
发起流程
![image](https://user-images.githubusercontent.com/27945492/210239952-253b73b6-e70d-4fcc-a44f-ff9dcf43ea3a.png)
![image](https://user-images.githubusercontent.com/27945492/210239987-a5059f97-baf8-4a9c-90d8-54aebdba49d6.png)
我的流程
![image](https://user-images.githubusercontent.com/27945492/210240040-f5d654bf-e712-40af-a235-96325a2b14d1.png)


### 嵌入在winform中的运行效果
![image](https://user-images.githubusercontent.com/27945492/210240738-32123971-b162-4b01-bf7e-027a61cb7da4.png)


### 嵌入在wpf中的运行效果
![image](https://user-images.githubusercontent.com/27945492/210240827-1c946ede-28ab-4c08-a7e7-91bdf75628a9.png)


### 同后台CS框架（WPF） https://gitee.com/akwkevin/aistudio.-wpf.-aclient
![image](https://user-images.githubusercontent.com/27945492/210240299-45d725ef-e776-400f-a21a-200dcd453ace.png)



