using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackOverflow.Solution.Authenticate;
using StackOverflow.Solution.Data.DatabaseContext;
using StackOverflow.Solution.Helper;
using StackOverflow.Solution.Services;
using System.Configuration;
using System.Text;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<Config>(builder.Configuration.GetSection("Config"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StackOverflowContext>(option => option.UseSqlServer(Config.ConnectionString));

builder.Services.AddTransient<IAuthenticateService, AuthenticateService>();



//builder.Services.AddMvc(options =>
//{
//    options.EnableEndpointRouting = false;

//}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseMiddleware(Options.Create<TokenProviderOptions>(options));

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<FirstLevelMiddleWare>();
app.MapControllers();

app.Run();
