using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree
{
    [BurstCompile]
    public static class MortonCodeEncoding
    {
        private const int Offset = 4096;
        
        public static long Encode3DtoMorton(int x, int y, int z)
        {
            return (ExpandCoordinateBits((uint)x + Offset) << 2) 
                   | (ExpandCoordinateBits((uint)y + Offset) << 1) | ExpandCoordinateBits((uint)z + Offset);
        }
        
        private static long ExpandCoordinateBits(uint coordinate)
        {
            ulong newCoord = coordinate;
            
            newCoord &= 0xFFFF;
            newCoord = (newCoord | (newCoord << 32)) & 0x1F00000000FFFF;
            newCoord = (newCoord | (newCoord << 16)) & 0x1F0000FF0000FF;
            newCoord = (newCoord | (newCoord << 8)) & 0x100F00F00F00F00F;
            newCoord = (newCoord | (newCoord << 4)) & 0x10C30C30C30C30C3;
            newCoord = (newCoord | (newCoord << 2)) & 0x1249249249249249;
            
            return (long)newCoord;
        }
        
        public static int3 DecodeMortonTo3D(long mortonCode)
        {
            return new int3((int)CompactCoordinateBits(mortonCode >> 2) - Offset, 
                (int)CompactCoordinateBits(mortonCode >> 1) - Offset, 
                (int)CompactCoordinateBits(mortonCode) - Offset);
        }

        private static long CompactCoordinateBits(long coordinate)
        {
            var newCoord = (ulong)coordinate;
            
            newCoord &= 0x1249249249249249;
            newCoord = (newCoord | (newCoord >> 2)) & 0x10C30C30C30C30C3;
            newCoord = (newCoord | (newCoord >> 4)) & 0x100F00F00F00F00F;
            newCoord = (newCoord | (newCoord >> 8)) & 0x1F0000FF0000FF;
            newCoord = (newCoord | (newCoord >> 16)) & 0x1F00000000FFFF;
            newCoord = (newCoord | (newCoord >> 32)) & 0xFFFF;
            
            return (long)newCoord;
        }
        
        public static long GetParentMortonCode(long childNodeMortonCode)
        {
            var parentCoords = DecodeMortonTo3D(childNodeMortonCode) / 2; 
            
            return Encode3DtoMorton(parentCoords.x, parentCoords.y, parentCoords.z);
        }

        public static void GetNeighborCodes(long mortonCode, ref FixedList512Bytes<long> neighbors)
        {
            var pos = DecodeMortonTo3D(mortonCode);

            neighbors[0] = Encode3DtoMorton(pos.x + 1, pos.y, pos.z);
            neighbors[1] = Encode3DtoMorton(pos.x - 1, pos.y, pos.z);
            neighbors[2] = Encode3DtoMorton(pos.x, pos.y + 1, pos.z); 
            neighbors[3] = Encode3DtoMorton(pos.x, pos.y - 1, pos.z); 
            neighbors[4] = Encode3DtoMorton(pos.x, pos.y, pos.z + 1); 
            neighbors[5] = Encode3DtoMorton(pos.x, pos.y, pos.z - 1);
        }

        public static float3 DecodeMortonTo3DFloat3(long mortonCode)
        {
            return new float3(CompactCoordinateBits(mortonCode >> 2) - Offset, 
                CompactCoordinateBits(mortonCode >> 1) - Offset, 
                CompactCoordinateBits(mortonCode) - Offset);
        }
    }
}