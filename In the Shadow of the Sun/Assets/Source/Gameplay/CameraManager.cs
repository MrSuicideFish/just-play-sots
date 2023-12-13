using Cinemachine;
using UnityEngine;

public enum ECameraType : int
{
    Intro,
    Home,
    Newspaper,
    Lawsuit,
    Phone,
    Count = 5
}

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
            }

            return _instance;
        }
    }
    
    public CinemachineVirtualCamera introCamera;
    public CinemachineVirtualCamera homeCamera;
    public CinemachineVirtualCamera newspaperCamera;
    public CinemachineVirtualCamera lawsuitCamera;
    public CinemachineVirtualCamera phoneCamera;

    public void GoToCamera(ECameraType cameraType)
    {
        introCamera.gameObject.SetActive(cameraType == ECameraType.Intro);
        homeCamera.gameObject.SetActive(cameraType == ECameraType.Home);
        newspaperCamera.gameObject.SetActive(cameraType == ECameraType.Newspaper);
        lawsuitCamera.gameObject.SetActive(cameraType == ECameraType.Lawsuit);
        phoneCamera.gameObject.SetActive(cameraType == ECameraType.Phone);
    }

    public CinemachineVirtualCamera GetCamera(ECameraType cameraType)
    {
        switch (cameraType)
        {
            case ECameraType.Intro:
                return introCamera;
            case ECameraType.Home:
                return homeCamera;
            case ECameraType.Newspaper:
                return newspaperCamera;
            case ECameraType.Lawsuit:
                return lawsuitCamera;
            case ECameraType.Phone:
                return phoneCamera;
        }

        return null;
    }
}