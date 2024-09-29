namespace SnackTech.Core.Domain.Types;

internal struct StatusPedidoValido
{
    internal static readonly StatusPedidoValido Iniciado = new StatusPedidoValido(1);
    internal static readonly StatusPedidoValido AguardandoPagamento = new StatusPedidoValido(2);
    internal static readonly StatusPedidoValido Recebido = new StatusPedidoValido(3);
    internal static readonly StatusPedidoValido EmPreparacao = new StatusPedidoValido(4);
    internal static readonly StatusPedidoValido Pronto = new StatusPedidoValido(5);
    internal static readonly StatusPedidoValido Finalizado = new StatusPedidoValido(6);

    private int valor;
    private readonly Dictionary<int, string> ValoresValidos = new()
    {
        {1,"Iniciado"},
        {2,"AguardandoPagamento"},
        {3,"Recebido"},
        {4,"EmPreparacao"},
        {5,"Pronto"},
        {6,"Finalizado"}
    };

    internal int Valor
    {
        readonly get { return valor; }
        set
        {
            ValidarValor(value);
            valor = value;
        }
    }

    internal StatusPedidoValido(int valor)
    {
        Valor = valor;
    }

    public static implicit operator StatusPedidoValido(int value)
    {
        return new StatusPedidoValido(value);
    }

    public static implicit operator int(StatusPedidoValido valor)
    {
        return valor.Valor;
    }

    public readonly override string ToString()
            => Valor.ToString();

    private readonly void ValidarValor(int value)
    {
        if (!ValoresValidos.Any((chaveValor) => chaveValor.Key == value))
        {
            throw new ArgumentException($"Valor {value} não é uma Categoria de Produto Válida");
        }
    }
}