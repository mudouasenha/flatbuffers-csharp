using BenchmarkDotNet.Running;
using FlatBuffers.Sender;
using Serialization.CrossCutting;
using Serialization.Domain.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCrossCutting();
builder.Services.AddScoped<SenderService>();
BenchmarkRunner.Run<SenderService>();
builder.Services.AddHttpClient<ReceiverClient>(httpClient => httpClient.BaseAddress = new Uri("https://localhost:5021"));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();