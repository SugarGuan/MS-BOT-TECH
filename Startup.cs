// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.3.0

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;

using CoreBot.Bots;
using CoreBot.Dialogs;

namespace CoreBot
{
    public class Startup
    {
        public Startup()
        {
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // 运行时调用，在这里向容器添加服务（这里使用ISC相关的依赖注入）
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the credential provider to be used with the Bot Framework Adapter.
            // 创建（与Bot Framework Adapter一起使用的）凭据提供程序
            services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();

            // Create the Bot Framework Adapter with error handling enabled.
            // 创建启用了错误处理的Bot Framework Adapter
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            // 创建被用于保存用户和对话状态的存储空间
            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the User state. (Used in this bot's Dialog implementation.)
            // 添加用户状态：用于保存一些用户的信息，比如年龄、性别等。一般，这些数据由用户主动输入
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            // 添加用户对话状态（类似session）
            services.AddSingleton<ConversationState>();

            // The Dialog that will be run by the bot.
            // bot加载一个对话
            services.AddSingleton<MainDialog>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            // 将机器人创建为瞬态。 在这种情况下，ASP控制器期待IBot。
            services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // 运行时调用，使用这个方法用于配置HTTP请求管道
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
