using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace WorldNS {
    public class NavMeshPath2D {
        public static NavMeshPath2D Instance { get; } = new NavMeshPath2D();

        private NavMeshDataInstance navMeshDataInstance;
        private readonly List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();

        private const int WALKABLE_AREA = 0;
        private const int NOT_WALKABLE_AREA = 1;
        private readonly Quaternion ROTATION = Quaternion.Euler(-90, 0, 0);
        private const float EXTEND = 20f;
        
        public void BuildNavMesh(Vector3 center, Vector2 navMeshSize) {
            var settings = GetBuildSettings();
            CollectSources();
            AddDefaultWalkableArea(center, navMeshSize);
            
            
            var bounds = new Bounds(Vector3.zero, new Vector3(navMeshSize.x, 10, navMeshSize.y));
            var data = NavMeshBuilder.BuildNavMeshData(settings, sources, bounds, center,
                Quaternion.Euler(new Vector3(-90, 0)));
            
            if (data != null) {
                RemoveData();
                //AddData();
            }
            
            navMeshDataInstance = NavMesh.AddNavMeshData(data, center, ROTATION);
        }

        private void RemoveData() {
            navMeshDataInstance.Remove();
            navMeshDataInstance = new NavMeshDataInstance();
        } 
        
        private NavMeshBuildSettings GetBuildSettings()
        {
            var buildSettings = NavMesh.GetSettingsByID(0);
            if (buildSettings.agentTypeID == -1)
            {
                Debug.LogWarning("No build settings for agent type ID " + 0);
            }
            return buildSettings;
        }
        
        private void CollectSources() {
            var modifiers = NavMeshModifier2d.ActiveModifiers;
            sources.Clear();
            
            foreach (var modifier in modifiers) {
                var collider2D = modifier.navMeshCollider;

                if (collider2D == null) {
                    Debug.LogError("There is no Collider on a NavMeshModifier2d");
                }
                
                var mesh = collider2D.CreateMesh(false, false);

                var src = new NavMeshBuildSource {
                    shape = NavMeshBuildSourceShape.Mesh,
                    area = NOT_WALKABLE_AREA,
                    component = collider2D,
                    sourceObject = mesh
                };
                
                if (collider2D.attachedRigidbody) {
                    src.transform = Matrix4x4.TRS(Vector3.Scale(collider2D.attachedRigidbody.transform.position, Vector3.one),
                        collider2D.attachedRigidbody.transform.rotation, Vector3.one);
                }
                else {
                    src.transform = Matrix4x4.identity;
                }

                sources.Add(src);
            }
        }

        private void AddDefaultWalkableArea(Vector3 position, Vector3 size) {
            var src = new NavMeshBuildSource {
                transform = Matrix4x4.Translate(position),
                shape = NavMeshBuildSourceShape.Box,
                size = size,
                area = WALKABLE_AREA
            };
            sources.Add(src);
        }
        public Vector2[] GetPath(Vector3 from, Vector3 to) {
            var center = (from + to) / 2;
            center.z = 0;
            
            var maxX = Mathf.Max(from.x, to.x);
            var maxY = Mathf.Max(from.y, to.y);
            var navMeshSize = new Vector2(maxX - center.x + EXTEND, maxY - center.y + EXTEND);

            BuildNavMesh(center, navMeshSize);
            
            var path = new NavMeshPath();
            NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
            
            var corners = path.corners.Select(point => (Vector2)point).ToArray();
            NavMeshDebugger.Instance.AddPathToList(corners);
            
            return corners;
        }
    }
}