using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreView : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Store store;

    private void Start()
    {
        inputField.onSubmit.AddListener(OnSubmit);
    }

    private void OnSubmit(string value)
    {
        Card card = store.GetCard();
        card.ChangePrice(string.IsNullOrEmpty(value) ? 0 : float.Parse(value));
    }
}
