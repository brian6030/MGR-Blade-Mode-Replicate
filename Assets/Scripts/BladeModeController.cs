using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class BladeModeController : MonoBehaviour
{
    public bool IsBladeMode = false;
    [SerializeField] KeyCode BladeModeKey = KeyCode.LeftControl;
    [SerializeField] GameObject CuttingQuad;

    [SerializeField] CinemachineVirtualCamera bladeModeCam;
    [Range(0, 1)][SerializeField] float bladeModeTimeScale;
    [SerializeField] float bladeModeSensitivity = 0.3f;
    float normalSensitivity;
    [SerializeField] float turnSpeed = 50;

    PlayerController playerController;
    ThirdPersonController thirdPersonController;
    Animator animator;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();

        normalSensitivity = thirdPersonController.Sensitivity;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(BladeModeKey))
        { // Blade Mode
            if (IsBladeMode || !playerController.isEquipped)
                return;

            IsBladeMode = true;
            CuttingQuad.SetActive(true);

            bladeModeCam.enabled = true;
            thirdPersonController.SetSensitivity(bladeModeSensitivity);

            Time.timeScale = bladeModeTimeScale;
        }
        else
        { // Exit Blade Mode
            IsBladeMode = false;
            CuttingQuad.SetActive(false);

            bladeModeCam.enabled = false;
            thirdPersonController.SetSensitivity(normalSensitivity);

            Time.timeScale = 1.0f;
        }

        animator.SetBool("BladeMode", IsBladeMode);

    }

    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
