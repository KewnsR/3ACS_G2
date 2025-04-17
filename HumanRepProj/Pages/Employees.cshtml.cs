﻿using HumanRepProj.Data;
using HumanRepProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRepProj.Pages
{
    public class EmployeesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;

        public EmployeesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public PaginatedList<Employee> Employees { get; private set; } = null!;
        public int TotalPages { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        [Range(1, MaxPageSize)]
        public int PageSize { get; set; } = DefaultPageSize;

        [BindProperty(SupportsGet = true)]
        [StringLength(100)]
        public string SearchTerm { get; set; } = string.Empty;

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var employeesQuery = BuildQuery();
            await LoadEmployeesAsync(employeesQuery);
        }

        private IQueryable<Employee> BuildQuery()
        {
            IQueryable<Employee> query = _context.Employees
                .Include(e => e.Department)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.FirstName, $"%{SearchTerm}%") ||
                    EF.Functions.Like(e.LastName, $"%{SearchTerm}%") ||
                    (e.Email != null && EF.Functions.Like(e.Email, $"%{SearchTerm}%")) ||
                    EF.Functions.Like(e.Position, $"%{SearchTerm}%") ||
                    (e.Department != null && EF.Functions.Like(e.Department.Name, $"%{SearchTerm}%")));
            }

            return query.OrderBy(e => e.LastName)
                      .ThenBy(e => e.FirstName);
        }

        private async Task LoadEmployeesAsync(IQueryable<Employee> query)
        {
            var totalItems = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            // Ensure CurrentPage is within valid range
            CurrentPage = Math.Clamp(CurrentPage, 1, TotalPages > 0 ? TotalPages : 1);

            Employees = await PaginatedList<Employee>.CreateAsync(
                query,
                CurrentPage,
                PageSize);
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Status = employee.Status == "Active" ? "Inactive" : "Active";
            await _context.SaveChangesAsync();

            StatusMessage = $"Employee {employee.FullName} status updated to {employee.Status}";
            return RedirectToPage(new { CurrentPage, SearchTerm });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            StatusMessage = $"Employee {employee.FullName} has been deleted";
            return RedirectToPage(new { CurrentPage, SearchTerm });
        }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}