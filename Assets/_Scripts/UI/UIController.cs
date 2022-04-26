using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [HideInInspector]
    public bool stepCounterActive = false;
    [HideInInspector]
    public bool canShowArrows = true;
    private AudioSource BGM;
    private AudioSource playerSoundEffect;
    private bool IsBgmMute = false;
    private float BGMVolume;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject PauseMene;
    [SerializeField] private GameObject cwArrow;
    [SerializeField] private GameObject ccwArrow;
    [SerializeField] private GameObject homeButton;
    [SerializeField] private GameObject restartButton;

    [Header("InGamePanel")]
    [SerializeField] private GameObject InGamePanel;
    [SerializeField] private RawImage _star1;
    [SerializeField] private RawImage _star2;
    [SerializeField] private RawImage _star3;
    [SerializeField] private TMP_Text StepCountText;
    [SerializeField] private Texture _filledStar;
    [SerializeField] private Texture _emptyStar;

    public GameObject NextButton;
    private int _stepCount = 0;
    private float pauseStart;
    private float pauseDuration;
    private Animation leftAnim;
    private Animation rightAnim;
    private bool arrowTutorial = false;

    //music
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _arrowClip;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _pauseClip;
    [SerializeField] private AudioClip _homeButtonClip;
    [SerializeField] private AudioClip _nextButtonClip;
    [SerializeField] private AudioClip _restartButtonClip;
    private float _initAudioVolumn;

    public AudioClip[] music;
    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;
    [SerializeField] private Sprite _sfxOn;
    [SerializeField] private Sprite _sfxOff;
    [SerializeField] private Image _musicImage;
    [SerializeField] private Image _sfxImage;

    public int StepCount
    {
        get => _stepCount;
        set
        {
            _stepCount = value;
            int starStepNumber = 0;
            if (MapTransition.instance.CurrentLevel != 0)
            {
                levelStar curLevelData = GameManager.instance.levelData.levelStarData[MapTransition.instance.CurrentLevel - 1];
                if (_stepCount <= curLevelData.ThreeStarStep)
                {
                    starStepNumber = curLevelData.ThreeStarStep;
                    UpdateStar(3);
                }
                else if (_stepCount == curLevelData.ThreeStarStep + 1)
                {
                    starStepNumber = curLevelData.TwoStarStep;
                    UpdateStar(2);
                }
                else if (_stepCount <= curLevelData.TwoStarStep)
                {
                    starStepNumber = curLevelData.TwoStarStep;
                }
                else if (_stepCount == curLevelData.TwoStarStep + 1)
                {
                    starStepNumber = curLevelData.OneStarStep;
                    UpdateStar(1);
                }
                /*else if (_stepCount <= curLevelData.OneStarStep)
                {
                    starStepNumber = curLevelData.OneStarStep;
                }
                else if (_stepCount == curLevelData.OneStarStep + 1)
                {
                    starStepNumber = curLevelData.OneStarStep;
                    UpdateStar(0);
                }*/
                else
                {
                    starStepNumber = curLevelData.OneStarStep;
                }
            }

            StepCountText.text = "Steps: " + _stepCount.ToString() + "/" + starStepNumber.ToString();
        }
    }

    private void UpdateStar(int starCount)
    {
        if (starCount == 3)
        {
            _star1.texture = _filledStar;
            _star2.texture = _filledStar;
            _star3.texture = _filledStar;
        }
        else if (starCount == 2)
        {
            _star1.texture = _filledStar;
            _star2.texture = _filledStar;
            _star3.texture = _emptyStar;
        }
        else if (starCount == 1)
        {
            _star1.texture = _filledStar;
            _star2.texture = _emptyStar;
            _star3.texture = _emptyStar;
        }
        else if (starCount == 0)
        {
            _star1.texture = _emptyStar;
            _star2.texture = _emptyStar;
            _star3.texture = _emptyStar;
        }
    }

    private void Start()
    {
        GameObject GameManager = GameObject.Find("GameManager");
        BGM = GameManager.GetComponent<AudioSource>();
        // Disable user control
        //MapTransition.instance.DisableController();

        // Reset UI
        CloseMenu();
        //Title.SetActive(true);
        //StartMenu.SetActive(true);
        InGamePanel.SetActive(true);
        StepCount = 0;

        // Enable step counter
        stepCounterActive = true;

        leftAnim = cwArrow.GetComponent<Animation>();
        rightAnim = ccwArrow.GetComponent<Animation>();

        _initAudioVolumn = _audioSource.volume;
        BGMVolume = BGM.volume;
    }
    

    private void CloseMenu()
    {
        Title.SetActive(false);
        StartMenu.SetActive(false);
        WinPanel.SetActive(false);
        PauseMene.SetActive(false);
    }

    
    public void WinUI()
    {
        WinPanel.SetActive(true);
        _audioSource.PlayOneShot(_winClip);
        // disable step counter
        stepCounterActive = false;
        // Send analytic event for steps

        // //End Tutorial if available
        // if (TutorialManager.current != null) {
        //     TutorialManager.current.EndPositionEnter();
        // }
    }

    public void StartButtonPressed()
    {
        CloseMenu();
        InGamePanel.SetActive(true);
        
        //MapTransition.instance.LevelTransition();
        GameManager.instance.CurrentState = GameManager.GameState.playing;
        MapTransition.instance.EnableController();
        GameManager.instance.startTime = Time.time;

        // //Show Tutorial if available
        // if (TutorialManager.current != null) {
        //     TutorialManager.current.StartPositionEnter();
        // }
    }

    public void HomeButtonPressed()
    {
        _audioSource.PlayOneShot(_homeButtonClip);
        CloseMenu();
        print("fsfsfss"+IsBgmMute);
        if (!IsBgmMute)
        {
            BGM.clip = music[0];
            BGM.Play();
        }
        InGamePanel.SetActive(true);
        MapTransition.instance.SelectLevel();
    }

    public void NextButtonPressed()
    {
        _audioSource.PlayOneShot(_nextButtonClip);
        CloseMenu();
        BGM.clip = music[0];
        BGM.Play();
        MapTransition.instance.LevelTransition();
    }
    public void RestartButtonPressed()
    {
        _audioSource.PlayOneShot(_restartButtonClip);
        CloseMenu();
        if (!IsBgmMute)
        {
            BGM.clip = music[0];
            BGM.Play();
        }
        stepCounterActive = true;
        InGamePanel.SetActive(true);
        // if player did not complete the level, record analytics data
        if ( ! GameManager.instance.IsLevelCompleted() )
        {
            GameManager.instance.SendLevelAnalytics("restartLevel");
        }
        
        GameManager.instance.CurrentState = GameManager.GameState.restart;
        MapTransition.instance.RestartLevel();
    }

    public void PauseMenuClicked()
    {
        if (WinPanel.activeInHierarchy)
            return;

        if (PauseMene.activeInHierarchy)
        {
            _audioSource.PlayOneShot(_pauseClip);
            PauseMene.SetActive(false);
            InGamePanel.SetActive(true);
            GameManager.instance.CurrentState = GameManager.GameState.starting;
            pauseDuration = Time.time - pauseStart;
            GameManager.instance.totalPauseDuration += pauseDuration;
        }
        else
        {
            _audioSource.PlayOneShot(_pauseClip);
            homeButton.SetActive(true);
            restartButton.SetActive(true);
            if (MapTransition.instance.CurrentLevel == 0)
            {
                homeButton.SetActive(false);
                restartButton.SetActive(false);
            }
            PauseMene.SetActive(true);
            InGamePanel.SetActive(false);
            GameManager.instance.CurrentState = GameManager.GameState.paused;
            pauseStart = Time.time;
        }
    }

    public void AddStep()
    {
        if (stepCounterActive)
        {
            StepCount++;
        }
        
    }

    public int GetStep()
    {
        return StepCount;
    }

    public void RotateCW()
    {
        if (MapTransition.instance.mouseRotation != null)
        {
            MapTransition.instance.mouseRotation.RotateMapCW();
            _audioSource.PlayOneShot(_arrowClip);
        }
    }

    public void RotateCCW()
    {
        if (MapTransition.instance.mouseRotation != null)
        {
            MapTransition.instance.mouseRotation.RotateMapCCW();
            _audioSource.PlayOneShot(_arrowClip);
        }
    }

    public void StartArrowAnim()
    {

        leftAnim.Play();
        rightAnim.Play();
    }
   
    public void StopArrowAnim()
    {
        leftAnim.Stop();
        rightAnim.Stop();
        ccwArrow.GetComponent<Image>().color = Color.white;
        cwArrow.GetComponent<Image>().color = Color.white;
        ccwArrow.transform.localScale = new Vector3(1, 1, 1);
        cwArrow.transform.localScale = new Vector3(1, 1, 1);
    }
    public void HideRotateArrow()
    {
        cwArrow.SetActive(false);
        ccwArrow.SetActive(false);
        canShowArrows = false;
    }
    public void ShowRotateArrow()
    {
        if (!canShowArrows)
            return;
        cwArrow.SetActive(true);
        ccwArrow.SetActive(true);
        canShowArrows = true;
    }
    public void muteBGM()
    {
        
        if (BGM.volume != 0)
        {
            BGM.Stop();
            BGM.volume = 0f;
            _musicImage.sprite = _musicOff;
            IsBgmMute = true;
        }
        else
        {
            BGM.Play();
            BGM.volume = BGMVolume;
            _musicImage.sprite = _musicOn;
            IsBgmMute = false;
        }
    }
    public void muteSoundEffect()
    {
        GameObject Player = GameObject.Find("PlayerCube_OneColor");
        playerSoundEffect = Player.GetComponent<AudioSource>();
        if (_audioSource.volume != 0)
        {
            _audioSource.volume = 0f;
            playerSoundEffect.enabled = false;

            _sfxImage.sprite = _sfxOff;
        }
        else
        {
            _audioSource.volume = _initAudioVolumn;
            playerSoundEffect.enabled = true;
            _sfxImage.sprite = _sfxOn;
        }
    }
    // add more methods to track other stats
}
