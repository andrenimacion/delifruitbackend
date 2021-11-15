using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplicationSyscompsa.Models;

namespace WebApplicationSyscompsa
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
           // var connectionB = @"Data Source = SVRDBSQ; Initial Catalog = DISTRILOGICO; Persist Security Info = True; User ID = sa; Password = Rootpass1";
           var connectionB = @"Data Source = 190.63.177.82,63131; Initial Catalog = delifruit; Persist Security Info = True; User ID = sa; Password = Rootpass1";
           //var connectionB = @"Data Source = SVRDBSQ; Initial Catalog = delifruit; Persist Security Info = True; User ID = sa; Password = Rootpass1";
           
           //var connectionB = @"Data Source = ENRIQUE\SQLEXPRESS,3030; Initial Catalog = delifruit; Persist Security Info = True; User ID = sa; Password = Rootpass1";
           //var connectionB = @"Data Source = 192.168.2.250\SQLEXPRESS; Initial Catalog = pofidel;  Persist Security Info = True; User ID = sa; Password = Rootpass1";

           services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionB));
           services.AddControllersWithViews().AddNewtonsoftJson(options =>
           options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        }
            
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            app.UseCors(builder => builder.WithOrigins("http://localhost:5000/",
                                                       "https://www.alp-cloud.com/",
                                                       "https://alp-cloud.com:8430", "")
            .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

        }
    }
}
