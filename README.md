# Robil over ROS2 guide
The following will guide you with running Robil project in two states:  
1. With HECTOR-SLAM  
Since HECTOR is built to work in ROS1, This will require you to have both ROS1 and ROS2 environments and run the ROS1_Bridge package.
so you must have both ROS versions installed on your machine.
2. All native ROS2 packages, including the ROS2 SLAM package --> SLAM Toolbox.

# 1. Running Robil with HECTOR-SLAM and ROS-Bridge
Since we are required to run both versions simultaneously, we must make sure that each terminal is set with **only one** of the versions environment (exept the ros_bridge). It should be absolutely clean from the other ones.
Therefore, make sure that your terminal is not automatically set with any of ROS's environment variables. means that when you open a new terminal and check the current environment, none is related to ROS.

if it is set automatically try the flollowing:
1. Remove any source command of ROS setup file from .bashrc file or any other file of this kind.
2. Check the folder **opt/ros/(your ROS version)/etc/catkin/profile.d**
This folder might be one of the places the o.s is looking for initial setting of environment variables.

Once the terminals are clean follow these steps:
1. clone/download this repository (https://github.com/ehud101/ROBIL.git)  
This repository contains 4 important file groups:
a. Unity files.
b. Operational files: all the environment files to be sourced and launch files.
c. ROS1 overlayed workspace: contains the HECTOR-SLAM package modified for ROBIL.
d. ROS2_dotnet: the package required for creating a C# wrapper for ROS messages (see the documentation "Architecture Over ROS2")

**In the follwoing steps, 4 Terminals will be used. we'll name them Terminal_1, Terminal_2....**  

3. Open the file "**OperationalFiles/.bashrc_ros1**". set the **ROS_DISTRO** variable to your ROS1 version.  
**Terminal_1:**  
&nbsp;a. Open a new terminal and source the **bashrc_ros1** file. This will set the environment to ROS1.  
&nbsp;b. run **roscore** . If you don't run the roscore, the rosbridge will fale to work.  

4. Open the file "**OperationalFiles/.bashrc_ros2**". set the **ROS_DISTRO** variable to your ROS2 version.  
**Terminal_2:** Open a new terminal and source the **bashrc_ros2** file. This will set the environment to ROS2.  

5. Open the file "**OperationalFiles/.bashrc_bridge**". make sure the two source lines point to your ROS1, and ROS2 correct paths respectively.  
**Terminal_3:**  
&nbsp;a. Open a new terminal and source the **bashrc_bridge** file.  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This will set the environment to both ROS1 and ROS2 which is mandatory for ROS1_bridge to work.  
&nbsp;&nbsp;b. Run the command: **"ros2 run ros1_bridge dynamic_bridge --bridge-all-2to1-topics"**   
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This will run the ros1_bridge node and set it to automatically bridge topics from both dierections

6. **Terminal_4:**  
&nbsp;&nbsp;a. Open a new terminal and source again **.bashrc_ros1**. From this terminal we will run the HECTOR-SLAM  
&nbsp;&nbsp;b. Run the command: "**rosparam set use_sim_time true**".  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This tells ROS1 packages (such as HECOTR-SLAM and RVIZ) to use the time published in the Clock topic. In ROBIL we publish UNITY time from the beginning of simulation.  
&nbsp;&nbsp;c. Run the command: "**roslaunch hector_slam_launch robil_slam.launch**"  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A launch-file which executes both RVIZ (where the SLAM map is displayed) and the HECTOR-SLAM itself.  

7. Open ROBIL project in Unity (the **ROBIL** directory you cloned/downloaded)  

8. Load the scene: **ROS2-Bridge-HectorSlam** and Hit the Play button. 

9. Wait about 20 secs and than in terminal_4 run the command "**roslaunch hector_slam_launch robil_slam.launch**" (which runs both RVIZ and the HECTOR-SLAM itself

An occupancy map should be displayed in RVIZ. move the robot in Unity and watch the map developes over time.

** The reason we wait some time before running the launch file is due to some synchronization issues. Unity and ROS are still not well synchronized in this project (reminder: This is not the official project but more an educative sub-project to experience ROS-Bridge)


# 1. Running ROBIL with SLAM-Toolbox over ROS2
This is the official project, althogh not complete as well.
Here we run the project in a clean environment of ROS2. so no need in remapping any messages and types, just using ROS2 native ones.

The main reason for moving to ROS2 is also ROS2 main novelty and the reason it was created: **Networking**
Though there are many other improvements in compare to ROS1 (such as Python lunch files instead of XML, DDS instead of ROSCore etc.) Networking is the major step of ROS2.
In ROS2 topics are automatically shared between all clients which are connected in LAN. But eventhough it is mandatory to know how to control the data in order to avoid a huge mess of it.

In this project we run again a very similar scene in Unity, But this time the user has an option to run multiple clients over the network.

We still use the ROS2-Unity interface we used in the bridge project.

1. Open Terminal and source **OperationalFiles/.bashrc_ros2**

2. Run Unity and load the scene: ROS2-SlamToolbox

3. Launch the file **OperationalFiles/robil_online_sync_launch.py**
