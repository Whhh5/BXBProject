using System.Collections.Generic;
using UnityEngine;
using BXB.Core;
using System;
using System.Reflection;
using System.Threading.Tasks;

[Serializable]
public class TemporiaryList<T>
{
    public List<T> list = new List<T>();
}
//[Serializable]
public partial class MainHandle : MiUIDialog
{
    [SerializeField] MiUIButton handleLeftButton;
    [SerializeField] MiUIButton handleRightButton;
    [SerializeField] MiUIButton cancelButton;
    [SerializeField] List<TemporiaryList<MiUIButton>> battleControllerFrame = new List<TemporiaryList<MiUIButton>>();
    [Header("RectTansform"),
        SerializeField] RectTransform handleLeft;
    [SerializeField] RectTransform handleRight;
    [SerializeField] RectTransform handleLeftTarget;
    [SerializeField] RectTransform handleRightTarget;

    [Header("Remote Script"),
        SerializeField] MainHandle_BloodController bloodController;

    [Header("Parameter"),
        SerializeField] Vector4 handleSpeedDistance;
    [SerializeField] Vector4 characterSpeedHeight;

    [Header("ReadOnly"),
        SerializeField, ReadOnly] bool temp;

    Action<uint> del_SlotId_SkillFrameDownClick;
    Action<uint> del_SlotId_SkillFrameUpClick;
    Action<uint> del_SlotId_SkillFrameClick;
    Action<uint> del_SlotId_SkillFrameEnterClick;
    Action<uint> del_SlotId_SkillFrameExitClick;
    Action<uint> del_SlotId_SkillFrameLongClick;

    Dictionary<uint, ulong> dic_SlotId_SkillId = new Dictionary<uint, ulong>()
    {
        { 1001, 0},         // jump
        { 1002, 0},         // handle
        { 1003 ,0},         // 
        { 2001, 10201010001}, // attack button
        { 2002, 10201010002}, // skill 1

    };
    Dictionary<MiUIButton, uint> dic_Button_SlotId = new Dictionary<MiUIButton, uint>();

    [SerializeField ,ReadOnly] public static HandleControllerInfo controllerInfo;
    //Temp Parameter
    [SerializeField, ReadOnly] CommonArrowBase weapons = null;
    [SerializeField, ReadOnly] bool isCloseButton = false;
    public new BattleSceneManager manager => BattleSceneManager.Instance;
    public override void OnInit()
    {
        dic_SlotId_SkillId = new Dictionary<uint, ulong>()
        {
            { 1001, 0},         // jump
            { 1002, 0},         // handle
            { 1003 ,0},         // 
            { 2001, 10201010001}, // attack button
            { 2002, 10201010002}, // skill 1
        };
        del_SlotId_SkillFrameDownClick = (x) => { };
        del_SlotId_SkillFrameUpClick = (x) => { };
        del_SlotId_SkillFrameClick = (x) => { };
        del_SlotId_SkillFrameEnterClick = (x) => { };
        del_SlotId_SkillFrameExitClick = (x) => { };
        del_SlotId_SkillFrameLongClick = (x) => { };
        del_SlotId_SkillFrameDownClick += HandleSkillSlotButtonDownClick;
        del_SlotId_SkillFrameUpClick += HandleSkillSlotButtonUpClick;
        del_SlotId_SkillFrameClick += HandleSkillSlotButtonClick;
        del_SlotId_SkillFrameEnterClick += HandleSkillSlotButtonEnterClick;
        del_SlotId_SkillFrameExitClick += HandleSkillSlotButtonExitClick;
        del_SlotId_SkillFrameLongClick += HandleSkillSlotButtonLongClick;
        for (uint i = 0; i < battleControllerFrame.Count; i++)
        {
            var list = battleControllerFrame[(int)i].list;
            for (uint j = 0; j < list.Count; j++)
            {
                dic_Button_SlotId.Add(list[(int)j], (i + 1) * 1000 + j + 1);
                var slotBtn = list[(int)j];
                var slotId = dic_Button_SlotId[slotBtn];
                slotBtn.AddOnPointerDownClick(async () =>
                {
                    del_SlotId_SkillFrameDownClick.Invoke(slotId);
                });
                slotBtn.AddOnPointerUpClick(async () =>
                {
                    del_SlotId_SkillFrameUpClick.Invoke(slotId);
                });
                slotBtn.AddOnPointerClick(async () =>
                {
                    del_SlotId_SkillFrameClick.Invoke(slotId);
                });
                slotBtn.AddOnPointerEnterClick(async () =>
                {
                    del_SlotId_SkillFrameEnterClick.Invoke(slotId);
                });
                slotBtn.AddOnPointerExitClick(async () =>
                {
                    del_SlotId_SkillFrameExitClick.Invoke(slotId);
                });
                slotBtn.AddOnPointerLongDownClick(async () =>
                {
                    del_SlotId_SkillFrameLongClick.Invoke(slotId);
                });
            }
        }
        gameObject.SetActive(true);
    }
    public override void OnSetInit(object[] value)
    {
    }
    public override async Task SetParameter<T>(params object[] value)
    {
        await AsyncDefaule();
    }
    public HandleControllerInfo GetInfo()
    {
        return controllerInfo;
    }

