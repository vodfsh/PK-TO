namespace Lab3.Sim;

public sealed class CareTaker
{
    public List<SimulationMemento> Snapshots { get; } = new();
    public SimulationMemento? LastSnapshot => Snapshots.Count == 0 ? null : Snapshots[^1];

    public void Save(Simulation simulation)
    {
        Snapshots.Add(simulation.CreateMemento());
    }

    public SimulationMemento? Restore(int index)
    {
        return index >= 0 && index < Snapshots.Count ? Snapshots[index] : null;
    }

    public void Clear()
    {
        Snapshots.Clear();
    }
}