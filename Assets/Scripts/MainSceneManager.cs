using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] AudioClip clickSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReturnToMenu()
    {
        GameManager.CharacterSpeed = 0;
        Time.timeScale = 1;
        audioSource.PlayOneShot(clickSound);
        Thread.Sleep(100);
        SceneManager.LoadScene(0);
    }

}
