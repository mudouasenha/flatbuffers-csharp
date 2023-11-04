using Serialization.Domain.Builders;
using Serialization.Serializers.FlatBuffers;
using Serialization.Services;
using System.Net;

ServicePointManager.DefaultConnectionLimit = 5000;
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxConcurrentConnections = 10000;
});

builder.Services.AddSingleton<WorkloadService>();
builder.Services.AddControllers();
//.AddSingleton<IRequestCounter, RequestCounter>()
//.AddLogging()

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

var myService = app.Services.GetRequiredService<WorkloadService>();

Thread.Sleep(2000);

await myService.RunParallelRestAsync(
    new FlatBuffersSerializer(),
    new ChannelBuilder().Generate().GetType(),
    100);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

