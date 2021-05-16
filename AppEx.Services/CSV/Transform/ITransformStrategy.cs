using AppEx.Services.Models;

namespace AppEx.Services.CSV.Transform
{
    public interface ITransformStrategy
    {
        dynamic Transform(TransformOptions options, dynamic expando);
    }
}
