using AsterNET.ARI;
using AsterNET.ARI.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
    ariClient.Channels.Answer(startEvent.Channel.Id);
    ariClient.Channels.Play(startEvent.Channel.Id, "sound:moo2").Wait(ariClient);
    ariClient.Channels.Hangup(startEvent.Channel.Id);
};

client.Connect();
Console.ReadKey();






