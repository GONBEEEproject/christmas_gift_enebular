using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GiftCommentPopup : MonoBehaviour
{
    [SerializeField]
    private Transform lookCanvas;

    [SerializeField]
    private TMP_Text nameText, commentText;

    [SerializeField]
    private GameObject tip;

    private void Start()
    {
        tip.SetActive(false);
    }

    private void Update()
    {
        Vector3 p = Camera.main.transform.position;
        p.y = transform.position.y;
        lookCanvas.transform.LookAt(p);
        lookCanvas.transform.Rotate(0, 180, 0);
    }

    public void Initialize(string name, string comment)
    {
        nameText.text = name;
        commentText.text = comment;
    }

    public void OnClick()
    {
        tip.SetActive(false);
        StartCoroutine(ClickSequence());
    }

    private IEnumerator ClickSequence()
    {
        tip.SetActive(true);
        AudioManager.Instance.PlayPopi();

        yield return new WaitForSeconds(5.0f);

        tip.SetActive(false);
    }
}