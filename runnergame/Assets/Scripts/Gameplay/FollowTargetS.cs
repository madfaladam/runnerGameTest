using UnityEngine;

public class FollowTargetS : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    private void LateUpdate()
    {
        if (!GameM.Instance.isStart)
        {
            return;
        }
        if (target == null)
        {
            return;
        }

        transform.position = new Vector3(target.transform.position.x + offset.x, 0f, 0f);
    }
}
