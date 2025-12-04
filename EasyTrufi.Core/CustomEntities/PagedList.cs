using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTrufi.Core.CustomEntities;

/// <summary>
/// Representa una lista paginada de elementos, utilizada para manejar datos divididos en páginas.
/// </summary>
/// <typeparam name="T">El tipo de los elementos contenidos en la lista.</typeparam>
public class PagedList<T> : List<T>
{
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
    /// Tamaño de la página (cantidad de elementos por página).
    /// </summary>
    [SwaggerSchema("Cantidad de elementos por página", Nullable = false)]
    public int PageSize { get; set; }

    /// <summary>
    /// Número total de elementos en la colección.
    /// </summary>
    [SwaggerSchema("Número total de elementos en la colección", Nullable = false)]
    public int TotalCount { get; set; }

    /// <summary>
    /// Indica si hay una página anterior disponible.
    /// </summary>
    [SwaggerSchema("Indica si existe una página anterior", Nullable = false)]
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Indica si hay una página siguiente disponible.
    /// </summary>
    [SwaggerSchema("Indica si existe una página siguiente", Nullable = false)]
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// Número de la página siguiente, si está disponible.
    /// </summary>
    [SwaggerSchema("Número de la página siguiente (null si no hay más páginas)", Nullable = true)]
    public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;

    /// <summary>
    /// Número de la página anterior, si está disponible.
    /// </summary>
    [SwaggerSchema("Número de la página anterior (null si no hay más páginas)", Nullable = true)]
    public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;

    /// <summary>
    /// Constructor que inicializa una nueva instancia de la clase <see cref="PagedList{T}"/>.
    /// </summary>
    /// <param name="items">Lista de elementos en la página actual.</param>
    /// <param name="count">Número total de elementos en la colección.</param>
    /// <param name="pageNumber">Número de la página actual.</param>
    /// <param name="pageSize">Tamaño de la página (cantidad de elementos por página).</param>
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="PagedList{T}"/> a partir de una colección de origen.
    /// </summary>
    /// <param name="source">Colección de origen.</param>
    /// <param name="pageNumber">Número de la página actual.</param>
    /// <param name="pageSize">Tamaño de la página (cantidad de elementos por página).</param>
    /// <returns>Una instancia de <see cref="PagedList{T}"/> con los elementos paginados.</returns>
    public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
