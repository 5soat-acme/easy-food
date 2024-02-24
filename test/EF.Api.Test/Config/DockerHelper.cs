using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;

public class DockerHelper
{
    private DockerClient _client;
    private string _containerId;
    private readonly string _postgresPassword = "acme";

    public DockerHelper()
    {
        var dockerUri = Environment.GetEnvironmentVariable("DOCKER_URI") ?? "npipe://./pipe/docker_engine";
        _client = new DockerClientConfiguration(new Uri(dockerUri)).CreateClient();
    }

    public async Task StartContainerAsync()
    {
        if (!await ImageExistsAsync("postgres"))
        {
            await _client.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = "postgres", Tag = "latest" }, null, new Progress<JSONMessage>());
        }

        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? _postgresPassword;
        
        var response = await _client.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "postgres:latest",
            Env = new List<string>
            {
                $"POSTGRES_PASSWORD={postgresPassword}",
                "POSTGRES_USER=acme",
                "POSTGRES_DB=easy-food"
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "5432/tcp", new List<PortBinding> { new() { HostPort = "5432" } } }
                }
            },
            Name = "easy-food-db-test"
        });

        _containerId = response.ID;
        await _client.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
        var pathScript = GetSolutionDirectory() + "\\deploy\\database\\init.sql";
        await ExecuteSqlScriptAsync(pathScript); 
    }

    public async Task RemoveContainerAsync()
    {
        await _client.Containers.StopContainerAsync(_containerId, new ContainerStopParameters());
        await _client.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters());
    }
    
    private async Task<bool> ImageExistsAsync(string imageName)
    {
        var images = await _client.Images.ListImagesAsync(new ImagesListParameters 
        { 
            Filters = new Dictionary<string, IDictionary<string, bool>>
            {
                {"reference", new Dictionary<string, bool> {{imageName, true}}}
            }
        });
        return images.Any();
    }
    
    private async Task ExecuteSqlScriptAsync(string scriptPath)
    {
        var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? _postgresPassword;
        var connectionString = $"Host=localhost;Database=easy-food;Username=acme;Password={postgresPassword}";

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var script = await File.ReadAllTextAsync(scriptPath);

        await using var command = new NpgsqlCommand(script, connection);
        await command.ExecuteNonQueryAsync();
    }
    
    private static string GetSolutionDirectory()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory != null)
        {
            if (directory.GetFiles("*.sln").Length > 0)
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new Exception("Solution directory could not be found.");
    }
}