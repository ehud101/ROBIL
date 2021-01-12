using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rclcs;
using geometry_msgs.msg;
using tf2_msgs.msg;
using UnityEngine.UI;

public class BaseOdomTransform : MonoBehaviourRosNode
{
    
    public UnityEngine.Transform robotTF;

    public Text text;

    private string currentNodeName = "odometryTransform";
    private string tfTopic = "tf";
   
    private Publisher<tf2_msgs.msg.TFMessage> tfPublisher;
    
    //geometry_msgs/TransformStamped[] transforms 
    // ref: https://github.com/DynoRobotics/rcldotnet_custom_interfaces/blob/master/tf2_msgs/msg/TFMessage.msg
    private tf2_msgs.msg.TFMessage tfMessage;
    
    protected override string nodeName{
        get{
            return currentNodeName;
        }
    }

     protected override void StartRos(){

        tfPublisher = node.CreatePublisher<tf2_msgs.msg.TFMessage>(tfTopic);
        tfMessage = new tf2_msgs.msg.TFMessage();
        tfMessage.Transforms = new TransformStamped[1];
        tfMessage.Transforms[0] = new TransformStamped();
    }

     private void FixedUpdate(){


         if (GameObject.Find("RobotForNetworking(Clone)")){
             robotTF = GameObject.Find("RobotForNetworking(Clone)").transform;

         }

         if (GameObject.Find("Robot")){
             robotTF = GameObject.Find("Robot").transform;

         }
         // Early out if we don't have a target
         if (!robotTF){
            return;
         }

         float time = Time.realtimeSinceStartup;
        

         int secs = (int)time + 1;
         uint nanSecs =  (uint)(1e9 * (time - secs));
        // tfMessage.Transforms[0].Header.Stamp.Sec = secs;
        // tfMessage.Transforms[0].Header.Stamp.Nanosec = nanSecs;
        
        //Debug.Log(robotTF.position);
        
        tfMessage.Transforms[0].UpdateHeaderTime(secs, nanSecs);
        tfMessage.Transforms[0].Header.Frame_id = "odom";
        tfMessage.Transforms[0].Child_frame_id = "base_footprint";
        tfMessage.Transforms[0].Transform.Translation.X = robotTF.position.z;
        tfMessage.Transforms[0].Transform.Translation.Y = -robotTF.position.x;
        tfMessage.Transforms[0].Transform.Translation.Z = robotTF.position.y;
        tfMessage.Transforms[0].Transform.Rotation.X = -robotTF.rotation.z;
        tfMessage.Transforms[0].Transform.Rotation.Y =  robotTF.rotation.x;
        tfMessage.Transforms[0].Transform.Rotation.Z = -robotTF.rotation.y;
        tfMessage.Transforms[0].Transform.Rotation.W =  robotTF.rotation.w;

        // text.text = tfMessage.Transforms[0].Transform.Translation.X.ToString();

        tfPublisher.Publish(tfMessage);     

    }

}
