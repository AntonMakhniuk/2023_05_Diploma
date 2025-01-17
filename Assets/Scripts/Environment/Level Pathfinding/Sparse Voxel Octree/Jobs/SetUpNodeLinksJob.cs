using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree.Jobs
{
    [BurstCompile]
    public struct SetUpNodeLinksJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<SparseNode> InputNodes;
        public NativeArray<SparseNode> OutputNodes;

        public void Execute(int index)
        {
            var node = InputNodes[index];
            var neighborCodes = new FixedList512Bytes<long>();

            for (var i = 0; i < 6; i++)
            {
                neighborCodes.Add(-1);
            }
            
            MortonCodeEncoding.GetNeighborCodes(node.MortonCode, ref neighborCodes);
            
            for (var i = 0; i < 6; i++)
            {
                var neighborCode = neighborCodes[i];
                var neighborIndex = -1;

                for (var j = 0; j < InputNodes.Length; j++)
                {
                    if (InputNodes[j].MortonCode != neighborCode)
                    {
                        continue;
                    }
                    
                    neighborIndex = j;
                    
                    break;
                }

                node.NeighborIndices[i] = neighborIndex;
            }

            OutputNodes[index] = node;
        }
    }
}