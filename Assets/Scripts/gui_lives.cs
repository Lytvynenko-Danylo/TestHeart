using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

public class gui_lives : MonoBehaviour
{
    public const int LivesMax = 5;
    public static int lives = LivesMax;
    private static System.Timers.Timer aTimer;
    public Vector3 destination;
    public float speed = 0.1f;
    private bool show = false;
    private bool move = false;
    public GameObject form;
    public static Text textCountL;
    public static Text textCount;
    public static Text textTimeL;
    public static Text textTime;
    public GameObject green;
    public GameObject blue;
    private static int counter = 0;
    private DateTime initial_time;
    private float min = -680;
    private float max = 10;
    // Use this for initialization
    void Start()
    {
        destination = form.transform.position;
        textCountL = GameObject.Find("TextLivesCountL").GetComponent<Text>();
        textCount = GameObject.Find("TextLivesCount").GetComponent<Text>();
        textTimeL = GameObject.Find("TextTimePassL").GetComponent<Text>();
        textTime = GameObject.Find("TextTimePass").GetComponent<Text>();
        TextUpdate();

    }

    // Update is called once per frame
    void Update()
    {
        Btn();
        if (lives != LivesMax)
        {
            DateTime current_time = DateTime.Now;
            var time1 = current_time - initial_time;
            textTimeL.text = textTime.text = time1.Minutes.ToString() + " : " + (15 - time1.Seconds).ToString();
            if (time1.Seconds == 15)
            {
                lives++;
                initial_time = DateTime.Now;
                TextUpdate();
            }
        }
        if (move)
        {
            if (show)
            {
                form.transform.Translate(Vector2.right * speed * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
            }
            else
            {
                form.transform.Translate(-Vector2.right * speed * Time.deltaTime);
            }
            if (form.transform.position.x < min || form.transform.position.x > max)
            {
                if (form.transform.position.x > max)
                    max = form.transform.position.x;
                else
                    min = form.transform.position.x;
                move = false;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.name == "gui-lives")
            {
                show = !show;
                move = true;
                // Vector3 whereSpawn = new Vector3(-0.86f, 0.365f, -9f);
                // this.shot = Instantiate(gameObject, whereSpawn, Quaternion.identity);
                // this.shot.transform.localScale = new Vector3(shotSize.x, shotSize.y, shotSize.z);
            }
        }
    }
    void TextUpdate()
    {
        textCountL.text = textCount.text = lives.ToString();
        if (lives == LivesMax)
        {
            ReachedMax();
        }
        var icon = GameObject.Find("life-icon");
        if (textTime.text == "")
        {
            icon.transform.localScale = new Vector3(0.574f, 0.636f, icon.transform.localScale.z);
            icon.transform.position = new Vector3(icon.transform.position.x, 60f, icon.transform.position.z);
        }
        else
        {
            icon.transform.localScale = new Vector3(0.4f, 0.4f, icon.transform.localScale.z);
            icon.transform.position = new Vector3(icon.transform.position.x, 120f, icon.transform.position.z);
        }
    }
    public void UseLife()
    {
        if (lives != 0)
        {
            lives--;
            counter = 15;
            SetTimer();
            textTimeL.text = textTime.text = "00:15";
            TextUpdate();
        }
    }
    public void Refill()
    {
        if (lives != LivesMax)
        {
            lives = LivesMax;
            TextUpdate();
        }

    }
    public void Close()
    {
        show = false;
        move = true;
    }
    void Btn()
    {
        if (lives == LivesMax)
        {
            blue.SetActive(false);
            green.SetActive(true);
        }
        else if (lives == 0)
        {
            blue.SetActive(true);
            green.SetActive(false);
        }
        else
        {
            if (!blue.activeSelf)
                blue.SetActive(true);
            if (!green.activeSelf)
                green.SetActive(true);
        }
        textCount.transform.position.Set(textCount.transform.position.x, textCount.transform.position.y, -7f);
    }
    void ReachedMax()
    {
        textTimeL.text = "Full";
        textTime.text = "";
    }
    private void SetTimer()
    {
        // if (aTimer != null)
        // {
        //     aTimer.Stop();
        //     aTimer.Dispose();
        // }
        // // Create a timer with a two second interval.
        // aTimer = new System.Timers.Timer(1000);
        // // Hook up the Elapsed event for the timer. 
        // aTimer.Elapsed += OnTimedEvent;
        // aTimer.AutoReset = true;
        // aTimer.Enabled = true;
        initial_time = DateTime.Now;
    }
    private static void OnTimedEvent(System.Object source, ElapsedEventArgs e)
    {
        if (lives == LivesMax)
        {
            aTimer.Stop();
            aTimer.Dispose();
            counter = 15;
        }
        if (counter == 0)
        {
            lives++;
            counter = 15;
        }
        textTimeL.text = textTime.text = "00:" + counter;
        counter--;

    }
}