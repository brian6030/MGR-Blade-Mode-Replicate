using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeModeController : MonoBehaviour
{
    public bool IsBladeMode = false;
    [SerializeField] KeyCode BladeModeKey = KeyCode.LeftControl;
    [SerializeField] GameObject CuttingQuad;

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
        }
        else
        {
            IsBladeMode = false;
            CuttingQuad.SetActive(false);
        }

        animator.SetBool("BladeMode", IsBladeMode);
    }
}
