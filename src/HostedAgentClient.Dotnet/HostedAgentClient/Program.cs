using Azure.AI.Extensions.OpenAI;
using Azure.AI.Projects;
using Azure.AI.Projects.Agents;
using Azure.Identity;
using OpenAI.Responses;

#pragma warning disable AAIP001, OPENAI001, SCME0001

const string endpoint = "https://<myproject>.services.ai.azure.com/api/projects/proj-default-project";
const string agentName = "workflow-agent";
string agentVersion = "1";

Console.WriteLine($"Agent: {agentName}, version: {agentVersion}");

AIProjectClient projectClient = new(new Uri(endpoint), new DefaultAzureCredential());
AgentAdministrationClient admin = projectClient.AgentAdministrationClient;

// Para pegar a última versão em tempo de execução:
//   ProjectsAgentRecord agent = await admin.GetAgentAsync(agentName);
//   agentVersion = agent.Versions.Latest.Version;

// = project_client.agents.create_session(...)
ProjectAgentSession session = await admin.CreateSessionAsync(
    agentName, new VersionRefIndicator(agentVersion));
Console.WriteLine($"Created session: {session.AgentSessionId}");

try
{
    // = project_client.get_openai_client(agent_name=...)
    ProjectResponsesClient responses = projectClient.ProjectOpenAIClient
        .GetProjectResponsesClientForAgentEndpoint(agentName);

    CreateResponseOptions req = new()
    {
        InputItems = { ResponseItem.CreateUserMessageItem("Hello, what can you help me with?") }
    };
    // = extra_body={"agent_session_id": ...}
    req.SessionId = session.AgentSessionId;
    // Se o projeto não estiver em C# 14 / .NET 10, troque a linha acima por:
    // req.Patch.Set("$.agent_session_id"u8, session.AgentSessionId);

    ResponseResult response = await responses.CreateResponseAsync(req);
    Console.WriteLine($"Response: {response.GetOutputText()}");
}
finally
{
    // = project_client.agents.delete_session(...)
    await admin.DeleteSessionAsync(agentName, session.AgentSessionId);
}