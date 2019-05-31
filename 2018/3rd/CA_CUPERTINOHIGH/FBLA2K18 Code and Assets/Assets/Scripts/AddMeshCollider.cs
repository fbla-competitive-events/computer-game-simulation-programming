using UnityEngine;

public class AddMeshCollider : MonoBehaviour
{
    void Start()
    {
        Mesh m = null;
        var mf = GetComponent<MeshFilter>();
        if (mf != null)
            m = mf.sharedMesh;
        var smr = GetComponent<SkinnedMeshRenderer>();
        if (smr != null)
            m = smr.sharedMesh;
        if (m != null)
        {
            var col = GetComponent<MeshCollider>();
            if (col == null)
                col = gameObject.AddComponent<MeshCollider>();
            col.sharedMesh = m;
        }
    }
}
