using Consul;

namespace Service.Discovery.Demo.Api
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        private readonly IConsulClient _consulClient;

        public ServiceDiscovery(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }        

        public async Task RegisterServiceAsync(string serviceName, string serviceId, string serviceAddress, int servicePort)
        {
            var registration = new AgentServiceRegistration
            {
                ID = serviceId,
                Name = serviceName,
                Address = serviceAddress,
                Port = servicePort
            };
            await _consulClient.Agent.ServiceDeregister(serviceId);
            await _consulClient.Agent.ServiceRegister(registration);
        }

        public async Task RegisterServiceAsync(AgentServiceRegistration serviceRegistration)
        {
            await _consulClient.Agent.ServiceDeregister(serviceRegistration.ID);
            await _consulClient.Agent.ServiceRegister(serviceRegistration);
        }
        public async Task DeRegisterServiceAsync(string serviceId)
        {
            await _consulClient.Agent.ServiceDeregister(serviceId);
        }
    }
}
