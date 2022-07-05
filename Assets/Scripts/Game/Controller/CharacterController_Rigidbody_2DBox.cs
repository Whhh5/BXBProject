using UnityEngine;
using BXB.Core;

public class CharacterController_Rigidbody_2DBox : BaseGameObject_Game
{
    [SerializeField] Rigidbody2D rigi;
    [SerializeField] SpriteRenderer mainSpriteRenderer = new SpriteRenderer();
    [SerializeField] LayerMask layers;
    [SerializeField] Vector4 characterSpeedHeight;

    [SerializeField,ReadOnly] public static CharacterControllerInfo controllerInfo = new CharacterControllerInfo();
    [SerializeField] RaycastHit2D temp_hit;

    public CharacterControllerInfo GetCharacterInfo()
    {
        return controllerInfo;
    }
    public override void Prepare()
    {

    }
    public override void SetParameter(params object[] value)
    {

    }
    private void UpdateControllerInfo()
    {
        controllerInfo.bloodProportion = objInfo.presentBlood / objInfo.MaxBlood;
        controllerInfo.nowBlood = objInfo.presentBlood;
        controllerInfo.maxBlood = objInfo.MaxBlood;
    }
    public override void SetBlood(float value)
    {
        base.SetBlood(value);
        UpdateControllerInfo();
    }
    private void Update()
    {
        IsGround();
    }
    private void IsGround()
    {
        temp_hit = Physics2D.Raycast(transform.position, -Vector2.up, float.MaxValue, layerMask: layers);
        if (temp_hit.collider != null)
        {
            if (mainSpriteRenderer.sprite.bounds.extents.y >= temp_hit.distance)
            {
                controllerInfo.isGround = true;
            }
            else
            {
                controllerInfo.isGround = false;
            }
            Debug.DrawLine(transform.position, temp_hit.point, Color.red);
        }
        else
        {
            controllerInfo.isGround = false;
        }
    }
    public void Move_Rigidbody(Vector3 force)
    {
        var treeX = MainHandle.controllerInfo.moveController;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Battle2DManager.Instance.camera_Controller.OffsetToValueX(-5);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Battle2DManager.Instance.camera_Controller.OffsetToValueX(5);
        }

        rigi.AddForce(force * treeX, mode: ForceMode2D.Force);
    }
}
