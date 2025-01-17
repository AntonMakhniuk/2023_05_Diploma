using System;
using Environment.Level_Pathfinding.Sparse_Voxel_Octree.Jobs;
using Scriptable_Object_Templates.Singletons;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment.Level_Pathfinding.Sparse_Voxel_Octree
{
    public struct SparseVoxelOctree : IDisposable
    {
        public NativeArray<SparseNode>[] Levels;

        private readonly float _voxelSize;
        private readonly int _maxLevels;

        public SparseVoxelOctree(float voxelSize, int maxLevels)
        {
            _voxelSize = voxelSize;
            _maxLevels = maxLevels;
            Levels = new NativeArray<SparseNode>[maxLevels];
            
            var gameSpaceBounds = CreateGameSpaceBounds();
            
            var layer1VoxelSize = _voxelSize * 4f * 2f;
            var layer1MortonCodes = RasterizeToMorton(gameSpaceBounds, layer1VoxelSize);
            
            var numLeafNodes = layer1MortonCodes.Length * 8;
            Levels[0] = new NativeArray<SparseNode>(numLeafNodes, Allocator.Persistent);
            
            GenerateLeafNodes(layer1MortonCodes);
            layer1MortonCodes.Dispose();

            for (var level = 1; level < _maxLevels; level++)
            {
                var parentNodes = new NativeList<SparseNode>(Allocator.TempJob);
                var childParentIndices = new NativeArray<int>(Levels[level - 1].Length, Allocator.TempJob);
                
                var generateParentNodesJob = new GenerateParentNodesJob
                {
                    ChildNodes = Levels[level - 1],
                    ParentNodes = parentNodes,
                    ChildParentIndices = childParentIndices
                };
                
                generateParentNodesJob.Schedule().Complete();
                
                Levels[level] = new NativeArray<SparseNode>(parentNodes, Allocator.Persistent);
                
                var updateParentIndicesJob = new UpdateParentIndicesJob
                {
                    ChildNodes = Levels[level - 1],
                    ChildParentIndices = childParentIndices
                };
                
                updateParentIndicesJob.Schedule(Levels[level - 1].Length, 64).Complete();
                
                parentNodes.Dispose();
                childParentIndices.Dispose();
            }
            
            SetUpNodeLinks();
            RasterizeLeafNodes(gameSpaceBounds);
        }

        private static Bounds CreateGameSpaceBounds()
        {
            var radius = LevelBoundaryDictionary.Instance
                .dictionary[SceneManager.GetActiveScene().name]
                .zone3Radius;
            
            return new Bounds
            {
                extents = new Vector3(radius, radius, radius)
            };
        }
        
        private NativeList<long> RasterizeToMorton(Bounds gameSpaceBounds, float nodeSize)
        {
            var occupiedMortonCodes = new NativeList<long>(Allocator.Temp);
            var offset = nodeSize * 0.5f;
            var boxOffsets = new float3(offset);
            
            var minInt = new int3(gameSpaceBounds.min / nodeSize);
            var maxInt = new int3(gameSpaceBounds.max / nodeSize);

            for (var x = minInt.x; x < maxInt.x; x++)
            {
                for (var y = minInt.y; y < maxInt.y; y++)
                {
                    for (var z = minInt.z; z < maxInt.z; z++)
                    {
                        var nodeCenter = new float3(x + 0.5f, y + 0.5f, z + 0.5f) * nodeSize;
                        
                        if (Physics.CheckBox(nodeCenter, boxOffsets))
                        {
                            occupiedMortonCodes.Add(MortonCodeEncoding.Encode3DtoMorton(x, y, z));
                        }
                    }
                }
            }
            
            var uniqueMortonCodesSet = 
                new NativeHashSet<long>(occupiedMortonCodes.Length, Allocator.Temp);
            
            foreach (var mortonCode in occupiedMortonCodes)
            {
                uniqueMortonCodesSet.Add(mortonCode);
            }
            
            occupiedMortonCodes.Dispose();
            
            var uniqueMortonCodes = new NativeList<long>(uniqueMortonCodesSet.Count, Allocator.TempJob);
            
            uniqueMortonCodes.CopyFrom(uniqueMortonCodesSet.ToNativeArray(Allocator.Temp));
            uniqueMortonCodesSet.Dispose();

            return uniqueMortonCodes;
        }

        private void GenerateLeafNodes(NativeList<long> parentMortonCodes)
        {
            var generateLeafNodesJob = new GenerateLeafNodesJob
            {
                ParentMortonCodes = parentMortonCodes,
                LeafNodes = Levels[0]
            };
            
            generateLeafNodesJob.Schedule().Complete();
        }

        private void SetUpNodeLinks()
        {
            var inputNodes = new NativeArray<SparseNode>(0, Allocator.TempJob);
            var outputNodes = new NativeArray<SparseNode>(0, Allocator.TempJob);
            
            for (var level = 0; level < _maxLevels; level++)
            {
                if (inputNodes.Length != Levels[level].Length)
                {
                    if (inputNodes.IsCreated)
                    {
                        inputNodes.Dispose();
                    }
                    
                    inputNodes = new NativeArray<SparseNode>(Levels[level].Length, Allocator.TempJob);
                }
                if (outputNodes.Length != Levels[level].Length)
                {
                    if (outputNodes.IsCreated)
                    {
                        outputNodes.Dispose();
                    }
                    
                    outputNodes = new NativeArray<SparseNode>(Levels[level].Length, Allocator.TempJob);
                }
                
                inputNodes.CopyFrom(Levels[level]);
                
                var job = new SetUpNodeLinksJob()
                {
                    InputNodes = inputNodes,
                    OutputNodes = outputNodes
                };
                
                job.Schedule(Levels[level].Length, 64).Complete();
                
                Levels[level].CopyFrom(outputNodes);
            }
            
            inputNodes.Dispose();
            outputNodes.Dispose();
        }
        
        private void RasterizeLeafNodes(Bounds gameSpaceBounds)
        {
            var nodeSize = _voxelSize * 4;
            var offset = nodeSize * 0.5f;
            var boxOffsets = new float3(offset);
            
            var localVoxelPositions = new NativeArray<float3>(64, Allocator.Temp);
            var index = 0;
            
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    for (var z = 0; z < 4; z++)
                    {
                        localVoxelPositions[index++] = new float3(x, y, z) * _voxelSize + offset;
                    }
                }
            }

            for (var i = 0; i < Levels[0].Length; i++)
            {
                var node = Levels[0][i];
                var nodeCoords = MortonCodeEncoding.DecodeMortonTo3DFloat3(node.MortonCode);
                var nodeMinPosition = nodeCoords * nodeSize + (float3)gameSpaceBounds.min;
                var occupationData = 0UL;

                for (var j = 0; j < 64; j++)
                {
                    var globalVoxelPosition = nodeMinPosition + localVoxelPositions[j];
                    
                    if (Physics.CheckBox(globalVoxelPosition, boxOffsets))
                    {
                        occupationData |= 1UL << j;
                    }
                }
                
                node.LeafData = occupationData;
                Levels[0][i] = node;
            }
            
            localVoxelPositions.Dispose();
        }

        public void UpdateObject(Bounds oldBounds, Bounds newBounds)
        {
            //RemoveVoxels(oldBounds);
            //AddVoxels(newBounds);
        }
        
        public void Dispose()
        {
            for (var i = 0; i < Levels.Length; i++)
            {
                if (Levels[i].IsCreated)
                {
                    Levels[i].Dispose();
                }
            }
        }
    }
}