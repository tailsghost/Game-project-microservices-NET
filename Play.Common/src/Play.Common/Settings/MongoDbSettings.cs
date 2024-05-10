namespace Play.Common.Settings;

public class MongoDbSettings
{
    public string Host {  get; init; }

    public string Port { get; init; }

    public string Password { get; init; }

    public string DatabaseName { get; init; }

    public string ConnectionString => $"mongodb://{DatabaseName}:{Password}@{Host}:{Port}";
}
