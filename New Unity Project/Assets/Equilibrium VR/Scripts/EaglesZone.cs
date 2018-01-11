using UnityEngine;
using System.Collections;

////////////Using for fish 4th gamemode. Eagle bots fly and caught fish when they entered the trigger

public class EaglesZone : MonoBehaviour {
     
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            if (other.gameObject.name == "EagleBotHolder_1")
            {
                Camera.main.gameObject.GetComponent<GameLogic>().BotsCounts[0] += 1;
            }
            if (other.gameObject.name == "EagleBotHolder_2")
            {
                Camera.main.gameObject.GetComponent<GameLogic>().BotsCounts[1] += 1;
            }
        }
    }
}
