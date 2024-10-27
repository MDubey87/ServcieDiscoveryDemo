using Consul;

using (var client = new ConsulClient(consulConfig =>
{
    consulConfig.Address = new Uri("http://localhost:8500");
}))
{
    var services = await client.Catalog.Service("DemoAPi");
    foreach (var service in services.Response)
    {
        Console.WriteLine($"Service ID: {service.ServiceID}, Address: {service.ServiceAddress}, Port: {service.ServicePort}");
    }
}
Console.ReadLine();