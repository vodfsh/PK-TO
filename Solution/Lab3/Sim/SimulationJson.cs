using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab3.Sim;

public static class SimulationJson
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static string Serialize(SimulationMemento memento)
    {
        return JsonSerializer.Serialize(memento, Options);
    }

    public static SimulationMemento Deserialize(string json)
    {
        return JsonSerializer.Deserialize<SimulationMemento>(json, Options)
               ?? throw new InvalidDataException("The snapshot file is empty or invalid.");
    }
}