using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace WooHooFly.NodeSystem {

    public class Clickable : MonoBehaviour, IPointerDownHandler
    { 
        private Node currentNode;

        public Action<Clickable, Vector3> clickAction; 
        private void Awake()
        {
            currentNode = this.GetComponent<Node>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Click on object");
            if (clickAction != null)
            {
                // invoke the clickAction with world space raycast hit position
                clickAction.Invoke(this, eventData.pointerPressRaycast.worldPosition);
            }
        }
    }

}

