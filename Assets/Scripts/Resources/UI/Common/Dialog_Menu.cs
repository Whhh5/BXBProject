using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;
using UnityEngine.UI;

public class Dialog_Menu : MiUIDialog
{
    [SerializeField] InputField idField;
    [SerializeField] InputField countField;
    [SerializeField] MiUIButton add;
    [SerializeField] MiUIButton remove;

    protected override async Task InitializationAsync()
    {
        await base.InitializationAsync();
    }
    public async Task SetUpShowAsync()
    {
        await ShowAsync();

        add.onClick.SubscribeEventAsync(async () => 
        {
            var id = ulong.Parse(idField.text);
            var count = ulong.Parse(countField.text);
            Log(color: Color.black, $"{id} {count}");
            //Article 下面已经弃用
            //await MiServiceManager.Instance.RequestAriticle_AddAsync(new List<ulong>(new ulong[] { id }), new List<ulong>(new ulong[] { count }));
        });
        remove.onClick.SubscribeEventAsync(async () =>
        {
            await AsyncDefaule();
            var id = ulong.Parse(idField.text);
            var count = ulong.Parse(countField.text);
            //Article 下面已经弃用
            //await MiServiceManager.Instance.RequestAriticle_RemoveAsync(new List<ulong>(new ulong[] { id }), new List<ulong>(new ulong[] { count }));
        });
    }

    public override void Prepare()
    {
        
    }

    public override void SetParameter(object[] value)
    {
        
    }

    public override Task SetParameter<T>(params object[] value)
    {
        throw new System.NotImplementedException();
    }
}
