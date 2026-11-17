namespace Lab3.Sim;

// splitmix64: small, fast and easy to snapshot because all state is one ulong.
public sealed class Rng
{
    public Rng(ulong seed)
    {
        State = seed;
    }

    public ulong State { get; set; }

    public ulong NextULong()
    {
        State += 0x9E3779B97F4A7C15UL;
        var z = State;
        z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
        z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
        return z ^ (z >> 31);
    }

    public double NextDouble()
    {
        return (NextULong() >> 11) * (1.0 / (1UL << 53));
    }

    public double NextDouble(double min, double max)
    {
        return min + NextDouble() * (max - min);
    }

    public int NextInt(int maxExclusive)
    {
        if (maxExclusive <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));
        }

        return (int)(NextULong() % (ulong)maxExclusive);
    }

    public bool Chance(double probability)
    {
        return NextDouble() < probability;
    }
}