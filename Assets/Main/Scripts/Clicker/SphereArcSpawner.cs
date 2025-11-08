using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SphereArcSpawner
{
    public event Action<SpawnPoint> OnSpawnCompleted;

    private readonly ISphereArcBuilder arcBuilder;
    private readonly ISphereArcAnimator animator;

    public SphereArcSpawner(ISphereArcBuilder arcBuilder, ISphereArcAnimator animator)
    {
        this.arcBuilder = arcBuilder;
        this.animator = animator;
    }

    public void Spawn(ThoughtUIView view, NegativeThoughtForm config, SpawnPoint spawnPoint)
    {
        Disable(spawnPoint);

        var points = arcBuilder.SampleArcPoints(spawnPoint.Data);

        for (int i = 0; i < points.Count; i++)
        {
            var prefab = i == points.Count - 1 ? config.Head : config.Body;
            var sphere = GameObject.Instantiate(prefab, points[i], Quaternion.identity);
            sphere.transform.localScale = Vector3.zero;

            spawnPoint.Mesh.Add(sphere);
            animator.Animate(sphere, i, points.Count, points[i], spawnPoint.Data);
        }

        InstallView(view, points, spawnPoint);
        spawnPoint.SetActive(true);
    }

    public void Disable(SpawnPoint spawnPoint)
    {
        foreach (var obj in spawnPoint.Mesh)
        {
            if (obj == null) continue;
            obj.transform.DOKill();
            obj.transform.DOScale(0, .2f).OnComplete(() => GameObject.Destroy(obj));
        }

        spawnPoint.Mesh.Clear();
        spawnPoint.SetActive(false);
    }

    private void InstallView(ThoughtUIView view, List<Vector3> arcPoints, SpawnPoint spawnPoint)
    {
        spawnPoint.SetThoughtUIView(view);

        var anchor = spawnPoint.Mesh[^1];
        anchor.AddComponent<ThoughtMeshView>();

        view.transform.SetParent(anchor.transform);
        view.transform.localPosition = Vector3.zero;
        view.transform.localScale = Vector3.zero;

        view.transform.DOScale(.00053f, 1f)
            .From(0)
            .SetDelay((arcPoints.Count - 1) * spawnPoint.Data.DelayStep)
            .OnComplete(() => OnSpawnCompleted?.Invoke(spawnPoint));

        AlignCanvas(view.transform, anchor.transform, Camera.main);
        view.gameObject.SetActive(true);
    }

    private void AlignCanvas(Transform canvas, Transform sphere, Camera cam, float offset = -0.55f)
    {
        canvas.rotation = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);
        canvas.localPosition = sphere.InverseTransformDirection(cam.transform.forward * offset);
    }
}
