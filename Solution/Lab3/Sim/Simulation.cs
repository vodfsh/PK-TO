namespace Lab3.Sim;

public sealed class Simulation
{
    public const int StepsPerSecond = 25;
    public const double Dt = 1.0 / StepsPerSecond;

    public const double MinSpeed = 0.25;
    public const double MaxSpeed = 2.0;
    public const double MaxTurnRadians = Math.PI / 9.0; // 20 degrees
    public const double VelocityChangeChance = 0.02;

    public const double InfectionRadius = 2.0;
    public const double InfectionRadiusSquared = InfectionRadius * InfectionRadius;
    public const double ExposureNeeded = 3.0;
    public const double AsymptomaticInfectionChance = 0.5;
    public const double SymptomaticInfectionChance = 1.0;

    public const double InfectionMinSeconds = 20.0;
    public const double InfectionExtraSeconds = 10.0;
    public const double SymptomaticChance = 0.5;

    public const double EntrantInfectedChance = 0.10;
    public const int SpawnEveryNSteps = 10;
    public const double BorderBounceChance = 0.5;

    public Simulation(Settings settings)
    {
        Settings = settings;
        Rng = new Rng(settings.Seed);
        People = new List<Person>();
        NextId = 1;
        Step = 0;
        SpawnCounter = 0;

        InitializePopulation();
    }

    private Simulation(Settings settings, long step, ulong rngState, int spawnCounter,
        int nextId, List<Person> people)
    {
        Settings = settings;
        Step = step;
        Rng = new Rng(rngState);
        SpawnCounter = spawnCounter;
        NextId = nextId;
        People = people;
    }

    public Settings Settings { get; }
    public List<Person> People { get; }
    public Rng Rng { get; }
    public long Step { get; private set; }
    public int SpawnCounter { get; private set; }
    public int NextId { get; private set; }

    public double TimeSeconds => Step / (double)StepsPerSecond;
    public int HealthyCount => People.Count(p => p.Kind == StateKind.Healthy);
    public int AsymptomaticCount => People.Count(p => p.Kind == StateKind.InfectedAsymptomatic);
    public int SymptomaticCount => People.Count(p => p.Kind == StateKind.InfectedSymptomatic);
    public int ImmuneCount => People.Count(p => p.Kind == StateKind.Immune);

    private void InitializePopulation()
    {
        var max = Math.Max(0, Settings.Max);
        var start = Math.Clamp(Settings.Start, 0, max);
        var immuneCount = Math.Clamp(
            (int)Math.Round(Settings.ImmunePercent * start, MidpointRounding.AwayFromZero),
            0, start);
        var infectedCount = Math.Clamp(Settings.InitialInfected, 0, start - immuneCount);
        var healthyCount = start - immuneCount - infectedCount;

        var kinds = new List<InitialKind>(start);
        for (var i = 0; i < immuneCount; i++)
        {
            kinds.Add(InitialKind.Immune);
        }

        for (var i = 0; i < infectedCount; i++)
        {
            kinds.Add(InitialKind.Infected);
        }

        for (var i = 0; i < healthyCount; i++)
        {
            kinds.Add(InitialKind.Healthy);
        }

        for (var i = kinds.Count - 1; i > 0; i--)
        {
            var j = Rng.NextInt(i + 1);
            (kinds[i], kinds[j]) = (kinds[j], kinds[i]);
        }

        foreach (var kind in kinds)
        {
            var position = new Vector2D(
                Rng.NextDouble(0, Settings.W),
                Rng.NextDouble(0, Settings.H));
            var person = new Person(NextId++, position, RandomVelocity(), StateKind.Healthy);

            switch (kind)
            {
                case InitialKind.Immune:
                    person.SetState(StateKind.Immune);
                    break;
                case InitialKind.Infected:
                    person.Infect(Rng);
                    break;
            }

            People.Add(person);
        }
    }

    public void Advance()
    {
        MovePeople();
        HandleBorders();
        TrySpawn();
        UpdateContacts();

        foreach (var person in People)
        {
            person.State.Update(person, this);
        }

        Step++;
    }

    private void MovePeople()
    {
        foreach (var person in People)
        {
            if (Rng.Chance(VelocityChangeChance))
            {
                person.Vel.Rotate(Rng.NextDouble(-MaxTurnRadians, MaxTurnRadians));
                person.Vel.Radius = Rng.NextDouble(MinSpeed, MaxSpeed);
            }

            person.Pos += person.Vel * Dt;
        }
    }

