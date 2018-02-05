using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class moveplayer : MonoBehaviour
{

#if UNITY_EDITOR
    public KeyCode moveL;
    public KeyCode moveR;

#endif
    public float wantedX = 0, horiDiff = 3;
    int[] rows = new int[] { -1, 0, 1 };
    public int currRow, wantedRow;
    public static float moveSpeed = 8.5f;
    public GameObject Placeholder, Shield;
    public float horiDistance = 3;
    public bool centeringZero = true;
    Rigidbody rigid;
    public static int coinCollected;
    public Text CoinText;
    public bool BlinkActive = false, Unstoppable = false;
    public Image PwrBlinkDuration, PwrUnstoppableDuration;
    public bool StartTimeElapseBlink = false, StartTimeElapseUnstoppable = false, GameEnd = false, FlyingAway = false;
    public float timeEnabledBlink, timeEnabledUnstoppable;
    public float TimeDuration = 5;
    public GameObject Particle, AudioManager;
    ParticleSystem exp;
    public AudioClip AsteroidSmash, CoinPickUp, BlinkTransition;
    AudioSource ASource;
    public static float DistanceTravled = 0;
    Vector3 StartPostion, EndPosition;
    public static bool StartGame = false;
    public static Animator Anim;
    public static int segmentPart = 0;
    // Use this for initialization
    void Start()
    {
        currRow = rows[1];
        rigid = GetComponent<Rigidbody>();
        wantedRow = 0;
        centeringZero = true;
        coinCollected = 0;
        exp = Particle.GetComponent<ParticleSystem>();
        ASource = AudioManager.GetComponent<AudioSource>();
        StartPostion = transform.localPosition;
        Anim = GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        StartGame = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (StartGame)
        {
            if (!GameEnd)
            {
#if UNITY_EDITOR

                if (Input.GetKeyDown(moveL) && wantedRow > -1)
                {
                    centeringZero = false;
                    wantedRow--;
                }
                if (Input.GetKeyDown(moveR) && wantedRow < 1)
                {
                    centeringZero = false;
                    wantedRow++;
                }
#endif
                //Debug.Log("Wanted row: " + wantedRow);
                if (wantedRow == -1)
                    wantedX = -horiDiff;
                else if (wantedRow == 0)
                    wantedX = 0;
                else if (wantedRow == 1)
                    wantedX = horiDiff;

                if (!BlinkActive)
                {
                    if (centeringZero)
                    {
                        if (!Unstoppable)
                            transform.Translate(new Vector3(0, 0, moveSpeed) * Time.deltaTime);
                        else
                            transform.Translate(new Vector3(0, 0, moveSpeed) * Time.deltaTime);
                        //Debug.Log("0, 0, moveSpeed");
                    }
                    else if (transform.localPosition.x < wantedX && !centeringZero)
                    {
                        if (!Unstoppable)
                            transform.Translate(new Vector3(horiDistance, 0, moveSpeed) * Time.deltaTime);
                        else
                            transform.Translate(new Vector3(horiDistance, 0, moveSpeed) * Time.deltaTime);
                        //Debug.Log("translate 3,0,movespeed");
                        if (wantedRow == 0)
                        {
                            if (transform.localPosition.x > -0.1f && transform.localPosition.x < 0.1f && !centeringZero)
                            {
                                Vector3 fixX = transform.localPosition;
                                fixX.x = 0;
                                transform.localPosition = fixX;
                                currRow = 0;
                                centeringZero = true;
                               // Debug.Log("centering 0");
                            }
                        }
                        if (wantedRow == 1)
                        {
                            if (transform.localPosition.x < horiDiff + 0.1f && transform.localPosition.x > horiDiff - 0.1f && !centeringZero)
                            {
                                Vector3 fixX = transform.localPosition;
                                fixX.x = horiDiff;
                                transform.localPosition = fixX;
                                currRow = 1;
                                centeringZero = true;
                               // Debug.Log("centering " + horiDiff);
                            }
                        }

                    }
                    else if (transform.localPosition.x > wantedX && !centeringZero)
                    {
                        if (!Unstoppable)
                            transform.Translate(new Vector3(-horiDistance, 0, moveSpeed) * Time.deltaTime);
                        else
                            transform.Translate(new Vector3(-horiDistance, 0, moveSpeed) * Time.deltaTime);
                        //Debug.Log("translate -3,0,movespeed");
                        if (wantedRow == 0)
                        {
                            if (transform.localPosition.x > -0.1f && transform.localPosition.x < 0.1f && !centeringZero)
                            {
                                Vector3 fixX = transform.localPosition;
                                fixX.x = 0;
                                transform.localPosition = fixX;
                                currRow = 0;
                                centeringZero = true;
                                //Debug.Log("centering " + 0);
                            }
                        }
                        if (wantedRow == -1)
                        {
                            if (transform.localPosition.x > -horiDiff - 0.1f && transform.localPosition.x < -horiDiff + 0.1f && !centeringZero)
                            {
                                Vector3 fixX = transform.localPosition;
                                fixX.x = -horiDiff;
                                transform.localPosition = fixX;
                                currRow = -1;
                                centeringZero = true;
                                //Debug.Log("centering " + -horiDiff);
                            }
                        }
                    }
                }
                else
                {
                    // ASource.clip = BlinkTransition;
                    if (!Unstoppable)
                        transform.Translate(new Vector3(0, 0, moveSpeed) * Time.deltaTime);
                    else
                        transform.Translate(new Vector3(0, 0, moveSpeed) * Time.deltaTime);
                    if (wantedRow == -1 && !centeringZero)
                    {
                        StartCoroutine(BlinkEffect(-horiDiff));
                        centeringZero = true;
                    }
                    else if (wantedRow == 0 && !centeringZero)
                    {

                        StartCoroutine(BlinkEffect(0));
                        centeringZero = true;
                    }
                    else if (wantedRow == 1 && !centeringZero)
                    {
                        StartCoroutine(BlinkEffect(horiDiff));
                        centeringZero = true;
                    }
                }
            }
        }
    }
    public IEnumerator BlinkEffect(float dif)
    {
        transform.localScale = new Vector3(0, 0, 0);
        Vector3 setPos = transform.localPosition;
        setPos.x = dif;
        transform.localPosition = setPos;
        exp.Play();
        ASource.PlayOneShot(BlinkTransition);
        yield return new WaitForSeconds(0.2f);
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "segmentCreateNew")
        {
            segmentPart++;
            Placeholder.GetComponent<PolygonGeneratorDE>().CreateNewSegment(segmentPart);
            if (moveSpeed < 21.5f)
            {
                moveSpeed++;
                movecamera.moveSpeed++;
            }
        }
        if (other.tag == "lethal")
        {
            if (!Unstoppable)
            {
                // Time.timeScale = 0;
                GameEnd = true;
                FlyingAway = true;
                movecamera.moveSpeed = 0;
                StartCoroutine(GameOver());
               // StartCoroutine(newGame());
            }
            else
            {
                ASource.PlayOneShot(AsteroidSmash);
                foreach (Transform t in other.gameObject.GetComponentInChildren<Transform>())
                {
                    //Debug.Log(t.gameObject.name);
                    t.gameObject.GetComponent<asteriodScript>().Explode = true;
                }
            }
        }
        if (other.tag == "coin")
        {


            Destroy(other.gameObject);
            coinCollected++;
            ASource.PlayOneShot(CoinPickUp);
            CoinText.text = "Coin collected: " + coinCollected;
        }
        if (other.tag == "pwrShield")
        {
            UnstoppableCoroutine();
            Destroy(other.gameObject);
        }
        if (other.tag == "pwrBlink")
        {
            StartBlinkCoroutine();
            Destroy(other.gameObject);
        }
       
    }
    public void MoveLeft()
    {
        if (wantedRow > -1)
        {
            centeringZero = false;
            wantedRow--;
        }
    }
    public void MoveRight()
    {
        if (wantedRow < 1)
        {
            centeringZero = false;
            wantedRow++;
        }
    }
    IEnumerator newGame()
    {
        yield return new WaitForSecondsRealtime(1f);
        Vector3 startPos = transform.localPosition;
        startPos.z = 0;
        startPos.x = 0;
        transform.localPosition = startPos;
        Time.timeScale = 1;
    }
    private void FixedUpdate()
    {
        if (StartTimeElapseBlink && BlinkActive)
        {
            PwrBlinkDuration.fillAmount = (timeEnabledBlink - Time.time) / (TimeDuration);
        }
        if (StartTimeElapseUnstoppable && Unstoppable)
        {
            PwrUnstoppableDuration.fillAmount = (timeEnabledUnstoppable - Time.time) / (TimeDuration);
        }
        if(FlyingAway)
        {
            rigid.AddRelativeForce(new Vector3(0.1f, 0.3f, 0.5f) * 1.5f, ForceMode.Impulse);
            transform.Rotate(1f, 1f, 1f, Space.Self);
        }

    }
    public void UnstoppableCoroutine()
    {
        PwrUnstoppableDuration.transform.parent.gameObject.SetActive(true);
        StartCoroutine(UnstoppableDuration());
    }
    public void StartBlinkCoroutine()
    {
        PwrBlinkDuration.transform.parent.gameObject.SetActive(true);
        StartCoroutine(BlinkDuration());
    }
    public IEnumerator GameOver()
    {
        EndPosition = transform.localPosition;
        DistanceTravled = Vector3.Distance(StartPostion, EndPosition);
        yield return new WaitForSeconds(2);
        FlyingAway = false;
        var r = GetComponent<Rigidbody>();
        r.isKinematic = true;
        SceneManager.LoadScene("GameOver",LoadSceneMode.Single);

    }
    public IEnumerator BlinkDuration()
    {

        StartTimeElapseBlink = true;
        BlinkActive = true;
        timeEnabledBlink = Time.time + TimeDuration;
        //Debug.Log((timeEnabledBlink - Time.time) / (TimeDuration));
        while (Time.time < timeEnabledBlink)
            yield return null;
        StartTimeElapseBlink = false;
        BlinkActive = false;
        PwrBlinkDuration.transform.parent.gameObject.SetActive(false);
    }
    public IEnumerator UnstoppableDuration()
    {
        StartTimeElapseUnstoppable = true;
        Unstoppable = true;
        timeEnabledUnstoppable = Time.time + TimeDuration;
        movecamera.moveSpeed = moveSpeed;
        //Debug.Log((timeEnabledUnstoppable - Time.time) / (TimeDuration));
        while (Time.time < timeEnabledUnstoppable)
            yield return null;
        StartTimeElapseUnstoppable = false;
        movecamera.moveSpeed = moveSpeed;
        Unstoppable = false;
        PwrUnstoppableDuration.transform.parent.gameObject.SetActive(false);
    }
}
