using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using TMPro;

using Random = UnityEngine.Random;

public class EnebularManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField nameText, commentText;

    [SerializeField]
    private GameObject[] giftPrefab;

    [SerializeField]
    private Transform areaStart, areaEnd, loadSpin;

    private void Start()
    {
        StartCoroutine(DownloadSeq());
    }

    private IEnumerator DownloadSeq()
    {
        string result;
        using (var request = UnityWebRequest.Get("https://lcdp003.enebular.com/download/"))
        {
            request.SetRequestHeader("Access-Control-Allow-Credentials", "true");
            request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
            request.SetRequestHeader("Access-Control-Allow-Credentials", "GET, POST, OPTIONS");
            request.SetRequestHeader("Access-Control-Allow-Origin", "*");


            yield return request.SendWebRequest();
            result = request.downloadHandler.text;
            result = result.Remove(0, 1);
            result = result.Remove(result.Length - 1, 1);

            string[] splited = result.Split(",");

            for (int i = 0; i < splited.Length; i += 2)
            {
                string s1 = splited[i];
                s1 = s1.Replace("\"", "");
                string s2 = splited[i + 1];
                s2 = s2.Replace("\"", "");
                MakeGift(s1, s2);
                yield return new WaitForSeconds(0.2f);
            }
        }

        loadSpin.gameObject.SetActive(false);
    }

    public void MakeGift(string name, string comment)
    {
        GameObject g = Instantiate(giftPrefab[Random.Range(0, giftPrefab.Length)]);
        Vector3 p = new Vector3(
                Random.Range(areaStart.position.x, areaEnd.position.x),
                Random.Range(areaStart.position.y, areaEnd.position.y),
                Random.Range(areaStart.position.z, areaEnd.position.z));

        g.transform.position = p;

        g.GetComponent<GiftCommentPopup>().Initialize(name, comment);
    }

    public void UploadComment()
    {
        StartCoroutine(UploadSeq());
    }

    private IEnumerator UploadSeq()
    {
        loadSpin.gameObject.SetActive(true);

        string s1 = nameText.text;
        string s2 = commentText.text;


        if (s1 == "")
        {
            s1 = " ";
        }

        if (s2 == "")
        {
            s2 = " ";
        }

        string json = $"[\"{s1}\",\"{s2}\"]";
        Debug.Log(json);

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        var request = new UnityWebRequest("https://lcdp003.enebular.com/upload/", "POST");
        request.SetRequestHeader("Access-Control-Allow-Credentials", "true");
        request.SetRequestHeader("Access-Control-Allow-Headers", "Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        request.SetRequestHeader("Access-Control-Allow-Credentials", "GET, POST, OPTIONS");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return request.SendWebRequest();

        MakeGift(s1, s2);
        AudioManager.Instance.PlayPin();
        loadSpin.gameObject.SetActive(false);
    }
}