namespace OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface til mapning af objekter fra en type til 
    /// en andentype, som typisk bruges ved konvertering
    /// af objekter i domænemodellen til objekter i
    /// viewmodellen.
    /// </summary>
    public interface IObjectMapper
    {
        /// <summary>
        /// Mapper et objekt af typen TSource til et objekt af typen TDestination.
        /// </summary>
        TDestination Map<TSource, TDestination>(TSource source);
    }
}
