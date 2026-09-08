using System.Threading.Tasks;

namespace Calculator;

public partial class CalculatorApp : Form
{
    private readonly SynchronizationContext _main;
    public CalculatorApp()
    {
        InitializeComponent();
        _main = SynchronizationContext.Current;
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b))
        {
            var t1 = new Task<int>(() => LongAdd(a, b));
            t1.ContinueWith(t => _main.Post(UpdateAnswer, t.Result));
            t1.Start();
            //int result = LongAdd(a, b);
            //UpdateAnswer(result);
        }
    }

    private void UpdateAnswer(object? result)
    {
        lblAnswer.Text = result?.ToString();
    }

    private int LongAdd(int a, int b)
    {
        Task.Delay(10000).Wait();
        return a + b;
    }
}