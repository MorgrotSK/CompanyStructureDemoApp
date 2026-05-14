namespace DemoKROS.Constants;

public static class ValidationPatterns
{
    public const string PersonName = @"^\p{Lu}[\p{L}\p{M}'\- ]*$";
    public const string OrganizationName = @"^[\p{L}\p{N}\p{M}&().,\-'/ ]+$";
    public const string OrganizationCode = @"^[A-Z0-9\-_]+$";
}