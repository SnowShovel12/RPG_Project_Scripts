using UnityEditor.Rendering;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemSO itemObject;
    [SerializeField]
    private float delay = 0.5f;
    public bool CanPickUp => delay <= 0;

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = itemObject.icon;
    }

    private void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
    }
}
