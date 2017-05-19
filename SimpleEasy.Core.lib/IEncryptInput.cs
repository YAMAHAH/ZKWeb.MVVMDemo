using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleEasy.Core.lib
{
    public interface IEncryptInput
    {
        string requestId { get; set; }
        string data { get; set; }
        bool encrypt { get; set; }
    }

    public class EncryptInput : IEncryptInput
    {
        public string requestId { get; set; }
        public string data { get; set; }
        public bool encrypt { get; set; }
    }
}
