using UnityEngine;
using UnityEngine.Rendering;

public class Ysort : MonoBehaviour
{
    private SortingGroup sortingGroup;

    void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    void LateUpdate()
    {
        sortingGroup.sortingOrder = -(int)(transform.position.y * 100);
    }
}