namespace MobiasFunctionBuilder.Interface
{
    public interface IFunctionBody
    {
        IFunctionReturn Body(params IBodyLine[] bodyLines);
    }
}