using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SCR_FlashUI : MonoBehaviour {

    public static SCR_FlashUI FlashNitroUIPolice;
    public static SCR_FlashUI FlashNitroUITruck;

    public bool IsPolice;

    public float FlashDuration;
    public Color LerpToColor;

    private Color _endColor;            // 
    private Color _defaultColor;        // normal color -> Flashout: _endColor = _defaultColor;
    private Color _startColor;          // start lerp from here
    private float _flashTime;
    private Image _image;
    private bool _bIsLerping;
    private float _lerpStartTime;
    private float _startPercentageCompleted;    // if flash was still flashing -> start from correct value
    private float _startTime;
    private bool _bFlashingIn = false;
    private bool _bFlashingOut = false;

    void Awake()
    {
        if (IsPolice)
        {
            if (FlashNitroUIPolice == null)
            {
                FlashNitroUIPolice = this;
            }
            else if (FlashNitroUIPolice != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (FlashNitroUITruck == null)
            {
                FlashNitroUITruck = this;
            }
            else if (FlashNitroUITruck != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Use this for initialization
    void Start () {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
        Debug.Log("Default color: " + _defaultColor);
	}
	
	// Update is called once per frame
	void Update () {

        if (_bFlashingIn || _bFlashingOut)
        {
            FlashLerp();
        }

	}

    private void FlashIn()
    {
        Debug.Log("FlashIn");
        _startColor = _image.color;
        _endColor = LerpToColor;
        _lerpStartTime = Time.time;
        float timeSinceStarted = Time.time - _lerpStartTime;
        float percentageComplete = timeSinceStarted / FlashDuration + _startTime;
        _bFlashingIn = true;
    }
    private void FlashOut()
    {
        _startColor = LerpToColor;
        _endColor = _defaultColor;
        _lerpStartTime = Time.time;
        _startTime = 0f;
        _bFlashingOut = true;
    }

    public void FlashImage()
    {
        _startColor = _image.color;
        float percentageComplete = 1 - (LerpToColor.r - _startColor.r) / (LerpToColor.r - _defaultColor.r) ;
        float _startTime = percentageComplete * (FlashDuration);
        _flashTime = FlashDuration - _startTime;
        StartCoroutine("FlashInAndOut");
    }

    IEnumerator FlashInAndOut()
    {
        _bFlashingOut = false;
        FlashIn();
        yield return new WaitForSeconds(_flashTime);
        _bFlashingIn = false;
        FlashOut();
    }

    void FlashLerp()
    {
        Color currentColor = _image.color;

        float timeSinceStarted = Time.time - _lerpStartTime + _startTime;
        float percentageComplete = timeSinceStarted / FlashDuration + _startTime;

        currentColor = Color.Lerp(_startColor, _endColor, percentageComplete);

        if (percentageComplete >= 1 && _bFlashingOut)
        {
            _bFlashingOut = false;
        }
        
        _image.color = currentColor;
    }
}
