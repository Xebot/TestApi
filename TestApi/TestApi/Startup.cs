using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using TestApi.Core.Services.OrderService;
using TestApi.Core.Services.ProductService;
using TestApi.Infrastructure;
using TestApi.Infrastructure.Mapping;

namespace TestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<AppDbContext>
                (o => o.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Создание БД, если она не создана, накат миграций, заполнение тестовыми данными

            db.Database.EnsureCreated();
            db.Database.Migrate();
            ExecuteSqlScript();

            #endregion

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ExecuteSqlScript()
        {
            try
            {
                var connectionString = Configuration.GetConnectionString("Database");
                var file = new FileInfo(Configuration["SqlScriptPath"]);
                string script = file.OpenText().ReadToEnd();
                var conn = new SqlConnection(connectionString);
                var server = new Server(new ServerConnection(conn));
                server.ConnectionContext.ExecuteNonQuery(script);
            }
            catch (Exception e)
            {
                //TODO: Add logging;
            }            
        }
    }
}
