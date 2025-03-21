public class OnlineConsulting : ConsultingMethod
{
    public OnlineConsulting(IServiceLevel serviceLevel) : base(serviceLevel) { }

    public override string Consult()
    {
        return $"Tư vấn online - Gói {_serviceLevel.GetLevel()}";
    }
}