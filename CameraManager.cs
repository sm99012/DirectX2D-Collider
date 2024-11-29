using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager S_CameraManager;
    public static CameraManager I_CameraManager
    {
        get
        {
            if (S_CameraManager == null)
                return null;
            else
                return S_CameraManager;
        }
    }

    [SerializeField] Vector3 m_vCameraPos_Origin;
    [SerializeField] Vector3 m_vCameraPos_Current;

    private void Awake()
    {
        if (S_CameraManager == null)
        { 
            S_CameraManager = this; DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(S_CameraManager);
        }

        m_vCameraPos_Origin = new Vector3(0.3f, 1.15f, -1.3f);
        m_vCameraPos_Current = new Vector3(0.3f, 1.15f, -1.3f);
    }

    public void Update_Camera_Transform_x(float fangle)
    {
        
    }

    [SerializeField] float m_fAngle;     // 카메라 기울기(x 각)
    [SerializeField] float m_fSin_Value; // sin(카메라 기울기) - y
    [SerializeField] float m_fCos_Value; // cos(카메라 기울기) - z
    public void Update_Camera_Transform_z(float fangle)
    {
        if (this.gameObject.transform.localRotation.x <= Quaternion.Euler(Vector3.right * 70).x && this.gameObject.transform.localRotation.x >= Quaternion.Euler(Vector3.left * 70).x)
        {
            m_fAngle = 360 - this.gameObject.transform.localRotation.eulerAngles.x;
            m_fSin_Value = (float)Math.Sin(m_fAngle * Mathf.Deg2Rad); // 호도법 Degree to Radian
            m_fCos_Value = (float)Math.Cos(m_fAngle * Mathf.Deg2Rad); // 호도법 Degree to Radian
            m_vCameraPos_Current.z = (-1.3f * m_fCos_Value);
            m_vCameraPos_Current.y = (-1.3f * m_fSin_Value) + 1.15f;
            this.gameObject.transform.localPosition = m_vCameraPos_Current;

            this.gameObject.transform.Rotate(Vector3.right * (fangle / 360));

            if (this.gameObject.transform.localRotation.x > Quaternion.Euler(Vector3.right * 70f).x)
                this.gameObject.transform.localRotation = Quaternion.Euler(Vector3.right * 70);
            if (this.gameObject.transform.localRotation.x < Quaternion.Euler(Vector3.left * 70f).x)
                this.gameObject.transform.localRotation = Quaternion.Euler(Vector3.left * 70);
        }
    }
}
