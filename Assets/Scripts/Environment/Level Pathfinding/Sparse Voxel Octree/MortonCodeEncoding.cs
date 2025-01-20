using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree
{
    public struct Long3
    {
        public long x;
        public long y;
        public long z;

        public Long3(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Long3 operator >>(Long3 a, int shift)
        {
            return new Long3(a.x >> shift, a.y >> shift, a.z >> shift);
        }
        
        public static Long3 operator +(Long3 a, Long3 b)
        {
            return new Long3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        
        public static Long3 operator +(Long3 a, int3 b)
        {
            return new Long3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        
        public static Long3 operator *(Long3 a, int b)
        {
            return new Long3(a.x * b, a.y * b, a.z * b);
        }
        
        public static explicit operator float3(Long3 a)
        {
            return new float3(a.x, a.y, a.z);
        }
    }
    
    [BurstCompile]
    public static class MortonCodeEncoding
    {
        private const int Offset = 4096;
        
        public static ulong Encode3DtoMorton(long x, long y, long z)
        {
            return (ExpandCoordinateBits((ulong)(x + Offset)) << 2)
                   | (ExpandCoordinateBits((ulong)(y + Offset)) << 1)
                   | ExpandCoordinateBits((ulong)(z + Offset));
        }
        
        private static ulong ExpandCoordinateBits(ulong coordinate)
        {
            var newCoord = coordinate;
            
            newCoord &= 0x1FFFFF;
            newCoord = (newCoord | (newCoord << 32)) & 0x1F00000000FFFF;
            newCoord = (newCoord | (newCoord << 16)) & 0x1F0000FF0000FF;
            newCoord = (newCoord | (newCoord << 8)) & 0x100F00F00F00F00F;
            newCoord = (newCoord | (newCoord << 4)) & 0x10C30C30C30C30C3;
            newCoord = (newCoord | (newCoord << 2)) & 0x1249249249249249;
            
            return newCoord;
        }
        
        public static Long3 DecodeMortonTo3D(ulong mortonCode)
        {
            return new Long3((long)CompactCoordinateBits(mortonCode >> 2) - Offset,
                (long)CompactCoordinateBits(mortonCode >> 1) - Offset,
                (long)CompactCoordinateBits(mortonCode) - Offset);
        }

        private static ulong CompactCoordinateBits(ulong coordinate)
        {
            var newCoord = coordinate;
            
            newCoord &= 0x1249249249249249;
            newCoord = (newCoord | (newCoord >> 2)) & 0x10C30C30C30C30C3;
            newCoord = (newCoord | (newCoord >> 4)) & 0x100F00F00F00F00F;
            newCoord = (newCoord | (newCoord >> 8)) & 0x1F0000FF0000FF;
            newCoord = (newCoord | (newCoord >> 16)) & 0x1F00000000FFFF;
            newCoord = (newCoord | (newCoord >> 32)) & 0x1FFFFF;
            
            return newCoord;
        }
        
        public static ulong GetParentMortonCode(ulong childNodeMortonCode)
        {
            var parentCoords = DecodeMortonTo3D(childNodeMortonCode) >> 1;
            
            return Encode3DtoMorton(parentCoords.x, parentCoords.y, parentCoords.z);
        }

        public static void GetNeighborCodes(ulong mortonCode, ref NativeList<ulong> neighbors)
        {
            var pos = DecodeMortonTo3D(mortonCode);

            neighbors.Clear();

            neighbors.Add(Encode3DtoMorton(pos.x + 1, pos.y, pos.z));
            neighbors.Add(Encode3DtoMorton(pos.x - 1, pos.y, pos.z));
            neighbors.Add(Encode3DtoMorton(pos.x, pos.y + 1, pos.z));
            neighbors.Add(Encode3DtoMorton(pos.x, pos.y - 1, pos.z));
            neighbors.Add(Encode3DtoMorton(pos.x, pos.y, pos.z + 1));
            neighbors.Add(Encode3DtoMorton(pos.x, pos.y, pos.z - 1));
        }

        public static float3 DecodeMortonTo3DFloat3(ulong mortonCode)
        {
            return new float3((long)CompactCoordinateBits(mortonCode >> 2) - Offset, 
                (long)CompactCoordinateBits(mortonCode >> 1) - Offset, 
                (long)CompactCoordinateBits(mortonCode) - Offset);
        }
    }
}