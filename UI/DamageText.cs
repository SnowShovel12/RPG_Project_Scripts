using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        StartCoroutine(DisableWithDelay());
    }

    public void Set(string text)
    {
        _textMeshPro.text = text;
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }

    IEnumerator DisableWithDelay()
    {
        yield return new WaitForSeconds(0.5f);

        transform.SetParent(GameManager.Instance.textPoolManager.transform);
        gameObject.SetActive(false);
    }
}
