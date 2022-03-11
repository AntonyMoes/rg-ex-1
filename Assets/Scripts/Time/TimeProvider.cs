using System.Collections.Generic;

namespace Time {
    public class TimeProvider {
        private readonly List<IFrameProcessor> _frameProcessors = new List<IFrameProcessor>();
        private readonly List<IFrameProcessor> _physicsFrameProcessors = new List<IFrameProcessor>();

        public void RegisterFrameProcessor(IFrameProcessor processor) {
            if (!_frameProcessors.Contains(processor)) {
                _frameProcessors.Add(processor);
            }
        }

        public void UnregisterFrameProcessor(IFrameProcessor processor) {
            _frameProcessors.Remove(processor);
        }

        public void ProcessFrame(float frameTime) {
            foreach (var processor in _frameProcessors) {
                processor.ProcessFrame(frameTime);
            }
        }

        public void RegisterPhysicsFrameProcessor(IFrameProcessor processor) {
            if (!_frameProcessors.Contains(processor)) {
                _physicsFrameProcessors.Add(processor);
            }
        }

        public void UnregisterPhysicsProcessor(IFrameProcessor processor) {
            _physicsFrameProcessors.Remove(processor);
        }

        public void ProcessPhysicsFrame(float frameTime) {
            foreach (var processor in _physicsFrameProcessors) {
                processor.ProcessFrame(frameTime);
            }
        }
    }
}
