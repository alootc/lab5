using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private float shake_intensity = 1f;
    private float shake_time = 0.2f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        StopShake();

        Player.onTakeDamage += ShakeCamera;
    }

    private void OnDestroy()
    {
        Player.onTakeDamage -= ShakeCamera;
    }

    public void ShakeCamera(float damage)
    {

        if(damage < 0)
        {
            CinemachineBasicMultiChannelPerlin cbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmcp.m_AmplitudeGain = shake_intensity;

            timer = shake_time * Mathf.Abs(damage);
        }
        
    }

    private void StopShake()
    { 
        CinemachineBasicMultiChannelPerlin cbmcp = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 0f;

        timer = 0;

    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                StopShake();
            }
        }
    }
}
