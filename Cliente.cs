namespace LojaVendas.Models;

/// <summary>
/// Representa o cliente de uma venda.
///
/// ENCAPSULAMENTO: o CPF só pode ser definido no construtor (propriedade
/// somente com "get"). Depois de criado o objeto, não existe nenhuma forma
/// de alterar o CPF de fora da classe.
/// </summary>
public class Cliente
{
    public string Nome { get; private set; }

    // Sem "set" -> só é possível atribuir valor dentro do construtor.
    public string Cpf { get; }

    public Cliente(string nome, string cpf)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("O CPF do cliente é obrigatório.", nameof(cpf));

        Nome = nome;
        Cpf = cpf;
    }
}
