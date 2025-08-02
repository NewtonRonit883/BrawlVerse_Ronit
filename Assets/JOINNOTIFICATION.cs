using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
public class JOINNOTIFICATION : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform notificationPanel;
    public TextMeshProUGUI notificationText;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void ShowNotification(string name)
    {
        GetComponent<PhotonView>().RPC("ShowNotificationRPC", RpcTarget.AllBuffered, name);

    }
    [PunRPC]
    public void ShowNotificationRPC(string name)
    {
        notificationText.text = $"{name} has joined the game!";
        notificationPanel.gameObject.SetActive(true);
        FindObjectOfType<SoundManager>().JoinSound(); // Play join sound
        StartCoroutine(HideNotificationAfterDelay(3f)); // Hide after 3 seconds
    }
    public void LeftNotification(string name)
    {
        GetComponent<PhotonView>().RPC("LeftNotificationRPC", RpcTarget.AllBuffered, name);

    }
    [PunRPC]
    public void LeftNotificationRPC(string name)
    {
        notificationText.text = $"{name} has left the game!";
        notificationPanel.gameObject.SetActive(true);
        FindObjectOfType<SoundManager>().JoinSound(); // Play join sound
        StartCoroutine(HideNotificationAfterDelay(3f)); // Hide after 3 seconds
    }
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationPanel.gameObject.SetActive(false);
    }
}
