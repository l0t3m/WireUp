
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;

public struct HandleNavMesh : IJob
{
    public NavMeshSurface Surface;
    
    public void Execute()
    {
        Surface.BuildNavMesh();
    }  
}
