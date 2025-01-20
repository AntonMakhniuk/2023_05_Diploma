using Unity.Collections;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree
{
    public struct SparseNode
    {
        public ulong MortonCode;
        public int ParentIndex;
        public int FirstChildIndex;
        public FixedList64Bytes<int> NeighborIndices;
        public bool IsLeaf;
        public ulong LeafData;
        
        public SparseNode(ulong mortonCode)
        {
            MortonCode = mortonCode;
            IsLeaf = false;
            FirstChildIndex = -1;
            ParentIndex = -1;
            NeighborIndices = new FixedList64Bytes<int>();
            
            for (var i = 0; i < 6; i++)
            {
                NeighborIndices.Add(-1);
            }
            
            LeafData = 0;
        }
    }
}