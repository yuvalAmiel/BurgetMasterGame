using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressBox : MonoBehaviour
{
    public Ease easeType;
    [SerializeField]
    private Image fillBG;
    public Canvas myCanvas;
    [SerializeField]
    private Slider mSlider;
    public SettingsMenu LevelUpMenu;

    public int level = 0;
    private float CurrAmount = 0;
    private Coroutine routine, StartRoutine;
    public CoinCollection m_CoinCondition;
    public Transform targetLocation;

    Animator GiftAnimation, StarsAnimation;

    public bool tempBoolForAnim = false, animFinished = false, updatingLevel = false;
    void Start()
    {
        level = 0;
        CurrAmount = 0;
        //fillBG.fillAmount = CurrAmount;
        mSlider.value = CurrAmount;
        GiftAnimation = LevelUpMenu.transform.GetChild(1).GetComponent<Animator>();
        StarsAnimation = LevelUpMenu.transform.GetChild(0).GetComponent<Animator>();
    }

    public void UpdateProgress(float amount, float duration = 0.1f)
    {
        if (routine != null)
            StopCoroutine(routine);
        float target = CurrAmount + amount;
        if (amount == -1f)
            target = 0;
        if (target < 0)
        {
            target = 0;
            return;
        }
        routine = StartCoroutine(FillRoutine(target, duration));
    }

    private IEnumerator FillRoutine(float target, float duration)
    {
        float time = 0;
        float tempAmount = CurrAmount;
        float diff = target - tempAmount;
        CurrAmount = target;

        while (time < duration)
        {
            time += Time.deltaTime;
            float precent = time / duration;
            mSlider.value = tempAmount + diff * precent;
            yield return null;
        }
        mSlider.value = target;
        Debug.Log(mSlider.value);
     //   if (CurrAmount >= 1)
     //       LevelUp();

    }
    
    void Update()
    {
        if (GiftAnimation.GetBool("ScreenPressed") &&
            GiftAnimation.GetCurrentAnimatorStateInfo(0).IsName("KnifeBoardToWorldAnim") && !tempBoolForAnim &&
            level == 1)
        {
            tempBoolForAnim = true;
            Debug.Log("in csdfds");
            Transform KnifeBoard = LevelUpMenu.transform.GetChild(1);

            Vector3 targetPos = Camera.main.WorldToScreenPoint(new Vector3
                (targetLocation.position.x, targetLocation.position.y, targetLocation.position.z));

            KnifeBoard.DOScale(new Vector3(3, 2, 1), 2.5f);
            KnifeBoard.transform.DOMove(targetPos, 2f)
                .SetEase(easeType)
                .OnComplete(() => {
                    tempBoolForAnim = false;
                    animFinished = true;
                    SetBoardKnifeInGame();
                    KnifeBoard.gameObject.SetActive(false);

                });
        }
        if (mSlider.value >= 1 && !m_CoinCondition.inCoinMovement && !updatingLevel)
        {
            LevelUp();
        }

        /* if (mSlider.value >= 1 && !m_CoinCondition.inCoinMovement && !tempBoolForAnim && !updatingLevel)
         {
             Debug.Log("from update");
             LevelUp();
         }*/
    }

    private void SetBoardKnifeInGame()
    {
        GiftAnimation.SetBool("LevelUpPanel", false);
        GiftAnimation.SetBool("ScreenPressed", false);
        //Time.timeScale = 1f;
        LevelUpMenu.gameObject.SetActive(false);
        Image tempImage = LevelUpMenu.GetComponent<Image>();
        tempImage.enabled = true;
        targetLocation.GetComponent<emptyContainer>().enabled = false;
        targetLocation.GetChild(0).gameObject.SetActive(true);
        Debug.Log("in board func");
        updatingLevel = false;
    }

    public void LevelUp()
    {
        updatingLevel = true;
        LevelUpMenu.transform.GetComponent<Image>().enabled = false; // מכבה תמונה
        LevelUpMenu.gameObject.SetActive(true); // מדליק את האבא
        StarsAnimation.SetBool("ReachProgress", true); //מריץ כוכבים
    }

    public float GetSliderValue()
    {
        return mSlider.value;
    }

    public void UpdateMyLevel()
    {
        Debug.Log("in updateee level");
        StarsAnimation.SetBool("ReachProgress", false);
        LevelUpMenu.transform.GetComponent<Image>().enabled = true;

        LevelUpMenu.PauseNShowMenu();
        level++;
        Debug.Log("level is " + level);

        if (level == 1) // knifeBoard Animation
        {
            Transform child = LevelUpMenu.transform.GetChild(1);
            child.gameObject.SetActive(true);
            GiftAnimation.SetBool("LevelUpPanel", true);
            GiftAnimation.SetInteger("Level", level);
            updatingLevel = true;
            Debug.Log("in if");
        }
        UpdateProgress(-1f, 0.2f);
    }

}
