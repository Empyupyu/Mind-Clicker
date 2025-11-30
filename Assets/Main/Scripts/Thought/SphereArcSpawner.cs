using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SphereArcSpawner
{
    public event Action<SpawnPoint> OnSpawnCompleted;

    private readonly ISphereArcBuilder arcBuilder;
    private readonly ISphereArcAnimator animator;
    private readonly SphereArcConfig sphereArcConfig;

    public SphereArcSpawner(ISphereArcBuilder arcBuilder, ISphereArcAnimator animator, SphereArcConfig sphereArcConfig)
    {
        this.arcBuilder = arcBuilder;
        this.animator = animator;
        this.sphereArcConfig = sphereArcConfig;
    }

    public void Spawn(ThoughtUIView view, NegativeThoughtForm config, SpawnPoint spawnPoint)
    {
        ClearMesh(spawnPoint);

        var points = arcBuilder.SampleArcPoints(spawnPoint.Data);
        CreateMesh(config, spawnPoint, points);

        InstallView(view, points, spawnPoint);
        spawnPoint.SetActive(true);
    }

    private void CreateMesh(NegativeThoughtForm config, SpawnPoint spawnPoint, List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            var prefab = i == points.Count - 1 ? config.Head : config.Body;
            var sphere = GameObject.Instantiate(prefab, points[i], Quaternion.identity);
            sphere.transform.localScale = Vector3.zero;

            spawnPoint.Mesh.Add(sphere);
            animator.Animate(sphere, i, points.Count, points[i], spawnPoint.Data);
        }
    }

    private void ClearMesh(SpawnPoint spawnPoint)
    {
        foreach (var obj in spawnPoint.Mesh)
        {
            if (obj == null) continue;
            obj.transform.DOKill();
            obj.transform.DOScale(sphereArcConfig.ViewScaleOnStart, sphereArcConfig.DisableScaleDuration).OnComplete(() => GameObject.Destroy(obj));
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

        ViewAnimation(view, arcPoints, spawnPoint, anchor);
    }

    private void ViewAnimation(ThoughtUIView view, List<Vector3> arcPoints, SpawnPoint spawnPoint, GameObject anchor)
    {
        DOTween.Sequence()
                    .AppendInterval(sphereArcConfig.ViewAppearDelay)
                    .AppendCallback(() =>
                    {
                        var cam = Camera.main;
                        var sphereWorldCenter = anchor.transform.position;

                        var screenPos = cam.WorldToScreenPoint(sphereWorldCenter);
                        screenPos.z = Mathf.Max(screenPos.z - sphereArcConfig.CameraZOffset, sphereArcConfig.MinScreenZ);

                        var canvasWorldPos = cam.ScreenToWorldPoint(screenPos);

                        view.transform.position = canvasWorldPos;
                        view.transform.rotation = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);
                        view.gameObject.SetActive(true);
                    })
                    .Append(view.transform.DOScale(sphereArcConfig.ViewScale, sphereArcConfig.ViewScaleDuration)
                        .From(0)
                        .SetDelay((arcPoints.Count - 1) * spawnPoint.Data.DelayStep)
                        .OnComplete(() => OnSpawnCompleted?.Invoke(spawnPoint)));
    }

    public void Disable(SpawnPoint spawnPoint)
    {
        ClearMesh(spawnPoint);
    }
}
