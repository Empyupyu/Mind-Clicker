using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint
{
    public bool IsActive { get; private set; }
    public ThoughtUIView ThoughtUIView { get; private set; }
    public ThoughtSpawnPointData Data { get; private set; }
    public List<GameObject> Mesh { get; private set; } = new();

    public SpawnPoint(ThoughtSpawnPointData data)
    {
        this.Data = data;
    }

    public void SetThoughtUIView(ThoughtUIView thoughtUIView)
    {
        ThoughtUIView = thoughtUIView;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }
}
