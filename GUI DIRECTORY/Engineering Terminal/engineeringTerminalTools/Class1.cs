using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using commSockServer;
using videoManager;

namespace engineeringTerminalTools
{
    public class engineeringNetworkManager
    {
        commSockReceiver comSock;
        videoManager.ToolboxControl VM;

        public engineeringNetworkManager(commSockReceiver _comSock, videoManager.ToolboxControl _VM)
        {
            VM = _VM;
            VM.intendedCameraStatusChanged+=VM_intendedCameraStatusChanged;
            comSock = _comSock;
            comSock.IncomingLine += comSock_IncomingLine;
        }

        /// <summary>
        /// Example valid video update string: "VideoUpdate_Oculus_1"
        /// </summary>
        /// <param name="obj"></param>
        void comSock_IncomingLine(string obj)
        {
                if(obj!=null){
                    int state = -1;
                    if (obj.StartsWith("VideoUpdate_")) //Is it a video status update?
                    {
                        obj = obj.Replace("VideoUpdate_", "");

                        if (obj.StartsWith("Oculus_"))  //Is it a video update for the oculus
                        {
                            obj = obj.Replace("Oculus_", "");
                            if(int.TryParse(obj,out state)){
                                if (state == 1)
                                {
                                    VM.setReportedStatus(ToolboxControl.FeedID.OculusPT, true);
                                }
                                else if (state == 0)
                                {
                                    VM.setReportedStatus(ToolboxControl.FeedID.OculusPT, false);
                                }
                            }
                        }

                        else if (obj.StartsWith("Workspace_"))
                        {
                            obj = obj.Replace("Workspace_", "");
                            if (int.TryParse(obj, out state))
                            {
                                if (state == 1)
                                {
                                    VM.setReportedStatus(ToolboxControl.FeedID.Nose, true);
                                }
                                else if (state == 0)
                                {
                                    VM.setReportedStatus(ToolboxControl.FeedID.Nose, false);
                                }
                            }
                        }

                        else if (obj.StartsWith("Palm_"))
                        {
                            obj = obj.Replace("Palm_", "");
                            if (int.TryParse(obj, out state))
                            {
                                if (state == 1)
                                {
                                    VM.setReportedStatus(ToolboxControl.FeedID.Palm, true);
                                }
                                else if (state == 0)
                                {
                                    VM.setReportedStatus(ToolboxControl.FeedID.Palm, false);
                                }
                            }
                        }
                    }
               }
        }

        private void VM_intendedCameraStatusChanged(ToolboxControl.FeedID videoFeedID, bool feedState)
        {
            Console.WriteLine("BLARG!!!!!!!");
        }
    }
}
