namespace DevCenter.Domain.Entieties;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string NIP { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }
    public string Street { get; set; }
    public string CompanyEmail { get; set; }
}