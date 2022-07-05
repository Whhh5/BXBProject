using UnityEngine;
using BXB.Core;
using System.Threading.Tasks;

public partial class MiUIpopupHander : MiBaseClass
{
    private Dialog_LoadingScene dialogLoading = null;
    public async Task Showdialog_Loading(AsyncOperation value, bool isShow = true)
    {
        if (dialogLoading == null)
        {
            dialogLoading = 
                (await ResourceManager.Instance.loadUIElementAsync<GameObject>
                ($"Prefab/UI/Common", "Dialog_LoadingScene", CanvasLayer.Loading))
                .GetComponent<Dialog_LoadingScene>();
        }
        if (isShow)
        {
            await dialogLoading.SetUpShow(value);
        }
    }

    //-Menu
    private Dialog_Menu dialog_Menu = null;
    public async Task ShowDialog_Meun()
    {
        if (dialog_Menu == null)
        {
            dialog_Menu = 
                (await ResourceManager.Instance.loadUIElementAsync<GameObject>
                ($"Prefab/UI/Common", "Dialog_Menu", CanvasLayer.System))
                .GetComponent<Dialog_Menu>();
        }
        await dialog_Menu.SetUpShowAsync();
    }

    //-Hint 01
    private Dialog_Common_Hint_01 dialog_Common_Hint_01 = null;
    public async Task ShowDialog_Common_Hint_01Async(string str)
    {
        if (dialog_Common_Hint_01 == null)
        {
            dialog_Common_Hint_01 = 
                (await ResourceManager.Instance.loadUIElementAsync<GameObject>
                ($"Prefab/UI/Common", "Dialog_Common_Hint_01", CanvasLayer.System))
                .GetComponent<Dialog_Common_Hint_01>();
        }
        //await dialog_Common_Hint_01.SetUpShowAsync(str);
    }
}
