using Serialization.CrossCutting;
using Serialization.Serializers.SystemTextJson;
using Serialization.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCrossCutting();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<SenderService>();

var myService = new SenderService();

await myService.RunParallelProcessingAsync(new SytemTextJsonSerializer(), 100, 1000);

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();