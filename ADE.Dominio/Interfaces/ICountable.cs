namespace ADE.Dominio.Interfaces
{
    public interface ICountable
    {
        System.Threading.Tasks.Task<int> Count();
    }
}
