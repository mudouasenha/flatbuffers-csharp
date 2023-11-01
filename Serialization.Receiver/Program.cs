using Serialization.Receiver.Formatters;
using Serialization.Receiver.Services;
using Serialization.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.AddSingleton<RequestCounterService>();
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Add(new ByteArrayInputFormatter());
    //options.OutputFormatters.Insert(0, new ByteArrayOutputFormatter());
}
);
// builder.Services.AddSingleton<IRequestCounter, RequestCounter>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<RequestCounterMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();