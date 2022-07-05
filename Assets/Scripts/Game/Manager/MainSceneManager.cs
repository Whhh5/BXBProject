using BXB.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;

public class MainSceneManager : MiSingletonMonoBeHaviour<MainSceneManager>
{
    public Camera mainCamera;
    protected override void InitalizationInteriorParameter()
    {
        base.InitalizationInteriorParameter();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    protected override void Start()
    {
        base.Start();

        JsonData jsonData = new JsonData()
        {
            ["qwe"] = 1,
            ["s"] = "qwe",
        };
        foreach (var item in jsonData)
        {
            var data = (JsonData)item;
            Log(Color.green, data.ToJson(), item, data);
        }
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    protected override async Task InitializationAsync()
    {
        await base.InitializationAsync();
        //DontDestroyOnLoad(gameObject);
        await ResourceManager.Instance.LoadSceneAsync("UI", mode: LoadSceneMode.Additive);
    }

    public async Task GameStart()
    {
        await AsyncDefaule();

    }
}
