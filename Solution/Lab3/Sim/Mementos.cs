namespace Lab3.Sim;

public sealed record PersonMemento(
    int Id,
    double X,
    double Y,
    double Vx,
    double Vy,
    StateKind Kind,
    double Timer,
    double Exposure);

public sealed record Settings(
    double W,
    double H,
    int Start,
    int Max,
    int InitialInfected,
    double ImmunePercent,
    ulong Seed);

public sealed record SimulationMemento(
    Settings Settings,
    long Step,
    ulong RngState,
    int SpawnCounter,
    int NextId,
    List<PersonMemento> People);