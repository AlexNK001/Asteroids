public sealed class PriceConfig
{
    public PriceConfig(int smallAsteroid, int bigAsteroid, int ufo)
    {
        SmallAsteroid = smallAsteroid;
        BigAsteroid = bigAsteroid;
        Ufo = ufo;
    }

    public int SmallAsteroid { get; }
    public int BigAsteroid { get; }
    public int Ufo { get; }
}