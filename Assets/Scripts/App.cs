using Physics;
using Physics.Calculators;
using Time;
using UnityEngine;
using Values;

public class App : MonoBehaviour {
    [SerializeField] private PhysicsBodiesInitializer _initializer;
    [SerializeField] private double _gravityConstant;
    [SerializeField] private double _trajectoryFrameTime;
    [SerializeField] private int _trajectoryFrameCount;

    private PhysicsEngine _physicsEngine;
    private TimeProvider _timeProvider;

    private void Start() {
        _timeProvider = new TimeProvider();

        _physicsEngine = CreatePhysicsEngine();
        _timeProvider.RegisterPhysicsFrameProcessor(_physicsEngine);

        // var physicsBodies = _objectsRoot.GetComponentsInChildren<PhysicsBodyAdapter>();
        var physicsBodies = _initializer.InitializePhysicsBodies();
        foreach (var body in physicsBodies) {
            _physicsEngine.RegisterBody(body);
        }
    }

    private void Update() {
        _timeProvider.ProcessFrame(UnityEngine.Time.deltaTime);
    }

    private void FixedUpdate() {
        _timeProvider.ProcessPhysicsFrame(UnityEngine.Time.fixedDeltaTime);
    }

    private PhysicsEngine CreatePhysicsEngine() {
        var gravityConstantProxy = new ProxyValue<double>(
            () => _gravityConstant,
            value => _gravityConstant = value);
        var trajectoryFrameTimeProxy = new ProxyValue<double>(
            () => _trajectoryFrameTime,
            value => _trajectoryFrameTime = value);
        var trajectoryFrameCountProxy = new ProxyValue<int>(
            () => _trajectoryFrameCount,
            value => _trajectoryFrameCount = value);

        var physicsCalculator = new SimplifiedPhysicsCalculator(1e4);
        return new PhysicsEngine(physicsCalculator, gravityConstantProxy, trajectoryFrameTimeProxy,
            trajectoryFrameCountProxy);
    }
}
