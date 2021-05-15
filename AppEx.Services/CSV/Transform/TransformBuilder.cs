using AppEx.Services.Models;

namespace AppEx.Services.CSV.Transform
{
    public class TransformBuilder
    {
        private dynamic _expando { get; set; }
        private WaterConnectOptions _options { get; set; }

        public TransformBuilder(WaterConnectOptions options, dynamic expando)
        {
            _expando = expando;
            _options = options;
        }

        public dynamic GetInstance()
        {
            return _expando;
        }

        public TransformBuilder RemoveColumns()
        {
            var strategy = new RemoveColumnStrategy();
            strategy.Transform(_options, _expando);
            return this;
        }

        public TransformBuilder NewSumColumns()
        {
            var strategy = new NewSumColumnStrategy();
            strategy.Transform(_options, _expando);
            return this;
        }
    }
}
