using System.Collections.Generic;

namespace WorldNS {
	public class AreaDetection {
		public List<AreaDetectionLayerBase> areaDetectionLayers = new List<AreaDetectionLayerBase>();

		public bool IsClean(DetectionSet detectionSet) {
			foreach (var detectionLayer in areaDetectionLayers) {
				if (!detectionLayer.IsClean(detectionSet)) {
					return false;
				}
			}

			return true;
		}
	}
}