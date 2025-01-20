using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree.Jobs
{
    [BurstCompile]
    public struct UpdateParentIndicesJob : IJobParallelFor
    {
        public NativeArray<SparseNode> ChildNodes;
        [ReadOnly] public NativeArray<int> ChildParentIndices;
        
        public void Execute(int index)
        {
            var childNode = ChildNodes[index];

            childNode.ParentIndex = ChildParentIndices[index];
            ChildNodes[index] = childNode;
        }
    }
}