using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GetInsideOnTrigger : MonoBehaviour
{
    public Transform front;
    public Transform back;
    public SortingGroup sortingGroup;
    public List<Transform> unwantedChildren;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            if (sr.sortingLayerID == sortingGroup.sortingLayerID && sr.sortingOrder == sortingGroup.sortingOrder)
            {
                var ct = collision.gameObject.transform;
                if (ct.parent == null)
                {
                    ct.SetParent(transform);
                    unwantedChildren.Add(ct);
                    back.SetSiblingIndex(0);
                    ct.SetSiblingIndex(1);
                    front.SetSiblingIndex(2);
                }
            }
        }
    }

    void ReleaseUnwantedChildren()
    {
        foreach (var uc in unwantedChildren)
        {
            uc.SetParent(null);
        }
        unwantedChildren.Clear();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ReleaseUnwantedChildren();
    }

    private void OnDestroy()
    {
        ReleaseUnwantedChildren();
    }
}
