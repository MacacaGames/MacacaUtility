using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SnackBar : MonoBehaviour
{
    static SnackBar _instance;
    public static SnackBar Instance
    {
        get
        {
            if (_instance == null)
            {
                var a = Resources.Load<GameObject>("SnackBar");
                GameObject Canvas = GameObject.Find("Canvas");
				var temp = Instantiate(a,Canvas.transform);

                temp.transform.SetParent(Canvas.transform, false);
                temp.transform.position = new Vector3(0, -40, 0);
                _instance = temp.GetComponent<SnackBar>();
                _instance.rectTransform = temp.GetComponent<RectTransform>();
                temp.SetActive(false);
            }
            return _instance;
        }
    }
    RectTransform rectTransform;
    [SerializeField]
    Text Message;
    [SerializeField]
    Text Button;
    // Use this for initialization
    void OnEnable()
    {
        
    }

    public void Show(string msg,string btn)
    {
        if (hideTweener != null) hideTweener.Kill();
        gameObject.SetActive(true);
        rectTransform.DOAnchorPosY(0, 0.3f);
        timer = 0;
		Message.text = msg;
		Button.text = btn;
    }
    Tweener hideTweener;
    public void Hide()
    {
        hideTweener = rectTransform.DOAnchorPosY(-40, 0.3f).OnComplete(
            () =>
            {
                gameObject.SetActive(false);
            }
        );
    }
    float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            Hide();
        }
    }

    public delegate void Callback();
    // - Mission
    public event Callback BtnClickEvent;
    public void OnButtonClick()
    {
        if (BtnClickEvent != null) BtnClickEvent();
    }
}
