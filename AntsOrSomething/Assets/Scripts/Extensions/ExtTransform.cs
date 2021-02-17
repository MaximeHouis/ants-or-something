using UnityEngine;

namespace ExtensionsTransform
{
    public static class UnityTransform
    {
        [ContextMenu("Round")]
        public static void Round(this Transform transform)
        {
            var pos = transform.position;
            var rot = transform.eulerAngles;
            var sca = transform.localScale;

            transform.position =
                new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
            transform.rotation = Quaternion.Euler(
                new Vector3(Mathf.RoundToInt(rot.x), Mathf.RoundToInt(rot.y), Mathf.RoundToInt(rot.z)));
            transform.localScale =
                new Vector3(Mathf.RoundToInt(sca.x), Mathf.RoundToInt(sca.y), Mathf.RoundToInt(sca.z));
        }
    }
}