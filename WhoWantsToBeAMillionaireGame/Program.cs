using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using WhoWantsToBeAMillionaireGame.Business.ServicesImplementations;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Data.Abstractions;
using WhoWantsToBeAMillionaireGame.Data.Abstractions.Repositories;
using WhoWantsToBeAMillionaireGame.Data.Repositories;
using WhoWantsToBeAMillionaireGame.DataBase;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.File(GetPathToLogFile(),
                    LogEventLevel.Information));

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<WhoWantsToBeAMillionaireGameDbContext>(
                optionBuilder => optionBuilder.UseSqlServer(connectionString));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllersWithViews();

            // Add business services
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<IAnswerService, AnswerService>();
            
            // Add repositories
            builder.Services.AddScoped<IRepository<Question>, Repository<Question>>();
            builder.Services.AddScoped<IRepository<Answer>, Repository<Answer>>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        /// <summary>
        /// Returns the path for log file recording.
        /// </summary>
        /// <returns>A string whose value contains a path to the log file</returns>
        private static string GetPathToLogFile()
        {
            var sb = new StringBuilder();
            sb.Append(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            sb.Append(@"\logs\");
            sb.Append($"{DateTime.Now:yyyyMMddhhmmss}");
            sb.Append("data.log");
            return sb.ToString();
        }
    }
}