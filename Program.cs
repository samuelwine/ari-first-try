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
        var dict = new Dictionary<string, string> { { "JITTERBUFFER(adaptive)", "default" } };
        var externalMediaChannel = ariClient.Channels.ExternalMedia("hello-world", "192.168.8.121:12272", "g722", null, dict, null, null, null, "in");
        var channelVars = externalMediaChannel.Channelvars as Dictionary<string, object>;
        var listeningPort = channelVars["UNICASTRTP_LOCAL_PORT"].ToString();
        var bridge = ariClient.Bridges.Create();
        ariClient.Bridges.AddChannel(bridge.Id, startEvent.Channel.Id);
        ariClient.Bridges.AddChannel(bridge.Id, externalMediaChannel.Id);

        //var testString = "This is a test string being sent to Asterisk via RTP";
        //var synthesizer = new SpeechSynthesizer();
        //synthesizer.SetOutputToWaveFile(@"C:\Users\samue\Downloads\mynewstring.wav");
        ////synthesizer.SetOutputToDefaultAudioDevice();
        //synthesizer.Speak(testString);
        //ffmpeg - re - i "c:/users/samue/downloads/mynewstring.wav" - c:a copy -f rtp - payload_type 0 "rtp://192.168.8.122:17896"

        var ffmpegPath = "C:\\Users\\samue\\OneDrive\\Desktop\\ffmpeg.exe";
        //var ffmpegArgs = $"-re -i \"c:/users/samue/downloads/mynewstring.wav\" -c:a copy -f rtp -payload_type 9 \"rtp://192.168.8.122:17896\"";

        //var ffmpegArgs = $"-re -i \"C:\\Users\\samue\\Downloads\\shemaBeni.wav\" -f rtp -payload_type 0 \"rtp://192.168.8.122:17896\"";
        //var ffmpegArgs = $"-loglevel 48 -readrate 1 -i \"C:\\Users\\samue\\Downloads\\counting.wav\" -packetsize 200 -f rtp -payload_type 0 \"rtp://{ip}:{listeningPort}\" ";
        var ffmpegArgs = $"-loglevel 48 -re -i \"C:\\Users\\samue\\Downloads\\shemaBeni.wav\"  -f rtp -payload_type 0 \"rtp://{ip}:{listeningPort}\" ";
        //var ffmpegArgs = $"-re -i \"c:/users/samue/downloads/mynewstring.g722\" -acodec g722 -filter:a \"atempo=0.5,atempo=0.5,atempo=0.5 \" -f rtp -payload_type 9 \"rtp://{ip}:{listeningPort}\"";
        var proc = System.Diagnostics.Process.Start(ffmpegPath, ffmpegArgs);

        //var ffmpeg = new Engine();
        //ffmpeg.ExecuteAsync(
        //    @$"-re -i c:/users/samue/downloads/mynewstring.wav -f rtp -payload_type 0 rtp://{ip}:{listeningPort}",
        //    new CancellationToken());
    }

    //var c = ariClient.Channels.Get(externalMediaChannel.Id);
    //ariClient.Channels.Answer(startEvent.Channel.Id);
    //ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2").Wait(ariClient);
    //ariClient.Channels.Hangup(startEvent.Channel.Id);
};

client.Connect();
Console.ReadKey();






