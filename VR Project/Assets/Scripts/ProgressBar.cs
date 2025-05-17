using UnityEngine;
using UnityEngine.UI;

public class ProgressBar: MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        barImage.fillAmount = 0;

        HideBar();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progress;

        if (e.progress == 0f || e.progress == 1f)
            HideBar();
        else
            ShowBar();
    }

    private void ShowBar()
    { 
        gameObject.SetActive(true);
    }

    private void HideBar()
    {
        gameObject.SetActive(false);
    }
}