using UnityEngine;
using GameSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] PlayerCam playerCam;

    void Start()
    {
        playerCam.Start();
    }

    void LateUpdate()
    {
        playerCam.RefreshPosition(transform.position);
     }
}
