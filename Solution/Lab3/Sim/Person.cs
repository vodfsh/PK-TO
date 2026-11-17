namespace Lab3.Sim;

public sealed class Person
{
    public Person(int id, Vector2D pos, V2DPolarInheritance vel, StateKind kind,
        double timer = 0, double exposure = 0)
    {
        Id = id;
        Pos = pos;
        Vel = vel;
        State = States.For(kind);
        Timer = timer;
        Exposure = exposure;
    }

    public int Id { get; }
    public Vector2D Pos { get; set; }
    public V2DPolarInheritance Vel { get; set; }
    public IPersonState State { get; private set; }

    public StateKind Kind => State.Kind;

    public double Timer { get; set; }
    public double Exposure { get; set; }

    // Recalculated from scratch every step
    public double Contact { get; set; }

    public void SetState(StateKind kind)
    {
        State = States.For(kind);

        if (kind is StateKind.Immune or StateKind.Healthy)
        {
            Timer = 0;
            Exposure = 0;
        }
    }

    public void Infect(Rng rng)
    {
        var kind = rng.Chance(Simulation.SymptomaticChance)
            ? StateKind.InfectedSymptomatic
            : StateKind.InfectedAsymptomatic;

        SetState(kind);
        Timer = Simulation.InfectionMinSeconds + Simulation.InfectionExtraSeconds * rng.NextDouble();
        Exposure = 0;
    }

    public PersonMemento CreateMemento()
    {
        return new PersonMemento(Id, Pos.X, Pos.Y, Vel.X, Vel.Y, Kind, Timer, Exposure);
    }

    public static Person FromMemento(PersonMemento memento)
    {
        return new Person(
            memento.Id,
            new Vector2D(memento.X, memento.Y),
            new V2DPolarInheritance(memento.Vx, memento.Vy),
            memento.Kind,
            memento.Timer,
            memento.Exposure);
    }
}