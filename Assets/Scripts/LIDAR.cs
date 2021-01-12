using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.UI;
using rclcs;

public class LIDAR : MonoBehaviourRosNode
{
    public string currentNodeName = "myScanner";
    public string scanTopic = "scan";
    public string frameID = "base_footprint";
    public float angleRes;
    public float minAngle;
    public float maxAngle;
    public float minRange;
    public float maxRange;
    public Transform sensorRotator;

    private Publisher<sensor_msgs.msg.LaserScan> LaserScanPublisher;
    private int oneCycleNumOfReadings;
    private float[] dist; //distance im m
    private float[] angle; //angle in degrees
    private int measurmenstsCounter;
    private float measurmentTime; //RP Lidar A2 time for a single measurment
    private float rotationFrequency;
    private int samplesPerPhysStep;
    private float currentAngle;
    private NativeArray<RaycastHit> results;
    private NativeArray<RaycastCommand> commands; 
    private sensor_msgs.msg.LaserScan message;
    private float[] azimuth;
    private float[] intensities;
    //private float scanTime;
    
    protected override string nodeName{
        get{
            return currentNodeName;
        }
    }

    protected override void StartRos(){
        frameID = "base_footprint";
        angleRes = 0.9f;
        oneCycleNumOfReadings = Mathf.RoundToInt(360/angleRes);
        dist = new float[oneCycleNumOfReadings];
        angle = new float[oneCycleNumOfReadings];
        measurmentTime = 0.00025f;
        //rotationFrequency = 1f / (((maxAngle - minAngle) / angleRes) * measurmentTime); // Not used anywhere
        samplesPerPhysStep = Mathf.RoundToInt(Time.fixedDeltaTime/measurmentTime);
        currentAngle = minAngle;
        sensorRotator.localEulerAngles = new Vector3(0, currentAngle, 0);
        azimuth = new float[samplesPerPhysStep];
        intensities = new float[oneCycleNumOfReadings];

        LaserScanPublisher = node.CreatePublisher<sensor_msgs.msg.LaserScan>(scanTopic);
        message = new sensor_msgs.msg.LaserScan();
        message.Ranges = new float[oneCycleNumOfReadings];
        message.Intensities = new float[message.Ranges.Length];
    }

    private void FixedUpdate()
        {        
          RaycastJobs();  // uing the multithread (jobs) raycast function
        }
    

    private void RaycastJobs()
        {
            var results = new NativeArray<RaycastHit>(samplesPerPhysStep, Allocator.TempJob);
            var commands = new NativeArray<RaycastCommand>(samplesPerPhysStep, Allocator.TempJob);
            int i;
            
            //completed one scan
            if (currentAngle >= 360)
            { 
                currentAngle = 0;
                sensorRotator.localEulerAngles = new Vector3(0, 0, 0);
            }

            Vector3 scanerPos = sensorRotator.position;//  Vector3 ScanerLinearStep = ScannerVel * Time.fixedTime;
            int add_val = 0;
            float startAngle = currentAngle;
            
            for (i = 0; i < samplesPerPhysStep; i++)
            {
                if (currentAngle >= 360)
                {
                    startAngle = 0;
                    sensorRotator.localEulerAngles = new Vector3(0, 0, 0);
                    add_val = 0;
                }

                commands[i] = new RaycastCommand(scanerPos, sensorRotator.TransformDirection(Vector3.forward), 6);
             
                add_val++;
                azimuth[i] = currentAngle;
                currentAngle = startAngle + add_val * angleRes;
                sensorRotator.localEulerAngles = new Vector3(0, currentAngle, 0);
            }


            // Schedule the batch of raycasts
            JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 8, default(JobHandle));// m_ResWidth * ConfigRef.Channels
            // Wait for the batch processing job to complete
            handle.Complete();

            for (int loc = 0; loc < results.Length; loc++)
            {
                RaycastHit raycastHit = results[loc];
             
                if (raycastHit.collider != null){
                
                    dist[measurmenstsCounter] = raycastHit.distance;
                }
                else
                {
                    dist[measurmenstsCounter] = 0;
                }

                angle[measurmenstsCounter] = azimuth[loc];

                if(measurmenstsCounter == oneCycleNumOfReadings -1){
                    measurmenstsCounter = 0;
                }

                else{
                    measurmenstsCounter++;
                }

                Array.Copy(dist, message.Ranges, oneCycleNumOfReadings); 
                
                // For HECTOR SLAM only: reverting the ranges to simulate counter clockwise scanning
                float tmp;
                for(int j = 0; j < Mathf.Floor(message.Ranges.Length / 2); j++){
                    tmp = message.Ranges[j];
                    message.Ranges[j] = message.Ranges[message.Ranges.Length - 1 - j];
                    message.Ranges[message.Ranges.Length - 1 - j] = tmp;
                }
                
                                
            }
            FillLaserScanMessage();
            LaserScanPublisher.Publish(message);      
            results.Dispose();
            commands.Dispose();

        }

        private void FillLaserScanMessage(){
            message.Header = new std_msgs.msg.Header{Frame_id = frameID};
            float time = Time.realtimeSinceStartup;
            message.Header.Stamp.Sec = (int)time;
            message.Header.Stamp.Nanosec = (uint)(1e9 * (time - message.Header.Stamp.Sec));
            message.Angle_min       = angle[0] * Mathf.Deg2Rad;
            message.Angle_max       = angle[oneCycleNumOfReadings -1] * Mathf.Deg2Rad;
            message.Angle_increment = angleRes * Mathf.Deg2Rad;
            message.Time_increment  = measurmentTime;
            //message.Scan_time       = scanTime;// not used
            message.Range_min       = minRange;
            message.Range_max       = maxRange;
            // intensities = intensities.clear
            Array.Clear(message.Intensities, 0, message.Intensities.Length);
        }

}
