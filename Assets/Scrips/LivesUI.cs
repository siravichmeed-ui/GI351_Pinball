
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Image[] ballIcons;

    public void UpdateLives(int remaining)
    {
        for (int i = 0; i < ballIcons.Length; i++)
        {
            ballIcons[i].enabled = i < remaining;
        }
    }
}
