using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ActionType{
    NULL = 0,
    BEAT,
    SHA_START,
    SHA_END,
    SHOOT,
    CLAP,
    ZEN,
    SI,
    ENDING_START,
    FINAL

}

public class TimeMarkItem{

    public float seconds;
    public ActionType action; 

}

public class OverallScript : MonoBehaviour {

    public UnityEngine.UI.Image title;
    List<TimeMarkItem> items;

    int cur_item_ind;
    float time_start;

    public DirectorBase director;
    bool playing = false;
    public UnityEngine.UI.Image img_cover;
    public AudioSource ass_main;

	// Use this for initialization
	void Start () {
        LoadCsv();

        
        director.OnInit();
        StartCoroutine(LaterStart());
	}

    IEnumerator LaterStart() {

        img_cover.DOFade(0, 3);
        title.DOFade(0,4).SetDelay(2);

        yield return new WaitForSeconds(5);

        StartPlay();
    }
	
	// Update is called once per frame
	void Update () {

        if (playing == false) return;

        float curTime = Time.time - time_start;
        if (cur_item_ind >= items.Count) { 
            //all done
            return;
        }

        for (int i = cur_item_ind; i < items.Count; i++) {

            if (items[i].seconds < curTime) { 
                //exectute action
                //print("execute " + items[i].action);

                director.OnAction(items[i].action);
                cur_item_ind++;
            }
        }

	}

    void LoadCsv() {

        items = new List<TimeMarkItem>();

        TextAsset textAss = Resources.Load<TextAsset>("bugNoticed");
        string content = textAss.text;

        string[] lines = content.Split('\n');
        print(lines.Length);

        int min = 0;
        float last_timeMark = 0;
        for (int i = 0; i < lines.Length; i++) {
            string line = lines[i].Trim('\r');
            if(line.Length == 0) continue;

            if (line[0] == '#') continue;

            string[] subs = line.Split(',');
            if (subs.Length != 5) continue;

            //3.45,[ACT1],[ACT2],[ACT3],[MINUTE]

            string str_timeMark = subs[0];
            float f_timeMark = -1;
            float.TryParse(str_timeMark, out f_timeMark);
            if (f_timeMark == -1) {
                continue;
            }  

            string str_min = subs[4];
            int int_min = -1;
            int.TryParse(str_min, out int_min);
            if (int_min != -1 && int_min > min) {
                min = int_min;
            }

            f_timeMark += min * 60;

            if (f_timeMark < last_timeMark) {
                print("WARN Disorder at" + str_min + "-" + str_timeMark);
            }
            last_timeMark = f_timeMark;

            for (int k = 1; k < 4; k++) {
                string str_act = subs[k];

                TimeMarkItem item = new TimeMarkItem();
                item.action = ActionType.NULL;
                item.seconds = f_timeMark;

                if (str_act == "BEAT") {
                    item.action = ActionType.BEAT;
                } else if (str_act == "SHA_START") {
                    item.action = ActionType.SHA_START;
                } else if (str_act == "SHA_END") {
                    item.action = ActionType.SHA_END;
                } else if (str_act == "SHOOT") {
                    item.action = ActionType.SHOOT;
                } else if (str_act == "CLAP") {
                    item.action = ActionType.CLAP;
                } else if (str_act == "ZEN") {
                    item.action = ActionType.ZEN;
                } else if (str_act == "SI") {
                    item.action = ActionType.SI;
                } else if (str_act == "ENDING_START") {
                    item.action = ActionType.ENDING_START;
                } else if (str_act == "FINAL") {
                    item.action = ActionType.FINAL;
                }

                if (item.action != ActionType.NULL) {
                    items.Add(item);
                }

            }

        }
    }

    void StartPlay() {
        playing = true;
        time_start = Time.time;
        cur_item_ind = 0;
        ass_main.Play();
    }

}
