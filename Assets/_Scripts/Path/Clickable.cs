using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace WooHooFly.NodeSystem {

    public class Clickable : MonoBehaviour, IPointerDownHandler
    { 
        public Node clickedNode;
        public Action<Clickable, Vector3> clickAction;
        private void Awake()
        {
            clickedNode = this.transform.parent.GetComponentInChildren<Node>();
            EnablePointer(false);
        }

        public void EnablePointer(bool enable)
        {
            var pointer = this.GetComponent<IPointerDownHandler>() as MonoBehaviour;
            if (pointer != null) {
                pointer.enabled = enable;
            }
                
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (clickAction != null)
            {
                // invoke the clickAction with world space raycast hit position
                //Debug.Log("invoke");
                clickAction.Invoke(this, eventData.pointerPressRaycast.worldPosition);
            }
        }
    }

}

