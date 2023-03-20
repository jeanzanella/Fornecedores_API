using Fornecedores_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fornecedores_API.DataContext
{
    public class SqlDataContext : DbContext
    {
        public SqlDataContext(DbContextOptions<SqlDataContext> options) : base(options) { }

        public DbSet<Fornecedor> Fornecedores { get; set;}
        public DbSet<Endereco> Enderecos { get; set;}
    }
}
