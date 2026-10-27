using UnityEngine;
using Zenject;

public class Bootstrap : MonoInstaller
{
    // [SerializeField] private GameFlow _gameFlow;
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
        
        // Container.Bind<GameFlow>().FromInstance(_gameFlow).AsSingle().NonLazy();
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
        PoolContainer container = new PoolContainer();
        container.BigAsteroidPool = new Pool<BigAsteroid>(_bigAsteroidPrefab);
        container.SmallAsteroidPool = new Pool<SmallAsteroid>(_smallAsteroidPrefab);
        container.UfoPool = new Pool<Ufo>(_ufoPrefab);
        container.RocketPool = new Pool<Rocket>(_rocketPrefab);
        Container.BindInstance(container).AsSingle().NonLazy();
    }

    private void BindServices()
    {
        Container.Bind<ContactHandler>().AsSingle().NonLazy();
        Container.Bind<HealthService>().AsSingle().NonLazy();
        Container.Bind<PauseServiсe>().AsSingle().NonLazy();
        Container.Bind<ScoreService>().AsSingle().NonLazy();
        Container.Bind<SpeedService>().AsSingle().NonLazy();
        // Container.Bind<PhysicService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PhysicService>().AsSingle().NonLazy();
    }

    private void DeclareSignals()
    {
        Container.DeclareSignal<EnemyDefeatedSignal>();
        // Container.DeclareSignal<FixedUpdateSignal>();
        // Container.DeclareSignal<FrameUpdateSignal>();
        Container.DeclareSignal<ObjectCreatedSignal>();
        Container.DeclareSignal<ObjectReleaseSignal>();
        Container.DeclareSignal<PauseButtonPressedSignal>(); //
        Container.DeclareSignal<PauseStateChanged>();
        Container.DeclareSignal<RocketFiredSignal>();
        Container.DeclareSignal<ScoreChangedSignal>();
        Container.DeclareSignal<ShipSpeedChangedSignal>();
        Container.DeclareSignal<ShipTakeDamagedSignal>();
    }
}