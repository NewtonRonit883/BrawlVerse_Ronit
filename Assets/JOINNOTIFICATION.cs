using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        notificationText.text = $"{name} has joined the game!";
        notificationPanel.gameObject.SetActive(true);
        StartCoroutine(HideNotificationAfterDelay(3f)); // Hide after 3 seconds

    }
    private IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationPanel.gameObject.SetActive(false);
    }
}
