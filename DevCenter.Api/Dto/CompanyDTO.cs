namespace DevCenter.Api.Dto
{
    public record CompanyDTO(
            string Name,
            string NIP,
            string Country,
            string City,
            string PostalCode,
            string Street,
            string CompanyEmail
        );
}
