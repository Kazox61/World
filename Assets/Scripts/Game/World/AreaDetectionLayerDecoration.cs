using System;

namespace GameNS {
    public class AreaDetectionLayerDecoration: AreaDetectionLayerBase {
        public override bool IsClean(DetectionSet detectionSet) {
            return true;
        }
    }
}