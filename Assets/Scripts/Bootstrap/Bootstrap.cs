using UnityEngine;
using Zenject;

public class Bootstrap : MonoInstaller
{
    [SerializeField] private Ship _ship;
    [Header("Prefabs")] 
    [SerializeField] private SmallAsteroid _smallAsteroidPrefab;
    [SerializeField] private BigAsteroid _bigAsteroidPrefab;
    [SerializeField] private Ufo _ufoPrefab;
    [SerializeField] private Rocket _rocketPrefab;

    [Header("Owners Construct")] 
    [SerializeField] private Spawner _spawner;

    [Header("View")]
    [SerializeField] private ScoreView _scoreView;
    [SerializeField] private ShipSpeedView _shipSpeedView;
    [SerializeField] private UserInput _userInput;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        DeclareSignals();
        BindConfig();
        
        Container.Bind<Ship>().FromInstance(_ship).AsSingle().NonLazy();
        Container.Bind<Spawner>().FromInstance(_spawner).AsSingle().NonLazy();

        BindServices();
        BindPools();
        
        BindViewModel();
        BindView();
        Container.Bind<CustomScanner>().AsSingle().NonLazy();
        Container.Bind<Resolver>().AsSingle().NonLazy();
    }

    private void BindViewModel()
    {
        Container.Bind<ViewModel>().AsSingle().NonLazy();
    }

    private void BindView()
    {
        Container.Bind<ScoreView>().FromInstance(_scoreView).AsSingle().NonLazy();
        Container.Bind<ShipSpeedView>().FromInstance(_shipSpeedView).AsSingle().NonLazy();
        Container.Bind<UserInput>().FromInstance(_userInput).AsSingle().NonLazy();
    }

    [ContextMenu(nameof(RefreshConfig))]
    private void RefreshConfig()
    {
         JsonReader reader = new JsonReader();
         Config config = reader.Read();
         reader.Write(config);
    } 

    private void BindConfig()
    {
        JsonReader reader = new JsonReader();
        Config config = reader.Read();
        Container.Bind<Config>().FromInstance(config).AsSingle().NonLazy();
    }

    private void BindPools()
    {
        var container = new PoolContainer
        {
            BigAsteroidPool = new Pool<BigAsteroid>(_bigAsteroidPrefab),
            SmallAsteroidPool = new Pool<SmallAsteroid>(_smallAsteroidPrefab),
            UfoPool = new Pool<Ufo>(_ufoPrefab),
            RocketPool = new Pool<Rocket>(_rocketPrefab)
        };
        
        Container.BindInstance(container).AsSingle().NonLazy();
    }

    private void BindServices()
    {
        Container.Bind<CollisionService>().AsSingle().NonLazy();
        Container.Bind<HealthService>().AsSingle().NonLazy();
        Container.Bind<PauseService>().AsSingle().NonLazy();
        Container.Bind<ScoreService>().AsSingle().NonLazy();
        Container.Bind<SpeedService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PhysicsService>().AsSingle().NonLazy();
    }

    private void DeclareSignals()
    {
        Container.DeclareSignal<EnemyDefeatedSignal>();
        Container.DeclareSignal<BodyCreatedSignal>();
        Container.DeclareSignal<BodyReleaseSignal>();
        Container.DeclareSignal<PauseStateChangedSignal>(); //
        Container.DeclareSignal<PauseStateChanged>();
        Container.DeclareSignal<RocketFiredSignal>();
        Container.DeclareSignal<ScoreChangedSignal>();
        Container.DeclareSignal<ShipSpeedChangedSignal>();
        Container.DeclareSignal<ShipTakeDamagedSignal>();
    }
}