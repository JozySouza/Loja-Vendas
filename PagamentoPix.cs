namespace LojaVendas.Pagamentos;

/// <summary>
/// PIX: aplica 5% de desconto sobre o valor da compra.
/// </summary>
public class PagamentoPix : FormaPagamento
{
    private const decimal PercentualDesconto = 0.05m;

    public override string Nome => "PIX";

    public override decimal CalcularValorFinal(decimal valor)
    {
        return valor - (valor * PercentualDesconto);
    }
}
