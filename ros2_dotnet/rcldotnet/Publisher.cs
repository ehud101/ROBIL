﻿/*
© Dyno Robotics, 2019
Author: Samuel Lindgren (samuel@dynorobotics.se)
Licensed under the Apache License, Version 2.0 (the "License");

You may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Diagnostics;
using ROS2.Interfaces;

namespace rclcs
{

    public class Publisher<T>: IPublisher<T> where T : Message, new ()
    {
        rcl_publisher_t handle;
        rcl_node_t nodeHandle;

        private IntPtr publisherOptions = new IntPtr();

        private bool disposed;

        public Publisher(string topic, Node node, QualityOfServiceProfile qos = null)
        {
            nodeHandle = node.handle;
            handle = NativeMethods.rcl_get_zero_initialized_publisher();
            IntPtr publisherOptions = NativeMethods.rclcs_publisher_create_default_options();
            int qosProfileRmw = qos == null ? (int)QosProfiles.DEFAULT : (int)qos.Profile;
            NativeMethods.rclcs_publisher_set_qos_profile(publisherOptions, qosProfileRmw);

            //TODO(samiam): Figure out why System.Reflection is not available
            //when building with colcon/xtool on ubuntu 18.04 and mono 4.5

            //MethodInfo m = typeof(T).GetTypeInfo().GetDeclaredMethod("_GET_TYPE_SUPPORT");
            //IntPtr typeSupportHandle = (IntPtr)m.Invoke(null, new object[] { });

            IMessageInternals msg = new T();
            IntPtr typeSupportHandle = msg.TypeSupportHandle;
            Utils.CheckReturnEnum(NativeMethods.rcl_publisher_init(
                                    ref handle,
                                    ref nodeHandle,
                                    typeSupportHandle,
                                    topic,
                                    publisherOptions));
        }

        ~Publisher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    // Dispose managed resources.
                }

                DestroyPublisher();

                disposed = true;
            }
        }


        private void DestroyPublisher()
        {
            Utils.CheckReturnEnum(NativeMethods.rcl_publisher_fini(ref handle, ref nodeHandle));
            NativeMethods.rclcs_publisher_dispose_options(publisherOptions);
        }


        public void Publish(T msg)
        {
            msg.WriteNativeMessage();
            Utils.CheckReturnEnum(NativeMethods.rcl_publish(ref handle, msg.Handle));
        }
    }
}
