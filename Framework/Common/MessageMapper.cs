namespace Framework.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dtos;
    using Model;
    using Security;

    public  class MessageMapper
    {
        private readonly IEncryptor encryptor;

        public MessageMapper(IEncryptor encryptor)
        {
            this.encryptor = encryptor;
        }

        public  MessageDto Map(ActivityMessage activityMessage)
        {
            if (activityMessage == null)
            {
                throw new ArgumentNullException("activityMessage","Object to map was null");
            }
            return new MessageDto
                       {
                           PrId =  encryptor.Encode(activityMessage.ProjectId.ToString()),
                           AcId =  encryptor.Encode(activityMessage.ActivityId.ToString()),
                           Start = TimeStampConverter.ConvertToUnixTimestamp(activityMessage.StartTime),
                           End = TimeStampConverter.ConvertToUnixTimestamp(activityMessage.StopTime),

                       };
        }

        public IEnumerable<MessageDto> MapCollection(IEnumerable<ActivityMessage> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }
            return list.Select(Map).ToList();
        }
    }
}