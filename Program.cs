using AsterNET.ARI;
using FFmpeg.NET;
using Microsoft.Extensions.Configuration;
using System.Speech.Synthesis;

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

client.OnStasisStartEvent += (ariClient, startEvent) =>
{
    if (startEvent.Channel.Name.StartsWith("PJSIP"))
    {
        //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2");
        //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2");
        //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2");
        //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2");
        //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2");
        //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2");

        var externalMediaChannel = ariClient.Channels.ExternalMedia("hello-world", "192.168.8.122:12272", "slin");
        var channelVars = externalMediaChannel.Channelvars as Dictionary<string, object>;
        var listeningPort = channelVars["UNICASTRTP_LOCAL_PORT"].ToString();
        var bridge = ariClient.Bridges.Create();
        ariClient.Bridges.AddChannel(bridge.Id, startEvent.Channel.Id);
        ariClient.Bridges.AddChannel(bridge.Id, externalMediaChannel.Id);

        var testString = "This is a test string being sent to Asterisk via RTP";
        var synthesizer = new SpeechSynthesizer();
        synthesizer.SetOutputToWaveFile("C:\\Users\\HP\\Downloads\\");

        var ffmpeg = new Engine("C:\\ProgramData\\chocolatey\\bin");
        ffmpeg.ExecuteAsync(
            @$"-re -i c:/users/hp/downloads/enter-account-number.g722 -f rtp -payload_type 0 rtp://{ip}:{listeningPort}",
            new CancellationToken());
    }

    //var c = ariClient.Channels.Get(externalMediaChannel.Id);
    //ariClient.Channels.Answer(startEvent.Channel.Id);
    //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2").Wait(ariClient);
    //ariClient.Channels.Hangup(startEvent.Channel.Id);
};

client.Connect();
Console.ReadKey();






