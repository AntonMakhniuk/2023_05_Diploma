using NaughtyAttributes;
using UnityEngine;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree
{
    public class SVOGenerator : MonoBehaviour
    {
        public static SVOGenerator Instance;
        
        [Foldout("SVO Data")] [SerializeField]
        private float voxelSize = 1f;
        [Foldout("SVO Data")] [SerializeField]
        private int maxLevels = 4;
        
        [Foldout("Visualisation Data")] [SerializeField]
        private bool drawLeafNodes = true;
        [Foldout("Visualisation Data")] [SerializeField]
        private bool drawParentNodes;
        [Foldout("Visualisation Data")] [SerializeField]
        private int levelToDraw = -1;
        
        private SparseVoxelOctree _svo;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            GenerateSVO();
        }

        [Button("Debug: Generate Instance (first)", enabledMode: EButtonEnableMode.Editor)]
        private void GenerateInstance()
        {
            Awake();
        }
        
        [Button("Debug: Generate SVO (second)", enabledMode: EButtonEnableMode.Editor)]
        public void GenerateSVO()
        {
            if (_svo.Levels != null)
            {
                _svo.Dispose();
            }
            
            _svo = new SparseVoxelOctree(voxelSize, maxLevels);

            if (!Application.isEditor)
            {
                return;
            }
            
            #if UNITY_EDITOR
                UnityEditor.SceneView.RepaintAll();
            #endif
        }

        [Button("Debug: Log SVO Data")]
        private void GenerateDebugData()
        {
            if (_svo.Levels == null)
            {
                return;
            }
            
            for (var level = 0; level < _svo.Levels.Length; level++)
            {
                Debug.Log(level + " " + _svo.Levels[level].Length);
            }
        }
        
        private void OnDrawGizmos()
        {
            if (_svo.Levels == null)
            {
                return;
            }

            for (var level = 0; level < _svo.Levels.Length; level++)
            {
                if (levelToDraw != -1 && level != levelToDraw)
                {
                    continue;
                }

                if (!_svo.Levels[level].IsCreated)
                {
                    continue;
                }
                
                foreach (var node in _svo.Levels[level])
                {
                    if (drawLeafNodes && node.IsLeaf)
                    {
                        DrawNode(node, level, Color.green);
                    }
                    else if (drawParentNodes && !node.IsLeaf)
                    {
                        DrawNode(node, level, Color.yellow);
                    }
                }
            }
        }
        
        private void DrawNode(SparseNode node, int level, Color color)
        {
            var position = MortonCodeEncoding.DecodeMortonTo3DFloat3(node.MortonCode);
            position *= (voxelSize * Mathf.Pow(2, level));
            var size = voxelSize * Mathf.Pow(2, level);

            Gizmos.color = color;
            Gizmos.DrawWireCube(position, new Vector3(size, size, size));
        }

        private void OnDestroy()
        {
            _svo.Dispose();
        }
    }
}