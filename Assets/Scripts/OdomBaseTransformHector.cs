using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rclcs;
using geometry_msgs.msg;
using tf2_msgs.msg;

public class OdomBaseTransformHector : MonoBehaviourRosNode
{
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

         float time = Time.realtimeSinceStartup;

         int secs = (int)time;
         uint nanSecs =  (uint)(1e9 * (time - secs));
        // tfMessage.Transforms[0].Header.Stamp.Sec = secs;
        // tfMessage.Transforms[0].Header.Stamp.Nanosec = nanSecs;

        tfMessage.Transforms[0].UpdateHeaderTime(secs, nanSecs);
        tfMessage.Transforms[0].Header.Frame_id = "odom";
        tfMessage.Transforms[0].Child_frame_id = "base_footprint";
        tfMessage.Transforms[0].Transform.Translation.X = 0.0f;
        tfMessage.Transforms[0].Transform.Translation.Y = 0.0f;
        tfMessage.Transforms[0].Transform.Translation.Z = 1.0f;
        tfMessage.Transforms[0].Transform.Rotation.X = 0.0f;
        tfMessage.Transforms[0].Transform.Rotation.Y = 0.0f;
        tfMessage.Transforms[0].Transform.Rotation.Z = 0.0f;
        tfMessage.Transforms[0].Transform.Rotation.W = 1.0f;

        tfPublisher.Publish(tfMessage);     

    }
}
