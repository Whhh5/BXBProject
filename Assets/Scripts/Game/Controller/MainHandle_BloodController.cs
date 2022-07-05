using UnityEngine;
using BXB.Core;
using DG.Tweening;
using UnityEngine.UI;

public class MainHandle_BloodController : MiBaseMonoBeHaviourClass
{
    [SerializeField] CanvasGroup enemyGroup;
    [Tooltip("show - duration - hide"), SerializeField] Vector3 enemyBloodTime;
    [Header("Blood"),
        SerializeField] RectTransform bloodAligning;
    [SerializeField] RectTransform bloodMaskUp;
    [SerializeField] RectTransform bloodMaskDown;
    [SerializeField] MiUIText charaName;
    [SerializeField] MiUIText charaBloodNumber;
    [SerializeField] Image iocn;
    [SerializeField] Image dieSignIcon;

    [Header("Enemy"),
        SerializeField] RectTransform enemyBloodAligning;
    [SerializeField] RectTransform enemyBloodMaskUp;
    [SerializeField] RectTransform enemyBloodMaskDown;
    [SerializeField] MiUIText enemyName;
    [SerializeField] MiUIText enemyBloodNumber;
    [SerializeField] Image enemyIcon;
    [SerializeField] GameObject enemyDieSignIcon;

    Tween DG_showEnemyBlood;
    protected override void Initialization()
    {
        base.Initialization();
        DG_showEnemyBlood = DOTween.To(() => enemyGroup.alpha, value => { enemyGroup.alpha = value; }, 1, enemyBloodTime.x).OnComplete(() =>
        {
            DG_showEnemyBlood = DOTween.To(() => 2, value => { }, 1, enemyBloodTime.y).OnComplete(() =>
           {
               DG_showEnemyBlood = DOTween.To(() => enemyGroup.alpha, value => { enemyGroup.alpha = value; }, 0, enemyBloodTime.z).OnComplete(() =>
               {

               });
           });
        });
        MiDataManager.Instance.dataProceccing.CharacterBloodEvent += (value) =>
        {
            enemyDieSignIcon.SetActive(false);
            switch (value.GetObjectType())
            {
                case ObjectType.None:
                    break;
                case ObjectType.Character:
                    bloodMaskDown.sizeDelta = new Vector2(-(1 - value.GetInfo().proportionBlood) * bloodAligning.rect.width, bloodMaskUp.sizeDelta.y);
                    bloodMaskUp.DOSizeDelta(new Vector2(-(1 - value.GetInfo().proportionBlood) * bloodAligning.rect.width, bloodMaskUp.sizeDelta.y),
                        1.0f, false).SetUpdate(false);
                    DOTween.To(() => value.GetInfo().lastBlood, updateValue =>
                    {
                        var number = string.Format("<color=#FF3700>{0}</color> / <color=#000000>{1}</color>", updateValue.ToString("#0.00"), value.GetInfo().MaxBlood.ToString("0.00"));
                        charaBloodNumber.SetRawText(number).Wait();
                    }, value.GetInfo().presentBlood, 1.0f);
                    break;
                case ObjectType.Enemy:
                    if (DG_showEnemyBlood.active)
                    {
                        DG_showEnemyBlood.Kill();
                    }
                    DG_showEnemyBlood.Play();
                    //Log(Color.green, $"{DG_showEnemyBlood != null}");
                    DOTween.To(() => enemyGroup.alpha, value => { enemyGroup.alpha = value; }, 1, enemyBloodTime.x).OnComplete(() =>
                    {
                        DG_showEnemyBlood = DOTween.To(() => 2, value => { }, 1, enemyBloodTime.y).OnComplete(() =>
                        {
                            DG_showEnemyBlood = DOTween.To(() => enemyGroup.alpha, value => { enemyGroup.alpha = value; }, 0, enemyBloodTime.z).OnComplete(() =>
                            {

                            });
                        });
                    });
                    enemyBloodMaskDown.sizeDelta = new Vector2(-(1 - value.GetInfo().proportionBlood) * enemyBloodAligning.rect.width, enemyBloodMaskUp.sizeDelta.y);
                    enemyBloodMaskUp.DOSizeDelta(new Vector2(-(1 - value.GetInfo().proportionBlood) * enemyBloodAligning.rect.width, enemyBloodMaskUp.sizeDelta.y),
                        1.0f, false).SetUpdate(false);
                    enemyName.SetRawText(value.GetInfo().name).Wait();
                    DOTween.To(() => value.GetInfo().lastBlood, updateValue =>
                    {
                        var number = string.Format("<color=#FF3700>{0}</color> / <color=#000000>{1}</color>", updateValue.ToString("#0.00"), value.GetInfo().MaxBlood.ToString("0.00"));
                        enemyBloodNumber.SetRawText(number).Wait();
                    }, value.GetInfo().presentBlood, 1.0f);
                    if (value.GetInfo().presentBlood <= 0)
                    {
                        enemyDieSignIcon.SetActive(true);
                    }
                    break;
                case ObjectType.Boos:
                    break;
                default:
                    break;
            }
        };
    }
}
