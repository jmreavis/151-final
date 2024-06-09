using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    int time = 350;
    int coinCount;
    private void Start()
    {
        coinCount = GameObject.FindGameObjectsWithTag("Coin").Length;
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/sequence_time", time);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music", 0);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music_vol", 0);
        Debug.Log("Total coins in level: " + coinCount.ToString());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount--;
            Debug.Log("Remaining coins: " + coinCount.ToString());
            time -= 80;
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/sequence_time", time);

            other.gameObject.SetActive(false);
        }

        if (coinCount == 0)
        {
         //   OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music", 1);//call the function and turn on music audio
           // OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music_vol", 0.5f);//call the function and turn on music audio
            //OSCHandler.Instance.SendMessageToClient("pd", "/unity/sequence_vol", 0);
            StartCoroutine(Song_Length());

        }
    }

    private IEnumerator Song_Length()
    {
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music", 1);//call the function and turn on music audio
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music_vol", 0.5f);//call the function and turn on music audio
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/sequence_vol", 0);
        yield return new WaitForSeconds(10f);
        OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music", 0);
        yield break;//call the function and turn on music audio
       // OSCHandler.Instance.SendMessageToClient("pd", "/unity/victory_music_vol", 0.5f);//call the function and turn on music audio
       // OSCHandler.Instance.SendMessageToClient("pd", "/unity/sequence_vol", 0);

    }
}
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Coin")) {
            OSCHandler.Instance.SendMessageToClient("pd", "/unity/test1", 0);
            other.gameObject.SetActive(false);

        }
    }
}
*/