using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_NotificationQueue : MonoBehaviour {

    public static SCR_NotificationQueue NotificationPolice;
    public static SCR_NotificationQueue NotificationTruck;

    public bool IsPolice = true;

    public float DisplayDuration = 5f;
    public float FadeDuration = 0.15f;
    public Image BackgroundImage;

    private Text Text;
    private List<string> _textQueue = new List<string>();
    private float _lerpStart= 0f;
    private bool _bIsLerping = false;
    private float _startAplha = 0f;
    private float _endAlpha = 1f;
    private float _timeElapsed;
    private bool _bIsDisplayingText;

    // To prevent bug where notification got send twice
    private float _timeBetweenNotifications = 0.0f;
    private float _timeSinceNotification = 0.0f;

    void Awake()
    {
        if (IsPolice)
        {
          if (NotificationPolice == null)
          {
               NotificationPolice = this;
          }
          else if (NotificationPolice != this)
          {
              Destroy(gameObject);
          }
        }
        else
        {
            if (NotificationTruck == null)
            {
                NotificationTruck = this;
            }
            else if (NotificationTruck != this)
            {
                Destroy(gameObject);
            }
        }
    }

        // Use this for initialization
        void Start () {
        Text = GetComponentInChildren<Text>();

        Color bgColor = BackgroundImage.color;
        BackgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f);
        Color textColor = Text.color;
        Text.color = new Color(textColor.r, textColor.g, textColor.b, 0f);

        NotificationTruck.DisplayText("Come get my donuts");
        NotificationPolice.DisplayText("Suspect found im on route");

    }

    // Update is called once per frame
    void Update () {

        // temp
        _timeSinceNotification += Time.deltaTime;

        if (_textQueue.Count > 0 && !_bIsDisplayingText )
        {
            _bIsDisplayingText = true;
            Text.text = _textQueue[0];
            StartCoroutine("FadeInAndOut");
        }

        if (_bIsLerping)
        {
            FadeText();
            FadeBGImage();
        }
	}

    public void DisplayText(string textToDisplay, float displayDuration = 3f, float fadeDuration = 0.15f)
    {
       //if (_timeSinceNotification > _timeBetweenNotifications)
       //{
            DisplayDuration = displayDuration;
            FadeDuration = fadeDuration;
            _textQueue.Add(textToDisplay);
            _timeSinceNotification = 0.0f;
       // }
    }

    IEnumerator FadeInAndOut()
    {
        StartFadeIn();
        yield return new WaitForSeconds(DisplayDuration - FadeDuration);
        StartFadeOut();
        yield return new WaitForSeconds(FadeDuration);
        _textQueue.RemoveAt(0);
        _bIsDisplayingText = false;
    }

    void StartFadeIn()
    {
        Debug.Log("StartFadeIn");
        _startAplha = 0f;
        _endAlpha = 1f;
        _lerpStart = Time.time;
        _bIsLerping = true;
    }

    void StartFadeOut()
    {
        Debug.Log("StartFadeOut");
        _startAplha = 1f;
        _endAlpha = 0f;
        _lerpStart = Time.time;
        _bIsLerping = true;
    }

    void FadeText()
    {
        Color textColor = Text.color;

        float timeSinceStarted = Time.time - _lerpStart;
        float percentageComplete = timeSinceStarted / FadeDuration;

        float a = Mathf.Lerp(_startAplha, _endAlpha, percentageComplete);

        if (percentageComplete >= 1)
            _bIsLerping = false;    

        Text.color = new Color(textColor.r, textColor.g, textColor.b, a);
    }

    void FadeBGImage()
    {
        Color bgColor = BackgroundImage.color;

        float timeSinceStarted = Time.time - _lerpStart;
        float percentageComplete = timeSinceStarted / FadeDuration;

        float a = Mathf.Lerp(_startAplha, _endAlpha, percentageComplete);

        if (percentageComplete >= 1)
            _bIsLerping = false;


         BackgroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, a);
    }


}
