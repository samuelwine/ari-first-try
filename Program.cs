// See https://aka.ms/new-console-template for more information

//Not associated
//https://run.mocky.io/v3/0998fb1c-e38d-4799-a1a6-3cc240fae9d5
//https://designer.mocky.io/manage/delete/0998fb1c-e38d-4799-a1a6-3cc240fae9d5/OtKgPpa76wOVx48itNAgJDlH3O4lXpCHgEHM

//Associated but PIN not set on all accounts
//https://run.mocky.io/v3/35f18658-c49e-4652-ba43-434a7f260e0d
//https://designer.mocky.io/manage/delete/35f18658-c49e-4652-ba43-434a7f260e0d/LucWwWVHkNd27Hq3GQyXSU4dtX1LM003xMRZ

//All set
//https://run.mocky.io/v3/7b053044-6663-4c62-8592-5725c6220c12
//https://designer.mocky.io/manage/delete/7b053044-6663-4c62-8592-5725c6220c12/RbKe9ENOtg5t5Gg8KJXlClvpOexhdpSJbd1D

using AsterNET.ARI;
using AsterNET.ARI.Helpers;

string host = "135.181.204.69";
int port = 8088;
string username = "aht";
string password = "hello";
string application = "hello-world";
StasisEndpoint endpoint = new StasisEndpoint(host, port, username, password);
var uri = $"ws://{host}:{port}/ari/events?api_key={username}:{password}&app={application}";

AriClient client = new AriClient(endpoint, application);
client.OnStasisStartEvent += (ac, se) =>
{
    ac.Channels.Answer(se.Channel.Id);
    SyncHelper.Wait(ac.Channels.Play(se.Channel.Id, "sound:moo2"), client);
    ac.Channels.Hangup(se.Channel.Id);
};
client.Connect();
Console.ReadKey();




