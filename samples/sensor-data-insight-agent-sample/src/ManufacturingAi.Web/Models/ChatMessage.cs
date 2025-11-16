namespace ManufacturingAi.Web.Models;

public record ChatMessage(
    string Role,        // "user" or "assistant"
    string Content,
    DateTime Timestamp,
    bool IsError = false
);
