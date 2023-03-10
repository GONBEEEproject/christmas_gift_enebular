using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private string presentTag = "present";

    // Update is called once per frame  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //マウスクリックした場所からRayを飛ばし、オブジェクトがあればtrue 
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == presentTag)
                {
                    hit.collider.GetComponent<GiftCommentPopup>().OnClick();
                }
            }
        }
    }
}
