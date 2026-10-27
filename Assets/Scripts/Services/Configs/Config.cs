public sealed class Config
{
    public Config(SpeedConfig speed, FieldBoundaryConfig fieldBoundary, PriceConfig price)
    {
        Speed = speed ?? new SpeedConfig(0f, 0f, 0f, 0f, 0f, 0f);
        FieldBoundary = fieldBoundary ?? new FieldBoundaryConfig(0f, 0f, 0f, 0f);
        Price = price ?? new PriceConfig(0, 0, 0);
    }

    public SpeedConfig Speed { get; }
    public FieldBoundaryConfig FieldBoundary { get; }
    public PriceConfig Price { get; }
}