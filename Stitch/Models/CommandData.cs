namespace Stitch.Models;

public record CommandData(Predicate<CommandOptions> Pattern, Type CommandType);