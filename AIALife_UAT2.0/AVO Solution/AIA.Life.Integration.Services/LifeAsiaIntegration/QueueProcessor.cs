using AIA.Life.Models.MQModel;
using AIA.Life.Repository.AIAEntity;
using IBM.WMQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace AIA.Life.Integration.Services.LifeAsiaIntegration
{
    public static class QueueProcessor
    {
        static string _mqHostServer = Convert.ToString(ConfigurationManager.AppSettings["MQHostServer"]);
        static string _userName = Convert.ToString(ConfigurationManager.AppSettings["MQLoginUserName"]);
        static string _password = Convert.ToString(ConfigurationManager.AppSettings["MQLoginPassword"]);
        static string _mqManager = Convert.ToString(ConfigurationManager.AppSettings["MQManager"]);
        static string _portNumber = Convert.ToString(ConfigurationManager.AppSettings["PortNumber"]);
        public static string PutQueue(AVOAIALifeEntities entity,string RequestText, string strCorrelId,string callingFunc,string responseFunc)
        {
            WrapperImpersonationContext context = new WrapperImpersonationContext();
            context.Enter();
            string MessageText = string.Empty;
            MQQueueManager mqQMgr;
            MQQueue requestQueue;
            MQMessage requestMessage;
            QueueModel.MQRCText mqrcText = new QueueModel.MQRCText();
            Hashtable props = new Hashtable();
            var queueInfo = entity.tblSubFunctionMappings.Where(a => a.IntegrationFunctionName == callingFunc).FirstOrDefault();
            var resQueueInfo = entity.tblSubFunctionMappings.Where(a => a.IntegrationFunctionName == responseFunc).FirstOrDefault();

            string MQChannel = queueInfo.MQChannel;
            string RequestQueue = queueInfo.Queue;
            string ResponseQueue = resQueueInfo.Queue;
            string strRequestText = RequestText;

            //Step 1. Connect Queue Manager Object on the remote host.
            try
            {
                props.Add(MQC.HOST_NAME_PROPERTY, _mqHostServer);
                props.Add(MQC.CHANNEL_PROPERTY, MQChannel);
                props.Add(MQC.PORT_PROPERTY, _portNumber); // port number
                props.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_CLIENT);
                props.Add(MQC.USER_ID_PROPERTY, _userName);
                props.Add(MQC.PASSWORD_PROPERTY, _password);
                mqQMgr = new MQQueueManager(_mqManager, props);

            }
            catch (IBM.WMQAX.MQException mqe)
            {
                string strError = mqrcText.getMQRCText(mqe.Reason);
                MessageText = "Error trying to create Queue Manager Object. Error: " + mqe.Message + ", Details: " + strError;
                return MessageText;
            }
            catch (Exception ex)
            {
                MessageText = "Error trying to create Queue Manager Object. Error: " + ex.Message;
                return MessageText;
            }

            //Step 2. Open Request Queue for writing our request
            try
            {
                requestQueue = mqQMgr.AccessQueue(RequestQueue, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);		// but not if MQM stopping
            }
            catch (MQException mqe)
            {
                string strError = mqrcText.getMQRCText(mqe.Reason);
                MessageText = "Error trying to open Request Queue for writing. Error: " + mqe.Message + ", Details: " + strError;
                if (mqQMgr.IsConnected)
                    mqQMgr.Disconnect();
                return MessageText;
            }

            //Step 4. PUT Request Message in Request Queue. Note the options needed to be set.
            //Note that once PUT is successful, you can close the Request Queue. Note that you are
            //asking whoever receives this request to copy the MSG ID to CORREL ID so that
            //you can later "match" the response you get with the request you sent.

            requestMessage = new MQMessage();
            try
            {
                requestMessage.CharacterSet = 437;
                requestMessage.WriteString(RequestText);
                requestMessage.Format = MQC.MQFMT_STRING;
                requestMessage.MessageType = MQC.MQMT_REQUEST;
                requestMessage.Report = MQC.MQRO_COPY_MSG_ID_TO_CORREL_ID;
                requestMessage.ReplyToQueueName = ResponseQueue;

                requestMessage.ReplyToQueueManagerName = _mqManager;

                //Damo: Fix for multi thread Q mode. Assign the Correlation and MSG ID to the same reference key for the retrival.          
                //requestMessage.CorrelationId = System.Text.Encoding.UTF8.GetBytes(strCorrelId);
                //requestMessage.MessageId = requestMessage.CorrelationId;

                requestMessage.CorrelationId = System.Text.Encoding.UTF8.GetBytes(strCorrelId);
                requestMessage.MessageId = requestMessage.CorrelationId;

                requestQueue.Put(requestMessage);
                if (requestQueue.OpenStatus)
                    requestQueue.Close();
            }


            catch (MQException mqe)
            {
                string strError = mqrcText.getMQRCText(mqe.Reason);
                MessageText = "Error trying to PUT Message to Request Queue. Error: " + mqe.Message + ", Details: " + strError;

                return MessageText;
            }
            catch (Exception)
            {

            }

            context.Leave();
            return MessageText;
        }

        public static string GetQueue(AVOAIALifeEntities entity,byte[] CorId,string responseFunc)
        {
            WrapperImpersonationContext context = new WrapperImpersonationContext();
            context.Enter();
            MQQueueManager mqQMgr;
            MQQueue requestQueue;
            QueueModel.MQRCText mqrcText = new QueueModel.MQRCText();
            MQMessage requestMessage;
            var respQueueInfo = entity.tblSubFunctionMappings.Where(a => a.IntegrationFunctionName == responseFunc).FirstOrDefault();
            string Message = string.Empty;
            string strRequestQueueName = respQueueInfo.Queue;

            Hashtable props = new Hashtable();
            string MQChannel = respQueueInfo.MQChannel;

            //Step 1. Create Queue Manager Object. This will also CONNECT the Queue Manager
            try
            {
                props.Add(MQC.HOST_NAME_PROPERTY, _mqHostServer);
                props.Add(MQC.CHANNEL_PROPERTY, MQChannel);
                props.Add(MQC.PORT_PROPERTY, _portNumber); // port number
                props.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_CLIENT);
                props.Add(MQC.USER_ID_PROPERTY, _userName);
                props.Add(MQC.PASSWORD_PROPERTY, _password);
                mqQMgr = new MQQueueManager(_mqManager, props);
            }
            catch (MQException mqe)
            {
                string strError = mqrcText.getMQRCText(mqe.Reason);
                Message = "Error trying to create Queue Manager Object. Error: " + mqe.Message + ", Details: " + strError;
                context.Leave();
                return Message;
            }

            //Step 2. Open Request Queue for reading/ getting the request
            try
            {
                requestQueue = mqQMgr.AccessQueue(strRequestQueueName,
                    MQC.MQOO_INPUT_AS_Q_DEF				// open queue for input
                    + MQC.MQOO_FAIL_IF_QUIESCING);		// but not if MQM stopping
            }
            catch (MQException mqe)
            {
                string strError = mqrcText.getMQRCText(mqe.Reason);
                Message = "Error trying to open Request Queue for reading. Error: " + mqe.Message + ", Details: " + strError;
                if (mqQMgr.IsConnected)
                    mqQMgr.Disconnect();
                context.Leave();
                return Message;
            }

            //Step 3. GET the request message. Note that you decide how long you wait for a message
            //to show up. As you are the server now, you do no matching - you must serve to all
            //clients. Guess why this method needed a separate thread? Because the GET() can hang the
            //thread if it is long enough. Note that request queue is NOT closed after GET is over.
            try
            {
                requestMessage = new MQMessage();
                MQGetMessageOptions gmo = new MQGetMessageOptions();
                gmo.Options = MQC.MQGMO_WAIT;                  // must be specified if you use wait interval
                gmo.WaitInterval = MQC.MQWI_UNLIMITED;

                gmo.WaitInterval = Convert.ToInt32(ConfigurationManager.AppSettings["WaitTime2"]);

                // Copy correlationID of the message you want to receive       
                requestMessage.MessageId = CorId;

                requestQueue.Get(requestMessage, gmo);
                Message = requestMessage.ToString();
                Message = requestMessage.ReadString(requestMessage.MessageLength);
                return Message;
            }
            catch (MQException mqe)
            {
                string strError = mqrcText.getMQRCText(mqe.Reason);
                Message = "Error trying to GET message from Request Queue. Error: " + mqe.Message + ", Details: " + strError;
                if (requestQueue.OpenStatus)
                    requestQueue.Close();
                if (mqQMgr.IsConnected)
                    mqQMgr.Disconnect();
                context.Leave();
                return Message;
            }

        }
        
    }
}
