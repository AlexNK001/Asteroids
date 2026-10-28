public class EnemyDefeatedSignal
{
    public EnemyDefeatedSignal(PhysicBody enemy)
    {
        Enemy = enemy;
    }

    public PhysicBody Enemy { get; } 
}