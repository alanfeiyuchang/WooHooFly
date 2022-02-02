/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RW.MonumentValley
{
    // class to activate/deactivate special Edges between Nodes based on rotation
    [System.Serializable]
    public class RotationLink
    {
        // transform to check
        public Transform linkedTransform;

        // euler angle needed to activate link
        public Vector3 activeEulerAngle;
        [Header("Nodes to activate")]
        public Node nodeA;
        public Node nodeB;
    }

    [System.Serializable]
    public class RotationErase
    {
        public Transform controllerTransform;
        public Vector3 activeEulerAngle1;
        public Vector3 activeEulerAngle2;
        public Vector3 activeEulerAngle3;
        [Header("GameObjects to show / hide")]
        public GameObject item;
    }

    // activates or deactivates special Edges between Nodes
    public class Linker : MonoBehaviour
    {
        [SerializeField] public RotationLink[] rotationLinks;
        [SerializeField] public RotationErase[] rotationErases;

        // toggle active state of Edge between neighbor Nodes
        public void EnableLink(Node nodeA, Node nodeB, bool state)
        {
            if (nodeA == null || nodeB == null)
                return;

            nodeA.EnableEdge(nodeB, state);
            nodeB.EnableEdge(nodeA, state);
        }

        // enable/disable based on transform's euler angles
        public void UpdateRotationLinks()
        {
            foreach (RotationLink l in rotationLinks)
            {
                if (l.linkedTransform == null || l.nodeA == null || l.nodeB == null)
                    continue;

                // check difference between desired and current angle
                Quaternion targetAngle = Quaternion.Euler(l.activeEulerAngle);
                float angleDiff = Quaternion.Angle(l.linkedTransform.rotation, targetAngle);

                // enable the linked Edges if the angle matches; otherwise disable
                if (Mathf.Abs(angleDiff) < 0.01f)
                {
                    EnableLink(l.nodeA, l.nodeB, true);
                }
                else
                {
                    EnableLink(l.nodeA, l.nodeB, false);
                }
            }
        }

        public void ShowItem(GameObject item, bool state)
        {
            if (item == null) return;
            item.SetActive(state);
            /*if (state)
                StartCoroutine(ShowItemAnim(0.5f, item));
            else
            {
                Color c = item.GetComponent<MeshRenderer>().material.color;
                c.a = 0f;
                item.GetComponent<MeshRenderer>().material.color = c;
            }*/
                
        }
/*
        private IEnumerator ShowItemAnim(float duration, GameObject item)
        {
            float timeElapsed = 0f;
            while (timeElapsed <= duration)
            {
                Color c = item.GetComponent<MeshRenderer>().material.color;
                c.a = Mathf.Lerp(0f, 1f, timeElapsed/duration);
                item.GetComponent<MeshRenderer>().material.color = c;

                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
*/
        public void HideItem()
        {
            foreach (RotationErase e in rotationErases)
            {
                if (e.item == null)
                    continue;

                    ShowItem(e.item, false);
            }
        }

        private bool inBetweenAngle(Vector3 target, Vector3 angle1, Vector3 angle2)
        {
            target = clampAngle(target);
            angle1 = clampAngle(angle1);
            angle2 = clampAngle(angle2);
            //Debug.Log(target);
            bool ans = true;
            if (angle1.x > angle2.x)
                ans = ans && target.x <= angle1.x && target.x >= angle2.x;
            else
                ans = ans && target.x <= angle2.x && target.x >= angle1.x;
            Debug.Log("1. " + ans);
            if (angle1.y > angle2.y)
                ans = ans && target.y <= angle1.y && target.y >= angle2.y;
            else 
                ans = ans && target.y <= angle2.y && target.y >= angle1.y;
            Debug.Log("2. " + ans);
            if (angle1.z > angle2.z)
                ans = ans && target.z <= angle1.z && target.z >= angle2.z;
            else
                ans = ans && target.z <= angle2.z && target.z >= angle1.z;
            Debug.Log("3. " + ans);
            return ans;
        }

        private Vector3 clampAngle(Vector3 target)
        {
            float x;
            if (target.x > 180)
                x = target.x - 360;
            else if (target.x < -180)
                x = target.x + 360;
            else
                x = target.x;

            float y;
            if (target.y > 180)
                y = target.y - 360;
            else if (target.y < -180)
                y = target.y + 360;
            else
                y = target.y;

            float z;
            if (target.z > 180)
                z = target.z - 360;
            else if (target.z < -180)
                z = target.z + 360;
            else
                z = target.z;
            return new Vector3(x, y, z);
        }
        public void UpdateRotationErase()
        {
            foreach (RotationErase e in rotationErases)
            {
                if (e.controllerTransform == null || e.item == null)
                    continue;

                if (inBetweenAngle(e.controllerTransform.rotation.eulerAngles, e.activeEulerAngle1, e.activeEulerAngle2))
                {
                    ShowItem(e.item, true);
                    Debug.Log("in 1");

                }
                else if (inBetweenAngle(e.controllerTransform.rotation.eulerAngles, e.activeEulerAngle2, e.activeEulerAngle3))
                {
                    ShowItem(e.item, false);
                    Debug.Log("in 2");
                }

/*
                if (Mathf.Abs(angleDiff) < 0.01f)
                    ShowItem(e.item, true);
                else
                    ShowItem(e.item, false);*/
            }
        }

        // update links when we begin
        private void Start()
        {
            UpdateRotationLinks();
        }
    }
}