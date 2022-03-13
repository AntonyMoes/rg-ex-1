using System;
using System.Linq;
using Physics;
using Physics.Calculators;
using UnityEngine;

public class PhysicsBodiesInitializer : MonoBehaviour {
    [SerializeField] private PhysicsBodyAdapter[] _stationaryBodies;
    [SerializeField] private OrbitingBody[] _orbitingBodies;

    public PhysicsBody[] InitializePhysicsBodies() {
        foreach (var stationaryBody in _stationaryBodies) {
            stationaryBody.Setup();
            stationaryBody.Setup(new StationarySimplifiedCalculator(stationaryBody.PhysicsBody.Position, 0f));
        }

        foreach (var ob in _orbitingBodies) {
            if (ob.orbitingTarget.PhysicsBody == null) {
                ob.orbitingTarget.Setup();
            }

            ob.orbiter.Setup();
            ob.orbiter.Setup(new OrbitingSimplifiedCalculator(ob.orbitingTarget.PhysicsBody,
                ob.orbiter.PhysicsBody.Position, true));
        }

        var allBodies = GetComponentsInChildren<PhysicsBodyAdapter>();

        return _orbitingBodies.SelectMany(ob => new[] {ob.orbitingTarget, ob.orbiter})
            .Concat(_stationaryBodies)
            .Concat(allBodies)
            .Where(b => b.gameObject.activeInHierarchy)
            .Select(b => {
                if (b.PhysicsBody == null) {
                    b.Setup();
                }

                return b.PhysicsBody;
            })
            .ToHashSet()
            .ToArray();
    }

    [Serializable]
    public struct OrbitingBody {
        public PhysicsBodyAdapter orbitingTarget;
        public PhysicsBodyAdapter orbiter;
    }
}
