using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private float ShakeIntensity = 1f;

    private CinemachineVirtualCamera m_VCam;

    private void Awake()
    {
        m_VCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        CameraShakeStop();
    }

    public void CameraShakeStart(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = m_VCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = intensity;
    }

    public void CameraShakeStop()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = m_VCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0f;
    }
}
