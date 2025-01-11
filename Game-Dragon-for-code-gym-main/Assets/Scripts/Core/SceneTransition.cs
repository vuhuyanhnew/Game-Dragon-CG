using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f; 
    [SerializeField] private Animator fadeAnimator; 

    private bool isTransitioning = false;

    [SerializeField] private string nextSceneName = "Level2"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("Level2"); 
        }
    }
    private IEnumerator TransitionToNextScene()
    {
        isTransitioning = true;

        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("FadeOut");
        }

        yield return new WaitForSeconds(fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isTransitioning = false;
    }
}