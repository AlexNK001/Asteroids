public class EnemyDefeatedSignal
{
    public EnemyDefeatedSignal(BaseObject enemy)
    {
        // Type = type;
        Enemy = enemy;
    }

    public BaseObject Enemy { get; } 
    // public static readonly EnemyDefeatedSignal SmallAsteroid = new(CollisionType.SmallAsteroid);
    // public static readonly EnemyDefeatedSignal BigAsteroid = new(CollisionType.BigAsteroid);
    // public static readonly EnemyDefeatedSignal Ufo = new(CollisionType.Ufo);
    //
    // public CollisionType Type { get; }
}