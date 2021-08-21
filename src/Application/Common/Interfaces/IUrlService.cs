namespace Application.Common.Interfaces
{
    public interface IUrlService
    {
        string GenerateEmailConfirmationLink(string userId, string token);
    }
}