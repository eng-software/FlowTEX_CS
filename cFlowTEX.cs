/*
   This example code is in the Public Domain

   This software is distributed on an "AS IS" BASIS, 
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
   either express or implied.

   Este código de exemplo é de uso publico,

   Este software é distribuido na condição "COMO ESTÁ",
   e NÃO SÃO APLICÁVEIS QUAISQUER GARANTIAS, implicitas 
   ou explicitas
*/

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
        public float zero;
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

        static private class cMTDOpcodes
        {
            public static readonly byte OPC_READ_FLOW = (byte)'F';		// Command 'F' - Host gets flow/temp. calc values
            public static readonly byte OPC_GET_MODEL = (byte)'m';		// Command 'm' - Host gets device Model
            public static readonly byte OPC_GET_SERIAL_NUMBER = (byte)'n';		// Command 'n' - Host gets device Serial Number
            public static readonly byte OPC_SAVE = (byte)'S';		// Command 'S' - Host sets device firmware Version number
            public static readonly byte OPC_GET_VERSION = (byte)'v';		// Command 'v' - Host gets device firmware Version number
            public static readonly byte OPC_STATUS = (byte)'h';        // Command 'h' - System status
            public static readonly byte OPC_LOCK = (byte)'L';        // Command 'L' - Lock device to no accept parameters change
            public static readonly byte OPC_UNLOCK = (byte)'l';        // Command 'l' - Unlock device for changes
            public static readonly byte OPC_SET_I2C_ADDRESS = (byte)'A';        // Command 'A' - Set I2C Address
            public static readonly byte OPC_GET_I2C_ADDRESS = (byte)'a';        // Command 'a' - Get I2C Address

        }

        public cFlowTEX()
        {
            TheSerialPort = new SerialPort();
            RequestMState = eRequestMState.eFLOW;
            bActive = false;
            RequestStatus = eRequestStatus.eSTANDBY;
            SerialMode = eSerialMode.eCLOSED;
            flow = 0;
            temperature = 0;
            zero = 0;
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
                            bError = false;
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
                        TexNET.sendRequest(cMTDOpcodes.OPC_READ_FLOW , null );
                        RequestMState = eRequestMState.eFLOW_ANSWER;
                        break;
                    }

                    case eRequestMState.eFLOW_ANSWER:
                    {
                        if(!TexNET.isWaitingAnswer())
                        {
                            if(TexNET.getAnswer(out byte opcode, out byte[] msg, out int length))
                            {
                                if((opcode == cMTDOpcodes.OPC_READ_FLOW) &&(length>=8))
                                {
                                    flow = BitConverter.ToSingle(msg, 0);
                                    flow -= zero;
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

                NewRequest.Opcode = cMTDOpcodes.OPC_GET_MODEL;
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

                NewRequest.Opcode = cMTDOpcodes.OPC_GET_SERIAL_NUMBER;
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

                NewRequest.Opcode = cMTDOpcodes.OPC_GET_VERSION;
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

                NewRequest.Opcode = cMTDOpcodes.OPC_STATUS;
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

        public bool setI2CAddress(byte Address)
        {
            if(bActive)
            {
                setLocked(false);
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                { Thread.Sleep(10); }

                NewRequest = new cTexNET.cMessage();
                NewRequest.Msg = new byte[1];
                NewRequest.Msg[0] = (byte)Address;
                NewRequest.Opcode = cMTDOpcodes.OPC_SET_I2C_ADDRESS;
                NewRequest.Length = (byte)NewRequest.Msg.Length;
                RequestStatus = eRequestStatus.eNEW_REQUEST;
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                { Thread.Sleep(10); }

                Save();

                setLocked(true);
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                { Thread.Sleep(10); }

                if(RequestStatus == eRequestStatus.eSUCCESS)
                { return true; }

            }

            return false;
        }

        public bool getI2CAddress(out byte Address)
        {
            if(bActive)
            {
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }
                NewRequest = new cTexNET.cMessage();
                NewRequest.Msg = new byte[1];

                NewRequest.Opcode = cMTDOpcodes.OPC_GET_I2C_ADDRESS;
                NewRequest.Length = (byte)NewRequest.Msg.Length;
                RequestStatus = eRequestStatus.eNEW_REQUEST;

                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }

                if(RequestStatus != eRequestStatus.eSUCCESS)
                {
                    Address = 0;
                    return false;
                }

                Address = (byte)NewRequest.Msg[0];
                return true;
            }

            Address = 0;
            return false;
        }


        public bool setLocked(bool Locked)
        {
            if(bActive)
            {
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }

                NewRequest = new cTexNET.cMessage();


                byte[] lockMsg;
                int len;

                if(Locked)
                {
                    lockMsg = Encoding.Default.GetBytes("FlowTEXLOCK");
                    len = 11;
                }
                else
                {
                    lockMsg = Encoding.Default.GetBytes("FlowTEXUNLOCK");
                    len = 13;
                }

                NewRequest.Msg = new byte[len];
                Buffer.BlockCopy(lockMsg, 0, NewRequest.Msg, 0, len);

                if(Locked)
                {
                    NewRequest.Opcode = cMTDOpcodes.OPC_LOCK;
                }
                else
                {
                    NewRequest.Opcode = cMTDOpcodes.OPC_UNLOCK;
                }

                NewRequest.Length = (byte)len;
                RequestStatus = eRequestStatus.eNEW_REQUEST;

                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                {
                    Thread.Sleep(10);
                }

                if(RequestStatus == eRequestStatus.eSUCCESS)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Save()
        {
            if(bActive)
            {
                setLocked(false);
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                { Thread.Sleep(10); }

                NewRequest = new cTexNET.cMessage();
                NewRequest.Msg = new byte[1];
                NewRequest.Msg[0] = cMTDOpcodes.OPC_SAVE;
                NewRequest.Opcode = cMTDOpcodes.OPC_SAVE;
                NewRequest.Length = (byte)NewRequest.Msg.Length;
                RequestStatus = eRequestStatus.eNEW_REQUEST;
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                { Thread.Sleep(10); }

                setLocked(true);
                while((RequestStatus == eRequestStatus.eNEW_REQUEST) || (RequestStatus == eRequestStatus.eWAITTING))
                { Thread.Sleep(10); }

                if(RequestStatus == eRequestStatus.eSUCCESS)
                {
                    return true;
                }
            }

            return false;
        }

        public void setZero()
        {
            zero += flow;
        }

        public void clearZero()
        {
            zero = 0;
        }


    }

}
