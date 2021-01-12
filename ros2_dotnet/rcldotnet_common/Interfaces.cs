/* Copyright 2016-2018 Esteve Fernandez <esteve@apache.org>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace ROS2 {
  namespace Interfaces {

    //TODO - handle interfaces so there is not too much exposed on the library user side
    public interface IMessageInternals : System.IDisposable
    {
      IntPtr Handle { get; }
      IntPtr TypeSupportHandle { get; }
      void ReadNativeMessage();
      void WriteNativeMessage();
      void ReadNativeMessage(IntPtr handle);
      void WriteNativeMessage(IntPtr handle);
    }

    public interface Message : IMessageInternals
    {
    }

    public interface MessageWithHeader : Message
    {
      //An utility interface for messages with header
      void SetHeaderFrame(string frameID);
      string GetHeaderFrame();
      void UpdateHeaderTime(int sec, uint nanosec);
    }

    // rosidl
    #pragma warning disable 0169
    public struct rosidl_message_type_support_t
    {
        private IntPtr typesupport_indentifier;
        private IntPtr data;
        private IntPtr func;
    }
    #pragma warning restore 0169

  }
}
