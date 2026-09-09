using Application.Commons.DTOs;
using Application.Commons.Models;
using MediatR;

namespace Application.Queries.Dashboard.GetDashboardSummary
{
    public class GetDashboardSummaryQuery : IRequest<ApiResponse<DashboardSummaryDto>>
    {
    }
}