    private void HandleBorders()
    {
        List<Person>? toRemove = null;

        foreach (var person in People)
        {
            var outsideX = person.Pos.X < 0 || person.Pos.X > Settings.W;
            var outsideY = person.Pos.Y < 0 || person.Pos.Y > Settings.H;
            if (!outsideX && !outsideY)
            {
                continue;
            }

            if (!Rng.Chance(BorderBounceChance))
            {
                (toRemove ??= new List<Person>()).Add(person);
                continue;
            }

            if (person.Pos.X < 0)
            {
                person.Pos.X = 0;
                person.Vel.X = Math.Abs(person.Vel.X);
            }
            else if (person.Pos.X > Settings.W)
            {
                person.Pos.X = Settings.W;
                person.Vel.X = -Math.Abs(person.Vel.X);
            }

            if (person.Pos.Y < 0)
            {
                person.Pos.Y = 0;
                person.Vel.Y = Math.Abs(person.Vel.Y);
            }
            else if (person.Pos.Y > Settings.H)
            {
                person.Pos.Y = Settings.H;
                person.Vel.Y = -Math.Abs(person.Vel.Y);
            }
        }

        if (toRemove is not null)
        {
            People.RemoveAll(p => toRemove.Contains(p));
        }
    }

    private void TrySpawn()
    {
        if (++SpawnCounter < SpawnEveryNSteps)
        {
            return;
        }

        SpawnCounter = 0;
        if (People.Count < Settings.Max)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        var side = Rng.NextInt(4);
        double x;
        double y;
        double angle;

        switch (side)
        {
            case 0: // bottom, moving inward/up
                x = Rng.NextDouble(0, Settings.W);
                y = 0;
                angle = Rng.NextDouble(0, Math.PI);
                break;
            case 1: // right, moving inward/left
                x = Settings.W;
                y = Rng.NextDouble(0, Settings.H);
                angle = Rng.NextDouble(Math.PI / 2, 3 * Math.PI / 2);
                break;
            case 2: // top, moving inward/down
                x = Rng.NextDouble(0, Settings.W);
                y = Settings.H;
                angle = Rng.NextDouble(Math.PI, Math.Tau);
                break;
            default: // left, moving inward/right
                x = 0;
                y = Rng.NextDouble(0, Settings.H);
                angle = Rng.NextDouble(-Math.PI / 2, Math.PI / 2);
                break;
        }

        var person = new Person(
            NextId++,
            new Vector2D(x, y),
            V2DPolarInheritance.FromPolar(angle, Rng.NextDouble(MinSpeed, MaxSpeed)),
            StateKind.Healthy);

        if (Rng.Chance(EntrantInfectedChance))
        {
            person.Infect(Rng);
        }
        else if (Rng.Chance(Settings.ImmunePercent))
        {
            person.SetState(StateKind.Immune);
        }

        People.Add(person);
    }

    private void UpdateContacts()
    {
        foreach (var person in People)
        {
            person.Contact = 0;
            if (person.Kind != StateKind.Healthy)
            {
                continue;
            }

            var bestChance = 0.0;
            foreach (var other in People)
            {
                var chance = other.State.InfectionChance;
                if (chance <= bestChance)
                {
                    continue;
                }

                if (person.Pos.DistanceSquared(other.Pos) <= InfectionRadiusSquared)
                {
                    bestChance = chance;
                }
            }

            person.Contact = bestChance;
        }
    }

    private V2DPolarInheritance RandomVelocity()
    {
        return V2DPolarInheritance.FromPolar(
            Rng.NextDouble(0, Math.Tau),
            Rng.NextDouble(MinSpeed, MaxSpeed));
    }

    public SimulationMemento CreateMemento()
    {
        return new SimulationMemento(
            Settings,
            Step,
            Rng.State,
            SpawnCounter,
            NextId,
            People.Select(p => p.CreateMemento()).ToList());
    }

    public static Simulation FromMemento(SimulationMemento memento)
    {
        ArgumentNullException.ThrowIfNull(memento);

        var settings = memento.Settings
                       ?? throw new InvalidDataException("Snapshot has no settings.");

        if (settings.W <= 0 || settings.H <= 0)
        {
            throw new InvalidDataException("Snapshot has invalid room dimensions.");
        }

        if (settings.Max < 0 || settings.Start < 0 || settings.Start > settings.Max)
        {
            throw new InvalidDataException("Snapshot has invalid population settings.");
        }

        if (settings.InitialInfected < 0 || settings.InitialInfected > settings.Start)
        {
            throw new InvalidDataException("Snapshot has an invalid initial infected count.");
        }

        if (settings.ImmunePercent < 0 || settings.ImmunePercent > 1)
        {
            throw new InvalidDataException("Snapshot has an invalid immune percentage.");
        }

        if (memento.People is null)
        {
            throw new InvalidDataException("Snapshot has no population.");
        }

        return new Simulation(
            settings,
            memento.Step,
            memento.RngState,
            memento.SpawnCounter,
            memento.NextId,
            memento.People.Select(Person.FromMemento).ToList());
    }

    private enum InitialKind
    {
        Healthy,
        Infected,
        Immune
    }
}