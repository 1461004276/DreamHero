using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class FadePanel : MonoBehaviour
{
    private VisualElement background;

    private void Awake()
    {
        background = GetComponent<VisualElement>().Q<VisualElement>("Background");
    }

    public void FadeIn(float duration)
    {
        DOVirtual.Float(0, 1, duration, value =>
        {
            background.style.opacity = value;
        }).SetEase(Ease.InOutCirc);
    }

    public void FadeOut(float duration)
    {
        DOVirtual.Float(1, 0, duration, value =>
        {
            background.style.opacity = value;
        }).SetEase(Ease.InOutCirc);
    }
}
