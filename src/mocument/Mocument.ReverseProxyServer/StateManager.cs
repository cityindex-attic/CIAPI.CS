using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Mocument.ReverseProxyServer
{
    public enum PlaybackMode
    {
        Default,
        Sequential
    }
    public class StateManager
    {
        public StateManager()
        {
            Items = new ConcurrentBag<ClientState>();
        }
        public ConcurrentBag<ClientState> Items { get; set; } 
    }

    public class ClientState
    {
        public ClientState()
        {
            Items=new ConcurrentDictionary<IPAddress, State>();
        }
        public ConcurrentDictionary<IPAddress, State> Items { get; set; }
    }
    public class State
    {
        public State()
        {
            PlaybackMode = PlaybackMode.Default;
        }
        public string TapeId { get; set; }
        public PlaybackMode PlaybackMode { get; set; }
        public int Position { get; set; }
    }
}
