using Consul;
using Service.Discovery.Demo.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConsulClient>(p => new ConsulClient(consulConfig =>
{
    var consulHost = builder.Configuration["Consul:Host"];
    var consulPort = Convert.ToInt32(builder.Configuration["Consul:Port"]);
    consulConfig.Address = new Uri($"http://{consulHost}:{consulPort}");
}));
builder.Services.AddSingleton<IServiceDiscovery, ServiceDiscovery>();

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
#region Service Registration
var discovery = app.Services.GetRequiredService<IServiceDiscovery>();
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var serviceName = builder.Configuration["ServiceRegistration:serviceName"];
var serviceId = builder.Configuration["ServiceRegistration:serviceId"];
var serviceAddress = builder.Configuration["ServiceRegistration:servcieHost"];
var servicePort = Convert.ToInt32(builder.Configuration["ServiceRegistration:servciePort"]);

lifetime.ApplicationStarted.Register(async () =>
{
    var registration = new AgentServiceRegistration
    {
        ID = serviceId,
        Name = serviceName,
        Address = serviceAddress,
        Port = servicePort
    };
    await discovery.RegisterServiceAsync(registration);
});

lifetime.ApplicationStopping.Register(async () =>
{
    await discovery.DeRegisterServiceAsync(serviceId);
});
#endregion
app.Run();
