namespace LojaVendas.Pagamentos;

/// <summary>
/// Cartão de crédito: aplica 3% de taxa sobre o valor da compra.
/// </summary>
public class PagamentoCartao : FormaPagamento
{
    private const decimal PercentualTaxa = 0.03m;

    public override string Nome => "Cartão de crédito";

    public override decimal CalcularValorFinal(decimal valor)
    {
        return valor + (valor * PercentualTaxa);
    }
}
