using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaunchState
{
    NotClicked,
    Holding,
    Flying,
}


public class SlimeLauncherBehaviour : MonoBehaviour
{
    private const int gizmosPreviewQuantity = 4;

    [SerializeField] private Transform basePosition;
    [SerializeField] private GameObject previewGizmoPrefab;

    private LaunchState currentLaunchState;
    private Vector2 initialClickPosition;
    private List<GameObject> previewGizmos;

    private void Awake()
    {
        initialClickPosition = Vector2.zero;
        previewGizmos = new List<GameObject>();
        for(var i = 0; i < gizmosPreviewQuantity; i++)
        {
            var gizmo = Instantiate(previewGizmoPrefab);
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
        }
    }
    public void OnPointerUp() => currentLaunchState = LaunchState.NotClicked;

    private void Update()
    {
        if(currentLaunchState == LaunchState.Holding)
        {
            UpdateGizmosPosition();
        }
        //Debug.Log((Vector2)Input.mousePosition - initialClickPosition);
    }

    private void UpdateGizmosPosition()
    {
        var force = Vector2.Distance(Input.mousePosition, initialClickPosition)/50f;]
        ///TODO, identificar se esta no segundo ou terceiro qudrante
        var angle = Vector2.Angle(Vector2.left, (Vector2)Input.mousePosition - initialClickPosition) * Mathf.Deg2Rad;
        Debug.Log("Angulo: " + angle * Mathf.Rad2Deg + " graus");
        for(var i = 0; i < gizmosPreviewQuantity; i++)
        {
            var gizmo = previewGizmos[i];
            gizmo.SetActive(true);
            gizmo.transform.position = GetGizmoPosition(force, angle, i * 0.25f);
        }
    }
    private Vector2 GetGizmoPosition(float force, float angle, float time)
    {
        //x = x0 + vo.t
        var x = basePosition.transform.position.x + force * Mathf.Cos(angle) * time;
        //y = y0 + v0.sen(o)t.gt^2 / 2
        var y = basePosition.transform.position.y + force * Mathf.Sin(angle) * time * Physics2D.gravity.y * time * time/2;
        return new Vector2(x, y);
    }
}
