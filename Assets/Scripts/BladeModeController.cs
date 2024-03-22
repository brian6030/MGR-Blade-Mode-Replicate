using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BladeModeController : MonoBehaviour
{
    public bool IsBladeMode = false;
    [SerializeField] KeyCode BladeModeKey = KeyCode.LeftControl;
    [SerializeField] GameObject CuttingQuad;

    [SerializeField] CinemachineVirtualCamera bladeModeCam;
    [Range(0, 1)][SerializeField] float bladeModeTimeScale;

    PlayerController playerController;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(BladeModeKey))
        {
            if (IsBladeMode || !playerController.isEquipped)
                return;

            IsBladeMode = true;
            CuttingQuad.SetActive(true);
            bladeModeCam.enabled = true;
            Time.timeScale = bladeModeTimeScale;
        }
        else
        {
            IsBladeMode = false;
            CuttingQuad.SetActive(false);
            bladeModeCam.enabled = false;
            Time.timeScale = 1.0f;
        }

        animator.SetBool("BladeMode", IsBladeMode);
    }
}
