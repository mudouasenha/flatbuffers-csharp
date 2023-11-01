using Serialization.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<WorkloadService>();
builder.Services.AddControllers();
//.AddSingleton<IRequestCounter, RequestCounter>()
//.AddLogging()

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//var myService = app.Services.GetRequiredService<WorkloadService>();

//await myService.DispatchAsync(
//    new FlatBuffersSerializer(),
//    new ChannelBuilder().Generate().ToString(),
//    100);