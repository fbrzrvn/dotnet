using ErrorOr;
using FreeStuff.Categories.Application.Shared.Dto;
using MediatR;

namespace FreeStuff.Categories.Application.GetAll;

public record GetAllCategoriesQuery() : IRequest<ErrorOr<List<CategoryDto>>>;
