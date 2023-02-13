using System.Collections.Generic;

namespace ServiceNS {
	public static class ObjectPool<T> where T : PoolableObject, new() {
		public static Stack<T> pool = new();

		public static T Get() {
			if (pool.Count == 0) {
				return new T();
			}

			return pool.Pop();
		}

		public static void Release(T poolableObject) {
			pool.Push(poolableObject);
		}
	}
}