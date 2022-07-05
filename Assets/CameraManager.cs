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
   private int cameraIndex = -1;
   void Awake()
   {
      instance = this;
   }


   public void ChangeEnvironmentCamera()
   {
      currentCamera.Priority = 0;
      
      cameraIndex = (cameraIndex + 1) % cameras.Count;
      currentCamera = cameras[cameraIndex];
      currentCamera.Priority = 41;
   }

   //Zoom in on Character Attacking
   public void ZoomInOnCharacter(CharacterController player)
   {
      battleCamera.Priority = 50;
     battleCamera.Follow = player.transform;
     //battleCamera.LookAt = player.transform;
     battleCamera.transform.position = player.transform.position;
   }

   public void CombatEnd()
   {
      battleCamera.Priority = 0;
   }

}
