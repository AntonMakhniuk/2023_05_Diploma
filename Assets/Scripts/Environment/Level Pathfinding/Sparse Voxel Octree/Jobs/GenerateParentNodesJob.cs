using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree.Jobs
{
    [BurstCompile]
    public struct GenerateParentNodesJob : IJob
    {
        [ReadOnly] public NativeArray<SparseNode> ChildNodes;
        public NativeList<SparseNode> ParentNodes;
        [NativeDisableParallelForRestriction]
        public NativeArray<int> ChildParentIndices;
        
        public void Execute()
        {
            var parentNodeMap = new NativeHashMap<ulong, int>(ChildNodes.Length, Allocator.Temp);

            foreach (var childNode in ChildNodes)
            {
                var parentCode = MortonCodeEncoding.GetParentMortonCode(childNode.MortonCode);

                if (parentNodeMap.ContainsKey(parentCode))
                {
                    continue;
                }
                
                var parentIndex = ParentNodes.Length;
                
                ParentNodes.Add(new SparseNode(parentCode) { IsLeaf = false });
                parentNodeMap.Add(parentCode, parentIndex);
            }

            for (var i = 0; i < ChildNodes.Length; i++)
            {
                var childNode = ChildNodes[i];
                var parentCode = MortonCodeEncoding.GetParentMortonCode(childNode.MortonCode);
                
                if (parentNodeMap.TryGetValue(parentCode, out var parentIndex))
                {
                    ChildParentIndices[i] = parentIndex;
                }
                else
                {
                    ChildParentIndices[i] = -1;
                    Debug.LogError($"Parent node not found for child {i} with parent code {parentCode}");
                }
            }
            
            parentNodeMap.Dispose();
        }
    }
}