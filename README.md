# Foundry Hosted Agent Clients

This repository contains small .NET and Python console applications that show how to call a versioned agent hosted in a Microsoft Foundry project. Both samples use Microsoft Entra ID authentication, create an agent session, send a prompt through the Responses API, print the reply, and delete the session when finished.

## Projects

- `src/HostedAgentClient.Dotnet/HostedAgentClient` is the .NET console client. It uses the Azure AI Projects and Azure AI Extensions OpenAI SDKs and targets .NET 10.
- `src/HostedAgentClient.Python` is the Python console client. It uses the Azure AI Projects SDK and its OpenAI Responses client.

The clients are independent examples; run either one. The Python sample uses the preview agent-session API.

## Prerequisites

- A Microsoft Foundry project with a deployed/published agent and a version to call.
- Permission for your signed-in identity to access the project and use the agent.
- A supported local credential for `DefaultAzureCredential`. For local development, sign in with Azure CLI (`az login`) or use another credential source supported by the Azure Identity SDK.
- .NET 10 SDK to run the .NET client; Python and pip to run the Python client.

## Configure

In the chosen client's source file, replace the sample `endpoint` with your Foundry project endpoint, and set `agentName`/`agent_name` and `agentVersion`/`agent_version` to the agent and version you want to call. The endpoint has this form:

```text
https://<your-foundry-resource>.services.ai.azure.com/api/projects/<your-project-name>
```

The samples currently send `Hello, what can you help me with?`. Change that prompt in the source to try another request. Each run creates a fresh session and deletes it after the response.

## Run the .NET client

From the repository root:

```powershell
dotnet run --project src/HostedAgentClient.Dotnet/HostedAgentClient/HostedAgentClient.csproj
```

## Run the Python client

From the repository root, install the dependencies and run the sample:

```powershell
python -m pip install "azure-ai-projects==2.8.0" azure-identity
python src/HostedAgentClient.Python/client.py
```

Both programs print the selected agent and version, the created session ID, and the agent's response. The session is cleaned up in a `finally` block so it is deleted even if the request fails.
