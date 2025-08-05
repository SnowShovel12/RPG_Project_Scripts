using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SkillButton : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform background;
    private RectTransform handle;
    public Image cooltimeCycle;
    public TextMeshProUGUI cooltimeText;

    public int attackIndex;

    [SerializeField]
    private PlayerAttackController attackController;

    private float pressStartTime;
    public float pressThreshold = 0.1f;
    private bool isHold = false;

    public Vector2 InputVector { get; private set; }

    private void Start()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
        handle.gameObject.SetActive(false);
        cooltimeText.enabled = false;
    }

    private void Update()
    {
        if (attackController.GetCurrentCooltime(attackIndex) >= 0)
        {
            if (cooltimeCycle != null)
            {
                cooltimeCycle.fillAmount = attackController.GetCooltimeRatio(attackIndex);
            }
            if (cooltimeText != null)
            {
                cooltimeText.text = attackController.GetCurrentCooltime(attackIndex).ToString("F1");
                if (attackController.GetCurrentCooltime(attackIndex) == 0)
                {
                    cooltimeText.enabled = false;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isHold) return;

        handle.gameObject.SetActive(true);
        Vector2 touchPosition = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            touchPosition = Vector2.ClampMagnitude(touchPosition, background.sizeDelta.x / 2);
            handle.anchoredPosition = touchPosition;

            InputVector = touchPosition / (background.sizeDelta.x / 2);

            attackController.UpdateAttackDirection(new Vector3(InputVector.x, 0, InputVector.y));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressStartTime = Time.time;
        isHold = false;
        StartCoroutine(CheckLongPress(eventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        handle.gameObject.SetActive(false);
        attackController.RemoveAttackDirection();

        if (isHold)
        {
            attackController.ExecuteAttack(attackIndex, new Vector3(InputVector.x, 0, InputVector.y));
            cooltimeText.enabled = true;
        }
        else
        {
            attackController.ExecuteAttack(attackIndex);
            cooltimeText.enabled = true;
        }
        handle.anchoredPosition = Vector2.zero;
        InputVector = Vector2.zero;
    }

    private IEnumerator CheckLongPress(PointerEventData eventData)
    {
        yield return new WaitForSeconds(pressThreshold);

        isHold = true;
        attackController.DrawAttackDirection(attackIndex);

        OnDrag(eventData);
    }
}
