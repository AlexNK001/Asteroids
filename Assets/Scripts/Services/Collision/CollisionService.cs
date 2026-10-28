using UnityEngine;
using Zenject;

public class CollisionService
{
    private const BodyType Ship = BodyType.Ship;
    private const BodyType Rocket = BodyType.Rocket;
    private const BodyType Laser = BodyType.Laser;
    private const BodyType SmallAsteroid = BodyType.SmallAsteroid;
    private const BodyType BigAsteroid = BodyType.BigAsteroid;
    private const BodyType Ufo = BodyType.Ufo;

    private readonly CollisionMap _collisionTable;
    private readonly SignalBus _signalBus;

    public CollisionService(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _collisionTable = new CollisionMap();

        _collisionTable.Registr(Ship, Ship, HandleShipShip).
            Registr(Ship, BigAsteroid, HandleShipBigAsteroid).
            Registr(Ship, SmallAsteroid, HandleShipSmallAsteroid).
            Registr(Ship, Rocket, HandleShipRocket).
            Registr(Ship, Laser, HandleShipLaser).
            Registr(Ship, Ufo, HandleShipUfo).
            Registr(BigAsteroid, BigAsteroid, HandleBigAsteroidBigAsteroid).
            Registr(BigAsteroid, SmallAsteroid, HandleBigAsteroidSmallAsteroid).
            Registr(BigAsteroid, Rocket, HandleBigAsteroidRocket).
            Registr(BigAsteroid, Laser, HandleBigAsteroidLaser).
            Registr(BigAsteroid, Ufo, HandleBigAsteroidUfo).
            Registr(SmallAsteroid, SmallAsteroid, HandleSmallAsteroidSmallAsteroid).
            Registr(SmallAsteroid, Rocket, HandleSmallAsteroidRocket).
            Registr(SmallAsteroid, Laser, HandleSmallAsteroidLaser).
            Registr(SmallAsteroid, Ufo, HandleSmallAsteroidUfo).
            Registr(Rocket, Rocket, HandleRocketRocket).
            Registr(Rocket, Laser, HandleRocketLaser).
            Registr(Rocket, Ufo, HandleRocketUfo).
            Registr(Laser, Laser, HandleLaserLaser).
            Registr(Laser, Ufo, HandleLaserUfo).
            Registr(Ufo, Ufo, HandleUfoUfo);
    }

    public void Handle(PhysicBody first, PhysicBody second)
    {
        _collisionTable.Invoke(first, second);
    }

    private void HandleShipShip(PhysicBody firstShip, PhysicBody secondShip) { }

    private void HandleShipBigAsteroid(PhysicBody ship, PhysicBody bigAsteroid)
    {
        _signalBus.Fire<ShipTakeDamagedSignal>();
        ResolveCollision(ship, bigAsteroid);
    }

    private void HandleShipSmallAsteroid(PhysicBody ship, PhysicBody smallAsteroid)
    {
        _signalBus.Fire<ShipTakeDamagedSignal>();
    }

    private void HandleShipRocket(PhysicBody ship, PhysicBody rocket) { }

    private void HandleShipLaser(PhysicBody ship, PhysicBody laser) { }

    private void HandleShipUfo(PhysicBody ship, PhysicBody ufo)
    {
        _signalBus.Fire<ShipTakeDamagedSignal>();
    }

    private void HandleBigAsteroidBigAsteroid(PhysicBody firstBigAsteroid, PhysicBody SecondBigAsteroid)
    {
        ResolveCollision(firstBigAsteroid, SecondBigAsteroid);
    }

    private void HandleBigAsteroidSmallAsteroid(PhysicBody bigAsteroid, PhysicBody smallAsteroid) { }

    private void HandleBigAsteroidRocket(PhysicBody bigAsteroid, PhysicBody rocket)
    {
        _signalBus.Fire(new EnemyDefeatedSignal(bigAsteroid));
        _signalBus.Fire(new BodyReleaseSignal(rocket));
        _signalBus.Fire(new BodyReleaseSignal(bigAsteroid));
    }

    private void HandleBigAsteroidLaser(PhysicBody bigAsteroid, PhysicBody laser) { }

    private void HandleBigAsteroidUfo(PhysicBody bigAsteroid, PhysicBody ufo) { }

    private void HandleSmallAsteroidSmallAsteroid(PhysicBody firstSmall, PhysicBody secondSmall) { }

    private void HandleSmallAsteroidRocket(PhysicBody smallAsteroid, PhysicBody rocket)
    {
        _signalBus.Fire(new EnemyDefeatedSignal(smallAsteroid));
        _signalBus.Fire(new BodyReleaseSignal(rocket));
        _signalBus.Fire(new BodyReleaseSignal(smallAsteroid));
    }

    private void HandleSmallAsteroidLaser(PhysicBody smallAsteroid, PhysicBody laser) { }

    private void HandleSmallAsteroidUfo(PhysicBody smallAsteroid, PhysicBody ufo) { }

    private void HandleRocketRocket(PhysicBody firstRocket, PhysicBody secondRocket) { }

    private void HandleRocketLaser(PhysicBody rocket, PhysicBody laser) { }

    private void HandleRocketUfo(PhysicBody rocket, PhysicBody ufo)
    {
        _signalBus.Fire(new EnemyDefeatedSignal(ufo));
        _signalBus.Fire(new BodyReleaseSignal(rocket));
        _signalBus.Fire(new BodyReleaseSignal(ufo));
    }

    private void HandleLaserLaser(PhysicBody firstLaser, PhysicBody secondLaser) { }

    private void HandleLaserUfo(PhysicBody laser, PhysicBody ufo) { }

    private void HandleUfoUfo(PhysicBody firstUfo, PhysicBody secondUfo) { }
    
    private void ResolveCollision(PhysicBody first, PhysicBody second)
    {
        Vector2 normal = (first.Position - second.Position).normalized;

        Vector2 oldFirstDirection = first.LookDirection;
        Vector2 oldSecondDirection = second.LookDirection;

        Vector2 relativeDirection = oldFirstDirection - oldSecondDirection;
        float velAlongNormal = Vector2.Dot(relativeDirection, normal);

        Vector2 newFirstDirection = oldFirstDirection - velAlongNormal * normal;
        first.LookAt(newFirstDirection);

        Vector2 newSecondDirection = oldSecondDirection + velAlongNormal * normal;
        second.LookAt(newSecondDirection);
    }
}