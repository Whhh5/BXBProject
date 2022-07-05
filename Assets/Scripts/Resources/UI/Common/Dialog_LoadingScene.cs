using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using BXB.Core;
public class Dialog_LoadingScene : MiUIDialog
{
    [SerializeField] MiUISlider slider;
    [SerializeField] MiUIButton nextBtn;

    protected override void Initialization()
    {
        base.Initialization();
    }
    public async Task SetUpShow(AsyncOperation opera)
    {
        await ShowAsync( DialogMode.none);
        Log(color: Color.black, $"{opera != null}");
        if (opera!=null)
        {
            StartCoroutine(LoadScene(opera));
        }
    }

    IEnumerator LoadScene(AsyncOperation opera)
    {
        ShowAsync(DialogMode.none).Wait();
        opera.allowSceneActivation = false;
        float schedule = 0.0f;
        while (schedule < 1)
        {
            schedule = opera.progress * 100 / 90.0f;
            slider.SetValue(schedule);
            yield return null;
        }
        nextBtn.onClick.SubscribeEventAsync(async () => 
        {
            await HideAsync();
            opera.allowSceneActivation = true;
        });

    }

    public override void Prepare()
    {
        
    }

    public override void SetParameter(object[] value)
    {
        
    }
    public override async Task SetParameter<T>(params object[] value)
    {
        AsyncOperation opera = (AsyncOperation)value[0];
        await ShowAsync(DialogMode.none);
        Log(color: Color.black, $"{opera != null}");
        if (opera != null)
        {
            StartCoroutine(LoadScene(opera));
        }
    }
}
