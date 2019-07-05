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
    private const int gizmosPreviewQuantity = 4;

    [SerializeField] private SlimeBehaviour slimeBehaviour;
    [SerializeField] private Transform basePosition;
    [SerializeField] private Transform GizmosParent;
    [SerializeField] private GameObject previewGizmoPrefab;
    [SerializeField] private float baseForce;

    private LaunchState currentLaunchState;
    private Vector2 initialClickPosition;
    private List<GameObject> previewGizmos;

    private void Awake()
    {
        Assert.IsNotNull(slimeBehaviour);
        Assert.IsNotNull(basePosition);
        Assert.IsNotNull(previewGizmoPrefab);
        initialClickPosition = Vector2.zero;
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
        if(currentLaunchState == LaunchState.NotClicked)
        {
            currentLaunchState = LaunchState.Holding;
            initialClickPosition = Input.mousePosition;
            UpdateGizmoState();
        }
    }
    public void OnPointerUp()
    {
        currentLaunchState = LaunchState.NotClicked;
        UpdateGizmoState();
        slimeBehaviour.LaunchSlime(GetForce() * (initialClickPosition - (Vector2)Input.mousePosition).normalized);
    }

    private void Update()
    {
        UpdateGizmosPosition();
    }

    private void UpdateGizmosPosition()
    {
        if(currentLaunchState == LaunchState.Holding)
        {
            var force = GetForce();
            var angle = Vector2.SignedAngle(Vector2.left, (Vector2)Input.mousePosition - initialClickPosition) * Mathf.Deg2Rad;
            Assert.IsTrue(angle < Mathf.PI * 2);
            for(var i = 0; i < gizmosPreviewQuantity; i++)
            {
                var gizmo = previewGizmos[i];
                gizmo.transform.position = GetGizmoPosition(force, angle, (i+1) * 0.25f);
            }
        }
    }
    private void UpdateGizmoState()
    {
        foreach(var gizmo in previewGizmos)
            gizmo.SetActive(currentLaunchState == LaunchState.Holding);
    }
    private float GetForce() => baseForce + Vector2.Distance(Input.mousePosition, initialClickPosition)/50f;
    private Vector2 GetGizmoPosition(float force, float angle, float time)
    {
        //x = x0 + vo.t
        var x = basePosition.transform.position.x + force * Mathf.Cos(angle) * time;
        //y = y0 + v0.sen(o)t.gt^2 / 2
        var y = basePosition.transform.position.y + force * Mathf.Sin(angle) * time + Physics2D.gravity.y * time * time/2;
        return new Vector2(x, y);
    }
}
