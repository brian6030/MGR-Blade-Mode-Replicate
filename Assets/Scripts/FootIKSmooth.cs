using UnityEngine;

public class FootIKSmooth : MonoBehaviour
{
    public bool IkActive = true;
    [Range(0f, 1f)]
    public float WeightPosition = 1f;
    [Range(0f, 1f)]
    public float WeightRotation = 0f;

    Animator anim;
    [Tooltip("Offset for Foot position")]
    public Vector3 offsetFoot;
    [Tooltip("Layer where foot can adjust to surface")]
    public LayerMask RayMask;
    [SerializeField] float maxRayDistance = 1.2f;

    [Header("DEBUG")]
    //This line can be delete
    public bool DebugEnable = true;
    public Transform FootRight = null;
    public Transform FootLeft = null;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    RaycastHit hit;

    void OnAnimatorIK(int _layerIndex)
    {
        if(IkActive)
        {
            Vector3 FootPos;

			FootPos = anim.GetIKPosition(AvatarIKGoal.RightFoot); //get current foot position (After animation apply)
            SetFootIK(FootPos, AvatarIKGoal.RightFoot, FootRight);

            FootPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot); //get current foot position
            SetFootIK(FootPos, AvatarIKGoal.LeftFoot, FootLeft);
        }
        else //IK is turn off, we not set anything
        {
            SetWeights(AvatarIKGoal.LeftFoot);
            SetWeights(AvatarIKGoal.RightFoot);
        }
    }

    void DrawDebugLines(RaycastHit hit, Transform foot, Color color1, Color color2)
    {
        Debug.DrawLine(hit.point, Vector3.ProjectOnPlane(hit.normal, foot.right), color1);
        Debug.DrawLine(foot.position, foot.position + foot.right, color2);
    }

    void SetWeights(AvatarIKGoal foot, float positionWeight = 0f, float rotationWeight = 0f) 
    {
        anim.SetIKPositionWeight(foot, positionWeight);
        anim.SetIKRotationWeight(foot, rotationWeight);
    }

    void SetFootIK(Vector3 footPos, AvatarIKGoal foot, Transform footTransform) 
    {
        if (Physics.Raycast(footPos + Vector3.up, Vector3.down, out hit, maxRayDistance, RayMask)) //Throw raycast to down
        {
            SetWeights(foot, WeightPosition, WeightRotation);
            anim.SetIKPosition(foot, hit.point + offsetFoot);

            // Draw Debug
            if (DebugEnable)
            {
                DrawDebugLines(hit, footTransform, Color.blue, Color.yellow);
            }

            if (WeightRotation > 0f) //adjust foot if is enable
            {
                //Little formula to calculate foot rotation (This can be better)
                Quaternion footRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hit.normal), hit.normal);
                anim.SetIKRotation(foot, footRotation);
            }
        }
        else //Raycast does not hit anything, so we keep original position and rotation
        {
            SetWeights(foot);
        }
    }
}