    #region UnityEngine Key Controller
    private void FixedUpdate()
    {
        manager.chara_Controller.Move_Rigidbody(new Vector3(characterSpeedHeight.x, 0, 0));
        if (Input.GetKeyDown(KeyCode.Space) && CharacterController_Rigidbody_2DBox.controllerInfo.isGround) 
        {
            manager.chara_Rigi.AddForce(new Vector3(0, characterSpeedHeight.y, 0), mode: ForceMode2D.Impulse);
        }
        var x = Input.GetAxis("Horizontal");

        HandleLeftMove(x);
    }
    void HandleLeftMove(float percentage)
    {
        Vector3 endAnchored;
        if ((handleLeftButton.GetButtonStatus() & ButtonStstus.Down) != 0)
        {
            var direction = Input.mousePosition - handleLeft.position;
            var cross = Vector3.Cross(new Vector3(0, 1, 0), direction).normalized.z;
            var distance = Mathf.Abs(Input.mousePosition.x - handleLeft.position.x);
            var anchoredX = distance > (handleSpeedDistance.y * 0.5f) 
                ? handleSpeedDistance.y * 0.5f * cross * -1 
                : distance * cross * -1;
            endAnchored = new Vector3(anchoredX, 0, 0); 
        }
        else
        {
            endAnchored = new Vector3(percentage * handleSpeedDistance.y * 0.5f, 0, 0);
        }
        handleLeftTarget.anchoredPosition3D =
            Vector3.Lerp(handleLeftTarget.anchoredPosition3D, endAnchored, handleSpeedDistance.x * Time.deltaTime);

        //controllerInfo.moveController = handleLeftTarget.anchoredPosition3D.x / handleSpeedDistance.y / 2;
        controllerInfo.moveController = handleLeftTarget.anchoredPosition3D.x;
        //Log(Color.green, controllerInfo.moveController);
    }
    void HandleRightMove()
    {
        float angle;
        Vector3 anchored;
        var mousePos = Input.mousePosition;
        var handleRightPosition = handleRight.position + new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        var vec = mousePos - handleRightPosition;
        vec.z = 0;
        var dot = Mathf.Acos(Vector3.Dot(new Vector3(0, 1, 0), vec.normalized)) * Mathf.Rad2Deg;
        var cross = Vector3.Cross(new Vector3(0, 1, 0), vec.normalized).normalized.z;
        angle = dot * cross + 90.0f;
        var distance = Mathf.Pow( Mathf.Pow(vec.x,2) + Mathf.Pow(vec.y,2),0.5f);
        distance = distance > handleSpeedDistance.w ? handleSpeedDistance.w : distance;
        anchored = (handleRightButton.GetButtonStatus() & ButtonStstus.Down) != 0 ? GetCirclePosition2D(angle,(ulong)distance) : new Vector3(0, 0, 0);

        handleRightTarget.anchoredPosition3D = 
            Vector3.Lerp(handleRightTarget.anchoredPosition3D, anchored, handleSpeedDistance.z * Time.deltaTime);

        //manager.tempImage.SetPosition(anchored);
        controllerInfo.handleAngle = angle;
        controllerInfo.angleRadius = distance;
    }
    #endregion

    #region button click add click

    void HandleSkillSlotButtonDownClick(uint slotId)
    {
        CommonAddCilck(state: ButtonStstus.Down, slotId);
    }
    void HandleSkillSlotButtonUpClick(uint slotId)
    {
        CommonAddCilck(state: ButtonStstus.Up, slotId);
    }
    void HandleSkillSlotButtonClick(uint slotId)
    {
        CommonAddCilck(state: ButtonStstus.Click, slotId);
    }
    void HandleSkillSlotButtonLongClick(uint slotId)
    {
        CommonAddCilck(state: ButtonStstus.Long, slotId);
    }
    void HandleSkillSlotButtonExitClick(uint slotId)
    {
        CommonAddCilck(state: ButtonStstus.Exit, slotId);
    }
    void HandleSkillSlotButtonEnterClick(uint slotId)
    {
        CommonAddCilck(state: ButtonStstus.Enter, slotId);
    }
    void CommonAddCilck(ButtonStstus state, uint slotId)
    {
        var methodName = string.Format("SkillSlot{0}Click_{1}", state, slotId);
        var method = GetType().GetMethod($"{methodName}", BindingFlags.Instance | BindingFlags.NonPublic);
        if (method != null)
        {
            method.Invoke(this, new object[] { slotId });
        }
        else
        {
            //Log(Color.red, $"Absent Method :  {methodName}");
        }
    }
    #endregion

    //commmon mathf
    Vector3 GetCirclePosition2D(float angle, ulong distance)
    {
        var y = distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        var x = distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector3(x, y, 0);
    }
}

public partial class MainHandle
{
    void SkillSlotDownClick_(uint slotId)
    {

    }
    void SkillSlotUpClick_(uint slotId)
    {

    }
    void SkillSlotClickClick_(uint slotId)
    {

    }
    void SkillSlotEnterClick_(uint slotId)
    {

    }
    void SkillSlotExitClick_(uint slotId)
    {

    }
    void SkillSlotLongClick_(uint slotId)
    {

    }

}
