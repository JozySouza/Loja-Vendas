namespace LojaVendas.Pagamentos;

/// <summary>
/// ABSTRAÇÃO: define o "contrato" que toda forma de pagamento precisa
/// seguir, sem dizer como cada uma calcula o valor final. Por ser
/// abstract, ninguém pode instanciar "new FormaPagamento()" diretamente
/// — só faz sentido usar uma implementação concreta (Pix, Cartão, etc).
///
/// HERANÇA: PagamentoPix, PagamentoCartao e PagamentoDinheiro vão herdar
/// desta classe.
/// </summary>
public abstract class FormaPagamento
{
    // Nome amigável usado nas telas/relatórios.
    public abstract string Nome { get; }

    // Cada subclasse decide sua própria regra de cálculo.
    public abstract decimal CalcularValorFinal(decimal valor);
}
