using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Eff_Common_Cloud_1 : CommonEffectsBase
{
    public override void Active(params object[] objs)
    {
        Play();
    }

    public override void Prepare()
    {

    }

    public override void SetParameter(params object[] value)
    {
        var mainMode = mainParticle.main;
        mainMode.startLifetime = (float)value[1];
        mainMode.startDelay = (float)value[0];
    }
    private void Update()
    {
        if (mainParticle.particleCount <= 0)
        {
            Destroy();
        }
    }
}
