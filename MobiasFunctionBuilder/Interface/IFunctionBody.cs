namespace MobiasFunctionBuilder.Interface
{
    public interface IFunctionBody
    {
        IFunctionReturn Body(IBodyLine firstBodyLine, params IBodyLine[] bodyLines);
    }
}