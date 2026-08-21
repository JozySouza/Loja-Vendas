using LojaVendas.Pagamentos;

namespace LojaVendas.Models;

/// <summary>
/// Situação possível de uma venda.
/// </summary>
public enum SituacaoVenda
{
    Pendente,
    Pago
}

/// <summary>
/// Representa uma venda da loja.
///
/// ENCAPSULAMENTO: "ValorCompra" e "Situacao" só têm "get" público.
/// Ninguém consegue fazer, de fora da classe, algo como:
///     venda.Situacao = SituacaoVenda.Pago;
/// A única forma de mudar a situação para "Pago" é chamando o método
/// Pagar(...), que é o dono da regra de negócio do pagamento.
/// </summary>
public class Venda
{
    public int Numero { get; }
    public Cliente Cliente { get; }

    public decimal ValorCompra { get; private set; }
    public SituacaoVenda Situacao { get; private set; }

    // Guardados após o pagamento, para poder exibi-los na listagem depois.
    public FormaPagamento? FormaPagamentoUtilizada { get; private set; }
    public decimal? ValorFinal { get; private set; }

    public Venda(int numero, Cliente cliente, decimal valorCompra)
    {
        if (cliente is null)
            throw new ArgumentNullException(nameof(cliente));

        // Regra de negócio: o valor da venda deve ser maior que zero.
        if (valorCompra <= 0)
            throw new ArgumentException("O valor da venda deve ser maior que zero.", nameof(valorCompra));

        Numero = numero;
        Cliente = cliente;
        ValorCompra = valorCompra;

        // Toda venda nasce Pendente.
        Situacao = SituacaoVenda.Pendente;
    }

    /// <summary>
    /// Única operação capaz de "pagar" a venda.
    ///
    /// POLIMORFISMO: este método recebe qualquer objeto do tipo
    /// FormaPagamento (a classe abstrata) e chama CalcularValorFinal
    /// sem precisar saber, em nenhum momento, se é Pix, Cartão ou
    /// Dinheiro. Quem decide o cálculo é o próprio objeto recebido.
    /// </summary>
    public decimal Pagar(FormaPagamento formaPagamento)
    {
        if (formaPagamento is null)
            throw new ArgumentNullException(nameof(formaPagamento));

        // Regra de negócio: uma venda já paga não pode ser paga novamente.
        if (Situacao == SituacaoVenda.Pago)
            throw new InvalidOperationException($"A venda {Numero} já foi paga e não pode ser paga novamente.");

        decimal valorFinal = formaPagamento.CalcularValorFinal(ValorCompra);

        FormaPagamentoUtilizada = formaPagamento;
        ValorFinal = valorFinal;
        Situacao = SituacaoVenda.Pago;

        return valorFinal;
    }
}
