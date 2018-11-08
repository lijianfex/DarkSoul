using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 相机控制器
/// </summary>
public class CameraController : MonoBehaviour
{
    public IPlayerInput pi;
    public float HorizontalSpeed = 100.0f;
    public float VerticalSpeed = 80.0f;
    public float CameraDamValue = 0.05f;
    public bool LockState = false;
    public Image Lockdot;

    private GameObject palyerHandle;
    private GameObject cameraHandle;
    private GameObject model;
    private GameObject cameraMain;

    private float tempEuerAnglesX;
    private Vector3 cameraDamVelocity;

    private LockTarget lockTarget;//锁定目标

    void Awake()
    {
        tempEuerAnglesX = 20.0f;

        cameraHandle = transform.parent.gameObject;
        palyerHandle = cameraHandle.transform.parent.gameObject;
        model = palyerHandle.GetComponent<ActorController>().Model;

        cameraMain = Camera.main.transform.gameObject;
        Lockdot.enabled = false;

        //隐藏鼠标
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        pi = palyerHandle.GetComponent<ActorController>().pi;
    }

    public void Update()
    {
        if(lockTarget!=null)
        {
            Lockdot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight, 0));
            if(Vector3.Distance(model.transform.position,lockTarget.obj.transform.position)>10.0f)
            {
                lockTarget = null;
                Lockdot.enabled = false;
                LockState = false;
            }
        }
        
    }

    private void FixedUpdate()
    {
        if(lockTarget==null)
        {
            Vector3 tempModelEnur = model.transform.eulerAngles;

            palyerHandle.transform.Rotate(Vector3.up, pi.JRight * HorizontalSpeed * Time.fixedDeltaTime);

            //cameraHandle.transform.Rotate(Vector3.right, pi.JUp * -VerticalSpeed * Time.deltaTime);//出现同位角问题

            tempEuerAnglesX -= pi.JUp * VerticalSpeed * Time.fixedDeltaTime;
            tempEuerAnglesX = Mathf.Clamp(tempEuerAnglesX, -40, 30);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEuerAnglesX, 0f, 0f);

            model.transform.eulerAngles = tempModelEnur;
        }
        else
        {
            Vector3 tempForward = lockTarget.obj.transform.position - model.transform.position;
            tempForward.y = 0;
            palyerHandle.transform.forward = tempForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }
        

        //cameraMain.transform.position = Vector3.Lerp(cameraMain.transform.position, transform.position, 0.2f);
        cameraMain.transform.position = Vector3.SmoothDamp(cameraMain.transform.position, transform.position, ref cameraDamVelocity, CameraDamValue);

        //cameraMain.transform.eulerAngles = transform.eulerAngles;
        cameraMain.transform.LookAt(cameraHandle.transform.position);
    }

    public void LockUnLock()
    {
        
        //Try to lock;
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, LayerMask.GetMask("Enemy"));
        if(cols.Length==0)
        {
            lockTarget = null;
            Lockdot.enabled = false;
            LockState = false;
        }
        else
        {
            foreach (var col in cols)
            {
                if(lockTarget!=null&&lockTarget.obj==col.gameObject)
                {
                    lockTarget = null;
                    Lockdot.enabled = false;
                    LockState = false;
                    break;
                }
                lockTarget=new LockTarget(col.gameObject,col.bounds.extents.y);
                Lockdot.enabled = true;                
                LockState = true;
                break;
            }
        }        

    }

    private class LockTarget
    {
        public GameObject obj;
        public float halfHeight;

        public LockTarget(GameObject _obj,float _halfHeight)
        {
            obj = _obj;
            halfHeight = _halfHeight;
        }
    }
}
