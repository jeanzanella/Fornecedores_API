namespace Fornecedores_API.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CNPJ { get; set; }
        public long Telefone { get; set; }
        public string Email { get; set; }
        public List<Endereco>? Enderecos { get; set; }
    }
}
