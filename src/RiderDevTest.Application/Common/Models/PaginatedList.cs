using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace RiderDevTest.Application.Common.Models;

public class PaginatedList<T>
{

    public List<T> Items { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    
    [JsonConstructor]
    public PaginatedList(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static PaginatedList<T> Create(List<T> items, int pageSize)
    {
        return Create(items, 0, pageSize);
    }
    
    public static PaginatedList<T> Create(List<T> items, int pageIndex, int pageSize)
    {
        return  new PaginatedList<T>(items, pageIndex, pageSize, items.Count);
    }
    
    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, pageIndex, pageSize, count);
    }
}