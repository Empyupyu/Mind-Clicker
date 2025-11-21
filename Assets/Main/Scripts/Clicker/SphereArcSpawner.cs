using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO
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

        DOTween.Sequence()
            .AppendInterval(1)
            .AppendCallback(() =>
            {
                var cam = Camera.main;
                var sphereWorldCenter = anchor.transform.position;

                var screenPos = cam.WorldToScreenPoint(sphereWorldCenter);
                screenPos.z = Mathf.Max(screenPos.z - 0.58f, 0.1f);

                var canvasWorldPos = cam.ScreenToWorldPoint(screenPos);

                view.transform.position = canvasWorldPos;
                view.transform.rotation = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);
                view.gameObject.SetActive(true);
            })
            .Append(view.transform.DOScale(.00053f, 1f)
                .From(0)
                .SetDelay((arcPoints.Count - 1) * spawnPoint.Data.DelayStep)
                .OnComplete(() => OnSpawnCompleted?.Invoke(spawnPoint)));
    }
}
