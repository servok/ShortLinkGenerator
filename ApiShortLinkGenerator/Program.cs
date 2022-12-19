using ApiShortLinkGenerator.Interfaces;
using ApiShortLinkGenerator.Models;
using ApiShortLinkGenerator.Services.IronBarcodeGenerator;
using ApiShortLinkGenerator.Services.SimpleTokenGenerator;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;
string? connection = Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddControllers();

builder.Services.AddSingleton<IConfiguration>(Configuration);
builder.Services.AddTransient<ITokenGenerator, SimpleTokenGenerator>();
builder.Services.AddTransient<IQRCodeGenerator, IronBarcodeGenerator>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<ApplicationContext>(options => { options.UseNpgsql(connection); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
