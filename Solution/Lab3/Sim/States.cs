namespace Lab3.Sim;

public enum StateKind
{
    Healthy,
    InfectedAsymptomatic,
    InfectedSymptomatic,
    Immune
}

public interface IPersonState
{
    StateKind Kind { get; }
    double InfectionChance { get; }
    string Color { get; }
    void Update(Person person, Simulation simulation);
}

public static class States
{
    public static readonly IPersonState Healthy = new HealthyState();
    public static readonly IPersonState InfectedAsymptomatic = new AsymptomaticState();
    public static readonly IPersonState InfectedSymptomatic = new SymptomaticState();
    public static readonly IPersonState Immune = new ImmuneState();

    public static IPersonState For(StateKind kind)
    {
        return kind switch
        {
            StateKind.Healthy => Healthy,
            StateKind.InfectedAsymptomatic => InfectedAsymptomatic,
            StateKind.InfectedSymptomatic => InfectedSymptomatic,
            StateKind.Immune => Immune,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown state kind.")
        };
    }
}

// All states are stateless singletons. The actual data lives in Person, which
// keeps the memento small and makes restoring a snapshot unambiguous.
public sealed class HealthyState : IPersonState
{
    public StateKind Kind => StateKind.Healthy;
    public double InfectionChance => 0;
    public string Color => "#16a34a";

    public void Update(Person person, Simulation simulation)
    {
        if (person.Contact <= 0)
        {
            person.Exposure = 0;
            return;
        }

        person.Exposure += Simulation.Dt;
        if (person.Exposure + 1e-9 < Simulation.ExposureNeeded)
            return;

        person.Exposure = 0;
        if (simulation.Rng.Chance(person.Contact))
            person.Infect(simulation.Rng);
    }
}

public abstract class InfectedState : IPersonState
{
    public abstract StateKind Kind { get; }
    public abstract double InfectionChance { get; }
    public abstract string Color { get; }

    public void Update(Person person, Simulation simulation)
    {
        person.Timer -= Simulation.Dt;
        if (person.Timer <= 0)
            person.SetState(StateKind.Immune);
    }
}

public sealed class AsymptomaticState : InfectedState
{
    public override StateKind Kind => StateKind.InfectedAsymptomatic;
    public override double InfectionChance => Simulation.AsymptomaticInfectionChance;
    public override string Color => "#f59e0b";
}

public sealed class SymptomaticState : InfectedState
{
    public override StateKind Kind => StateKind.InfectedSymptomatic;
    public override double InfectionChance => Simulation.SymptomaticInfectionChance;
    public override string Color => "#dc2626";
}

public sealed class ImmuneState : IPersonState
{
    public StateKind Kind => StateKind.Immune;
    public double InfectionChance => 0;
    public string Color => "#2563eb";

    public void Update(Person person, Simulation simulation)
    {
    }
}
