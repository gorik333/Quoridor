using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    private Camera _camera;

    private RaycastHit _raycastHit;


    private void Start()
    {
        _camera = Camera.main;
    }


    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                ClickOnGrid(touch);
        }
    }


    private void ClickOnGrid(Touch touch)
    {
        Ray ray = _camera.ScreenPointToRay(touch.position);

        if (Physics.Raycast(ray, out _raycastHit, 100f))
        {
            if (_raycastHit.collider.TryGetComponent(out GridPart gridPart))
            {
                var isAllowMoveTo = gridPart.MoveTo();

                if (!isAllowMoveTo)
                    DenyActions();
            }
        }
    }


    private void DenyActions()
    {

    }
}
