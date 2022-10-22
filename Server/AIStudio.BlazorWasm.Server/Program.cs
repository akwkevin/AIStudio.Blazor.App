using AIStudio.Api;

//与AIStudio.Api使用相同的启动代码，作为wasm的宿主一起发布，这样就可以使用一个端口号了
Startup.Run(args,
    services =>
    {

    },
    app =>
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseBlazorFrameworkFiles();

        app.MapFallbackToFile("index.html");
    });
