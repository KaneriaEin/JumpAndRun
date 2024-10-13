using Assets.Scripts.Manager;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook cameraFreeLook;
    private CinemachineImpulseSource impulse;

    private void Awake()
    {
        CameraManager.Instance.cameraController = this;
        impulse = cameraFreeLook.GetComponent<CinemachineImpulseSource>();
    }

    public void ScreenShake()
    {
        impulse.GenerateImpulse(Vector3.right);
    }
}
