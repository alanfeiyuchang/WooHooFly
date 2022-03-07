using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WooHooFly.NodeSystem
{
    [Serializable]
    public class HeatMapData
    {
        public int id;
        public int VisitCount;
        public float pos_x;
        public float pos_y;
        public float pos_z;
        public float ang_x;
        public float ang_y;
        public float ang_z;
        
        


        public HeatMapData(Node Node, int id)
        {
            this.id = id;
            this.VisitCount = Node.getVisitedCount();

            Vector3 vec = Node.GetInitialPosition();
            
            this.pos_x = Mathf.Round(vec.x * 10) / 10;
            this.pos_y = Mathf.Round(vec.y * 10) / 10;
            this.pos_z = Mathf.Round(vec.z * 10) / 10;

            vec = Node.getInitialEulerAngles();
            this.ang_x = Mathf.Round(vec.x * 10) / 10;
            this.ang_y = Mathf.Round(vec.y * 10) / 10;
            this.ang_z = Mathf.Round(vec.z * 10) / 10;
        }
    }

}