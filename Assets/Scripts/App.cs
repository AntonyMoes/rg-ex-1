using Physics;
using Time;
using UnityEngine;
using Values;

public class App : MonoBehaviour {
    [SerializeField] private Transform _objectsRoot;
    [SerializeField] private double _gravityConstant;
    [SerializeField] private float _trajectoryFrameTime;
    [SerializeField] private int _trajectoryFrameCount;

    private PhysicsEngine _physicsEngine;
    private TimeProvider _timeProvider;

    private void Start() {
        _timeProvider = new TimeProvider();

        var gravityConstantProxy = new ProxyValue<double>(
            () => _gravityConstant,
            value => _gravityConstant = value);
        var trajectoryFrameTimeProxy = new ProxyValue<float>(
            () => _trajectoryFrameTime,
            value => _trajectoryFrameTime = value);
        var trajectoryFrameCountProxy = new ProxyValue<int>(
            () => _trajectoryFrameCount,
            value => _trajectoryFrameCount = value);
        _physicsEngine = new PhysicsEngine(gravityConstantProxy, trajectoryFrameTimeProxy, trajectoryFrameCountProxy);
        _timeProvider.RegisterPhysicsFrameProcessor(_physicsEngine);

        var physicsBodies = _objectsRoot.GetComponentsInChildren<PhysicsBodyAdapter>();
        foreach (var body in physicsBodies) {
            body.Setup();
            _physicsEngine.RegisterBody(body.PhysicsBody);
        }
    }

    private void Update() {
        _timeProvider.ProcessFrame(UnityEngine.Time.deltaTime);
    }

    private void FixedUpdate() {
        _timeProvider.ProcessPhysicsFrame(UnityEngine.Time.fixedDeltaTime);
    }
}
