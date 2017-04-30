using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DirectorB : DirectorBase {

    bool isSHAing = false;
    bool firstWaveDrum = true;
    bool endingStart = false;
    float line_radio = 0.8f;

    public Light light;
    public LineRenderer lr;
    public Transform drumPad;
    public Transform drumPad2;

    public Material mat_lr;
    public ParticleSystem ps_dots;

    List<GameObject> pillars;
    public GameObject pillar_contenter;
    public GameObject tp_shooter;
    List<GameObject> shooters;
    public Material mat_pillar;

    public UnityEngine.UI.Image title;

    public override void OnInit() {
        base.OnInit();

        shooters = new List<GameObject>();
        pillars = new List<GameObject>();
        int num = pillar_contenter.transform.childCount;
        for (int i = 0; i < num; i++) {
            GameObject newGo = Instantiate<GameObject>(tp_shooter, pillar_contenter.transform.GetChild(i));
            newGo.SetActive(true);
            newGo.transform.localPosition = new Vector3(0,0, 0);
            newGo.transform.localScale = new Vector3(1, 0, 1);

            shooters.Add(newGo);
            pillars.Add(pillar_contenter.transform.GetChild(i).gameObject);
        }

        mat_lr.SetColor("_TintColor", new Color(1, 1, 1, 0.3f));
        mat_pillar.SetColor("_Color", new Color(1, 1, 1));    
    }


    public override void OnAction(ActionType act) {
        base.OnAction(act);

        switch (act) {
            case ActionType.BEAT:
                DoBeat(drumPad);
                DoBeat(drumPad2, 0.05f); 
                if (firstWaveDrum) PS_Splash();
                PillarsBeat();
                break;
            case ActionType.CLAP:
                GenLine();
                LinePop();
                LineColor();
                break;
            case ActionType.ZEN:
                GenLine();
                LineFade();
                break;
            case ActionType.SHOOT:
                PillarUp();
                LinePop();
                break;
            case ActionType.SI:
                PS_Splash();
                mat_lr.SetColor("_TintColor", new Color(1, 1, 1, 0.3f));
                LinePop();
                break;
            case ActionType.SHA_START:
                firstWaveDrum = false;
                mat_lr.SetColor("_TintColor", new Color(1, 1, 1, 0.3f));
                isSHAing = true;
                break;
            case ActionType.SHA_END:
                isSHAing = false;
                endingStart = false;
                break;
            case ActionType.ENDING_START:
                endingStart = true;
                break;
            case ActionType.FINAL:
                LineFadeLong();
                title.DOFade(1, 3);
                break;
        }

    }

    public void Update() {
        if (isSHAing) {
             GenLine();
        }

        if (endingStart) {
            line_radio *= 1.001f;
            lr.transform.Translate(new Vector3(0, 0.006f, 0));
        }
    }

    void DoBeat(Transform tf, float delay = 0) {

        tf.DOKill();
        tf.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        tf.DOScale(1, 0.1f).SetDelay(delay);

    }

    void DoShoot(Transform tf) {

        tf.DOKill();
        tf.localScale = new Vector3(1, 10, 1);
        tf.DOScale(1, 0.1f);
    }

    void PillarsBeat() {
        foreach (GameObject go in pillars) {
            go.transform.localScale = new Vector3(1.7f, 1, 1.7f);
            go.transform.DOKill();
            go.transform.DOScale(1, 0.8f);
        }
    }

    void GenLine() {
        
        int count = 20;
        float r = line_radio;
        lr.positionCount = count;
        for (int i = 0; i < count-1; i++) {
            Vector3 pos = new Vector3(Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
            lr.SetPosition(i, pos);
        }

        lr.SetPosition(count-1, lr.GetPosition(0));
    }

    void LinePop() {
        lr.transform.localScale = new Vector3(1.2f, 1.2f ,1.2f);
        lr.transform.DOKill();
        lr.transform.DOScale(1, 0.2f);
        mat_lr.SetColor("_TintColor", new Color(1,1,1,0.3f));
    }

    void LineColor() { 
        Color c = new Color(Random.Range(0.2f, 0.9f),Random.Range(0.2f, 0.9f),Random.Range(0.2f, 0.9f));
        mat_pillar.SetColor("_Color", c);
        c.a = 0.3f;
        light.color = c;
        mat_lr.SetColor("_TintColor", c);
    }

    void PS_Splash() {
        ps_dots.Emit(30);
    }

    void LineFade() {
        Color c = new Color(Random.Range(0.2f, 0.9f),Random.Range(0.2f, 0.9f),Random.Range(0.2f, 0.9f));
        mat_lr.SetColor("_TintColor", c);
        mat_pillar.SetColor("_Color", c);
        c.a = 0;
        mat_lr.DOKill();
        mat_lr.DOFade(0, "_TintColor", 0.3f);
    }

    void PillarUp() {
        foreach (GameObject go in shooters) {
            go.transform.DOKill();
            go.transform.localScale = new Vector3(1, 6, 1);
            go.transform.DOScaleY(0, 0.8f);
        }
    }

    void LineFadeLong() {
        Color c = new Color(Random.Range(0.2f, 0.9f), Random.Range(0.2f, 0.9f), Random.Range(0.2f, 0.9f));
        mat_lr.SetColor("_TintColor", c);
        mat_pillar.SetColor("_Color", c);
        c.a = 0;
        mat_lr.DOKill();
        mat_lr.DOFade(0, "_TintColor", 5f);
    }

}
