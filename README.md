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
1. clone/download this repository (https://github.com/ehud101/robil4.git)  
These are the main ROBIL files
2. clone/download the repository --> https://github.com/ehud101/ros1_ws.git  
This is a workspace for ROS1 overlay which contains the HECTOR-SLAM package modified for ROBIL.  
**Important!** put the folder in your **$HOME** directory.  

**In the follwoing steps, 4 Terminals will be used. we'll name them Terminal_1, Terminal_2....**  

3. Open the file "**filesToSource/.bashrc_ros1**". set the **ROS_DISTRO** variable to your ROS1 version.  
**Terminal_1:**  
&nbsp;a. Open a new terminal and source the **bashrc_ros1** file. This will set the environment to ROS1.  
&nbsp;b. run **roscore** . If you don't run the roscore, the rosbridge will fale to work.  

4. Open the file "**filesToSource/.bashrc_ros2**". set the **ROS_DISTRO** variable to your ROS2 version.  
**Terminal_2:** Open a new terminal and source the **bashrc_ros2** file. This will set the environment to ROS2.  

5. Open the file "**filesToSource/.bashrc_bridge**". make sure the two source lines point to your ROS1, and ROS2 correct paths respectively.  
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

7. Open ROBIL4 project in Unity (the **robil4** directory you cloned/downloaded)  

8. Hit the Play button in Unity. Now the beginning of map should be displayed in RVIZ (what the LIDAR "sees" at the beginning).
move the robot in Unity and watch the map developes with time.

Enjoy
