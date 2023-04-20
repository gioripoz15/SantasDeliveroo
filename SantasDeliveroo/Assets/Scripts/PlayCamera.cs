using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayCamera : MonoBehaviour
{
    public enum CameraType
    {
        TACTICAL,
        FREE,
        POV
    }

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject UI;

    [SerializeField]
    private Transform tacticalViewPoint;

    [SerializeField]
    private InputActionProperty cameraMove;
    [SerializeField]
    private InputActionProperty sapceBar;
    [SerializeField]
    private InputActionProperty pov;

    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector2 mouseSpeed;

    [SerializeField]
    private float postionMoveTime = 3;
    private CameraType cameraType = CameraType.TACTICAL;
    private Vector2 moveValue = Vector2.zero;

    private float rotationX;
    private float rotationLimit = 89.8f;

    public Action<CameraType> typeChanged;

    private void Start()
    {
        LevelManager.Instance.mainCamera = this;
        cameraMove.action.performed += ReadMoveValue;
        cameraMove.action.canceled += ReadMoveValue;
        sapceBar.action.performed += ReadSpacebar;
        pov.action.performed += TryPOV;
    }

    private void LateUpdate()
    {
        if (cameraType == CameraType.FREE)
        {
            MovePostion();
            MoveRotation();
        }
        if(cameraType == CameraType.POV)
        {
            var target = SelectionHandler.Instance.SelectedSanta.POVTransform;
            transform.rotation = target.rotation;
            Follow(target);
            MoveRotation();
        }
    }

    private void MovePostion()
    {
        transform.position += transform.forward * moveValue.y * Time.deltaTime;
        transform.position += transform.right * moveValue.x * Time.deltaTime;
    }

    //allow rotation on constraints looking up
    private void MoveRotation()
    {
        var rotationHorizontal = Input.GetAxis("Mouse X") * mouseSpeed.x * Time.deltaTime;
        var rotationVertical = Input.GetAxis("Mouse Y") * mouseSpeed.y * Time.deltaTime;

        //applying mouse rotation
        // always rotate Y in global world space to avoid gimbal lock

        rotationX += rotationVertical;
        rotationX = Mathf.Clamp(rotationX, -rotationLimit, rotationLimit);
        //limit the speed based on the angle to prevent fast spinning when looking up or down
        transform.Rotate(Vector3.up * rotationHorizontal / ((Mathf.Abs(rotationX) / rotationLimit) + 1) / 2, Space.World);

        var rotationY = transform.localEulerAngles.y;

        transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);
    }

    private void SetCameraPosition(Transform target)
    {
        StartCoroutine(cMoveTo(target));
    }

    private IEnumerator cMoveTo(Transform target)
    {
        postionMoveTime = 3;
        float timer = 0;
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        for (; timer <= postionMoveTime;)
        {
            transform.position = Vector3.Lerp(startPosition, target.position, (timer / postionMoveTime));
            transform.rotation = Quaternion.Lerp(startRotation, target.rotation, (timer / postionMoveTime));
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Follow(Transform target)
    {
        transform.position = target.position;
        //transform.rotation = target.rotation;
    }


    private void ReadSpacebar(CallbackContext ctx)
    {
        StopAllCoroutines();
        if (cameraType != CameraType.TACTICAL)
        {
            SetCameraPosition(tacticalViewPoint);
            cameraType = CameraType.TACTICAL;
        }
        else
        {
            cameraType = CameraType.FREE;
            rotationX = -transform.eulerAngles.x;

        }
        typeChanged?.Invoke(cameraType);
        UI.SetActive(cameraType == CameraType.TACTICAL);
    }

    private void TryPOV(CallbackContext ctx)
    {
        if (SelectionHandler.Instance.SelectedSanta)
        {
            cameraType = CameraType.POV;
            UI.SetActive(cameraType == CameraType.TACTICAL);
        }

    }

    private void ReadMoveValue(CallbackContext ctx)
    {
        var value = ctx.ReadValue<Vector2>();
        moveValue = value * speed;
    }

}
