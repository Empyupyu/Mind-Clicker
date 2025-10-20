using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SphereArcSpawner : MonoBehaviour
{
    public event Action<SphereArcSpawner> OnSpawnCompleted;
    public bool IsActive {  get; private set; }
    public ThoughtUIView ThoughtUIView {  get; private set; }

    [Header("Setup")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Arc Settings")]
    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private float minDistanceBetweenSpheres = 1f;
    [SerializeField] private int maxSphereCount = 10;
    [SerializeField] private Vector3 targetOffset;

    [Header("Animation")]
    [SerializeField] private float baseScale = 0.5f;
    [SerializeField] private float scaleExponent = 2f;
    [SerializeField] private float baseDuration = 1.2f;
    [SerializeField] private float durationDecay = 0.1f;
    [SerializeField] private float delayStep = 0.1f;
    [SerializeField] private float approachDistance = 2f;

    private List<GameObject> thoughtMesh = new List<GameObject>();
    private Transform target;

    private void Start()
    {
        target = Camera.main.transform;
    }

    private void MoveTowardTargetLocal(Transform targetObj)
    {
        if (target == null) return;

        Vector3 worldDirection = ((target.position + targetOffset) - targetObj.position).normalized;

        Vector3 localDirection = targetObj.InverseTransformDirection(worldDirection);

        targetObj.localPosition += localDirection * approachDistance;
    }

    public void AlignCanvasToSphereCenter(Transform canvas, Transform sphereTransform, Camera camera, float canvasDepthOffset = -0.55f)
    {
        Vector3 sphereWorldPos = sphereTransform.position;
        Vector3 screenPoint = camera.WorldToScreenPoint(sphereWorldPos);

        Vector3 projectedForward = camera.transform.forward;
        Vector3 projectedUp = camera.transform.up;

        canvas.transform.rotation = Quaternion.LookRotation(projectedForward, projectedUp);

        Vector3 offset = camera.transform.forward * canvasDepthOffset;
        canvas.transform.localPosition = sphereTransform.InverseTransformDirection(offset);

        canvas.transform.localPosition = sphereTransform.InverseTransformDirection(camera.transform.forward * canvasDepthOffset);

        canvas.transform.rotation = Quaternion.LookRotation(camera.transform.forward, camera.transform.up);
    }

    public void SpawnAlongArc(ThoughtUIView view, NegativeThoughtForm thoughtConfig)
    {
        List<Vector3> arcPoints = SampleArcPoints(minDistanceBetweenSpheres, maxSphereCount);
        DisableView();

        for (int i = 0; i < arcPoints.Count; i++)
        {
            Vector3 pos = arcPoints[i];

            GameObject thougthPice = i == arcPoints.Count - 1 ? thoughtConfig.Head : thoughtConfig.Body;
            GameObject sphere = Instantiate(thougthPice, pos, Quaternion.identity);

            sphere.transform.localScale = Vector3.zero;

            thoughtMesh.Add(sphere);
            SpawnAnimation(arcPoints, i, pos, sphere);
        }

        ThoughtViewInstall(view, arcPoints);

        IsActive = true;
    }

    private void SpawnAnimation(List<Vector3> arcPoints, int i, Vector3 pos, GameObject sphere)
    {
        float t = (float)i / (arcPoints.Count - 1);
        float scale = baseScale + Mathf.Pow(t, scaleExponent);
        float duration = Mathf.Max(0.2f, baseDuration - t * durationDecay);
        float delay = i * delayStep;

        sphere.transform.DOScale(scale, duration)
            .SetEase(Ease.OutBack)
            .SetDelay(delay);

        float floatAmplitude = Random.Range(0.05f, 0.15f);
        float floatDuration = Random.Range(1.5f, 3f);

        sphere.transform.DOMoveY(pos.y + floatAmplitude, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay + duration);
    }

    private void ThoughtViewInstall(ThoughtUIView view, List<Vector3> arcPoints)
    {
        ThoughtUIView = view;

        thoughtMesh[thoughtMesh.Count - 1].AddComponent<ThoughtMeshView>();
        view.transform.SetParent(thoughtMesh[thoughtMesh.Count - 1].transform);
        view.transform.localPosition = Vector3.zero;
        view.transform.localScale = Vector3.zero;
        view.transform.DOScale(.00053f, 1).From(0).SetDelay((arcPoints.Count - 1) * delayStep).OnComplete(() =>
        {
            OnSpawnCompleted?.Invoke(this);
        });

        AlignCanvasToSphereCenter(view.transform, thoughtMesh[thoughtMesh.Count - 1].transform, Camera.main);

        view.gameObject.SetActive(true);
    }

    public void DisableView()
    {
        if(thoughtMesh.Count > 0)
        {
            thoughtMesh[thoughtMesh.Count - 1].transform.GetChild(0).DOKill();

            foreach(var gameObject in thoughtMesh)
            {
                gameObject.transform.DOKill();
                gameObject.transform.DOScale(0, .2f).OnComplete(() =>
                {
                    Destroy(gameObject);
                });
            }
        }

        thoughtMesh = new List<GameObject>();

        IsActive = false;
    }

    private List<Vector3> SampleArcPoints(float minDistance, int maxCount)
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 p0 = pointA.position;
        Vector3 p2 = pointB.position;
        Vector3 mid = (p0 + p2) * 0.5f + Vector3.up * arcHeight;
        int samples = 100;

        Vector3 lastPoint = Bezier(p0, mid, p2, 0f);
        points.Add(lastPoint);

        for (int i = 1; i <= samples && points.Count < maxCount; i++)
        {
            float t = (float)i / samples;
            Vector3 current = Bezier(p0, mid, p2, t);
            if (Vector3.Distance(lastPoint, current) >= minDistance)
            {
                points.Add(current);
                lastPoint = current;
            }
        }

        return points;
    }

    private Vector3 Bezier(Vector3 a, Vector3 control, Vector3 b, float t)
    {
        Vector3 ab = Vector3.Lerp(a, control, t);
        Vector3 bc = Vector3.Lerp(control, b, t);
        return Vector3.Lerp(ab, bc, t);
    }
}

