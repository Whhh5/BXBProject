using UnityEngine;
using DG.Tweening;
using BXB.Core;

public class CameraController : MonoBehaviour
{
    //TransformÄ£¿é--------------------------------------------------
    [Header("TransformÄ£¿é")]
    [SerializeField] GameObject cameraObj;
    [SerializeField] GameObject targetObj;
    [SerializeField] Vector3 relativePosition;
    [SerializeField] Vector3 relativeRotation;
    [SerializeField] [Range(0,10)] float moveSpeed;
    [SerializeField] [Range(0, 50)] float angleSpeed;
    [SerializeField] bool isY;
    Transform cameraTransform => cameraObj.transform;
    Transform targetTransform => targetObj.transform;
    void Update()
    {
        if (enabled)
        {
            BorderObjRay();
        }
        CameraPositionAndRotationMove(targetTransform, relativePosition);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetTransform"></param>
    /// <param name="relativePosition"></param>
    void CameraPositionAndRotationMove(Transform targetTransform, Vector3 relativePosition)
    {
        var targetPos = targetTransform.TransformDirection(targetTransform.position) + 
            targetTransform.TransformDirection(targetTransform.forward * relativePosition.z) + 
            targetTransform.TransformDirection(targetTransform.up * relativePosition.y) + 
            targetTransform.TransformDirection(targetTransform.right * relativePosition.x);

        if (isY)
        {
            targetPos = new Vector3(targetPos.x, relativePosition.y, targetPos.z);
        }
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, 
            moveSpeed * Time.deltaTime);

        cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, 
            Quaternion.Euler(relativeRotation + targetTransform.rotation.eulerAngles), 
            angleSpeed * Time.deltaTime);
    }




    //´¥Ãþ·´À¡Ä£¿é-------------------------------------------------
    [Header("´¥Ãþ·´À¡Ä£¿é")]
    [SerializeField] bool enabled;
    [SerializeField] Material material;
    [SerializeField] LayerMask layerMask;
    [SerializeField,ReadOnly] GameObject borderObj;
    [SerializeField] Transform borderTransform => borderObj.transform;
    [SerializeField] MeshFilter borderMesh => borderObj.GetComponent<MeshFilter>();
    void BorderObjRay()
    {
        if (borderObj == null)
        {
            borderObj = new GameObject("Border Obj");
            borderObj.AddComponent<MeshFilter>();
            borderObj.AddComponent<MeshRenderer>().material = material;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, layerMask))
        {
            borderMesh.mesh = hit.collider.GetComponent<MeshFilter>().mesh;

            borderTransform.SetPositionAndRotation(hit.collider.transform.position, hit.collider.transform.rotation);
            borderTransform.localScale = hit.collider.transform.localScale;
            borderObj.SetActive(true);
        }
        else
        {
            borderObj.SetActive(false);
        }
    }


    public void OffsetToValue(Vector3 value)
    {

    }
    public void OffsetIncrement(Vector3 value)
    {

    }
    public void OffsetToValueX(float value)
    {
        DOTween.To(() => relativePosition.x, value => { relativePosition.x = value; }, value, 1.0f);
    }
    public void Shake()
    {
        var t1 = Random.Range(0.05f, 0.1f);
        var t2 = Random.Range(0.05f, 0.1f);
        var t3 = Random.Range(0.05f, 0.1f);

        DOTween.To(() => 2, value => { }, 0, t1)
            .OnStart(()=> cameraTransform.position = cameraTransform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)))
            .OnComplete(()=> {
                DOTween.To(() => 2, value => { }, 0, t2)
                    .OnStart(() => cameraTransform.position = cameraTransform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)))
                    .OnComplete(() => {
                        DOTween.To(() => 2, value => { }, 0, t3)
                            .OnStart(() => cameraTransform.position = cameraTransform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)))
                            .OnComplete(() => { });
                    });
            });
    }
}
