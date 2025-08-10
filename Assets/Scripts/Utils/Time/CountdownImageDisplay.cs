using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CountdownImageDisplay : MonoBehaviour
{
    [SerializeField] private TimeController timer;
    [SerializeField] private Image countdownImage;

    [Header("Sprites personalizados")]
    public Sprite sprite3;
    public Sprite sprite2;
    public Sprite sprite1;
    public Sprite spriteGo;

    private int lastTime = -1;

    private void Start()
    {
        timer.eventTimeModified += OnTimeChanged;
        countdownImage.enabled = true;
    }

    private void OnTimeChanged(float time)
    {
        int roundedTime = Mathf.CeilToInt(time);

        if (roundedTime == lastTime)
            return;

        lastTime = roundedTime;

        switch (roundedTime)
        {
            case 3:
                SetImage(sprite3);
                break;
            case 2:
                SetImage(sprite2);
                break;
            case 1:
                SetImage(sprite1);
                break;
            case 0:
                SetImage(spriteGo);
                Invoke(nameof(HideImage), 1f);
                break;
            default:
                countdownImage.enabled = false;
                break;
        }
    }
    void SetImage(Sprite sprite)
    {
        countdownImage.enabled = true;
        countdownImage.sprite = sprite;
        countdownImage.SetNativeSize();

        countdownImage.transform.localScale = Vector3.zero;
        countdownImage.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
    }

    void HideImage()
    {
        UIManager.instance.ShowTimerPanel(false);
    }
}

