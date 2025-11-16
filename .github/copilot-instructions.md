# Copilot Agent Instructions — Azure AI From Zero to Hero

You (Copilot Agent) are assisting in building a long-term learning repository called:

**Azure AI — From Zero to Hero**

This is a personal learning journey focused on Azure AI, Azure App Service, and .NET development.
Your job is to help generate and maintain notebooks, labs, sample applications, and documentation in a consistent, high-quality manner.

---

## Project Purpose

The repository aims to:

- Document the author's weekly learning progress
- Build a complete practical guide for Azure AI and App Service
- Provide runnable code examples using C# and .NET
- Include hands-on labs, architecture notes, and sample deployments
- Keep everything simple, clear, and useful for real developers

This is not a finished guide; it will grow incrementally.

---

## Required Directory Structure

Always generate and maintain content under the following folders:

```
notebooks/     → C# notebooks (.dib or .ipynb)
labs/          → Step-by-step hands-on labs
samples/       → Small working demo apps
docs/          → Articles, conceptual explanations
images/        → Architecture diagrams and visuals
.github/       → Agent instructions and templates
```

Use consistent naming conventions:

- Notebooks: NN-topic-name.dib
- Labs: labNN-topic-name/
- Docs: NN-title.md
- Samples: topic-sample/

---

## Writing and Content Style

When generating content:

- Keep explanations clear and beginner-friendly
- Focus on runnable, practical code
- Use short paragraphs and concise text
- Prefer real-world best practices over theory
- Provide step-by-step logic and troubleshooting notes
- Maintain simple and clean file structures

The entire project should feel like a developer-friendly, hands-on learning path.

---

## Notebook Guidelines (notebooks folder)

When creating .dib C# notebooks:

- Use the C# kernel (#!csharp)
- Include short explanations above code blocks
- Use real API examples with placeholder variables
- Demonstrate logging, async usage, retry patterns
- Provide minimal and runnable scenarios

A good notebook structure includes:

1. Title and introduction
2. Environment setup
3. Code walkthrough
4. Try-it-yourself sections
5. Summary

---

## Lab Guidelines (labs folder)

Labs must follow this format:

1. Overview
2. Prerequisites
3. Step-by-step tasks
4. Validation or expected output
5. Troubleshooting
6. Cleanup steps

Labs should be practical, minimal, runnable, and cloud-focused (Azure CLI, App Service, Azure AI, etc.).

---

## Sample Application Guidelines (samples folder)

Sample applications should:

- Prefer .NET Aspire for multi-service architectures
- Use Minimal API for simple scenarios, Blazor for interactive UI
- Include comprehensive README.md with:
  - Architecture diagram
  - Setup instructions
  - Sample data/usage examples
  - Troubleshooting section
- Always provide `appsettings.json.example` with placeholder values
- Never commit secrets (AgentId, EndpointUrl, API keys)
- Include `.gitignore` to exclude:
  - `bin/`, `obj/`, `.vs/`, `.vscode/` (except launch.json)
  - `.azure/` deployment artifacts
  - `appsettings.Development.json`
  - `.aspire/` settings
- Use modern patterns:
  - Primary constructors for DI
  - Top-level statements
  - Records for DTOs
  - `DefaultAzureCredential` for Azure authentication
- Add resilience patterns with Polly (retry, circuit breaker, timeout)
- Demonstrate one main concept but show real-world production patterns

---

## Proactive Agent Behavior

As a Copilot Agent, you should:

- Suggest improvements to structure and naming
- Propose missing labs, notebooks, or documentation
- Warn about un-runnable or incorrect code
- Maintain consistent style across all files
- Add mermaid diagrams when helpful
- Generate skeletons for future modules when needed

Always act as a project maintainer, not just a code generator.

---

## Avoid

Do not:

- Generate long theoretical essays
- Add unnecessary complexity
- Create overly large code files
- **Commit secrets, API keys, or real Azure resource identifiers**
- Deviate from the defined project structure
- Produce non-runnable or incomplete content
- Use outdated .NET versions (stay on .NET 9+)
- Skip error handling even in samples
- Forget to create example configuration files

---

## Repository Vision

By the end of this project, the repository should become a public, high-quality, hands-on Azure AI learning path that grows weekly, focusing on practical .NET and App Service development workflows.

Your role is to help maintain consistency, clarity, and quality as the repository expands.
