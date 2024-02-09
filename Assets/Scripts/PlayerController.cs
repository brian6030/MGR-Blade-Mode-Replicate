using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;

    [SerializeField] GameObject sword;
    [SerializeField] GameObject swordOnShoulder;

    [SerializeField] KeyCode equipSwordKey = KeyCode.R;
    [SerializeField] KeyCode blockKey = KeyCode.Mouse1;
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;

    BladeModeController bladeModeController;

    public bool isEquipping;
    public bool isEquipped;
    public bool isBlocking;
    public bool isAttacking;
    float timeSinceAttack;
    int currentAttack = 0;

    void Start()
    {
        bladeModeController = GetComponent<BladeModeController>();
    }

    public void Update()
    {
        timeSinceAttack += Time.deltaTime;

        Attack();

        Equip();
        Block();
    }

    void Equip() 
    {
        if(Input.GetKeyUp(equipSwordKey) && playerAnimator.GetBool("Grounded")) 
        {
            isEquipping = true;
            playerAnimator.SetTrigger("Equip");
        }
    }

    public void ActiveWeapon() 
    {
        if (!isEquipped)
        {
            sword.SetActive(true);
            swordOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
        }
        else 
        {
            sword.SetActive(false);
            swordOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
        }
    }

    public void Equipped() 
    {
        isEquipping = false;
    }

    void Block()
    {
        if (Input.GetKey(blockKey) && playerAnimator.GetBool("Grounded") && !bladeModeController.IsBladeMode)
        {
            playerAnimator.SetBool("Block", true);
            isBlocking = true;
        }
        else 
        {
            playerAnimator.SetBool("Block", false);
            isBlocking = false;
        }
    }

    void Attack() 
    {
        if (Input.GetKeyDown(attackKey) && playerAnimator.GetBool("Grounded") && timeSinceAttack > 0.8f) 
        {
            if (!isEquipped || bladeModeController.IsBladeMode)
            {
                return;
            }

            currentAttack++;
            isAttacking = true;

            if (currentAttack > 3)
            {
                currentAttack = 1;
            }

            // Reset
            if (timeSinceAttack > 1.0f)
            {
                currentAttack = 1;
            }

            playerAnimator.SetTrigger("Attack" + currentAttack);

            // Reset timer
            timeSinceAttack = 0.0f;
        }

    }

    public void ResetAttack() 
    {
        isAttacking = false;
    }
}
