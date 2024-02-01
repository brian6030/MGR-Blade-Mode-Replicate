using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

namespace StarterAssets
{
    public class FootIK : MonoBehaviour
    {
        Animator anim;
        StarterAssetsInputs starterAssetsInputs;
        ThirdPersonController thirdPersonController;
        
        public Transform bone;

        public Vector3 footIk_offset;
        [Range(0f, 1f)] public float ik_Weight;
        public float lerpSpeed;

        public float rayDistance;

        Vector3 leftFoot;
        Vector3 rightFoot;
        Vector3 l_Hit;
        Vector3 L_Normal;
        Vector3 R_Hit;
        Vector3 R_Normal;

        public float fallMin = 0.7f;
        public float fallMax = 0.9f;

        void Awake()
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            anim = GetComponent<Animator>();
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        }

        void Update()
        {
            // Calculate IK weight based on input.
            if (starterAssetsInputs.move != Vector2.zero || !thirdPersonController.Grounded)
            {
                ik_Weight = Mathf.Lerp(ik_Weight, 0f, Time.deltaTime * lerpSpeed);
            }
            else
            {
                ik_Weight = Mathf.Lerp(ik_Weight, 1, Time.deltaTime * lerpSpeed);
            }

            // Adjust bone position based on foot height difference for balancing.
            if (Mathf.Abs(leftFoot.y - rightFoot.y) < fallMin)
            {
                bone.transform.localPosition = new Vector3(0, -Mathf.Abs(leftFoot.y - rightFoot.y) * ik_Weight, 0);
            }
            else if(Mathf.Abs(leftFoot.y - rightFoot.y) >= fallMin && Mathf.Abs(leftFoot.y - rightFoot.y) < fallMax)
            {
                bone.transform.localPosition = new Vector3(0, -Mathf.Abs(leftFoot.y - rightFoot.y)/2 * ik_Weight, 0);
            }
        }

        void OnAnimatorIK(int layerIndex)
        {
            // Get positions of left and right feet.
            leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot).position;
            rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot).position;

            // Get hit information for left foot and adjust its position.
            GetHitInfo(leftFoot, ref l_Hit, ref L_Normal, anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position);
            GetHitInfo(rightFoot, ref R_Hit, ref R_Normal, anim.GetBoneTransform(HumanBodyBones.RightLowerLeg).position);

            leftFoot = l_Hit + footIk_offset;
            rightFoot = R_Hit + footIk_offset;

            // Set IK position and rotation for left foot.
            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ik_Weight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ik_Weight);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot);
            //anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, L_Normal));
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg).forward, L_Normal));


            // Calculate the difference between knee and foot height for balancing.
            float test = anim.GetIKHintPosition(AvatarIKHint.LeftKnee).y - anim.GetIKPosition(AvatarIKGoal.LeftFoot).y;

            // Set IK position and rotation for right foot.
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, ik_Weight);
            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, ik_Weight);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot);
            //anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, R_Normal));
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(anim.GetBoneTransform(HumanBodyBones.RightLowerLeg).forward, R_Normal));

        }

        // Function to get hit information for a raycast with a limit on vertical movement.
        void GetHitInfo(Vector3 origin, ref Vector3 point, ref Vector3 normal, Vector3 limit)
        {
            //Bottom Check
            if (Physics.Linecast(origin + Vector3.up, origin+Vector3.down * rayDistance, out RaycastHit hit))
            {
                if(hit.point.y < limit.y - 0.1f)
                {
                    point = hit.point;
                }
                normal = hit.normal;
            }
        }
    }

}
