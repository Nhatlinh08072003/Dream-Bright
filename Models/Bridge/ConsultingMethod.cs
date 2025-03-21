public abstract class ConsultingMethod
{
    protected readonly IServiceLevel _serviceLevel;  // Cầu nối (Bridge)

    protected ConsultingMethod(IServiceLevel serviceLevel)
    {
        _serviceLevel = serviceLevel;
    }

    public abstract string Consult();  // Phương thức trừu tượng
}
