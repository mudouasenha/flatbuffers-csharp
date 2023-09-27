using Serialization.CrossCutting;
using Serialization.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCrossCutting();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<WorkloadService>();

var myService = new WorkloadService();

//await myService.RunParallelAsync(new Serialization.Serializers.SystemTextJson.NewtonsoftJsonSerializer(), 100, 1000);

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();