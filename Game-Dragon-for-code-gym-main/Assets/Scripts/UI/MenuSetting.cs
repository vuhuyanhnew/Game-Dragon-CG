using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSetting : MonoBehaviour
{
    [Header("Menu Elements")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private AudioClip buttonClickSound;

    private void Awake()
    {
        mainMenuScreen.SetActive(true);

        Time.timeScale = 1f;
        Debug.Log("Loading Menu...");
    }

    #region Menu Functions
    public void StartGame()
    {
        if (buttonClickSound != null)
            SoundManager.instance.PlaySound(buttonClickSound);

        Debug.Log("Loading Level1...");
        // Load the Level1 scene using the correct path
        SceneManager.LoadScene("Levels/Level1");
        Debug.Log("Level1 loaded!");
        
    }
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
       
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

    public void Quit()
    {
        if (buttonClickSound != null)
            SoundManager.instance.PlaySound(buttonClickSound);

        Application.Quit(); 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
    }
    #endregion
}