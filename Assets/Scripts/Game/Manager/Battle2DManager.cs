using UnityEngine;
using BXB.Core;
using System;
using System.Threading.Tasks;

public class Battle2DManager : MiSingletonMonoBeHaviour<Battle2DManager>
{
    public Camera mainCamera;
    public GameObject character;
    public GameObject mainHandle;

    //public CharacterControllerInfo controllerInfo => chara_Controller.GetCharacterInfo();
    //[SerializeField] public HandleControllerInfo mainHandle_Info => mainHandle_Controller.GetInfo();

    public CameraController camera_Controller => mainCamera.GetComponent<CameraController>();
    public Rigidbody2D chara_Rigi => character.GetComponent<Rigidbody2D>();
    public Transform chara_Transform => character.GetComponent<Transform>();
    public CharacterController_Rigidbody_2DBox chara_Controller => character.GetComponent<CharacterController_Rigidbody_2DBox>();
    public MainHandle mainHandle_Controller => mainHandle.GetComponent<MainHandle>();


    [SerializeField] private Action<float> del_SetBloodClick =  (x) => { };
    [SerializeField] private Action del_Finish = () => { };
    //Temp Parameter

    public static CharacterControllerInfo controllerInfo => CharacterController_Rigidbody_2DBox.controllerInfo;
    public static HandleControllerInfo mainHandle_Info => MainHandle.controllerInfo;

    protected override async Task InitalizationInteriorParameterAsync()
    {
        await base.InitalizationInteriorParameterAsync();
        AddDelegate_Finish(() => { Log(Color.red, $"Defeated"); });
        chara_Controller.AddBSetBloodClick((value) =>
        {
            Log(Color.black, $"{value.presentBlood}");
            if (value.presentBlood <= 0)
            {
                del_Finish.Invoke();
            }
        });
        string path = CommonManager.Instance.filePath.PreUIDialogPath;
        var mainHandler = await TableManager.Instance.tableData.ShowUIDialog<LocalizeUIDialogData, MiUIDialog>(path, 10104070001, CanvasLayer.Second);
        DontDestroyOnLoad(gameObject);
    }
    protected override void Initialization()
    {
        base.Initialization();
        del_SetBloodClick.Invoke(CharacterController_Rigidbody_2DBox.controllerInfo.bloodProportion);
    }
    public void SetCharacterBlood(float value)
    {
        chara_Controller.SetBlood(value);
        Log(Color.red, CharacterController_Rigidbody_2DBox.controllerInfo.bloodProportion.ToString());
        del_SetBloodClick.Invoke(CharacterController_Rigidbody_2DBox.controllerInfo.bloodProportion);

    }
    public void AddDelegate_SetBloodClick(Action<float> action)
    {
        del_SetBloodClick += action;
    }
    public void AddDelegate_Finish(Action action)
    {
        del_Finish += action;
    }
}
