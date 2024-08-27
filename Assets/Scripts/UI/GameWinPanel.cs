using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    public VisualElement rootEltment;
    private Button pickCardButton;
    private Button backToMapButton;

    [Header("广播事件")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;

    private void OnEnable()
    {
        Debug.Log("GameWinPanel OnEnable");
        rootEltment = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = rootEltment.Q<Button>("PickCardButton");
        backToMapButton = rootEltment.Q<Button>("BackToMapButton");

        backToMapButton.clicked += OnBackToMapButtonClicked;
        pickCardButton.clicked += OnPickCardButtonClicked;
    }

    private void OnPickCardButtonClicked()
    {
        Debug.Log("OnPickCardButtonClicked");
        pickCardEvent.RaiseEvent(null, this);
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaiseEvent(null, this);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardButton.style.display = DisplayStyle.None;
    }
}
