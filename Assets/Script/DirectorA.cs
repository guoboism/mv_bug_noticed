using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DirectorA : DirectorBase {

    public SpriteRenderer sr_1;
    public SpriteRenderer sr_2;
    public SpriteRenderer sr_3;
    public SpriteRenderer bg;
    public bool isSHAing = false;

    public override void OnAction(ActionType act) {
        base.OnAction(act);


        switch (act) {
            case ActionType.BEAT:
                DoBeat(sr_1.transform);
                break;
            case ActionType.CLAP:
                DoBeat(sr_2.transform);
                break;
            case ActionType.ZEN:
                DoBeat(sr_3.transform);
                break;
            case ActionType.SHOOT:
                DoShoot(sr_2.transform);
                break;
            case ActionType.SHA_START:
                isSHAing = true;
                break;
            case ActionType.SHA_END:
                isSHAing = false;
                break;
        }

    }

    public void Update() {
        if (isSHAing) {
            bg.color = new Color(Random.Range( 0.7f,1), Random.Range(0.7f,1), Random.Range(0.7f, 1));
        }
    }

    void DoBeat(Transform tf) {

        tf.DOKill();
        tf.localScale = new Vector3(2, 2, 2);
        tf.DOScale(1, 0.1f);

    }

    void DoShoot(Transform tf) {

        tf.DOKill();
        tf.localScale = new Vector3(1, 10, 1);
        tf.DOScale(1, 0.1f);

    }

}
