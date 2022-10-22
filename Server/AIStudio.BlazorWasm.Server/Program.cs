using AIStudio.Api;

//��AIStudio.Apiʹ����ͬ���������룬��Ϊwasm������һ�𷢲��������Ϳ���ʹ��һ���˿ں���
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
