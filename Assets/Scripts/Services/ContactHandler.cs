using Zenject;

public class ContactHandler
{
    private const ObjectType Ship = ObjectType.Ship;
    private const ObjectType Rocket = ObjectType.Rocket;
    private const ObjectType Laser = ObjectType.Laser;
    private const ObjectType SmallAsteroid = ObjectType.SmallAsteroid;
    private const ObjectType BigAsteroid = ObjectType.BigAsteroid;
    private const ObjectType Ufo = ObjectType.Ufo;

    private readonly TwinKeyList _actions;
    private readonly SignalBus _signalBus;

    public ContactHandler(SignalBus signalBus)
    {
        _signalBus = signalBus;

        _actions = new TwinKeyList();

        _actions.Registration(Ship, Ship, HandleShipShip).
            Registration(Ship, BigAsteroid, HandleShipBigAsteroid).
            Registration(Ship, SmallAsteroid, HandleShipSmallAsteroid).
            Registration(Ship, Rocket, HandleShipRocket).
            Registration(Ship, Laser, HandleShipLaser).
            Registration(Ship, Ufo, HandleShipUfo).
            Registration(BigAsteroid, BigAsteroid, HandleBigAsteroidBigAsteroid).
            Registration(BigAsteroid, SmallAsteroid, HandleBigAsteroidSmallAsteroid).
            Registration(BigAsteroid, Rocket, HandleBigAsteroidRocket).
            Registration(BigAsteroid, Laser, HandleBigAsteroidLaser).
            Registration(BigAsteroid, Ufo, HandleBigAsteroidUfo).
            Registration(SmallAsteroid, SmallAsteroid, HandleSmallAsteroidSmallAsteroid).
            Registration(SmallAsteroid, Rocket, HandleSmallAsteroidRocket).
            Registration(SmallAsteroid, Laser, HandleSmallAsteroidLaser).
            Registration(SmallAsteroid, Ufo, HandleSmallAsteroidUfo).
            Registration(Rocket, Rocket, HandleRocketRocket).
            Registration(Rocket, Laser, HandleRocketLaser).
            Registration(Rocket, Ufo, HandleRocketUfo).
            Registration(Laser, Laser, HandleLaserLaser).
            Registration(Laser, Ufo, HandleLaserUfo).
            Registration(Ufo, Ufo, HandleUfoUfo);
    }

    public void Handle(BaseObject first, BaseObject second)
    {
        _actions.InvokeSignal(first, second);
    }

    private void HandleShipShip(BaseObject firstShip, BaseObject secondShip) { }

    private void HandleShipBigAsteroid(BaseObject ship, BaseObject second)
    {
        _signalBus.Fire<ShipTakeDamagedSignal>();
    }

    private void HandleShipSmallAsteroid(BaseObject ship, BaseObject smallAsteroid)
    {
        _signalBus.Fire<ShipTakeDamagedSignal>();
    }

    private void HandleShipRocket(BaseObject ship, BaseObject rocket) { }

    private void HandleShipLaser(BaseObject ship, BaseObject laser) { }

    private void HandleShipUfo(BaseObject ship, BaseObject ufo)
    {
        _signalBus.Fire<ShipTakeDamagedSignal>();
    }

    private void HandleBigAsteroidBigAsteroid(BaseObject firstBigAsteroid, BaseObject SecondBigAsteroid) { }

    private void HandleBigAsteroidSmallAsteroid(BaseObject bigAsteroid, BaseObject smallAsteroid) { }

    private void HandleBigAsteroidRocket(BaseObject bigAsteroid, BaseObject rocket)
    {
        _signalBus.Fire(new EnemyDefeatedSignal(bigAsteroid));
        _signalBus.Fire(new ObjectReleaseSignal(rocket));
        _signalBus.Fire(new ObjectReleaseSignal(bigAsteroid));
    }

    private void HandleBigAsteroidLaser(BaseObject bigAsteroid, BaseObject laser) { }

    private void HandleBigAsteroidUfo(BaseObject bigAsteroid, BaseObject ufo) { }

    private void HandleSmallAsteroidSmallAsteroid(BaseObject firstSmall, BaseObject secondSmall) { }

    private void HandleSmallAsteroidRocket(BaseObject smallAsteroid, BaseObject rocket)
    {
        _signalBus.Fire(new EnemyDefeatedSignal(smallAsteroid));
        _signalBus.Fire(new ObjectReleaseSignal(rocket));
        _signalBus.Fire(new ObjectReleaseSignal(smallAsteroid));
    }

    private void HandleSmallAsteroidLaser(BaseObject smallAsteroid, BaseObject laser) { }

    private void HandleSmallAsteroidUfo(BaseObject smallAsteroid, BaseObject ufo) { }

    private void HandleRocketRocket(BaseObject firstRocket, BaseObject secondRocket) { }

    private void HandleRocketLaser(BaseObject rocket, BaseObject laser) { }

    private void HandleRocketUfo(BaseObject rocket, BaseObject ufo)
    {
        _signalBus.Fire(new EnemyDefeatedSignal(ufo));
        _signalBus.Fire(new ObjectReleaseSignal(rocket));
        _signalBus.Fire(new ObjectReleaseSignal(ufo));
    }

    private void HandleLaserLaser(BaseObject firstLaser, BaseObject secondLaser) { }

    private void HandleLaserUfo(BaseObject laser, BaseObject ufo) { }

    private void HandleUfoUfo(BaseObject firstUfo, BaseObject secondUfo) { }
}