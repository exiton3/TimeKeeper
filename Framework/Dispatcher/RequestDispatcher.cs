using Framework.Common;
using Framework.Common.Security;

namespace Framework.Dispatcher
{
    using System;
    using System.ComponentModel;
    using Model;
    using QueueManagment;
    using Repository;
    using TimeKeeperServiceReference;

    public class RequestDispatcher : IRequestDispatcher
    {
        private readonly IQueueRepository<ActivityMessage> queueRepository;
        private readonly IEncryptor encryptor;

        public RequestDispatcher(IQueueRepository<ActivityMessage> queueRepository, IEncryptor encryptor)
        {
            this.queueRepository = queueRepository;
            this.encryptor = encryptor;
        }

        public void Execute(object sender, DoWorkEventArgs e)
        {
            var pendingData = QueueManager<ActivityMessage>.Instance.PendingData;

            if (pendingData.Count > 0)
            {
                // Dispatch all messages
                if (DispatchAll(pendingData))
                {
                    pendingData.Clear();
                }
            }

            queueRepository.Save(pendingData);
        }

        private bool DispatchAll(ObservableQueue<ActivityMessage> pendingData)
        {
            string sendJson = GetSendJson(pendingData);
            string userName = GetUserName();
            bool result = SendData(userName, sendJson);
            return result;
        }

        private string GetUserName()
        {
            return encryptor.Encode(Environment.UserName);
            //return encryptor.Encode("amdar");
        }

        private string GetSendJson(ObservableQueue<ActivityMessage> pendingData)
        {
            var messageMapper = new MessageMapper(encryptor);
            var messages = messageMapper.MapCollection(pendingData.ToList());
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Serialize(messages);
        }

        private bool SendData(string userName, string data)
        {
            var portClient = new TimekeeperPortClient();

            System.Diagnostics.Debug.WriteLine("user name: ", userName);
            System.Diagnostics.Debug.WriteLine("Saving to webservice data: ", data);

            var result = portClient.storeActivity(userName, data);
            if (result.status.ToUpper() == "SUCCESS")
            {
                return true;

            }
            return false;

        }

    }
}