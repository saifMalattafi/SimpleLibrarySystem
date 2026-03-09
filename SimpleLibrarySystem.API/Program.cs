using SimpleLibrarySystem.Application.Applications.Interfaces;
using SimpleLibrarySystem.Application.Applications.UseCases.BookUseCases;
using SimpleLibrarySystem.Application.Applications.UseCases.LoanUseCases;
using SimpleLibrarySystem.Domain.Interfaces;
using SimpleLibrarySystem.Infrastructure.Repositories;
using SimpleLibrarySystem.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

builder.Services.AddScoped<BorrowBookUseCase>();
builder.Services.AddScoped<ReturnBookUseCase>();
builder.Services.AddScoped<ExtendLoanUseCase>();
builder.Services.AddScoped<INotificationService, EmailNotificationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
