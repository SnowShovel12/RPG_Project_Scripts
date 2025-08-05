using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance { get; private set; }

    [SerializeField] 
    private Transform buttonParent;
    [SerializeField] 
    private GameObject interactButtonPrefab;

    private Dictionary<IInteractable, GameObject> _interactButtons = new Dictionary<IInteractable, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddInteractButton(IInteractable interactable)
    {
        if (_interactButtons.ContainsKey(interactable)) return;

        GameObject buttonGO = Instantiate(interactButtonPrefab, buttonParent);
        Button button = buttonGO.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = interactable.InteractText;

        button.onClick.AddListener(() => interactable.Interact());

        _interactButtons.Add(interactable, buttonGO);
    }

    public void RemoveInteractButton(IInteractable interactable)
    {
        if (!_interactButtons.ContainsKey(interactable)) return;

        Destroy(_interactButtons[interactable]);
        _interactButtons.Remove(interactable);
    }
}
