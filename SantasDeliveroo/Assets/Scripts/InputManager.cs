using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Gioripoz;
using System.Linq;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : Singleton<InputManager>
{

    [SerializeField]
    private InputActionProperty leftClick;
    [SerializeField]
    private InputActionProperty rightClick;
    [SerializeField]
    private InputActionProperty CTRLClick;
    private bool ctrlPressed;

    private void Start()
    {
        leftClick.action.performed += LeftClickRay;
        rightClick.action.performed += RightClickRay;
        CTRLClick.action.performed += (CallbackContext ctx) => ctrlPressed = true;
        CTRLClick.action.canceled += (CallbackContext ctx) => ctrlPressed = false;
    }

    //selection click
    private void LeftClickRay(CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        bool deselect = false;
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI")) continue;

            Santa santa = hit.collider.gameObject.GetComponent<Santa>();

            if (santa)
            {
                Santa clickedSanta = santa;
                SelectionHandler.Instance.SelectedSanta = clickedSanta;
                deselect = false;
                break;
            }
            else
            {
                Gift gift = hit.collider.gameObject.GetComponent<Gift>();
                if (gift)
                {
                    Gift clickedGift = gift.gameObject.GetComponent<Gift>();
                    SelectionHandler.Instance.SelectedGift = clickedGift;
                    deselect = false;
                    break;
                }
                else
                {
                    deselect = true;
                }

            }
        }
        if (deselect)
        {
            SelectionHandler.Instance.DeselectAll();
        }
    }
    //move click
    private void RightClickRay(CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        PathPoint pathPoint = new PathPoint();
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("UI")) break; ;

            Gift gift = hit.collider.gameObject.GetComponent<Gift>();
            if (gift)
            {
                Gift clickedGift = gift.gameObject.GetComponent<Gift>();
                pathPoint.position = hit.point;
                pathPoint.targettedObject = clickedGift.gameObject;
                pathPoint.pointType = PathPoint.PointType.GIFT;
                break;
            }
            else
            {
                House house = hit.collider.gameObject.GetComponent<House>();
                if (!house)
                {
                    house = hit.collider.transform.parent?.gameObject.GetComponentInParent<House>();
                }
                if (house)
                {

                    House clickedHouse = house.gameObject.GetComponent<House>();
                    pathPoint.position = hit.point;
                    pathPoint.targettedObject = clickedHouse.gameObject;
                    pathPoint.pointType = PathPoint.PointType.HOUSE;

                }
                else
                {
                    pathPoint.position = hit.point;
                    pathPoint.targettedObject = hit.collider.gameObject;
                    pathPoint.pointType = PathPoint.PointType.POINT;
                }
            }
        }
        if (hits.Length > 0)
        {
            ModifyPath(pathPoint);
        }

    }

    private void ModifyPath(PathPoint point)
    {
        Santa selectedSanta = SelectionHandler.Instance.SelectedSanta;
        if (!selectedSanta) return;
        if (ctrlPressed)
        {
            selectedSanta.AddPathPoint(point);
        }
        else
        {
            selectedSanta.AddNewPath(point);
        }

    }
    /*
    private struct InputButton
    {
        private bool isPressing;
        public bool IsPressing
        {
            get
            {
                return isPressing;
            }
            set
            {
                if (isPressing)
                {
                    if (!value)
                    {
                        Released.Invoke();
                        isPressing = false;
                    }
                }
                else//if ispressing = false
                {
                    if (value)
                    {
                        Pressed.Invoke();
                        isPressing = true;
                    }
                }
                isPressing = value;
            }
        }

        public Action Pressed;
        public Action Released;
    }*/
}
