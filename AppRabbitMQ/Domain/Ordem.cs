namespace AppRabbitMQ.Domain
{
    public sealed class Ordem
    {
        private int _Numero;
        public int Numero
        {
            get { return _Numero; }
            set { _Numero = value; }
        }

        private string _Produto;
        public string Produto
        {
            get { return _Produto; }
            set { _Produto = value; }
        }

        private decimal _ValorTotal;
        public decimal ValorTotal
        {
            get { return _ValorTotal; }
            set { _ValorTotal = value; }
        }
    }
}
