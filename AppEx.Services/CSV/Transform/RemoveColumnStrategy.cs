using AppEx.Services.Models;
using System.Collections.Generic;

namespace AppEx.Services.CSV.Transform
{
    /// <summary>
    /// 
    /// </summary>
    public class RemoveColumnStrategy : ITransformStrategy
    {
        public dynamic Transform(TransformOptions options, dynamic expando)
        {
            if (options?.RemoveColumns?.Count > 0)
            {
                options.RemoveColumns.ForEach(column =>
                {
                    ((IDictionary<string, object>)expando).Remove(column);
                });
            }

            return expando;
        }
    }
}
