using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rclcs;
using rosgraph_msgs.msg;

public class ClockPublisher : MonoBehaviourRosNode
{
    
    private string currentNodeName = "unityTimer";
    private string clockTopic = "clock";
   
    // private Publisher<builtin_interfaces.msg.Time> timePublisher;
    // private builtin_interfaces.msg.Time timeMessage;

    private Publisher<rosgraph_msgs.msg.Clock> timePublisher;
    private rosgraph_msgs.msg.Clock timeMessage;
    
    protected override string nodeName{
        get{
            return currentNodeName;
        }
    }

     protected override void StartRos(){

        // timePublisher = node.CreatePublisher<builtin_interfaces.msg.Time>(clockTopic);
        // timeMessage = new builtin_interfaces.msg.Time();

        timePublisher = node.CreatePublisher<rosgraph_msgs.msg.Clock>(clockTopic);
        timeMessage = new rosgraph_msgs.msg.Clock();
    }

     private void FixedUpdate(){
        float time = Time.realtimeSinceStartup;

        // timeMessage.Sec =  (int)time;
        // timeMessage.Nanosec = (uint)(1e9 * (time - timeMessage.Sec));

        timeMessage.Clock_.Sec = (int)time;
        timeMessage.Clock_.Nanosec = (uint)(1e9 * (time - timeMessage.Clock_.Sec));

    
        
        timePublisher.Publish(timeMessage);     

    }
}
