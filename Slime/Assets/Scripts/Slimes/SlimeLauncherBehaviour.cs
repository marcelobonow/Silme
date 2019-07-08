using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static Flags;

[Flags]
public enum LaunchState
{
    NotClicked = 0,
    Holding = 1,
    Flying = 2,
}


public class SlimeLauncherBehaviour : MonoBehaviour
{
    [SerializeField] private float gizmosDistance = 0.25f;
    [SerializeField] private SlimeBehaviour slimeBehaviour;
    [SerializeField] private Transform basePosition;
    [SerializeField] private Transform GizmosParent;
    [SerializeField] private GameObject previewGizmoPrefab;
    [SerializeField] private float force;
    private const int gizmosPreviewQuantity = 4;

    private LaunchState currentLaunchState;
    private List<GameObject> previewGizmos;

    private void Awake()
    {
        Assert.IsNotNull(slimeBehaviour);
        Assert.IsNotNull(basePosition);
        Assert.IsNotNull(previewGizmoPrefab);
        previewGizmos = new List<GameObject>();
        for(var i = 0; i < gizmosPreviewQuantity; i++)
        {
            var gizmo = Instantiate(previewGizmoPrefab, GizmosParent);
            gizmo.SetActive(false);
            previewGizmos.Add(gizmo);
        }
    }

    public void OnPointerDown()
    {
        if(slimeBehaviour.isAttached)
        {
            currentLaunchState = LaunchState.Holding;
            UpdateGizmoState();
        }
    }
    public void OnPointerUp()
    {
        var mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(CanLaunch(slimeBehaviour.currentWall, mousePosition))
            slimeBehaviour.LaunchSlime(force * (mousePosition - (Vector2)slimeBehaviour.transform.position).normalized);

        currentLaunchState = LaunchState.NotClicked;
        UpdateGizmoState();
    }

    private void Update()
    {
        UpdateGizmosPosition();
    }

    private void UpdateGizmosPosition()
    {
        if(currentLaunchState == LaunchState.Holding)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var angle = Vector2.SignedAngle(Vector2.left, (Vector2)slimeBehaviour.transform.position - (Vector2)mousePosition) * Mathf.Deg2Rad;
            Assert.IsTrue(angle < Mathf.PI * 2);
            for(var i = 0; i < gizmosPreviewQuantity; i++)
            {
                var gizmo = previewGizmos[i];
                gizmo.transform.position = GetGizmoPosition(force, angle, (i+1) * gizmosDistance * 1/force);
            }
        }
    }
    private void UpdateGizmoState()
    {
        foreach(var gizmo in previewGizmos)
            gizmo.SetActive(currentLaunchState == LaunchState.Holding);
    }
    private Vector2 GetGizmoPosition(float force, float angle, float time)
    {
        //x = x0 + vo.t
        var x = basePosition.transform.position.x + force * Mathf.Cos(angle) * time;
        //y = y0 + v0.sen(o)t + gt^2 / 2
        var y = basePosition.transform.position.y + force * Mathf.Sin(angle) * time + Physics2D.gravity.y * time * time/2;
        return new Vector2(x, y);
    }
    private bool CanLaunch(Walls currentWall, Vector2 mousePosition)
        => currentWall == Walls.left ? GetAngle(Vector2.up, mousePosition) > 0 : GetAngle(Vector2.down, mousePosition) > 0
            && slimeBehaviour.isActiveAndEnabled;

    private float GetAngle(Vector2 direction, Vector2 mousePosition)
        => Vector2.SignedAngle(direction, (Vector2)slimeBehaviour.transform.position - mousePosition);
}
