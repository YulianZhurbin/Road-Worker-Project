using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecordSceneManager : MonoBehaviour
{
    [SerializeField] Text recordText;
    [SerializeField] AudioClip clickSound;

    GameObject player;
    Animator playerAnim;  
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        playerAnim = player.GetComponent<Animator>();
        playerAnim.SetFloat("Speed_f", 0);
        playerAnim.SetInteger("Animation_int", 1);
        player.GetComponent<Rigidbody>().isKinematic = true;

        Storage.instance.LoadRecord();

        RecordContainer recordContainer = Storage.instance.RecordContainer;

        if (recordContainer != null)
        {
            recordText.text = $"{recordContainer.name}:  {recordContainer.score}";
        }
    }

    public void ReturnToMenu()
    {
        audioSource.PlayOneShot(clickSound);
        Thread.Sleep(100);
        SceneManager.LoadScene(0);
    }
}