using AsterNET.ARI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

string ip = config.GetValue<string>("aht_ari_ip");
int port = config.GetValue<int>("aht_ari_port");
string username = config.GetValue<string>("aht_ari_username");
string password = config.GetValue<string>("aht_ari_password");
string application = config.GetValue<string>("aht_ari_application");

StasisEndpoint endpoint = new StasisEndpoint(ip, port, username, password);
AriClient client = new AriClient(endpoint, application);

var times = 0;
client.OnStasisStartEvent += (ariClient, startEvent) =>
{

    if (times == 0)
    {
        times++;
        var externalMediaChannel = ariClient.Channels.ExternalMedia("hello-world", "rtp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4", "slin");
        var bridge = ariClient.Bridges.Create();
        ariClient.Bridges.AddChannel(bridge.Id, externalMediaChannel.Id);
        ariClient.Bridges.AddChannel(bridge.Id, startEvent.Channel.Id);
    }

    //var c = ariClient.Channels.Get(externalMediaChannel.Id);
    //ariClient.Channels.Answer(startEvent.Channel.Id);
    //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2").Wait(ariClient);
    //ariClient.Channels.Hangup(startEvent.Channel.Id);
};

client.Connect();
Console.ReadKey();






