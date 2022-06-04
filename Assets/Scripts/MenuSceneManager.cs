using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuSceneManager : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] AudioClip clickSound;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnEndEdit()
    {
        Storage.instance.storedName = inputField.text;
    }

    public void StartNew()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene(1);
    }

    public void ShowRecords()
    {
        audioSource.PlayOneShot(clickSound);
        Thread.Sleep(100);
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        audioSource.PlayOneShot(clickSound);
        Thread.Sleep(100);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}