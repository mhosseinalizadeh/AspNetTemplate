using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.ClientEntity.ViewModel
{
    public class MessageModel
    {
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType {
        success,
        danger,
        warning,
        info
    }
}
