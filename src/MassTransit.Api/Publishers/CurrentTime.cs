namespace MassTransit.Api.Publishers;

public record CurrentTime
{
    public string Value { get; init; } = string.Empty;
}