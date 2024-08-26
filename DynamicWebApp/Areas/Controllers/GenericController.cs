namespace DynamicWebApp.Areas.Controllers;

public interface IRepository<T, TKey> where T : BaseEntity<TKey>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(TKey id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TKey id);
    Task RemoveAsync(TKey id);
}

public class Repository<T, TKey>(ProjectDbContext dbContext)
    : IRepository<T, TKey> where T : BaseEntity<TKey>
{
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(TKey id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        dbContext.Set<T>().Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            dbContext.Set<T>().Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }

    public virtual async Task RemoveAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}

public interface IGenericController<TKey, TItemViewModel, TItemDetailViewModel, TCreateViewModel, TUpdateViewModel>
    where TItemViewModel : BaseEntityViewModel<TKey>, IListItemViewModel
    where TItemDetailViewModel : BaseEntityViewModel<TKey>, IDetailViewModel
    where TCreateViewModel : BaseEntityViewModel<TKey>, ICreateViewModel
    where TUpdateViewModel : BaseEntityViewModel<TKey>, IUpdateViewModel
{
    Task<IActionResult> Index();
    Task<IActionResult> Detail(TKey id);
    Task<IActionResult> Create();
    Task<IActionResult> Create(TCreateViewModel viewModel);
    Task<IActionResult> Update(TKey id);
    Task<IActionResult> Update(TUpdateViewModel viewModel);
    Task<IActionResult> Delete(TKey id);
    Task<IActionResult> DeleteConfirmed(TKey id);
    Task<IActionResult> Remove(TKey id);
    Task<IActionResult> RemoveConfirmed(TKey id);
}

public abstract class GenericController<T, TKey, TItemViewModel, TItemDetailViewModel, TCreateViewModel, TUpdateViewModel>(IRepository<T, TKey> repository, IMapper mapper)
    : Controller, IGenericController<TKey, TItemViewModel, TItemDetailViewModel, TCreateViewModel, TUpdateViewModel>
    where T : BaseEntity<TKey>
    where TItemViewModel : BaseEntityViewModel<TKey>, IListItemViewModel
    where TItemDetailViewModel : BaseEntityViewModel<TKey>, IDetailViewModel
    where TCreateViewModel : BaseEntityViewModel<TKey>, ICreateViewModel
    where TUpdateViewModel : BaseEntityViewModel<TKey>, IUpdateViewModel
{
    [HttpGet]
    public virtual async Task<IActionResult> Index()
    {
        await DataBind();
        var models = await repository.GetAllAsync();
        var viewModels = mapper.Map<List<TItemViewModel>>(models);
        return View(viewModels);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Detail(TKey id)
    {
        await DataBind();
        var model = await repository.GetByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        var viewModel = mapper.Map<TItemDetailViewModel>(model);
        return View(viewModel);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Create()
    {
        await DataBind();

        var createViewModel = Activator.CreateInstance<TCreateViewModel>();

        return View("AddOrEdit", createViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Create(TCreateViewModel viewModel)
    {
        await DataBind();
        if (!ModelState.IsValid)
        {
            return View("AddOrEdit", viewModel);
        }

        var model = mapper.Map<T>(viewModel);
        await repository.AddAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public virtual async Task<IActionResult> Update(TKey id)
    {
        await DataBind();
        var model = await repository.GetByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        var viewModel = mapper.Map<TUpdateViewModel>(model);
        await SetSelectedValues(viewModel);
        return View("AddOrEdit", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Update(TUpdateViewModel viewModel)
    {
        await DataBind();
        if (!ModelState.IsValid)
        {
            return View("AddOrEdit", viewModel);
        }

        var model = mapper.Map<T>(viewModel);
        await repository.UpdateAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public virtual async Task<IActionResult> Delete(TKey id)
    {
        await DataBind();
        var model = await repository.GetByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        var viewModel = mapper.Map<TItemDetailViewModel>(model);
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> DeleteConfirmed(TKey id)
    {
        await repository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public virtual async Task<IActionResult> Remove(TKey id)
    {
        await DataBind();
        var model = await repository.GetByIdAsync(id);
        if (model == null)
        {
            return NotFound();
        }
        var viewModel = mapper.Map<TItemDetailViewModel>(model);
        return View(viewModel);
    }

    [HttpPost, ActionName("Remove")]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> RemoveConfirmed(TKey id)
    {
        await repository.RemoveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public virtual async Task DataBind() => await Task.CompletedTask;

    public virtual async Task SetSelectedValues(TUpdateViewModel viewModel)
    {
        var modelType = typeof(TUpdateViewModel);

        foreach (var property in modelType.GetProperties())
        {
            if (property.Name.Equals("Id"))
                continue;

            if (property.Name.EndsWith("Id"))
            {
                var selectedValue = property.GetValue(viewModel);
                if (selectedValue != null)
                {
                    var selectList = ViewData[property.Name] as SelectList;
                    if (selectList != null)
                    {
                        ViewData[property.Name] = new SelectList(selectList.Items, selectList.DataValueField, selectList.DataTextField, selectedValue);
                    }
                }
            }
            else if (property.Name.EndsWith("Ids") || (property.PropertyType.IsArray && property.PropertyType.GetElementType().IsEnum))
            {
                var selectedValuesArray = property.GetValue(viewModel) as Array;

                if (selectedValuesArray != null)
                {
                    var selectList = ViewData[property.Name] as SelectList;
                    if (selectList != null)
                    {
                        var selectedValueStrings = selectedValuesArray
                            .Cast<object>()
                            .Select(v => Convert.ToInt32(v).ToString())  // Enum'un sayısal karşılığını al ve string'e çevir
                            .ToList();

                        ViewData[property.Name] = new SelectList(selectList.Items, selectList.DataValueField, selectList.DataTextField, selectedValueStrings);
                    }
                }
            }
            else if (property.PropertyType.IsEnum)
            {
                var selectedValue = property.GetValue(viewModel);
                if (selectedValue != null)
                {
                    var selectList = ViewData[property.Name] as SelectList;
                    if (selectList != null)
                    {
                        ViewData[property.Name] = new SelectList(selectList.Items, selectList.DataValueField, selectList.DataTextField, selectedValue);
                    }
                }
            }
        }

        await Task.CompletedTask;
    }
}
