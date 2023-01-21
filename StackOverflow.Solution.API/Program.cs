using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackOverflow.Solution.Authenticate;
using StackOverflow.Solution.Data.DatabaseContext;
using System.Text;


var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("drUNFZEpjBXiNHcDYdjXoYi"));

TokenValidationParameters GetSigningKey()
{
    //var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("drUNFZEpjBXiNHcDYdjXoYi"));

    var tokenValidationParameters = new TokenValidationParameters
    {
        //The signing key must be match
        ValidateIssuerSigningKey=true,
        IssuerSigningKey=signingKey,

        //Validate the JWT Issuere(iss) claim
        ValidateIssuer=true,
        ValidIssuer= "stackoverflow.hmaths.com",

        //validate the JWT Audience (aud) claim
        ValidateAudience=true,
        ValidAudience="Different projects",

        //Validate the token expiry
        ValidateLifetime=true,


        //set the certain amount of clock drift
        ClockSkew=TimeSpan.Zero
    };

    return tokenValidationParameters;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StackOverflowContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.Audience = "Different Projects";
    options.ClaimsIssuer = "stackoverflow.hmaths.com";
    options.TokenValidationParameters = GetSigningKey();

});

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

var options = new TokenProviderOptions
{
    Audience = "Different Projects",
    Issuer= "stackoverflow.hmaths.com",
    SigningCredentials=new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha512)
};

//app.UseMiddleware(options.cre)
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
