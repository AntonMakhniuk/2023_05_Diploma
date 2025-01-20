using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree.Jobs
{
    [BurstCompile]
    public struct GenerateLeafNodesJob : IJob
    {
        [ReadOnly] public NativeArray<ulong> ParentMortonCodes;        
        public NativeArray<SparseNode> LeafNodes;
        
        public void Execute()
        {
            var leafIndex = 0;

            for (var i = 0; i < ParentMortonCodes.Length; i++)
            {
                var parentCoords = MortonCodeEncoding.DecodeMortonTo3D(ParentMortonCodes[i]);
                
                for (var x = 0; x < 2; x++)
                {
                    for (var y = 0; y < 2; y++)
                    {
                        for (var z = 0; z < 2; z++)
                        {
                            var leafLocalCoords = new int3(x, y, z);
                            var leafCoords = parentCoords * 2 + leafLocalCoords;
                            var leafMortonCode = 
                                MortonCodeEncoding.Encode3DtoMorton(leafCoords.x, leafCoords.y, leafCoords.z);

                            var leafNode = new SparseNode(leafMortonCode)
                            {
                                IsLeaf = true
                            };
                            
                            LeafNodes[leafIndex++] = leafNode;
                        }
                    }
                }
            }
        }
    }
}