using Swashbuckle.AspNetCore.Annotations;
using System;

namespace EasyTrufi.Core.CustomEntities;

/// <summary>
/// Representa los detalles de paginación de una colección de datos.
/// </summary>
public class Pagination
{
    /// <summary>
    /// Número total de elementos en la colección.
    /// </summary>
    [SwaggerSchema("Número total de elementos en la colección", Nullable = false)]
    public int TotalCount { get; set; }

    /// <summary>
    /// Cantidad de elementos por página.
    /// </summary>
    [SwaggerSchema("Cantidad de elementos por página", Nullable = false)]
    public int PageSize { get; set; }

    /// <summary>
    /// Número de la página actual.
    /// </summary>
    [SwaggerSchema("Número de la página actual", Nullable = false)]
    public int CurrentPage { get; set; }

    /// <summary>
    /// Número total de páginas.
    /// </summary>
    [SwaggerSchema("Número total de páginas", Nullable = false)]
    public int TotalPages { get; set; }

    /// <summary>
    /// Indica si hay una página siguiente disponible.
    /// </summary>
    [SwaggerSchema("Indica si existe una página siguiente", Nullable = false)]
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Indica si hay una página anterior disponible.
    /// </summary>
    [SwaggerSchema("Indica si existe una página anterior", Nullable = false)]
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Constructor por defecto que inicializa una nueva instancia de la clase <see cref="Pagination"/>.
    /// </summary>
    public Pagination()
    {
    }

    /// <summary>
    /// Constructor que inicializa una nueva instancia de la clase <see cref="Pagination"/> a partir de una lista paginada.
    /// </summary>
    /// <param name="lista">Una instancia de <see cref="PagedList{T}"/> que contiene los datos de paginación.</param>
    public Pagination(PagedList<object> lista)
    {
        TotalCount = lista.TotalCount;
        PageSize = lista.PageSize;
        CurrentPage = lista.CurrentPage;
        TotalPages = lista.TotalPages;
        HasNextPage = lista.HasNextPage;
        HasPreviousPage = lista.HasPreviousPage;
    }
}
