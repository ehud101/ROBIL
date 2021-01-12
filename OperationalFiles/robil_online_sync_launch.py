from launch import LaunchDescription
from launch.actions import DeclareLaunchArgument
from launch.substitutions import LaunchConfiguration
from launch_ros.actions import Node
from ament_index_python.packages import get_package_share_directory


def generate_launch_description():
    use_sim_time = LaunchConfiguration('use_sim_time')

    declare_use_sim_time_argument = DeclareLaunchArgument(
        'use_sim_time',
        default_value='true')

    start_sync_slam_toolbox_node = Node(
        parameters=[
          get_package_share_directory("slam_toolbox") + '/config/mapper_params_online_sync.yaml',
          {'use_sim_time': use_sim_time}
        ],
        package='slam_toolbox',
        node_executable='sync_slam_toolbox_node',
        name='slam_toolbox',
        output='screen')

    start_rviz = Node(
	package='rviz2',
        node_executable='rviz2',
        name='rviz2',
	output='screen')


    ld = LaunchDescription()

    ld.add_action(declare_use_sim_time_argument)
    ld.add_action(start_sync_slam_toolbox_node)
    ld.add_action(start_rviz)


    return ld