using Fornecedores_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fornecedores_API.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly SqlDataContext _context;

        public FornecedorController(SqlDataContext context) 
        { 
            _context = context;
        }
        [HttpGet("Listar")]
        public async Task<ActionResult<List<Fornecedor>>> ListarFornecedores(string? nome, int? cnpj, string? cidade)
        {
            List<Fornecedor> fornecedores = await _context.Fornecedores.ToListAsync();
            if(!String.IsNullOrEmpty(nome))
                fornecedores = fornecedores.FindAll(f => f.Nome == nome);
            if (cnpj != null)
                fornecedores = fornecedores.FindAll(f => f.CNPJ == cnpj);
            if (!String.IsNullOrEmpty(cidade))
            {
                fornecedores = fornecedores.FindAll(f => f.Enderecos.FirstOrDefault(e => e.Cidade == cidade) != null);
            }

            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> Get(int id)
        {
            Fornecedor fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
                return BadRequest("Fornecedor não encontrado");
            return Ok(fornecedor);
        }

        [HttpPost("CriarFornecedor")]
        public async Task<ActionResult<List<Fornecedor>>> CriarFornecedor([FromBody]Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                _context.Fornecedores.Add(fornecedor);
                foreach (var endereco in fornecedor.Enderecos)
                {
                    _context.Enderecos.Add(endereco);
                }
                await _context.SaveChangesAsync();
                List<Fornecedor> fornecedores = await _context.Fornecedores.ToListAsync();
                return Ok(fornecedores);
            }
            else
                return BadRequest("Erro ao criar fornecedor, favor validar dados");
        }

        [HttpPut("EditarFornecedor")]
        public async Task<ActionResult<Fornecedor>> EditarFornecedor([FromBody]Fornecedor request)
        {
            if (ModelState.IsValid)
            {
                Fornecedor fornecedor = await _context.Fornecedores.FindAsync(request.Id);
                if (fornecedor == null)
                    return BadRequest("Fornecedor não encontrado");

                fornecedor.Nome = request.Nome;
                fornecedor.CNPJ = request.CNPJ;
                fornecedor.Telefone = request.Telefone;
                fornecedor.Email = request.Email;
                //sei que essa parte precisaria de um tratamento melhor, mas por falta de tempo deixei assim
                foreach (var endereco in request.Enderecos)
                {
                    if(endereco.Id != 0)
                    {
                        Endereco enderecoBanco = await _context.Enderecos.FindAsync(endereco.Id);
                        enderecoBanco.CEP = endereco.CEP;
                        enderecoBanco.Rua = endereco.Rua;
                        enderecoBanco.Numero = endereco.Numero;
                        enderecoBanco.Cidade = endereco.Cidade;
                        enderecoBanco.Complemento = endereco.Complemento;
                        enderecoBanco.Estado = endereco.Estado;
                        enderecoBanco.Pais = endereco.Pais;
                    }
                    else
                    {
                        _context.Enderecos.Add(endereco);
                    }
                }
                fornecedor.Enderecos = request.Enderecos;

                await _context.SaveChangesAsync();
                return Ok(fornecedor);
            }
            else
                return BadRequest("Erro ao editar fornecedor, favor validar dados");
        }
    }
}
