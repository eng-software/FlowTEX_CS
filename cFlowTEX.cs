using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace TEX
{
    public class cFlowTEX
    {
        SerialPort TheSerialPort;
        Task TheTask;
        bool TaskEnd = false;
        Action TaskAction;
        cTexNET TexNET;
        cTexNET.cMessage NewRequest;

        int portOpenRetry = 3;
        bool bError = true;
        bool bDisconnect = false;
        int portIdx = -1;
        bool AutoDetectPort = true;

        public float flow;
        public float temperature;
        bool bActive;

        enum eSerialMode
        {
            eCLOSED,
            eSETUP,            
            eOPEN,
            eTEXNET,
        };

        enum eRequestMState
        {
            eFLOW,
            eFLOW_ANSWER,
            eRAW,
            eRAW_ANSWER,
            eUSER_REQUEST,
            eUSER_ANSWER,
        };

        enum eRequestStatus
        {
            eSTANDBY,
            eNEW_REQUEST,
            eWAITTING,
            eSUCCESS,
            eFAIL,
        };

        eRequestMState RequestMState;
        eRequestStatus RequestStatus;
        eSerialMode SerialMode;
        
        public cFlowTEX()
        {
            TheSerialPort = new SerialPort();
            RequestMState = eRequestMState.eFLOW;
            bActive = false;
            RequestStatus = eRequestStatus.eSTANDBY;
            SerialMode = eSerialMode.eCLOSED;
            flow = 0;
            temperature = 0;
        }

        ~cFlowTEX()
        {
            if(TheTask != null)
            {                
                if(TheTask.Status == TaskStatus.Running)
                {
                    TaskEnd = true;
                    while(TheTask.Status != TaskStatus.RanToCompletion)
                    {
                        Task.Delay(100);
                    }
                }
                TheTask.Dispose();
            }
        }

        public bool hasError()
        {
            return bError;
        }

        public void setSerialPort(string comName)
        {
            if(comName != null)
            {
                try
                {
                    bool bWasConnected = isConnected();
                    disconnect();
                    portIdx = -1;
                    AutoDetectPort = false;
                    TheSerialPort.PortName = comName;

                    if(bWasConnected)
                    {
                        connect();
                    }
                }
                catch
                {

                }
            }
            else
            {
                portIdx = -1;
                AutoDetectPort = true;
            }
        }

        public void init()
        {
            if(!bActive)
            {
                TexNET = new cTexNET();
                TaskAction = new Action(task);           
                TheTask = new Task(TaskAction);
                TaskEnd = false;
                TheTask.Start();
                bActive = true;               
            }

            connect();
        }
        
        public void connect()
        {
            bDisconnect = false;

            if(bActive)
            {
                SerialMode = eSerialMode.eSETUP;
                portOpenRetry = 3;
            }
        }

        public bool isConnected()
        {
            return (bActive&&(SerialMode!= eSerialMode.eCLOSED));
        }

        public void disconnect()
        {
            if(bActive)
            {
                if(TheSerialPort.IsOpen)
                {
                    TheSerialPort.Close();
                }
            }

            bDisconnect = true;
            SerialMode = eSerialMode.eCLOSED;
        }

        public bool isActive()
        {
            return bActive;
        }
           
        void task()
        {
            while(!TaskEnd)
            {
                if(bDisconnect)
                {
                    SerialMode = eSerialMode.eCLOSED;
                }

                switch(SerialMode)
                {
                    case eSerialMode.eCLOSED:
                    {
                        if(TheSerialPort.IsOpen)
                        {
                            TheSerialPort.Close();
                        }
                        bError = true;
                        bDisconnect = false;
                                                
                        if((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                        {
                            RequestStatus = eRequestStatus.eFAIL;
                        }
                        break;
                    }

                    case eSerialMode.eSETUP:
                    {
                        if(TheSerialPort.IsOpen)
                        {
                            TheSerialPort.Close();
                        }

                        if(AutoDetectPort)
                        {
                            try
                            {
                                List<string> Ports = new List<string>(System.IO.Ports.SerialPort.GetPortNames());

                                if(Ports.Count > 0)
                                {
                                    Ports.Sort();

                                    if(portIdx < 0)
                                    {
                                        portIdx = 0;
                                    }
                                    else
                                    {
                                        portIdx++;
                                    }

                                    if(portIdx >= Ports.Count)
                                    {
                                        portIdx = 0;
                                    }

                                    TheSerialPort.PortName = Ports[portIdx];
                                }
                                else
                                {
                                    portIdx = -1;
                                }
                            }
                            catch
                            {
                                portIdx = -1;
                            }
                        }

                        SerialMode = eSerialMode.eOPEN;
                        break;
                    }

                    case eSerialMode.eOPEN:
                    {
                        if(!TheSerialPort.IsOpen)
                        {
                            try
                            {                                    
                                TheSerialPort.BaudRate = 115200;
                                TheSerialPort.Open();
                            }
                            catch
                            {
                                bError = true;

                                if(portOpenRetry == 0)
                                {
                                    portOpenRetry = 3;
                                    SerialMode = eSerialMode.eSETUP;
                                }
                                else
                                {
                                    portOpenRetry--;
                                }
                            }                                                  
                        }
                        else
                        {
                            TheSerialPort.Close();
                        }

                        if( TheSerialPort.IsOpen )
                        {                            
                            SerialMode = eSerialMode.eTEXNET;
                        }
                        break;
                    }

                    case eSerialMode.eTEXNET:
                    {
                        if(!TheSerialPort.IsOpen)
                        {                            
                            portOpenRetry = 3;
                            SerialMode = eSerialMode.eSETUP;
                            bError = true;
                        }
                        else
                        {
                            poll();
                        }
                        break;
                    }
                }

                Thread.Sleep(10);
            }
        }

        void poll()
        {
            int data = -1;

            try
            {   
                if(TheSerialPort.IsOpen)
                {
                    do
                    {
                        try
                        {
                            if(TheSerialPort.BytesToRead > 0)
                            {   
                                data = TheSerialPort.ReadByte();
                            }
                            else
                            {
                                data = -1;
                            }
                        }
                        catch
                        {
                            data = -1;
                        }
                        finally
                        {
                            TexNET.messagePoll(data);
                            TexNET.process();
                            byte[] bufferTX = TexNET.getDataToSend();
                            if((bufferTX != null)&&(bufferTX.Length > 0))
                            {
                                TheSerialPort.Write(bufferTX, 0, bufferTX.Length);
                                TexNET.dataSent();
                            }
                        }
                    }
                    while((data >= 0)&&(TheSerialPort.IsOpen));
                }
            }
            catch
            {

            }

            if((bError) && (RequestStatus == eRequestStatus.eNEW_REQUEST))
            {
                RequestStatus = eRequestStatus.eFAIL;
            }

            if(TheSerialPort.IsOpen)
            {
                switch( RequestMState )
                {
                    default:
                    case eRequestMState.eFLOW:
                    {
                        TexNET.sendRequest((byte)'F', null );
                        RequestMState = eRequestMState.eFLOW_ANSWER;
                        break;
                    }

                    case eRequestMState.eFLOW_ANSWER:
                    {
                        if(!TexNET.isWaitingAnswer())
                        {
                            if(TexNET.getAnswer(out byte opcode, out byte[] msg, out int length))
                            {
                                if((opcode == (byte)'F')&&(length>=8))
                                {
                                    flow = BitConverter.ToSingle(msg, 0);
                                    temperature = BitConverter.ToSingle(msg,4);
                                    bError = false;
                                }
                            }
                            else
                            {
                                if(SerialMode != eSerialMode.eCLOSED)
                                {
                                    portOpenRetry = 3;
                                    SerialMode = eSerialMode.eSETUP;
                                    bError = true;
                                }
                            }

                            if(RequestStatus == eRequestStatus.eNEW_REQUEST)
                            {
                                RequestMState = eRequestMState.eUSER_REQUEST;
                            }
                            else
                            {
                                RequestMState = eRequestMState.eFLOW;
                            }
                        }
                        break;
                    }

                    case eRequestMState.eRAW:
                    {
                        TexNET.sendRequest((byte)'R', null );
                        RequestMState = eRequestMState.eRAW_ANSWER;
                        break;
                    }

                    case eRequestMState.eUSER_REQUEST:
                    {
                        TexNET.sendRequest(NewRequest.Opcode, NewRequest.Msg);
                        RequestStatus = eRequestStatus.eWAITTING;
                        RequestMState = eRequestMState.eUSER_ANSWER;
                        break;
                    }

                    case eRequestMState.eUSER_ANSWER:
                    {
                        if(!TexNET.isWaitingAnswer())
                        {
                            if(TexNET.getAnswer(out byte opcode, out byte[] msg, out int length))
                            {
                                if(opcode == NewRequest.Opcode)
                                {
                                    NewRequest.Opcode = opcode;
                                    NewRequest.Msg = msg;
                                    NewRequest.Length = (byte)length;
                                    RequestStatus = eRequestStatus.eSUCCESS;
                                    bError = false;
                                }
                                else
                                {
                                    RequestStatus = eRequestStatus.eFAIL;
                                    if(SerialMode != eSerialMode.eCLOSED)
                                    {
                                        portOpenRetry = 3;
                                        SerialMode = eSerialMode.eSETUP;
                                        bError = true;
                                    }
                                }
                            }
                            else
                            {
                                RequestStatus = eRequestStatus.eFAIL;
                                if(SerialMode != eSerialMode.eCLOSED)
                                {
                                    portOpenRetry = 3;
                                    SerialMode = eSerialMode.eSETUP;
                                    bError = true;
                                }
                            }
                            
                            RequestMState = eRequestMState.eFLOW;
                        }
                        break;
                    }
                }
            }
        }

        public bool getModel(out string model)
        {
            if(bActive)
            {
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }
                NewRequest = new cTexNET.cMessage();
                NewRequest.Msg = new byte[1];

                NewRequest.Opcode = (byte)'m';
                NewRequest.Length = (byte)NewRequest.Msg.Length;
                RequestStatus = eRequestStatus.eNEW_REQUEST;

                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }

                if(RequestStatus != eRequestStatus.eSUCCESS)
                {
                    model = "";
                    return false;
                }

                model = Encoding.ASCII.GetString(NewRequest.Msg, 0, 20);

                return true;
            }

            model = "";
            return false;
        }
        
        public bool getSerialNumber(out string SerialNumber)
        {
            if(bActive)
            {
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }
                NewRequest = new cTexNET.cMessage();
                NewRequest.Msg = new byte[1];

                NewRequest.Opcode = (byte)'n';
                NewRequest.Length = (byte)NewRequest.Msg.Length;
                RequestStatus = eRequestStatus.eNEW_REQUEST;

                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }

                if(RequestStatus != eRequestStatus.eSUCCESS)
                {
                    SerialNumber = "";
                    return false;
                }

                SerialNumber = Encoding.ASCII.GetString(NewRequest.Msg, 0, 10);

                return true;
            }

            SerialNumber = "";
            return false;
        }

        public bool getVersion(out string version)
        {
            if(bActive)
            {
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }
                NewRequest = new cTexNET.cMessage();
                NewRequest.Msg = new byte[1];

                NewRequest.Opcode = (byte)'v';
                NewRequest.Length = (byte)NewRequest.Msg.Length;
                RequestStatus = eRequestStatus.eNEW_REQUEST;

                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }

                if(RequestStatus != eRequestStatus.eSUCCESS)
                {
                    version = "";
                    return false;
                }

                version = Encoding.ASCII.GetString(NewRequest.Msg, 0, 10);

                return true;
            }

            version = "";
            return false;
        }

        public bool getStatus(out UInt32 FwChks, out UInt32 FwCalcChks)
        {
            if(bActive)
            {
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }
                NewRequest = new cTexNET.cMessage();
                NewRequest.Msg = new byte[1];

                NewRequest.Opcode = (byte)'h';
                NewRequest.Length = (byte)NewRequest.Msg.Length;
                RequestStatus = eRequestStatus.eNEW_REQUEST;

                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }

                if(RequestStatus != eRequestStatus.eSUCCESS)
                {
                    FwChks = 0;
                    FwCalcChks = 0;
                }
                else
                {
                    FwChks = BitConverter.ToUInt32(NewRequest.Msg, 0);
                    FwCalcChks = BitConverter.ToUInt32(NewRequest.Msg, 4);
                }
                
                return true;
            }

            FwChks = 0;
            FwCalcChks = 0;
            return false;
        }

        public double getFlow()
        {
            return flow;
        }

        public double getTemperature()
        {
            return temperature;
        }
    }

}
