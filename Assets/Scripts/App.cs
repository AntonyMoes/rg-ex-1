using Physics;
using Time;
using UnityEngine;
using Values;

public class App : MonoBehaviour {
    [SerializeField] private Transform _objectsRoot;
    [SerializeField] private double _gravityConstant;
    private PhysicsEngine _physicsEngine;

    private TimeProvider _timeProvider;

    private void Start() {
        _timeProvider = new TimeProvider();

        var gravityConstantProperty = new ProxyValue<double>(
            () => _gravityConstant,
            value => _gravityConstant = value);
        _physicsEngine = new PhysicsEngine(gravityConstantProperty);
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
