using System.Net.Sockets;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitMq = builder.AddRabbitMQ("dms-mq")
    .WithManagementPlugin();

var seq = builder.AddSeq("dms-seq")
    .ExcludeFromManifest()
    .WithAnnotation(new EndpointAnnotation(ProtocolType.Tcp, uriScheme: "http", name: "seq", port: 5341, targetPort: 80))
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEnvironment("ACCEPT_EULA", "Y");

builder.AddProject<DMS_Monitor_Api>("dms-monitor")
    .WithReference(rabbitMq)
    .WithReference(seq)
    .WaitFor(rabbitMq)
    .WaitFor(seq);

await builder.Build().RunAsync();
