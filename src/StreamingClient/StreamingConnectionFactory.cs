using System;
using Lightstreamer.DotNet.Client;
using StreamingClient;
using TradingApi.Client.Core.ClientDTO;
using TradingApi.Client.Core.Lightstreamer;


public class TestStreamingConnectionFactory : IStreamingConnectionFactory
{
    public ILightstreamerConnection Create(Uri streamingUri, string userName, string sessionId)
    {
        return new TestStreamingConnection();
    }

    
}

public class TestStreamingConnection : ILightstreamerConnection
{
    public void FireUpdate(string status)
    {
        throw new NotImplementedException();
        OnStatusChanged(new StatusEventArgs() { Status = status});
    }

    public bool IsOpen
    {
        get { throw new NotImplementedException(); }
    }

    public LSClient LSClient
    {
        get { throw new NotImplementedException(); }
    }

    public string ServerUrl
    {
        get { throw new NotImplementedException(); }
    }

    public void Open()
    {
        throw new NotImplementedException();
    }

    public event EventHandler<StatusEventArgs> StatusChanged;

    public void OnStatusChanged(StatusEventArgs e)
    {
        throw new NotImplementedException();
    }

    public void Close()
    {
        throw new NotImplementedException();
    }
}


public class CityIndexStreamingConnectionFactory : IStreamingConnectionFactory
{
    private const string AdapterName = "CITYINDEXSTREAMING";
    public CityIndexStreamingConnectionFactory()
        : this(AdapterName)
    {
    }
    public CityIndexStreamingConnectionFactory(string adapterName)
    {
        _adapterName = adapterName;
    }

    private readonly string _adapterName;

    public ILightstreamerConnection Create(Uri streamingUri, string userName, string sessionId)
    {
        ILightstreamerConnection connection = new LightstreamerConnection(streamingUri.AbsoluteUri, userName, sessionId, _adapterName);
        return connection;
    }
}