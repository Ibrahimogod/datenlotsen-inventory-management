using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Datenlotsen.WPF.Commands;
using Datenlotsen.WPF.Services;

namespace Datenlotsen.WPF.ViewModels
{
    public enum StockStatus
    {
        LowStock = 1,
        InStock = 2,
    }

    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class InventoryItemModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal StockQuantity { get; set; }
        public CategoryModel Category { get; set; } = new CategoryModel();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IInventoryApiService _apiService;
        private InventoryItemModel? _selectedItem;
        private string? _searchText;
        private StockStatus? _selectedStockStatus;
        private CategoryModel? _selectedCategory;
        private bool _isEditing;
        private string? _errorMessage;
        private string _defaultStockStatusText;

        public ObservableCollection<InventoryItemModel> InventoryItems { get; } = new();
        public ObservableCollection<CategoryModel> Categories { get; } = new();
        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CancelEditCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            _apiService = new InventoryApiService();
            SearchCommand = new RelayCommand(async _ => await LoadInventoryAsync());
            AddCommand = new RelayCommand(_ => StartAdd());
            EditCommand = new RelayCommand(_ => StartEdit(), _ => SelectedItem != null);
            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => CanSave());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync(), _ => SelectedItem != null);
            CancelEditCommand = new RelayCommand(_ => CancelEdit());
            _ = LoadCategoriesAsync();
            _ = LoadInventoryAsync();
            DefaultStockStatusText = "Stock Status";
        }

        public string DefaultStockStatusText
        {
            get => _defaultStockStatusText;
            set
            {
                _defaultStockStatusText = value;
                OnPropertyChanged();
            }
        }
        
        public InventoryItemModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public StockStatus? SelectedStockStatus
        {
            get => _selectedStockStatus;
            set
            {
                _selectedStockStatus = value;
                OnPropertyChanged();
            }
        }

        public CategoryModel? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private async Task LoadCategoriesAsync()
        {
            Categories.Clear();
            Categories.Add(new CategoryModel { Id = Guid.Empty, Name = "All Categories" });
            var cats = await _apiService.GetCategoriesAsync();
            foreach (var c in cats)
                Categories.Add(c);
            SelectedCategory = Categories.FirstOrDefault();
        }

        private async Task LoadInventoryAsync()
        {
            InventoryItems.Clear();
            Guid? categoryId = (SelectedCategory != null && SelectedCategory.Id != Guid.Empty) ? SelectedCategory.Id : null;
            var items = await _apiService.GetInventoryItemsAsync(SearchText, SelectedStockStatus, categoryId);
            foreach (var item in items)
                InventoryItems.Add(item);
        }

        private void StartAdd()
        {
            SelectedItem = new InventoryItemModel();
            // Set to first real category (not 'All Categories') if available
            SelectedItem.Category = Categories.FirstOrDefault(c => c != null && c.Id != Guid.Empty) ?? new CategoryModel();
            IsEditing = true;
        }

        private void StartEdit()
        {
            if (SelectedItem != null)
            {
                // Clone the item for editing
                var editingItem = new InventoryItemModel
                {
                    Id = SelectedItem.Id,
                    Name = SelectedItem.Name,
                    StockQuantity = SelectedItem.StockQuantity,
                    CreatedAt = SelectedItem.CreatedAt,
                    UpdatedAt = SelectedItem.UpdatedAt
                };
                // Set Category to the reference from Categories collection
                editingItem.Category = Categories.FirstOrDefault(c => c != null && c.Id == SelectedItem.Category.Id) ?? new CategoryModel();
                SelectedItem = editingItem;
                IsEditing = true;
            }
        }

        private async Task SaveAsync()
        {
            if (SelectedItem == null)
                return;

            try
            {
                if (SelectedItem.Id == Guid.Empty)
                {
                    await _apiService.CreateInventoryItemAsync(SelectedItem.Name, SelectedItem.StockQuantity, SelectedItem.Category.Id);
                }
                else
                {
                    await _apiService.UpdateInventoryItemAsync(SelectedItem.Id, SelectedItem.Name, SelectedItem.StockQuantity, SelectedItem.Category.Id);
                }

                await LoadInventoryAsync();
                IsEditing = false;
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedItem == null)
                return;

            try
            {
                await _apiService.DeleteInventoryItemAsync(SelectedItem.Id);
                await LoadInventoryAsync();
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private void CancelEdit()
        {
            IsEditing = false;
            ErrorMessage = null;
        }

        private bool CanSave()
        {
            return SelectedItem != null && !string.IsNullOrWhiteSpace(SelectedItem.Name) && SelectedItem.StockQuantity >= 0;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}