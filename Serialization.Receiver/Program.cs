using Serialization.CrossCutting;
using Serialization.Receiver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCrossCutting().AddSingleton<IRequestCounter, RequestCounter>().AddLogging().AddSwaggerGen().AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseMiddleware<RequestCounterMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();