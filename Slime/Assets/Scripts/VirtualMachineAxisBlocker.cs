using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")]
public class VirtualMachineAxisBlocker : CinemachineExtension
{
    [Tooltip("Lock the camera's X position to this value")]
    public float originalPosition;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.x = originalPosition;
            state.RawPosition = pos;
        }
    }
}
