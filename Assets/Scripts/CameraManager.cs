using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   public static CameraManager instance;

   [SerializeField]private List<CinemachineVirtualCamera> cameras;
   [SerializeField] private CinemachineVirtualCamera currentCamera;
   public CinemachineVirtualCamera battleCamera;
   
   [Header("Camera Settings")]
   [SerializeField]private Vector3 battleCameraOffset;
   private int cameraIndex = -1;
   void Awake()
   {
      instance = this;
   }

public CinemachineVirtualCamera GetCurrentCamera()
   {
      return currentCamera;
   }
   
   
   public void ChangeEnvironmentCamera()
   {
      currentCamera.Priority = 0;
      
      cameraIndex = (cameraIndex + 1) % cameras.Count;
      currentCamera = cameras[cameraIndex];
      currentCamera.Priority = 41;
   }

   //Zoom in on Character Attacking
   public void ZoomInOnCharacter(PlayerController player)
   {
      battleCamera.Priority = 50;

      battleCamera.transform.parent = player.transform;
      
      //lerp the camera left of the player
      battleCamera.transform.localPosition = battleCameraOffset;
      
      //Rotate the camera to look at the player with offset
      battleCamera.transform.LookAt(player.transform.position);
   }

   public void CombatEnd()
   {
      battleCamera.Priority = 0;
      
      //reset parent
      battleCamera.transform.parent = null;
   }

}
