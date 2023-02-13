using System.Collections.Generic;
using SetupNS;

namespace WorldNS {
	public class AreaDetectionBuilder {
		public static AreaDetectionBuilder Instance { get; } = new();

		private readonly List<AreaDetectionLayerBase> areaDetectionLayers = new() {
			new AreaDetectionLayerGround(),
			new AreaDetectionLayerDecoration(),
			new AreaDetectionLayerEntity()
		};

		public AreaDetection GetAreaDetection(AreaDetectionLayers layers) {
			var areaDetection = new AreaDetection();
			if (!layers.ignoreLayerGround) {
				areaDetection.areaDetectionLayers.Add(areaDetectionLayers[0]);
			}

			if (!layers.ignoreLayerDecoration) {
				areaDetection.areaDetectionLayers.Add(areaDetectionLayers[1]);
			}

			if (!layers.ignoreLayerEntity) {
				areaDetection.areaDetectionLayers.Add(areaDetectionLayers[2]);
			}

			return areaDetection;
		}
	}
}