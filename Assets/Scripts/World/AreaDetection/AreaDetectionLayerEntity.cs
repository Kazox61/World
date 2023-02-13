namespace WorldNS {
	public class AreaDetectionLayerEntity : AreaDetectionLayerBase {
		public override bool IsClean(DetectionSet detectionSet) {
			return IsCleanAgainstEntity(detectionSet);
		}

		private bool IsCleanAgainstEntity(DetectionSet detectionSet) {
			if (IsOverlappingEntities(detectionSet)) {
				return false;
			}

			return true;
		}

		private bool IsOverlappingEntities(DetectionSet detectionSet) {
			var setupEntity = detectionSet.entitySetup;
			var field = detectionSet.field;

			return EntityHelper.GetOverlappingEntities(setupEntity, field).Count > 0;
		}
	}
}