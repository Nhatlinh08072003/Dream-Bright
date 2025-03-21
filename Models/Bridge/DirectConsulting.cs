public class DirectConsulting : ConsultingMethod
{
    public DirectConsulting(IServiceLevel serviceLevel) : base(serviceLevel) { }

    public override string Consult()
    {
        return $"Tư vấn trực tiếp - Gói {_serviceLevel.GetLevel()}";
    }
}