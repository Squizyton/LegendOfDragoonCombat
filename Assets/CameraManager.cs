using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   public static CameraManager instance;

   [SerializeField]private List<CinemachineVirtualCamera> cameras;
   [SerializeField] private CinemachineVirtualCamera currentCamera;
   [SerializeField]private CinemachineVirtualCamera battleCamera;
   private int cameraIndex = 0;
   void Awake()
   {
      instance = this;
   }


   public void ChangeEnvironmentCamera()
   {
      currentCamera.Priority = 0;
      
      cameraIndex = (cameraIndex + 1) % cameras.Count;
      currentCamera.Priority = 41;
   }

   //Zoom in on Character Attacking
   public void ZoomInOnCharacter(CharacterController player)
   {
      battleCamera.Follow = player.transform;
      battleCamera.LookAt = player.transform;
      battleCamera.ForceCameraPosition(player.transform.position + new Vector3(-1, 0, 0),
         battleCamera.transform.rotation);
   }
   
}
