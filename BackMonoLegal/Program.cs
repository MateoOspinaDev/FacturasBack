using Microsoft.Extensions.Options;
using AutoMapper;
using BackMonoLegal.Domain.Servicios;
using BackMonoLegal.PersistenceAdapter.Repository;
using BackMonoLegal.NotificationAdapter.EmailNotification;
using BackMonoLegal.PersistenceAdapter.MonoLegalDBSettings;
using BackMonoLegal.ServicesImplementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MonoLegalSettings>(
       builder.Configuration.GetSection("MonoLegalsetttings"));

builder.Services.AddSingleton<IMonoLegalSettings>(sp =>
       sp.GetRequiredService<IOptions<MonoLegalSettings>>().Value);

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFacturaClienteService, FacturaClienteService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
