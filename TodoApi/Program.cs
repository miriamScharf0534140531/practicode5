
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ToDoDbContext>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/", async(ToDoDbContext toDo) =>{ 
    var todos = await toDo.Items.ToListAsync();
    return  todos;
    });
    
app.MapPost("/", async (Item item,HttpContext context, ToDoDbContext toDo) =>
{
    //var item = await context.Request.ReadFromJsonAsync<Item>();
        toDo.Items.Add(item);
        await toDo.SaveChangesAsync();
        //return "good";
        //return new OkObjectResult(item);
        return new CreatedResult($"/{item.Id}", item);
});
async Task<IActionResult> PutItem(int id,Item it, HttpContext context, ToDoDbContext toDo)
{
    var item = await toDo.Items.FindAsync(id);
    if (item == null)
    {
        return new NotFoundResult();
    }
    // if(it.Name!=null&&it.Name!="")
    //     item.Name=it.Name;
    item.IsComplete=it.IsComplete;
    await toDo.SaveChangesAsync();
    //return new OkObjectResult(item);
    return new CreatedResult($"/{item.Id}", item);
     
}

app.MapPut("/{id}", PutItem);

static async Task<IActionResult> DeleteItem(int id, ToDoDbContext toDo)
{
    var item = await toDo.Items.FindAsync(id);
    if (item == null)
    {
        return new NotFoundResult();
    }

    toDo.Items.Remove(item);
    await toDo.SaveChangesAsync();
    return new NoContentResult();
}

app.MapDelete("/{id}", DeleteItem);
app.UseCors("AllowAll");
app.Run();
