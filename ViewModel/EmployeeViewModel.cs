using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Mod7Exer1.Model;
using Mod7Exer1.Services;

namespace Mod7Exer1.ViewModel
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private readonly EmployeeService _employeeService;
        public ObservableCollection<Employee> EmployeeList { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public EmployeeViewModel()
        {
            _employeeService = new EmployeeService();
            EmployeeList = new ObservableCollection<Employee>();

            LoadDataCommand = new Command(async () => await LoadData());
            EditCommand = new Command<Employee>(OnEditEmployee);
            DeleteCommand = new Command<Employee>(OnDeleteEmployee);

            LoadData();
        }

        public async Task LoadData()
        {
            if (IsBusy) return;

            IsBusy = true;
            StatusMessage = "Loading employee data...";
            try
            {
                var employees = await _employeeService.GetAllPersonalsAsync();
                EmployeeList.Clear();
                foreach (var employee in employees)
                {
                    EmployeeList.Add(employee);
                }
                StatusMessage = "Employee data loaded successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to load data: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnEditEmployee(Employee employee)
        {
            if (employee != null)
            {
                // Here, navigate to an EditEmployee page or open a dialog
                StatusMessage = $"Editing {employee.Name}";
            }
        }

        private void OnDeleteEmployee(Employee employee)
        {
            if (employee != null && EmployeeList.Contains(employee))
            {
                EmployeeList.Remove(employee);
                StatusMessage = $"{employee.Name} has been deleted.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
