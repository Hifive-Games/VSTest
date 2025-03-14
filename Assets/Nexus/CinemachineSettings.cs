using UnityEngine;
using Cinemachine;

public class CinemachineSettings : MonoBehaviour
{
    //get player from the scene and set the follow and look at camera
    private void Start()
    {
        Invoke("SetCamera", 0.1f);
    }

    private void SetCamera()
    {
        Transform player = FindObjectOfType<CharacterController>().transform;
        CinemachineVirtualCamera vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = player;
        vcam.LookAt = player;
    }
}
