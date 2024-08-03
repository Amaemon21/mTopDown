using UnityEngine;

public class UIRootVeiw : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;

    private void Awake()
    {
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        _loadingScreen.gameObject.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        _loadingScreen.gameObject.SetActive(false);
    }
}