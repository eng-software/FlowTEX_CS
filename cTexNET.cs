using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TEX
{
    class cTexNET
    {
        const byte STX                          = 0x02;
        const byte NAK                          = 0x03;
        const UInt32 MaxRetry			        = 3;
        const UInt32 Timeout                    = 1000;

        Stopwatch MessageTimer;
        Stopwatch RequestTimer;
        
        public class cMessage
        {            
            public byte Opcode;
            public byte Length;
            public byte[] Msg = new byte[255];

            public byte chks()
            {
                byte chksValue  = (byte)(Opcode + Length);
                for(int i = 0; i < (int)Length; i++)
                {
                    chksValue += Msg[i];
                }

                return chksValue;
            }
        };

        cMessage ReceivedMessage = new cMessage();
        cMessage MessageToSend = new cMessage();
                             
        enum eMessageState
        {
            eSTX,
            eOPCODE,
            eLENGTH,
            eMSG,
            eCHKS,
            eNEW_MSG,
            eNAK_RECEIVED,
            eNAK_TO_SEND,
        };

        enum eRequestState
        {
            eSTANDBY, 
            eSENDING_REQUEST,
            eREQUEST_SENT,
            eWAITTING_ANSWER,
            eNEW_ANSWER,

            eNEW_REQUEST,
            eSENDING_ANSWER,
            eANSWER_SENT,

            eSENDING_NAK,
            eNAK_SENT,                       
        };
        
        eMessageState MessageState;
        eRequestState RequestState;
        eRequestState RequestStateBeforeNAK;

        byte MessageReceivedBytes =0;
        UInt32 retry = 0;

        public cTexNET()
        {
            MessageState = eMessageState.eSTX;
            RequestState = eRequestState.eSTANDBY;
            MessageTimer = new Stopwatch();
            RequestTimer = new Stopwatch();
            MessageTimer.Start();
            RequestTimer.Start();
        }

        public void messagePoll(int data)
        {
            switch(MessageState)
            {
                default:
                case eMessageState.eSTX:
                {
                    if(data >= 0)
                    {
                        if( (byte)data == STX)
                        {
                            MessageState = eMessageState.eOPCODE;
                            MessageTimer.Restart();
                        }
                        else if( (byte)data == NAK)
                        {
                            MessageState = eMessageState.eNAK_RECEIVED;
                            MessageTimer.Restart();
                        }
                    }
                    break;
                }

                case eMessageState.eOPCODE:
                {
                    if(data >= 0)
                    {
                        ReceivedMessage.Opcode = (byte)data;
                        MessageTimer.Restart();
                        MessageState = eMessageState.eLENGTH;
                    }
                    else if (MessageTimer.ElapsedMilliseconds > Timeout)
                    {
                        MessageState = eMessageState.eSTX;
                    }
                    break;
                }

                case eMessageState.eLENGTH:
                {
                    if(data >= 0)                    
                    {
                        ReceivedMessage.Length = (byte)data;
                        MessageTimer.Restart();
                    
                        if(ReceivedMessage.Length > 0)
                        {
                            MessageReceivedBytes = 0;
                            MessageState = eMessageState.eMSG;
                        }
                        else
                        {
                            MessageState = eMessageState.eCHKS;
                        }
                    }
                    else if (MessageTimer.ElapsedMilliseconds > Timeout)
                    {
                        MessageState = eMessageState.eSTX;
                    }
                    break;
                }
                
                case eMessageState.eMSG:
                {
                    if(data >= 0)
                    {
                        ReceivedMessage.Msg[MessageReceivedBytes] = (byte)data;
                        MessageReceivedBytes++;
                        MessageTimer.Restart();

                        if(MessageReceivedBytes == ReceivedMessage.Length)
                        {
                            MessageState = eMessageState.eCHKS;
                        }
                        else if(MessageReceivedBytes > ReceivedMessage.Length)
                        {
                            MessageState = eMessageState.eSTX;
                        }
                    }
                    else if (MessageTimer.ElapsedMilliseconds > Timeout)
                    {
                        MessageState = eMessageState.eSTX;
                    }

                    break;
                }

                case eMessageState.eCHKS:
                {
                    if(data >= 0)
                    {
                        MessageTimer.Restart();

                        if((byte)data == ReceivedMessage.chks())
                        {
                            MessageState = eMessageState.eNEW_MSG;
                        }
                        else
                        {
                            MessageState = eMessageState.eNAK_TO_SEND;
                        }
                    }
                    else if (MessageTimer.ElapsedMilliseconds > Timeout)
                    {
                        MessageState = eMessageState.eSTX;
                    }
                    break;
                }

                case eMessageState.eNEW_MSG:
                case eMessageState.eNAK_RECEIVED:
                case eMessageState.eNAK_TO_SEND:
                {
                    break;
                }
            }
        }

        public void process()
        {
            if(MessageState == eMessageState.eNAK_TO_SEND)
            {
                if((RequestState == eRequestState.eSTANDBY)||
                   (RequestState == eRequestState.eWAITTING_ANSWER))
                {
                    RequestStateBeforeNAK = RequestState;
                    RequestState = eRequestState.eSENDING_NAK;
                    RequestTimer.Start();
                    RequestTimer.Restart();
                }
            }
                        
            switch(RequestState)
            {
                case eRequestState.eSTANDBY:
                {
                    if(MessageState == eMessageState.eNEW_MSG)
                    {
                        RequestState = eRequestState.eNEW_REQUEST;
                        RequestTimer.Restart();
                    }
                    else if(MessageState == eMessageState.eNAK_RECEIVED)
                    {
                        if(retry > 0)
                        {          
                            RequestState = eRequestState.eSENDING_ANSWER;
                            RequestTimer.Restart();                            

                            retry--;
                        }
                        else
                        {
                            MessageState = eMessageState.eSTX;
                            MessageTimer.Restart();
                        }
                    }
                    break;
                }
                
                case eRequestState.eNEW_REQUEST:
                {
                    if(RequestTimer.ElapsedMilliseconds > Timeout)
                    {
                        RequestState = eRequestState.eSTANDBY;
                        MessageState = eMessageState.eSTX;
                        RequestTimer.Restart();
                        MessageTimer.Restart();
                    }

                    break;
                }

                case eRequestState.eSENDING_ANSWER:
                case eRequestState.eANSWER_SENT:
                {
                    if(RequestTimer.ElapsedMilliseconds > Timeout)
                    {
                        RequestState = eRequestState.eSTANDBY;
                        MessageState = eMessageState.eSTX;
                        RequestTimer.Restart();
                        MessageTimer.Restart();
                    }

                    break;
                }
                
                case eRequestState.eSENDING_NAK:
                case eRequestState.eNAK_SENT:
                {
                    if(RequestTimer.ElapsedMilliseconds > Timeout)
                    {
                        RequestState  = RequestStateBeforeNAK;
                        MessageState = eMessageState.eSTX;
                        RequestTimer.Restart();
                        MessageTimer.Restart();
                    }

                    break;
                }

                case eRequestState.eWAITTING_ANSWER:
                {
                    if(MessageState == eMessageState.eNEW_MSG)
                    {
                        RequestTimer.Start();
                        RequestTimer.Restart();
                        RequestState = eRequestState.eNEW_ANSWER;
                    }
                    else
                    {
                        if(RequestTimer.ElapsedMilliseconds > Timeout)
                        {
                            if(retry > 0)
                            {
                                RequestState = eRequestState.eSENDING_REQUEST;
                                MessageState = eMessageState.eSTX;
                                RequestTimer.Restart();
                                MessageTimer.Restart();
                                retry--;
                            }
                            else
                            {
                                RequestState = eRequestState.eSTANDBY;
                                MessageState = eMessageState.eSTX;
                                RequestTimer.Restart();
                                MessageTimer.Restart();
                            }
                        }
                    }
                    break;
                }

                case eRequestState.eNEW_ANSWER:
                {
                    if(RequestTimer.ElapsedMilliseconds > Timeout)
                    {
                        RequestState = eRequestState.eSTANDBY;
                        MessageState = eMessageState.eSTX;
                        RequestTimer.Restart();
                        MessageTimer.Restart();
                    }

                    break;
                }                
            }                    
        }

        public byte[] getDataToSend()
        {
            byte[]  bufferToSend = null;

            if(RequestState == eRequestState.eSENDING_NAK)
            {
                bufferToSend = new byte[1];
                bufferToSend[0] = NAK;   
                RequestState = eRequestState.eNAK_SENT;
            }
            else if(RequestState == eRequestState.eSENDING_ANSWER)
            {
                bufferToSend = new byte[1+1+1+MessageToSend.Length+1];
                bufferToSend[0] = STX;
                bufferToSend[1] = MessageToSend.Opcode;
                bufferToSend[2] = MessageToSend.Length;

                if(MessageToSend.Length > 0)
                {
                    Buffer.BlockCopy(MessageToSend.Msg, 0, bufferToSend, 3, MessageToSend.Length);
                }

                bufferToSend[MessageToSend.Length + 3] = MessageToSend.chks();
                RequestTimer.Start();
                RequestTimer.Restart();
                RequestState  = eRequestState.eANSWER_SENT;
            }
            else if(RequestState == eRequestState.eSENDING_REQUEST)
            {
                bufferToSend = new byte[1+1+1+MessageToSend.Length+1];
                bufferToSend[0] = STX;
                bufferToSend[1] = MessageToSend.Opcode;
                bufferToSend[2] = MessageToSend.Length;
                
                if(MessageToSend.Length > 0)
                {
                    Buffer.BlockCopy(MessageToSend.Msg, 0, bufferToSend, 3, MessageToSend.Length);
                }

                bufferToSend[MessageToSend.Length + 3] = MessageToSend.chks();
                RequestTimer.Start();
                RequestTimer.Restart();
                RequestState  = eRequestState.eREQUEST_SENT;
            }

            return bufferToSend;
        }

        public void dataSent()
        {
            if(RequestState == eRequestState.eNAK_SENT)
            {
                RequestState = RequestStateBeforeNAK;
                MessageState = eMessageState.eSTX;
                RequestTimer.Restart();
                MessageTimer.Restart();
            }
            else if(RequestState == eRequestState.eANSWER_SENT)
            {
                RequestState = eRequestState.eSTANDBY;
                MessageState = eMessageState.eSTX;
                RequestTimer.Restart();
                MessageTimer.Restart();
            }
            else if(RequestState == eRequestState.eREQUEST_SENT)
            {
                RequestState = eRequestState.eWAITTING_ANSWER;
                MessageState = eMessageState.eSTX;
                RequestTimer.Restart();
                MessageTimer.Restart();
            }
        }

        public bool sendRequest(byte opcode, byte[] msg)
        {
            if(RequestState == eRequestState.eSTANDBY)
            {
                MessageToSend.Opcode = opcode;
                
                if(msg != null)
                {    
                    MessageToSend.Length = (byte)msg.Length;
                    Buffer.BlockCopy( msg, 0, MessageToSend.Msg, 0, msg.Length);   
                }
                else
                {
                    MessageToSend.Length = 0;                    
                }

                retry = MaxRetry;   
                RequestState = eRequestState.eSENDING_REQUEST;
                RequestTimer.Restart();
                return true;
            }

            return true;
        }

        public bool isWaitingAnswer()
        {
            if((RequestState == eRequestState.eSENDING_REQUEST)||
               (RequestState == eRequestState.eREQUEST_SENT)||
               (RequestState == eRequestState.eWAITTING_ANSWER))
            {
                return true;
            }

            return false;
        }
        
        public bool getAnswer(out byte opcode, out byte[] msg, out int length)
        {
            if(RequestState == eRequestState.eNEW_ANSWER)
            {
                opcode = ReceivedMessage.Opcode; 
                length = ReceivedMessage.Length;

                if(ReceivedMessage.Length > 0)
                {
                    msg = new byte[ReceivedMessage.Length];
                    Buffer.BlockCopy( ReceivedMessage.Msg, 0, msg, 0,  ReceivedMessage.Length);
                }
                else
                {
                    msg = null;
                }
                
                RequestState = eRequestState.eSTANDBY;
                MessageState = eMessageState.eSTX;
                RequestTimer.Restart();
                MessageTimer.Restart();
                return true;
            }

            opcode = 0;
            msg = null;
            length = 0;

            return false;
        }

        public bool getRequest(out byte opcode, out byte[] msg, out int length)
        {
            if(RequestState == eRequestState.eNEW_REQUEST)
            {
                opcode = ReceivedMessage.Opcode; 
                length = ReceivedMessage.Length;

                if(ReceivedMessage.Length > 0)
                {
                    msg = new byte[ReceivedMessage.Length];
                    Buffer.BlockCopy( ReceivedMessage.Msg, 0, msg, 0,  ReceivedMessage.Length);
                }
                else
                {
                    msg = null;
                }

                return true;
            }

            opcode = 0;
            msg = null;
            length = 0;

            return false;
        }

        public bool sendAnswer(byte opcode, byte[] msg)
        {
            if(RequestState == eRequestState.eNEW_REQUEST)
            {
                MessageToSend.Opcode = opcode;

                if(msg != null)
                {
                    MessageToSend.Length = (byte)msg.Length;
                    Buffer.BlockCopy( msg, 0, MessageToSend.Msg, 0, msg.Length);
                }
                else
                {
                    MessageToSend.Length = 0;
                }

                retry = MaxRetry;   
                RequestState = eRequestState.eSENDING_ANSWER;
                RequestTimer.Restart();
                return true;
            }

            return true;
        }

    }
}


