using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DockerHelper
{
    private DockerClient _client;
    private string _containerId;

    public DockerHelper()
    {
        var dockerUri = Environment.GetEnvironmentVariable("DOCKER_URI") ?? "npipe://./pipe/docker_engine";
        _client = new DockerClientConfiguration(new Uri(dockerUri)).CreateClient();
    }

    public async Task StartContainerAsync()
    {
        if (!await ImageExistsAsync("postgres"))
        {
            // Pull the image
            await _client.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = "postgres", Tag = "latest" }, null, new Progress<JSONMessage>());
        }

        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "Your_password123";

        // Create the container
        var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "postgres:latest",
            Env = new List<string>
            {
                $"POSTGRES_PASSWORD={postgresPassword}"
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "5432/tcp", new List<PortBinding> { new PortBinding { HostPort = "5432" } } }
                }
            }
        });

        _containerId = response.ID;

        // Start the container
        await _client.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
    }

    public async Task RemoveContainerAsync()
    {
        // Stop the container
        await _client.Containers.StopContainerAsync(_containerId, new ContainerStopParameters());

        // Remove the container
        await _client.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters());
    }
}