using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CinemachinePOV povComponent;
    private CinemachinePOV PovComponent
    {
        get
        {
            if (povComponent == null)
            {
                povComponent = CameraManager.Instance.GetCamera(ECameraType.Home)
                    .GetCinemachineComponent<CinemachinePOV>();
            }

            return povComponent;
        }
    }

    private void OnEnable()
    {
        PovComponent.m_VerticalAxis.m_InputAxisName = "";
        PovComponent.m_HorizontalAxis.m_InputAxisName = "";
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            PovComponent.m_HorizontalAxis.Value += Input.GetAxis("Mouse X");
            PovComponent.m_VerticalAxis.Value -= Input.GetAxis("Mouse Y");
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
