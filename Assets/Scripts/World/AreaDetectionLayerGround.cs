using System;

namespace WorldNS {
    public class AreaDetectionLayerGround: AreaDetectionLayerBase {
        public override bool IsClean(DetectionSet detectionSet) {
            return true;
        }
    }
}