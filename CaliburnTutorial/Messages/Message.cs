using System;

namespace CaliburnTutorial.Messages
{
    public class Message
    {
        public String MessageText { get; private set; }

        public Message(String message)
        {
            MessageText = message;
        }
    }
}