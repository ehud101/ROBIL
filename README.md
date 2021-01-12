# Robil over ROS2 guide
The following are the steps for running the Robil project in two modes:  
1. With HECTOR-SLAM  
Since HECTOR is meant to work on top of ROS1, This will require you to have both ROS1 and ROS2 environments and run the ROS1_Bridge package.
so you must have both ROS versions installed on your machine.
2. With SLAM Toolbox a native ROS2 packages.

# 1. Running Robil with HECTOR-SLAM and ROS-Bridge
Since we are required to run both versions simultaneously, we must make sure that each terminal's environment is set with only **one** of the versions (exept the one running ROS Bridge). It should be absolutely clean from the other ones.
Therefore, make sure that your terminal is not automatically set with none of ROS's environment variables. Means that when you open a new terminal and check the current environment, no variable is related to ROS.

If it is set automatically try the flollowing:
1. Remove any source command of ROS setup file from the .bashrc file or any other file of this kind.
2. Check the folder **opt/ros/(your ROS distro)/etc/catkin/profile.d**
This folder might be one of the places the Linux is looking for initial setting of environment variables.

Once the terminals are clean, follow these steps:
1. clone/download this repository (https://github.com/ehud101/ROBIL.git).
This repository contains 4 important file groups:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a. Unity files.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;b. Operational files: all the environment files to be sourced and launch files.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;c. ROS1 overlayed workspace: contains the HECTOR-SLAM package modified for ROBIL.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;d. ROS2_dotnet: the package required for creating a C# wrapper for ROS messages (see the documentation "Architecture Over ROS2")

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
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This will run the ros1_bridge node and set it to automatically bridge topics from both directions

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


# 2. Running ROBIL with SLAM-Toolbox over ROS2
This is the official project, which is still under developement.
Here we run the project in a clean environment of ROS2. so no need in remapping any messages and types, just using ROS2 native ones.

The main reason for moving to ROS2 is also ROS2 main novelty and the reason it was created: **Networking**. 

Though there are many other improvements in compare to ROS1 (such as Python launch files instead of XML, DDS instead of ROS-core etc.) Networking is the heart of ROS2.

In ROS2, topics are automatically shared between all clients which are connected in LAN. But even though, it is mandatory to know how to control the data in order to avoid a huge mess of it.

In this project we run again a very similar scene in Unity, But this time the user has an option to run multiple clients over the network. For this we use a package called **"Mirror"** which provides a network manager for Unity.
(Unity had its own manager which is now deprecated).

We still use the ROS2-Unity interface we used in the bridge project.

1. Open Terminal and source **OperationalFiles/.bashrc_ros2**

2. Run Unity and load the scene: ROS2-SlamToolbox

3. Hit the **Play** button.

4. Now we need to select our role in the network: server, client or host (which is server + client)
We are not yet testing the network mode, so click the **"Host"** button.
The robot will be set in its start position.


5. Launch the file **OperationalFiles/robil_online_sync_launch.py**

Like in the previous project, an occupancy map will be immediately drawn in RVIZ.

Drive the robot with the arrow keys and watch the map keeps developed over time.

### Run the project in LAN:

Now after testing the SLAM localy for only one user, it's time to run it over the network and have several clients.

**1. Local machine:**
Let's start with running the multyplayer mode on the local machine. We run the host inside Unity editor (server + one client) and another client in a built project.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a. Set your machine IP in the input field next to the **Client** button. Since we still work only on the same machine you may set it to **"localhost"**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;b. Build the project:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;b.1. Go to **File** --> **Build Settings**
a Build window is shown.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;b.2. make sure that only the SlamToolbox scene appears in the section **Scenes In Build**. if it is not already there, drad inside the scene from the Project window (in the editor)
Make sure the Target Platform is set to **Linux**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;b.3. Click **Build** and select where to save the built files.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;c. Run the project from Unity editor (remember to select **Host**) and the launch file.

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;d. run the **ROBIL.x86_64** file(the executable file) you've just built and select this time the **Client** Option.

Result: You should see now 2 robots (clients) each is controlled from its own environment. In RVIZ, you should notice two maps being developed independently- one for each robot client.

**Problem**: The TF (transformation frames) of the two clients are not synchronized, for this reason the two maps do not match. This is the next task of the project


**2. On different machines connected on LAN:**
To run the project on different machines:

&nbsp;&nbsp;&nbsp;&nbsp;a. Make sure that both machines have ROS2 installed (make sure that the SLAM Toolbox package is installed as well)

&nbsp;&nbsp;&nbsp;&nbsp;b. Make sure that each machine has a copy of the following files:

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;* executable **ROBIL.x86_64**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;* launch file **robil_online_sync_launch.py**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;* bash file **.bashrc_ros2**

&nbsp;&nbsp;&nbsp;&nbsp;c. source the bash file

&nbsp;&nbsp;&nbsp;&nbsp;d. launch the launch file

&nbsp;&nbsp;&nbsp;&nbsp;e. run the executable (remember to select one machine as the host and the other as client)

**This is it**


